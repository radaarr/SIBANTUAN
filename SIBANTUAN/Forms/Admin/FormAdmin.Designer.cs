namespace SIBANTUAN.Forms
{
    partial class FormAdmin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLaporan = new System.Windows.Forms.Button();
            this.btnKelolaUser = new System.Windows.Forms.Button();
            this.btnProgramBantuan = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(95)))), ((int)(((byte)(138)))));
            this.panel1.Controls.Add(this.btnLaporan);
            this.panel1.Controls.Add(this.btnKelolaUser);
            this.panel1.Controls.Add(this.btnProgramBantuan);
            this.panel1.Controls.Add(this.btnDashboard);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 450);
            this.panel1.TabIndex = 0;
            // 
            // btnLaporan
            // 
            this.btnLaporan.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLaporan.FlatAppearance.BorderSize = 0;
            this.btnLaporan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLaporan.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLaporan.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnLaporan.Location = new System.Drawing.Point(0, 150);
            this.btnLaporan.Name = "btnLaporan";
            this.btnLaporan.Size = new System.Drawing.Size(250, 50);
            this.btnLaporan.TabIndex = 3;
            this.btnLaporan.Text = "Laporan dan Statistik";
            this.btnLaporan.UseVisualStyleBackColor = true;
            this.btnLaporan.Click += new System.EventHandler(this.btnLaporan_Click);
            // 
            // btnKelolaUser
            // 
            this.btnKelolaUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnKelolaUser.FlatAppearance.BorderSize = 0;
            this.btnKelolaUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKelolaUser.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKelolaUser.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnKelolaUser.Location = new System.Drawing.Point(0, 100);
            this.btnKelolaUser.Name = "btnKelolaUser";
            this.btnKelolaUser.Size = new System.Drawing.Size(250, 50);
            this.btnKelolaUser.TabIndex = 2;
            this.btnKelolaUser.Text = "Kelola User";
            this.btnKelolaUser.UseVisualStyleBackColor = true;
            this.btnKelolaUser.Click += new System.EventHandler(this.btnKelolaUser_Click);
            // 
            // btnProgramBantuan
            // 
            this.btnProgramBantuan.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnProgramBantuan.FlatAppearance.BorderSize = 0;
            this.btnProgramBantuan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProgramBantuan.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProgramBantuan.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnProgramBantuan.Location = new System.Drawing.Point(0, 50);
            this.btnProgramBantuan.Name = "btnProgramBantuan";
            this.btnProgramBantuan.Size = new System.Drawing.Size(250, 50);
            this.btnProgramBantuan.TabIndex = 1;
            this.btnProgramBantuan.Text = "Program Bantuan";
            this.btnProgramBantuan.UseVisualStyleBackColor = true;
            this.btnProgramBantuan.Click += new System.EventHandler(this.btnProgramBantuan_Click);
            // 
            // btnDashboard
            // 
            this.btnDashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnDashboard.Location = new System.Drawing.Point(0, 0);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(250, 50);
            this.btnDashboard.TabIndex = 0;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.UseVisualStyleBackColor = true;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(250, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(550, 60);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(250, 60);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(550, 390);
            this.panel3.TabIndex = 2;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // FormAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FormAdmin";
            this.Text = "FormAdmin";
            this.Load += new System.EventHandler(this.FormAdmin_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnLaporan;
        private System.Windows.Forms.Button btnKelolaUser;
        private System.Windows.Forms.Button btnProgramBantuan;
        private System.Windows.Forms.Button btnDashboard;
    }
}