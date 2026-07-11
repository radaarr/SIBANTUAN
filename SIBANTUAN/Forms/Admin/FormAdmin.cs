using MySql.Data.MySqlClient;
using SIBANTUAN.Forms.Admin;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SIBANTUAN.Forms
{
    public partial class FormAdmin : Form
    {
        // Variabel penampung agar form yang lama bisa ditutup saat form baru dibuka
        private Form activeForm = null;

        public FormAdmin()
        {
            InitializeComponent();
            SetupModernUI();
            AddAdminNavButtons();
            TampilkanDashboard(); // Tampilkan dashboard langsung saat aplikasi pertama kali dibuka
        }

        private void AddAdminNavButtons()
        {
            Button btnPenduduk = new Button();
            btnPenduduk.Text = "Data Penduduk";
            btnPenduduk.Dock = DockStyle.Top;
            btnPenduduk.FlatAppearance.BorderSize = 0;
            btnPenduduk.FlatStyle = FlatStyle.Flat;
            btnPenduduk.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnPenduduk.ForeColor = Color.WhiteSmoke;
            btnPenduduk.Height = 50;
            btnPenduduk.Click += BtnPenduduk_Click;
            panel1.Controls.Add(btnPenduduk);
            panel1.Controls.SetChildIndex(btnPenduduk, 0);

            Button btnPermohonan = new Button();
            btnPermohonan.Text = "Permohonan";
            btnPermohonan.Dock = DockStyle.Top;
            btnPermohonan.FlatAppearance.BorderSize = 0;
            btnPermohonan.FlatStyle = FlatStyle.Flat;
            btnPermohonan.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnPermohonan.ForeColor = Color.WhiteSmoke;
            btnPermohonan.Height = 50;
            btnPermohonan.Click += BtnPermohonanAdmin_Click;
            panel1.Controls.Add(btnPermohonan);
            panel1.Controls.SetChildIndex(btnPermohonan, 0);
        }

        // ====================================================
        // 1. PENGATURAN UI OTOMATIS (Merapikan Sidebar & Form)
        // ====================================================
        private void SetupModernUI()
        {
            this.Text = "Dashboard Admin - SIBANTUAN";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1024, 768);

            // Mewarnai panel utama dengan warna background abu-abu sangat muda agar form anak terlihat kontras
            if (panel3 != null)
            {
                panel3.BackColor = Color.FromArgb(245, 246, 250);
            }
        }

        // ====================================================
        // 2. FUNGSI MENAMPILKAN ISI DASHBOARD (Home)
        // ====================================================
        private void TampilkanDashboard()
        {
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm = null;
            }

            panel3.Controls.Clear();

            Label lblJudul = new Label();
            lblJudul.Text = "Selamat Datang di SIBANTUAN";
            lblJudul.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblJudul.ForeColor = Color.FromArgb(44, 62, 80);
            lblJudul.AutoSize = true;
            lblJudul.Location = new Point(30, 30);
            panel3.Controls.Add(lblJudul);

            Label lblSubJudul = new Label();
            lblSubJudul.Text = "Sistem Informasi Bantuan Sosial Terpadu - Halaman Administrator";
            lblSubJudul.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            lblSubJudul.ForeColor = Color.Gray;
            lblSubJudul.AutoSize = true;
            lblSubJudul.Location = new Point(35, 75);
            panel3.Controls.Add(lblSubJudul);

            // Ambil data live dari database
            int totalProgram = 0;
            int totalPenerima = 0;
            int menungguVerifikasi = 0;

            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    MySqlCommand cmd1 = new MySqlCommand(
                        "SELECT COUNT(*) FROM program_bantuan WHERE status_program = 'aktif'", conn);
                    totalProgram = Convert.ToInt32(cmd1.ExecuteScalar());

                    MySqlCommand cmd2 = new MySqlCommand(
                        "SELECT COUNT(*) FROM penduduk p JOIN users u ON p.user_id = u.id WHERE u.role = 'penerima_bantuan' AND p.status_verifikasi = 'Disetujui'", conn);
                    totalPenerima = Convert.ToInt32(cmd2.ExecuteScalar());

                    MySqlCommand cmd3 = new MySqlCommand(
                        "SELECT COUNT(*) FROM permohonan WHERE status_permohonan = 'pending'", conn);
                    menungguVerifikasi = Convert.ToInt32(cmd3.ExecuteScalar());
                }
            }
            catch { }

            Panel card1 = BuatKartu("Program Aktif", totalProgram + " Program", Color.FromArgb(52, 152, 219), 30, 130);
            panel3.Controls.Add(card1);

            Panel card2 = BuatKartu("Penerima Bantuan", totalPenerima + " Jiwa", Color.FromArgb(46, 204, 113), 260, 130);
            panel3.Controls.Add(card2);

            Panel card3 = BuatKartu("Menunggu Verifikasi", menungguVerifikasi + " Pengajuan", Color.FromArgb(241, 196, 15), 490, 130);
            panel3.Controls.Add(card3);
        }

        // Fungsi bantuan (Helper) untuk mendesain kartu Dashboard
        private Panel BuatKartu(string judul, string nilai, Color warnaAksen, int x, int y)
        {
            Panel card = new Panel();
            card.Size = new Size(210, 120);
            card.Location = new Point(x, y);
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;

            // Garis warna di atas kartu
            Panel headerCard = new Panel();
            headerCard.Size = new Size(210, 5);
            headerCard.Dock = DockStyle.Top;
            headerCard.BackColor = warnaAksen;
            card.Controls.Add(headerCard);

            Label lblJudul = new Label();
            lblJudul.Text = judul;
            lblJudul.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblJudul.ForeColor = Color.Gray;
            lblJudul.Location = new Point(15, 20);
            lblJudul.AutoSize = true;
            card.Controls.Add(lblJudul);

            Label lblNilai = new Label();
            lblNilai.Text = nilai;
            lblNilai.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblNilai.ForeColor = Color.Black;
            lblNilai.Location = new Point(15, 50);
            lblNilai.AutoSize = true;
            card.Controls.Add(lblNilai);

            return card;
        }

        // ====================================================
        // 3. FUNGSI MEMUAT FORM ANAK KE PANEL TENGAH
        // ====================================================
        private void LoadFormToMainPanel(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            childForm.BackColor = Color.FromArgb(245, 246, 250); // Menyelaraskan warna background anak

            if (panel3 == null) throw new InvalidOperationException("Panel utama tidak ditemukan.");

            panel3.Controls.Clear(); // Bersihkan dashboard sebelum memuat form baru
            panel3.Controls.Add(childForm);
            panel3.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        // ====================================================
        // 4. EVENT KLIK TOMBOL MENU SIDEBAR
        // ====================================================

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            TampilkanDashboard(); // Memanggil fungsi pembuat dashboard
        }

        private void btnProgramBantuan_Click(object sender, EventArgs e)
        {
            LoadFormToMainPanel(new FormProgramBantuan());
        }

        private void btnKelolaUser_Click(object sender, EventArgs e)
        {
            LoadFormToMainPanel(new FormKelolaUser());
        }

        private void btnLaporan_Click(object sender, EventArgs e)
        {
            LoadFormToMainPanel(new FormLaporanStatistik());
        }

        private void BtnPenduduk_Click(object sender, EventArgs e)
        {
            if (activeForm != null) { activeForm.Close(); activeForm = null; }
            panel3.Controls.Clear();
            DataGridView dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            panel3.Controls.Add(dgv);

            Label lbl = new Label();
            lbl.Text = "Data Seluruh Penduduk";
            lbl.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lbl.Location = new Point(20, 20);
            lbl.AutoSize = true;
            panel3.Controls.Add(lbl);

            dgv.Location = new Point(20, 70);
            dgv.Size = new Size(panel3.Width - 40, panel3.Height - 90);
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT p.nik, p.nama_lengkap, p.rt_rw, p.kelurahan, p.status_ekonomi,
                                            CASE WHEN u.role = 'petugas_rtrw' THEN 'Disetujui' ELSE p.status_verifikasi END AS status_verifikasi
                                     FROM penduduk p
                                     LEFT JOIN users u ON p.user_id = u.id
                                     ORDER BY p.kelurahan, p.rt_rw, p.nama_lengkap";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgv.DataSource = dt;
                    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void BtnPermohonanAdmin_Click(object sender, EventArgs e)
        {
            if (activeForm != null) { activeForm.Close(); activeForm = null; }
            panel3.Controls.Clear();
            DataGridView dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            panel3.Controls.Add(dgv);

            Label lbl = new Label();
            lbl.Text = "Semua Permohonan";
            lbl.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lbl.Location = new Point(20, 20);
            lbl.AutoSize = true;
            panel3.Controls.Add(lbl);

            dgv.Location = new Point(20, 70);
            dgv.Size = new Size(panel3.Width - 40, panel3.Height - 90);
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT p.nama_lengkap, p.rt_rw, p.kelurahan, pb.nama_program, pm.tanggal_pengajuan, pm.status_permohonan FROM permohonan pm JOIN penduduk p ON pm.penduduk_id = p.id JOIN program_bantuan pb ON pm.program_id = pb.id ORDER BY pm.tanggal_pengajuan DESC";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgv.DataSource = dt;
                    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // ====================================================
        // EVENT KOSONG UNTUK MENCEGAH ERROR DESAINER
        // ====================================================
        private void FormAdmin_Load(object sender, EventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void button2_Click(object sender, EventArgs e) { }
    }
}