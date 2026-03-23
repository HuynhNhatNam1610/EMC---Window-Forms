//using EMC.DAO;
//using Emgu.CV;
//using Emgu.CV.CvEnum;
//using Emgu.CV.Face;
//using Emgu.CV.Structure;
//using Emgu.CV.Util;

//namespace EMC.Service
//{
//    /// <summary>
//    /// FaceAuthService (Emgu.CV + LBPH) – phiên bản tương thích EmguCV không cần Bitmap.ToMat()
//    /// - Đăng ký: detect largest face → gray + equalize + resize → lưu PNG (VARBINARY)
//    /// - Xác thực: load PNG đã lưu → train LBPH → predict ảnh live
//    /// - Liveness (đơn giản): chọn khung sắc nét nhất trong batch, kiểm tra có chuyển động nhỏ giữa các khung
//    /// - Trả Confidence=0.0 khi không thấy mặt hoặc ảnh quá mờ/che
//    /// </summary>
//    public class FaceAuthService
//    {
//        private static FaceAuthService instance;
//        public static FaceAuthService Instance => instance ??= new FaceAuthService();

//        private readonly AccountDAO accountDao = new AccountDAO();

//        // ======= Config =======
//        public string CascadePath { get; set; } =
//            System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haarcascade_frontalface_default.xml");

//        public int EnrollSize { get; set; } = 200;                // Kích thước ảnh chuẩn
//        public double ThresholdDistance { get; set; } = 75.0;     // Khoảng cách LBPH càng THẤP càng tốt có thể thử 70–85
//        public bool UseHistogramEqualization { get; set; } = true;

//        public bool RequireLiveness { get; set; } = false;   // <- mặc định tắt
//        public double MotionThreshold { get; set; } = 0.35;   // 0.35–0.55 (thấp -> dễ qua)

//        public bool BlockSunglasses { get; set; } = false;   // bật chặn kính đen
//        public string EyeCascadePath { get; set; } =
//            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haarcascade_eye.xml");
//        public string EyeGlassCascadePath { get; set; } =
//            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haarcascade_eye_tree_eyeglasses.xml");

//        private CascadeClassifier eyeDetector, eyeGlassDetector;
//        private CascadeClassifier EyeDetector => eyeDetector ??= LoadCascade(EyeCascadePath);
//        private CascadeClassifier EyeGlassDetector => eyeGlassDetector ??= LoadCascade(EyeGlassCascadePath);

//        private CascadeClassifier faceDetector;
//        private CascadeClassifier FaceDetector => faceDetector ??= LoadCascade(CascadePath);

//        private CascadeClassifier LoadCascade(string path)
//        {

//            // Nếu chưa có file tại path mặc định
//            if (!File.Exists(path))
//            {
//                // Dò theo 3 hướng phổ biến và thư mục thật của bạn
//                var candidates = new[]
//                {
//                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haarcascade_frontalface_default.xml"),
//                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "haarcascade_frontalface_default.xml"),
//                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Data", "haarcascade_frontalface_default.xml"),

//                    // THƯ MỤC THẬT
//                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\UI\Resources\Data\haarcascade_frontalface_default.xml")
//                };

//                foreach (var p in candidates)
//                {
//                    if (File.Exists(p))
//                    {
//                        path = Path.GetFullPath(p);
//                        break;
//                    }
//                }
//            }

//            if (!File.Exists(path))
//                throw new FileNotFoundException($"Không tìm thấy file cascade tại: {path}");

//            return new CascadeClassifier(path);
//        }
//        // ======= Result DTO =======
//        public class FaceAuthResult
//        {
//            public bool Success { get; set; }
//            public string Message { get; set; }
//            public double Confidence { get; set; }  // 0..1 (chuẩn hoá để hiển thị)
//        }
//        // ==================== ĐĂNG KÝ ====================
//        public FaceAuthResult RegisterFace(int accountId, Bitmap frame)
//        {
//            if (frame == null)
//                return Fail("Không có khung hình từ camera", 0);

//            if (!TryExtractFacePatch(frame, out var grayFace, out string qualityMsg))
//                return Fail(qualityMsg, 0);

//            try
//            {
//                var norm = Preprocess(grayFace);
//                byte[] pngBytes = MatToPng(norm);
//                if (pngBytes == null || pngBytes.Length == 0)
//                    return Fail("Không thể lưu dữ liệu khuôn mặt", 0);

//                accountDao.UpdateFaceId(accountId, pngBytes, true, "Đã kích hoạt");
//                return new FaceAuthResult { Success = true, Message = "Đăng ký FaceID thành công", Confidence = 1.0 };
//            }
//            finally
//            {
//                grayFace?.Dispose();
//            }
//        }
//        // ==================== XÁC THỰC (1 ảnh) ====================
//        public FaceAuthResult VerifyFace(int accountId, Bitmap frame)
//        {
//            if (frame == null)
//                return Fail("Không có khung hình từ camera", 0);

