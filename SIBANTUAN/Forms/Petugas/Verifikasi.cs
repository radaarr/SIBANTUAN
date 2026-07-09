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
            label3.Text = Session.Nama;
            label4.Text = Session.WilayahRtRw + " - " + Session.WilayahKelurahan;
        }

        private void LoadData()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT nama_lengkap, nik, jenis_kelamin, created_at, status_verifikasi FROM penduduk WHERE rt_rw = @rw AND kelurahan = @kel";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@rw", Session.WilayahRtRw);
                    cmd.Parameters.AddWithValue("@kel", Session.WilayahKelurahan);
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
                            row["status_verifikasi"].ToString()
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
            status_cmb.Items.Add("Belum Diverifikasi");
            status_cmb.Items.Add("Disetujui");
            status_cmb.Items.Add("Ditolak");
            status_cmb.SelectedIndex = 0;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                // Form sizing sudah dikonfigurasi di Designer (FixedSingle, CenterScreen)
                // FormHelper.SetFullscreenMode(this);

                Panel footerPanel = this.Controls["pnlFooter"] as Panel;
                FormHelper.SetPanelDocking(panel1, null, footerPanel);
                FormHelper.SetDataGridViewResponsive(dataGridView1);

                // Set Anchor untuk semua panel agar responsive
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is Panel || ctrl is DataGridView)
                    {
                        ctrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | 
                                     AnchorStyles.Right | AnchorStyles.Bottom;
                    }
                }

                LoadStatusFilter();
                LoadData();
                SetupCrudButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error di Form_Load: " + ex.Message);
            }
        }

        private void SetupCrudButtons()
        {
            Button btnTambah = new Button();
            btnTambah.Text = "Tambah Data";
            btnTambah.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold);
            btnTambah.Location = new Point(panel3.Width - 220, 38);
            btnTambah.Size = new Size(100, 27);
            btnTambah.Click += BtnTambah_Click;
            panel3.Controls.Add(btnTambah);

            Button btnSimpan = new Button();
            btnSimpan.Text = "Simpan";
            btnSimpan.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            btnSimpan.BackColor = Color.DodgerBlue;
            btnSimpan.ForeColor = Color.White;
            btnSimpan.Location = new Point(340, 317);
            btnSimpan.Size = new Size(80, 32);
            btnSimpan.Click += BtnSimpan_Click;
            panel5.Controls.Add(btnSimpan);

            Button btnHapus = new Button();
            btnHapus.Text = "Hapus";
            btnHapus.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            btnHapus.BackColor = Color.OrangeRed;
            btnHapus.ForeColor = Color.White;
            btnHapus.Location = new Point(426, 317);
            btnHapus.Size = new Size(80, 32);
            btnHapus.Click += BtnHapus_Click;
            panel5.Controls.Add(btnHapus);
        }

        private bool isNewRecord = false;

        private void BtnTambah_Click(object sender, EventArgs e)
        {
            ClearDetailForm();
            isNewRecord = true;
        }

        private void BtnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("NIK dan Nama Lengkap wajib diisi");
                return;
            }
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    if (isNewRecord)
                    {
                        string query = @"INSERT INTO penduduk (nik, nama_lengkap, alamat, rt_rw, kelurahan, tanggal_lahir, jenis_kelamin, status_ekonomi, nomor_telepon, status_verifikasi)
                                         VALUES (@nik, @nama, @alamat, @rw, @kel, @tgl, @jk, 'miskin', @telp, 'Belum Diverifikasi')";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@nik", textBox1.Text);
                        cmd.Parameters.AddWithValue("@nama", textBox6.Text);
                        cmd.Parameters.AddWithValue("@alamat", textBox3.Text);
                        cmd.Parameters.AddWithValue("@rw", Session.WilayahRtRw);
                        cmd.Parameters.AddWithValue("@kel", textBox9.Text);
                        cmd.Parameters.AddWithValue("@tgl", DateTime.TryParse(textBox2.Text, out var tgl) ? tgl.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@jk", textBox7.Text == "L" ? "laki_laki" : "perempuan");
                        cmd.Parameters.AddWithValue("@telp", textBox4.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data penduduk berhasil ditambahkan");
                    }
                    else
                    {
                        string query = @"UPDATE penduduk SET nama_lengkap=@nama, alamat=@alamat, kelurahan=@kel, tanggal_lahir=@tgl, jenis_kelamin=@jk, nomor_telepon=@telp WHERE nik=@nik";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@nik", textBox1.Text);
                        cmd.Parameters.AddWithValue("@nama", textBox6.Text);
                        cmd.Parameters.AddWithValue("@alamat", textBox3.Text);
                        cmd.Parameters.AddWithValue("@kel", textBox9.Text);
                        cmd.Parameters.AddWithValue("@tgl", DateTime.TryParse(textBox2.Text, out var tgl) ? tgl.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@jk", textBox7.Text == "L" ? "laki_laki" : "perempuan");
                        cmd.Parameters.AddWithValue("@telp", textBox4.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data penduduk berhasil diperbarui");
                    }
                    ClearDetailForm();
                    LoadData();
                    isNewRecord = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void BtnHapus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Pilih data penduduk terlebih dahulu");
                return;
            }
            DialogResult dr = MessageBox.Show("Yakin hapus data penduduk NIK: " + textBox1.Text + "?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = DBHelper.GetConnection())
                    {
                        conn.Open();
                        string query = "DELETE FROM penduduk WHERE nik = @nik";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@nik", textBox1.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data berhasil dihapus");
                        ClearDetailForm();
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
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
                        query = "SELECT nama_lengkap, nik, jenis_kelamin, created_at, status_verifikasi FROM penduduk WHERE rt_rw = @rw AND kelurahan = @kel";
                    }
                    else
                    {
                        query = "SELECT nama_lengkap, nik, jenis_kelamin, created_at, status_verifikasi FROM penduduk WHERE status_verifikasi = @status AND rt_rw = @rw AND kelurahan = @kel";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@rw", Session.WilayahRtRw);
                    cmd.Parameters.AddWithValue("@kel", Session.WilayahKelurahan);
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
                            row["status_verifikasi"].ToString()
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
                    string query = "SELECT nama_lengkap, nik, jenis_kelamin, created_at, status_verifikasi FROM penduduk WHERE (nama_lengkap LIKE @searchTerm OR nik LIKE @searchTerm) AND rt_rw = @rw AND kelurahan = @kel";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                    cmd.Parameters.AddWithValue("@rw", Session.WilayahRtRw);
                    cmd.Parameters.AddWithValue("@kel", Session.WilayahKelurahan);

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
                            row["status_verifikasi"].ToString()
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
                                            status_verifikasi, status_ekonomi, kelurahan, rt_rw, nomor_telepon
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
                        textBox8.Text = reader["status_verifikasi"].ToString();
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

        private void button5_Click(object sender, EventArgs e) // Setujui
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Silakan pilih pendaftar terlebih dahulu");
                return;
            }

            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string nik = textBox1.Text;

                    string query = "UPDATE penduduk SET status_verifikasi = 'Disetujui' WHERE nik = @nik";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nik", nik);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Pendaftar berhasil disetujui");
                        ClearDetailForm();
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Gagal mengupdate pendaftar");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e) // Tolak
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Silakan pilih pendaftar terlebih dahulu");
                return;
            }

            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string nik = textBox1.Text;

                    string catatan = textBox5.Text.Length > 0 ? textBox5.Text : "Tidak memenuhi kriteria";
                    string query = "UPDATE penduduk SET status_verifikasi = 'Ditolak', catatan_verifikasi = @catatan WHERE nik = @nik";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@catatan", catatan);
                    cmd.Parameters.AddWithValue("@nik", nik);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Pendaftar berhasil ditolak");
                        ClearDetailForm();
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Gagal mengupdate pendaftar");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ClearDetailForm()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
        }

        private void button7_Click(object sender, EventArgs e) // Reset
        {
            ClearDetailForm();
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
        private void verifikasi_bt_Click(object sender, EventArgs e) { /* already here */ }
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
        private void distribus_bt_Click(object sender, EventArgs e) 
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
        private void laporan_bt_Click(object sender, EventArgs e) 
        { 
            try
            {
                if (DashboardPetugas.Instance != null)
                {
                    Laporan form = new Laporan();
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
        private void keluar_bt_Click(object sender, EventArgs e) { Application.Exit(); }
    }
}
