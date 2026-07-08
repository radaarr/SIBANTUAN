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
        private object panelMain;

        public FormAdmin()
        {
            InitializeComponent();
            SetupModernUI();
        }

        // 1. Pengaturan UI awal agar form terlihat clean
        private void SetupModernUI()
        {
            this.Text = "Dashboard Admin - SIBANTUAN";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1024, 768);
        }

        // 2. Fungsi inti untuk memuat form anak ke dalam Panel Tengah
        private void LoadFormToMainPanel(Form childForm)
        {
            // Jika ada form yang sedang terbuka, tutup terlebih dahulu agar memori tidak bocor
            if (activeForm != null)
            {
                activeForm.Close();
            }

            activeForm = childForm;

            // Menghilangkan sifat "jendela bawaan" agar bisa ditempel
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // Memasukkan form anak ke dalam panelMain
            // Pastikan kamu sudah membuat Panel bernama "panelMain" di mode Designer
            this.panelMain.Controls.Add(childForm);
            this.panelMain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        // ====================================================
        // 3. EVENT KLIK TOMBOL MENU (Navigasi Sidebar)
        // ====================================================

        private void btnProgramBantuan_Click(object sender, EventArgs e)
        {
            // Memanggil Form Program Bantuan
            LoadFormToMainPanel(new FormProgramBantuan());
        }

        private void btnKelolaUser_Click(object sender, EventArgs e)
        {
            // Memanggil Form Kelola User
            LoadFormToMainPanel(new FormKelolaUser());
        }

        private void btnLaporan_Click(object sender, EventArgs e)
        {
            // Memanggil Form Laporan Statistik
            LoadFormToMainPanel(new FormLaporanStatistik());
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            // Jika kamu punya form khusus untuk ringkasan dashboard awal
            // LoadFormToMainPanel(new FormRingkasanDashboard());

            // Atau cukup tutup form yang aktif untuk menampilkan panel kosong/default
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm = null;
            }
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}