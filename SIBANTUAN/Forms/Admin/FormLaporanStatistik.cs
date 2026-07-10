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
        private DataGridView dgvWilayah;
        private Label lblTotalPenerima;
        private Label lblTotalRealisasi;
        private Label lblTotalDistribusi;

        public FormLaporanStatistik()
        {
            InitializeComponent();
            this.AutoScroll = true;
            BuatKartuMetrik();
            StyleTabelAtRuntime();
            LoadDataFromDatabase();
            LoadDataWilayah();
        }

        private void BuatKartuMetrik()
        {
            // Labels untuk menampilkan hasil query (diupdate setelah LoadDataFromDatabase)
            lblTotalPenerima = new Label();
            lblTotalRealisasi = new Label();
            lblTotalDistribusi = new Label();
        }

        private Panel BuatKartu(string judul, Label lblNilai, Color warna, int x, int y)
        {
            Panel card = new Panel();
            card.Size = new Size(200, 90);
            card.Location = new Point(x, y);
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;

            Panel header = new Panel();
            header.Size = new Size(200, 5);
            header.Dock = DockStyle.Top;
            header.BackColor = warna;
            card.Controls.Add(header);

            Label lblJudul = new Label();
            lblJudul.Text = judul;
            lblJudul.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblJudul.ForeColor = Color.Gray;
            lblJudul.Location = new Point(12, 18);
            lblJudul.AutoSize = true;
            card.Controls.Add(lblJudul);

            lblNilai.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblNilai.ForeColor = Color.Black;
            lblNilai.Location = new Point(12, 42);
            lblNilai.AutoSize = true;
            card.Controls.Add(lblNilai);

            return card;
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

        private void StyleWilayahTabel(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.GridColor = Color.LightGray;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 242, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    // ── METRIK ──
                    MySqlCommand cmdPenerima = new MySqlCommand(
                        "SELECT COUNT(DISTINCT pm.penduduk_id) FROM permohonan pm WHERE pm.status_permohonan = 'disetujui'", conn);
                    int totalPenerima = Convert.ToInt32(cmdPenerima.ExecuteScalar());

                    MySqlCommand cmdRealisasi = new MySqlCommand(
                        "SELECT COALESCE(SUM(d.jumlah_bantuan), 0) FROM distribusi d", conn);
                    decimal realisasi = Convert.ToDecimal(cmdRealisasi.ExecuteScalar());

                    MySqlCommand cmdDistribusi = new MySqlCommand(
                        "SELECT COUNT(*) FROM distribusi", conn);
                    int totalDistribusi = Convert.ToInt32(cmdDistribusi.ExecuteScalar());

                    // ── Tampilkan kartu metrik ──
                    lblTotalPenerima.Text = totalPenerima + " Jiwa";
                    lblTotalRealisasi.Text = "Rp " + string.Format("{0:N0}", realisasi);
                    lblTotalDistribusi.Text = totalDistribusi + " Kali";

                    this.Controls.Add(BuatKartu("Total Penerima", lblTotalPenerima, Color.FromArgb(46, 204, 113), 20, 60));
                    this.Controls.Add(BuatKartu("Realisasi Anggaran", lblTotalRealisasi, Color.FromArgb(52, 152, 219), 240, 60));
                    this.Controls.Add(BuatKartu("Distribusi Tercatat", lblTotalDistribusi, Color.FromArgb(155, 89, 182), 460, 60));

                            // ── Reposisi tombol export ──
                    btnExport.Location = new Point(20, 168);

                    // ── DATA DISTRIBUSI ──
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
                    dgvLaporan.Location = new Point(20, 175);
                    dgvLaporan.Size = new Size(720, 220);
                    dgvLaporan.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    dgvLaporan.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error memuat laporan: " + ex.Message);
            }
        }

        private void LoadDataWilayah()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    string queryWilayah = @"
                        SELECT 
                          p.kelurahan,
                          p.rt_rw,
                          COUNT(DISTINCT pm.penduduk_id) AS penerima,
                          COUNT(d.id) AS distribusi,
                          COALESCE(SUM(d.jumlah_bantuan), 0) AS total_bantuan
                        FROM penduduk p
                        LEFT JOIN permohonan pm ON p.id = pm.penduduk_id AND pm.status_permohonan = 'disetujui'
                        LEFT JOIN distribusi d ON d.permohonan_id = pm.id
                        GROUP BY p.kelurahan, p.rt_rw
                        ORDER BY p.kelurahan, p.rt_rw";

                    MySqlDataAdapter adapterWilayah = new MySqlDataAdapter(queryWilayah, conn);
                    DataTable dtWilayah = new DataTable();
                    adapterWilayah.Fill(dtWilayah);

                    Label lblWilayahTitle = new Label();
                    lblWilayahTitle.Text = "Statistik per Wilayah";
                    lblWilayahTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                    lblWilayahTitle.ForeColor = Color.FromArgb(44, 62, 80);
                    lblWilayahTitle.AutoSize = true;
                    lblWilayahTitle.Location = new Point(20, 415);
                    this.Controls.Add(lblWilayahTitle);

                    dgvWilayah = new DataGridView();
                    dgvWilayah.Location = new Point(20, 445);
                    dgvWilayah.Size = new Size(720, 200);
                    dgvWilayah.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    dgvWilayah.DataSource = dtWilayah;
                    StyleWilayahTabel(dgvWilayah);
                    this.Controls.Add(dgvWilayah);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error memuat data wilayah: " + ex.Message);
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
                    sb.AppendLine("=== RINGKASAN ===");
                    sb.AppendLine("Total Penerima," + lblTotalPenerima.Text);
                    sb.AppendLine("Realisasi Anggaran," + lblTotalRealisasi.Text);
                    sb.AppendLine("Total Distribusi," + lblTotalDistribusi.Text);
                    sb.AppendLine();
                    sb.AppendLine("=== DETAIL DISTRIBUSI ===");
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
