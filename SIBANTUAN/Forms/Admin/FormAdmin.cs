using SIBANTUAN.Forms.Admin;
using System;
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
            TampilkanDashboard(); // Tampilkan dashboard langsung saat aplikasi pertama kali dibuka
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
            // Tutup form anak yang sedang terbuka (jika ada)
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm = null;
            }

            // Bersihkan isi panel3
            panel3.Controls.Clear();

            // --- MEMBUAT JUDUL DASHBOARD ---
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

            // --- MEMBUAT KARTU INFORMASI (WIDGETS) ---
            // Kartu 1: Total Program
            Panel card1 = BuatKartu("Total Program", "3 Aktif", Color.FromArgb(52, 152, 219), 30, 130);
            panel3.Controls.Add(card1);

            // Kartu 2: Total Penerima
            Panel card2 = BuatKartu("Penerima Bantuan", "1,245 Jiwa", Color.FromArgb(46, 204, 113), 260, 130);
            panel3.Controls.Add(card2);

            // Kartu 3: Menunggu Verifikasi
            Panel card3 = BuatKartu("Menunggu Verifikasi", "12 Pengajuan", Color.FromArgb(241, 196, 15), 490, 130);
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
            // Pastikan FormKelolaUser sudah ada di project Anda
            // LoadFormToMainPanel(new FormKelolaUser()); 
        }

        private void btnLaporan_Click(object sender, EventArgs e)
        {
            // Pastikan FormLaporanStatistik sudah ada di project Anda
            // LoadFormToMainPanel(new FormLaporanStatistik());
        }

        // ====================================================
        // EVENT KOSONG UNTUK MENCEGAH ERROR DESAINER
        // ====================================================
        private void FormAdmin_Load(object sender, EventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void button2_Click(object sender, EventArgs e) { }
    }
}