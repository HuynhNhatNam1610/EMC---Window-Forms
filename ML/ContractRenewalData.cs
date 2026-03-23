using EMC.DTO;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Diagnostics;

namespace EMC.ML
{
    // ==== DỮ LIỆU TRAIN/PREDICT (4 đặc trưng theo định nghĩa mới) ====
    public class ContractRenewalData
    {
        // Biểu đồ
        public string CustomerName { get; set; }
        public int RenewCount { get; set; }
        public double YearsOfRelation { get; set; }
        public double OnTimePaymentRate { get; set; }
        public double CustomerSatisfaction { get; set; }
        public int ComplaintCount { get; set; }
        public double TotalContractValue { get; set; }

        //AI dự đoán tỷ lệ tái ký
        public float YearsOfRelationToNow { get; set; }          // năm hợp tác tới hiện tại
        public float CumulativeTotalValue { get; set; }          // tổng giá trị tích lũy / 1e6
        public float DaysSinceLatestContractToNow { get; set; }  // ngày từ HĐ gần nhất tới hiện tại
        public float OnTimeEmailRate { get; set; }               // %
        public float TotalContractCount { get; set; }
        public float PrevGapDays { get; set; }
        public float PrevGapScore { get; set; }             // = 1 - min(prevGap,180)/180 → [0..1]

        // Tỷ lệ email trễ trên lịch sử: 0..1
        public float LateEmailRate { get; set; }            // = lateCount / totalCount

        // Điểm trễ email của HĐ hiện tại/“mới nhất”: 0 khi <=20d, 1 khi >=60d
        public float LatestEmailDelayScore { get; set; }
        public bool WillRenewWithin180Days { get; set; }
        public float DaysSinceLatestContractScore { get; set; }
        // Nhãn nhị phân
        [ColumnName("Label")]
        public bool WillRenew { get; set; }
    }

    public class RenewalPrediction
    {
        // Xác suất đã hiệu chuẩn
        [ColumnName("Probability")]
        public float Probability { get; set; }
    }

    public class ContractRenewalPredictor
    {
        private readonly MLContext mlContext;
        private ITransformer model;
        private readonly string modelPath;

        public ContractRenewalPredictor(string modelPath = "renewal_model.zip")
        {
            mlContext = new MLContext(seed: 0);
            this.modelPath = modelPath;
        }