//            var stored = accountDao.GetFaceIdData(accountId);
//            if (stored == null || stored.Length == 0)
//                return Fail("Tài khoản chưa đăng ký FaceID", 0);

//            if (!TryExtractFacePatch(frame, out var liveFace, out string qualityMsg))
//                return Fail(qualityMsg, 0);

//            try
//            {
//                var live = Preprocess(liveFace);

//                // --- Load mẫu khuôn mặt đã đăng ký ---
//                using var model = new LBPHFaceRecognizer(1, 8, 8, 8, double.MaxValue);
//                using var enrolled = PngToGrayMat(stored);
//                var enrolledPrep = Preprocess(enrolled);

//                // Train với 1 mẫu (label=1)
//                model.Train(new[] { enrolledPrep }, new[] { 1 });

//                // Predict bình thường
//                var pred = model.Predict(live);
//                double distance = pred.Distance;   // ✅ khai báo biến ở đây

//                // Predict ảnh lật ngang (giúp dễ khớp hơn nếu quay nhẹ)
//                using (var liveFlip = new Mat())
//                {
//                    CvInvoke.Flip(live, liveFlip, Emgu.CV.CvEnum.FlipType.Horizontal);
//                    var predFlip = model.Predict(liveFlip);
//                    distance = Math.Min(distance, predFlip.Distance);
//                }

//                // So ngưỡng nới lỏng
//                bool accept = (distance <= ThresholdDistance);

//                // Chuẩn hóa confidence (0–1) nhẹ tay hơn
//                double conf01 = Math.Max(0, Math.Min(1, 1.0 - (distance / (ThresholdDistance * 1.5))));

//                return new FaceAuthResult
//                {
//                    Success = accept,
//                    Message = accept ? "✔ Khuôn mặt khớp" : "❌ Khuôn mặt không khớp",
//                    Confidence = conf01
//                };
//            }
//            finally
//            {
//                liveFace?.Dispose();
//            }
//        }
//        // ==================== XÁC THỰC + LIVENESS (nhiều ảnh) ====================
//        public FaceAuthResult VerifyFaceWithLiveness(int accountId, IEnumerable<Bitmap> frames)
//        {
//            if (frames == null) return Fail("Không có khung hình từ camera", 0);

//            // ===== 1) chọn khung sắc nét nhất + gom vài khung tốt để dự đoán =====
//            var goodMats = new List<Mat>();         // dùng cho voting distance
//            Bitmap bestBmp = null; double bestSharp = double.NegativeInfinity;

//            foreach (var f in frames)
//            {
//                if (f == null) continue;
//                if (!TryExtractFacePatch(f, out var face, out _)) continue;
//                double sharp = EstimateSharpness(face);
//                if (sharp > bestSharp) { bestBmp?.Dispose(); bestBmp = (Bitmap)f.Clone(); bestSharp = sharp; }
//                // giữ lại tối đa 5 khung tốt để ensemble
//                if (goodMats.Count < 5) goodMats.Add(Preprocess(face));
//                face.Dispose();
//            }

//            if (bestBmp == null) return Fail("Không thấy mặt hoặc ảnh quá mờ", 0);

//            // ===== 2) kiểm tra chuyển động (liveness) — có thể tắt =====
//            if (RequireLiveness)
//            {
//                bool hasMotion = HasSlightMotion(frames);
//                if (!hasMotion)
//                {
//                    bestBmp.Dispose();
//                    return Fail("Không phát hiện chuyển động tự nhiên (nháy mắt/nhúc nhích)", 0);
//                }
//            }

//            // ===== 3) dự đoán — dùng ensemble để tăng tỉ lệ khớp =====
//            var rs = VerifyFaceEnsemble(accountId, bestBmp, goodMats);
//            bestBmp.Dispose();
//            foreach (var m in goodMats) m.Dispose();
//            return rs;
//        }

//        private FaceAuthResult VerifyFaceEnsemble(int accountId, Bitmap mainFrame, List<Mat> extraLiveMats)
//        {
//            var stored = accountDao.GetFaceIdData(accountId);
//            if (stored == null || stored.Length == 0) return Fail("Tài khoản chưa đăng ký FaceID", 0);

//            if (!TryExtractFacePatch(mainFrame, out var liveFace, out string msg))
//                return Fail(msg, 0);

//            using var live = Preprocess(liveFace);

//            // ⛔ Chặn kính đen (nếu bật)
//            if (BlockSunglasses && IsLikelySunglasses(live))
//            {
//                return new FaceAuthResult
//                {
//                    Success = false,
//                    Message = " Vui lòng tháo kính đen để xác thực.",
//                    Confidence = 0.0
//                };
//            }

