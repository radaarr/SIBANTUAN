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

namespace SIBANTUAN.Forms.Penerima
{
    public partial class RiwayatBantuan : Form
    {
        public RiwayatBantuan()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        // ================================================================
        // LOAD
        // ================================================================
        private void RiwayatBantuan_Load(object sender, EventArgs e)
        {
            SetupDGV();
            LoadRiwayat();
        }

        // ================================================================
        // 1. SETUP DataGridView
        // ================================================================
        private void SetupDGV()
        {
            dgvRiwayat.ReadOnly = true;
            dgvRiwayat.AllowUserToAddRows = false;
            dgvRiwayat.RowHeadersVisible = false;
            dgvRiwayat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRiwayat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRiwayat.BackgroundColor = Color.White;
            dgvRiwayat.BorderStyle = BorderStyle.FixedSingle;
            dgvRiwayat.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9f, FontStyle.Bold);
            dgvRiwayat.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgvRiwayat.RowTemplate.Height = 36;
        }

        // ================================================================
        // 2. LOAD DATA dari database
        // ================================================================
        private void LoadRiwayat()
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    // Ambil penduduk_id dulu
                    var cmdPid = new MySqlCommand(
                        "SELECT id FROM penduduk WHERE user_id = @uid", conn);
                    cmdPid.Parameters.AddWithValue("@uid", Session.UserId);
                    int pendudukId = Convert.ToInt32(cmdPid.ExecuteScalar());

                    if (pendudukId == 0)
                    {
                        MessageBox.Show(
                            "Data kependudukan belum tersedia.\n" +
                            "Hubungi Petugas RT/RW.",
                            "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Ambil riwayat distribusi yang sudah diterima
                    var cmd = new MySqlCommand(@"
                        SELECT
                            pb.nama_program       AS 'Program',
                            d.tanggal_distribusi  AS 'Tanggal',
                            d.bentuk_bantuan      AS 'Bentuk',
                            d.jumlah_bantuan      AS 'Jumlah',
                            d.keterangan          AS 'Keterangan'
                        FROM distribusi d
                        JOIN permohonan p   ON d.permohonan_id = p.id
                        JOIN program_bantuan pb ON p.program_id = pb.id
                        WHERE p.penduduk_id = @pid
                        ORDER BY d.tanggal_distribusi DESC", conn);
                    cmd.Parameters.AddWithValue("@pid", pendudukId);

                    var dt = new DataTable();
                    new MySqlDataAdapter(cmd).Fill(dt);

                    dgvRiwayat.DataSource = dt;

                    // Atur lebar kolom
                    if (dgvRiwayat.Columns["Program"] != null) dgvRiwayat.Columns["Program"].FillWeight = 30;
                    if (dgvRiwayat.Columns["Tanggal"] != null) dgvRiwayat.Columns["Tanggal"].FillWeight = 20;
                    if (dgvRiwayat.Columns["Bentuk"] != null) dgvRiwayat.Columns["Bentuk"].FillWeight = 15;
                    if (dgvRiwayat.Columns["Jumlah"] != null) dgvRiwayat.Columns["Jumlah"].FillWeight = 20;
                    if (dgvRiwayat.Columns["Keterangan"] != null) dgvRiwayat.Columns["Keterangan"].FillWeight = 30;

                    // Hitung statistik
                    int totalKali = dt.Rows.Count;
                    decimal totalNilai = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        // Jumlah hanya dihitung kalau bentuknya uang/voucher
                        string bentuk = row["Bentuk"].ToString();
                        if (bentuk == "uang_tunai" || bentuk == "voucher")
                        {
                            totalNilai += Convert.ToDecimal(row["Jumlah"]);
                        }
                    }

                    // Isi label statistik
                    // Ganti nama label sesuai yang ada di Designer kamu
                    lblTotalKali.Text = totalKali + " kali";
                    lblTotalNilai.Text = "Rp " + totalNilai.ToString("N0");

                    // Warna
                    lblTotalKali.ForeColor = Color.FromArgb(83, 74, 183);  // ungu
                    lblTotalNilai.ForeColor = Color.FromArgb(39, 80, 10);   // hijau

                    // Warnai baris berdasarkan bentuk bantuan
                    WarnaiBaris();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat riwayat:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================================================================
        // 3. WARNAI BARIS berdasarkan bentuk bantuan
        // ================================================================
        private void WarnaiBaris()
        {
            foreach (DataGridViewRow row in dgvRiwayat.Rows)
            {
                string bentuk = row.Cells["Bentuk"]?.Value?.ToString() ?? "";
                switch (bentuk)
                {
                    case "uang_tunai":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(234, 243, 222); // hijau muda
                        break;
                    case "sembako":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(238, 237, 254); // ungu muda
                        break;
                    case "voucher":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(250, 238, 218); // oranye muda
                        break;
                }
            }
        }

        // ================================================================
        // TOMBOL NAVIGASI SIDEBAR
        // ================================================================
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAjukanPermohonan_Click(object sender, EventArgs e)
        {
            var form = new AjukanPermohonan();
            form.ShowDialog();
        }

        private void btnStatusPermohonan_Click(object sender, EventArgs e)
        {
            var form = new StatusPermohonan();
            form.ShowDialog();
        }

        private void btnRiwayatBantuan_Click(object sender, EventArgs e)
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
