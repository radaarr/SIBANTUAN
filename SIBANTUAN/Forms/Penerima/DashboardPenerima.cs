using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIBANTUAN.Forms.Penerima
{
    public partial class DashboardPenerima : Form
    {
        // ── Simpan penduduk_id & NIK setelah load ──
        private int _pendudukId;
        private string _nik;

        public DashboardPenerima()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        // ================================================================
        // LOAD — dipanggil saat form pertama kali dibuka
        // ================================================================
        private void DashboardPenerima_Load(object sender, EventArgs e)
        {
            LoadDataPenduduk();
            LoadStatistik();
            LoadPermohonanTerbaru();
        }
        // ================================================================
        // 1. ISI DATA PENDUDUK (Selamat datang, NIK, Data Kependudukan)
        // ================================================================
        private void LoadDataPenduduk()
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    var cmd = new MySqlCommand(@"
                        SELECT p.id, p.nik, p.nama_lengkap,
                               p.alamat, p.rt_rw, p.kelurahan,
                               p.status_ekonomi
                        FROM penduduk p
                        WHERE p.user_id = @uid", conn);

                    // Ambil user_id dari Session (set waktu login)
                    cmd.Parameters.AddWithValue("@uid", Session.UserId);

                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            _pendudukId = Convert.ToInt32(r["id"]);
                            _nik = r["nik"].ToString();

                            // ── Selamat datang & NIK ──
                            // Ganti "lblWelcome" dengan nama label "Selamat datang," milikmu
                            lblWelcome.Text = $"Selamat datang, {r["nama_lengkap"]}";
                            lblNIK.Text = $"NIK: {r["nik"]}  RT/RW {r["rt_rw"]}";

                            // ── Data Kependudukan ──
                            lblAlamat.Text = r["alamat"].ToString();
                            lblStatusEkonomi.Text = FormatStatusEkonomi(r["status_ekonomi"].ToString());
                            lblKelayakan.Text = "Layak";
                            lblKelayakan.BackColor = Color.FromArgb(0, 188, 212);
                            lblKelayakan.ForeColor = Color.White;
                        }
                        else
                        {
                            lblWelcome.Text = "Selamat datang,";
                            lblNIK.Text = "Data kependudukan belum tersedia";
                            lblAlamat.Text = "-";
                            lblStatusEkonomi.Text = "-";
                            lblKelayakan.Text = "Belum terdaftar";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memuat data penduduk:\n{ex.Message}\n\nDebug: UserId={Session.UserId}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================================================================
        // 2. ISI STATISTIK (Total Diterima, Sedang Diproses, Ditolak)
        // ================================================================
        private void LoadStatistik()
        {
            if (Session.UserId == null || Session.UserId == 0) return;

            if (_pendudukId == 0) return;

            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    var cmd = new MySqlCommand(@"
                        SELECT
                          SUM(CASE WHEN status_permohonan = 'disetujui' THEN 1 ELSE 0 END) AS diterima,
                          SUM(CASE WHEN status_permohonan = 'pending'   THEN 1 ELSE 0 END) AS diproses,
                          SUM(CASE WHEN status_permohonan = 'ditolak'   THEN 1 ELSE 0 END) AS ditolak
                        FROM permohonan
                        WHERE penduduk_id = @pid", conn);
                    cmd.Parameters.AddWithValue("@pid", _pendudukId);

                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            // Ganti nama label sesuai kontrol angka di Designer kamu
                            lblTotalDiterima.Text = r["diterima"] == DBNull.Value ? "0" : r["diterima"].ToString();
                            lblSedangDiproses.Text = r["diproses"] == DBNull.Value ? "0" : r["diproses"].ToString();
                            lblDitolak.Text = r["ditolak"] == DBNull.Value ? "0" : r["ditolak"].ToString();

                            // Warna angka
                            lblTotalDiterima.ForeColor = Color.FromArgb(83, 74, 183);   // ungu
                            lblSedangDiproses.ForeColor = Color.FromArgb(180, 100, 20);  // oranye
                            lblDitolak.ForeColor = Color.FromArgb(160, 40, 40);   // merah
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memuat statistik:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================================================================
        // 3. ISI TABEL PERMOHONAN TERBARU (DataGridView)
        // ================================================================
        private void LoadPermohonanTerbaru()
        {
            if (Session.UserId == null || Session.UserId == 0) return;

            if (_pendudukId == 0) return;

            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    var cmd = new MySqlCommand(@"
                        SELECT pb.nama_program  AS 'Program Bantuan',
                               p.status_permohonan AS 'Status'
                        FROM permohonan p
                        JOIN program_bantuan pb ON p.program_id = pb.id
                        WHERE p.penduduk_id = @pid
                        ORDER BY p.created_at DESC
                        LIMIT 5", conn);
                    cmd.Parameters.AddWithValue("@pid", _pendudukId);

                    var dt = new DataTable();
                    new MySqlDataAdapter(cmd).Fill(dt);

                    // Ganti "dataGridView1" dengan nama DGV milikmu
                    dgvPermohonanTerbaru.DataSource = dt;
                    dgvPermohonanTerbaru.AllowUserToAddRows = false;
                    dgvPermohonanTerbaru.ReadOnly = true;
                    dgvPermohonanTerbaru.RowHeadersVisible = false;
                    dgvPermohonanTerbaru.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvPermohonanTerbaru.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgvPermohonanTerbaru.BackgroundColor = Color.White;

                    // Warnai baris berdasarkan status
                    foreach (DataGridViewRow row in dgvPermohonanTerbaru.Rows)
                    {
                        var status = row.Cells["Status"]?.Value?.ToString();
                        switch (status)
                        {
                            case "disetujui":
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(39, 80, 10);
                                break;
                            case "ditolak":
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(160, 40, 40);
                                break;
                            default: // pending
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(180, 100, 20);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memuat permohonan:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================================================================
        // TOMBOL NAVIGASI SIDEBAR
        // ================================================================

        // Ganti nama method sesuai nama button di Designer kamu
        private void btnAjukanPermohonan_Click(object sender, EventArgs e)
        {
            var form = new AjukanPermohonan();
            form.ShowDialog();
            // Refresh data setelah kembali dari form AjukanPermohonan
            LoadStatistik();
            LoadPermohonanTerbaru();
        }

        private void btnStatusPermohonan_Click(object sender, EventArgs e)
        {
            var form = new StatusPermohonan();
            form.ShowDialog();
        }

        private void btnRiwayatBantuan_Click(object sender, EventArgs e)
        {
            var form = new RiwayatBantuan();
            form.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var konfirmasi = MessageBox.Show(
                "Yakin ingin logout?", "Konfirmasi Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (konfirmasi == DialogResult.Yes)
            {
                Session.UserId = 0;
                Session.Username = null;
                Session.Role = null;
                this.Hide();
                // Buka form login milik Daffa
                new Form1().Show();
            }
        }

        // ================================================================
        // EVENT BAWAAN (biarkan kosong atau hapus kalau tidak dipakai)
        // ================================================================
        private void label4_Click(object sender, EventArgs e) { }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        // ================================================================
        // HELPER
        // ================================================================
        private string FormatStatusEkonomi(string status)
        {
            switch (status)
            {
                case "sangat_miskin": return "Sangat miskin";
                case "miskin": return "Miskin";
                case "rentan": return "Rentan";
                default: return status;
            }
        }
    }
}