//            // nạp mẫu đã đăng ký
//            using var model = new LBPHFaceRecognizer(1, 8, 8, 8, double.MaxValue);
//            using var enrolled = PngToGrayMat(stored);
//            var enrolledPrep = Preprocess(enrolled);
//            model.Train(new[] { enrolledPrep }, new[] { 1 });

//            // danh sách các live mats để đánh giá (main + extra + flip)
//            var candidates = new List<Mat> { live };
//            if (extraLiveMats != null) candidates.AddRange(extraLiveMats);

//            // thêm bản lật ngang cho mỗi ảnh
//            int n = candidates.Count;
//            for (int i = 0; i < n; i++)
//            {
//                var src = candidates[i];
//                var flip = new Mat();
//                CvInvoke.Flip(src, flip, Emgu.CV.CvEnum.FlipType.Horizontal);
//                candidates.Add(flip);
//            }
//            // lọc kính đen nếu cần
//            if (BlockSunglasses)
//            {
//                candidates = candidates.Where(m => !IsLikelySunglasses(m)).ToList();
//                if (candidates.Count == 0)
//                {
//                    return new FaceAuthResult
//                    {
//                        Success = false,
//                        Message = "Vui lòng tháo kính đen để xác thực.",
//                        Confidence = 0.0
//                    };
//                }
//            }
//            // lấy min distance trong tất cả dự đoán
//            double bestDist = double.MaxValue;
//            foreach (var c in candidates)
//            {
//                var pred = model.Predict(c);
//                if (pred.Distance < bestDist) bestDist = pred.Distance;
//            }

//            // quyết định
//            bool accept = (bestDist <= ThresholdDistance);
//            double conf01 = Math.Max(0, Math.Min(1, 1.0 - (bestDist / (ThresholdDistance * 1.5))));

//            // dọn flip extra
//            for (int i = n; i < candidates.Count; i++) candidates[i].Dispose();

//            return new FaceAuthResult
//            {
//                Success = accept,
//                Message = accept ? "✔ Khuôn mặt khớp" : "❌ Khuôn mặt không khớp",
//                Confidence = conf01
//            };
//        }
//        // ==================== Helpers ====================
//        private FaceAuthResult Fail(string message, double conf) => new FaceAuthResult
//        {
//            Success = false,
//            Message = message,
//            Confidence = conf
//        };

//        /// <summary>
//        /// Cắt khuôn mặt lớn nhất từ Bitmap, trả về Mat (grayscale). False nếu không thấy mặt/ảnh kém.
//        /// </summary>
//        //private bool TryExtractFacePatch(Bitmap frame, out Mat grayFace, out string message)
//        //{
//        //    grayFace = null; message = string.Empty;
//        //    try
//        //    {
//        //        using var gray = BitmapToGrayMat(frame);
//        //        var rects = FaceDetector.DetectMultiScale(gray, 1.08, 3, new Size(60, 60), Size.Empty);
//        //        if (rects == null || rects.Length == 0)
//        //        {
//        //            message = "Không thấy mặt – đưa mặt vào khung";
//        //            return false;
//        //        }
//        //        var best = LargestRect(rects);
//        //        using var roi = new Mat(gray, best);
//        //        if (!HasEnoughEdges(roi))
//        //        {
//        //            message = "Ảnh mờ/tối hoặc bị che – hãy tăng sáng & giữ yên";
//        //            return false;
//        //        }
//        //        grayFace = roi.Clone();
//        //        return true;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        message = "Lỗi phát hiện khuôn mặt: " + ex.Message;
//        //        grayFace?.Dispose(); grayFace = null;
//        //        return false;
//        //    }
//        //}
//        private bool TryExtractFacePatch(Bitmap frame, out Mat grayFace, out string message)
//        {
//            grayFace = null; message = string.Empty;
//            try
//            {
//                using var gray = BitmapToGrayMat(frame);

//                // Cascade nhẹ hơn → dễ nhận diện hơn
//                var rects = FaceDetector.DetectMultiScale(
//                    gray,
//                    1.03,      // từ 1.08 → 1.03
//                    2,         // từ 3 → 2
//                    new Size(50, 50),   // nhỏ hơn → dễ detect hơn
//                    Size.Empty
//                );

//                if (rects == null || rects.Length == 0)
//                {
//                    message = "Không thấy mặt – đưa mặt vào khung";
//                    return false;
//                }

//                var best = LargestRect(rects);
//                using var roi = new Mat(gray, best);

//                // Sharpness từ 6.0 → 3.0
//                if (!HasEnoughEdges(roi))
//                {
//                    message = "Ảnh hơi mờ — hãy giữ yên một chút";
//                    return false;
//                }

