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
    public partial class FormVerifikasi : Form
    {
        private int selectedRowIndex = -1;
        private int permohonanId = -1;

        public FormVerifikasi()
        {
            InitializeComponent();
        }

        private void FormVerifikasi_Load(object sender, EventArgs e)
        {
            LoadDataPermohonan();
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
                                   "WHERE p.status_permohonan = 'pending' " +
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

        private void ClearForm()
        {
            tb_nik.Clear();
            tb_nama.Clear();
            tb_program.Clear();
            tb_catatan.Clear();
            tb_alasan.Clear();
            rb_disetujui.Checked = false;
            rb_ditolak.Checked = false;
            tb_alasan.Visible = false;
            label9.Visible = false;
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

                    string query = "SELECT p.id, pd.nik, pd.nama_lengkap, pb.nama_program, p.catatan_petugas " +
                                   "FROM permohonan p " +
                                   "JOIN penduduk pd ON p.penduduk_id = pd.id " +
                                   "JOIN program_bantuan pb ON p.program_id = pb.id " +
                                   "WHERE pd.nik = @nik AND p.status_permohonan = 'pending'";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nik", nik);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        permohonanId = Convert.ToInt32(reader["id"]);
                        tb_nik.Text = reader["nik"].ToString();
                        tb_nama.Text = reader["nama_lengkap"].ToString();
                        tb_program.Text = reader["nama_program"].ToString();
                        tb_catatan.Text = reader["catatan_petugas"].ToString() ?? "";
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat detail: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rb_disetujui_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_disetujui.Checked)
            {
                tb_alasan.Visible = false;
                label9.Visible = false;
                tb_alasan.Clear();
            }
        }

        private void rb_ditolak_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_ditolak.Checked)
            {
                tb_alasan.Visible = true;
                label9.Visible = true;
                tb_alasan.Focus();
            }
        }

        private void bt_simpan_Click(object sender, EventArgs e)
        {
            if (permohonanId < 0)
            {
                MessageBox.Show("Pilih permohonan yang ingin diverifikasi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!rb_disetujui.Checked && !rb_ditolak.Checked)
            {
                MessageBox.Show("Pilih keputusan verifikasi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (rb_ditolak.Checked && string.IsNullOrWhiteSpace(tb_alasan.Text))
            {
                MessageBox.Show("Masukkan alasan penolakan!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tb_alasan.Focus();
                return;
            }

            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    string status = rb_disetujui.Checked ? "disetujui" : "ditolak";
                    string alasanTolak = rb_ditolak.Checked ? tb_alasan.Text : "";

                    string query = "UPDATE permohonan SET status_permohonan = @status, catatan_petugas = @catatan, " +
                                   "alasan_tolak = @alasan, verified_by = 1, verified_at = NOW() " +
                                   "WHERE id = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@catatan", tb_catatan.Text);
                    cmd.Parameters.AddWithValue("@alasan", alasanTolak);
                    cmd.Parameters.AddWithValue("@id", permohonanId);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Verifikasi berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataPermohonan();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan verifikasi: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
