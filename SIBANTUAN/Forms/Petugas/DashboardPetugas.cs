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
            label4.Text = Session.WilayahRtRw + " - " + Session.WilayahKelurahan;
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    MySqlCommand cmd1 = new MySqlCommand("SELECT COUNT(*) FROM penduduk WHERE rt_rw = @rw AND kelurahan = @kel", conn);
                    cmd1.Parameters.AddWithValue("@rw", Session.WilayahRtRw);
                    cmd1.Parameters.AddWithValue("@kel", Session.WilayahKelurahan);
                    lblCard1Value.Text = cmd1.ExecuteScalar().ToString();

                    MySqlCommand cmd2 = new MySqlCommand("SELECT COUNT(*) FROM penduduk WHERE status_verifikasi = 'Belum Diverifikasi' AND rt_rw = @rw AND kelurahan = @kel", conn);
                    cmd2.Parameters.AddWithValue("@rw", Session.WilayahRtRw);
                    cmd2.Parameters.AddWithValue("@kel", Session.WilayahKelurahan);
                    lblCard2Value.Text = cmd2.ExecuteScalar().ToString();

                    MySqlCommand cmd3 = new MySqlCommand("SELECT COUNT(*) FROM permohonan_view pv JOIN penduduk p ON pv.nik = p.nik WHERE pv.status = 'Pending' AND p.rt_rw = @rw AND p.kelurahan = @kel", conn);
                    cmd3.Parameters.AddWithValue("@rw", Session.WilayahRtRw);
                    cmd3.Parameters.AddWithValue("@kel", Session.WilayahKelurahan);
                    lblCard3Value.Text = cmd3.ExecuteScalar().ToString();

                    MySqlCommand cmd4 = new MySqlCommand("SELECT COUNT(*) FROM distribusi d JOIN permohonan pm ON d.permohonan_id = pm.id JOIN penduduk p ON pm.penduduk_id = p.id WHERE p.rt_rw = @rw AND p.kelurahan = @kel", conn);
                    cmd4.Parameters.AddWithValue("@rw", Session.WilayahRtRw);
                    cmd4.Parameters.AddWithValue("@kel", Session.WilayahKelurahan);
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