//                grayFace = roi.Clone();
//                return true;
//            }
//            catch
//            {
//                message = "Lỗi xử lý ảnh — thử lại";
//                return false;
//            }
//        }
//        private Rectangle LargestRect(Rectangle[] rects)
//        {
//            Rectangle best = rects[0];
//            int bestArea = best.Width * best.Height;
//            for (int i = 1; i < rects.Length; i++)
//            {
//                int area = rects[i].Width * rects[i].Height;
//                if (area > bestArea) { best = rects[i]; bestArea = area; }
//            }
//            return best;
//        }

//        private bool HasEnoughEdges(Mat grayFace)
//        {
//            using var edges = new Mat();
//            CvInvoke.Laplacian(grayFace, edges, DepthType.Cv64F);

//            MCvScalar mean = new MCvScalar();
//            MCvScalar std = new MCvScalar();
//            CvInvoke.MeanStdDev(edges, ref mean, ref std, (IInputArray)null);

//            // từ 6.0 → 3.0
//            return std.V0 >= 3.0;
//        }

//        private double EstimateSharpness(Mat grayFace)
//        {
//            using var edges = new Mat();
//            CvInvoke.Laplacian(grayFace, edges, DepthType.Cv64F);

//            MCvScalar mean = new MCvScalar();
//            MCvScalar std = new MCvScalar();
//            CvInvoke.MeanStdDev(edges, ref mean, ref std, (IInputArray)null);
//            // <- dùng ref
//            return std.V0;
//        }

//        private Mat Preprocess(Mat grayFace)
//        {
//            var target = new Mat();
//            CvInvoke.Resize(grayFace, target, new Size(160, 160), 0, 0, Emgu.CV.CvEnum.Inter.Linear);
//            // Thêm: làm mượt nhẹ để LBPH bớt nhạy với nhiễu/da hạt
//            CvInvoke.GaussianBlur(target, target, new Size(3, 3), 0);
//            if (UseHistogramEqualization)
//            {
//                CvInvoke.EqualizeHist(target, target);
//            }
//            return target;
//        }

//        private byte[] MatToPng(Mat mat)
//        {
//            try
//            {
//                using var buf = new VectorOfByte();
//                CvInvoke.Imencode(".png", mat, buf);
//                return buf.ToArray();
//            }
//            catch { return null; }
//        }
//        private Mat BitmapToGrayMat(Bitmap bmp)
//        {
//            using var ms = new MemoryStream();
//            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
//            using var buf = new VectorOfByte(ms.ToArray());
//            var dst = new Mat();
//            // ÉP KIỂU để chọn đúng overload nhận IInputArray
//            CvInvoke.Imdecode((IInputArray)buf, ImreadModes.Grayscale, dst);
//            return dst;
//        }
//        private Mat PngToGrayMat(byte[] bytes)
//        {
//            using var buf = new VectorOfByte(bytes);
//            var dst = new Mat();
//            CvInvoke.Imdecode((IInputArray)buf, ImreadModes.Grayscale, dst);
//            return dst;
//        }

//        private bool HasSlightMotion(IEnumerable<Bitmap> frames)
//        {
//            double prev = double.NaN; int moves = 0; int checkedCnt = 0;

//            foreach (var f in frames)
//            {
//                if (f == null) continue;
//                checkedCnt++;

//                using var gray = BitmapToGrayMat(f);     // <- KHÔNG truyền Bitmap trực tiếp
//                var m = CvInvoke.Mean(gray);
//                double val = m.V0;

//                if (!double.IsNaN(prev) && Math.Abs(val - prev) > 1.2) moves++;
//                prev = val;
//            }
//            return checkedCnt >= 3 && moves >= 1;
//        }

//        private bool IsLikelySunglasses(Mat preprocessedFace)
//        {
//            // preprocessedFace: ảnh GRAY 200x200 sau Preprocess
//            // Lấy nửa trên của mặt (vùng mắt)
//            var roiRect = new Rectangle(0, 0, preprocessedFace.Width, preprocessedFace.Height / 2);
//            using var top = new Mat(preprocessedFace, roiRect);

//            // 1) Độ sáng trung bình vùng mắt (0..255)
//            var meanTop = CvInvoke.Mean(top).V0;

//            // 2) Thử detect mắt bằng 2 cascade
//            var eyes1 = EyeDetector.DetectMultiScale(top, 1.1, 3, new Size(18, 12), Size.Empty);
//            var eyes2 = EyeGlassDetector.DetectMultiScale(top, 1.1, 3, new Size(18, 12), Size.Empty);
//            int eyeCount = (eyes1?.Length ?? 0) + (eyes2?.Length ?? 0);

