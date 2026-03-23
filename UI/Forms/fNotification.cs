using EMC.DTO;
using EMC.Service;
using EMC.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EMC.UI.Forms
{

    public partial class fNotification : Form
    {
        private readonly int staffId;   // NEW: truyền vào form
        private string currentFilter = "Tất cả";
        private bool renderAsUnread = true;  // mặc định: ép hiển thị chưa đọc khi load
        private System.Windows.Forms.Timer autoEmailTimer;


        public fNotification(int staffId)
        {
            this.staffId = staffId;

            InitializeComponent();
        }
        public fNotification() : this(ResolveStaffIdFromConfig()) { }
        private static int ResolveStaffIdFromConfig()
        {
            try
            {
                var raw = System.Configuration.ConfigurationManager.AppSettings["CurrentStaffId"];
                if (int.TryParse(raw, out var id) && id > 0) return id;
            }
            catch { }
            return 1;
        }
    }
}
