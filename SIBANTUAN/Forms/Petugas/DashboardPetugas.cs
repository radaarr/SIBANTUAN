using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace SIBANTUAN.Forms.Petugas
{
    public partial class DashboardPetugas : Form
    {
        private int userId;
        private string nama;
        public static DashboardPetugas Instance { get; private set; }

        public DashboardPetugas(int userId, string nama)
        {
            InitializeComponent();
            this.userId = userId;
            this.nama = nama;
            Instance = this;
        }

        private void DashboardPetugas_Load(object sender, EventArgs e)
        {
            // Form sizing sudah dikonfigurasi di Designer (FixedSingle, CenterScreen)
            // FormHelper.SetFullscreenMode(this);

            Panel footerPanel = this.Controls["pnlFooter"] as Panel;
            FormHelper.SetPanelDocking(panel1, null, footerPanel);

            lblGreeting.Text = "Selamat datang, " + nama + ". Ada tugas yang perlu ditindaklanjuti.";
            lblDate.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            label3.Text = nama;
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    MySqlCommand cmd1 = new MySqlCommand("SELECT COUNT(*) FROM penduduk", conn);
                    lblCard1Value.Text = cmd1.ExecuteScalar().ToString();

                    MySqlCommand cmd2 = new MySqlCommand("SELECT COUNT(*) FROM penduduk WHERE status_verifikasi = 'Belum Diverifikasi'", conn);
                    lblCard2Value.Text = cmd2.ExecuteScalar().ToString();

                    MySqlCommand cmd3 = new MySqlCommand("SELECT COUNT(*) FROM permohonan_view WHERE status = 'Pending'", conn);
                    lblCard3Value.Text = cmd3.ExecuteScalar().ToString();

                    MySqlCommand cmd4 = new MySqlCommand("SELECT COUNT(*) FROM distribusi", conn);
                    lblCard4Value.Text = cmd4.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error memuat statistik: " + ex.Message);
            }
        }

        public void ShowFormInContent(Form form)
        {
            // Form sekarang dibuka sebagai independent window, tidak perlu dikonfigurasi
            form.Show();
        }

        private void AdjustFormPositions(Form form)
        {
            // Metode ini tidak lagi digunakan
        }

        public void HideContentControls()
        {
            pnlStatistik.Visible = false;
            pnlNotifikasi.Visible = false;
            pnlMenuCepat.Visible = false;
            lblGreeting.Visible = false;
            lblDate.Visible = false;
        }

        public void ShowContentControls()
        {
            pnlStatistik.Visible = true;
            pnlNotifikasi.Visible = true;
            pnlMenuCepat.Visible = true;
            lblGreeting.Visible = true;
            lblDate.Visible = true;
        }

        private void OpenForm(Form form)
        {
            ShowFormInContent(form);
        }

        private void dashboard_bt_Click(object sender, EventArgs e)
        {
            // Show dashboard content
            this.Controls.Clear();
            InitializeComponent();
            DashboardPetugas_Load(this, EventArgs.Empty);
        }

        private void verifikasi_bt_Click(object sender, EventArgs e)
        {
            Verifikasi form = new Verifikasi();
            form.Show();
        }

        private void permohonan_bt_Click(object sender, EventArgs e)
        {
            Permohonan form = new Permohonan();
            form.Show();
        }

        private void distribus_bt_Click(object sender, EventArgs e)
        {
            Distribusi form = new Distribusi();
            form.Show();
        }

        private void laporan_bt_Click(object sender, EventArgs e)
        {
            Laporan form = new Laporan();
            form.Show();
        }

        private void keluar_bt_Click(object sender, EventArgs e)
        {
            DialogResult konfirmasi = MessageBox.Show(
                "Apakah kamu yakin ingin keluar?", "Konfirmasi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (konfirmasi == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnMenuVerifikasi_Click(object sender, EventArgs e)
        {
            Verifikasi form = new Verifikasi();
            form.Show();
        }

        private void btnMenuPermohonan_Click(object sender, EventArgs e)
        {
            Permohonan form = new Permohonan();
            form.Show();
        }

        private void btnMenuDistribusi_Click(object sender, EventArgs e)
        {
            Distribusi form = new Distribusi();
            form.Show();
        }

        private void btnMenuLaporan_Click(object sender, EventArgs e)
        {
            Laporan form = new Laporan();
            form.Show();
        }
    }
}
