namespace EMC.UI.Controls
{
    partial class ActionButtons
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.rbtnEye = new RoundedButton();
            this.rbtnPencil = new RoundedButton();
            this.rbtnTrash = new RoundedButton();
            this.SuspendLayout();
            // 
            // rbtnEye
            // 
            this.rbtnEye.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.rbtnEye.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnEye.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbtnEye.ForeColor = System.Drawing.Color.White;
            this.rbtnEye.Location = new System.Drawing.Point(3, 3);
            this.rbtnEye.Name = "rbtnEye";
            this.rbtnEye.Size = new System.Drawing.Size(35, 30);
            this.rbtnEye.TabIndex = 0;
            this.rbtnEye.Text = "👁";
            this.rbtnEye.UseVisualStyleBackColor = false;
            this.rbtnEye.Click += new System.EventHandler(this.rbtnEye_Click);
            // 
            // rbtnPencil
            // 
            this.rbtnPencil.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            this.rbtnPencil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnPencil.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbtnPencil.ForeColor = System.Drawing.Color.White;
            this.rbtnPencil.Location = new System.Drawing.Point(44, 3);
            this.rbtnPencil.Name = "rbtnPencil";
            this.rbtnPencil.Size = new System.Drawing.Size(35, 30);
            this.rbtnPencil.TabIndex = 1;
            this.rbtnPencil.Text = "✎";
            this.rbtnPencil.UseVisualStyleBackColor = false;
            this.rbtnPencil.Click += new System.EventHandler(this.rbtnPencil_Click);
            // 
            // rbtnTrash
            // 
            this.rbtnTrash.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.rbtnTrash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnTrash.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbtnTrash.ForeColor = System.Drawing.Color.White;
            this.rbtnTrash.Location = new System.Drawing.Point(85, 3);
            this.rbtnTrash.Name = "rbtnTrash";
            this.rbtnTrash.Size = new System.Drawing.Size(35, 30);
            this.rbtnTrash.TabIndex = 2;
            this.rbtnTrash.Text = "✖";
            this.rbtnTrash.UseVisualStyleBackColor = false;
            this.rbtnTrash.Click += new System.EventHandler(this.rbtnTrash_Click);
            // 
            // ActionButtons
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.rbtnEye);
            this.Controls.Add(this.rbtnPencil);
            this.Controls.Add(this.rbtnTrash);
            this.Name = "ActionButtons";
            this.Size = new System.Drawing.Size(125, 36);
            this.ResumeLayout(false);
        }

        private RoundedButton rbtnEye;
        private RoundedButton rbtnPencil;
        private RoundedButton rbtnTrash;
    }
}
