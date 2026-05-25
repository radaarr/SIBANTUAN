namespace SIBANTUAN.Forms.Petugas
{
    partial class FormPetugas
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_nik = new System.Windows.Forms.TextBox();
            this.tb_nama = new System.Windows.Forms.TextBox();
            this.tb_alamat = new System.Windows.Forms.TextBox();
            this.tb_rt = new System.Windows.Forms.TextBox();
            this.tb_rw = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.bt_tambah = new System.Windows.Forms.Button();
            this.bt_edit = new System.Windows.Forms.Button();
            this.bt_hapus = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.NIK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nama_Warga = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Alamat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RTRW = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tgl_lahir = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status_ekonomi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.bt_data = new System.Windows.Forms.Button();
            this.bt_verif = new System.Windows.Forms.Button();
            this.bt_catat = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(29, 242);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "NIK :";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 277);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = " Nama :";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 311);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = " Alamat :";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(29, 345);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "RT/RW :";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(29, 376);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Tgl Lahir :";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // tb_nik
            // 
            this.tb_nik.Font = new System.Drawing.Font("Microsoft PhagsPa", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_nik.Location = new System.Drawing.Point(143, 238);
            this.tb_nik.Name = "tb_nik";
            this.tb_nik.Size = new System.Drawing.Size(225, 29);
            this.tb_nik.TabIndex = 5;
            this.tb_nik.TextChanged += new System.EventHandler(this.tb_nik_TextChanged);
            // 
            // tb_nama
            // 
            this.tb_nama.Font = new System.Drawing.Font("Microsoft PhagsPa", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_nama.Location = new System.Drawing.Point(143, 275);
            this.tb_nama.Name = "tb_nama";
            this.tb_nama.Size = new System.Drawing.Size(225, 29);
            this.tb_nama.TabIndex = 6;
            this.tb_nama.TextChanged += new System.EventHandler(this.tb_nama_TextChanged);
            // 
            // tb_alamat
            // 
            this.tb_alamat.Font = new System.Drawing.Font("Microsoft PhagsPa", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_alamat.Location = new System.Drawing.Point(143, 308);
            this.tb_alamat.Name = "tb_alamat";
            this.tb_alamat.Size = new System.Drawing.Size(225, 29);
            this.tb_alamat.TabIndex = 7;
            this.tb_alamat.TextChanged += new System.EventHandler(this.tb_alamat_TextChanged);
            // 
            // tb_rt
            // 
            this.tb_rt.Font = new System.Drawing.Font("Microsoft PhagsPa", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_rt.Location = new System.Drawing.Point(143, 342);
            this.tb_rt.Name = "tb_rt";
            this.tb_rt.Size = new System.Drawing.Size(41, 29);
            this.tb_rt.TabIndex = 8;
            this.tb_rt.TextChanged += new System.EventHandler(this.tb_rt_TextChanged);
            // 
            // tb_rw
            // 
            this.tb_rw.Font = new System.Drawing.Font("Microsoft PhagsPa", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_rw.Location = new System.Drawing.Point(198, 343);
            this.tb_rw.Name = "tb_rw";
            this.tb_rw.Size = new System.Drawing.Size(41, 29);
            this.tb_rw.TabIndex = 9;
            this.tb_rw.TextChanged += new System.EventHandler(this.tb_rw_TextChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(579, 238);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(169, 28);
            this.comboBox1.TabIndex = 14;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(428, 240);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "Status Ekonomi :";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // bt_tambah
            // 
            this.bt_tambah.Location = new System.Drawing.Point(432, 309);
            this.bt_tambah.Name = "bt_tambah";
            this.bt_tambah.Size = new System.Drawing.Size(95, 31);
            this.bt_tambah.TabIndex = 16;
            this.bt_tambah.Text = "Tambah";
            this.bt_tambah.UseVisualStyleBackColor = true;
            this.bt_tambah.Click += new System.EventHandler(this.bt_tambah_Click);
            // 
            // bt_edit
            // 
            this.bt_edit.Location = new System.Drawing.Point(544, 309);
            this.bt_edit.Name = "bt_edit";
            this.bt_edit.Size = new System.Drawing.Size(95, 31);
            this.bt_edit.TabIndex = 17;
            this.bt_edit.Text = "Edit";
            this.bt_edit.UseVisualStyleBackColor = true;
            this.bt_edit.Click += new System.EventHandler(this.bt_edit_Click);
            // 
            // bt_hapus
            // 
            this.bt_hapus.Location = new System.Drawing.Point(653, 309);
            this.bt_hapus.Name = "bt_hapus";
            this.bt_hapus.Size = new System.Drawing.Size(95, 31);
            this.bt_hapus.TabIndex = 18;
            this.bt_hapus.Text = "Hapus";
            this.bt_hapus.UseVisualStyleBackColor = true;
            this.bt_hapus.Click += new System.EventHandler(this.bt_hapus_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NIK,
            this.Nama_Warga,
            this.Alamat,
            this.RTRW,
            this.tgl_lahir,
            this.status_ekonomi});
            this.dataGridView1.Location = new System.Drawing.Point(33, 423);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(715, 143);
            this.dataGridView1.TabIndex = 19;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // NIK
            // 
            this.NIK.HeaderText = "NIK";
            this.NIK.MinimumWidth = 6;
            this.NIK.Name = "NIK";
            this.NIK.Width = 125;
            // 
            // Nama_Warga
            // 
            this.Nama_Warga.HeaderText = "Nama Warga";
            this.Nama_Warga.MinimumWidth = 6;
            this.Nama_Warga.Name = "Nama_Warga";
            this.Nama_Warga.Width = 125;
            // 
            // Alamat
            // 
            this.Alamat.HeaderText = "Alamat";
            this.Alamat.MinimumWidth = 6;
            this.Alamat.Name = "Alamat";
            this.Alamat.Width = 125;
            // 
            // RTRW
            // 
            this.RTRW.HeaderText = "RT/RW";
            this.RTRW.MinimumWidth = 6;
            this.RTRW.Name = "RTRW";
            this.RTRW.Width = 70;
            // 
            // tgl_lahir
            // 
            this.tgl_lahir.HeaderText = "tgl_lahir";
            this.tgl_lahir.MinimumWidth = 6;
            this.tgl_lahir.Name = "tgl_lahir";
            this.tgl_lahir.Width = 70;
            // 
            // status_ekonomi
            // 
            this.status_ekonomi.HeaderText = "status_ekonomi";
            this.status_ekonomi.MinimumWidth = 6;
            this.status_ekonomi.Name = "status_ekonomi";
            this.status_ekonomi.Width = 125;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Location = new System.Drawing.Point(-12, -3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(817, 106);
            this.panel1.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("MS Reference Sans Serif", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(315, 46);
            this.label7.TabIndex = 2;
            this.label7.Text = "SIBANTUAN";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.label7.Click += new System.EventHandler(this.label7_Click);
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
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(143, 382);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(269, 22);
            this.dateTimePicker1.TabIndex = 23;
            // 
            // bt_data
            // 
            this.bt_data.Font = new System.Drawing.Font("Mongolian Baiti", 13.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_data.Location = new System.Drawing.Point(63, 124);
            this.bt_data.Name = "bt_data";
            this.bt_data.Size = new System.Drawing.Size(176, 60);
            this.bt_data.TabIndex = 11;
            this.bt_data.Text = "Data Warga";
            this.bt_data.UseVisualStyleBackColor = true;
            this.bt_data.Click += new System.EventHandler(this.bt_data_Click);
            // 
            // bt_verif
            // 
            this.bt_verif.Font = new System.Drawing.Font("Mongolian Baiti", 13.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_verif.Location = new System.Drawing.Point(296, 124);
            this.bt_verif.Name = "bt_verif";
            this.bt_verif.Size = new System.Drawing.Size(176, 60);
            this.bt_verif.TabIndex = 21;
            this.bt_verif.Text = "Verifikasi Permohonan";
            this.bt_verif.UseVisualStyleBackColor = true;
            this.bt_verif.Click += new System.EventHandler(this.bt_verifikasi_Click);
            // 
            // bt_catat
            // 
            this.bt_catat.Font = new System.Drawing.Font("Mongolian Baiti", 13.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_catat.Location = new System.Drawing.Point(528, 124);
            this.bt_catat.Name = "bt_catat";
            this.bt_catat.Size = new System.Drawing.Size(176, 60);
            this.bt_catat.TabIndex = 22;
            this.bt_catat.Text = "Catat Penyaluran";
            this.bt_catat.UseVisualStyleBackColor = true;
            this.bt_catat.Click += new System.EventHandler(this.bt_catat_Click);
            // 
            // FormPetugas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(800, 670);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.bt_catat);
            this.Controls.Add(this.bt_verif);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.bt_hapus);
            this.Controls.Add(this.bt_edit);
            this.Controls.Add(this.bt_tambah);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.bt_data);
            this.Controls.Add(this.tb_rw);
            this.Controls.Add(this.tb_rt);
            this.Controls.Add(this.tb_alamat);
            this.Controls.Add(this.tb_nama);
            this.Controls.Add(this.tb_nik);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormPetugas";
            this.Load += new System.EventHandler(this.FormPetugas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_nik;
        private System.Windows.Forms.TextBox tb_nama;
        private System.Windows.Forms.TextBox tb_alamat;
        private System.Windows.Forms.TextBox tb_rt;
        private System.Windows.Forms.TextBox tb_rw;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button bt_tambah;
        private System.Windows.Forms.Button bt_edit;
        private System.Windows.Forms.Button bt_hapus;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn NIK;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nama_Warga;
        private System.Windows.Forms.DataGridViewTextBoxColumn Alamat;
        private System.Windows.Forms.DataGridViewTextBoxColumn RTRW;
        private System.Windows.Forms.DataGridViewTextBoxColumn tgl_lahir;
        private System.Windows.Forms.DataGridViewTextBoxColumn status_ekonomi;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button bt_data;
        private System.Windows.Forms.Button bt_verif;
        private System.Windows.Forms.Button bt_catat;
    }
}