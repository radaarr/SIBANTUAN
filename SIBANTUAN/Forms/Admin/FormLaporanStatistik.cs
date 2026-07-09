using System;
using System.Drawing;
using System.Windows.Forms;

namespace SIBANTUAN.Forms.Admin
{
    public partial class FormLaporanStatistik : Form
    {
        // ==========================================
        // DEKLARASI WARNA TEMA
        // ==========================================
        private Color warnaAksen = Color.FromArgb(52, 152, 219);       // Biru untuk header tabel
        private Color warnaPilih = Color.FromArgb(230, 242, 255);      // Biru muda untuk baris yang diklik

        public FormLaporanStatistik()
        {
            InitializeComponent();
            StyleTabelAtRuntime(); // Menjalankan fungsi polesan tabel
            LoadDataDummy();       // Memuat data sementara
        }

        // ==========================================
        // 1. MEMPERCANTIK TABEL (Melengkapi Desainer)
        // ==========================================
        private void StyleTabelAtRuntime()
        {
            // Pengaturan tambahan agar tabel terlihat selaras dengan form Kelola User
            dgvLaporan.EnableHeadersVisualStyles = false;
            dgvLaporan.GridColor = Color.LightGray;
            dgvLaporan.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // Header Tabel
            dgvLaporan.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvLaporan.ColumnHeadersDefaultCellStyle.BackColor = warnaAksen;
            dgvLaporan.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLaporan.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            // Baris Tabel
            dgvLaporan.DefaultCellStyle.SelectionBackColor = warnaPilih;
            dgvLaporan.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvLaporan.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
        }

        // ==========================================
        // 2. MENGISI DATA SEMENTARA
        // ==========================================
        private void LoadDataDummy()
        {
            dgvLaporan.ColumnCount = 5;
            dgvLaporan.Columns[0].Name = "ID Penyaluran";
            dgvLaporan.Columns[1].Name = "Tanggal";
            dgvLaporan.Columns[2].Name = "Nama Program Bantuan";
            dgvLaporan.Columns[3].Name = "Jumlah Penerima";
            dgvLaporan.Columns[4].Name = "Total Dana (Rp)";

            dgvLaporan.Rows.Add("TRX-001", "01-07-2026", "Bantuan Sembako Warga", "150 Keluarga", "30.000.000");
            dgvLaporan.Rows.Add("TRX-002", "05-07-2026", "Bantuan Tunai BLT", "75 Orang", "45.000.000");
            dgvLaporan.Rows.Add("TRX-003", "08-07-2026", "Bedah Rumah Desa", "5 Keluarga", "100.000.000");
            dgvLaporan.Rows.Add("TRX-004", "10-07-2026", "Bantuan Alat Usaha", "20 UMKM", "50.000.000");

            dgvLaporan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLaporan.ClearSelection();
        }

        // ==========================================
        // 3. FUNGSI KLIK TOMBOL EXPORT
        // ==========================================
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvLaporan.Rows.Count > 0)
            {
                MessageBox.Show("Data laporan sedang diproses untuk diexport ke format Excel (.xlsx)...",
                                "Export Berhasil",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Tidak ada data laporan yang bisa diexport saat ini.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}