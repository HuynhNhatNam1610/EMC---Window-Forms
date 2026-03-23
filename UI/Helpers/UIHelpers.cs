using EMC.UI.Controls;
using NAudio.Wave;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Vosk;

namespace EMC.UI.Helpers
{
    public static class UIHelpers
    {
        public static void LoadImage(PictureBox pictureBox, string relativePath, PictureBoxSizeMode sizeMode)
        {
            string fullPath = Path.Combine(Application.StartupPath, relativePath);

            if (File.Exists(fullPath))
            {
                pictureBox.Image = Image.FromFile(fullPath);
                pictureBox.SizeMode = sizeMode;
            }
            else
            {
                pictureBox.Image = null; // hoặc gán ảnh mặc định
            }
        }

        private static void TogglePassword(object sender, EventArgs e)
        {
            if (sender is PictureBox pb && pb.Tag is TextBox tb)
            {
                tb.PasswordChar = (tb.PasswordChar == '*') ? '\0' : '*';
            }
        }

        public static void SetupPlaceholder(TextBox textBox, string placeholder, bool isPassword = false, PictureBox pbShow = null)
        {
            // Gán placeholder mặc định
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;
            textBox.PasswordChar = '\0';

            // Khi focus vào textbox
            textBox.Enter += (s, e) =>
            {
                if (string.IsNullOrEmpty(textBox.Text) || textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
                if (isPassword)
                    textBox.PasswordChar = '*';
            };

            // Khi rời textbox
            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                    textBox.PasswordChar = '\0'; // bỏ chế độ mật khẩu
                }
            };

            // Nếu có nút show/hide mật khẩu
            if (isPassword && pbShow != null)
            {
                pbShow.Tag = textBox; // lưu tham chiếu
                pbShow.Click -= TogglePassword;
                pbShow.Click += TogglePassword;
            }
        }

        #region Search

