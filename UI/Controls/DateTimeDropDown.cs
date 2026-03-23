// File: Controls/DateTimeDropDown.cs
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EMC.UI.Controls
{
    public class DateTimeDropDown : UserControl
    {
        private MonthCalendar cal;
        private NumericUpDown nudHour, nudMinute;
        private ComboBox cboAmPm;
        private Button btnOk, btnCancel;

        public DateTime SelectedDateTime { get; private set; }

        public DateTimeDropDown(DateTime seed)
        {
            this.AutoSize = true;
            this.Padding = new Padding(8);

            cal = new MonthCalendar { MaxSelectionCount = 1 };
            cal.SetDate(seed);

            nudHour = new NumericUpDown { Minimum = 1, Maximum = 12, Value = (seed.Hour % 12 == 0 ? 12 : seed.Hour % 12), Width = 50 };
            nudMinute = new NumericUpDown { Minimum = 0, Maximum = 59, Value = seed.Minute, Width = 60, Increment = 1 };
            cboAmPm = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 60 };
            cboAmPm.Items.AddRange(new[] { "AM", "PM" });
            cboAmPm.SelectedIndex = seed.Hour >= 12 ? 1 : 0;

            btnOk = new Button { Text = "Chọn", AutoSize = true };
            btnCancel = new Button { Text = "Hủy", AutoSize = true };

            var timeRow = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true, Margin = new Padding(0, 6, 0, 6) };
            timeRow.Controls.Add(new Label { Text = "Giờ:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(0, 8, 4, 0) });
            timeRow.Controls.Add(nudHour);
            timeRow.Controls.Add(new Label { Text = ":", AutoSize = true, Margin = new Padding(4, 8, 4, 0) });
            timeRow.Controls.Add(nudMinute);
            timeRow.Controls.Add(new Label { Text = " ", AutoSize = true, Margin = new Padding(4, 8, 4, 0) });
            timeRow.Controls.Add(cboAmPm);

            var buttons = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Bottom, AutoSize = true };
            buttons.Controls.Add(btnOk);
            buttons.Controls.Add(btnCancel);

            var layout = new TableLayoutPanel { AutoSize = true, ColumnCount = 1 };
            layout.Controls.Add(cal, 0, 0);
            layout.Controls.Add(timeRow, 0, 1);
            layout.Controls.Add(buttons, 0, 2);

            this.Controls.Add(layout);

            btnOk.Click += (s, e) =>
            {
                var d = cal.SelectionStart.Date;
                int hour12 = (int)nudHour.Value % 12;
                int hour24 = (cboAmPm.SelectedIndex == 1) ? (hour12 == 12 ? 12 : hour12 + 12) : (hour12 == 12 ? 0 : hour12);
                SelectedDateTime = new DateTime(d.Year, d.Month, d.Day, hour24, (int)nudMinute.Value, 0);
                var form = this.FindForm();
                if (form != null)
                {
                    form.DialogResult = DialogResult.OK;
                }
            };
            btnCancel.Click += (s, e) =>
            {
                var form = this.FindForm();
                if (form != null)
                {
                    form.Close();
                }
            };
        }
    }
}
