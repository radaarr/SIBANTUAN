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
            try
            {
                // Form sizing sudah dikonfigurasi di Designer (FixedSingle, CenterScreen)
                // FormHelper.SetFullscreenMode(this);

                Panel footerPanel = this.Controls["pnlFooter"] as Panel;
                FormHelper.SetPanelDocking(panel1, null, footerPanel);
                FormHelper.SetDataGridViewResponsive(dataGridView1);

                // Set Anchor untuk responsiveness
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is Panel || ctrl is DataGridView)
                    {
                        ctrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    }
                }

                label3.Text = Session.Nama;
                label4.Text = Session.WilayahRtRw + " - " + Session.WilayahKelurahan;
                LoadFilterData();
                LoadStatistics();
                LoadLaporanData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error di Laporan_Load: " + ex.Message);
            }
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
                    string queryProgram = "SELECT DISTINCT nama_program FROM program_bantuan WHERE status_program = 'aktif'";
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

                    string rw = Session.WilayahRtRw;
                    string kel = Session.WilayahKelurahan;

                    // Total Warga Terdaftar
                    MySqlCommand cmd1 = new MySqlCommand("SELECT COUNT(*) FROM penduduk WHERE rt_rw = @rw AND kelurahan = @kel", conn);
                    cmd1.Parameters.AddWithValue("@rw", rw);
                    cmd1.Parameters.AddWithValue("@kel", kel);
                    object result1 = cmd1.ExecuteScalar();
                    lblCard1Value.Text = result1 != null ? result1.ToString() : "0";

                    // Warga Dapat Bantuan (penduduk yang sudah menerima distribusi)
                    MySqlCommand cmd2 = new MySqlCommand("SELECT COUNT(DISTINCT pm.penduduk_id) FROM distribusi d JOIN permohonan pm ON d.permohonan_id = pm.id JOIN penduduk p ON pm.penduduk_id = p.id WHERE p.rt_rw = @rw2 AND p.kelurahan = @kel2", conn);
                    cmd2.Parameters.AddWithValue("@rw2", rw);
                    cmd2.Parameters.AddWithValue("@kel2", kel);
                    object result2 = cmd2.ExecuteScalar();
                    lblCard2Value.Text = result2 != null ? result2.ToString() : "0";

                    // Permohonan Ditolak (wilayah)
                    MySqlCommand cmd3 = new MySqlCommand("SELECT COUNT(*) FROM permohonan pm JOIN penduduk p ON pm.penduduk_id = p.id WHERE pm.status_permohonan = 'ditolak' AND p.rt_rw = @rw3 AND p.kelurahan = @kel3", conn);
                    cmd3.Parameters.AddWithValue("@rw3", rw);
                    cmd3.Parameters.AddWithValue("@kel3", kel);
                    object result3 = cmd3.ExecuteScalar();
                    lblCard3Value.Text = result3 != null ? result3.ToString() : "0";

                    // Belum Pernah Dapat (penduduk yang belum menerima distribusi)
                    MySqlCommand cmd4 = new MySqlCommand("SELECT COUNT(*) FROM penduduk p WHERE p.rt_rw = @rw4 AND p.kelurahan = @kel4 AND p.id NOT IN (SELECT pm.penduduk_id FROM distribusi d JOIN permohonan pm ON d.permohonan_id = pm.id)", conn);
                    cmd4.Parameters.AddWithValue("@rw4", rw);
                    cmd4.Parameters.AddWithValue("@kel4", kel);
                    object result4 = cmd4.ExecuteScalar();
                    lblCard4Value.Text = result4 != null ? result4.ToString() : "0";

                    // Total Anggaran Terpakai (wilayah)
                    MySqlCommand cmd5 = new MySqlCommand("SELECT COALESCE(SUM(d.jumlah_bantuan), 0) FROM distribusi d JOIN permohonan pm ON d.permohonan_id = pm.id JOIN penduduk p ON pm.penduduk_id = p.id WHERE p.rt_rw = @rw5 AND p.kelurahan = @kel5", conn);
                    cmd5.Parameters.AddWithValue("@rw5", rw);
                    cmd5.Parameters.AddWithValue("@kel5", kel);
                    object result5 = cmd5.ExecuteScalar();
                    long totalAnggaran = result5 != null ? Convert.ToInt64(result5) : 0;
                    lblCard5Value.Text = "Rp " + string.Format("{0:N0}", totalAnggaran);

                    // Program Aktif (global — tidak perlu filter wilayah)
                    MySqlCommand cmd6 = new MySqlCommand("SELECT COUNT(*) FROM program_bantuan WHERE status_program = 'aktif'", conn);
                    object result6 = cmd6.ExecuteScalar();
                    int programCount = result6 != null ? Convert.ToInt32(result6) : 0;
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
                                      d.id,
                                      p.nama_lengkap,
                                      pb.nama_program,
                                      d.tanggal_distribusi,
                                      d.jumlah_bantuan,
                                      d.bentuk_bantuan
                                   FROM distribusi d
                                   JOIN permohonan pm ON d.permohonan_id = pm.id
                                   JOIN penduduk p ON pm.penduduk_id = p.id
                                   JOIN program_bantuan pb ON pm.program_id = pb.id
                                   WHERE p.rt_rw = @rw AND p.kelurahan = @kel";

                    string selectedProgram = program_cmb.SelectedItem?.ToString() ?? "Semua Program";
                    string selectedPeriode = periode_cmb.SelectedItem?.ToString() ?? "Semua Periode";

                    if (selectedProgram != "Semua Program")
                    {
                        query += " AND pb.nama_program = @program";
                    }

                    if (selectedPeriode == "Bulan Ini")
                    {
                        query += " AND d.tanggal_distribusi >= DATE_FORMAT(CURDATE(), '%Y-%m-01')";
                    }
                    else if (selectedPeriode == "3 Bulan Terakhir")
                    {
                        query += " AND d.tanggal_distribusi >= DATE_SUB(CURDATE(), INTERVAL 3 MONTH)";
                    }
                    else if (selectedPeriode == "6 Bulan Terakhir")
                    {
                        query += " AND d.tanggal_distribusi >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)";
                    }
                    else if (selectedPeriode == "Tahun Ini")
                    {
                        query += " AND d.tanggal_distribusi >= DATE_FORMAT(CURDATE(), '%Y-01-01')";
                    }

                    query += " ORDER BY d.tanggal_distribusi DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@rw", Session.WilayahRtRw);
                    cmd.Parameters.AddWithValue("@kel", Session.WilayahKelurahan);
                    if (selectedProgram != "Semua Program")
                    {
                        cmd.Parameters.AddWithValue("@program", selectedProgram);
                    }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
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
                sb.AppendLine("LAPORAN WILAYAH " + Session.WilayahRtRw + " " + Session.WilayahKelurahan.ToUpper());
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

        private void dashboard_bt_Click(object sender, EventArgs e) 
        { 
            try
            {
                if (DashboardPetugas.Instance != null)
                {
                    DashboardPetugas.Instance.ShowContentControls();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Dashboard tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void verifikasi_bt_Click(object sender, EventArgs e) 
        { 
            try
            {
                if (DashboardPetugas.Instance != null)
                {
                    Verifikasi form = new Verifikasi();
                    DashboardPetugas.Instance.ShowFormInContent(form);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Dashboard tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void permohonan_bt_Click(object sender, EventArgs e) 
        { 
            try
            {
                if (DashboardPetugas.Instance != null)
                {
                    Permohonan form = new Permohonan();
                    DashboardPetugas.Instance.ShowFormInContent(form);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Dashboard tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void distribusi_bt_Click(object sender, EventArgs e) 
        { 
            try
            {
                if (DashboardPetugas.Instance != null)
                {
                    Distribusi form = new Distribusi();
                    DashboardPetugas.Instance.ShowFormInContent(form);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Dashboard tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void laporan_bt_Click(object sender, EventArgs e) { /* already here */ }
        private void keluar_bt_Click(object sender, EventArgs e) { Application.Exit(); }

        private void lblFooter_Click(object sender, EventArgs e)
        {

        }

        private void pnlStatistik_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblCard6Value_Click(object sender, EventArgs e)
        {

        }

        private void lblStatistikTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
