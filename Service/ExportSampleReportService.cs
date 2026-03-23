using EMC.DTO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.Text;
using iText = iTextSharp.text;
using iTextPdf = iTextSharp.text.pdf;

namespace EMC.Service
{
    public class ExportSampleReportService
    {
        private static readonly Lazy<ExportSampleReportService> instance
            = new Lazy<ExportSampleReportService>(() => new ExportSampleReportService());

        public static ExportSampleReportService Instance => instance.Value;

        // ================================================================
        // 🎨 LOGO CACHE ĐỘNG (tương tự UIWatermark)
        // ================================================================
        private static iText.Image cachedLogo;
        private static string cachedLogoPath;
        private static DateTime cachedLogoWrite;
        private static readonly object logoCacheLock = new object();

        /// <summary>
        /// Lấy logo từ UI\Resources\uploads\logo, ưu tiên logo.png
        /// Nếu không có, lấy file ảnh đầu tiên trong thư mục đó
        /// </summary>
        private iText.Image GetDynamicLogo()
        {
            try
            {
                string logoDirPath = ResolveLogoDirPath();
                if (!Directory.Exists(logoDirPath))
                {
                    System.Diagnostics.Debug.WriteLine($"Logo directory not found: {logoDirPath}");
                    return null;
                }

                string preferredLogo = Path.Combine(logoDirPath, "logo.png");
                string logoPath = null;

                if (File.Exists(preferredLogo))
                {
                    logoPath = preferredLogo;
                }
                else
                {
                    var patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp", "*.gif" };
                    foreach (var pattern in patterns)
                    {
                        var files = Directory.GetFiles(logoDirPath, pattern, SearchOption.TopDirectoryOnly);
                        if (files.Length > 0)
                        {
                            logoPath = files[0];
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(logoPath) || !File.Exists(logoPath))
                {
                    System.Diagnostics.Debug.WriteLine($"No logo file found in: {logoDirPath}");
                    return null;
                }

                var lastWrite = File.GetLastWriteTime(logoPath);
                lock (logoCacheLock)
                {
                    if (cachedLogo != null &&
                        string.Equals(cachedLogoPath, logoPath, StringComparison.OrdinalIgnoreCase) &&
                        lastWrite == cachedLogoWrite)
                    {
                        return cachedLogo;
                    }

                    byte[] logoData = LoadImageBytes(logoPath);
                    if (logoData != null && logoData.Length > 0)
                    {
                        cachedLogo = iText.Image.GetInstance(logoData);
                        cachedLogoPath = logoPath;
                        cachedLogoWrite = lastWrite;
                        return cachedLogo;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetDynamicLogo error: {ex.Message}");
                return null;
            }
        }

        private byte[] LoadImageBytes(string filePath)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadImageBytes error: {ex.Message}");
                return null;
            }
        }

        private string ResolveLogoDirPath()
        {
            //string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            //string projectDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..", "UI", "Resources", "uploads", "logo"));

            //if (Directory.Exists(projectDir))
            //    return projectDir;

            //string[] candidates = new[]
            //{
            //    baseDir,
            //    Path.Combine(baseDir, @"..\..\..", "UI", "Resources", "images"),
            //    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "images")
            //};

            //foreach (var candidate in candidates)
            //{
            //    if (Directory.Exists(candidate))
            //        return candidate;
            //}

            return UIWatermark.PermanentDir;
        }

        public static void InvalidateLogoCacheOnChange()
        {
            lock (logoCacheLock)
            {
                cachedLogo = null;
                cachedLogoPath = null;
                cachedLogoWrite = default;
            }
        }

        // ================================================================
        // 📄 XUẤT REPORT
        // ================================================================
        public bool ExportReport(ExportData data, string outputPath, string format)
        {
            try
            {
                if (data == null || data.SampleInfo == null)
                    throw new ArgumentNullException(nameof(data));

                DateTime now = DateTime.Now;

                if (data.ResultInfo != null)
                {
                    data.ResultInfo.EmailedDate = now;

                    if (data.ResultInfo.Id > 0)
                    {
                        ResultService.Instance.UpdateEmailedDate(data.ResultInfo.Id, now);
                    }
                }

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                format = format?.ToLower() ?? "";

                return format switch
                {
                    "pdf" => ExportToPdf(data, outputPath),
                    _ => false
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Export error: {ex.Message}");
                return false;
            }
        }

        // ================================================================
        // 🧾 XUẤT PDF NHIỀU MẪU VỚI SỐ TRANG THEO LOẠI MẪU
        // ================================================================
        public bool ExportMultiSamplesToSinglePdf(List<ExportData> allSamples, string outputPath)
        {
            if (allSamples == null || allSamples.Count == 0)
                return false;

            try
            {
                // ✅ BƯỚC 1: Nhóm theo SampleTypeId và chia subgroup
                var sampleGroups = allSamples
                    .GroupBy(s => s.SampleInfo.SampleTypeId)
                    .OrderBy(g => g.Key)
                    .ToList();

                var allSubGroups = new List<List<ExportData>>();

                foreach (var group in sampleGroups)
                {
                    var samplesInGroup = group.ToList();
                    var subGroups = SplitIntoSubGroups(samplesInGroup, 5);
                    allSubGroups.AddRange(subGroups);
                }

                var firstData = allSamples.First();

                // ⭐ BƯỚC 2: Render lần 1 để ĐẾM TRANG CHÍNH XÁC
                var reportPageCounts = new Dictionary<int, int>();

                for (int i = 0; i < allSubGroups.Count; i++)
                {
                    using (var ms = new MemoryStream())
                    {
                        var doc = new iText.Document(iText.PageSize.A4, 20, 20, 20, 20);
                        var writer = iTextPdf.PdfWriter.GetInstance(doc, ms);

                        doc.Open();
                        RenderGroupContent(doc, writer, firstData, allSubGroups[i]);
                        doc.Close();

                        // ⭐ Đếm trang bằng PdfReader – LUÔN ĐÚNG
                        var reader = new iTextPdf.PdfReader(ms.ToArray());
                        reportPageCounts[i] = reader.NumberOfPages;
                        reader.Close();
                    }
                }

                // ✅ BƯỚC 3: Render lần 2 với đánh số trang đúng
                var pageEventHandler = new ReportPageNumberEventHandler(reportPageCounts);

                iText.Document docFinal = new iText.Document(iText.PageSize.A4, 20, 20, 20, 20);
                using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var writerFinal = iTextPdf.PdfWriter.GetInstance(docFinal, fs);
                    writerFinal.PageEvent = pageEventHandler;

                    docFinal.Open();

                    for (int i = 0; i < allSubGroups.Count; i++)
                    {
                        // ⭐ 1: Luôn tạo trang mới TRƯỚC khi gán index
                        docFinal.NewPage();

                        // ⭐ 2: Gán report index SAU khi NewPage
                        pageEventHandler.SetReportIndex(i);

                        // ⭐ 3: Render nội dung phiếu
                        RenderGroupContent(docFinal, writerFinal, firstData, allSubGroups[i]);
                    }


                    docFinal.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Multi PDF export failed: {ex.Message}");
                return false;
            }
        }


        public class ReportPageNumberEventHandler : iTextPdf.PdfPageEventHelper
        {
            private Dictionary<int, int> _reportPageCounts; // reportIdx -> totalPages
            private int _currentReportIdx = 0;
            private int _currentPageInReport = 0;
            private int _totalPagesInReport = 1;


            public ReportPageNumberEventHandler(Dictionary<int, int> reportPageCounts)
            {
                _reportPageCounts = reportPageCounts;
            }

            public void SetReportIndex(int idx)
            {
                _currentReportIdx = idx;
                _currentPageInReport = 1; // reset trang
                _totalPagesInReport = _reportPageCounts[idx]; // set tổng trang đúng
            }


            public override void OnStartPage(iTextPdf.PdfWriter writer, iText.Document document)
            {
                _currentPageInReport++;
                base.OnStartPage(writer, document);
            }
            private void AddWatermark(PdfWriter writer, Document document)
            {
                try
                {
                    var logo = ExportSampleReportService.Instance.GetType()
                        .GetMethod("GetDynamicLogo", System.Reflection.BindingFlags.NonPublic |
                                                   System.Reflection.BindingFlags.Instance)
                        ?.Invoke(ExportSampleReportService.Instance, null) as iText.Image;

                    if (logo == null) return;

                    float pageWidth = document.PageSize.Width;
                    float pageHeight = document.PageSize.Height;

                    var scaledLogo = iText.Image.GetInstance(logo);
                    scaledLogo.ScalePercent(30);

                    float x = (pageWidth - scaledLogo.ScaledWidth) / 2;
                    float y = (pageHeight - scaledLogo.ScaledHeight) / 2;
                    scaledLogo.SetAbsolutePosition(x, y);

                    var canvas = writer.DirectContentUnder;
                    var gState = new PdfGState { FillOpacity = 0.15f };
                    canvas.SaveState();
                    canvas.SetGState(gState);
                    canvas.AddImage(scaledLogo);
                    canvas.RestoreState();
                }
                catch { }
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                string text = $"{_currentPageInReport}/{_totalPagesInReport}";

                var font = GetFont(9);
                var cb = writer.DirectContent;
                cb.BeginText();
                cb.SetFontAndSize(font.BaseFont, font.Size);

                cb.SetTextMatrix(document.PageSize.Width / 2 - 10, 20);
                cb.ShowText(text);

                cb.EndText();

                base.OnEndPage(writer, document);
                AddWatermark(writer, document);

            }

            private iTextSharp.text.Font GetFont(int size)
            {
                string fontPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
                    "times.ttf");

                var bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                return new iTextSharp.text.Font(bf, size, iTextSharp.text.Font.NORMAL);
            }



        }

        private void RenderGroupContent(iText.Document doc, iTextPdf.PdfWriter writer, ExportData firstData, List<ExportData> subGroup)
        {

            AddPdfHeader(doc, firstData);
            AddPdfGeneralInfo(doc, subGroup);
            AddDynamicLogoToPage(writer);

            doc.Add(new iText.Paragraph(" "));
            var resultsTitle = new iText.Paragraph(
                "II. KẾT QUẢ PHÂN TÍCH",
                GetUnicodeFont(12, iText.Font.BOLD))
            {
                Alignment = iText.Element.ALIGN_LEFT,
                SpacingBefore = 5,
                SpacingAfter = 10
            };
            doc.Add(resultsTitle);

            AddCombinedParameterTable(doc, subGroup);
            AddPdfNotes(doc, subGroup);
            AddPdfSignature(doc, firstData);
        }

        // ================================================================
        // 🧾 XUẤT PDF ĐƠN VỚI ĐÁNH SỐ TRANG
        // ================================================================
        private bool ExportToPdf(ExportData data, string outputPath)
        {
            try
            {
                int totalPages = 0;
                iText.Document doc1 = new iText.Document(iText.PageSize.A4, 20, 20, 20, 20);
                using (var ms = new MemoryStream())
                {
                    var writer1 = iTextPdf.PdfWriter.GetInstance(doc1, ms);
                    doc1.Open();

                    AddCenterWatermark(writer1);
                    AddPdfHeader(doc1, data);
                    AddPdfSampleInfo(doc1, data);
                    AddPdfParameterTable(doc1, data);
                    AddDynamicLogoToPage(writer1);
                    AddPdfNotesForSingleSample(doc1, data);
                    AddPdfSignature(doc1, data);

                    doc1.Close();
                    totalPages = writer1.PageNumber;
                }



                iText.Document doc2 = new iText.Document(iText.PageSize.A4, 20, 20, 20, 20);
                using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var writer2 = iTextPdf.PdfWriter.GetInstance(doc2, fs);


                    doc2.Open();

                    AddCenterWatermark(writer2);
                    AddPdfHeader(doc2, data);
                    AddPdfSampleInfo(doc2, data);
                    AddPdfParameterTable(doc2, data);
                    AddDynamicLogoToPage(writer2);
                    AddPdfNotesForSingleSample(doc2, data);
                    AddPdfSignature(doc2, data);

                    doc2.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PDF export failed: {ex.Message}");
                return false;
            }
        }


        // ================================================================
        // 🔢 PHƯƠNG THỨC CHIA NHÓM MẪU
        // ================================================================
        private List<List<ExportData>> SplitIntoSubGroups(List<ExportData> samples, int maxPerGroup)
        {
            var result = new List<List<ExportData>>();

            for (int i = 0; i < samples.Count; i += maxPerGroup)
            {
                var subGroup = samples.Skip(i).Take(maxPerGroup).ToList();
                result.Add(subGroup);
            }

            return result;
        }



        // 🎨 THÊM LOGO ĐỘNG VÀO TRANG
        // ================================================================
        private void AddDynamicLogoToPage(iTextPdf.PdfWriter writer)
        {
            try
            {
                var logo = GetDynamicLogo();
                if (logo == null)
                {
                    System.Diagnostics.Debug.WriteLine("Dynamic logo not available");
                    return;
                }

                logo.SetAbsolutePosition(15, 740);
                logo.ScalePercent(10);
                logo.GrayFill = 0.9f;
                writer.DirectContentUnder.AddImage(logo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Add dynamic logo error: {ex.Message}");
            }
        }

        // ================================================================
        // 🌊 WATERMARK Ở GIỮA
        // ================================================================
        private void AddCenterWatermark(iTextPdf.PdfWriter writer)
        {
            try
            {
                var logo = GetDynamicLogo();
                if (logo == null)
                    return;

                float pageWidth = iText.PageSize.A4.Width;
                float pageHeight = iText.PageSize.A4.Height;

                var scaledLogo = iText.Image.GetInstance(logo);
                scaledLogo.ScalePercent(30);

                float x = (pageWidth - scaledLogo.ScaledWidth) / 2;
                float y = (pageHeight - scaledLogo.ScaledHeight) / 2;
                scaledLogo.SetAbsolutePosition(x, y);

                var canvas = writer.DirectContentUnder;
                var gState = new iTextPdf.PdfGState { FillOpacity = 0.15f, StrokeOpacity = 0.15f };
                canvas.SaveState();
                canvas.SetGState(gState);
                canvas.AddImage(scaledLogo);
                canvas.RestoreState();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Watermark error: {ex.Message}");
            }
        }

        // ================================================================
        // 📝 GHI CHÚ & BẢNG (giữ nguyên từ file gốc)
        // ================================================================
        private void AddPdfNotes(iText.Document doc, List<ExportData> currentSubGroup)
        {
            var titleFont = GetUnicodeFont(11, iText.Font.BOLD);
            var sectionFont = GetUnicodeFont(9, iText.Font.BOLD);
            var normalFont = GetUnicodeFont(9);
            var italicFont = GetUnicodeFont(9, iText.Font.ITALIC);

            doc.Add(new iText.Paragraph(" "));
            doc.Add(new iText.Paragraph("Ghi chú:", titleFont));
            doc.Add(new iText.Paragraph(" ", GetUnicodeFont(4)));

            var notesParagraph = new iText.Paragraph();
            notesParagraph.Add(new iText.Chunk("1. Vị trí lấy mẫu:", sectionFont));
            notesParagraph.Add(iText.Chunk.NEWLINE);

            foreach (var sample in currentSubGroup)
            {
                string coordinates = "";
                if (sample.SampleInfo.Longitude.HasValue && sample.SampleInfo.Latitude.HasValue)
                {
                    coordinates = $"     Tọa độ: X: {sample.SampleInfo.Longitude:F7}     Y: {sample.SampleInfo.Latitude:F6}";
                }

                notesParagraph.Add(new iText.Chunk($"- {sample.SampleInfo.SampleCode}: ", sectionFont));
                notesParagraph.Add(new iText.Chunk(sample.SampleInfo.PositionSite + coordinates ?? "", normalFont));
                notesParagraph.Add(iText.Chunk.NEWLINE);
            }

            doc.Add(notesParagraph);

            var standardParagraph = new iText.Paragraph();
            standardParagraph.Add(new iText.Chunk("2. Quy chuẩn so sánh:", sectionFont));
            standardParagraph.Add(iText.Chunk.NEWLINE);

            standardParagraph.Add(new iText.Chunk("QCVN 05:2023/BTNMT: ", sectionFont));
            standardParagraph.Add(new iText.Chunk(
                "Quy chuẩn kỹ thuật quốc gia về chất lượng không khí (Bảng 1: giá trị giới hạn tối đa các thông số cơ bản trong không khí xung quanh-TB 1h)",
                italicFont));
            standardParagraph.Add(iText.Chunk.NEWLINE);

            standardParagraph.Add(new iText.Chunk("(1): QCVN 26:2010/BTNMT: ", sectionFont));
            standardParagraph.Add(new iText.Chunk("Quy chuẩn kỹ thuật quốc gia về tiếng ồn", italicFont));
            standardParagraph.Add(iText.Chunk.NEWLINE);

            standardParagraph.Add(new iText.Chunk("(2): QCVN 27:2010/BTNMT: ", sectionFont));
            standardParagraph.Add(new iText.Chunk("Quy chuẩn kỹ thuật quốc gia về độ rung", italicFont));

            doc.Add(standardParagraph);
            doc.Add(new iText.Paragraph(" "));
        }

        private void AddPdfNotesForSingleSample(iText.Document doc, ExportData data)
        {
            var titleFont = GetUnicodeFont(11, iText.Font.BOLD);
            var sectionFont = GetUnicodeFont(9, iText.Font.BOLD);
            var normalFont = GetUnicodeFont(9);
            var italicFont = GetUnicodeFont(9, iText.Font.ITALIC);

            doc.Add(new iText.Paragraph(" "));
            doc.Add(new iText.Paragraph("Ghi chú:", titleFont));
            doc.Add(new iText.Paragraph(" ", GetUnicodeFont(4)));

            var notesParagraph = new iText.Paragraph();
            notesParagraph.Add(new iText.Chunk("1. Vị trí lấy mẫu:", sectionFont));
            notesParagraph.Add(iText.Chunk.NEWLINE);

            string coordinates = "";
            if (data.SampleInfo.Longitude.HasValue && data.SampleInfo.Latitude.HasValue)
            {
                coordinates = $"     Tọa độ: X: {data.SampleInfo.Longitude:F7}     Y: {data.SampleInfo.Latitude:F6}";
            }

            notesParagraph.Add(new iText.Chunk($"- {data.SampleInfo.SampleCode}: ", sectionFont));
            notesParagraph.Add(new iText.Chunk(data.SampleInfo.PositionSite + coordinates ?? "", normalFont));
            notesParagraph.Add(iText.Chunk.NEWLINE);

            doc.Add(notesParagraph);

            var standardParagraph = new iText.Paragraph();
            standardParagraph.Add(new iText.Chunk("2. Quy chuẩn so sánh:", sectionFont));
            standardParagraph.Add(iText.Chunk.NEWLINE);

            standardParagraph.Add(new iText.Chunk("QCVN 05:2023/BTNMT: ", sectionFont));
            standardParagraph.Add(new iText.Chunk(
                "Quy chuẩn kỹ thuật quốc gia về chất lượng không khí (Bảng 1: giá trị giới hạn tối đa các thông số cơ bản trong không khí xung quanh-TB 1h)",
                italicFont));
            standardParagraph.Add(iText.Chunk.NEWLINE);

            standardParagraph.Add(new iText.Chunk("(1): QCVN 26:2010/BTNMT: ", sectionFont));
            standardParagraph.Add(new iText.Chunk("Quy chuẩn kỹ thuật quốc gia về tiếng ồn", italicFont));
            standardParagraph.Add(iText.Chunk.NEWLINE);

            standardParagraph.Add(new iText.Chunk("(2): QCVN 27:2010/BTNMT: ", sectionFont));
            standardParagraph.Add(new iText.Chunk("Quy chuẩn kỹ thuật quốc gia về độ rung", italicFont));

            doc.Add(standardParagraph);
        }

        private void AddPdfGeneralInfo(iText.Document doc, List<ExportData> currentSubGroup)
        {
            var labelFont = GetUnicodeFont(8, iText.Font.BOLD);
            var normalFont = GetUnicodeFont(8);
            var titleFont = GetUnicodeFont(11, iText.Font.BOLD);

            doc.Add(new iText.Paragraph("I. THÔNG TIN CHUNG", titleFont));
            doc.Add(new iText.Paragraph(" ", GetUnicodeFont(4)));

            var first = currentSubGroup.First();
            var r = first.ResultInfo ?? new Result();
            var s = first.SampleInfo;

            var infoTable = new iTextPdf.PdfPTable(2) { WidthPercentage = 100 };
            infoTable.SetWidths(new float[] { 20, 80 });

            AddInfoRowWithBorder(infoTable, "Tên khách hàng", r.CustomerName ?? "N/A", labelFont, normalFont);
            AddInfoRowWithBorder(infoTable, "Địa chỉ", r.Address ?? "N/A", labelFont, normalFont);
            AddInfoRowWithBorder(infoTable, "Loại mẫu", first.SampleInfo.SampleTypeName ?? "N/A", labelFont, normalFont);

            var positionsText = new StringBuilder();
            foreach (var sample in currentSubGroup)
            {
                string coordinates = "";
                if (sample.SampleInfo.Longitude.HasValue && sample.SampleInfo.Latitude.HasValue)
                {
                    coordinates = $" - X={sample.SampleInfo.Longitude:F4}, Y={sample.SampleInfo.Latitude:F4}";
                }
                positionsText.AppendLine($"{sample.SampleInfo.SampleCode}: {sample.SampleInfo.PositionSite}{coordinates}");
            }
            AddInfoRowWithBorder(infoTable, "Vị trí quan trắc", positionsText.ToString().Trim(), labelFont, normalFont);
            AddInfoRowWithBorder(infoTable, "Ngày quan trắc", first.SampleInfo.CreatedAt?.ToString("dd/MM/yyyy") ?? "N/A", labelFont, normalFont);

            string ngayPhanTich = (s.CreatedAt != null || r.ConfirmDate != null)
                ? $"{s.CreatedAt:dd/MM/yyyy} - {r.ConfirmDate:dd/MM/yyyy}"
                : "N/A";

            AddInfoRowWithBorder(infoTable, "Ngày phân tích", ngayPhanTich, labelFont, normalFont);
            AddInfoRowWithBorder(infoTable, "Ngày trả kết quả", DateTime.Now.ToString("dd/MM/yyyy"), labelFont, normalFont);

            doc.Add(infoTable);
        }

        private void AddCombinedParameterTable(iText.Document doc, List<ExportData> allSamples)
        {
            var headerFont = GetUnicodeFont(9, iText.Font.BOLD);
            var normalFont = GetUnicodeFont(9);

            var allParamsDict = new Dictionary<int, Sample_Parameter>();
            foreach (var sample in allSamples)
            {
                if (sample.Parameters == null) continue;
                foreach (var param in sample.Parameters)
                {
                    if (!allParamsDict.ContainsKey(param.ParameterId))
                        allParamsDict[param.ParameterId] = param;
                }
            }

            var allParams = allParamsDict.Values.OrderBy(p => p.ParameterId).ToList();
            if (allParams.Count == 0)
            {
                doc.Add(new iText.Paragraph("Không có chỉ tiêu nào.", normalFont));
                return;
            }

            int fixedCols = 4;
            int resultCols = allSamples.Count;
            int qcCol = 1;
            int colCount = fixedCols + resultCols + qcCol;

            var table = new iTextPdf.PdfPTable(colCount) { WidthPercentage = 100 };

            var widths = new float[colCount];
            float chiTieuWidth = 25f;

            if (resultCols >= 3) chiTieuWidth = 20f;
            if (resultCols >= 5) chiTieuWidth = 15f;
            if (resultCols >= 8) chiTieuWidth = 12f;

            widths[0] = 5;
            widths[1] = chiTieuWidth;
            widths[2] = 10;
            widths[3] = 12;

            float resultColWidth = 12f;
            if (resultCols >= 5) resultColWidth = 10f;
            if (resultCols >= 8) resultColWidth = 8f;

            for (int i = 4; i < 4 + resultCols; i++)
                widths[i] = resultColWidth;

            widths[colCount - 1] = 15;
            table.SetWidths(widths);

            table.AddCell(CreateHeaderCell("TT", headerFont, rowspan: 2));
            table.AddCell(CreateHeaderCell("Chỉ tiêu phân tích", headerFont, rowspan: 2));
            table.AddCell(CreateHeaderCell("Đơn vị", headerFont, rowspan: 2));
            table.AddCell(CreateHeaderCell("Ngày xác nhận", headerFont, rowspan: 2));

            var ketQuaHeader = CreateHeaderCell("Kết quả", headerFont);
            ketQuaHeader.Colspan = resultCols;
            table.AddCell(ketQuaHeader);

            table.AddCell(CreateHeaderCell("QCVN\n05:2023/BTNMT", headerFont, rowspan: 2));

            foreach (var sample in allSamples)
            {
                table.AddCell(CreateHeaderCell(sample.SampleInfo.SampleCode, headerFont));
            }

            int idx = 1;
            foreach (var param in allParams)
            {
                table.AddCell(CreatePdfCell(idx.ToString(), normalFont, iText.Element.ALIGN_CENTER));

                var chiTieuCellFont = (resultCols >= 5) ? GetUnicodeFont(8) : normalFont;
                table.AddCell(CreatePdfCell(param.ParameterName ?? "", chiTieuCellFont, iText.Element.ALIGN_CENTER));

                table.AddCell(CreatePdfCell(param.ParameterUnit ?? "", normalFont, iText.Element.ALIGN_CENTER));

                string confirmDateStr = param.ConfirmDate != DateTime.MinValue
                    ? param.ConfirmDate.ToString("dd/MM/yyyy")
                    : (allSamples.First().ResultInfo?.ConfirmDate?.ToString("dd/MM/yyyy") ?? "");
                table.AddCell(CreatePdfCell(confirmDateStr, normalFont, iText.Element.ALIGN_CENTER));

                foreach (var s in allSamples)
                {
                    var p = s.Parameters?.FirstOrDefault(x => x.ParameterId == param.ParameterId);
                    string v = p?.Value?.ToString() ?? "—";
                    table.AddCell(CreatePdfCell(v, normalFont, iText.Element.ALIGN_CENTER));
                }

                var paramData = ParameterService.Instance.GetParameterById(param.ParameterId);
                string qcValue = "";

                if (paramData != null)
                {
                    if (paramData.MaxLimit.HasValue)
                        qcValue = paramData.MaxLimit.Value.ToString("0.##");
                    else if (paramData.MinLimit.HasValue)
                        qcValue = paramData.MinLimit.Value.ToString("0.##");
                }

                table.AddCell(CreatePdfCell(qcValue, normalFont, iText.Element.ALIGN_CENTER));
                idx++;
            }

            doc.Add(table);
        }

        private void AddPdfParameterTable(iText.Document doc, ExportData data)
        {
            var headerFont = GetUnicodeFont(9, iText.Font.BOLD);
            var normalFont = GetUnicodeFont(9);

            var table = new iTextPdf.PdfPTable(6) { WidthPercentage = 100 };

            float chiTieuWidth = 25f;
            if (data.Parameters != null && data.Parameters.Count > 8)
            {
                chiTieuWidth = 18f;
            }
            else if (data.Parameters != null && data.Parameters.Count > 15)
            {
                chiTieuWidth = 15f;
            }

            table.SetWidths(new float[] { 8, chiTieuWidth, 18, 12, 18, 18 });

            string[] headers = { "TT", "Chỉ tiêu phân tích", "Kết quả", "Đơn vị", "Ngày xác nhận", "Ngày trả kết quả" };
            foreach (var h in headers)
            {
                table.AddCell(CreateHeaderCell(h, headerFont));
            }

            if (data.Parameters != null)
            {
                int idx = 1;
                foreach (var p in data.Parameters)
                {
                    table.AddCell(CreatePdfCell(idx.ToString(), normalFont, iText.Element.ALIGN_CENTER));

                    var chiTieuFont = (data.Parameters.Count > 10) ? GetUnicodeFont(8) : normalFont;
                    table.AddCell(CreatePdfCell(p.ParameterName ?? "", chiTieuFont, iText.Element.ALIGN_LEFT));

                    table.AddCell(CreatePdfCell(p.Value?.ToString() ?? "", normalFont, iText.Element.ALIGN_CENTER));
                    table.AddCell(CreatePdfCell(p.ParameterUnit ?? "", normalFont, iText.Element.ALIGN_CENTER));

                    string confirmDateStr = p.ConfirmDate != DateTime.MinValue
                        ? p.ConfirmDate.ToString("dd/MM/yyyy")
                        : (data.ResultInfo?.ConfirmDate?.ToString("dd/MM/yyyy") ?? "");
                    table.AddCell(CreatePdfCell(confirmDateStr, normalFont, iText.Element.ALIGN_CENTER));

                    table.AddCell(CreatePdfCell(DateTime.Now.ToString("dd/MM/yyyy"), normalFont, iText.Element.ALIGN_CENTER));
                    idx++;
                }
            }

            doc.Add(table);
            doc.Add(new iText.Paragraph(" "));
        }

        // ================================================================
        // 🔤 FONT & CELL HELPERS
        // ================================================================
        private iText.Font GetUnicodeFont(int size, int style = iText.Font.NORMAL)
        {
            try
            {
                string fontPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
                    "times.ttf");

                var baseFont = iTextPdf.BaseFont.CreateFont(fontPath, iTextPdf.BaseFont.IDENTITY_H, iTextPdf.BaseFont.EMBEDDED);
                return new iText.Font(baseFont, size, style);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Font load error: {ex.Message}");
                return iText.FontFactory.GetFont("Arial", size, style);
            }
        }

        private iTextPdf.PdfPCell CreateHeaderCell(string text, iText.Font font, int rowspan = 1, int colspan = 1)
        {
            return new iTextPdf.PdfPCell(new iText.Phrase(text, font))
            {
                BackgroundColor = new iText.BaseColor(200, 200, 200),
                HorizontalAlignment = iText.Element.ALIGN_CENTER,
                VerticalAlignment = iText.Element.ALIGN_MIDDLE,
                Padding = 6,
                Rowspan = rowspan,
                Colspan = colspan
            };
        }

        private iTextPdf.PdfPCell CreatePdfCell(string text, iText.Font font, int align = iText.Element.ALIGN_LEFT)
        {
            return new iTextPdf.PdfPCell(new iText.Phrase(text, font))
            {
                Padding = 8,
                HorizontalAlignment = align
            };
        }

        private void AddInfoRowWithBorder(iTextPdf.PdfPTable table, string label, string value, iText.Font labelFont, iText.Font valueFont)
        {
            var labelCell = new iTextPdf.PdfPCell(new iText.Phrase(label, labelFont))
            {
                Padding = 8,
                BackgroundColor = new iText.BaseColor(240, 240, 240),
                HorizontalAlignment = iText.Element.ALIGN_LEFT
            };
            var valueCell = new iTextPdf.PdfPCell(new iText.Phrase(value, valueFont))
            {
                Padding = 8,
                HorizontalAlignment = iText.Element.ALIGN_LEFT
            };
            table.AddCell(labelCell);
            table.AddCell(valueCell);
        }

        // ================================================================
        // 🏢 HEADER PDF
        // ================================================================
        private void AddPdfHeader(iText.Document doc, ExportData data)
        {
            var titleFont = GetUnicodeFont(14, iText.Font.BOLD);
            var normalFont = GetUnicodeFont(10);
            var smallFont = GetUnicodeFont(9);

            EMC.DTO.Company company = null;
            try
            {
                company = EMC.Service.CompanyService.Instance.GetCompanyInfo();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Company info load failed: {ex.Message}");
            }

            string companyName = company?.Name?.Trim();
            string ShortName = company?.ShortName?.Trim();
            if (string.IsNullOrEmpty(companyName))
            {
                companyName = "CÔNG TY ENVIRONMENT MONITORING & CONTROL (EMC)";
            }
            else
            {
                if (!companyName.StartsWith("CÔNG TY", StringComparison.OrdinalIgnoreCase))
                    companyName = "CÔNG TY " + companyName;

                if (!companyName.Contains("(EMC)", StringComparison.OrdinalIgnoreCase))
                    companyName += " " + "(" + ShortName + ")";
            }

            var title = new iText.Paragraph(companyName.ToUpper(), titleFont)
            {
                Alignment = iText.Element.ALIGN_CENTER
            };
            doc.Add(title);

            if (company != null)
            {
                if (!string.IsNullOrWhiteSpace(company.Address))
                {
                    var addrPara = new iText.Paragraph($"Địa chỉ: {company.Address}", smallFont)
                    {
                        Alignment = iText.Element.ALIGN_CENTER,
                        SpacingAfter = 2f
                    };
                    doc.Add(addrPara);
                }

                var parts = new List<string>();
                if (!string.IsNullOrWhiteSpace(company.Hotline)) parts.Add($"Hotline: {company.Hotline}");
                if (!string.IsNullOrWhiteSpace(company.Email)) parts.Add($"Email: {company.Email}");

                if (parts.Count > 0)
                {
                    var contactPara = new iText.Paragraph(string.Join("    ", parts), smallFont)
                    {
                        Alignment = iText.Element.ALIGN_CENTER
                    };
                    doc.Add(contactPara);
                }
            }

            var subtitle = new iText.Paragraph("PHIẾU KẾT QUẢ QUAN TRẮC", titleFont)
            { Alignment = iText.Element.ALIGN_CENTER };
            doc.Add(subtitle);

            // ⭐ THAY ĐỔI PHẦN NÀY - Lấy mã đơn hàng từ ResultInfo
            string orderCode = data.SampleInfo?.OrderCode ?? data.SampleInfo?.SampleCode ?? "N/A";
            var reportNo = new iText.Paragraph($"Số: KQ-{orderCode}", normalFont)
            { Alignment = iText.Element.ALIGN_RIGHT };
            doc.Add(reportNo);
        }


        private void AddPdfSampleInfo(iText.Document doc, ExportData data)
        {
            var titleFont = GetUnicodeFont(11, iText.Font.BOLD);
            var labelFont = GetUnicodeFont(8, iText.Font.BOLD);
            var normalFont = GetUnicodeFont(8);

            doc.Add(new iText.Paragraph("I. THÔNG TIN CHUNG", titleFont));
            doc.Add(new iText.Paragraph(" ", GetUnicodeFont(4)));

            var s = data.SampleInfo;
            var r = data.ResultInfo ?? new Result();

            var infoTable = new iTextPdf.PdfPTable(2) { WidthPercentage = 100 };
            infoTable.SetWidths(new float[] { 20, 80 });

            AddInfoRowWithBorder(infoTable, "Tên khách hàng", r.CustomerName ?? "N/A", labelFont, normalFont);
            AddInfoRowWithBorder(infoTable, "Địa chỉ", r.Address ?? "N/A", labelFont, normalFont);
            AddInfoRowWithBorder(infoTable, "Loại mẫu", s.SampleTypeName ?? "N/A", labelFont, normalFont);
            AddInfoRowWithBorder(infoTable, "Vị trí quan trắc", s.SampleCode + ": " + s.PositionSite ?? "N/A", labelFont, normalFont);
            AddInfoRowWithBorder(infoTable, "Ngày quan trắc", s.CreatedAt?.ToString("dd/MM/yyyy") ?? "N/A", labelFont, normalFont);

            string ngayPhanTich = (s.CreatedAt != null || r.ConfirmDate != null)
                ? $"{s.CreatedAt:dd/MM/yyyy} - {r.ConfirmDate:dd/MM/yyyy}"
                : "N/A";

            AddInfoRowWithBorder(infoTable, "Ngày phân tích", ngayPhanTich, labelFont, normalFont);
            AddInfoRowWithBorder(infoTable, "Ngày trả kết quả", DateTime.Now.ToString("dd/MM/yyyy"), labelFont, normalFont);

            doc.Add(infoTable);
        }

        // ================================================================
        // ✍️ CHỮ KÝ
        // ================================================================
        private void AddPdfSignature(iText.Document doc, ExportData data)
        {
            var smallFont = GetUnicodeFont(8);
            doc.Add(new iText.Paragraph(" "));
            var sigTable = new iTextPdf.PdfPTable(3) { WidthPercentage = 100 };
            string[] sigHeaders = { "Người lập phiếu", "Trưởng phòng", "Phê duyệt" };
            foreach (var h in sigHeaders)
            {
                var cell = new iTextPdf.PdfPCell(new iText.Phrase(h, smallFont))
                {
                    HorizontalAlignment = iText.Element.ALIGN_CENTER,
                    PaddingTop = 50
                };
                sigTable.AddCell(cell);
            }
            doc.Add(sigTable);
        }

        // ================================================================
        // 🔗 GỘP PDF
        // ================================================================
        public bool MergePdfFiles(List<string> pdfPaths, string outputPath)
        {
            try
            {
                using (var stream = new FileStream(outputPath, FileMode.Create))
                using (var doc = new iText.Document())
                using (var copy = new iTextPdf.PdfCopy(doc, stream))
                {
                    doc.Open();

                    foreach (var path in pdfPaths)
                    {
                        if (!File.Exists(path)) continue;
                        var reader = new iTextPdf.PdfReader(path);
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            copy.AddPage(copy.GetImportedPage(reader, i));
                        }
                        reader.Close();
                    }

                    doc.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Merge PDF failed: {ex.Message}");
                return false;
            }
        }
    }

    public class ExportData
    {
        public Sample SampleInfo { get; set; }
        public Result ResultInfo { get; set; }
        public List<Sample_Parameter> Parameters { get; set; } = new();
    }
}