        public List<ContractRenewalData> PrepareTrainingData(List<Contract> histories)
        {
            var list = new List<ContractRenewalData>();

            foreach (var h in histories)
            {
                if (h.Contracts.Count < 2) continue;

                var sorted = h.Contracts.OrderBy(c => c.SignDate).ToList();
                var first = sorted.First();

                // ✅ Tạo mẫu training từ MỖI hợp đồng (trừ cuối cùng)
                for (int i = 0; i < sorted.Count - 1; i++)
                {
                    var cur = sorted[i];
                    var next = sorted[i + 1];

                    // ✅ MỐC THỜI GIAN = NGAY SAU KHI KÝ HĐ HIỆN TẠI (không phải next.SignDate!)
                    // Giả lập: "Đứng tại thời điểm vừa ký xong HĐ cur, liệu có tái ký không?"
                    DateTime refTime = cur.SignDate.AddDays(1); // hoặc cur.SignDate

                    // ✅ Nhãn: Có tái ký trong 210 ngày TỪ refTime
                    bool willRenew = (next.SignDate - refTime).TotalDays <= 210;

                    // ===== TÍNH FEATURES TẠI refTime (không nhìn về tương lai!) =====

                    // 1. Năm hợp tác TỪ HĐ ĐẦU TIÊN đến refTime
                    float yearsOfRelation = (float)((refTime - first.SignDate).TotalDays / 365.0);

                    // 2. Tổng giá trị ĐẾN HĐ HIỆN TẠI (i+1 hợp đồng)
                    float cumulativeValue = (float)(sorted.Take(i + 1).Sum(x => (double)x.TotalValue) / 1_000_000.0);

                    // 3. Số HĐ ĐẾN HIỆN TẠI
                    int contractCount = i + 1;

                    // 4. Khoảng cách 2 HĐ gần nhất (nếu có)
                    float prevGapScore = 0f;
                    if (i > 0)
                    {
                        var prev = sorted[i - 1];
                        float gap = (float)(cur.SignDate - prev.SignDate).TotalDays;
                        prevGapScore = 1f - Math.Min(gap, 210f) / 210f; // 0..1
                    }

                    // 5. Email trễ TRONG LỊCH SỬ (đến i)
                    int validEmails = 0;
                    int late = 0;
                    float curDelay = 0f;

                    for (int k = 0; k <= i; k++)
                    {
                        var ck = sorted[k];
                        if (ck.FirstSampleDate.HasValue && ck.EmailedDate.HasValue)
                        {
                            validEmails++;
                            float d = (float)(ck.EmailedDate.Value - ck.FirstSampleDate.Value).TotalDays;

                            if (k == i) curDelay = d; // delay của HĐ hiện tại
                            if (d > 20f) late++;
                        }
                    }

                    float lateRate = validEmails > 0 ? (float)late / validEmails : 0f;
                    float latestDelayScore = Math.Clamp((curDelay - 20f) / 40f, 0f, 1f);

                    float daysSinceLatestScore = 0.5f; // default
                    if (i > 0)
                    {
                        var prevContract = sorted[i - 1];
                        float daysSincePrev = (float)(cur.SignDate - prevContract.SignDate).TotalDays;
                        // Chuyển thành điểm: 0 khi 365+ ngày, 1 khi 0 ngày
                        daysSinceLatestScore = Math.Max(0f, 1f - (daysSincePrev / 365f));
                    }

                    var row = new ContractRenewalData
                    {
                        YearsOfRelationToNow = yearsOfRelation,
                        CumulativeTotalValue = cumulativeValue,
                        TotalContractCount = contractCount,
                        PrevGapScore = prevGapScore,
                        LateEmailRate = lateRate,
                        LatestEmailDelayScore = latestDelayScore,
                        DaysSinceLatestContractScore = daysSinceLatestScore,  // ✅ THÊM MỚI
                        WillRenew = willRenew
                    };

                    list.Add(row);

                    // ✅ DEBUG: Kiểm tra logic
                    if (list.Count <= 5)
                    {
                        Debug.WriteLine($"[TRAIN {list.Count}] CusId={h.CustomerId}, " +
                                          $"HĐ {i + 1}, Gap={(next.SignDate - cur.SignDate).TotalDays:F0}d, " +
                                          $"Years={yearsOfRelation:F2}, PrevGap={prevGapScore:F2}, " +
                                          $"LateRate={lateRate:F2}, Label={willRenew}");
                    }
                }
            }

            Debug.WriteLine($"\n📊 Total: {list.Count}, TRUE: {list.Count(x => x.WillRenew)}, " +
                              $"FALSE: {list.Count(x => !x.WillRenew)}");

            return list;
        }

        public void TrainModel(List<Contract> histories)
        {
            var trainRows = PrepareTrainingData(histories);

            if (trainRows.Count < 20)
            {
                Debug.WriteLine($"⚠️ Chỉ có {trainRows.Count} mẫu training - model sẽ không chính xác!");
            }

            // ✅ KIỂM TRA LABEL
            int trueCount = trainRows.Count(x => x.WillRenew);
            int falseCount = trainRows.Count - trueCount;
            Debug.WriteLine($"\n📈 Label distribution: TRUE={trueCount} ({trueCount * 100.0 / trainRows.Count:F1}%), " +
                              $"FALSE={falseCount} ({falseCount * 100.0 / trainRows.Count:F1}%)");

            if (trueCount == 0 || falseCount == 0)
            {
                throw new Exception("❌ NGUY HIỂM: Tất cả label đều giống nhau! Model sẽ vô dụng.");
            }

            var trainData = mlContext.Data.LoadFromEnumerable(trainRows);

            var features = new[] {
                nameof(ContractRenewalData.YearsOfRelationToNow),
                nameof(ContractRenewalData.CumulativeTotalValue),
                nameof(ContractRenewalData.TotalContractCount),
                nameof(ContractRenewalData.PrevGapScore),
                nameof(ContractRenewalData.LateEmailRate),
                nameof(ContractRenewalData.LatestEmailDelayScore),
                nameof(ContractRenewalData.DaysSinceLatestContractScore)
            };

            var pipeline =
                mlContext.Transforms.Concatenate("Features", features)
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .AppendCacheCheckpoint(mlContext)
                .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                    labelColumnName: "Label",
                    featureColumnName: "Features",
                    l1Regularization: 0.001f,
                    l2Regularization: 0.001f
                ));

