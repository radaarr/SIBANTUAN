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
    public partial class Verifikasi : Form
    {
        public Verifikasi()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT nama_lengkap, nik, jenis_kelamin, created_at, status_ekonomi FROM penduduk";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.Rows.Clear();
                    int no = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        dataGridView1.Rows.Add(
                            no++,
                            row["nama_lengkap"].ToString(),
                            row["nik"].ToString(),
                            row["jenis_kelamin"].ToString(),
                            Convert.ToDateTime(row["created_at"]).ToString("dd/MM/yyyy"),
                            row["status_ekonomi"].ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadStatusFilter()
        {
            status_cmb.Items.Clear();
            status_cmb.Items.Add("Semua Status");
            status_cmb.Items.Add("sangat_miskin");
            status_cmb.Items.Add("miskin");
            status_cmb.Items.Add("rentan");
            status_cmb.SelectedIndex = 0;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            LoadStatusFilter();
            LoadData();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void cari_bt_Click(object sender, EventArgs e)
        {
            SearchData();
        }

        private void reset_bt_Click(object sender, EventArgs e)
        {
            cari_tb.Clear();
            status_cmb.SelectedIndex = 0;
            LoadData();
        }

        private void FilterData()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string status = status_cmb.SelectedItem.ToString();
                    string query;

                    if (status == "Semua Status")
                    {
                        query = "SELECT nama_lengkap, nik, jenis_kelamin, created_at, status_ekonomi FROM penduduk";
                    }
                    else
                    {
                        query = "SELECT nama_lengkap, nik, jenis_kelamin, created_at, status_ekonomi FROM penduduk WHERE status_ekonomi = @status";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (status != "Semua Status")
                    {
                        cmd.Parameters.AddWithValue("@status", status);
                    }
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.Rows.Clear();
                    int no = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        dataGridView1.Rows.Add(
                            no++,
                            row["nama_lengkap"].ToString(),
                            row["nik"].ToString(),
                            row["jenis_kelamin"].ToString(),
                            Convert.ToDateTime(row["created_at"]).ToString("dd/MM/yyyy"),
                            row["status_ekonomi"].ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void SearchData()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string searchTerm = cari_tb.Text;
                    string query = "SELECT nama_lengkap, nik, jenis_kelamin, created_at, status_ekonomi FROM penduduk WHERE nama_lengkap LIKE @searchTerm OR nik LIKE @searchTerm";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.Rows.Clear();
                    int no = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        dataGridView1.Rows.Add(
                            no++,
                            row["nama_lengkap"].ToString(),
                            row["nik"].ToString(),
                            row["jenis_kelamin"].ToString(),
                            Convert.ToDateTime(row["created_at"]).ToString("dd/MM/yyyy"),
                            row["status_ekonomi"].ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    string nik = dataGridView1.Rows[e.RowIndex].Cells["nik"].Value.ToString();
                    LoadDetailPendaftar(nik);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void LoadDetailPendaftar(string nik)
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT nik, tanggal_lahir, alamat, nama_lengkap, jenis_kelamin,
                                            status_ekonomi, kelurahan, rt_rw, nomor_telepon
                                     FROM penduduk
                                     WHERE nik = @nik";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nik", nik);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        textBox1.Text = reader["nik"].ToString();
                        textBox2.Text = Convert.ToDateTime(reader["tanggal_lahir"]).ToString("dd/MM/yyyy");
                        textBox3.Text = reader["alamat"].ToString();
                        textBox9.Text = reader["kelurahan"].ToString();
                        textBox6.Text = reader["nama_lengkap"].ToString();
                        textBox7.Text = reader["jenis_kelamin"].ToString();
                        textBox8.Text = reader["status_ekonomi"].ToString();
                        textBox4.Text = reader["nomor_telepon"] == DBNull.Value ? "" : reader["nomor_telepon"].ToString();
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void dashboard_bt_Click(object sender, EventArgs e) { new DashboardPetugas(0, "Petugas").ShowDialog(); }
        private void verifikasi_bt_Click(object sender, EventArgs e) { /* already here */ }
        private void permohonan_bt_Click(object sender, EventArgs e) { new Permohonan().ShowDialog(); }
        private void distribus_bt_Click(object sender, EventArgs e) { new Distribusi().ShowDialog(); }
        private void laporan_bt_Click(object sender, EventArgs e) { new Laporan().ShowDialog(); }
        private void keluar_bt_Click(object sender, EventArgs e) { Application.Exit(); }
    }
}