        // Biến key tìm kiếm thành chữ thường
        public static string NormalizeForSearch(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            var formD = s.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in formD)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(char.ToLowerInvariant(ch));
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        //So khớp để tìm kiếm
        public static bool MatchStatusVi(string statusValueVi, string statusKeyViNoAccent)
        {
            if (statusKeyViNoAccent == null) return true;

            string cleanStatus = (statusValueVi ?? "").Trim();
            var norm = NormalizeForSearch(cleanStatus);

            return norm.Equals(statusKeyViNoAccent, StringComparison.OrdinalIgnoreCase);
        }
        public static void ForceRefreshDataGridView(DataGridView dgv)
        {
            if (dgv == null || dgv.Rows.Count == 0) return;

            dgv.Invalidate();
            dgv.Refresh();

            // Force repaint cho các cell trong cột button/custom paint
            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.OwningColumn is DataGridViewButtonColumn ||
                        cell.OwningColumn.Name == "ThaoTac")
                    {
                        dgv.InvalidateCell(cell);
                    }
                }
            }
        }
        // Lọc bởi thành tìm kiếm
        public static IEnumerable<T> FilterBySearch<T>(
            IEnumerable<T> data,
            string searchQuery,
            Func<T, string> searchFields)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                return data;

            string q = NormalizeForSearch(searchQuery);

            return data.Where(item =>
            {
                string haystack = NormalizeForSearch(searchFields(item));
                return haystack.Contains(q);
            });
        }

        //Lọc bởi thanh lọc
        public static IEnumerable<T> FilterByStatus<T>(
            IEnumerable<T> data,
            string statusKeyVi,
            Func<T, string> statusSelector)
        {
            if (string.IsNullOrEmpty(statusKeyVi))
                return data;

            return data.Where(item => MatchStatusVi(statusSelector(item), statusKeyVi));
        }

        //Lọc kết hợp
        public static IEnumerable<T> ApplyCombinedFilter<T>(
            IEnumerable<T> data,
            string textQuery,
            string statusKeyVi,
            Func<T, string> searchFields,
            Func<T, string> statusSelector)
        {
            data = FilterBySearch(data, textQuery, searchFields);
            data = FilterByStatus(data, statusKeyVi, statusSelector);

            return data;
        }

        //Lấy từ khóa từ thanh lọc trạng thái
        public static string GetSelectedStatusKey(RoundedComboBox rcb) // tiếng Việt -> khóa không dấu để so khớp
        {
            if (rcb.SelectedIndex <= 0) return null; // "Trạng thái" = không lọc
            switch (rcb.SelectedItem?.ToString())
            {
                case "Hoàn thành": return "hoan thanh";
                case "Đang xử lý": return "đang xu ly";
                case "Chưa tiến hành": return "chua tien hanh";
                case "Đã hủy": return "da huy";
                case "Đạt": return "đat";
                case "Chưa đạt": return "chua đat";
                case "Chưa đánh giá": return "chua đanh gia";
                case "Đang làm việc": return "đang lam viec";
                case "Nghỉ việc": return "nghi viec";
                case "Tạm nghỉ": return "tam nghi";
                case "Chờ nhận việc": return "cho nhan viec";
                case "Đã kích hoạt": return "đa kich hoat";
                case "Chưa kích hoạt": return "chua kich hoat";
                case "Vô hiệu hóa": return "vo hieu hoa";
                default: return null;
            }
        }
        public static void FormatStatusCell(DataGridViewCellFormattingEventArgs e, string statusValue)
        {
            if (string.IsNullOrWhiteSpace(statusValue))
                return;

            string s = statusValue.Trim().ToLowerInvariant();

            Color back = Color.White;
            Color fore = Color.Black;

            // ===== Nhóm màu =====
            if (s == "đang làm việc" || s == "hoàn thành" || s == "đạt" || s == "đã kích hoạt")
            {
                back = Color.FromArgb(209, 250, 229); // xanh nhạt
            }
            else if (s == "đang xử lý")
            {
                back = Color.FromArgb(219, 234, 254); // xanh lam nhạt
            }
            else if (s.Contains("tạm nghỉ") || s.Contains("chưa tiến hành") || s.Contains("chưa đánh giá") || s.Contains("chưa kích hoạt"))
            {
                back = Color.FromArgb(255, 243, 205); // vàng nhạt
            }
            else if (s.Contains("chờ nhận việc"))
            {
                back = Color.LightGray; // xám
            }
            else if (s.Contains("nghỉ") || s.Contains("chưa đạt") || s.Contains("vô hiệu hóa") || s.Contains("đã hủy"))
            {
                back = Color.FromArgb(254, 226, 226); // đỏ nhạt
            }

            // ===== Gán màu =====
            e.CellStyle.BackColor = back;
            e.CellStyle.ForeColor = fore;

            // Giữ nguyên màu khi chọn
            e.CellStyle.SelectionBackColor = back;
            e.CellStyle.SelectionForeColor = fore;
        }

        #endregion

        #region Actions Buttonc

        private static GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float r = radius;
            path.AddArc(rect.X, rect.Y, r, r, 180, 90);
            path.AddArc(rect.X + rect.Width - r, rect.Y, r, r, 270, 90);
            path.AddArc(rect.X + rect.Width - r, rect.Y + rect.Height - r, r, r, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - r, r, r, 90, 90);
            path.CloseAllFigures();
            return path;
        }

        public static void DrawActionButtons(
            Graphics g,
            Rectangle cellBounds,
            bool canDelete,
            int iconWidth = 35,
            int iconHeight = 25,
            int paddingLeft = 10,
            int spacing = 40,
            int cornerRadius = 8)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int x1 = cellBounds.Left + paddingLeft;
            int y = cellBounds.Top + (cellBounds.Height - iconHeight) / 2;

            // EDIT
            Rectangle r1 = new Rectangle(x1, y, iconWidth, iconHeight);

            // DELETE (nếu có quyền)
            Rectangle r2 = Rectangle.Empty;
            if (canDelete)
            {
                int x2 = x1 + spacing;
                r2 = new Rectangle(x2, y, iconWidth, iconHeight);
            }

            using (var bg = new SolidBrush(Color.FromArgb(240, 240, 240)))
            using (var border = new Pen(Color.FromArgb(120, 120, 120), 1))
            {
                using (var p1 = GetRoundedRectPath(r1, cornerRadius))
                {
                    g.FillPath(bg, p1);
                    g.DrawPath(border, p1);
                }

                if (canDelete)
                {
                    using (var p2 = GetRoundedRectPath(r2, cornerRadius))
                    {
                        g.FillPath(bg, p2);
                        g.DrawPath(border, p2);
                    }
                }
            }

            string iconEdit = "✏";
            string iconDelete = "🗑";

            using (var iconFont = new Font("Segoe UI Emoji", 12, FontStyle.Regular, GraphicsUnit.Pixel))
            using (var iconBrush = new SolidBrush(Color.Black))
            {
                SizeF s;

                s = g.MeasureString(iconEdit, iconFont);
                g.DrawString(iconEdit, iconFont, iconBrush,
                    r1.Left + (r1.Width - s.Width) / 2f,
                    r1.Top + (r1.Height - s.Height) / 2f);

                if (canDelete)
                {
                    s = g.MeasureString(iconDelete, iconFont);
                    g.DrawString(iconDelete, iconFont, iconBrush,
                        r2.Left + (r2.Width - s.Width) / 2f,
                        r2.Top + (r2.Height - s.Height) / 2f);
                }
            }
        }

        public enum ActionHit
        {
            None,
            Edit,
            Delete
        }

        /// <summary>
        /// Xác định nút nào được click (Edit/Delete) theo cùng config layout ở trên.
        /// </summary>
        public static ActionHit HitTestActionButtons(
            Rectangle cellBounds,
            Point clickPointClient,
            bool canDelete,
            int iconWidth = 35,
            int iconHeight = 25,
            int paddingLeft = 10,
            int spacing = 40)
        {
            int x1 = cellBounds.Left + paddingLeft;
            int y = cellBounds.Top + (cellBounds.Height - iconHeight) / 2;

            var r1 = new Rectangle(x1, y, iconWidth, iconHeight); // EDIT
            if (r1.Contains(clickPointClient))
                return ActionHit.Edit;

            if (canDelete)
            {
                int x2 = x1 + spacing;
                var r2 = new Rectangle(x2, y, iconWidth, iconHeight); // DELETE
                if (r2.Contains(clickPointClient))
                    return ActionHit.Delete;
            }

            return ActionHit.None;
        }

        #endregion

        #region Voice Search
        public static void InitializeVoskModel(out VoskRecognizer recognizer, out Model model)
        {
            recognizer = null;
            model = null;

            try
            {
                string[] possiblePaths = {
                    Path.Combine(Application.StartupPath, "Models", "vosk-model-vi-0.4"),
                    Path.Combine(Application.StartupPath, "..", "..", "Models", "vosk-model-vi-0.4"),
                };

                string modelPath = null;
                foreach (var path in possiblePaths)
                {
                    var full = Path.GetFullPath(path);
                    if (Directory.Exists(full) &&
                        Directory.Exists(Path.Combine(full, "am")) &&
                        Directory.Exists(Path.Combine(full, "graph")))
                    {
                        modelPath = full;
                        break;
                    }
                }

                if (modelPath == null)
                {
                    MessageBox.Show("Không tìm thấy model nhận diện giọng nói hợp lệ!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Vosk.Vosk.SetLogLevel(-1);
                model = new Model(modelPath);
                recognizer = new VoskRecognizer(model, 16000.0f);
                recognizer.SetMaxAlternatives(0);
                recognizer.SetWords(true);
                recognizer.SetPartialWords(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải model nhận diện giọng nói:\n{ex.Message}\n\nChi tiết: {ex.StackTrace}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                recognizer = null;
                model = null;
            }
        }

        public static WaveInEvent StartListening(
            StringBuilder recognizedText,
            Controls.RoundedButton rbtnVoice,
            Controls.PlaceholderTextBox2 ptbSearch,
            VoskRecognizer recognizer,
            Form form,
            Action<string> onFinalText // callback nhận text cuối
        )
        {
            try
            {
                recognizedText.Clear();

                var waveIn = new WaveInEvent
                {
                    WaveFormat = new WaveFormat(16000, 16, 1),
                    BufferMilliseconds = 50,
                    NumberOfBuffers = 3
                };

                waveIn.DataAvailable += (sender, e) =>
                {
                    if (recognizer != null && e.BytesRecorded > 0)
                    {
                        try
                        {
                            if (recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
                            {
                                var result = recognizer.Result();
                                ProcessRecognitionResult(result, false, recognizedText, ptbSearch, form);

                                // Chỉ yêu cầu dừng – cleanup sẽ ở RecordingStopped
                                form.BeginInvoke(new Action(() =>
                                {
                                    try { waveIn.StopRecording(); } catch { }
                                }));
                            }
                            else
                            {
                                var partial = recognizer.PartialResult();
                                ProcessPartialResult(partial, form, ptbSearch, recognizedText);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"DataAvailable error: {ex.Message}");
                        }
                    }
                };

                waveIn.RecordingStopped += (sender, eArgs) =>
                {
                    // Lấy kết quả cuối
                    string searchText = string.Empty;
                    try
                    {
                        if (recognizer != null)
                        {
                            string finalResult = recognizer.FinalResult();
                            var json = JObject.Parse(finalResult);
                            string finalText = json["text"]?.ToString() ?? "";
                            if (!string.IsNullOrWhiteSpace(finalText))
                            {
                                finalText = NormalizeVietnameseText(finalText);
                                recognizedText.Clear();
                                recognizedText.Append(finalText);
                            }
                            searchText = recognizedText.ToString().Trim();
                            if (!string.IsNullOrEmpty(searchText))
                                searchText = char.ToUpper(searchText[0]) + searchText.Substring(1);
                        }
                    }
                    catch (Exception ex) { Debug.WriteLine($"FinalResult error: {ex.Message}"); }

                    // Cập nhật UI input và đặt con trỏ ở cuối
                    form.BeginInvoke(new Action(() =>
                    {
                        var tb = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
                        if (tb != null)
                        {
                            tb.Text = searchText;
                            tb.ForeColor = Color.Black;
                            tb.PlaceholderText = "Tìm kiếm";

                            // ✅ ĐẶT CON TRỎ Ở CUỐI TEXT
                            tb.SelectionStart = tb.Text.Length;
                            tb.SelectionLength = 0;
                        }
                    }));

                    try
                    {
                        waveIn.Dispose();
                    }
                    catch { }

                    // báo về Form để Form tự set isListening=false và filter
                    try { onFinalText?.Invoke(searchText); } catch { }
                };

                // UI báo đang nghe
                rbtnVoice.BackColor = Color.FromArgb(220, 53, 69);
                rbtnVoice.ForeColor = Color.White;
                var textBox = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
                if (textBox != null) textBox.PlaceholderText = "🎤 Đang nghe... (nói rõ ràng)";

                waveIn.StartRecording();
                return waveIn;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi bắt đầu ghi âm:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // reset UI
                rbtnVoice.BackColor = Color.Gainsboro;
                rbtnVoice.ForeColor = Color.DarkGray;
                return null;
            }
        }

        public static string StopListening(
            WaveInEvent waveIn,
            VoskRecognizer recognizer,
            StringBuilder recognizedText,
            Controls.PlaceholderTextBox2 ptbSearch,
            Form form,
            Controls.RoundedButton rbtnVoice
        )
        {
            try { waveIn?.StopRecording(); } catch { }

            // UI trả về bình thường (phần còn lại đã làm trong RecordingStopped)
            rbtnVoice.BackColor = Color.Gainsboro;
            rbtnVoice.ForeColor = Color.DarkGray;

            // Trả text hiện có (RecordingStopped sẽ đã cập nhật)
            return recognizedText.ToString().Trim();
        }



        public static void WaveIn_DataAvailable(object sender, WaveInEventArgs e, VoskRecognizer recognizer, StringBuilder recognizedText, Controls.PlaceholderTextBox2 ptbSearch, Form form, WaveInEvent waveIn, bool isListening, Controls.RoundedButton rbtnVoice, RoundedComboBox rcbFilter)
        {
            if (recognizer != null && e.BytesRecorded > 0)
            {
                try
                {
                    // Kiểm tra có giọng nói không

                    if (recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
                    {
                        string result = recognizer.Result();
                        JObject jsonResult = JObject.Parse(result);
                        string text = jsonResult["text"]?.ToString() ?? "";


                        ProcessRecognitionResult(result, false, recognizedText, ptbSearch, form);

                        // Kiểm tra có giọng nói không
                        form.BeginInvoke(new Action(() => StopListening(waveIn, recognizer, recognizedText, ptbSearch, form, rbtnVoice)));
                    }
                    else
                    {
                        string partialResult = recognizer.PartialResult();
                        JObject jsonPartial = JObject.Parse(partialResult);
                        string partialText = jsonPartial["partial"]?.ToString() ?? "";

                        ProcessPartialResult(partialResult, form, ptbSearch, recognizedText);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"DataAvailable error: {ex.Message}");
                }
            }
        }

        public static void WaveIn_RecordingStopped(object sender, StoppedEventArgs e)
        {
            Debug.WriteLine("Recording stopped");
        }

        public static void ProcessRecognitionResult(string jsonResult, bool isFinal, StringBuilder recognizedText, Controls.PlaceholderTextBox2 ptbSearch, Form form)
        {
            try
            {
                JObject result = JObject.Parse(jsonResult);
                string text = result["text"]?.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(text))
                {
                    // Chuẩn hóa text tiếng Việt
                    text = NormalizeVietnameseText(text);

                    // Cập nhật recognizedText
                    recognizedText.Clear();
                    recognizedText.Append(text);

                    string finalText = recognizedText.ToString().Trim();

                    // Viết hoa chữ cái đầu
                    if (!string.IsNullOrEmpty(finalText))
                    {
                        finalText = char.ToUpper(finalText[0]) + finalText.Substring(1);
                    }

                    form.Invoke(new Action(() =>
                    {
                        var textBox = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
                        if (textBox != null)
                        {
                            textBox.Text = finalText;
                            textBox.ForeColor = isFinal ? Color.Black : Color.Gray;
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ProcessRecognitionResult error: {ex.Message}");
            }
        }

        public static void ProcessPartialResult(string jsonResult, Form form, Controls.PlaceholderTextBox2 ptbSearch, StringBuilder recognizedText)
        {
            try
            {
                JObject result = JObject.Parse(jsonResult);
                string partialText = result["partial"]?.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(partialText))
                {
                    // Chuẩn hóa partial text
                    partialText = UIHelpers.NormalizeVietnameseText(partialText);

                    form.Invoke(new Action(() =>
                    {
                        var textBox = ptbSearch.Controls.OfType<TextBox>().FirstOrDefault();
                        if (textBox != null)
                        {
                            string currentText = recognizedText.ToString().Trim();
                            string displayText = string.IsNullOrEmpty(currentText)
                                ? partialText
                                : $"{currentText} {partialText}";

                            // Viết hoa chữ cái đầu
                            if (!string.IsNullOrEmpty(displayText))
                            {
                                displayText = char.ToUpper(displayText[0]) + displayText.Substring(1);
                            }

                            textBox.Text = displayText;
                            textBox.ForeColor = Color.Gray; // Màu xám cho text tạm thời
                        }
                    }));

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ProcessPartialResult error: {ex.Message}");
            }
        }

        public static string NormalizeVietnameseText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            // Loại bỏ khoảng trắng thừa
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ").Trim();

            return text;
        }

        // ====================  FILTER WRAPPER FOR FORMS  ====================
        public static void ApplyFilterAndRender<T>(
            IEnumerable<T> allSamples,
            string textQuery,
            string statusKeyViNoAccent,
            Func<T, string> searchFields,
            Func<T, string> statusSelector,
            DataGridView gridView,
            Action<IEnumerable<T>> renderRows)
        {
            // Gọi lại 2 hàm lọc sẵn có
            var data = ApplyCombinedFilter(
                allSamples,
                textQuery,
                statusKeyViNoAccent,
                searchFields,
                statusSelector
            );

            // Gọi hàm render lại UI
            renderRows?.Invoke(data);

            // Nếu có DataGridView thì dọn selection và cuộn về đầu
            if (gridView != null && gridView.Rows.Count > 0)
            {
                gridView.ClearSelection();
                gridView.FirstDisplayedScrollingRowIndex = 0;
            }
        }

        #endregion

        #region Format Money
        // ======================== TIỀN TỆ: HÀM DÙNG CHUNG (NO NESTED CLASS) ========================
        private static readonly HashSet<TextBox> currencyFormatting = new();
        private static readonly Dictionary<TextBox, Control> currencyWrapper = new();
        private static readonly Dictionary<TextBox, bool> showUnitWhileTyping = new();

        /// <summary>
        /// Tìm TextBox con (inner) nằm bên trong một control composite (PlaceholderTextBox2...).
        /// </summary>
        public static TextBox FindInnerTextBox(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is TextBox tb) return tb;
                var found = FindInnerTextBox(ctrl);
                if (found != null) return found;
            }
            return parent as TextBox; // nếu chính nó là TextBox
        }

        /// <summary>
        /// Chỉ lấy chữ số từ chuỗi định dạng tiền (bỏ dấu phẩy/khoảng trắng/VNĐ).
        /// </summary>
        public static decimal ParseCurrency(string text)
        {
            if (string.IsNullOrEmpty(text)) return 0m;
            string digits = new string(text.Where(char.IsDigit).ToArray());
            if (string.IsNullOrEmpty(digits)) return 0m;
            return decimal.Parse(digits, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Định dạng "1,234,567 VNĐ" hoặc "1,234,567".
        /// </summary>
        public static string FormatCurrency(decimal value, bool withUnit = true)
        {
            var num = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:N0}", value);
            return withUnit ? $"{num} VNĐ" : num;
        }

        /// <summary>
        /// Set giá trị vào control (hiển thị luôn VNĐ).
        /// </summary>
        public static void SetCurrencyValue(Control wrapper, decimal value, bool withUnit = true)
        {
            var inner = FindInnerTextBox(wrapper);
            string text = FormatCurrency(value, withUnit);
            if (inner != null) inner.Text = text;
            wrapper.Text = text;
        }

        /// <summary>
        /// Lấy giá trị số từ control đã định dạng.
        /// </summary>
        //public static decimal ParseCurrencyFromControl(Control wrapper)
        //{
        //    var inner = FindInnerTextBox(wrapper);
        //    var text = inner != null ? inner.Text : wrapper.Text;
        //    return ParseCurrency(text);
        //}
        public static int ParseCurrencyFromControl(Control wrapper)
        {
            var inner = FindInnerTextBox(wrapper);
            var text = inner != null ? inner.Text : wrapper.Text;

            return ParseCurrencyToInt(text);
        }

        private static int ParseCurrencyToInt(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            // Loại bỏ ký tự không phải số (ví dụ: "12,000 ₫" → "12000")
            string clean = new string(text.Where(char.IsDigit).ToArray());

            if (int.TryParse(clean, out int result))
                return result;

            return 0;
        }


        /// <summary>
        /// Gắn behavior "định dạng tiền động" vào control (khi gõ tự nhóm nghìn + hiển thị VNĐ).
        /// Mặc định: luôn hiển thị "VNĐ" trong khi gõ (showUnitWhileTyping = true).
        /// </summary>
        public static void AttachDynamicCurrencyFormatting(Control wrapper, bool showUnit = true)
        {
            var inner = FindInnerTextBox(wrapper);
            if (inner == null) return;

            currencyWrapper[inner] = wrapper;
            showUnitWhileTyping[inner] = showUnit;

            // KeyPress: chỉ cho nhập số và phím điều khiển
            inner.KeyPress -= Currency_KeyPressOnlyDigits;
            inner.KeyPress += Currency_KeyPressOnlyDigits;

            // Enter: chuẩn hoá ngay khi vào
            inner.Enter -= Currency_EnterNormalize;
            inner.Enter += Currency_EnterNormalize;

            // TextChanged: reformat liên tục
            inner.TextChanged -= Currency_TextChanged_Reformat;
            inner.TextChanged += Currency_TextChanged_Reformat;

            // Leave: luôn đảm bảo có " VNĐ"
            inner.Leave -= Currency_LeaveEnsureUnit;
            inner.Leave += Currency_LeaveEnsureUnit;
        }

        public static void DetachDynamicCurrencyFormatting(Control wrapper)
        {
            var inner = FindInnerTextBox(wrapper);
            if (inner == null) return;

            inner.KeyPress -= Currency_KeyPressOnlyDigits;
            inner.Enter -= Currency_EnterNormalize;
            inner.TextChanged -= Currency_TextChanged_Reformat;
            inner.Leave -= Currency_LeaveEnsureUnit;

            currencyWrapper.Remove(inner);
            showUnitWhileTyping.Remove(inner);
            currencyFormatting.Remove(inner);
        }

        // ======================== Event handlers (private) ========================
        private static void Currency_KeyPressOnlyDigits(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private static void Currency_EnterNormalize(object? sender, EventArgs e)
        {
            if (sender is not TextBox inner) return;
            bool showUnit = showUnitWhileTyping.TryGetValue(inner, out var v) ? v : true;
            Reformat(inner, showUnit);
        }

        private static void Currency_LeaveEnsureUnit(object? sender, EventArgs e)
        {
            if (sender is not TextBox inner) return;
            Reformat(inner, appendUnit: true);
        }

        private static void Currency_TextChanged_Reformat(object? sender, EventArgs e)
        {
            if (sender is not TextBox inner) return;
            if (currencyFormatting.Contains(inner)) return; // đang re-entrant

            bool showUnit = showUnitWhileTyping.TryGetValue(inner, out var v) ? v : true;
            Reformat(inner, showUnit);
        }

        private static void Reformat(TextBox inner, bool appendUnit)
        {
            if (currencyFormatting.Contains(inner)) return;
            currencyFormatting.Add(inner);

            try
            {
                int oldCursor = inner.SelectionStart;
                string old = inner.Text ?? "";

                // Đếm số chữ số trước con trỏ
                int digitsBefore = 0;
                for (int i = 0; i < Math.Min(oldCursor, old.Length); i++)
                    if (char.IsDigit(old[i])) digitsBefore++;

                string digits = new string(old.Where(char.IsDigit).ToArray());
                if (string.IsNullOrEmpty(digits))
                {
                    inner.Text = "";
                    if (currencyWrapper.TryGetValue(inner, out var wrap1))
                        wrap1.Text = "";
                    return;
                }

                decimal number = decimal.Parse(digits, CultureInfo.InvariantCulture);
                string formatted = string.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:N0}", number);
                if (appendUnit) formatted += " VNĐ";

                // Tính lại vị trí con trỏ theo số chữ số đã thấy
                int newCursor = 0, seen = 0;
                for (int i = 0; i < formatted.Length && seen < digitsBefore; i++)
                {
                    newCursor++;
                    if (char.IsDigit(formatted[i])) seen++;
                }

                // Nếu có đơn vị, tránh nhảy vào phần " VNĐ"
                newCursor = appendUnit
                    ? Math.Min(newCursor, Math.Max(0, formatted.Length - 4))
                    : Math.Min(newCursor, formatted.Length);

                inner.Text = formatted;
                if (currencyWrapper.TryGetValue(inner, out var wrap2))
                    wrap2.Text = formatted;

                inner.SelectionStart = newCursor;
                inner.SelectionLength = 0;
            }
            catch
            {
                // nuốt lỗi format edge-case (paste linh tinh), không ném exception ra UI
            }
            finally
            {
                currencyFormatting.Remove(inner);
            }
        }


        #endregion

        #region Set DateTimePicker
        public static void MakeNull(RoundedDateTime dtp)
        {
            if (dtp == null) return;
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.CustomFormat = "'dd/mm/yy'"; // placeholder literal
            dtp.Tag = null;                  // đánh dấu null
        }

        public static void MakeDate(RoundedDateTime dtp, DateTime value)
        {
            if (dtp == null) return;
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.CustomFormat = "dd/MM/yy";
            dtp.Value = value;
            dtp.Tag = value;                 // đánh dấu đã chọn
        }

        // Gắn hành vi null cho 1 dtp: ValueChanged => có date; Phím Delete => về null
        public static void WireNullBehavior(RoundedDateTime dtp)
        {
            if (dtp == null) return;

            dtp.ValueChanged -= Dtp_ValueChanged_ToDate;
            dtp.KeyDown -= Dtp_KeyDown_Clear;

            dtp.ValueChanged += Dtp_ValueChanged_ToDate;
            dtp.KeyDown += Dtp_KeyDown_Clear;
        }

        public static void Dtp_ValueChanged_ToDate(object sender, EventArgs e)
        {
            if (sender is DateTimePicker dtp)
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "dd/MM/yy";
                dtp.Tag = dtp.Value;
            }
        }

        public static void Dtp_KeyDown_Clear(object sender, KeyEventArgs e)
        {
            if (sender is DateTimePicker dtp && e.KeyCode == Keys.Delete)
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "'dd/mm/yy'";
                dtp.Tag = null;
                e.Handled = true;
            }
        }
        // UIHelpers.cs
        public static void SetupFullDateTime(params EMC.UI.Controls.RoundedDateTime[] pickers)
        {
            if (pickers == null || pickers.Length == 0) return;

            foreach (var rdt in pickers)
            {
                if (rdt == null) continue;

                // format hiển thị đầy đủ
                rdt.Format = DateTimePickerFormat.Custom;
                rdt.CustomFormat = "dd/MM/yy hh:mm tt";

                // KHÔNG dùng WireNullBehavior (chỉ-ngày) ở đây
                WireFullNullBehavior(rdt);     // behavior cho ngày+giờ

                // Nếu bạn muốn trạng thái khởi tạo là rỗng:
                // MakeNullFull(rdt);

                // Nếu muốn mặc định bằng "bây giờ":
                // MakeFull(rdt, DateTime.Now);

                rdt.ShowUpDown = false;               // cho phép DropDown mặc định
                rdt.EnableDateTimeDropdown = true;    // popup tùy biến ngày+giờ
            }
        }


        // === DateTime (Ngày + Giờ) có placeholder ===
        public static void MakeNullFull(params RoundedDateTime[] pickers)
        {
            if (pickers == null || pickers.Length == 0) return;

            foreach (var dtp in pickers)
            {
                if (dtp == null) continue;
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "'dd/mm/yy 00:00'";
                dtp.Tag = null;
            }
        }

        public static void MakeFull(RoundedDateTime dtp, DateTime value)
        {
            if (dtp == null) return;
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.CustomFormat = "dd/MM/yy hh:mm tt";
            dtp.Value = value;
            dtp.Tag = value; // đánh dấu đã có giá trị
        }

        public static void WireFullNullBehavior(RoundedDateTime dtp)
        {
            if (dtp == null) return;
            dtp.ValueChanged -= Dtp_ValueChanged_ToFull;
            dtp.KeyDown -= Dtp_KeyDown_ClearFull;

            dtp.ValueChanged += Dtp_ValueChanged_ToFull;
            dtp.KeyDown += Dtp_KeyDown_ClearFull;
        }

        private static void Dtp_ValueChanged_ToFull(object sender, EventArgs e)
        {
            if (sender is DateTimePicker dtp)
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "dd/MM/yy hh:mm tt";
                dtp.Tag = dtp.Value; // đã có giá trị
            }
        }

        private static void Dtp_KeyDown_ClearFull(object sender, KeyEventArgs e)
        {
            if (sender is DateTimePicker dtp && e.KeyCode == Keys.Delete)
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "'dd/mm/yy 00:00'";
                dtp.Tag = null;
                e.Handled = true;
            }
        }

        public static DateTime? SafeGetDate(DateTimePicker picker)
        {
            if (picker == null) return null;
            if (picker.Tag == null) return null;
            // Nếu bạn dùng RoundedDateTime (cũng là DateTimePicker kế thừa), vẫn đọc Value được
            return picker.Value;
        }

        #endregion

        #region WaterMark Logo
        //public static class UIWatermark
        //{
        //    // cache logo dùng chung cho mọi form (load 1 lần)
        //    private static Image _cachedLogo;

        //    public static Image LoadLogoOnce(string relativePath = @"UI\Resources\images\logo.png")
        //    {
        //        if (_cachedLogo != null) return _cachedLogo;
        //        var full = System.IO.Path.Combine(Application.StartupPath, relativePath);
        //        if (System.IO.File.Exists(full)) _cachedLogo = Image.FromFile(full);
        //        return _cachedLogo;
        //    }

        //    // Áp watermark lên 1 control bất kỳ (Panel, TabPage, Form, DataGridView, GroupBox…)
        //    public static void Apply(Control target, Image logo, float opacity = 0.08f, float scaleWidth = 0.4f)
        //    {
        //        if (target == null || logo == null) return;

        //        // double-buffer cho mượt
        //        typeof(Control).GetProperty("DoubleBuffered",
        //            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
        //            ?.SetValue(target, true, null);

        //        // gỡ handler cũ nếu đã gắn
        //        target.Paint -= Target_Paint;
        //        target.Resize -= Target_Resize;

        //        // lưu cấu hình vào Tag
        //        target.Tag = new Tuple<Image, float, float>(logo, opacity, scaleWidth);

        //        // nếu là DataGridView: invalidate khi scroll/đổi cột để watermark luôn theo
        //        if (target is DataGridView dgv)
        //        {
        //            dgv.Scroll -= Dgv_Invalidate;
        //            dgv.RowHeightChanged -= Dgv_Invalidate;
        //            dgv.ColumnWidthChanged -= Dgv_Invalidate;
        //            dgv.RowPostPaint -= Dgv_RowPaint;
        //            dgv.Scroll += Dgv_Invalidate;
        //            dgv.RowHeightChanged += Dgv_Invalidate;
        //            dgv.ColumnWidthChanged += Dgv_Invalidate;
        //            dgv.RowPostPaint += Dgv_RowPaint;
        //        }

        //        target.Paint += Target_Paint;
        //        target.Resize += Target_Resize;
        //        target.Invalidate();
        //    }

        //    public static void Remove(Control target)
        //    {
        //        if (target == null) return;
        //        target.Paint -= Target_Paint;
        //        target.Resize -= Target_Resize;
        //        if (target is DataGridView dgv)
        //        {
        //            dgv.Scroll -= Dgv_Invalidate;
        //            dgv.RowHeightChanged -= Dgv_Invalidate;
        //            dgv.ColumnWidthChanged -= Dgv_Invalidate;
        //            dgv.RowPostPaint -= Dgv_RowPaint;
        //        }
        //        if (target.Tag is Tuple<Image, float, float>) target.Tag = null;
        //        target.Invalidate();
        //    }

        //    // Tiện ích: áp watermark cho mọi TabPage của 1 TabControl
        //    public static void ApplyToAllTabs(TabControl tc, Image logo, float opacity = 0.08f, float scaleWidth = 0.4f)
        //    {
        //        foreach (TabPage tp in tc.TabPages) Apply(tp, logo, opacity, scaleWidth);
        //    }

        //    private static void Target_Resize(object sender, EventArgs e) => (sender as Control)?.Invalidate();
        //    private static void Dgv_Invalidate(object sender, EventArgs e) => (sender as Control)?.Invalidate();
        //    private static void Dgv_RowPaint(object sender, DataGridViewRowPostPaintEventArgs e) => (sender as Control)?.Invalidate();

        //    private static void Target_Paint(object sender, PaintEventArgs e)
        //    {
        //        if (sender is not Control ctrl) return;
        //        if (ctrl.Tag is not Tuple<Image, float, float> info) return;

        //        var logo = info.Item1; float opacity = info.Item2; float scaleW = info.Item3;
        //        if (logo == null || ctrl.ClientSize.Width <= 0 || ctrl.ClientSize.Height <= 0) return;

        //        var area = ctrl.ClientRectangle;

        //        // Tránh đè header của GroupBox
        //        if (ctrl is GroupBox gb)
        //        {
        //            int header = TextRenderer.MeasureText(gb.Text ?? "", gb.Font).Height;
        //            area = new Rectangle(area.X, area.Y + header, area.Width, area.Height - header);
        //        }

        //        var g = e.Graphics;
        //        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        //        float targetW = Math.Max(1f, area.Width * scaleW);
        //        float scale = targetW / logo.Width;
        //        int w = (int)(logo.Width * scale);
        //        int h = (int)(logo.Height * scale);
        //        int x = area.Left + (area.Width - w) / 2;
        //        int y = area.Top + (area.Height - h) / 2;

        //        var cm = new ColorMatrix { Matrix33 = Math.Clamp(opacity, 0f, 1f) };
        //        using var ia = new ImageAttributes();
        //        ia.SetColorMatrix(cm);
        //        g.DrawImage(logo, new Rectangle(x, y, w, h),
        //                    0, 0, logo.Width, logo.Height, GraphicsUnit.Pixel, ia);
        //    }

        //}

        #endregion

        #region Check Null

        /// <summary>
        /// Kiểm tra số điện thoại Việt Nam - đúng 10 chữ số, bắt đầu bằng 0
        /// </summary>
        public static bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Loại bỏ khoảng trắng và ký tự đặc biệt
            string cleanPhone = Regex.Replace(phone, @"[\s\-\(\)\.]+", "");

            // Chỉ chấp nhận đúng 10 chữ số, bắt đầu bằng 0
            if (Regex.IsMatch(cleanPhone, @"^0\d{9}$"))
                return true;

            return false;
        }

        /// <summary>
        /// Kiểm tra email hợp lệ (sử dụng regex và MailAddress)
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Kiểm tra với regex cơ bản
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(email, pattern))
                    return false;

                // Kiểm tra thêm với MailAddress
                var addr = new System.Net.Mail.MailAddress(email);

                // Kiểm tra chiều dài
                if (email.Length > 254)
                    return false;

                // Kiểm tra từng phần
                var parts = email.Split('@');
                if (parts[0].Length > 64)  // Local part không quá 64 ký tự
                    return false;

                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy thông báo lỗi chi tiết cho số điện thoại (10 số, bắt đầu 0)
        /// </summary>
        public static string GetPhoneValidationMessage(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return "Số điện thoại không được để trống!";

            string cleanPhone = Regex.Replace(phone, @"[\s\-\(\)\.]+", "");

            if (!Regex.IsMatch(cleanPhone, @"^[0-9]+$"))
                return "Số điện thoại chỉ được chứa chữ số!";

            if (cleanPhone.Length < 10)
                return "Số điện thoại phải có đúng 10 chữ số!";

            if (cleanPhone.Length > 10)
                return "Số điện thoại chỉ được có 10 chữ số!";

            if (!cleanPhone.StartsWith("0"))
                return "Số điện thoại phải bắt đầu bằng 0!";

            if (!IsValidPhoneNumber(phone))
                return "Số điện thoại không hợp lệ!";

            return "";
        }

        /// <summary>
        /// Lấy thông báo lỗi chi tiết cho email
        /// </summary>
        public static string GetEmailValidationMessage(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "Email không được để trống!";

            if (email.Length > 254)
                return "Email không được vượt quá 254 ký tự!";

            if (!email.Contains("@"))
                return "Email phải chứa ký tự @!";

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "Email không đúng định dạng! Ví dụ: user@example.com";

            if (!IsValidEmail(email))
                return "Email không hợp lệ!";

            return "";
        }


        #endregion
    }
}
