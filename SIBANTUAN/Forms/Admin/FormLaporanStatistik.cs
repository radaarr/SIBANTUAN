using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SIBANTUAN.Forms.Admin
{
    public partial class FormLaporanStatistik : Form
    {
        private Color warnaAksen = Color.FromArgb(52, 152, 219);
        private Color warnaPilih = Color.FromArgb(230, 242, 255);
        private DataTable dataTable;

        public FormLaporanStatistik()
        {
            InitializeComponent();
            StyleTabelAtRuntime();
            LoadDataFromDatabase();
        }

        private void StyleTabelAtRuntime()
        {
            dgvLaporan.EnableHeadersVisualStyles = false;
            dgvLaporan.GridColor = Color.LightGray;
            dgvLaporan.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            dgvLaporan.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvLaporan.ColumnHeadersDefaultCellStyle.BackColor = warnaAksen;
            dgvLaporan.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLaporan.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            dgvLaporan.DefaultCellStyle.SelectionBackColor = warnaPilih;
            dgvLaporan.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvLaporan.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                          d.id,
                          d.tanggal_distribusi,
                          pb.nama_program,
                          p.nama_lengkap AS penerima,
                          d.jumlah_bantuan,
                          d.bentuk_bantuan
                        FROM distribusi d
                        JOIN permohonan pm ON d.permohonan_id = pm.id
                        JOIN penduduk p ON pm.penduduk_id = p.id
                        JOIN program_bantuan pb ON pm.program_id = pb.id
                        ORDER BY d.tanggal_distribusi DESC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dgvLaporan.ColumnCount = 6;
                    dgvLaporan.Columns[0].Name = "ID";
                    dgvLaporan.Columns[1].Name = "Tanggal";
                    dgvLaporan.Columns[2].Name = "Program Bantuan";
                    dgvLaporan.Columns[3].Name = "Penerima";
                    dgvLaporan.Columns[4].Name = "Jumlah (Rp)";
                    dgvLaporan.Columns[5].Name = "Bentuk";

                    dgvLaporan.Rows.Clear();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        DateTime tgl = Convert.ToDateTime(row["tanggal_distribusi"]);
                        decimal jumlah = row["jumlah_bantuan"] != DBNull.Value ? Convert.ToDecimal(row["jumlah_bantuan"]) : 0;
                        dgvLaporan.Rows.Add(
                            row["id"].ToString(),
                            tgl.ToString("dd/MM/yyyy"),
                            row["nama_program"].ToString(),
                            row["penerima"].ToString(),
                            "Rp " + string.Format("{0:N0}", jumlah),
                            row["bentuk_bantuan"].ToString()
                        );
                    }
                    dgvLaporan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvLaporan.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error memuat laporan: " + ex.Message);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvLaporan.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data laporan yang bisa diexport.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "CSV Files (*.csv)|*.csv";
            save.FileName = "Laporan_Statistik_" + DateTime.Now.ToString("ddMMyyyy");

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("LAPORAN STATISTIK SIBANTUAN");
                    sb.AppendLine("Tanggal: " + DateTime.Now.ToString("dd MMMM yyyy"));
                    sb.AppendLine();
                    sb.AppendLine("ID,Tanggal,Program,Penerima,Jumlah,Bentuk");

                    foreach (DataRow row in dataTable.Rows)
                    {
                        DateTime tgl = Convert.ToDateTime(row["tanggal_distribusi"]);
                        decimal jumlah = row["jumlah_bantuan"] != DBNull.Value ? Convert.ToDecimal(row["jumlah_bantuan"]) : 0;
                        sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5}",
                            row["id"], tgl.ToString("dd/MM/yyyy"), row["nama_program"], row["penerima"], jumlah, row["bentuk_bantuan"]));
                    }

                    System.IO.File.WriteAllText(save.FileName, sb.ToString());
                    MessageBox.Show("Data berhasil diexport!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error export: " + ex.Message);
                }
            }
        }
    }
}
