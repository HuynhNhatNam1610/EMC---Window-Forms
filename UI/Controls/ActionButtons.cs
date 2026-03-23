using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EMC.UI.Controls
{
    public partial class ActionButtons : UserControl
    {
        public event EventHandler EyeClick;
        public event EventHandler EditClick;
        public event EventHandler DeleteClick;

        public ActionButtons()
        {
            InitializeComponent();
        }

        private void rbtnEye_Click(object sender, EventArgs e) => EyeClick?.Invoke(this, e);

        private void rbtnPencil_Click(object sender, EventArgs e) => EditClick?.Invoke(this, e);

        private void rbtnTrash_Click(object sender, EventArgs e) => DeleteClick?.Invoke(this, e);
    }
}
