namespace SIBANTUAN.Forms.Penerima
{
    partial class StatusPermohonan
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRiwayatBantuan = new System.Windows.Forms.Button();
            this.btnStatusPermohonan = new System.Windows.Forms.Button();
            this.btnAjukanPermohonan = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblNIK = new System.Windows.Forms.Label();
            this.dgvStatus = new System.Windows.Forms.DataGridView();
            this.lblDetail = new System.Windows.Forms.Label();
            this.btnAjukanPenyaluran = new System.Windows.Forms.Button();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.MenuBar;
            this.panel4.Controls.Add(this.label1);
            this.panel4.Location = new System.Drawing.Point(1, 1);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1104, 46);
            this.panel4.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(504, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "SIBANTUAN - Status Permohonan";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnRiwayatBantuan);
            this.panel1.Controls.Add(this.btnStatusPermohonan);
            this.panel1.Controls.Add(this.btnAjukanPermohonan);
            this.panel1.Controls.Add(this.btnDashboard);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(1, 46);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(228, 466);
            this.panel1.TabIndex = 16;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClose.Location = new System.Drawing.Point(28, 418);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 28);
            this.btnClose.TabIndex = 25;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRiwayatBantuan
            // 
            this.btnRiwayatBantuan.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRiwayatBantuan.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRiwayatBantuan.Location = new System.Drawing.Point(28, 251);
            this.btnRiwayatBantuan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRiwayatBantuan.Name = "btnRiwayatBantuan";
            this.btnRiwayatBantuan.Size = new System.Drawing.Size(175, 28);
            this.btnRiwayatBantuan.TabIndex = 23;
            this.btnRiwayatBantuan.Text = "Riwayat Bantuan";
            this.btnRiwayatBantuan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRiwayatBantuan.UseVisualStyleBackColor = true;
            this.btnRiwayatBantuan.Click += new System.EventHandler(this.btnRiwayatBantuan_Click);
            // 
            // btnStatusPermohonan
            // 
            this.btnStatusPermohonan.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStatusPermohonan.ForeColor = System.Drawing.Color.Indigo;
            this.btnStatusPermohonan.Location = new System.Drawing.Point(28, 197);
            this.btnStatusPermohonan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStatusPermohonan.Name = "btnStatusPermohonan";
            this.btnStatusPermohonan.Size = new System.Drawing.Size(175, 28);
            this.btnStatusPermohonan.TabIndex = 22;
            this.btnStatusPermohonan.Text = "Status Permohonan";
            this.btnStatusPermohonan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStatusPermohonan.UseVisualStyleBackColor = true;
            this.btnStatusPermohonan.Click += new System.EventHandler(this.btnStatusPermohonan_Click);
            // 
            // btnAjukanPermohonan
            // 
            this.btnAjukanPermohonan.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAjukanPermohonan.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAjukanPermohonan.Location = new System.Drawing.Point(28, 142);
            this.btnAjukanPermohonan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAjukanPermohonan.Name = "btnAjukanPermohonan";
            this.btnAjukanPermohonan.Size = new System.Drawing.Size(175, 28);
            this.btnAjukanPermohonan.TabIndex = 21;
            this.btnAjukanPermohonan.Text = "Ajukan Permohonan";
            this.btnAjukanPermohonan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAjukanPermohonan.UseVisualStyleBackColor = true;
            this.btnAjukanPermohonan.Click += new System.EventHandler(this.btnAjukanPermohonan_Click);
            // 
            // btnDashboard
            // 
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDashboard.Location = new System.Drawing.Point(28, 89);
            this.btnDashboard.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(175, 28);
            this.btnDashboard.TabIndex = 20;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.UseVisualStyleBackColor = true;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label3.Location = new System.Drawing.Point(23, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Penerima Bantuan";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(23, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "SIBANTUAN";
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcome.Location = new System.Drawing.Point(265, 66);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(160, 23);
            this.lblWelcome.TabIndex = 18;
            this.lblWelcome.Text = "Status Permohonan";
            // 
            // lblNIK
            // 
            this.lblNIK.AutoSize = true;
            this.lblNIK.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNIK.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblNIK.Location = new System.Drawing.Point(265, 88);
            this.lblNIK.Name = "lblNIK";
            this.lblNIK.Size = new System.Drawing.Size(302, 20);
            this.lblNIK.TabIndex = 17;
            this.lblNIK.Text = "Semua permohonan yang pernah diajukan\r\n";
            // 
            // dgvStatus
            // 
            this.dgvStatus.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatus.Location = new System.Drawing.Point(269, 134);
            this.dgvStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvStatus.Name = "dgvStatus";
            this.dgvStatus.RowHeadersWidth = 62;
            this.dgvStatus.RowTemplate.Height = 28;
            this.dgvStatus.Size = new System.Drawing.Size(805, 279);
            this.dgvStatus.TabIndex = 19;
            // 
            // lblDetail
            // 
            this.lblDetail.BackColor = System.Drawing.Color.MistyRose;
            this.lblDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetail.ForeColor = System.Drawing.Color.DarkRed;
            this.lblDetail.Location = new System.Drawing.Point(266, 427);
            this.lblDetail.Name = "lblDetail";
            this.lblDetail.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.lblDetail.Size = new System.Drawing.Size(809, 48);
            this.lblDetail.TabIndex = 20;
            this.lblDetail.Text = "! Alasan ditolak: data ekonomi tidak sesuai kriteria program ini.";
            this.lblDetail.Visible = false;
            // 
            // btnAjukanPenyaluran
            // 
            this.btnAjukanPenyaluran.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnAjukanPenyaluran.FlatAppearance.BorderSize = 0;
            this.btnAjukanPenyaluran.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAjukanPenyaluran.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAjukanPenyaluran.ForeColor = System.Drawing.Color.White;
            this.btnAjukanPenyaluran.Location = new System.Drawing.Point(266, 478);
            this.btnAjukanPenyaluran.Name = "btnAjukanPenyaluran";
            this.btnAjukanPenyaluran.Size = new System.Drawing.Size(180, 32);
            this.btnAjukanPenyaluran.TabIndex = 28;
            this.btnAjukanPenyaluran.Text = "Ajukan Penyaluran";
            this.btnAjukanPenyaluran.UseVisualStyleBackColor = false;
            this.btnAjukanPenyaluran.Visible = false;
            this.btnAjukanPenyaluran.Click += new System.EventHandler(this.BtnAjukanPenyaluran_Click);
            // 
            // StatusPermohonan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1103, 540);
            this.Controls.Add(this.btnAjukanPenyaluran);
            this.Controls.Add(this.lblDetail);
            this.Controls.Add(this.dgvStatus);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.lblNIK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "StatusPermohonan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SIBANTUAN - Status Permohonan";
            this.Load += new System.EventHandler(this.StatusPermohonan_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRiwayatBantuan;
        private System.Windows.Forms.Button btnStatusPermohonan;
        private System.Windows.Forms.Button btnAjukanPermohonan;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblNIK;
        private System.Windows.Forms.DataGridView dgvStatus;
        private System.Windows.Forms.Label lblDetail;
        private System.Windows.Forms.Button btnAjukanPenyaluran;
        private System.Windows.Forms.Button btnClose;
    }
}