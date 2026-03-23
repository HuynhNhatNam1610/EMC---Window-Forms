using EMC.UI.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace EMC.UI.Forms
{
    partial class RatingChartControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dọn control mặc định
                if (components != null)
                    components.Dispose();

                // 🧹 Dọn tài nguyên custom bạn tạo thêm
                cachedMap?.Dispose();
                cachedMap = null;

                spinnerTimer?.Stop();
                spinnerTimer?.Dispose();
                spinnerTimer = null;

                spinnerImage?.Dispose();
                spinnerImage = null;

                spinnerPanel?.Dispose();
                spinnerPanel = null;
            }

            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            panel1 = new GoogleStyleScrollPanel();
            pEnvironmentChart = new Panel();
            pEnvironmentContent = new Panel();
            lEnvironmentTitle = new Label();
            pChartRow1 = new Panel();
            pOverdueChart = new Panel();
            pOverdueContent = new Panel();
            lOverdueTitle = new Label();
            pStatsCards = new Panel();
            cbYearFilter = new RoundedComboBox();
            pCard4 = new Panel();
            lCard4Title = new Label();
            lCard4Value = new Label();
            lCard4Icon = new Label();
            pCard3 = new Panel();
            lCard3Title = new Label();
            lCard3Value = new Label();
            lCard3Icon = new Label();
            pCard2 = new Panel();
            lCard2Title = new Label();
            lCard2Value = new Label();
            lCard2Icon = new Label();
            pCard1 = new Panel();
            lCard1Title = new Label();
            lCard1Value = new Label();
            lCard1Icon = new Label();
            pMonthlyChart = new Panel();
            pMonthlyContent = new Panel();
            lMonthlyTitle = new Label();
            panel1.SuspendLayout();
            pEnvironmentChart.SuspendLayout();
            pChartRow1.SuspendLayout();
            pOverdueChart.SuspendLayout();
            pStatsCards.SuspendLayout();
            pCard4.SuspendLayout();
            pCard3.SuspendLayout();
            pCard2.SuspendLayout();
            pCard1.SuspendLayout();
            pMonthlyChart.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.BackColor = Color.FromArgb(240, 244, 248);
            panel1.Controls.Add(pEnvironmentChart);
            //panel1.Controls.Add(pChartRow2);
            panel1.Controls.Add(pOverdueChart);
            panel1.Controls.Add(pStatsCards);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(30, 20, 30, 20);
            panel1.Size = new Size(1182, 674);
            panel1.TabIndex = 0;
            // 
            // pEnvironmentChart
            // 
            pEnvironmentChart.BackColor = Color.White;
            pEnvironmentChart.Controls.Add(pEnvironmentContent);
            pEnvironmentChart.Controls.Add(lEnvironmentTitle);
            pEnvironmentChart.Dock = DockStyle.Top;
            pEnvironmentChart.Location = new Point(30, 1060);
            pEnvironmentChart.Name = "pEnvironmentChart";
            pEnvironmentChart.Padding = new Padding(20);
            pEnvironmentChart.Size = new Size(1101, 1000);
            pEnvironmentChart.TabIndex = 3;
            // 
            // pEnvironmentContent
            // 
            pEnvironmentContent.BackColor = Color.FromArgb(248, 250, 252);
            pEnvironmentContent.Dock = DockStyle.Fill;
            pEnvironmentContent.Location = new Point(20, 62);
            pEnvironmentContent.Name = "pEnvironmentContent";
            pEnvironmentContent.Padding = new Padding(10);
            pEnvironmentContent.Size = new Size(1061, 918);
            pEnvironmentContent.TabIndex = 1;
            // 
            // lEnvironmentTitle
            // 
            lEnvironmentTitle.AutoSize = true;
            lEnvironmentTitle.Dock = DockStyle.Top;
            lEnvironmentTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lEnvironmentTitle.ForeColor = Color.FromArgb(51, 65, 85);
            lEnvironmentTitle.Location = new Point(20, 20);
            lEnvironmentTitle.Name = "lEnvironmentTitle";
            lEnvironmentTitle.Padding = new Padding(0, 0, 0, 10);
            lEnvironmentTitle.Size = new Size(662, 42);
            lEnvironmentTitle.TabIndex = 0;
            lEnvironmentTitle.Text = "🌍 Bản đồ nhiệt dự đoán tình trạng ô nhiễm môi trường";
            // 
            // pOverdueChart
            // 
            pOverdueChart.BackColor = Color.White;
            pOverdueChart.Controls.Add(pOverdueContent);
            pOverdueChart.Controls.Add(lOverdueTitle);
            pOverdueChart.Dock = DockStyle.Top;
            pOverdueChart.Location = new Point(0, 0);
            pOverdueChart.Name = "pOverdueChart";
            pOverdueChart.Padding = new Padding(20);
            pOverdueChart.Size = new Size(1101, 450);
            pOverdueChart.TabIndex = 0;
            pOverdueChart.Height = 450;
            // 
            // pOverdueContent
            // 
            pOverdueContent.BackColor = Color.FromArgb(248, 250, 252);
            pOverdueContent.Dock = DockStyle.Fill;
            pOverdueContent.Location = new Point(20, 60);
            pOverdueContent.Name = "pOverdueContent";
            pOverdueContent.Padding = new Padding(10);
            pOverdueContent.Size = new Size(1061, 370);
            pOverdueContent.TabIndex = 1;
            // 
            // lOverdueTitle
            // 
            lOverdueTitle.AutoSize = true;
            lOverdueTitle.Dock = DockStyle.Top;
            lOverdueTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lOverdueTitle.ForeColor = Color.FromArgb(51, 65, 85);
            lOverdueTitle.Location = new Point(20, 20);
            lOverdueTitle.Name = "lOverdueTitle";
            lOverdueTitle.Padding = new Padding(0, 0, 0, 10);
            lOverdueTitle.Size = new Size(367, 40);
            lOverdueTitle.TabIndex = 0;
            lOverdueTitle.Text = "📋 Khối lượng đơn hàng theo quý";
            // 
            // pStatsCards
            // 
            pStatsCards.Controls.Add(cbYearFilter);
            pStatsCards.Controls.Add(pCard4);
            pStatsCards.Controls.Add(pCard3);
            pStatsCards.Controls.Add(pCard2);
            pStatsCards.Controls.Add(pCard1);
            pStatsCards.Dock = DockStyle.Top;
            pStatsCards.Location = new Point(30, 20);
            pStatsCards.Name = "pStatsCards";
            pStatsCards.Padding = new Padding(0, 0, 0, 20);
            pStatsCards.Size = new Size(1101, 200);
            pStatsCards.TabIndex = 0;
            // 
            // cbYearFilter
            // 
            cbYearFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbYearFilter.BorderColor = Color.Gray;
            cbYearFilter.BorderRadius = 10;
            cbYearFilter.BorderSize = 1;
            cbYearFilter.DataSource = null;
            cbYearFilter.DisplayMember = "";
            cbYearFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cbYearFilter.IsReadOnly = false;
            cbYearFilter.Location = new Point(985, 0);
            cbYearFilter.Name = "cbYearFilter";
            cbYearFilter.SelectedIndex = -1;
            cbYearFilter.SelectedItem = null;
            cbYearFilter.SelectedValue = null;
            cbYearFilter.Size = new Size(96, 34);
            cbYearFilter.TabIndex = 6;
            cbYearFilter.ValueMember = "";
            // 
            // pCard4
            // 
            pCard4.BackColor = Color.White;
            pCard4.Controls.Add(lCard4Title);
            pCard4.Controls.Add(lCard4Value);
            pCard4.Controls.Add(lCard4Icon);
            pCard4.Location = new Point(818, 60);
            pCard4.Name = "pCard4";
            pCard4.Padding = new Padding(20, 15, 20, 15);
            pCard4.Size = new Size(260, 140);
            pCard4.TabIndex = 3;
            // 
            // lCard4Title
            // 
            lCard4Title.AutoSize = true;
            lCard4Title.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lCard4Title.ForeColor = Color.FromArgb(100, 116, 139);
            lCard4Title.Location = new Point(85, 85);
            lCard4Title.Name = "lCard4Title";
            lCard4Title.Size = new Size(129, 23);
            lCard4Title.TabIndex = 2;
            lCard4Title.Text = "Tỷ lệ đúng hạn";
            // 
            // lCard4Value
            // 
            lCard4Value.AutoSize = true;
            lCard4Value.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lCard4Value.ForeColor = Color.FromArgb(59, 130, 246);
            lCard4Value.Location = new Point(85, 20);
            lCard4Value.Name = "lCard4Value";
            lCard4Value.Size = new Size(113, 60);
            lCard4Value.TabIndex = 1;
            lCard4Value.Text = "95%";
            // 
            // lCard4Icon
            // 
            lCard4Icon.AutoSize = true;
            lCard4Icon.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lCard4Icon.ForeColor = Color.FromArgb(59, 130, 246);
            lCard4Icon.Location = new Point(20, 30);
            lCard4Icon.Name = "lCard4Icon";
            lCard4Icon.Size = new Size(79, 54);
            lCard4Icon.TabIndex = 0;
            lCard4Icon.Text = "✅";
            // 
            // pCard3
            // 
            pCard3.BackColor = Color.White;
            pCard3.Controls.Add(lCard3Title);
            pCard3.Controls.Add(lCard3Value);
            pCard3.Controls.Add(lCard3Icon);
            pCard3.Location = new Point(544, 60);
            pCard3.Name = "pCard3";
            pCard3.Padding = new Padding(20, 15, 20, 15);
            pCard3.Size = new Size(260, 140);
            pCard3.TabIndex = 2;
            // 
            // lCard3Title
            // 
            lCard3Title.AutoSize = true;
            lCard3Title.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lCard3Title.ForeColor = Color.FromArgb(100, 116, 139);
            lCard3Title.Location = new Point(85, 85);
            lCard3Title.Name = "lCard3Title";
            lCard3Title.Size = new Size(97, 23);
            lCard3Title.TabIndex = 2;
            lCard3Title.Text = "Đang xử lý";
            // 
            // lCard3Value
            // 
            lCard3Value.AutoSize = true;
            lCard3Value.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lCard3Value.ForeColor = Color.FromArgb(234, 179, 8);
            lCard3Value.Location = new Point(85, 20);
            lCard3Value.Name = "lCard3Value";
            lCard3Value.Size = new Size(75, 60);
            lCard3Value.TabIndex = 1;
            lCard3Value.Text = "12";
            // 
            // lCard3Icon
            // 
            lCard3Icon.AutoSize = true;
            lCard3Icon.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lCard3Icon.ForeColor = Color.FromArgb(234, 179, 8);
            lCard3Icon.Location = new Point(20, 30);
            lCard3Icon.Name = "lCard3Icon";
            lCard3Icon.Size = new Size(79, 54);
            lCard3Icon.TabIndex = 0;
            lCard3Icon.Text = "⏳";
            // 
            // pCard2
            // 
            pCard2.BackColor = Color.White;
            pCard2.Controls.Add(lCard2Title);
            pCard2.Controls.Add(lCard2Value);
            pCard2.Controls.Add(lCard2Icon);
            pCard2.Location = new Point(269, 60);
            pCard2.Name = "pCard2";
            pCard2.Padding = new Padding(20, 15, 20, 15);
            pCard2.Size = new Size(260, 140);
            pCard2.TabIndex = 1;
            // 
            // lCard2Title
            // 
            lCard2Title.AutoSize = true;
            lCard2Title.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lCard2Title.ForeColor = Color.FromArgb(100, 116, 139);
            lCard2Title.Location = new Point(85, 85);
            lCard2Title.Name = "lCard2Title";
            lCard2Title.Size = new Size(76, 23);
            lCard2Title.TabIndex = 2;
            lCard2Title.Text = "Quá hạn";
            // 
            // lCard2Value
            // 
            lCard2Value.AutoSize = true;
            lCard2Value.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lCard2Value.ForeColor = Color.FromArgb(239, 68, 68);
            lCard2Value.Location = new Point(85, 20);
            lCard2Value.Name = "lCard2Value";
            lCard2Value.Size = new Size(50, 60);
            lCard2Value.TabIndex = 1;
            lCard2Value.Text = "3";
            // 
            // lCard2Icon
            // 
            lCard2Icon.AutoSize = true;
            lCard2Icon.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lCard2Icon.ForeColor = Color.FromArgb(239, 68, 68);
            lCard2Icon.Location = new Point(20, 30);
            lCard2Icon.Name = "lCard2Icon";
            lCard2Icon.Size = new Size(79, 54);
            lCard2Icon.TabIndex = 0;
            lCard2Icon.Text = "⚠️";
            // 
            // pCard1
            // 
            pCard1.BackColor = Color.White;
            pCard1.Controls.Add(lCard1Title);
            pCard1.Controls.Add(lCard1Value);
            pCard1.Controls.Add(lCard1Icon);
            pCard1.Location = new Point(0, 60);
            pCard1.Margin = new Padding(0, 0, 15, 0);
            pCard1.Name = "pCard1";
            pCard1.Padding = new Padding(20, 15, 20, 15);
            pCard1.Size = new Size(260, 140);
            pCard1.TabIndex = 0;
            // 
            // lCard1Title
            // 
            lCard1Title.AutoSize = true;
            lCard1Title.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lCard1Title.ForeColor = Color.FromArgb(100, 116, 139);
            lCard1Title.Location = new Point(85, 85);
            lCard1Title.Name = "lCard1Title";
            lCard1Title.Size = new Size(135, 23);
            lCard1Title.TabIndex = 2;
            lCard1Title.Text = "Tổng hợp đồng";
            // 
            // lCard1Value
            // 
            lCard1Value.AutoSize = true;
            lCard1Value.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lCard1Value.ForeColor = Color.FromArgb(34, 197, 94);
            lCard1Value.Location = new Point(85, 20);
            lCard1Value.Name = "lCard1Value";
            lCard1Value.Size = new Size(100, 60);
            lCard1Value.TabIndex = 1;
            lCard1Value.Text = "156";
            // 
            // lCard1Icon
            // 
            lCard1Icon.AutoSize = true;
            lCard1Icon.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lCard1Icon.ForeColor = Color.FromArgb(34, 197, 94);
            lCard1Icon.Location = new Point(20, 30);
            lCard1Icon.Name = "lCard1Icon";
            lCard1Icon.Size = new Size(79, 54);
            lCard1Icon.TabIndex = 0;
            lCard1Icon.Text = "📋";
            // 
            // pMonthlyChart
            // 
            pMonthlyChart.BackColor = Color.White;
            pMonthlyChart.Controls.Add(pMonthlyContent);
            pMonthlyChart.Controls.Add(lMonthlyTitle);
            pMonthlyChart.Dock = DockStyle.Right;
            pMonthlyChart.Location = new Point(561, 450);
            pMonthlyChart.Name = "pMonthlyChart";
            pMonthlyChart.Padding = new Padding(20);
            pMonthlyChart.Size = new Size(540, 0);
            pMonthlyChart.TabIndex = 1;
            // 
            // pMonthlyContent
            // 
            pMonthlyContent.BackColor = Color.FromArgb(248, 250, 252);
            pMonthlyContent.Dock = DockStyle.Fill;
            pMonthlyContent.Location = new Point(20, 60);
            pMonthlyContent.Name = "pMonthlyContent";
            pMonthlyContent.Padding = new Padding(10);
            pMonthlyContent.Size = new Size(500, -80);
            pMonthlyContent.TabIndex = 1;
            // 
            // lMonthlyTitle
            // 
            lMonthlyTitle.AutoSize = true;
            lMonthlyTitle.Dock = DockStyle.Top;
            lMonthlyTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lMonthlyTitle.ForeColor = Color.FromArgb(51, 65, 85);
            lMonthlyTitle.Location = new Point(20, 20);
            lMonthlyTitle.Name = "lMonthlyTitle";
            lMonthlyTitle.Padding = new Padding(0, 0, 0, 10);
            lMonthlyTitle.Size = new Size(415, 40);
            lMonthlyTitle.TabIndex = 0;
            lMonthlyTitle.Text = "📊 Số lượng mẫu theo loại môi trường";
            // 
            // RatingChartControl
            // 
            BackColor = Color.White;
            Controls.Add(panel1);
            Name = "RatingChartControl";
            Size = new Size(1182, 674);
            panel1.ResumeLayout(false);
            pEnvironmentChart.ResumeLayout(false);
            pEnvironmentChart.PerformLayout();
            pChartRow1.ResumeLayout(false);
            pOverdueChart.ResumeLayout(false);
            pOverdueChart.PerformLayout();
            pStatsCards.ResumeLayout(false);
            pCard4.ResumeLayout(false);
            pCard4.PerformLayout();
            pCard3.ResumeLayout(false);
            pCard3.PerformLayout();
            pCard2.ResumeLayout(false);
            pCard2.PerformLayout();
            pCard1.ResumeLayout(false);
            pCard1.PerformLayout();
            pMonthlyChart.ResumeLayout(false);
            pMonthlyChart.PerformLayout();
            ResumeLayout(false);
        }

        #region Component Declarations
        private GoogleStyleScrollPanel panel1;
        private RoundedComboBox cbYearFilter;

        // Stats Cards
        private Panel pStatsCards;
        private Panel pCard1;
        private Label lCard1Icon;
        private Label lCard1Value;
        private Label lCard1Title;
        private Panel pCard2;
        private Label lCard2Icon;
        private Label lCard2Value;
        private Label lCard2Title;
        private Panel pCard3;
        private Label lCard3Icon;
        private Label lCard3Value;
        private Label lCard3Title;
        private Panel pCard4;
        private Label lCard4Icon;
        private Label lCard4Value;
        private Label lCard4Title;

        // Chart Row 1
        private Panel pChartRow1;
        private Panel pOverdueChart;
        private Label lOverdueTitle;
        private Panel pOverdueContent;
        private Panel pMonthlyChart;
        private Label lMonthlyTitle;
        private Panel pMonthlyContent;

        // Environment Chart (Full Width)
        private Panel pEnvironmentChart;
        private Label lEnvironmentTitle;
        private Panel pEnvironmentContent;
        #endregion
    }
}