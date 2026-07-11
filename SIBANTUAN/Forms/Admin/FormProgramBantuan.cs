using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace SIBANTUAN.Forms.Admin
{
    public partial class FormProgramBantuan : Form
    {
        private DataTable dtProgram;

        public FormProgramBantuan()
        {
            InitializeComponent();
            LoadDataFromDatabase();
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                // Terhubung ke database 'sibantuan' melalui DBHelper
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT id, nama_program, deskripsi, kuota_penerima, status_program FROM program_bantuan ORDER BY id";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    dtProgram = new DataTable();
                    adapter.Fill(dtProgram);

                    dgvProgram.DataSource = null;
                    dgvProgram.Columns.Clear();
                    dgvProgram.ColumnCount = 4;
                    dgvProgram.Columns[0].Name = "ID";
                    dgvProgram.Columns[1].Name = "Nama Program";
                    dgvProgram.Columns[2].Name = "Deskripsi";
                    dgvProgram.Columns[3].Name = "Status";

                    dgvProgram.Rows.Clear();
                    foreach (DataRow row in dtProgram.Rows)
                    {
                        string status = row["status_program"].ToString().ToLower() == "aktif" ? "Aktif" : "Nonaktif";
                        dgvProgram.Rows.Add(row["id"].ToString(), row["nama_program"].ToString(), row["deskripsi"].ToString(), status);
                    }
                    dgvProgram.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvProgram.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgvProgram.MultiSelect = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat memuat data: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTambah_Click_1(object sender, EventArgs e)
        {
            string nama = Microsoft.VisualBasic.Interaction.InputBox("Nama Program:", "Tambah Program", "");
            if (string.IsNullOrEmpty(nama)) return;

            string deskripsi = Microsoft.VisualBasic.Interaction.InputBox("Deskripsi:", "Tambah Program", "");
            string kuota = Microsoft.VisualBasic.Interaction.InputBox("Kuota Penerima:", "Tambah Program", "100");
            string anggaran = Microsoft.VisualBasic.Interaction.InputBox("Total Anggaran (Rp):", "Tambah Program", "0");

            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO program_bantuan (nama_program, deskripsi, kuota_penerima, anggaran_total, status_program, periode_mulai, periode_selesai) VALUES (@nama, @desk, @kuota, @anggaran, 'aktif', CURDATE(), DATE_ADD(CURDATE(), INTERVAL 1 YEAR))";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nama", nama);
                    cmd.Parameters.AddWithValue("@desk", deskripsi);
                    cmd.Parameters.AddWithValue("@kuota", int.TryParse(kuota, out int k) ? k : 100);
                    cmd.Parameters.AddWithValue("@anggaran", decimal.TryParse(anggaran, out decimal a) ? a : 0);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Program berhasil ditambahkan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataFromDatabase();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            if (dgvProgram.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih program terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string id = dgvProgram.SelectedRows[0].Cells["ID"].Value.ToString();
            string namaLama = dgvProgram.SelectedRows[0].Cells["Nama Program"].Value.ToString();
            string deskLama = dgvProgram.SelectedRows[0].Cells["Deskripsi"].Value.ToString();

            string namaBaru = Microsoft.VisualBasic.Interaction.InputBox("Nama Program:", "Edit Program", namaLama);
            if (string.IsNullOrEmpty(namaBaru)) return;
            string deskBaru = Microsoft.VisualBasic.Interaction.InputBox("Deskripsi:", "Edit Program", deskLama);

            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE program_bantuan SET nama_program = @nama, deskripsi = @desk WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nama", namaBaru);
                    cmd.Parameters.AddWithValue("@desk", deskBaru);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Program berhasil diperbarui", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataFromDatabase();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTutup_Click(object sender, EventArgs e)
        {
            if (dgvProgram.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih program terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string id = dgvProgram.SelectedRows[0].Cells["ID"].Value.ToString();
            string nama = dgvProgram.SelectedRows[0].Cells["Nama Program"].Value.ToString();
            string status = dgvProgram.SelectedRows[0].Cells["Status"].Value.ToString();

            if (status != "Aktif")
            {
                MessageBox.Show("Program ini sudah Nonaktif.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult dr = MessageBox.Show("Tutup program '" + nama + "'?\nProgram yang ditutup tidak akan bisa menerima penerima baru.", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = DBHelper.GetConnection())
                    {
                        conn.Open();
                        string query = "UPDATE program_bantuan SET status_program = 'nonaktif' WHERE id = @id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Program berhasil ditutup", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataFromDatabase();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ====================================================
        // FITUR BARU: HAPUS PROGRAM
        // ====================================================
        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvProgram.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih program terlebih dahulu yang ingin dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string id = dgvProgram.SelectedRows[0].Cells["ID"].Value.ToString();
            string nama = dgvProgram.SelectedRows[0].Cells["Nama Program"].Value.ToString();

            // Peringatan keras karena ini menghapus data permanen dari database sibantuan
            DialogResult dr = MessageBox.Show($"Apakah Anda yakin ingin MENGHAPUS PERMANEN program '{nama}'?\n\nSemua data yang terkait dengan program ini di database 'sibantuan' mungkin akan hilang.", "Konfirmasi Hapus Permanen", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

            if (dr == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = DBHelper.GetConnection())
                    {
                        conn.Open();
                        string query = "DELETE FROM program_bantuan WHERE id = @id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", id);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data program berhasil dihapus dari database.", "Sukses Hapus", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataFromDatabase();
                    }
                }
                catch (Exception ex)
                {
                    // Biasanya error terjadi jika ada foreign key (data sudah terpakai di tabel lain)
                    MessageBox.Show("Gagal menghapus! Pastikan data program ini belum dipakai di tabel lain.\nDetail: " + ex.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAktifkan_Click(object sender, EventArgs e)
        {
            if (dgvProgram.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih program terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string id = dgvProgram.SelectedRows[0].Cells["ID"].Value.ToString();
            string nama = dgvProgram.SelectedRows[0].Cells["Nama Program"].Value.ToString();
            string status = dgvProgram.SelectedRows[0].Cells["Status"].Value.ToString();

            if (status == "Aktif")
            {
                MessageBox.Show("Program ini sudah Aktif.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult dr = MessageBox.Show("Aktifkan kembali program '" + nama + "'?\nProgram akan dapat menerima penerima baru kembali.", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = DBHelper.GetConnection())
                    {
                        conn.Open();
                        string query = "UPDATE program_bantuan SET status_program = 'aktif' WHERE id = @id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Program berhasil diaktifkan kembali", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataFromDatabase();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Fungsi kosong jika tidak sengaja terklik di desainer
        private void btnTambah_Click(object sender, EventArgs e) { }
        private void btnEdit_Click(object sender, EventArgs e) { }
    }
}