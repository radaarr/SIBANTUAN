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
    public partial class FormPetugas : Form
    {
        private int selectedRowIndex = -1;

        public FormPetugas()
        {
            InitializeComponent();
        }

        private void FormPetugas_Load(object sender, EventArgs e)
        {
            LoadDataPenduduk();
            LoadStatusEkonomi();
        }

        private void LoadDataPenduduk()
        {
            try
            {
                dataGridView1.Rows.Clear();

                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT id, nik, nama_lengkap, alamat, rt_rw, tanggal_lahir, status_ekonomi FROM penduduk";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        dataGridView1.Rows.Add(
                            reader["nik"].ToString(),
                            reader["nama_lengkap"].ToString(),
                            reader["alamat"].ToString(),
                            reader["rt_rw"].ToString(),
                            Convert.ToDateTime(reader["tanggal_lahir"]).ToString("yyyy-MM-dd"),
                            reader["status_ekonomi"].ToString()
                        );
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStatusEkonomi()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Sangat Miskin");
            comboBox1.Items.Add("Miskin");
            comboBox1.Items.Add("Rentan");
            comboBox1.SelectedIndex = 0;
        }

        private void ClearForm()
        {
            tb_nik.Clear();
            tb_nama.Clear();
            tb_alamat.Clear();
            tb_rt.Clear();
            tb_rw.Clear();
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.SelectedIndex = 0;
            selectedRowIndex = -1;
        }

        private bool ValidasiForm()
        {
            if (string.IsNullOrWhiteSpace(tb_nik.Text))
            {
                MessageBox.Show("NIK tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tb_nik.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(tb_nama.Text))
            {
                MessageBox.Show("Nama tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tb_nama.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(tb_alamat.Text))
            {
                MessageBox.Show("Alamat tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tb_alamat.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(tb_rt.Text) || string.IsNullOrWhiteSpace(tb_rw.Text))
            {
                MessageBox.Show("RT/RW tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void bt_tambah_Click(object sender, EventArgs e)
        {
            if (!ValidasiForm())
                return;

            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    string rtRw = tb_rt.Text.Trim() + "/" + tb_rw.Text.Trim();
                    string statusEkonomi = comboBox1.SelectedItem.ToString().ToLower().Replace(" ", "_");

                    string query = "INSERT INTO penduduk (nik, nama_lengkap, alamat, rt_rw, kelurahan, tanggal_lahir, jenis_kelamin, status_ekonomi) " +
                                   "VALUES (@nik, @nama, @alamat, @rtrw, @kelurahan, @tgl_lahir, @jk, @status_ekonomi)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nik", tb_nik.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama", tb_nama.Text.Trim());
                    cmd.Parameters.AddWithValue("@alamat", tb_alamat.Text.Trim());
                    cmd.Parameters.AddWithValue("@rtrw", rtRw);
                    cmd.Parameters.AddWithValue("@kelurahan", "Kelurahan");
                    cmd.Parameters.AddWithValue("@tgl_lahir", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@jk", "laki_laki");
                    cmd.Parameters.AddWithValue("@status_ekonomi", statusEkonomi);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataPenduduk();
                    ClearForm();
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    MessageBox.Show("NIK sudah terdaftar!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Gagal menambah data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bt_edit_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex < 0)
            {
                MessageBox.Show("Pilih data yang ingin diedit!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidasiForm())
                return;

            try
            {
                string nikLama = dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString();

                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    string rtRw = tb_rt.Text.Trim() + "/" + tb_rw.Text.Trim();
                    string statusEkonomi = comboBox1.SelectedItem.ToString().ToLower().Replace(" ", "_");

                    string query = "UPDATE penduduk SET nik=@nik, nama_lengkap=@nama, alamat=@alamat, " +
                                   "rt_rw=@rtrw, tanggal_lahir=@tgl_lahir, status_ekonomi=@status_ekonomi " +
                                   "WHERE nik=@nik_lama";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nik", tb_nik.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama", tb_nama.Text.Trim());
                    cmd.Parameters.AddWithValue("@alamat", tb_alamat.Text.Trim());
                    cmd.Parameters.AddWithValue("@rtrw", rtRw);
                    cmd.Parameters.AddWithValue("@tgl_lahir", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@status_ekonomi", statusEkonomi);
                    cmd.Parameters.AddWithValue("@nik_lama", nikLama);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataPenduduk();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal mengedit data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bt_hapus_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex < 0)
            {
                MessageBox.Show("Pilih data yang ingin dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            try
            {
                string nik = dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString();

                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    string query = "DELETE FROM penduduk WHERE nik=@nik";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nik", nik);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataPenduduk();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menghapus data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            selectedRowIndex = e.RowIndex;

            tb_nik.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            tb_nama.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            tb_alamat.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();

            string rtRw = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            string[] parts = rtRw.Split('/');
            tb_rt.Text = parts.Length > 0 ? parts[0] : "";
            tb_rw.Text = parts.Length > 1 ? parts[1] : "";

            dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[4].Value);

            string statusEkonomi = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            switch (statusEkonomi.ToLower())
            {
                case "sangat_miskin":
                    comboBox1.SelectedIndex = 0;
                    break;
                case "miskin":
                    comboBox1.SelectedIndex = 1;
                    break;
                case "rentan":
                    comboBox1.SelectedIndex = 2;
                    break;
            }
        }

        private void bt_data_Click(object sender, EventArgs e)
        {
            LoadDataPenduduk();
            MessageBox.Show("Data berhasil dimuat!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bt_verifikasi_Click(object sender, EventArgs e)
        {
            FormVerifikasi formVerifikasi = new FormVerifikasi();
            formVerifikasi.ShowDialog();
        }

        private void bt_catat_Click(object sender, EventArgs e)
        {
            FormPenyaluran formPenyaluran = new FormPenyaluran();
            formPenyaluran.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label6_Click(object sender, EventArgs e)
        {
        }

        private void tb_tgl_TextChanged(object sender, EventArgs e)
        {
        }

        private void tb_rw_TextChanged(object sender, EventArgs e)
        {
        }

        private void tb_rt_TextChanged(object sender, EventArgs e)
        {
        }

        private void tb_alamat_TextChanged(object sender, EventArgs e)
        {
        }

        private void tb_nama_TextChanged(object sender, EventArgs e)
        {
        }

        private void tb_nik_TextChanged(object sender, EventArgs e)
        {
        }

        private void label5_Click(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label7_Click(object sender, EventArgs e)
        {
        }
    }
}