//            // 3) Tiêu chí “kính đen”: quá tối & không thấy mắt
//            // Ngưỡng gợi ý: meanTop < 60 (rất tối). Bạn có thể chỉnh 55–70.
//            bool tooDark = meanTop < 60.0;
//            bool noEyes = eyeCount == 0;

//            return tooDark && noEyes;
//        }

//    }
//}
using EMC.DAO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace EMC.Service
{
    /// <summary>
    /// FaceAuthService (Emgu.CV + LBPH) – phiên bản tương thích EmguCV không cần Bitmap.ToMat()
    /// - Đăng ký: detect largest face → gray + equalize + resize → lưu PNG (VARBINARY)
    /// - Xác thực: load PNG đã lưu → train LBPH → predict ảnh live
    /// - Liveness (đơn giản): chọn khung sắc nét nhất trong batch, kiểm tra có chuyển động nhỏ giữa các khung
    /// - Trả Confidence=0.0 khi không thấy mặt hoặc ảnh quá mờ/che
    /// </summary>
    public class FaceAuthService
    {
        private static FaceAuthService instance;
        public static FaceAuthService Instance => instance ??= new FaceAuthService();

        private readonly AccountDAO accountDao = new AccountDAO();

        // ======= Config =======
        public string CascadePath { get; set; } =
    Path.Combine(Application.StartupPath, "UI", "Resources", "Data", "haarcascade_frontalface_default.xml");


        public int EnrollSize { get; set; } = 200;                // Kích thước ảnh chuẩn
        public double ThresholdDistance { get; set; } = 75.0;     // Khoảng cách LBPH càng THẤP càng tốt có thể thử 70–85
        public bool UseHistogramEqualization { get; set; } = true;

        public bool RequireLiveness { get; set; } = false;   // <- mặc định tắt
        public double MotionThreshold { get; set; } = 0.35;   // 0.35–0.55 (thấp -> dễ qua)

        public bool BlockSunglasses { get; set; } = false;   // bật chặn kính đen
        public string EyeCascadePath { get; set; } =
    Path.Combine(Application.StartupPath, "UI", "Resources", "Data", "haarcascade_eye.xml");
        public string EyeGlassCascadePath { get; set; } =
    Path.Combine(Application.StartupPath, "UI", "Resources", "Data", "haarcascade_eye_tree_eyeglasses.xml");

        private CascadeClassifier eyeDetector, eyeGlassDetector;
        private CascadeClassifier EyeDetector => eyeDetector ??= LoadCascade(EyeCascadePath);
        private CascadeClassifier EyeGlassDetector => eyeGlassDetector ??= LoadCascade(EyeGlassCascadePath);

        private CascadeClassifier faceDetector;
        private CascadeClassifier FaceDetector => faceDetector ??= LoadCascade(CascadePath);

        private CascadeClassifier LoadCascade(string path)
        {

            // Nếu chưa có file tại path mặc định
            if (!File.Exists(path))
            {
                // Dò theo 3 hướng phổ biến và thư mục thật của bạn
                var candidates = new[]
                {
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haarcascade_frontalface_default.xml"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "haarcascade_frontalface_default.xml"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Data", "haarcascade_frontalface_default.xml"),
            
                    // THƯ MỤC THẬT
                    Path.Combine(Application.StartupPath, "UI", "Resources", "Data", "haarcascade_frontalface_default.xml")

                };

                foreach (var p in candidates)
                {
                    if (File.Exists(p))
                    {
                        path = Path.GetFullPath(p);
                        break;
                    }
                }
            }

            if (!File.Exists(path))
                throw new FileNotFoundException($"Không tìm thấy file cascade tại: {path}");

            return new CascadeClassifier(path);
        }
        // ======= Result DTO =======
        public class FaceAuthResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public double Confidence { get; set; }  // 0..1 (chuẩn hoá để hiển thị)
        }
        // ==================== ĐĂNG KÝ ====================
        public FaceAuthResult RegisterFace(int accountId, Bitmap frame)
        {
            if (frame == null)
                return Fail("Không có khung hình từ camera", 0);

            if (!TryExtractFacePatch(frame, out var grayFace, out string qualityMsg))
                return Fail(qualityMsg, 0);

            try
            {
                var norm = Preprocess(grayFace);
                byte[] pngBytes = MatToPng(norm);
                if (pngBytes == null || pngBytes.Length == 0)
                    return Fail("Không thể lưu dữ liệu khuôn mặt", 0);

                accountDao.UpdateFaceId(accountId, pngBytes, true, "Đã kích hoạt");
                return new FaceAuthResult { Success = true, Message = "Đăng ký FaceID thành công", Confidence = 1.0 };
            }
            finally
            {
                grayFace?.Dispose();
            }
        }
        // ==================== XÁC THỰC (1 ảnh) ====================
        public FaceAuthResult VerifyFace(int accountId, Bitmap frame)
        {
            if (frame == null)
                return Fail("Không có khung hình từ camera", 0);

            var stored = accountDao.GetFaceIdData(accountId);
            if (stored == null || stored.Length == 0)
                return Fail("Tài khoản chưa đăng ký FaceID", 0);

            if (!TryExtractFacePatch(frame, out var liveFace, out string qualityMsg))
                return Fail(qualityMsg, 0);

            try
            {
                var live = Preprocess(liveFace);

                // --- Load mẫu khuôn mặt đã đăng ký ---
                using var model = new LBPHFaceRecognizer(1, 8, 8, 8, double.MaxValue);
                using var enrolled = PngToGrayMat(stored);
                var enrolledPrep = Preprocess(enrolled);

                // Train với 1 mẫu (label=1)
                model.Train(new[] { enrolledPrep }, new[] { 1 });

                // Predict bình thường
                var pred = model.Predict(live);
                double distance = pred.Distance;   // ✅ khai báo biến ở đây

                // Predict ảnh lật ngang (giúp dễ khớp hơn nếu quay nhẹ)
                using (var liveFlip = new Mat())
                {
                    CvInvoke.Flip(live, liveFlip, Emgu.CV.CvEnum.FlipType.Horizontal);
                    var predFlip = model.Predict(liveFlip);
                    distance = Math.Min(distance, predFlip.Distance);
                }

                // So ngưỡng nới lỏng
                bool accept = (distance <= ThresholdDistance);

                // Chuẩn hóa confidence (0–1) nhẹ tay hơn
                double conf01 = Math.Max(0, Math.Min(1, 1.0 - (distance / (ThresholdDistance * 1.5))));

                return new FaceAuthResult
                {
                    Success = accept,
                    Message = accept ? "✔ Khuôn mặt khớp" : "❌ Khuôn mặt không khớp",
                    Confidence = conf01
                };
            }
            finally
            {
                liveFace?.Dispose();
            }
        }
        // ==================== XÁC THỰC + LIVENESS (nhiều ảnh) ====================
        public FaceAuthResult VerifyFaceWithLiveness(int accountId, IEnumerable<Bitmap> frames)
        {
            if (frames == null) return Fail("Không có khung hình từ camera", 0);

            // ===== 1) chọn khung sắc nét nhất + gom vài khung tốt để dự đoán =====
            var goodMats = new List<Mat>();         // dùng cho voting distance
            Bitmap bestBmp = null; double bestSharp = double.NegativeInfinity;

            foreach (var f in frames)
            {
                if (f == null) continue;
                if (!TryExtractFacePatch(f, out var face, out _)) continue;
                double sharp = EstimateSharpness(face);
                if (sharp > bestSharp) { bestBmp?.Dispose(); bestBmp = (Bitmap)f.Clone(); bestSharp = sharp; }
                // giữ lại tối đa 5 khung tốt để ensemble
                if (goodMats.Count < 5) goodMats.Add(Preprocess(face));
                face.Dispose();
            }

            if (bestBmp == null) return Fail("Không thấy mặt hoặc ảnh quá mờ", 0);

            // ===== 2) kiểm tra chuyển động (liveness) — có thể tắt =====
            if (RequireLiveness)
            {
                bool hasMotion = HasSlightMotion(frames);
                if (!hasMotion)
                {
                    bestBmp.Dispose();
                    return Fail("Không phát hiện chuyển động tự nhiên (nháy mắt/nhúc nhích)", 0);
                }
            }

            // ===== 3) dự đoán — dùng ensemble để tăng tỉ lệ khớp =====
            var rs = VerifyFaceEnsemble(accountId, bestBmp, goodMats);
            bestBmp.Dispose();
            foreach (var m in goodMats) m.Dispose();
            return rs;
        }

        private FaceAuthResult VerifyFaceEnsemble(int accountId, Bitmap mainFrame, List<Mat> extraLiveMats)
        {
            var stored = accountDao.GetFaceIdData(accountId);
            if (stored == null || stored.Length == 0) return Fail("Tài khoản chưa đăng ký FaceID", 0);

            if (!TryExtractFacePatch(mainFrame, out var liveFace, out string msg))
                return Fail(msg, 0);

            using var live = Preprocess(liveFace);

            // ⛔ Chặn kính đen (nếu bật)
            if (BlockSunglasses && IsLikelySunglasses(live))
            {
                return new FaceAuthResult
                {
                    Success = false,
                    Message = " Vui lòng tháo kính đen để xác thực.",
                    Confidence = 0.0
                };
            }

            // nạp mẫu đã đăng ký
            using var model = new LBPHFaceRecognizer(1, 8, 8, 8, double.MaxValue);
            using var enrolled = PngToGrayMat(stored);
            var enrolledPrep = Preprocess(enrolled);
            model.Train(new[] { enrolledPrep }, new[] { 1 });

            // danh sách các live mats để đánh giá (main + extra + flip)
            var candidates = new List<Mat> { live };
            if (extraLiveMats != null) candidates.AddRange(extraLiveMats);

            // thêm bản lật ngang cho mỗi ảnh
            int n = candidates.Count;
            for (int i = 0; i < n; i++)
            {
                var src = candidates[i];
                var flip = new Mat();
                CvInvoke.Flip(src, flip, Emgu.CV.CvEnum.FlipType.Horizontal);
                candidates.Add(flip);
            }
            // lọc kính đen nếu cần
            if (BlockSunglasses)
            {
                candidates = candidates.Where(m => !IsLikelySunglasses(m)).ToList();
                if (candidates.Count == 0)
                {
                    return new FaceAuthResult
                    {
                        Success = false,
                        Message = "Vui lòng tháo kính đen để xác thực.",
                        Confidence = 0.0
                    };
                }
            }
            // lấy min distance trong tất cả dự đoán
            double bestDist = double.MaxValue;
            foreach (var c in candidates)
            {
                var pred = model.Predict(c);
                if (pred.Distance < bestDist) bestDist = pred.Distance;
            }

            // quyết định
            bool accept = (bestDist <= ThresholdDistance);
            double conf01 = Math.Max(0, Math.Min(1, 1.0 - (bestDist / (ThresholdDistance * 1.5))));

            // dọn flip extra
            for (int i = n; i < candidates.Count; i++) candidates[i].Dispose();

            return new FaceAuthResult
            {
                Success = accept,
                Message = accept ? "✔ Khuôn mặt khớp" : "❌ Khuôn mặt không khớp",
                Confidence = conf01
            };
        }
        // ==================== Helpers ====================
        private FaceAuthResult Fail(string message, double conf) => new FaceAuthResult
        {
            Success = false,
            Message = message,
            Confidence = conf
        };

        /// <summary>
        /// Cắt khuôn mặt lớn nhất từ Bitmap, trả về Mat (grayscale). False nếu không thấy mặt/ảnh kém.
        /// </summary>
        //private bool TryExtractFacePatch(Bitmap frame, out Mat grayFace, out string message)
        //{
        //    grayFace = null; message = string.Empty;
        //    try
        //    {
        //        using var gray = BitmapToGrayMat(frame);
        //        var rects = FaceDetector.DetectMultiScale(gray, 1.08, 3, new Size(60, 60), Size.Empty);
        //        if (rects == null || rects.Length == 0)
        //        {
        //            message = "Không thấy mặt – đưa mặt vào khung";
        //            return false;
        //        }
        //        var best = LargestRect(rects);
        //        using var roi = new Mat(gray, best);
        //        if (!HasEnoughEdges(roi))
        //        {
        //            message = "Ảnh mờ/tối hoặc bị che – hãy tăng sáng & giữ yên";
        //            return false;
        //        }
        //        grayFace = roi.Clone();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        message = "Lỗi phát hiện khuôn mặt: " + ex.Message;
        //        grayFace?.Dispose(); grayFace = null;
        //        return false;
        //    }
        //}
        private bool TryExtractFacePatch(Bitmap frame, out Mat grayFace, out string message)
        {
            grayFace = null; message = string.Empty;
            try
            {
                using var gray0 = BitmapToGrayMat(frame);

                // ⭐ Tự tăng sáng 35%
                using var gray = BoostBrightness(gray0, 1.35);

                // Cascade nhẹ hơn → dễ nhận diện hơn
                var rects = FaceDetector.DetectMultiScale(
                    gray,
                    1.03,      // từ 1.08 → 1.03
                    2,         // từ 3 → 2
                    new Size(50, 50),   // nhỏ hơn → dễ detect hơn
                    Size.Empty
                );

                if (rects == null || rects.Length == 0)
                {
                    message = "Không thấy mặt – đưa mặt vào khung";
                    return false;
                }

                var best = LargestRect(rects);
                using var roi = new Mat(gray, best);

                // Sharpness từ 6.0 → 3.0
                if (!HasEnoughEdges(roi))
                {
                    message = "Ảnh hơi mờ — hãy giữ yên một chút";
                    return false;
                }

                grayFace = roi.Clone();
                return true;
            }
            catch (Exception ex)
            {
                message = "Lỗi xử lý ảnh: " + ex.Message;
                return false;
            }
        }
        private Rectangle LargestRect(Rectangle[] rects)
        {
            Rectangle best = rects[0];
            int bestArea = best.Width * best.Height;
            for (int i = 1; i < rects.Length; i++)
            {
                int area = rects[i].Width * rects[i].Height;
                if (area > bestArea) { best = rects[i]; bestArea = area; }
            }
            return best;
        }

        private bool HasEnoughEdges(Mat grayFace)
        {
            using var edges = new Mat();
            CvInvoke.Laplacian(grayFace, edges, DepthType.Cv64F);

            MCvScalar mean = new MCvScalar();
            MCvScalar std = new MCvScalar();
            CvInvoke.MeanStdDev(edges, ref mean, ref std, (IInputArray)null);

            // từ 6.0 → 3.0
            return std.V0 >= 2.2;
        }

        private double EstimateSharpness(Mat grayFace)
        {
            using var edges = new Mat();
            CvInvoke.Laplacian(grayFace, edges, DepthType.Cv64F);

            MCvScalar mean = new MCvScalar();
            MCvScalar std = new MCvScalar();
            CvInvoke.MeanStdDev(edges, ref mean, ref std, (IInputArray)null);
            // <- dùng ref
            return std.V0;
        }

        private Mat Preprocess(Mat grayFace)
        {
            var target = new Mat();
            CvInvoke.Resize(grayFace, target, new Size(160, 160), 0, 0, Emgu.CV.CvEnum.Inter.Linear);
            // Thêm: làm mượt nhẹ để LBPH bớt nhạy với nhiễu/da hạt
            CvInvoke.GaussianBlur(target, target, new Size(3, 3), 0);
            if (UseHistogramEqualization)
            {
                CvInvoke.EqualizeHist(target, target);
            }
            return target;
        }

        private byte[] MatToPng(Mat mat)
        {
            try
            {
                using var buf = new VectorOfByte();
                CvInvoke.Imencode(".png", mat, buf);
                return buf.ToArray();
            }
            catch { return null; }
        }
        private Mat BitmapToGrayMat(Bitmap bmp)
        {
            // Convert Bitmap to Mat using Emgu.CV's Bitmap constructor
            Mat mat = Emgu.CV.BitmapExtension.ToMat(bmp);
            Mat gray = new Mat();
            CvInvoke.CvtColor(mat, gray, ColorConversion.Bgr2Gray);
            mat.Dispose();
            return gray;
        }



        private Mat PngToGrayMat(byte[] bytes)
        {
            using var buf = new VectorOfByte(bytes);
            var dst = new Mat();
            CvInvoke.Imdecode((IInputArray)buf, ImreadModes.Grayscale, dst);
            return dst;
        }

        private bool HasSlightMotion(IEnumerable<Bitmap> frames)
        {
            double prev = double.NaN; int moves = 0; int checkedCnt = 0;

            foreach (var f in frames)
            {
                if (f == null) continue;
                checkedCnt++;

                using var gray = BitmapToGrayMat(f);     // <- KHÔNG truyền Bitmap trực tiếp
                var m = CvInvoke.Mean(gray);
                double val = m.V0;

                if (!double.IsNaN(prev) && Math.Abs(val - prev) > 1.2) moves++;
                prev = val;
            }
            return checkedCnt >= 3 && moves >= 1;
        }

        private bool IsLikelySunglasses(Mat preprocessedFace)
        {
            // preprocessedFace: ảnh GRAY 200x200 sau Preprocess
            // Lấy nửa trên của mặt (vùng mắt)
            var roiRect = new Rectangle(0, 0, preprocessedFace.Width, preprocessedFace.Height / 2);
            using var top = new Mat(preprocessedFace, roiRect);

            // 1) Độ sáng trung bình vùng mắt (0..255)
            var meanTop = CvInvoke.Mean(top).V0;

            // 2) Thử detect mắt bằng 2 cascade
            var eyes1 = EyeDetector.DetectMultiScale(top, 1.1, 3, new Size(18, 12), Size.Empty);
            var eyes2 = EyeGlassDetector.DetectMultiScale(top, 1.1, 3, new Size(18, 12), Size.Empty);
            int eyeCount = (eyes1?.Length ?? 0) + (eyes2?.Length ?? 0);

            // 3) Tiêu chí “kính đen”: quá tối & không thấy mắt
            // Ngưỡng gợi ý: meanTop < 60 (rất tối). Bạn có thể chỉnh 55–70.
            bool tooDark = meanTop < 60.0;
            bool noEyes = eyeCount == 0;

            return tooDark && noEyes;
        }
        private Mat BoostBrightness(Mat gray, double factor = 1.35)
        {
            var result = new Mat();
            gray.ConvertTo(result, DepthType.Cv8U, factor, 0);
            return result;
        }
    }
}
