using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SIBANTUAN.Forms.Petugas
{
    public partial class Distribusi : Form
    {
        private int currentDistribusiId = 0;
        private int currentPermohonanId = 0;

        private Panel panel2;
        private Button keluar_bt;
        private Button laporan_bt;
        private Button distribusi_bt;
        private Button permohonan_bt;
        private Button verifikasi_bt;
        private Button dashboard_bt;
        private Panel panel1;
        private Label label4;
        private Label label3;
        private Label label2;
        private Panel panel4;
        private DataGridView dataGridView1;
        private Label label9;
        private Panel panel3;
        private Label label8;
        private Button reset_bt;
        private Button cari_bt;
        private TextBox cari_tb;
        private Label label7;
        private ComboBox program_cmb;
        private Label label6;
        private Panel panel5;
        private Panel panel6;
        private TextBox keterangan_tb;
        private Label label20;
        private Button button3;
        private Button button1;
        private TextBox bentuk_bantuan_tb;
        private TextBox tgl_tb;
        private TextBox nik_tb;
        private TextBox bukti_tb;
        private TextBox jumlah_tb;
        private TextBox program_tb;
        private TextBox nama_tb;
        private Label label19;
        private Label label18;
        private Label label17;
        private Label label16;
        private Label label15;
        private Label label14;
        private Label label13;
        private Label label12;
        private Label label11;
        private ComboBox dicatat_cmb;
        private Label label1;
        private DataGridViewTextBoxColumn no;
        private DataGridViewTextBoxColumn nama_warga;
        private DataGridViewTextBoxColumn program_bantuan;
        private DataGridViewTextBoxColumn tgl_distribusi;
        private Label label21;

        public Distribusi()
        {
            InitializeComponent();
        }

        private void Distribusi_Load(object sender, EventArgs e)
        {
            try
            {
                // Form sizing sudah dikonfigurasi di Designer (FixedSingle, CenterScreen)
                // FormHelper.SetFullscreenMode(this);

                Panel footerPanel = this.Controls["pnlFooter"] as Panel;
                FormHelper.SetPanelDocking(panel1, null, footerPanel);
                FormHelper.SetDataGridViewResponsive(dataGridView1);

                // Set Anchor untuk semua panel agar responsive
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is Panel || ctrl is DataGridView)
                    {
                        ctrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | 
                                     AnchorStyles.Right | AnchorStyles.Bottom;
                    }
                }

                LoadProgramFilter();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error di Distribusi_Load: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT id_distribusi, nama_warga, program_bantuan, tgl_disetujui, status_permohonan AS status FROM distribusi_view";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.Rows.Clear();
                    int no = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        int rowIdx = dataGridView1.Rows.Add(
                            no++,
                            row["nama_warga"].ToString(),
                            row["program_bantuan"].ToString(),
                            row["tgl_disetujui"] != DBNull.Value ? Convert.ToDateTime(row["tgl_disetujui"]).ToString("dd/MM/yyyy") : ""
                        );
                        dataGridView1.Rows[rowIdx].Tag = row["id_distribusi"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadProgramFilter()
        {
            program_cmb.Items.Clear();
            program_cmb.Items.Add("Semua Program");
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT DISTINCT program_bantuan FROM distribusi_view";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        program_cmb.Items.Add(reader["program_bantuan"].ToString());
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading programs: " + ex.Message);
            }
            program_cmb.SelectedIndex = 0;

            // Load Dicatat Oleh
            dicatat_cmb.Items.Clear();
            dicatat_cmb.Items.Add("Pilih Petugas");
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT nama FROM users WHERE role = 'petugas_rtrw' AND is_active = 1";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        dicatat_cmb.Items.Add(reader["nama"].ToString());
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading petugas: " + ex.Message);
            }
            dicatat_cmb.SelectedIndex = 0;
        }

        private void program_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void cari_bt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cari_tb.Text))
            {
                MessageBox.Show("Masukkan nama warga yang ingin dicari");
                return;
            }
            SearchData();
        }

        private void reset_bt_Click(object sender, EventArgs e)
        {
            cari_tb.Clear();
            program_cmb.SelectedIndex = 0;
            LoadData();
            ClearDetailForm();
        }

        private void FilterData()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string program = program_cmb.SelectedItem.ToString();

                    string query = "SELECT id_distribusi, nama_warga, program_bantuan, tgl_disetujui, status_permohonan AS status FROM distribusi_view";

                    if (program != "Semua Program")
                    {
                        query += " WHERE program_bantuan = @program";
                    }

                    query += " ORDER BY tgl_disetujui DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (program != "Semua Program")
                    {
                        cmd.Parameters.AddWithValue("@program", program);
                    }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.Rows.Clear();
                    int no = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        int rowIdx = dataGridView1.Rows.Add(
                            no++,
                            row["nama_warga"].ToString(),
                            row["program_bantuan"].ToString(),
                            row["tgl_disetujui"] != DBNull.Value ? Convert.ToDateTime(row["tgl_disetujui"]).ToString("dd/MM/yyyy") : ""
                        );
                        dataGridView1.Rows[rowIdx].Tag = row["id_distribusi"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void SearchData()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT id_distribusi, nama_warga, program_bantuan, tgl_disetujui, status_permohonan AS status FROM distribusi_view WHERE nama_warga LIKE @searchTerm";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + cari_tb.Text + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.Rows.Clear();
                    int no = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        int rowIdx = dataGridView1.Rows.Add(
                            no++,
                            row["nama_warga"].ToString(),
                            row["program_bantuan"].ToString(),
                            row["tgl_disetujui"] != DBNull.Value ? Convert.ToDateTime(row["tgl_disetujui"]).ToString("dd/MM/yyyy") : ""
                        );
                        dataGridView1.Rows[rowIdx].Tag = row["id_distribusi"];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    int idDistribusi = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Tag);
                    LoadDetailDistribusiById(idDistribusi);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void LoadDetailDistribusiById(int idDistribusi)
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    // JOIN distribusi_view dengan tabel distribusi untuk mengambil semua data
                    string query = @"SELECT dv.id_distribusi, dv.nama_warga, dv.program_bantuan, dv.tgl_disetujui,
                                            d.jumlah_bantuan, d.bentuk_bantuan, d.bukti_penerimaan, 
                                            d.dicatat_oleh, d.keterangan
                                     FROM distribusi_view dv
                                     LEFT JOIN distribusi d ON dv.id_distribusi = d.id_distribusi
                                     WHERE dv.id_distribusi = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", idDistribusi);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Simpan data ke variable sebelum reader ditutup
                        currentDistribusiId = Convert.ToInt32(reader["id_distribusi"]);
                        currentPermohonanId = idDistribusi;
                        string namaPenerima = reader["nama_warga"].ToString();
                        string programBantuan = reader["program_bantuan"].ToString();
                        DateTime? tglDisetujui = reader["tgl_disetujui"] != DBNull.Value ? Convert.ToDateTime(reader["tgl_disetujui"]) : (DateTime?)null;
                        decimal jumlahBantuan = reader["jumlah_bantuan"] != DBNull.Value ? Convert.ToDecimal(reader["jumlah_bantuan"]) : 0;
                        string bentukBantuan = reader["bentuk_bantuan"] != DBNull.Value ? reader["bentuk_bantuan"].ToString() : "";
                        string buktiBantuan = reader["bukti_penerimaan"] != DBNull.Value ? reader["bukti_penerimaan"].ToString() : "";
                        string dicatatOleh = reader["dicatat_oleh"] != DBNull.Value ? reader["dicatat_oleh"].ToString() : "";
                        string keterangan = reader["keterangan"] != DBNull.Value ? reader["keterangan"].ToString() : "";

                        // Isi textbox dengan data yang sudah disimpan
                        nama_tb.Text = namaPenerima;
                        program_tb.Text = programBantuan;
                        tgl_tb.Text = tglDisetujui.HasValue ? tglDisetujui.Value.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                        jumlah_tb.Text = jumlahBantuan > 0 ? jumlahBantuan.ToString("N0") : "";
                        bentuk_bantuan_tb.Text = bentukBantuan;
                        bukti_tb.Text = buktiBantuan;
                        keterangan_tb.Text = keterangan;

                        // Set Dicatat Oleh combobox
                        if (!string.IsNullOrEmpty(dicatatOleh) && dicatat_cmb.Items.Contains(dicatatOleh))
                        {
                            dicatat_cmb.SelectedItem = dicatatOleh;
                        }
                        else
                        {
                            dicatat_cmb.SelectedIndex = 0;
                        }

                        reader.Close();

                        // Ambil NIK dari pendaftar berdasarkan nama
                        string queryNik = @"SELECT nik FROM pendaftar WHERE nama_lengkap = @nama LIMIT 1";
                        MySqlCommand cmdNik = new MySqlCommand(queryNik, conn);
                        cmdNik.Parameters.AddWithValue("@nama", namaPenerima);
                        MySqlDataReader readerNik = cmdNik.ExecuteReader();
                        if (readerNik.Read())
                        {
                            nik_tb.Text = readerNik["nik"].ToString();
                        }
                        readerNik.Close();

                        // Gunakan variable yang sudah disimpan, bukan reader yang sudah ditutup
                        label20.Text = "Form Pencatatan Distribusi — " + namaPenerima;
                        label21.Text = "Dipilih: " + namaPenerima + " | ID: " + currentDistribusiId;
                    }
                    else
                    {
                        MessageBox.Show("Data distribusi tidak ditemukan.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.keluar_bt = new System.Windows.Forms.Button();
            this.laporan_bt = new System.Windows.Forms.Button();
            this.distribusi_bt = new System.Windows.Forms.Button();
            this.permohonan_bt = new System.Windows.Forms.Button();
            this.verifikasi_bt = new System.Windows.Forms.Button();
            this.dashboard_bt = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label9 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.reset_bt = new System.Windows.Forms.Button();
            this.cari_bt = new System.Windows.Forms.Button();
            this.cari_tb = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.program_cmb = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dicatat_cmb = new System.Windows.Forms.ComboBox();
            this.keterangan_tb = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.bentuk_bantuan_tb = new System.Windows.Forms.TextBox();
            this.tgl_tb = new System.Windows.Forms.TextBox();
            this.nik_tb = new System.Windows.Forms.TextBox();
            this.bukti_tb = new System.Windows.Forms.TextBox();
            this.jumlah_tb = new System.Windows.Forms.TextBox();
            this.program_tb = new System.Windows.Forms.TextBox();
            this.nama_tb = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nama_warga = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.program_bantuan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tgl_distribusi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DarkGray;
            this.panel2.ForeColor = System.Drawing.Color.DarkGray;
            this.panel2.Location = new System.Drawing.Point(56, 124);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(771, 1);
            this.panel2.TabIndex = 22;
            // 
            // keluar_bt
            // 
            this.keluar_bt.BackColor = System.Drawing.Color.Red;
            this.keluar_bt.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keluar_bt.ForeColor = System.Drawing.Color.White;
            this.keluar_bt.Location = new System.Drawing.Point(756, 91);
            this.keluar_bt.Name = "keluar_bt";
            this.keluar_bt.Size = new System.Drawing.Size(118, 27);
            this.keluar_bt.TabIndex = 21;
            this.keluar_bt.Text = "Keluar";
            this.keluar_bt.UseVisualStyleBackColor = false;
            this.keluar_bt.Click += new System.EventHandler(this.keluar_bt_Click);
            // 
            // laporan_bt
            // 
            this.laporan_bt.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.laporan_bt.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.laporan_bt.Location = new System.Drawing.Point(612, 91);
            this.laporan_bt.Name = "laporan_bt";
            this.laporan_bt.Size = new System.Drawing.Size(118, 27);
            this.laporan_bt.TabIndex = 20;
            this.laporan_bt.Text = "Laporan";
            this.laporan_bt.UseVisualStyleBackColor = true;
            this.laporan_bt.Click += new System.EventHandler(this.laporan_bt_Click);
            // 
            // distribusi_bt
            // 
            this.distribusi_bt.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.distribusi_bt.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.distribusi_bt.Location = new System.Drawing.Point(463, 91);
            this.distribusi_bt.Name = "distribusi_bt";
            this.distribusi_bt.Size = new System.Drawing.Size(118, 27);
            this.distribusi_bt.TabIndex = 19;
            this.distribusi_bt.Text = "Distribusi";
            this.distribusi_bt.UseVisualStyleBackColor = true;
            this.distribusi_bt.Click += new System.EventHandler(this.distribusi_bt_Click);
            // 
            // permohonan_bt
            // 
            this.permohonan_bt.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.permohonan_bt.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.permohonan_bt.Location = new System.Drawing.Point(319, 91);
            this.permohonan_bt.Name = "permohonan_bt";
            this.permohonan_bt.Size = new System.Drawing.Size(118, 27);
            this.permohonan_bt.TabIndex = 18;
            this.permohonan_bt.Text = "Permohonan";
            this.permohonan_bt.UseVisualStyleBackColor = true;
            this.permohonan_bt.Click += new System.EventHandler(this.permohonan_bt_Click);
            // 
            // verifikasi_bt
            // 
            this.verifikasi_bt.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.verifikasi_bt.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.verifikasi_bt.Location = new System.Drawing.Point(171, 91);
            this.verifikasi_bt.Name = "verifikasi_bt";
            this.verifikasi_bt.Size = new System.Drawing.Size(118, 27);
            this.verifikasi_bt.TabIndex = 17;
            this.verifikasi_bt.Text = "Verifikasi";
            this.verifikasi_bt.UseVisualStyleBackColor = true;
            this.verifikasi_bt.Click += new System.EventHandler(this.verifikasi_bt_Click);
            // 
            // dashboard_bt
            // 
            this.dashboard_bt.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dashboard_bt.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dashboard_bt.Location = new System.Drawing.Point(15, 91);
            this.dashboard_bt.Name = "dashboard_bt";
            this.dashboard_bt.Size = new System.Drawing.Size(121, 27);
            this.dashboard_bt.TabIndex = 16;
            this.dashboard_bt.Text = "Dashboard";
            this.dashboard_bt.UseVisualStyleBackColor = true;
            this.dashboard_bt.Click += new System.EventHandler(this.dashboard_bt_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(95)))), ((int)(((byte)(138)))));
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(-3, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(883, 60);
            this.panel1.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(704, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(171, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "RT 001/RW 001 - Sukamaju";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(725, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Petugas RT 01";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(14, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(269, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Sistem Informasi Penyaluran Bantuan Sosial";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "SIBANTUAN";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.dataGridView1);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Location = new System.Drawing.Point(18, 205);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(856, 240);
            this.panel4.TabIndex = 27;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.no,
            this.nama_warga,
            this.program_bantuan,
            this.tgl_distribusi});
            this.dataGridView1.Location = new System.Drawing.Point(10, 31);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(835, 180);
            this.dataGridView1.TabIndex = 23;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(3, 5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(150, 20);
            this.label9.TabIndex = 22;
            this.label9.Text = "Daftar Distribusi";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.reset_bt);
            this.panel3.Controls.Add(this.cari_bt);
            this.panel3.Controls.Add(this.cari_tb);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.program_cmb);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Location = new System.Drawing.Point(18, 133);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(857, 66);
            this.panel3.TabIndex = 26;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(179, 20);
            this.label8.TabIndex = 21;
            this.label8.Text = "Filter dan Pencarian";
            // 
            // reset_bt
            // 
            this.reset_bt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reset_bt.Location = new System.Drawing.Point(717, 17);
            this.reset_bt.Name = "reset_bt";
            this.reset_bt.Size = new System.Drawing.Size(60, 23);
            this.reset_bt.TabIndex = 22;
            this.reset_bt.Text = "Reset";
            this.reset_bt.UseVisualStyleBackColor = true;
            this.reset_bt.Click += new System.EventHandler(this.reset_bt_Click);
            // 
            // cari_bt
            // 
            this.cari_bt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cari_bt.Location = new System.Drawing.Point(651, 17);
            this.cari_bt.Name = "cari_bt";
            this.cari_bt.Size = new System.Drawing.Size(60, 23);
            this.cari_bt.TabIndex = 21;
            this.cari_bt.Text = "Cari";
            this.cari_bt.UseVisualStyleBackColor = true;
            this.cari_bt.Click += new System.EventHandler(this.cari_bt_Click);
            // 
            // cari_tb
            // 
            this.cari_tb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cari_tb.Location = new System.Drawing.Point(282, 27);
            this.cari_tb.Name = "cari_tb";
            this.cari_tb.Size = new System.Drawing.Size(180, 24);
            this.cari_tb.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(237, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 20);
            this.label7.TabIndex = 19;
            this.label7.Text = "Cari :";
            // 
            // program_cmb
            // 
            this.program_cmb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.program_cmb.FormattingEnabled = true;
            this.program_cmb.Location = new System.Drawing.Point(79, 27);
            this.program_cmb.Name = "program_cmb";
            this.program_cmb.Size = new System.Drawing.Size(135, 26);
            this.program_cmb.TabIndex = 18;
            this.program_cmb.SelectedIndexChanged += new System.EventHandler(this.program_cmb_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(4, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 20);
            this.label6.TabIndex = 17;
            this.label6.Text = "Program :";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.DarkGray;
            this.panel5.ForeColor = System.Drawing.Color.DarkGray;
            this.panel5.Location = new System.Drawing.Point(55, 124);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(771, 1);
            this.panel5.TabIndex = 25;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.dicatat_cmb);
            this.panel6.Controls.Add(this.keterangan_tb);
            this.panel6.Controls.Add(this.label20);
            this.panel6.Controls.Add(this.button3);
            this.panel6.Controls.Add(this.button1);
            this.panel6.Controls.Add(this.bentuk_bantuan_tb);
            this.panel6.Controls.Add(this.tgl_tb);
            this.panel6.Controls.Add(this.nik_tb);
            this.panel6.Controls.Add(this.bukti_tb);
            this.panel6.Controls.Add(this.jumlah_tb);
            this.panel6.Controls.Add(this.program_tb);
            this.panel6.Controls.Add(this.nama_tb);
            this.panel6.Controls.Add(this.label19);
            this.panel6.Controls.Add(this.label18);
            this.panel6.Controls.Add(this.label17);
            this.panel6.Controls.Add(this.label16);
            this.panel6.Controls.Add(this.label15);
            this.panel6.Controls.Add(this.label14);
            this.panel6.Controls.Add(this.label13);
            this.panel6.Controls.Add(this.label12);
            this.panel6.Controls.Add(this.label11);
            this.panel6.Location = new System.Drawing.Point(18, 472);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(856, 304);
            this.panel6.TabIndex = 28;
            // 
            // dicatat_cmb
            // 
            this.dicatat_cmb.FormattingEnabled = true;
            this.dicatat_cmb.Location = new System.Drawing.Point(593, 130);
            this.dicatat_cmb.Name = "dicatat_cmb";
            this.dicatat_cmb.Size = new System.Drawing.Size(210, 24);
            this.dicatat_cmb.TabIndex = 28;
            // 
            // keterangan_tb
            // 
            this.keterangan_tb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keterangan_tb.Location = new System.Drawing.Point(28, 199);
            this.keterangan_tb.Multiline = true;
            this.keterangan_tb.Name = "keterangan_tb";
            this.keterangan_tb.Size = new System.Drawing.Size(800, 40);
            this.keterangan_tb.TabIndex = 27;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(3, 5);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(213, 18);
            this.label20.TabIndex = 25;
            this.label20.Text = "Form Pencatatan Distribusi";
            this.label20.Click += new System.EventHandler(this.label20_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(114, 261);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(80, 28);
            this.button3.TabIndex = 19;
            this.button3.Text = "Reset";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LimeGreen;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(25, 261);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 28);
            this.button1.TabIndex = 17;
            this.button1.Text = "Simpan";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bentuk_bantuan_tb
            // 
            this.bentuk_bantuan_tb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bentuk_bantuan_tb.Location = new System.Drawing.Point(593, 95);
            this.bentuk_bantuan_tb.Name = "bentuk_bantuan_tb";
            this.bentuk_bantuan_tb.Size = new System.Drawing.Size(210, 24);
            this.bentuk_bantuan_tb.TabIndex = 15;
            // 
            // tgl_tb
            // 
            this.tgl_tb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tgl_tb.Location = new System.Drawing.Point(593, 60);
            this.tgl_tb.Name = "tgl_tb";
            this.tgl_tb.Size = new System.Drawing.Size(210, 24);
            this.tgl_tb.TabIndex = 14;
            this.tgl_tb.TextChanged += new System.EventHandler(this.tgl_tb_TextChanged);
            // 
            // nik_tb
            // 
            this.nik_tb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nik_tb.Location = new System.Drawing.Point(593, 25);
            this.nik_tb.Name = "nik_tb";
            this.nik_tb.Size = new System.Drawing.Size(210, 24);
            this.nik_tb.TabIndex = 13;
            // 
            // bukti_tb
            // 
            this.bukti_tb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bukti_tb.Location = new System.Drawing.Point(152, 130);
            this.bukti_tb.Name = "bukti_tb";
            this.bukti_tb.Size = new System.Drawing.Size(210, 24);
            this.bukti_tb.TabIndex = 12;
            // 
            // jumlah_tb
            // 
            this.jumlah_tb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jumlah_tb.Location = new System.Drawing.Point(152, 95);
            this.jumlah_tb.Name = "jumlah_tb";
            this.jumlah_tb.Size = new System.Drawing.Size(210, 24);
            this.jumlah_tb.TabIndex = 11;
            // 
            // program_tb
            // 
            this.program_tb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.program_tb.Location = new System.Drawing.Point(152, 60);
            this.program_tb.Name = "program_tb";
            this.program_tb.Size = new System.Drawing.Size(210, 24);
            this.program_tb.TabIndex = 10;
            // 
            // nama_tb
            // 
            this.nama_tb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nama_tb.Location = new System.Drawing.Point(152, 25);
            this.nama_tb.Name = "nama_tb";
            this.nama_tb.Size = new System.Drawing.Size(210, 24);
            this.nama_tb.TabIndex = 9;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(25, 165);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(83, 18);
            this.label19.TabIndex = 8;
            this.label19.Text = "Keterangan";
            this.label19.Click += new System.EventHandler(this.label19_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(470, 130);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(89, 18);
            this.label18.TabIndex = 7;
            this.label18.Text = "Dicatat Oleh";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(470, 95);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(112, 18);
            this.label17.TabIndex = 6;
            this.label17.Text = "Bentuk Bantuan";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(470, 60);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(93, 18);
            this.label16.TabIndex = 5;
            this.label16.Text = "Tgl Distribusi";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(470, 25);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 18);
            this.label15.TabIndex = 4;
            this.label15.Text = "NIK";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(25, 130);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(116, 18);
            this.label14.TabIndex = 3;
            this.label14.Text = "Bukti Penrimaan";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(25, 95);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(114, 18);
            this.label13.TabIndex = 2;
            this.label13.Text = "Jumlah Bantuan";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(25, 60);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(124, 18);
            this.label12.TabIndex = 1;
            this.label12.Text = "Program Bantuan";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(25, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(96, 18);
            this.label11.TabIndex = 0;
            this.label11.Text = "Nama Warga";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.Gray;
            this.label21.Location = new System.Drawing.Point(3, 28);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(135, 17);
            this.label21.TabIndex = 29;
            // 
            // no
            // 
            this.no.HeaderText = "No";
            this.no.MinimumWidth = 6;
            this.no.Name = "no";
            this.no.Width = 35;
            // 
            // nama_warga
            // 
            this.nama_warga.HeaderText = "Nama Warga";
            this.nama_warga.MinimumWidth = 6;
            this.nama_warga.Name = "nama_warga";
            this.nama_warga.Width = 140;
            // 
            // program_bantuan
            // 
            this.program_bantuan.HeaderText = "Program Bantuan";
            this.program_bantuan.MinimumWidth = 6;
            this.program_bantuan.Name = "program_bantuan";
            this.program_bantuan.Width = 150;
            // 
            // tgl_distribusi
            // 
            this.tgl_distribusi.HeaderText = "Tgl Distribusi";
            this.tgl_distribusi.MinimumWidth = 6;
            this.tgl_distribusi.Name = "tgl_distribusi";
            this.tgl_distribusi.Width = 90;
            // 
            // Distribusi
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(894, 796);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.keluar_bt);
            this.Controls.Add(this.laporan_bt);
            this.Controls.Add(this.distribusi_bt);
            this.Controls.Add(this.permohonan_bt);
            this.Controls.Add(this.verifikasi_bt);
            this.Controls.Add(this.dashboard_bt);
            this.Controls.Add(this.panel1);
            this.Name = "Distribusi";
            this.Load += new System.EventHandler(this.Distribusi_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (currentDistribusiId == 0)
            {
                MessageBox.Show("Pilih data distribusi terlebih dahulu");
                return;
            }

            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"UPDATE distribusi 
                                    SET tanggal_distribusi = @tglDistribusi,
                                        jumlah_bantuan = @jumlahBantuan,
                                        bentuk_bantuan = @bentukBantuan,
                                        bukti_penerimaan = @buktiBantuan,
                                        dicatat_oleh = @dicatatOleh,
                                        keterangan = @keterangan
                                    WHERE id_distribusi = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", currentDistribusiId);
                    cmd.Parameters.AddWithValue("@tglDistribusi", string.IsNullOrEmpty(tgl_tb.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : tgl_tb.Text);
                    cmd.Parameters.AddWithValue("@jumlahBantuan", string.IsNullOrEmpty(jumlah_tb.Text) ? 0 : decimal.Parse(jumlah_tb.Text.Replace(".", "")));
                    cmd.Parameters.AddWithValue("@bentukBantuan", bentuk_bantuan_tb.Text);
                    cmd.Parameters.AddWithValue("@buktiBantuan", bukti_tb.Text);
                    cmd.Parameters.AddWithValue("@dicatatOleh", dicatat_cmb.SelectedItem?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@keterangan", keterangan_tb.Text);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Data distribusi berhasil disimpan");
                        LoadData();
                        ClearDetailForm();
                    }
                    else
                    {
                        MessageBox.Show("Gagal menyimpan data distribusi");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (currentDistribusiId == 0)
            {
                MessageBox.Show("Pilih data distribusi terlebih dahulu");
                return;
            }

            MessageBox.Show("Fungsi cetak bukti belum diimplementasikan");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearDetailForm();
            dataGridView1.ClearSelection();
        }

        private void OpenFormInPanel(Form form)
        {
            if (DashboardPetugas.Instance != null)
            {
                DashboardPetugas.Instance.ShowFormInContent(form);
                this.Close();
            }
        }

        private void dashboard_bt_Click(object sender, EventArgs e) 
        { 
            try
            {
                if (DashboardPetugas.Instance != null)
                {
                    DashboardPetugas.Instance.ShowContentControls();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Dashboard tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void verifikasi_bt_Click(object sender, EventArgs e) 
        { 
            try
            {
                if (DashboardPetugas.Instance != null)
                {
                    Verifikasi form = new Verifikasi();
                    DashboardPetugas.Instance.ShowFormInContent(form);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Dashboard tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void permohonan_bt_Click(object sender, EventArgs e) 
        { 
            try
            {
                if (DashboardPetugas.Instance != null)
                {
                    Permohonan form = new Permohonan();
                    DashboardPetugas.Instance.ShowFormInContent(form);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Dashboard tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void distribusi_bt_Click(object sender, EventArgs e) { /* already here */ }
        private void laporan_bt_Click(object sender, EventArgs e) 
        { 
            try
            {
                if (DashboardPetugas.Instance != null)
                {
                    Laporan form = new Laporan();
                    DashboardPetugas.Instance.ShowFormInContent(form);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Dashboard tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void keluar_bt_Click(object sender, EventArgs e) { Application.Exit(); }

        private void ClearDetailForm()
        {
            currentDistribusiId = 0;
            currentPermohonanId = 0;
            nama_tb.Clear();
            program_tb.Clear();
            jumlah_tb.Clear();
            bukti_tb.Clear();
            nik_tb.Clear();
            tgl_tb.Clear();
            bentuk_bantuan_tb.Clear();
            keterangan_tb.Clear();
            dicatat_cmb.SelectedIndex = 0;
            label20.Text = "Form Pencatatan Distribusi";
            label21.Text = "";
        }

        private void tgl_tb_TextChanged(object sender, EventArgs e)
        {

        }
    }
        }
