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
    public partial class FormPenyaluran : Form
    {
        private int selectedRowIndex = -1;
        private int permohonanId = -1;
        private int petugasId = 1;

        public FormPenyaluran()
        {
            InitializeComponent();
        }

        private void FormPenyaluran_Load(object sender, EventArgs e)
        {
            LoadDataPermohonan();
            LoadBentukBantuan();
            dt_distribusi.Value = DateTime.Now;
        }

        private void LoadDataPermohonan()
        {
            try
            {
                dataGridView1.Rows.Clear();

                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT p.id, pd.nik, pd.nama_lengkap, pb.nama_program, p.tanggal_pengajuan, p.status_permohonan " +
                                   "FROM permohonan p " +
                                   "JOIN penduduk pd ON p.penduduk_id = pd.id " +
                                   "JOIN program_bantuan pb ON p.program_id = pb.id " +
                                   "WHERE p.status_permohonan = 'disetujui' " +
                                   "ORDER BY p.tanggal_pengajuan ASC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        dataGridView1.Rows.Add(
                            reader["nik"].ToString(),
                            reader["nama_lengkap"].ToString(),
                            reader["nama_program"].ToString(),
                            Convert.ToDateTime(reader["tanggal_pengajuan"]).ToString("yyyy-MM-dd"),
                            reader["status_permohonan"].ToString()
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

        private void LoadBentukBantuan()
        {
            cb_bentuk.Items.Clear();
            cb_bentuk.Items.Add("Uang Tunai");
            cb_bentuk.Items.Add("Sembako");
            cb_bentuk.Items.Add("Voucher");
            cb_bentuk.SelectedIndex = 0;
        }

        private void ClearForm()
        {
            tb_nik.Clear();
            tb_nama.Clear();
            tb_program.Clear();
            tb_jumlah.Clear();
            tb_keterangan.Clear();
            cb_bentuk.SelectedIndex = 0;
            dt_distribusi.Value = DateTime.Now;
            selectedRowIndex = -1;
            permohonanId = -1;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            selectedRowIndex = e.RowIndex;

            try
            {
                string nik = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT p.id, pd.nik, pd.nama_lengkap, pb.nama_program " +
                                   "FROM permohonan p " +
                                   "JOIN penduduk pd ON p.penduduk_id = pd.id " +
                                   "JOIN program_bantuan pb ON p.program_id = pb.id " +
                                   "WHERE pd.nik = @nik AND p.status_permohonan = 'disetujui'";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nik", nik);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        permohonanId = Convert.ToInt32(reader["id"]);
                        tb_nik.Text = reader["nik"].ToString();
                        tb_nama.Text = reader["nama_lengkap"].ToString();
                        tb_program.Text = reader["nama_program"].ToString();
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat detail: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidasiForm()
        {
            if (permohonanId < 0)
            {
                MessageBox.Show("Pilih permohonan yang ingin dicatat!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(tb_jumlah.Text))
            {
                MessageBox.Show("Jumlah bantuan tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tb_jumlah.Focus();
                return false;
            }

            if (!decimal.TryParse(tb_jumlah.Text, out decimal jumlah) || jumlah <= 0)
            {
                MessageBox.Show("Jumlah bantuan harus berupa angka positif!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tb_jumlah.Focus();
                return false;
            }

            return true;
        }

        private void bt_simpan_Click(object sender, EventArgs e)
        {
            if (!ValidasiForm())
                return;

            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    string bentuk = cb_bentuk.SelectedItem.ToString().ToLower().Replace(" ", "_");

                    string query = "INSERT INTO distribusi (permohonan_id, petugas_id, tanggal_distribusi, jumlah_bantuan, bentuk_bantuan, keterangan) " +
                                   "VALUES (@permohonan_id, @petugas_id, @tanggal, @jumlah, @bentuk, @keterangan)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@permohonan_id", permohonanId);
                    cmd.Parameters.AddWithValue("@petugas_id", petugasId);
                    cmd.Parameters.AddWithValue("@tanggal", dt_distribusi.Value);
                    cmd.Parameters.AddWithValue("@jumlah", decimal.Parse(tb_jumlah.Text));
                    cmd.Parameters.AddWithValue("@bentuk", bentuk);
                    cmd.Parameters.AddWithValue("@keterangan", tb_keterangan.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data distribusi berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataPermohonan();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bt_batal_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void bt_refresh_Click(object sender, EventArgs e)
        {
            LoadDataPermohonan();
            MessageBox.Show("Data berhasil direfresh!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
