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
    public partial class Distribusi : Form
    {
        public Distribusi()
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
                    string query = "SELECT nama_warga, program_bantuan, tgl_disetujui, status FROM distribusi WHERE status IN ('Belum Dicatat', 'Sudah Dicatat')";
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
                            row["tgl_disetujui"].ToString(),
                            row["status"].ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadProgramFilter()
        {
            program_cmb.Items.Clear();
            program_cmb.Items.Add("Semua Program");
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT DISTINCT program_bantuan FROM distribusi";
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
            LoadProgramFilter();
            LoadData();
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Uang Tunai");
            comboBox1.Items.Add("Barang");
            comboBox1.Items.Add("Jasa");
            comboBox1.SelectedIndex = 0;
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
                    string program = program_cmb.SelectedItem.ToString();
                    string query = "SELECT nama_warga, program_bantuan, tgl_disetujui, status FROM distribusi WHERE status IN ('Belum Dicatat', 'Sudah Dicatat')";

                    if (program != "Semua Program")
                    {
                        query += " AND program_bantuan = '" + program + "'";
                    }

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
                            row["tgl_disetujui"].ToString(),
                            row["status"].ToString()
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
                    string query = "SELECT nama_warga, program_bantuan, tgl_disetujui, status FROM distribusi WHERE nama_warga LIKE '%" + searchTerm + "%'";

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
                            row["tgl_disetujui"].ToString(),
                            row["status"].ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    string nama_warga = dataGridView1.Rows[e.RowIndex].Cells["nama_warga"].Value.ToString();
                    LoadDetailDistribusi(nama_warga);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void LoadDetailDistribusi(string nama_warga)
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT nama_warga, program_bantuan, jumlah_bantuan, bukti_penerimaan, nik, tgl_distribusi, bentuk_bantuan, dicatat_oleh, keterangan FROM distribusi WHERE nama_warga = '" + nama_warga + "'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        textBox1.Text = reader["nama_warga"].ToString();
                        textBox2.Text = reader["program_bantuan"].ToString();
                        textBox3.Text = reader["jumlah_bantuan"].ToString();
                        textBox4.Text = reader["bukti_penerimaan"].ToString();
                        textBox5.Text = reader["nik"].ToString();
                        textBox6.Text = reader["tgl_distribusi"].ToString();
                        
                        // Set bentuk_bantuan combo box
                        string bentukBantuan = reader["bentuk_bantuan"].ToString();
                        if (comboBox1.Items.Contains(bentukBantuan))
                        {
                            comboBox1.SelectedItem = bentukBantuan;
                        }
                        
                        textBox7.Text = reader["dicatat_oleh"].ToString();
                        textBox8.Text = reader["keterangan"].ToString();

                        label10.Text = "Form Pencatatan Distribusi — " + nama_warga;
                        label20.Text = "Dipilih: " + nama_warga + " | Belum dicatat: 2 data";
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