            model = pipeline.Fit(trainData);
            mlContext.Model.Save(model, trainData.Schema, modelPath);

            Debug.WriteLine("✅ Training completed!");
        }


        public void LoadModel()
        {
            model = mlContext.Model.Load(modelPath, out _);
        }

        //public float PredictRenewal(Contract history)
        //{
        //    if (model == null) throw new InvalidOperationException("Model chưa train/load.");

        //    var sorted = history.Contracts.OrderBy(c => c.SignDate).ToList();
        //    if (sorted.Count == 1) return 50f; // Không đủ dữ liệu

        //    var first = sorted.First();
        //    var latest = sorted.Last();

        //    // ✅ MỐC THỜI GIAN = BÂY GIỜ (khi predict thực tế)
        //    var refTime = DateTime.Now;

        //    // ===== TÍNH FEATURES GIỐNG NHƯ TRAINING =====

        //    // 1. Năm hợp tác
        //    float yearsOfRelation = (float)((refTime - first.SignDate).TotalDays / 365.0);

        //    // 2. Tổng giá trị TẤT CẢ hợp đồng
        //    float cumulativeValue = (float)(sorted.Sum(x => (double)x.TotalValue) / 1_000_000.0);

        //    // 3. Số lượng HĐ
        //    int contractCount = sorted.Count;

        //    // 4. Khoảng cách 2 HĐ gần nhất
        //    float prevGapScore = 0f;
        //    if (sorted.Count >= 2)
        //    {
        //        var secondLast = sorted[sorted.Count - 2];
        //        float gap = (float)(latest.SignDate - secondLast.SignDate).TotalDays;
        //        prevGapScore = 1f - Math.Min(gap, 210f) / 210f;
        //    }

        //    // 5. Tỷ lệ email trễ
        //    int validEmails = 0;
        //    int late = 0;
        //    float latestDelay = 0f;

        //    foreach (var c in sorted)
        //    {
        //        if (c.FirstSampleDate.HasValue && c.EmailedDate.HasValue)
        //        {
        //            validEmails++;
        //            float d = (float)(c.EmailedDate.Value - c.FirstSampleDate.Value).TotalDays;

        //            if (c == latest) latestDelay = d;
        //            if (d > 20f) late++;
        //        }
        //    }

        //    float lateRate = validEmails > 0 ? (float)late / validEmails : 0f;
        //    float latestDelayScore = Math.Clamp((latestDelay - 20f) / 40f, 0f, 1f);

        //    float daysSinceLatestScore = 0.5f;
        //    if (sorted.Count >= 2)
        //    {
        //        var secondLast = sorted[sorted.Count - 2];
        //        float daysSincePrev = (float)(latest.SignDate - secondLast.SignDate).TotalDays;
        //        daysSinceLatestScore = Math.Max(0f, 1f - (daysSincePrev / 365f));
        //    }

        //    // ===== TẠO INPUT GIỐNG TRAINING =====
        //    var input = new ContractRenewalData
        //    {
        //        YearsOfRelationToNow = yearsOfRelation,
        //        CumulativeTotalValue = cumulativeValue,
        //        TotalContractCount = contractCount,
        //        PrevGapScore = prevGapScore,
        //        LateEmailRate = lateRate,
        //        LatestEmailDelayScore = latestDelayScore,
        //        DaysSinceLatestContractScore = daysSinceLatestScore  // ✅ THÊM MỚI
        //    };

        //    // ===== DỰ ĐOÁN =====
        //    var engine = mlContext.Model.CreatePredictionEngine<ContractRenewalData, RenewalPrediction>(model);
        //    var pred = engine.Predict(input);

        //    float rawProb = pred.Probability * 100f;

        //    // ✅ DEBUG
        //    Debug.WriteLine($"[PREDICT] CusId={history.CustomerId}, " +
        //                      $"Contracts={contractCount}, Years={yearsOfRelation:F2}, " +
        //                      $"DaysSinceLast={(refTime - latest.SignDate).TotalDays:F0}d, " +
        //                      $"PrevGap={prevGapScore:F2}, LateRate={lateRate:F2}, " +
        //                      $"RawProb={rawProb:F1}%");

        //    float daysSinceLast = (float)(refTime - latest.SignDate).TotalDays;
        //    // Penalty 1: Quá lâu không làm hợp đồng
        //    if (daysSinceLast > 730)  // >2 năm
        //    {
        //        rawProb *= 0.4f;
        //        Debug.WriteLine($"  ⚠️ Penalty (>2 years): {rawProb:F1}%");
        //    }
        //    else if (daysSinceLast > 365)  // >1 năm
        //    {
        //        rawProb *= 0.6f;
        //        Debug.WriteLine($"  ⚠️ Penalty (>1 year): {rawProb:F1}%");
        //    }
        //    else if (daysSinceLast > 180)  // >6 tháng
        //    {
        //        rawProb *= 0.8f;
        //        Debug.WriteLine($"  ⚠️ Penalty (>6 months): {rawProb:F1}%");
        //    }

        //    // Penalty: Email trễ nhiều
        //    if (lateRate > 0.5f)
        //    {
        //        rawProb *= 0.8f;
        //        Debug.WriteLine($"  → Penalty (late emails): {rawProb:F1}%");
        //    }

        //    return Math.Clamp(rawProb, 0f, 100f);
        //}
        public float PredictRenewal(Contract history)
        {
            if (model == null) throw new InvalidOperationException("Model chưa train/load.");

            var sorted = history.Contracts.OrderBy(c => c.SignDate).ToList();
            if (sorted.Count == 1) return 50f; // Không đủ dữ liệu

            var first = sorted.First();
            var latest = sorted.Last();
            var refTime = DateTime.Now;

            // ===== TÍNH FEATURES GIỐNG NHƯ TRAINING =====

            // 1. Năm hợp tác
            float yearsOfRelation = (float)((refTime - first.SignDate).TotalDays / 365.0);

            // 2. Tổng giá trị TẤT CẢ hợp đồng
            float cumulativeValue = (float)(sorted.Sum(x => (double)x.TotalValue) / 1_000_000.0);

            // 3. Số lượng HĐ
            int contractCount = sorted.Count;

            // 4. Khoảng cách 2 HĐ gần nhất
            float prevGapScore = 0f;
            if (sorted.Count >= 2)
            {
                var secondLast = sorted[sorted.Count - 2];
                float gap = (float)(latest.SignDate - secondLast.SignDate).TotalDays;
                prevGapScore = 1f - Math.Min(gap, 210f) / 210f;
            }

            // 5. Tỷ lệ email trễ (> 20 ngày = TRỄ HẠN)
            int validEmails = 0;
            int late = 0;
            float latestDelay = 0f;

            foreach (var c in sorted)
            {
                if (c.FirstSampleDate.HasValue && c.EmailedDate.HasValue)
                {
                    validEmails++;
                    float d = (float)(c.EmailedDate.Value - c.FirstSampleDate.Value).TotalDays;

                    if (c == latest) latestDelay = d;

                    // ✅ QUY ĐỊNH: > 20 ngày = TRỄ HẠN
                    if (d > 20f) late++;
                }
            }

            float lateRate = validEmails > 0 ? (float)late / validEmails : 0f;

            // ✅ latestDelayScore: 0 khi ≤20, tăng dần khi >20
            float latestDelayScore = latestDelay <= 20f
                ? 0f
                : Math.Clamp((latestDelay - 20f) / 40f, 0f, 1f);

            // 6. Điểm từ khoảng cách HĐ gần nhất
            float daysSinceLatestScore = 0.5f;
            if (sorted.Count >= 2)
            {
                var secondLast = sorted[sorted.Count - 2];
                float daysSincePrev = (float)(latest.SignDate - secondLast.SignDate).TotalDays;
                daysSinceLatestScore = Math.Max(0f, 1f - (daysSincePrev / 365f));
            }

            // ===== TẠO INPUT PREDICT =====
            var input = new ContractRenewalData
            {
                YearsOfRelationToNow = yearsOfRelation,
                CumulativeTotalValue = cumulativeValue,
                TotalContractCount = contractCount,
                PrevGapScore = prevGapScore,
                LateEmailRate = lateRate,
                LatestEmailDelayScore = latestDelayScore,
                DaysSinceLatestContractScore = daysSinceLatestScore
            };

            // ===== DỰ ĐOÁN TỪ MODEL =====
            var engine = mlContext.Model.CreatePredictionEngine<ContractRenewalData, RenewalPrediction>(model);
            var pred = engine.Predict(input);

            float rawProb = pred.Probability * 100f;
            float daysSinceLast = (float)(refTime - latest.SignDate).TotalDays;

            // ✅ DEBUG OUTPUT
            Debug.WriteLine($"\n[PREDICT] CustomerId={history.CustomerId}");
            Debug.WriteLine($"  Contracts: {contractCount}, Years: {yearsOfRelation:F2}, CumulativeValue: ${cumulativeValue:F2}M");
            Debug.WriteLine($"  PrevGapScore: {prevGapScore:F2}, LateEmailRate: {lateRate:F2} (valid={validEmails}, late={late})");
            Debug.WriteLine($"  LatestDelay: {latestDelay:F1} ngày" + (latestDelay > 20f ? " ⚠️ TRỄ HẠN" : ""));
            Debug.WriteLine($"  DaysSinceLast: {daysSinceLast:F0} ngày");
            Debug.WriteLine($"  Raw Prob từ Model: {rawProb:F1}%");

            // ===== APPLY PENALTIES =====

            // ✅ PENALTY 1: EMAIL TRỄ HẠN (>20 NGÀY) - CHÍNH QUAN TRỌNG NHẤT
            if (latestDelay > 20f)
            {
                float daysOver20 = latestDelay - 20f;

                // Mức độ trễ hạn:
                // - 20-30 ngày (trễ 0-10 ngày): giảm 20%
                // - 30-45 ngày (trễ 10-25 ngày): giảm 35%
                // - 45-60 ngày (trễ 25-40 ngày): giảm 50%
                // - >60 ngày (trễ >40 ngày): giảm 65%

                float penaltyMultiplier = daysOver20 switch
                {
                    <= 10f => 0.80f,   // 20-30 ngày: ×0.80 (giảm 20%)
                    <= 25f => 0.65f,   // 30-45 ngày: ×0.65 (giảm 35%)
                    <= 40f => 0.50f,   // 45-60 ngày: ×0.50 (giảm 50%)
                    _ => 0.35f         // >60 ngày: ×0.35 (giảm 65%)
                };

                rawProb *= penaltyMultiplier;
                Debug.WriteLine($"  ⚠️ EMAIL TRỄ HẠN: {latestDelay:F1} ngày (vượt 20d thêm {daysOver20:F1}d) → ×{penaltyMultiplier} = {rawProb:F1}%");
            }

            // ✅ PENALTY 2: TỶ LỆ EMAIL TRỄ TRONG LỊCH SỬ
            if (lateRate >= 0.5f)  // ✅ FIX: >= thay vì > (50% tính là có vấn đề)
            {
                float historyPenalty = lateRate > 0.75f ? 0.70f : 0.85f;
                rawProb *= historyPenalty;
                Debug.WriteLine($"  ⚠️ LỊCH SỬ EMAIL TRỄ {lateRate:P0} → ×{historyPenalty} = {rawProb:F1}%");
            }
            else if (lateRate >= 0.3f)  // ✅ THÊM: Nếu 30-50% email trễ → Penalty nhẹ
            {
                float historyPenalty = 0.95f;
                rawProb *= historyPenalty;
                Debug.WriteLine($"  ⚠️ LỊCH SỬ EMAIL TRỄ {lateRate:P0} (nhẹ) → ×{historyPenalty} = {rawProb:F1}%");
            }

            // ✅ BONUS 2.3: NĂM HỢP TÁC (Độ trung thành khách hàng)
            // Logic: Hợp tác lâu = trung thành, hợp tác ngắn = chưa biết
            float loyaltyBonus = 1.0f;

            if (yearsOfRelation < 0.5f)  // < 6 tháng
            {
                loyaltyBonus = 0.90f;  // Giảm 10% (mới, chưa biết)
                Debug.WriteLine($"  ⚠️ Hợp tác MỚI ({yearsOfRelation:F2} năm) → ×{loyaltyBonus} = {rawProb * loyaltyBonus:F1}%");
            }
            else if (yearsOfRelation < 1f)  // 6-12 tháng
            {
                loyaltyBonus = 0.95f;  // Giảm 5%
                Debug.WriteLine($"  ⚠️ Hợp tác ngắn ({yearsOfRelation:F2} năm) → ×{loyaltyBonus} = {rawProb * loyaltyBonus:F1}%");
            }
            else if (yearsOfRelation < 2f)  // 1-2 năm
            {
                loyaltyBonus = 1.00f;  // Giữ nguyên (baseline)
                Debug.WriteLine($"  ℹ️ Hợp tác vừa ({yearsOfRelation:F2} năm) → Giữ nguyên");
            }
            else if (yearsOfRelation < 5f)  // 2-5 năm
            {
                loyaltyBonus = 1.10f;  // Tăng 10%
                Debug.WriteLine($"  ✅ Hợp tác lâu ({yearsOfRelation:F2} năm) → ×{loyaltyBonus} = {rawProb * loyaltyBonus:F1}%");
            }
            else  // >= 5 năm
            {
                loyaltyBonus = 1.20f;  // Tăng 20% (trung thành cao)
                Debug.WriteLine($"  ✅ Hợp tác LÂU LẮMM ({yearsOfRelation:F2} năm) → ×{loyaltyBonus} = {rawProb * loyaltyBonus:F1}%");
            }

            rawProb *= loyaltyBonus;

            // ✅ PENALTY 2.5: TÁC ĐỘNG GIÁ TRỊ HỢP ĐỒNG (Tối đa +20%)
            // Logic: Khách hàng trị cao = khó rơi, khách hàng trị thấp = dễ đổi provider
            float valueMultiplier = 1.0f;

            if (cumulativeValue < 50f)  // < 50M: Siêu nhỏ
            {
                valueMultiplier = 0.85f;  // Giảm 15%
                Debug.WriteLine($"  ⚠️ Giá trị SIÊU THẤP (${cumulativeValue:F0}M) → ×{valueMultiplier} = {rawProb * valueMultiplier:F1}%");
            }
            else if (cumulativeValue < 100f)  // 50-100M: Nhỏ
            {
                valueMultiplier = 0.90f;  // Giảm 10%
                Debug.WriteLine($"  ⚠️ Giá trị THẤP (${cumulativeValue:F0}M) → ×{valueMultiplier} = {rawProb * valueMultiplier:F1}%");
            }
            else if (cumulativeValue < 200f)  // 100-200M: Trung bình nhỏ
            {
                valueMultiplier = 0.97f;  // Giảm 3%
                Debug.WriteLine($"  ℹ️ Giá trị trung bình nhỏ (${cumulativeValue:F0}M) → ×{valueMultiplier} = {rawProb * valueMultiplier:F1}%");
            }
            else if (cumulativeValue < 400f)  // 200-400M: Trung bình
            {
                valueMultiplier = 1.0f;  // Giữ nguyên
                Debug.WriteLine($"  ℹ️ Giá trị trung bình (${cumulativeValue:F0}M) → Giữ nguyên");
            }
            else if (cumulativeValue < 800f)  // 400-800M: Cao
            {
                valueMultiplier = 1.08f;  // Tăng 8%
                Debug.WriteLine($"  ✅ Giá trị CAO (${cumulativeValue:F0}M) → ×{valueMultiplier} = {rawProb * valueMultiplier:F1}%");
            }
            else  // >= 800M: Siêu cao
            {
                valueMultiplier = 1.20f;  // Tăng 20% (tối đa)
                Debug.WriteLine($"  ✅ Giá trị SIÊU CAO (${cumulativeValue:F0}M) → ×{valueMultiplier} = {rawProb * valueMultiplier:F1}%");
            }

            rawProb *= valueMultiplier;

            // ✅ PENALTY 3: QUÊN HỢP ĐỒNG LÂU (Thứ yếu)
            if (daysSinceLast > 730)  // >2 năm
            {
                rawProb *= 0.4f;
                Debug.WriteLine($"  ⚠️ Quên >2 năm: {rawProb:F1}%");
            }
            else if (daysSinceLast > 365)  // >1 năm
            {
                rawProb *= 0.6f;
                Debug.WriteLine($"  ⚠️ Quên >1 năm: {rawProb:F1}%");
            }
            else if (daysSinceLast > 180)  // >6 tháng
            {
                rawProb *= 0.8f;
                Debug.WriteLine($"  ⚠️ Quên >6 tháng: {rawProb:F1}%");
            }

            float finalProb = Math.Clamp(rawProb, 0f, 100f);

            // ✅ BONUS: Nếu còn < 210 ngày kể từ HĐ hiện tại → Tăng thêm
            // ⚠️ NHƯNG: Chỉ áp dụng nếu EMAIL ĐÚNG HẠN!
            // Nếu email trễ → Không được hưởng bonus timeline
            int daysUntilDeadline = 210 - (int)daysSinceLast;

            if (daysUntilDeadline > 0)
            {
                // ✅ CHỈ BONUS nếu email ≤ 20 ngày (đúng hạn)
                if (latestDelay <= 20f)
                {
                    // Hợp đồng vẫn trong window tái ký 210 ngày
                    // Càng gần ngày ký ban đầu → Càng khả năng tái ký cao
                    // Công thức: Tăng tối đa 30% khi mới ký, giảm dần về 0% khi gần 210 ngày
                    float timelineBonus = (daysUntilDeadline / 210f) * 0.30f;  // Tối đa +30%
                    finalProb *= (1.0f + timelineBonus);
                    Debug.WriteLine($"  ⏰ Email đúng hạn + Còn {daysUntilDeadline} ngày → ×{1.0f + timelineBonus:F2} = {finalProb:F1}%");
                }
                else
                {
                    // ❌ EMAIL TRỄ → Không được bonus timeline
                    Debug.WriteLine($"  ❌ Email trễ hạn → Không được bonus timeline (vẫn còn {daysUntilDeadline} ngày)");
                }
            }
            else
            {
                // Vượt quá 210 ngày → Penalty nặng (đã quá hạn tái ký)
                finalProb *= 0.50f;
                Debug.WriteLine($"  🔴 Vượt 210d window ({Math.Abs(daysUntilDeadline)} ngày) → ×0.50 = {finalProb:F1}%");
            }

            finalProb = Math.Clamp(finalProb, 0f, 100f);
            Debug.WriteLine($"  ✅ Final Prob: {finalProb:F1}%\n");

            return finalProb;
        }
    }
}