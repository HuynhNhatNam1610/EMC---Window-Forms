using EMC.Service;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace EMC.UI.Forms
{
    public partial class RatingChartControl : UserControl
    {
        private RatingChartService service;
        private int currentYear = DateTime.Now.Year;
        private bool suspendYearSelection = false;
        private Bitmap cachedMap = null;
        private bool isDrawingMap = false;
        private Panel spinnerPanel;
        private PictureBox spinnerImage;
        private System.Windows.Forms.Timer spinnerTimer;
        private float spinnerAngle = 0f;

        public RatingChartControl()
        {
            InitializeComponent();
            service = RatingChartService.Instance;
            InitializeResponsiveDesign();
        }

        public async void Initialize()
        {
            cbYearFilter.SelectedIndexChanged += cbYearFilter_SelectedIndexChanged;
            LoadAvailableYears();
            LoadStatisticsData(currentYear);
            LoadChartData(currentYear);
            LoadVietnamPollutionMapChart();
            ApplyResponsiveLayout();
        }

        #region UI
        public class GoogleStyleScrollPanel : Panel
        {
            [DllImport("user32.dll")]
            private static extern int GetScrollInfo(IntPtr hwnd, int bar, ref SCROLLINFO si);

            [DllImport("user32.dll")]
            private static extern int SetScrollInfo(IntPtr hwnd, int bar, ref SCROLLINFO si, bool bRedraw);

            [StructLayout(LayoutKind.Sequential)]
            private struct SCROLLINFO
            {
                public int cbSize;
                public int fMask;
                public int nMin;
                public int nMax;
                public int nPage;
                public int nPos;
                public int nTrackPos;
            }

            public GoogleStyleScrollPanel()
            {
                this.AutoScroll = true;
                this.HorizontalScroll.Enabled = false;
                this.HorizontalScroll.Visible = false;
                this.VerticalScroll.Enabled = true;
                this.Padding = new Padding(0, 0, 0, 0);
                this.Margin = new Padding(0);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                if (this.VerticalScroll.Visible && this.VerticalScroll.Maximum > 0)
                {
                    DrawCustomScrollBar(e.Graphics);
                }
            }

            private void DrawCustomScrollBar(Graphics g)
            {
                int scrollbarWidth = 8;
                int visibleHeight = this.ClientSize.Height;
                int totalHeight = this.DisplayRectangle.Height;
                if (totalHeight <= visibleHeight) return;

                float thumbHeight = (float)visibleHeight / totalHeight * visibleHeight;
                float thumbTop = (float)this.VerticalScroll.Value / (totalHeight - visibleHeight) * (visibleHeight - thumbHeight);

                Rectangle trackRect = new Rectangle(this.Width - scrollbarWidth - 1, 0, scrollbarWidth, visibleHeight);
                g.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), trackRect);

                Rectangle thumbRect = new Rectangle(this.Width - scrollbarWidth - 1, (int)thumbTop + 2, scrollbarWidth, (int)thumbHeight - 4);
                using (var brush = new SolidBrush(Color.FromArgb(180, 180, 180)))
                {
                    DrawRoundedRectangle(g, thumbRect, 4, brush.Color);
                }
            }

            private void DrawRoundedRectangle(Graphics g, Rectangle bounds, int radius, Color color)
            {
                using (var brush = new SolidBrush(color))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.FillPie(brush, bounds.X, bounds.Y, radius * 2, radius * 2, 180, 90);
                    g.FillPie(brush, bounds.Right - radius * 2, bounds.Y, radius * 2, radius * 2, 270, 90);
                    g.FillPie(brush, bounds.X, bounds.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                    g.FillPie(brush, bounds.Right - radius * 2, bounds.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    g.FillRectangle(brush, bounds.X + radius, bounds.Y, bounds.Width - radius * 2, bounds.Height);
                    g.FillRectangle(brush, bounds.X, bounds.Y + radius, bounds.Width, bounds.Height - radius * 2);
                }
            }

            protected override void OnMouseWheel(MouseEventArgs e)
            {
                base.OnMouseWheel(e);
                this.Invalidate();
            }

            protected override void OnScroll(ScrollEventArgs se)
            {
                base.OnScroll(se);
                this.Invalidate();
            }
        }
        #endregion

        #region Responsive Design
        private void InitializeResponsiveDesign()
        {
            this.Resize += RatingChartControl_Resize;
            SetDoubleBuffered(panel1);
            panel1.AutoScroll = true;
        }

        private void RatingChartControl_Resize(object sender, EventArgs e)
        {
            ApplyResponsiveLayout();
        }

        private void ApplyResponsiveLayout()
        {
            int formWidth = this.ClientSize.Width;
            int contentWidth = formWidth - 60;

            int cardCount = 4;
            int cardSpacing = 15;
            int cardWidth = (contentWidth - (cardSpacing * (cardCount - 1))) / cardCount;
            int cardTopMargin = 40;

            if (cardWidth < 200)
            {
                cardWidth = (contentWidth - cardSpacing) / 2;
            }

            pCard1.Size = new Size(cardWidth, 140);
            pCard1.Location = new Point(0, cardTopMargin);

            pCard2.Size = new Size(cardWidth, 140);
            pCard2.Location = new Point(pCard1.Right + cardSpacing, cardTopMargin);

            pCard3.Size = new Size(cardWidth, 140);
            pCard3.Location = new Point(pCard2.Right + cardSpacing, cardTopMargin);

            pCard4.Size = new Size(cardWidth, 140);
            pCard4.Location = new Point(pCard3.Right + cardSpacing, cardTopMargin);

            pOverdueChart.Width = contentWidth;
            pMonthlyChart.Width = contentWidth;
            pEnvironmentChart.Width = contentWidth;

            panel1.Width = contentWidth;
        }

        private void SetDoubleBuffered(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, control, new object[] { true });
        }
        #endregion

        #region Data Loading

        private void LoadStatisticsData(int year)
        {
            try
            {
                var stats = service.GetStatistics(year);
                lCard1Value.Text = stats.TotalContracts.ToString();
                lCard2Value.Text = stats.OverdueContracts.ToString();
                lCard3Value.Text = stats.ProcessingContracts.ToString();
                lCard4Value.Text = $"{stats.CompletionRate}%";
            }
            catch (Exception)
            {
                lCard1Value.Text = "156";
                lCard2Value.Text = "3";
                lCard3Value.Text = "12";
                lCard4Value.Text = "95%";
            }
        }

        private void LoadChartData()
        {
            LoadChartData(currentYear);
        }

        private void LoadChartData(int year)
        {
            LoadQuarterlyOrderVolumeChart(year);
        }
        #endregion

        #region Chart 1
        private void LoadQuarterlyOrderVolumeChart(int year)
        {
            pOverdueContent.Controls.Clear();
            Chart chart = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ChartAreas = { new ChartArea
                {
                    BackColor = Color.FromArgb(250, 251, 252),
                    BorderColor = Color.FromArgb(226, 232, 240),
                    BorderWidth = 1,
                    AxisX = { MajorGrid = { Enabled = false }, LabelStyle = { Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(51, 65, 85) } },
                    AxisY = { Title = "Số lượng đơn hàng", TitleFont = new Font("Segoe UI", 10F, FontStyle.Bold), TitleForeColor = Color.FromArgb(51, 65, 85), MajorGrid = { LineColor = Color.FromArgb(226, 232, 240), LineDashStyle = ChartDashStyle.Dash }, LabelStyle = { Font = new Font("Segoe UI", 9F), ForeColor = Color.FromArgb(100, 116, 139) } }
                }},
                Legends = { new Legend { Docking = Docking.Top, Alignment = StringAlignment.Center, Font = new Font("Segoe UI", 10F, FontStyle.Bold), BackColor = Color.Transparent } }
            };
            Series seriesOnTime = new Series("Đúng hạn") { ChartType = SeriesChartType.Column, Color = Color.FromArgb(34, 197, 94), IsValueShownAsLabel = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), LabelForeColor = Color.Black, BorderWidth = 2 };
            Series seriesLate = new Series("Trễ hạn") { ChartType = SeriesChartType.Column, Color = Color.FromArgb(239, 68, 68), IsValueShownAsLabel = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), LabelForeColor = Color.Black, BorderWidth = 2 };
            chart.Series.Add(seriesOnTime);
            chart.Series.Add(seriesLate);

            try
            {
                var quarterlyData = service.GetQuarterlyOrderVolume(year);
                if (quarterlyData == null || !quarterlyData.Any())
                {
                    seriesOnTime.Points.AddXY("Không có dữ liệu", 0);
                    seriesLate.Points.AddXY("Không có dữ liệu", 0);
                }
                else
                {
                    foreach (var data in quarterlyData)
                    {
                        seriesOnTime.Points.AddXY(data.Quarter, data.OnTime);
                        seriesLate.Points.AddXY(data.Quarter, data.Late);
                    }
                }
            }
            catch
            {
                seriesOnTime.Points.AddXY("Q1", 120);
                seriesLate.Points.AddXY("Q1", 15);
                seriesOnTime.Points.AddXY("Q2", 140);
                seriesLate.Points.AddXY("Q2", 20);
                seriesOnTime.Points.AddXY("Q3", 110);
                seriesLate.Points.AddXY("Q3", 25);
                seriesOnTime.Points.AddXY("Q4", 130);
                seriesLate.Points.AddXY("Q4", 10);
            }

            chart.MouseClick += (s, ev) =>
            {
                var hit = chart.HitTest(ev.X, ev.Y);
                if (hit.ChartElementType == ChartElementType.DataPoint)
                {
                    MessageBox.Show($"{hit.Series.Name}\n{hit.Series.Points[hit.PointIndex].AxisLabel}: {hit.Series.Points[hit.PointIndex].YValues[0]} hợp đồng",
                        "Chi tiết quý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            pOverdueContent.Controls.Add(chart);
        }
        #endregion

        private void cbYearFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suspendYearSelection) return;
            if (cbYearFilter.SelectedItem == null) return;

            int selectedYear;
            if (cbYearFilter.SelectedItem is int yi) selectedYear = yi;
            else if (!int.TryParse(cbYearFilter.SelectedItem.ToString(), out selectedYear)) return;

            currentYear = selectedYear;
            LoadStatisticsData(selectedYear);
            LoadChartData(selectedYear);
        }

        private void LoadAvailableYears()
        {
            var years = service.GetAvailableYears();

            suspendYearSelection = true;
            try
            {
                cbYearFilter.Items.Clear();

                foreach (var year in years)
                    cbYearFilter.Items.Add(year);

                int currentYearLocal = DateTime.Now.Year;
                if (!cbYearFilter.Items.Contains(currentYearLocal))
                    cbYearFilter.Items.Add(currentYearLocal);

                var sortedItems = cbYearFilter.Items.Cast<object>()
                                    .Select(o => o.ToString())
                                    .Distinct()
                                    .Select(s => int.TryParse(s, out var v) ? v : (int?)null)
                                    .Where(v => v.HasValue)
                                    .Select(v => v.Value)
                                    .OrderByDescending(y => y)
                                    .ToList();

                cbYearFilter.Items.Clear();
                foreach (var year in sortedItems)
                    cbYearFilter.Items.Add(year);

                int idx = cbYearFilter.Items.Cast<object>().ToList().FindIndex(i => i.ToString() == currentYearLocal.ToString());
                if (idx >= 0)
                {
                    cbYearFilter.SelectedIndex = idx;
                    currentYear = currentYearLocal;
                }
            }
            finally
            {
                suspendYearSelection = false;
            }
        }



        #region Chart 3 - Vietnam Pollution Map

        private void LoadVietnamPollutionMapChart()
        {
            pEnvironmentContent.Controls.Clear();
            var mainContainer = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(0) };

            var legendPanel = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(249, 250, 251), Padding = new Padding(25, 12, 25, 12), BorderStyle = BorderStyle.FixedSingle };
            var legendTitle = new Label { Text = "📌 Chú thích mức độ ô nhiễm (AQI - Chuẩn US EPA):", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105), AutoSize = true, Location = new Point(25, 15) };
            legendPanel.Controls.Add(legendTitle);

            // Hàng 1 (tất cả trên cùng 1 hàng)
            AddLegendItem(legendPanel, Color.FromArgb(0, 228, 0), "✅ Tốt (0-50)", 30, 45);
            AddLegendItem(legendPanel, Color.FromArgb(255, 255, 0), "🙂 Trung bình (51-100)", 200, 45);
            AddLegendItem(legendPanel, Color.FromArgb(255, 126, 0), "😐 Không tốt (101-150)", 410, 45);
            AddLegendItem(legendPanel, Color.FromArgb(255, 0, 0), "😷 Xấu (151-200)", 620, 45);
            AddLegendItem(legendPanel, Color.FromArgb(153, 0, 76), "😨 Rất xấu (201-300)", 830, 45);
            AddLegendItem(legendPanel, Color.FromArgb(76, 0, 38), "☠️ Nguy hiểm (301+)", 1040, 45);

            var mapPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true
            };

            mapPanel.Paint += (s, e) =>
            {
                Rectangle rect = mapPanel.ClientRectangle;
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    rect,
                    Color.FromArgb(225, 239, 254),
                    Color.FromArgb(186, 230, 253),
                    45f))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }
            };

            var vietnamMapContainer = new Panel
            {
                Size = new Size(950, 1200),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Location = new Point(20, 20)
            };

            vietnamMapContainer.Paint += async (s, e) =>
            {
                if (cachedMap != null)
                {
                    e.Graphics.DrawImageUnscaled(cachedMap, Point.Empty);
                    return;
                }

                if (isDrawingMap) return;
                isDrawingMap = true;

                ShowLoading(pEnvironmentContent, "Đang tải bản đồ Việt Nam...");

                await Task.Run(() =>
                {
                    Bitmap bmp = new Bitmap(vietnamMapContainer.Width, vietnamMapContainer.Height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        DrawVietnamMapWith34Provinces(g, vietnamMapContainer.ClientRectangle);
                    }
                    cachedMap = bmp;
                });

                HideLoading(pEnvironmentContent);
                vietnamMapContainer.Invalidate();
                isDrawingMap = false;
            };
            vietnamMapContainer.MouseClick += VietnamMapContainer_MouseClick;
            vietnamMapContainer.Cursor = Cursors.Hand;

            mapPanel.Controls.Add(vietnamMapContainer);
            mainContainer.Controls.Add(mapPanel);
            mainContainer.Controls.Add(legendPanel);
            pEnvironmentContent.Controls.Add(mainContainer);
        }

        private void DrawVietnamMapWith34Provinces(Graphics g, Rectangle bounds)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            using (var bgBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                bounds, Color.FromArgb(225, 239, 254), Color.FromArgb(186, 230, 253), 45f))
            {
                g.FillRectangle(bgBrush, bounds);
            }

            var provinceDataDict = EnvironmentalPollutionForecastService.Instance.GetRealTimeProvinceData();
            var vietnamProvinces = Get34ProvincesPolygons();

            foreach (var province in vietnamProvinces)
            {
                if (provinceDataDict.ContainsKey(province.Key))
                {
                    var data = provinceDataDict[province.Key];
                    Color color = GetRegionColor(data.PollutionLevel);

                    using (var shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
                    using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                        GetPolygonBounds(province.Value),
                        Color.FromArgb(220, color),
                        Color.FromArgb(180, color),
                        45f))
                    using (var pen = new Pen(Color.White, 3f))
                    using (var borderPen = new Pen(Color.FromArgb(203, 213, 225), 1.5f))
                    {
                        var shadowPoints = province.Value.Select(p => new Point(p.X + 3, p.Y + 3)).ToArray();
                        g.FillPolygon(shadowBrush, shadowPoints);

                        g.FillPolygon(brush, province.Value);
                        g.DrawPolygon(pen, province.Value);
                        g.DrawPolygon(borderPen, province.Value);
                    }

                    var center = GetPolygonCenter(province.Value);

                    using (var font = new Font("Segoe UI", 10F, FontStyle.Bold))
                    using (var textBrush = new SolidBrush(Color.FromArgb(15, 23, 42)))
                    using (var shadowBrush = new SolidBrush(Color.FromArgb(80, 255, 255, 255)))
                    {
                        var textSize = g.MeasureString(province.Key, font);
                        float x = center.X - textSize.Width / 2;
                        float y = center.Y - textSize.Height / 2;

                        g.DrawString(province.Key, font, shadowBrush, x + 1.5f, y + 1.5f);
                        g.DrawString(province.Key, font, textBrush, x, y);
                    }

                    using (var font = new Font("Segoe UI", 7.5F, FontStyle.Bold))
                    {
                        string aqi = $"{data.PollutionLevel:F0}";
                        var textSize = g.MeasureString(aqi, font);
                        float badgeX = center.X - textSize.Width / 2 - 4;
                        float badgeY = center.Y + 14;

                        using (var badgeBrush = new SolidBrush(Color.FromArgb(200, color)))
                        using (var textBrush = new SolidBrush(Color.White))
                        {
                            var badgeRect = new RectangleF(badgeX, badgeY, textSize.Width + 8, textSize.Height + 4);
                            g.FillRoundedRectangle(badgeBrush, Rectangle.Round(badgeRect), 4);
                            g.DrawString(aqi, font, textBrush, badgeX + 4, badgeY + 2);
                        }
                    }
                }
            }
        }

        private async void VietnamMapContainer_MouseClick(object sender, MouseEventArgs e)
        {
            var provinces = Get34ProvincesPolygons();

            foreach (var province in provinces)
            {
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    path.AddPolygon(province.Value);
                    if (path.IsVisible(e.Location))
                    {
                        await OpenInteractiveMapWithCommunePointsAsync(province.Key);
                        return;
                    }
                }
            }
        }

        private async Task OpenInteractiveMapWithCommunePointsAsync(string provinceName)
        {
            if (provinceName == "Hoàng Sa" || provinceName == "Trường Sa") { return; }
            ShowLoading(pEnvironmentContent, $"Đang tải dữ liệu chi tiết {provinceName}...");

            try
            {
                var result = await Task.Run(async () =>
                {

                    string provinceCode = await EnvironmentalPollutionForecastService.Instance
                        .GetProvinceCodeByNameAsync("2025-07-01", provinceName);

                    if (string.IsNullOrWhiteSpace(provinceCode))
                        throw new Exception($"Không tìm được mã tỉnh cho {provinceName}.");

                    var communePoints = await EnvironmentalPollutionForecastService.Instance.GetCommuneWeatherPointsByProvinceAsync(
                        "2025-07-01", provinceCode, provinceName
                    );
                    if (communePoints == null || communePoints.Count == 0)
                        throw new Exception($"Không có dữ liệu xã/phường cho {provinceName}.");

                    string htmlContent = GenerateCommuneMapHtml(provinceName, communePoints);
                    //string tempPath = Path.Combine(
                    //    AppDomain.CurrentDomain.BaseDirectory,
                    //    $"pollution_map_{provinceName}_communes.html"
                    //);
                    //File.WriteAllText(tempPath, htmlContent, Encoding.UTF8);
                    string safeProvince = provinceName.Replace(" ", "_");
                    string tempPath = Path.Combine(
                        Path.GetTempPath(),
                        $"pollution_map_{safeProvince}_{Guid.NewGuid()}.html"
                    );

                    File.WriteAllText(tempPath, htmlContent, Encoding.UTF8);

                    return tempPath;
                });

                this.Invoke((MethodInvoker)(() =>
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = result,
                        UseShellExecute = true
                    });
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu chi tiết cho {provinceName}: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideLoading(pEnvironmentContent);
            }
        }

        private string GenerateCommuneMapHtml(string provinceName, List<CommuneWeatherPoint> points)
        {
            if (points == null || points.Count == 0)
                return "<html><body><h1>Không có dữ liệu</h1></body></html>";


            double centerLat = points.Average(p => p.Lat);
            double centerLng = points.Average(p => p.Lon);
            var markers = points.Select(p =>
            {
                // ✅ Dùng GetCompleteLocationDataCached thay vì GetCompleteLocationData
                var fullData = EnvironmentalPollutionForecastService.Instance
                    .GetCompleteLocationDataCached(p.Lat, p.Lon);  // ← CACHE VERSION

                return new
                {
                    name = p.CommuneName,
                    address = p.Address,
                    lat = p.Lat,
                    lng = p.Lon,
                    temp = fullData.Temperature,
                    humidity = fullData.Humidity,
                    aqi = fullData.AQI,
                    pollutant = fullData.MainPollutant,
                    description = fullData.WeatherDescription,
                    pm25 = fullData.PM25,
                    pm10 = fullData.PM10,
                    o3 = fullData.O3,
                    no2 = fullData.NO2,
                    so2 = fullData.SO2,
                    co = fullData.CO,
                    windSpeed = fullData.WindSpeed,
                    forecast1 = fullData.Forecasts.Count > 0 ? fullData.Forecasts[0] : "📊 Chưa có dữ liệu",
                    forecast2 = fullData.Forecasts.Count > 1 ? fullData.Forecasts[1] : "📊 Chưa có dữ liệu",
                    forecast3 = fullData.Forecasts.Count > 2 ? fullData.Forecasts[2] : "📊 Chưa có dữ liệu"
                };
            }).ToList();

            // ✅ LẤY THỐNG KÊ SAU KHI FETCH (AQI thực tế)
            int goodCount = markers.Count(m => m.aqi <= 50);
            int moderateCount = markers.Count(m => m.aqi > 50 && m.aqi <= 100);
            int badCount = markers.Count(m => m.aqi > 100 && m.aqi <= 150);
            int dangerousCount = markers.Count(m => m.aqi > 150);

            double avgAQI = markers.Average(m => m.aqi);
            double maxAQI = markers.Max(m => m.aqi);
            double minAQI = markers.Min(m => m.aqi);

            var worstCommune = markers.OrderByDescending(m => m.aqi).First();
            var bestCommune = markers.OrderBy(m => m.aqi).First();

            string markersJson = JsonConvert.SerializeObject(markers);

            return $@"<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='utf-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <title>Bản đồ chi tiết: {provinceName}</title>
    <link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css' />
    <script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js'></script>
    <style>
        * {{ margin: 0; padding: 0; box-sizing: border-box; }}
        html, body {{ height: 100%; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; overflow: hidden; }}
        #map {{ height: 100vh; width: 100%; }}
        
        .info-panel {{ 
            position: absolute; 
            top: 100px; 
            left: 20px; 
            background: white; 
            border-radius: 15px; 
            box-shadow: 0 8px 24px rgba(0,0,0,0.2); 
            max-width: 450px; 
            z-index: 999; 
            max-height: calc(100vh - 140px); 
            overflow: hidden; 
            transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1); 
        }}
        
        .info-panel.collapsed {{ 
            max-width: 50px; 
            max-height: auto;
        }}
        
        .info-panel.collapsed .info-content {{ 
            display: none; 
        }}
        
        .info-panel.collapsed .info-header {{ 
            justify-content: center;
            padding: 10px;
        }}
        
        .info-header {{ 
            padding: 20px 25px; 
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
            color: white; 
            display: flex; 
            justify-content: space-between; 
            align-items: center; 
            cursor: pointer;
            border-radius: 15px 15px 0 0;
        }}
        
        .info-panel.collapsed .info-header {{ 
            border-radius: 15px;
        }}
        
        .info-title {{
            flex: 1;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            transition: all 0.3s ease;
        }}
        
        .info-panel.collapsed .info-title {{
            display: none;
        }}
        
        .toggle-btn {{ 
            background: none;
            border: none;
            color: white;
            font-size: 18px;
            cursor: pointer;
            padding: 5px;
            display: flex;
            align-items: center;
            justify-content: center;
            min-width: 30px;
            transition: transform 0.3s ease;
        }}
        
        .toggle-btn:hover {{
            transform: scale(1.1);
        }}
        
        .info-content {{ 
            padding: 25px; 
            font-size: 14px; 
            color: #475569; 
            max-height: calc(100vh - 180px); 
            overflow-y: auto; 
        }}
        
        .detail-panel {{ 
            position: absolute; 
            bottom: 30px; 
            right: 20px; 
            background: white; 
            border-radius: 12px; 
            box-shadow: 0 8px 24px rgba(0,0,0,0.2); 
            padding: 16px; 
            max-width: 550px; 
            width: 90%; 
            z-index: 998; 
            max-height: 500px; 
            overflow-y: auto; 
            display: none; 
            animation: slideIn 0.3s ease; 
        }}
        
        .detail-panel.show {{ 
            display: block; 
        }}
        
        .detail-row {{ 
            display: flex; 
            justify-content: space-between; 
            margin: 6px 0; 
            padding: 6px; 
            background: #f8fafc; 
            border-radius: 4px; 
            font-size: 13px; 
        }}
        
        .pollutant-grid {{ 
            display: grid; 
            grid-template-columns: 1fr 1fr 1fr; 
            gap: 6px; 
            margin-top: 8px; 
        }}
        
        .pollutant-item {{ 
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
            color: white; 
            padding: 8px; 
            border-radius: 6px; 
            text-align: center; 
            font-size: 10px; 
        }}
        
        .timestamp {{ 
            position: absolute; 
            top: 20px; 
            right: 20px; 
            background: rgba(0,0,0,0.7); 
            color: white; 
            padding: 12px 18px; 
            border-radius: 10px; 
            font-size: 12px; 
            z-index: 1000; 
        }}
        
        .legend {{ 
            position: absolute; 
            bottom: 30px; 
            left: 20px; 
            background: white; 
            padding: 18px; 
            border-radius: 12px; 
            box-shadow: 0 6px 20px rgba(0,0,0,0.15); 
            z-index: 997; 
        }}
        
        .leaflet-control-container {{
            z-index: 996 !important;
        }}

        .dot {{display: inline-block;
            width: 14px;
            height: 14px;
            border-radius: 50%;
            margin-right: 4px;
            vertical-align: middle;
        }}

        .aqi-good            {{background: #00E400; }}
        .aqi-moderate        {{background: #FFFF00; }}
        .aqi-unhealthy-sens  {{background: #FF7E00; }}
        .aqi-unhealthy       {{background: #FF0000; }}
        .aqi-very-unhealthy  {{background: #99004C; }}
        .aqi-hazardous       {{background: #4C0026; }}
        
        @keyframes slideIn {{ 
            from {{ transform: translateY(20px); opacity: 0; }} 
            to {{ transform: translateY(0); opacity: 1; }} 
        }}
    </style>
</head>
<body>
    <div id='map'></div>
    <div class='timestamp'>🕐 Cập nhật: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</div>
    
    <div class='info-panel' id='infoPanel'>
        <div class='info-header'>
            <div class='info-title'>📍 {provinceName}</div>
            <button class='toggle-btn' id='toggleBtn' onclick='togglePanel(event)' title='Thu gọn/Mở rộng'>⬅️</button>
        </div>
        <div class='info-content'>
            <div><strong>📊 Thống Kê:</strong></div>
            <div class='detail-row'><span>Tổng xã/phường:</span><span>{points.Count}</span></div>
            <div class='detail-row'><span>AQI Trung bình:</span><span>{avgAQI:F1}</span></div>
            <div class='detail-row'><span>AQI cao nhất:</span><span>{maxAQI:F1} ({worstCommune.name})</span></div>
            <div class='detail-row'><span>AQI thấp nhất:</span><span>{minAQI:F1} ({bestCommune.name})</span></div>
        </div>
    </div>
    
    <div class='detail-panel' id='detailPanel'>
        <div style='display:flex; justify-content:space-between; margin-bottom:10px;'>
            <span id='detailTitle' style='font-weight:bold;'>📍 Chi tiết</span>
            <span onclick='closeDetailPanel()' style='cursor:pointer; font-weight:bold;'>✕</span>
        </div>
        <div id='detailContent'></div>
    </div>
    
    <div class='legend'>
        <div><strong>📌 Chú Thích AQI</strong></div>
        <div style='margin-top:8px; font-size:12px; line-height:20px;'>

            <span class='dot aqi-good'></span> Tốt (0–50) |
            <span class='dot aqi-moderate'></span> TB (51–100) |
            <span class='dot aqi-unhealthy-sens'></span> Không tốt (101–150) |
            <span class='dot aqi-unhealthy'></span> Xấu (151–200) |
            <span class='dot aqi-very-unhealthy'></span> Rất xấu (201–300) |
            <span class='dot aqi-hazardous'></span> Nguy hiểm (>300)

        </div>
    </div>

    
    <script>
        const map = L.map('map').setView([{centerLat.ToString(CultureInfo.InvariantCulture)}, {centerLng.ToString(CultureInfo.InvariantCulture)}], 10);
        L.tileLayer('https://{{s}}.tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png', {{ attribution: '© OpenStreetMap', maxZoom: 19 }}).addTo(map);
        const markers = {markersJson};
        
        let panelCollapsed = false;
        
        function getColorByAQI(aqi) {{
            if (aqi <= 50) return '#22c55e';
            if (aqi <= 100) return '#eab308';
            if (aqi <= 150) return '#ef4444';
            return '#9333ea';
        }}
        
        markers.forEach(function(p, index) {{
            if (!p.lat || !p.lng) return;
            const color = getColorByAQI(p.aqi);

            const marker = L.circleMarker([p.lat, p.lng], {{
                radius: 7,
                fillColor: color,
                color: '#ffffff',
                weight: 2,
                opacity: 1,
                fillOpacity: 0.85
            }}).addTo(map);

            marker.bindTooltip(
                `<b>${{p.name}}</b><br/>AQI: ${{p.aqi}} (${{p.pollutant}})<br/>🌡️ ${{p.temp}}°C | 💧 ${{p.humidity}}%`,
                {{ permanent: false, direction: 'top', offset: [0, -10], opacity: 0.9 }}
            );

            marker.on('mouseover', function (e) {{
                this.openTooltip();
            }});
            
            marker.on('mouseout', function (e) {{
                this.closeTooltip();
            }});

            marker.on('click', function (e) {{
                showDetail(index);
            }});
        }});

        
        function showDetail(index) {{
            const p = markers[index];
            const color = getColorByAQI(p.aqi);
            document.getElementById('detailTitle').textContent = '📍 ' + p.name;
            let html = `
                <div class='detail-row'><span>🏘️ Xã/Phường:</span><span>${{p.name}}</span></div>
                <div class='detail-row'><span>AQI:</span><span style='color:${{color}};'>${{p.aqi}}</span></div>
                <div class='detail-row'><span>🌡️ Nhiệt độ:</span><span>${{p.temp}}°C</span></div>
                <div class='detail-row'><span>💧 Độ ẩm:</span><span>${{p.humidity}}%</span></div>
                <div class='detail-row'><span>💨 Gió:</span><span>${{p.windSpeed}} km/h</span></div>
                <div class='detail-row'><span>☁️ Điều kiện:</span>${{p.description}} </span>
            </div>
                <div style='margin-top:8px; padding-top:8px; border-top:1px solid #e2e8f0;'>
                    <div style='font-weight:bold; font-size:12px; margin-bottom:6px;'>🧪 Chất ô nhiễm:<small> (hiển thị theo µg/m³, đã chuẩn hóa để so sánh)</small></div>
                    <div class='pollutant-grid'>
                        <div class='pollutant-item'><div>PM2.5</div><div>${{p.pm25}}</div></div>
                        <div class='pollutant-item'><div>PM10</div><div>${{p.pm10}}</div></div>
                        <div class='pollutant-item'><div>O₃</div><div>${{p.o3}}</div></div>
                        <div class='pollutant-item'><div>NO₂</div><div>${{p.no2}}</div></div>
                        <div class='pollutant-item'><div>SO₂</div><div>${{p.so2}}</div></div>
                        <div class='pollutant-item'><div>CO</div><div>${{p.co}}</div></div>
                    </div>
                </div>
                <div style='background:#3b82f6; color:white; padding:10px; border-radius:6px; margin-top:8px; font-size:11px;'>
                    <strong>📅 Dự báo:</strong><br/>
                    ${{p.forecast1}}<br/>
                    ${{p.forecast2}}<br/>
                    ${{p.forecast3}}
                </div>
            `;
            document.getElementById('detailContent').innerHTML = html;
            document.getElementById('detailPanel').classList.add('show');
        }}
        
        function closeDetailPanel() {{
            document.getElementById('detailPanel').classList.remove('show');
        }}

        function togglePanel(event) {{
            event.stopPropagation();
            const panel = document.getElementById('infoPanel');
            const btn = document.getElementById('toggleBtn');

            panelCollapsed = !panelCollapsed;

            if (panelCollapsed) {{
                panel.classList.add('collapsed');
                btn.textContent = '➡️';
                btn.title = 'Mở rộng';
            }} else {{
                panel.classList.remove('collapsed');
                btn.textContent = '⬅️';
                btn.title = 'Thu gọn';
            }}
        }}

    </script>
</body>
</html>";
        }

        private Rectangle GetPolygonBounds(Point[] points)
        {
            int minX = points.Min(p => p.X);
            int minY = points.Min(p => p.Y);
            int maxX = points.Max(p => p.X);
            int maxY = points.Max(p => p.Y);
            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        private Color GetRegionColor(double level)
        {
            return EnvironmentalPollutionForecastService.GetAQIColor(level);
        }


        private PointF GetPolygonCenter(Point[] points)
        {
            float x = 0, y = 0;
            foreach (var p in points)
            {
                x += p.X;
                y += p.Y;
            }
            return new PointF(x / points.Length, y / points.Length);
        }

        private void AddLegendItem(Panel panel, Color color, string text, int x, int y)
        {
            var colorBox = new Panel { Size = new Size(16, 16), Location = new Point(x, y), BackColor = color, BorderStyle = BorderStyle.FixedSingle };
            var label = new Label { Text = text, Location = new Point(x + 22, y - 2), AutoSize = true, Font = new Font("Segoe UI", 9F), ForeColor = Color.FromArgb(71, 85, 105) };
            panel.Controls.Add(colorBox);
            panel.Controls.Add(label);
        }

        private Dictionary<string, Point[]> Get34ProvincesPolygons()
        {
            return new Dictionary<string, Point[]>
            {
                {"Hoàng Sa", new Point[] { new Point(580, 460), new Point(620, 450), new Point(630, 480), new Point(610, 500), new Point(570, 490) }},
                {"Trường Sa", new Point[] { new Point(590, 570), new Point(630, 560), new Point(640, 590), new Point(620, 610), new Point(580, 600) }},
                {"Sơn La", new Point[] { new Point(100, 50), new Point(150, 40), new Point(170, 60), new Point(160, 90), new Point(120, 100) }},
                {"Tuyên Quang", new Point[] { new Point(200, 60), new Point(250, 50), new Point(270, 70), new Point(260, 100), new Point(210, 110) }},
                {"Thái Nguyên", new Point[] { new Point(280, 80), new Point(330, 70), new Point(350, 90), new Point(340, 120), new Point(290, 130) }},
                {"Lạng Sơn", new Point[] { new Point(360, 50), new Point(410, 40), new Point(430, 60), new Point(420, 90), new Point(370, 100) }},
                {"Bắc Ninh", new Point[] { new Point(320, 130), new Point(370, 120), new Point(390, 140), new Point(380, 170), new Point(330, 180) }},
                {"Quảng Ninh", new Point[] { new Point(400, 110), new Point(450, 100), new Point(470, 120), new Point(460, 150), new Point(410, 160) }},
                {"Hà Nội", new Point[] { new Point(250, 140), new Point(310, 130), new Point(340, 150), new Point(330, 180), new Point(270, 190) }},
                {"Hải Phòng", new Point[] { new Point(360, 180), new Point(410, 170), new Point(430, 190), new Point(420, 220), new Point(370, 230) }},
                {"Ninh Bình", new Point[] { new Point(300, 210), new Point(350, 200), new Point(370, 220), new Point(360, 250), new Point(310, 260) }},
                {"Thanh Hóa", new Point[] { new Point(280, 260), new Point(340, 250), new Point(360, 270), new Point(350, 300), new Point(290, 310) }},
                {"Nghệ An", new Point[] { new Point(310, 310), new Point(360, 300), new Point(380, 320), new Point(370, 350), new Point(320, 360) }},
                {"Quảng Trị", new Point[] { new Point(330, 360), new Point(380, 350), new Point(400, 370), new Point(390, 400), new Point(340, 410) }},
                {"Huế", new Point[] { new Point(350, 410), new Point(400, 400), new Point(420, 420), new Point(410, 450), new Point(360, 460) }},
                {"Đà Nẵng", new Point[] { new Point(360, 460), new Point(410, 450), new Point(430, 470), new Point(420, 500), new Point(370, 510) }},
                {"Quảng Ngãi", new Point[] { new Point(380, 510), new Point(430, 500), new Point(450, 520), new Point(440, 550), new Point(390, 560) }},
                {"Gia Lai", new Point[] { new Point(320, 550), new Point(370, 540), new Point(390, 560), new Point(380, 590), new Point(330, 600) }},
                {"Khánh Hòa", new Point[] { new Point(400, 560), new Point(450, 550), new Point(470, 570), new Point(460, 600), new Point(410, 610) }},
                {"Đắk Lắk", new Point[] { new Point(340, 610), new Point(390, 600), new Point(410, 620), new Point(400, 650), new Point(350, 660) }},
                {"Lâm Đồng", new Point[] { new Point(360, 660), new Point(410, 650), new Point(430, 670), new Point(420, 700), new Point(370, 710) }},
                {"Hồ Chí Minh", new Point[] { new Point(280, 710), new Point(340, 700), new Point(360, 720), new Point(350, 750), new Point(290, 760) }},
                {"Đồng Nai", new Point[] { new Point(350, 760), new Point(400, 750), new Point(420, 770), new Point(410, 800), new Point(360, 810) }},
                {"Tây Ninh", new Point[] { new Point(260, 760), new Point(310, 750), new Point(330, 770), new Point(320, 800), new Point(270, 810) }},
                {"Long An", new Point[] { new Point(240, 810), new Point(290, 800), new Point(310, 820), new Point(300, 850), new Point(250, 860) }},
                {"Đồng Tháp", new Point[] { new Point(220, 860), new Point(270, 850), new Point(290, 870), new Point(280, 900), new Point(230, 910) }},
                {"An Giang", new Point[] { new Point(200, 910), new Point(250, 900), new Point(270, 920), new Point(260, 950), new Point(210, 960) }},
                {"Kiên Giang", new Point[] { new Point(180, 960), new Point(230, 950), new Point(250, 970), new Point(240, 1000), new Point(190, 1010) }},
                {"Cần Thơ", new Point[] { new Point(240, 1010), new Point(290, 1000), new Point(310, 1020), new Point(300, 1050), new Point(250, 1060) }},
                {"Vĩnh Long", new Point[] { new Point(270, 1060), new Point(320, 1050), new Point(340, 1070), new Point(330, 1100), new Point(280, 1110) }},
                {"Cà Mau", new Point[] { new Point(220, 1110), new Point(270, 1100), new Point(290, 1120), new Point(280, 1150), new Point(230, 1160) }},
                {"Hà Tĩnh", new Point[] { new Point(300, 320), new Point(340, 310), new Point(360, 330), new Point(350, 360), new Point(310, 370) }},
                {"Hưng Yên", new Point[] { new Point(290, 170), new Point(340, 160), new Point(360, 180), new Point(350, 210), new Point(300, 220) }},
                {"Lai Châu", new Point[] { new Point(60, 40), new Point(100, 30), new Point(120, 50), new Point(110, 80), new Point(70, 90) }},
                {"Cao Bằng", new Point[] { new Point(320, 30), new Point(370, 20), new Point(390, 40), new Point(380, 70), new Point(330, 80) }},
                {"Điện Biên", new Point[] { new Point(40, 80), new Point(90, 70), new Point(110, 90), new Point(100, 120), new Point(50, 130) }},
                {"Lào Cai", new Point[] { new Point(120, 20), new Point(170, 10), new Point(190, 30), new Point(180, 60), new Point(130, 70) }},
                {"Phú Thọ", new Point[] { new Point(180, 110), new Point(230, 100), new Point(250, 120), new Point(240, 150), new Point(190, 160) }}
            };
        }

        #endregion

        public void RefreshData()
        {
            LoadStatisticsData(currentYear);
            LoadChartData(currentYear);
        }

        private void ShowLoading(Control parent, string message)
        {
            if (spinnerPanel != null) return;

            spinnerPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(200, 255, 255, 255) };
            parent.Controls.Add(spinnerPanel);
            spinnerPanel.BringToFront();

            spinnerImage = new PictureBox
            {
                Size = new Size(64, 64),
                Location = new Point((spinnerPanel.Width - 64) / 2, (spinnerPanel.Height - 64) / 2),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            spinnerImage.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var pen = new Pen(Color.FromArgb(59, 130, 246), 6))
                {
                    e.Graphics.TranslateTransform(spinnerImage.Width / 2, spinnerImage.Height / 2);
                    e.Graphics.RotateTransform(spinnerAngle);
                    e.Graphics.DrawArc(pen, -20, -20, 40, 40, 0, 270);
                }
            };
            spinnerPanel.Controls.Add(spinnerImage);

            spinnerTimer = new System.Windows.Forms.Timer { Interval = 20 };
            spinnerTimer.Tick += (s, e) =>
            {
                spinnerAngle += 8;
                if (spinnerAngle >= 360) spinnerAngle = 0;
                spinnerImage.Invalidate();
            };
            spinnerTimer.Start();
        }

        private void HideLoading(Control parent)
        {
            if (spinnerTimer != null)
            {
                spinnerTimer.Stop();
                spinnerTimer.Dispose();
                spinnerTimer = null;
            }

            if (spinnerPanel != null)
            {
                parent.Controls.Remove(spinnerPanel);
                spinnerPanel.Dispose();
                spinnerPanel = null;
            }
        }
    }

    #region Class
    public static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, Rectangle rect, float radius)
        {
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
                path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
                path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                path.CloseFigure();
                g.FillPath(brush, path);
            }
        }
    }
    #endregion
}