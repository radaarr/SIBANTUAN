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
    public partial class Permohonan : Form
    {
        public Permohonan()
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
                    string query = "SELECT nama_warga, program_bantuan, tgl_pengajuan, status_ekonomi, status FROM permohonan_view";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.Rows.Clear();
                    int no = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        dataGridView1.Rows.Add(
                            no++,
                            row["nama_warga"].ToString(),
                            row["program_bantuan"].ToString(),
                            row["tgl_pengajuan"].ToString(),
                            row["status_ekonomi"].ToString(),
                            row["status"].ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error LoadData: " + ex.Message);
            }
        }

        private void LoadStatusFilter()
        {
            status_cmb.Items.Clear();
            status_cmb.Items.Add("Semua");
            status_cmb.Items.Add("Pending");
            status_cmb.Items.Add("Disetujui");
            status_cmb.Items.Add("Ditolak");
            status_cmb.SelectedIndex = 0;

            program_cmb.Items.Clear();
            program_cmb.Items.Add("Semua Program");
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT DISTINCT program_bantuan FROM permohonan_view";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        program_cmb.Items.Add(reader["program_bantuan"].ToString());
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading programs: " + ex.Message);
            }
            program_cmb.SelectedIndex = 0;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            LoadStatusFilter();
            LoadData();
        }

        private void status_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void program_cmb_SelectedIndexChanged(object sender, EventArgs e)
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
            program_cmb.SelectedIndex = 0;
            LoadData();
        }

        private void FilterData()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string status = status_cmb.SelectedItem?.ToString() ?? "Semua";
                    string program = program_cmb.SelectedItem?.ToString() ?? "Semua Program";

                    string query = "SELECT nama_warga, program_bantuan, tgl_pengajuan, status_ekonomi, status FROM permohonan_view WHERE 1=1";

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    if (status != "Semua")
                    {
                        query += " AND status = @status";
                        cmd.Parameters.AddWithValue("@status", status);
                    }

                    if (program != "Semua Program")
                    {
                        query += " AND program_bantuan = @program";
                        cmd.Parameters.AddWithValue("@program", program);
                    }

                    cmd.CommandText = query;
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.Rows.Clear();
                    int no = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        dataGridView1.Rows.Add(
                            no++,
                            row["nama_warga"].ToString(),
                            row["program_bantuan"].ToString(),
                            row["tgl_pengajuan"].ToString(),
                            row["status_ekonomi"].ToString(),
                            row["status"].ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error FilterData: " + ex.Message);
            }
        }

        private void SearchData()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT nama_warga, program_bantuan, tgl_pengajuan, status_ekonomi, status FROM permohonan_view WHERE nama_warga LIKE @searchTerm";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + cari_tb.Text + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.Rows.Clear();
                    int no = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        dataGridView1.Rows.Add(
                            no++,
                            row["nama_warga"].ToString(),
                            row["program_bantuan"].ToString(),
                            row["tgl_pengajuan"].ToString(),
                            row["status_ekonomi"].ToString(),
                            row["status"].ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error SearchData: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    string nama_warga = dataGridView1.Rows[e.RowIndex].Cells["nama_warga"].Value.ToString();
                    LoadDetailPermohonan(nama_warga);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void LoadDetailPermohonan(string nama_warga)
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT nama_warga, program_bantuan, status_ekonomi, kuota_program, nik, tgl_pengajuan, alamat, periode_program FROM permohonan_view WHERE nama_warga = @nama_warga";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nama_warga", nama_warga);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        textBox1.Text = reader["nama_warga"].ToString();
                        textBox2.Text = reader["program_bantuan"].ToString();
                        textBox3.Text = reader["status_ekonomi"].ToString();
                        textBox4.Text = reader["kuota_program"].ToString();
                        textBox5.Text = reader["nik"].ToString();
                        textBox6.Text = reader["tgl_pengajuan"].ToString();
                        textBox7.Text = reader["alamat"].ToString();
                        textBox8.Text = reader["periode_program"].ToString();

                        label20.Text = "Detail Permohonan — " + nama_warga;
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dashboard_bt_Click(object sender, EventArgs e) { new DashboardPetugas(0, "Petugas").ShowDialog(); }
        private void verifikasi_bt_Click(object sender, EventArgs e) { new Verifikasi().ShowDialog(); }
        private void permohonan_bt_Click(object sender, EventArgs e) { /* already here */ }
        private void distribusi_bt_Click(object sender, EventArgs e) { new Distribusi().ShowDialog(); }
        private void laporan_bt_Click(object sender, EventArgs e) { new Laporan().ShowDialog(); }
        private void keluar_bt_Click(object sender, EventArgs e) { Application.Exit(); }
    }
}
