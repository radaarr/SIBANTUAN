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

namespace SIBANTUAN.Forms.Petugas
{
    public partial class Laporan : Form
    {
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;
        private DataTable dataTable;

        public Laporan()
        {
            InitializeComponent();
        }

        private void Laporan_Load(object sender, EventArgs e)
        {
            LoadFilterData();
            LoadStatistics();
            LoadLaporanData();
        }

        private void LoadFilterData()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    // Load Program
                    program_cmb.Items.Clear();
                    program_cmb.Items.Add("Semua Program");
                    string queryProgram = "SELECT DISTINCT nama_program FROM program WHERE status = 'Aktif'";
                    MySqlDataAdapter adapterProgram = new MySqlDataAdapter(queryProgram, conn);
                    DataTable dtProgram = new DataTable();
                    adapterProgram.Fill(dtProgram);

                    foreach (DataRow row in dtProgram.Rows)
                    {
                        program_cmb.Items.Add(row["nama_program"].ToString());
                    }
                    program_cmb.SelectedIndex = 0;

                    // Load Periode
                    periode_cmb.Items.Clear();
                    periode_cmb.Items.Add("Semua Periode");
                    periode_cmb.Items.Add("Bulan Ini");
                    periode_cmb.Items.Add("3 Bulan Terakhir");
                    periode_cmb.Items.Add("6 Bulan Terakhir");
                    periode_cmb.Items.Add("Tahun Ini");
                    periode_cmb.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error memuat filter: " + ex.Message);
            }
        }

        private void LoadStatistics()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    // Total Warga Terdaftar
                    MySqlCommand cmd1 = new MySqlCommand("SELECT COUNT(*) FROM penduduk", conn);
                    lblCard1Value.Text = cmd1.ExecuteScalar().ToString();

                    // Warga Dapat Bantuan
                    MySqlCommand cmd2 = new MySqlCommand("SELECT COUNT(DISTINCT p.penduduk_id) FROM distribusi d JOIN permohonan p ON d.permohonan_id = p.id", conn);
                    lblCard2Value.Text = cmd2.ExecuteScalar().ToString();

                    // Permohonan Ditolak
                    MySqlCommand cmd3 = new MySqlCommand("SELECT COUNT(*) FROM permohonan WHERE status_permohonan = 'ditolak'", conn);
                    lblCard3Value.Text = cmd3.ExecuteScalar().ToString();

                    // Belum Pernah Dapat
                    MySqlCommand cmd4 = new MySqlCommand("SELECT COUNT(*) FROM penduduk WHERE id NOT IN (SELECT DISTINCT p.penduduk_id FROM distribusi d JOIN permohonan p ON d.permohonan_id = p.id)", conn);
                    lblCard4Value.Text = cmd4.ExecuteScalar().ToString();

                    // Total Anggaran Terpakai
                    MySqlCommand cmd5 = new MySqlCommand("SELECT COALESCE(SUM(jumlah_bantuan), 0) FROM distribusi", conn);
                    object result = cmd5.ExecuteScalar();
                    long totalAnggaran = Convert.ToInt64(result);
                    lblCard5Value.Text = "Rp\n" + string.Format("{0:N0}", totalAnggaran);

                    // Program Aktif
                    MySqlCommand cmd6 = new MySqlCommand("SELECT COUNT(*) FROM program WHERE status = 'Aktif'", conn);
                    int programCount = Convert.ToInt32(cmd6.ExecuteScalar());
                    lblCard6Value.Text = programCount + " Program";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error memuat statistik: " + ex.Message);
            }
        }

        private void LoadLaporanData()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT 
                                      d.id AS id_distribusi,
                                      pd.nama_lengkap,
                                      pb.nama_program,
                                      d.tanggal_distribusi,
                                      d.jumlah_bantuan,
                                      CASE 
                                        WHEN d.bentuk_bantuan = 'uang_tunai' THEN 'Uang Tunai'
                                        WHEN d.bentuk_bantuan = 'sembako' THEN 'Sembako'
                                        WHEN d.bentuk_bantuan = 'voucher' THEN 'Voucher'
                                      END AS bentuk_bantuan
                                   FROM distribusi d
                                   JOIN permohonan p ON d.permohonan_id = p.id
                                   JOIN penduduk pd ON p.penduduk_id = pd.id
                                   JOIN program_bantuan pb ON p.program_id = pb.id
                                   ORDER BY d.tanggal_distribusi DESC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    totalRecords = dataTable.Rows.Count;
                    currentPage = 1;
                    DisplayPage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error memuat data laporan: " + ex.Message);
            }
        }

        private void DisplayPage()
        {
            dataGridView1.Rows.Clear();

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                lblPagination.Text = "Halaman 1 dari 1";
                return;
            }

            int startIndex = (currentPage - 1) * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, dataTable.Rows.Count);
            int no = startIndex + 1;

            for (int i = startIndex; i < endIndex; i++)
            {
                DataRow row = dataTable.Rows[i];
                dataGridView1.Rows.Add(
                    no++,
                    row["nama_lengkap"].ToString(),
                    row["nama_program"].ToString(),
                    Convert.ToDateTime(row["tanggal_distribusi"]).ToString("dd/MM/yyyy"),
                    "Rp " + string.Format("{0:N0}", Convert.ToInt64(row["jumlah_bantuan"])),
                    row["bentuk_bantuan"].ToString()
                );
            }

            int totalPages = (totalRecords + pageSize - 1) / pageSize;
            lblPagination.Text = $"Halaman {currentPage} dari {totalPages}";
            btnFirstPage.Enabled = currentPage > 1;
            btnPrev.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;
            btnLastPage.Enabled = currentPage < totalPages;
        }

        private void tampilkan_bt_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadLaporanData();
        }

        private void cetak_bt_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Fungsi cetak belum diimplementasikan");
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            DisplayPage();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                DisplayPage();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int totalPages = (totalRecords + pageSize - 1) / pageSize;
            if (currentPage < totalPages)
            {
                currentPage++;
                DisplayPage();
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            int totalPages = (totalRecords + pageSize - 1) / pageSize;
            currentPage = totalPages;
            DisplayPage();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data untuk di-export");
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV Files (*.csv)|*.csv|Excel Files (*.xlsx)|*.xlsx";
                saveFileDialog.FileName = "Laporan_" + DateTime.Now.ToString("ddMMyyyy_HHmmss");

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCSV(saveFileDialog.FileName);
                    MessageBox.Show("Data berhasil di-export");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error export: " + ex.Message);
            }
        }

        private void ExportToCSV(string filePath)
        {
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                // Header
                sb.AppendLine("LAPORAN WILAYAH RT 001/RW 001 SUKAMAJU");
                sb.AppendLine();

                // Column Headers
                sb.AppendLine("No,Nama Warga,Program,Tanggal Distribusi,Jumlah,Bentuk Bantuan");

                // Data
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow dr = dataTable.Rows[i];
                    sb.Append((i + 1) + ",");
                    sb.Append("\"" + dr["nama_lengkap"].ToString() + "\",");
                    sb.Append("\"" + dr["nama_program"].ToString() + "\",");
                    sb.Append(Convert.ToDateTime(dr["tanggal_distribusi"]).ToString("dd/MM/yyyy") + ",");
                    sb.Append("Rp " + string.Format("{0:N0}", Convert.ToInt64(dr["jumlah_bantuan"])) + ",");
                    sb.AppendLine("\"" + dr["bentuk_bantuan"].ToString() + "\"");
                }

                System.IO.File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStatistics();
            LoadLaporanData();
            MessageBox.Show("Data telah diperbarui");
        }

        private void dashboard_bt_Click(object sender, EventArgs e) { new DashboardPetugas(0, "Petugas").ShowDialog(); }
        private void verifikasi_bt_Click(object sender, EventArgs e) { new Verifikasi().ShowDialog(); }
        private void permohonan_bt_Click(object sender, EventArgs e) { new Permohonan().ShowDialog(); }
        private void distribusi_bt_Click(object sender, EventArgs e) { new Distribusi().ShowDialog(); }
        private void laporan_bt_Click(object sender, EventArgs e) { /* already here */ }
        private void keluar_bt_Click(object sender, EventArgs e) { Application.Exit(); }
    }
}
