namespace SIBANTUAN.Forms.Petugas
{
    partial class FormVerifikasi
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
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_nik = new System.Windows.Forms.TextBox();
            this.tb_nama = new System.Windows.Forms.TextBox();
            this.tb_program = new System.Windows.Forms.TextBox();
            this.tb_catatan = new System.Windows.Forms.TextBox();
            this.rb_disetujui = new System.Windows.Forms.RadioButton();
            this.rb_ditolak = new System.Windows.Forms.RadioButton();
            this.tb_alasan = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.NIK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nama = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Program = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tanggal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bt_simpan = new System.Windows.Forms.Button();
            this.bt_batal = new System.Windows.Forms.Button();
            this.bt_refresh = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Location = new System.Drawing.Point(-12, -3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1100, 106);
            this.panel1.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("MS Reference Sans Serif", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(30, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(315, 46);
            this.label7.TabIndex = 2;
            this.label7.Text = "SIBANTUAN";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(41, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(309, 23);
            this.label8.TabIndex = 0;
            this.label8.Text = "Sistem Informasi Penyaluran Bantuan";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(33, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 15);
            this.label1.TabIndex = 21;
            this.label1.Text = "DAFTAR PERMOHONAN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(33, 300);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 20);
            this.label2.TabIndex = 22;
            this.label2.Text = "NIK :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(33, 335);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 20);
            this.label3.TabIndex = 23;
            this.label3.Text = " Nama :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(33, 370);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 20);
            this.label4.TabIndex = 24;
            this.label4.Text = " Program :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(33, 405);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 20);
            this.label5.TabIndex = 25;
            this.label5.Text = " Keputusan :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(33, 440);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 20);
            this.label6.TabIndex = 26;
            this.label6.Text = " Catatan :";
            // 
            // tb_nik
            // 
            this.tb_nik.Font = new System.Drawing.Font("Microsoft PhagsPa", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_nik.Location = new System.Drawing.Point(150, 300);
            this.tb_nik.Name = "tb_nik";
            this.tb_nik.ReadOnly = true;
            this.tb_nik.Size = new System.Drawing.Size(150, 24);
            this.tb_nik.TabIndex = 27;
            // 
            // tb_nama
            // 
            this.tb_nama.Font = new System.Drawing.Font("Microsoft PhagsPa", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_nama.Location = new System.Drawing.Point(150, 335);
            this.tb_nama.Name = "tb_nama";
            this.tb_nama.ReadOnly = true;
            this.tb_nama.Size = new System.Drawing.Size(150, 24);
            this.tb_nama.TabIndex = 28;
            // 
            // tb_program
            // 
            this.tb_program.Font = new System.Drawing.Font("Microsoft PhagsPa", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_program.Location = new System.Drawing.Point(150, 370);
            this.tb_program.Name = "tb_program";
            this.tb_program.ReadOnly = true;
            this.tb_program.Size = new System.Drawing.Size(150, 24);
            this.tb_program.TabIndex = 29;
            // 
            // tb_catatan
            // 
            this.tb_catatan.Font = new System.Drawing.Font("Microsoft PhagsPa", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_catatan.Location = new System.Drawing.Point(150, 440);
            this.tb_catatan.Multiline = true;
            this.tb_catatan.Name = "tb_catatan";
            this.tb_catatan.Size = new System.Drawing.Size(150, 50);
            this.tb_catatan.TabIndex = 30;
            // 
            // rb_disetujui
            // 
            this.rb_disetujui.AutoSize = true;
            this.rb_disetujui.Location = new System.Drawing.Point(150, 405);
            this.rb_disetujui.Name = "rb_disetujui";
            this.rb_disetujui.Size = new System.Drawing.Size(78, 20);
            this.rb_disetujui.TabIndex = 31;
            this.rb_disetujui.TabStop = true;
            this.rb_disetujui.Text = "Disetujui";
            this.rb_disetujui.UseVisualStyleBackColor = true;
            this.rb_disetujui.CheckedChanged += new System.EventHandler(this.rb_disetujui_CheckedChanged);
            // 
            // rb_ditolak
            // 
            this.rb_ditolak.AutoSize = true;
            this.rb_ditolak.Location = new System.Drawing.Point(234, 405);
            this.rb_ditolak.Name = "rb_ditolak";
            this.rb_ditolak.Size = new System.Drawing.Size(66, 20);
            this.rb_ditolak.TabIndex = 32;
            this.rb_ditolak.TabStop = true;
            this.rb_ditolak.Text = "Ditolak";
            this.rb_ditolak.UseVisualStyleBackColor = true;
            this.rb_ditolak.CheckedChanged += new System.EventHandler(this.rb_ditolak_CheckedChanged);
            // 
            // tb_alasan
            // 
            this.tb_alasan.Font = new System.Drawing.Font("Microsoft PhagsPa", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_alasan.Location = new System.Drawing.Point(405, 310);
            this.tb_alasan.Multiline = true;
            this.tb_alasan.Name = "tb_alasan";
            this.tb_alasan.Size = new System.Drawing.Size(180, 180);
            this.tb_alasan.TabIndex = 33;
            this.tb_alasan.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(335, 310);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 20);
            this.label9.TabIndex = 34;
            this.label9.Text = " Alasan :";
            this.label9.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(335, 135);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 15);
            this.label10.TabIndex = 35;
            this.label10.Text = "VERIFIKASI DATA";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NIK,
            this.Nama,
            this.Program,
            this.Tanggal,
            this.Status});
            this.dataGridView1.Location = new System.Drawing.Point(33, 160);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(291, 123);
            this.dataGridView1.TabIndex = 36;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // NIK
            // 
            this.NIK.HeaderText = "NIK";
            this.NIK.MinimumWidth = 6;
            this.NIK.Name = "NIK";
            this.NIK.Width = 65;
            // 
            // Nama
            // 
            this.Nama.HeaderText = "Nama";
            this.Nama.MinimumWidth = 6;
            this.Nama.Name = "Nama";
            this.Nama.Width = 65;
            // 
            // Program
            // 
            this.Program.HeaderText = "Program";
            this.Program.MinimumWidth = 6;
            this.Program.Name = "Program";
            this.Program.Width = 70;
            // 
            // Tanggal
            // 
            this.Tanggal.HeaderText = "Tanggal";
            this.Tanggal.MinimumWidth = 6;
            this.Tanggal.Name = "Tanggal";
            this.Tanggal.Width = 70;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 6;
            this.Status.Name = "Status";
            this.Status.Width = 70;
            // 
            // bt_simpan
            // 
            this.bt_simpan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_simpan.Location = new System.Drawing.Point(405, 500);
            this.bt_simpan.Name = "bt_simpan";
            this.bt_simpan.Size = new System.Drawing.Size(95, 35);
            this.bt_simpan.TabIndex = 37;
            this.bt_simpan.Text = "Simpan";
            this.bt_simpan.UseVisualStyleBackColor = true;
            this.bt_simpan.Click += new System.EventHandler(this.bt_simpan_Click);
            // 
            // bt_batal
            // 
            this.bt_batal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_batal.Location = new System.Drawing.Point(506, 500);
            this.bt_batal.Name = "bt_batal";
            this.bt_batal.Size = new System.Drawing.Size(79, 35);
            this.bt_batal.TabIndex = 38;
            this.bt_batal.Text = "Batal";
            this.bt_batal.UseVisualStyleBackColor = true;
            this.bt_batal.Click += new System.EventHandler(this.bt_batal_Click);
            // 
            // bt_refresh
            // 
            this.bt_refresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_refresh.Location = new System.Drawing.Point(193, 288);
            this.bt_refresh.Name = "bt_refresh";
            this.bt_refresh.Size = new System.Drawing.Size(70, 23);
            this.bt_refresh.TabIndex = 39;
            this.bt_refresh.Text = "Refresh";
            this.bt_refresh.UseVisualStyleBackColor = true;
            this.bt_refresh.Click += new System.EventHandler(this.bt_refresh_Click);
            // 
            // FormVerifikasi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(620, 550);
            this.Controls.Add(this.bt_refresh);
            this.Controls.Add(this.bt_batal);
            this.Controls.Add(this.bt_simpan);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tb_alasan);
            this.Controls.Add(this.rb_ditolak);
            this.Controls.Add(this.rb_disetujui);
            this.Controls.Add(this.tb_catatan);
            this.Controls.Add(this.tb_program);
            this.Controls.Add(this.tb_nama);
            this.Controls.Add(this.tb_nik);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Name = "FormVerifikasi";
            this.Text = "Verifikasi Permohonan Bantuan";
            this.Load += new System.EventHandler(this.FormVerifikasi_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_nik;
        private System.Windows.Forms.TextBox tb_nama;
        private System.Windows.Forms.TextBox tb_program;
        private System.Windows.Forms.TextBox tb_catatan;
        private System.Windows.Forms.RadioButton rb_disetujui;
        private System.Windows.Forms.RadioButton rb_ditolak;
        private System.Windows.Forms.TextBox tb_alasan;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn NIK;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nama;
        private System.Windows.Forms.DataGridViewTextBoxColumn Program;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tanggal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.Button bt_simpan;
        private System.Windows.Forms.Button bt_batal;
        private System.Windows.Forms.Button bt_refresh;
    }
}