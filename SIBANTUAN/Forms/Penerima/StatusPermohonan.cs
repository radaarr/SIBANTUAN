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
    public partial class StatusPermohonan : Form
    {
        public StatusPermohonan()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        // ================================================================
        // LOAD
        // ================================================================
        private void StatusPermohonan_Load(object sender, EventArgs e)
        {
            SetupDGV();
            LoadStatus();
        }

        // ================================================================
        // 1. SETUP KOLOM DataGridView
        // ================================================================
        private void SetupDGV()
        {
            dgvStatus.ReadOnly = true;
            dgvStatus.AllowUserToAddRows = false;
            dgvStatus.RowHeadersVisible = false;
            dgvStatus.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStatus.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStatus.BackgroundColor = Color.White;
            dgvStatus.BorderStyle = BorderStyle.FixedSingle;
            dgvStatus.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9f, FontStyle.Bold);
            dgvStatus.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgvStatus.RowTemplate.Height = 40;

            // Event klik baris — tampilkan detail di bawah
            dgvStatus.SelectionChanged += DgvStatus_SelectionChanged;
        }

        // ================================================================
        // 2. LOAD DATA dari database
        // ================================================================
        private void LoadStatus()
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

                    var cmd = new MySqlCommand(@"
                        SELECT
                            p.id,
                            pb.nama_program      AS 'Program',
                            p.tanggal_pengajuan  AS 'Tgl Ajuan',
                            p.status_permohonan  AS 'Status',
                            p.alasan_tolak,
                            p.catatan_petugas,
                            p.verified_at        AS 'Diverifikasi'
                        FROM permohonan p
                        JOIN program_bantuan pb ON p.program_id = pb.id
                        WHERE p.penduduk_id = @pid
                        ORDER BY p.created_at DESC", conn);
                    cmd.Parameters.AddWithValue("@pid", pendudukId);

                    var dt = new DataTable();
                    new MySqlDataAdapter(cmd).Fill(dt);

                    dgvStatus.DataSource = dt;

                    // Sembunyikan kolom yang tidak perlu ditampilkan
                    if (dgvStatus.Columns["id"] != null) dgvStatus.Columns["id"].Visible = false;
                    if (dgvStatus.Columns["alasan_tolak"] != null) dgvStatus.Columns["alasan_tolak"].Visible = false;
                    if (dgvStatus.Columns["catatan_petugas"] != null) dgvStatus.Columns["catatan_petugas"].Visible = false;

                    // Atur lebar kolom
                    if (dgvStatus.Columns["Program"] != null) dgvStatus.Columns["Program"].FillWeight = 40;
                    if (dgvStatus.Columns["Tgl Ajuan"] != null) dgvStatus.Columns["Tgl Ajuan"].FillWeight = 20;
                    if (dgvStatus.Columns["Status"] != null) dgvStatus.Columns["Status"].FillWeight = 20;
                    if (dgvStatus.Columns["Diverifikasi"] != null) dgvStatus.Columns["Diverifikasi"].FillWeight = 20;

                    WarnaiStatus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================================================================
        // 3. WARNAI BARIS berdasarkan status
        // ================================================================
        private void WarnaiStatus()
        {
            foreach (DataGridViewRow row in dgvStatus.Rows)
            {
                var status = row.Cells["Status"]?.Value?.ToString();
                switch (status)
                {
                    case "disetujui":
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(39, 80, 10);
                        row.DefaultCellStyle.BackColor = Color.FromArgb(234, 243, 222);
                        break;
                    case "ditolak":
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(160, 40, 40);
                        row.DefaultCellStyle.BackColor = Color.FromArgb(252, 235, 235);
                        break;
                    default: // pending
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(180, 100, 20);
                        row.DefaultCellStyle.BackColor = Color.FromArgb(250, 238, 218);
                        break;
                }
            }
        }


        // ================================================================
        // 4. KLIK BARIS — tampilkan detail (alasan tolak / catatan)
        // ================================================================
        private void DgvStatus_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStatus.SelectedRows.Count == 0) return;

            var row = dgvStatus.SelectedRows[0];
            string status = row.Cells["Status"]?.Value?.ToString() ?? "";
            string alasan = row.Cells["alasan_tolak"]?.Value?.ToString() ?? "";
            string catatan = row.Cells["catatan_petugas"]?.Value?.ToString() ?? "";

            // Tampilkan info di lblDetail (label di bawah DataGridView)
            switch (status)
            {
                case "ditolak":
                    lblDetail.Visible = true;
                    lblDetail.Text = "❌ Alasan ditolak: " +
                        (string.IsNullOrWhiteSpace(alasan) ? "(tidak ada keterangan)" : alasan);
                    lblDetail.BackColor = Color.FromArgb(252, 235, 235);
                    lblDetail.ForeColor = Color.FromArgb(160, 40, 40);
                    break;

                case "disetujui":
                    lblDetail.Visible = true;
                    lblDetail.Text = "✅ Permohonan disetujui! Bantuan akan segera disalurkan oleh Petugas RT/RW." +
                        (string.IsNullOrWhiteSpace(catatan) ? "" : "\n📝 Catatan: " + catatan);
                    lblDetail.BackColor = Color.FromArgb(234, 243, 222);
                    lblDetail.ForeColor = Color.FromArgb(39, 80, 10);
                    break;

                case "pending":
                    lblDetail.Visible = true;
                    lblDetail.Text = "🕐 Permohonan sedang menunggu verifikasi dari Petugas RT/RW.";
                    lblDetail.BackColor = Color.FromArgb(250, 238, 218);
                    lblDetail.ForeColor = Color.FromArgb(180, 100, 20);
                    break;

                default:
                    lblDetail.Visible = false;
                    break;
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
        }

        private void btnRiwayatBantuan_Click(object sender, EventArgs e)
        {
            var form = new RiwayatBantuan();
            form.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
