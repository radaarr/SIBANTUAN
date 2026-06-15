using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace SIBANTUAN.Forms.Petugas
{
    public partial class DashboardPetugas : Form
    {
        private int userId;
        private string nama;

        public DashboardPetugas(int userId, string nama)
        {
            InitializeComponent();
            this.userId = userId;
            this.nama = nama;
        }

        private void DashboardPetugas_Load(object sender, EventArgs e)
        {
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

                    MySqlCommand cmd2 = new MySqlCommand("SELECT COUNT(*) FROM pendaftar WHERE status = 'Belum Diverifikasi'", conn);
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

        private void OpenForm(Form form)
        {
            form.ShowDialog();
        }

        private void dashboard_bt_Click(object sender, EventArgs e)
        {
            // Already on dashboard
        }

        private void verifikasi_bt_Click(object sender, EventArgs e)
        {
            OpenForm(new Verifikasi());
        }

        private void permohonan_bt_Click(object sender, EventArgs e)
        {
            OpenForm(new Permohonan());
        }

        private void distribus_bt_Click(object sender, EventArgs e)
        {
            OpenForm(new Distribusi());
        }

        private void laporan_bt_Click(object sender, EventArgs e)
        {
            OpenForm(new Laporan());
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
            OpenForm(new Verifikasi());
        }

        private void btnMenuPermohonan_Click(object sender, EventArgs e)
        {
            OpenForm(new Permohonan());
        }

        private void btnMenuDistribusi_Click(object sender, EventArgs e)
        {
            OpenForm(new Distribusi());
        }

        private void btnMenuLaporan_Click(object sender, EventArgs e)
        {
            OpenForm(new Laporan());
        }
    }
}
