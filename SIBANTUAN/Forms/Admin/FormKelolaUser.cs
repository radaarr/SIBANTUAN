using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SIBANTUAN.Forms
{
    public partial class FormKelolaUser : Form
    {
        private Color warnaBackground = Color.FromArgb(245, 246, 250);
        private Color warnaAksen = Color.FromArgb(52, 152, 219);
        private Color warnaPilih = Color.FromArgb(230, 242, 255);

        private Label lblWilayah;
        private ComboBox cmbWilayah;
        private Label lblPassword;
        private TextBox txtPassword;
        private List<WilayahItem> daftarWilayah;
        private int _selectedUserId = 0;

        public FormKelolaUser()
        {
            InitializeComponent();
            SetupModernUI();
            LoadDataFromDatabase();
            BuatWilayahCombo();
            BuatPasswordField();
            LoadWilayahOptions();
            cmbRole.SelectedIndexChanged += CmbRole_SelectedIndexChanged;
            ToggleWilayahVisibility();
            AturUlangPosisi();
        }

        private class WilayahItem
        {
            public string RtRw { get; }
            public string Kelurahan { get; }
            public WilayahItem(string rtRw, string kelurahan)
            {
                RtRw = rtRw;
                Kelurahan = kelurahan;
            }
            public override string ToString() => $"{RtRw} - {Kelurahan}";
        }

        private void SetupModernUI()
        {
            this.BackColor = warnaBackground;

            dgvUser.EnableHeadersVisualStyles = false;
            dgvUser.BackgroundColor = warnaBackground;
            dgvUser.GridColor = Color.LightGray;
            dgvUser.BorderStyle = BorderStyle.None;
            dgvUser.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUser.AllowUserToAddRows = false;

            dgvUser.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvUser.ColumnHeadersDefaultCellStyle.BackColor = warnaAksen;
            dgvUser.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUser.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvUser.ColumnHeadersHeight = 40;

            dgvUser.DefaultCellStyle.SelectionBackColor = warnaPilih;
            dgvUser.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvUser.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            dgvUser.RowTemplate.Height = 35;

            cmbRole.Items.Clear();
            cmbRole.Items.Add("admin_pusat");
            cmbRole.Items.Add("petugas_rtrw");
            cmbRole.Items.Add("penerima_bantuan");
            cmbRole.SelectedIndex = 0;
        }

        private void BuatWilayahCombo()
        {
            lblWilayah = new Label();
            lblWilayah.Text = "Wilayah RT/RW";
            lblWilayah.Location = new Point(24, 320);
            lblWilayah.AutoSize = true;

            cmbWilayah = new ComboBox();
            cmbWilayah.FlatStyle = FlatStyle.Flat;
            cmbWilayah.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWilayah.Location = new Point(24, 344);
            cmbWilayah.Size = new Size(240, 28);

            this.Controls.Add(lblWilayah);
            this.Controls.Add(cmbWilayah);
        }

        private void BuatPasswordField()
        {
            lblPassword = new Label();
            lblPassword.Text = "Password Baru (opsional)";
            lblPassword.Location = new Point(24, 208);
            lblPassword.AutoSize = true;

            txtPassword = new TextBox();
            txtPassword.Location = new Point(24, 230);
            txtPassword.Size = new Size(240, 26);
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.Visible = false;

            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
        }

        private void AturUlangPosisi()
        {
            lblRole.Location = new Point(24, 262);
            cmbRole.Location = new Point(24, 287);
            btnSimpan.Location = new Point(24, 370);
            btnReset.Location = new Point(24, 420);
            btnNonaktif.Location = new Point(24, 470);
            dgvUser.Size = new Size(508, 474);
            this.ClientSize = new Size(800, 520);
        }

        private void LoadWilayahOptions()
        {
            daftarWilayah = new List<WilayahItem>();
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT DISTINCT rt_rw, kelurahan FROM penduduk ORDER BY kelurahan, rt_rw";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    using (MySqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            daftarWilayah.Add(new WilayahItem(
                                r["rt_rw"].ToString(),
                                r["kelurahan"].ToString()
                            ));
                        }
                    }
                }
            }
            catch { }

            cmbWilayah.Items.Clear();
            foreach (var w in daftarWilayah)
                cmbWilayah.Items.Add(w);

            if (cmbWilayah.Items.Count > 0)
                cmbWilayah.SelectedIndex = 0;
        }

        private void CmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleWilayahVisibility();
        }

        private void ToggleWilayahVisibility()
        {
            bool isPetugas = cmbRole.SelectedItem?.ToString() == "petugas_rtrw";
            lblWilayah.Visible = isPetugas;
            cmbWilayah.Visible = isPetugas;
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT id, nama, username, role, is_active FROM users ORDER BY id";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvUser.ColumnCount = 6;
                    dgvUser.Columns[0].Name = "ID";
                    dgvUser.Columns[1].Name = "Nama Lengkap";
                    dgvUser.Columns[2].Name = "Username";
                    dgvUser.Columns[3].Name = "Role";
                    dgvUser.Columns[4].Name = "Status";
                    dgvUser.Columns[5].Name = "Wilayah";

                    dgvUser.Rows.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        string roleDb = row["role"].ToString();
                        string role = roleDb == "admin_pusat" ? "Admin" : roleDb == "petugas_rtrw" ? "Petugas" : "Penerima";
                        string status = Convert.ToInt32(row["is_active"]) == 1 ? "Aktif" : "Nonaktif";

                        // Ambil wilayah dari tabel penduduk untuk petugas
                        string wilayah = "-";
                        if (roleDb == "petugas_rtrw")
                        {
                            int uid = Convert.ToInt32(row["id"]);
                            MySqlCommand cmdWil = new MySqlCommand(
                                "SELECT rt_rw, kelurahan FROM penduduk WHERE user_id = @uid LIMIT 1", conn);
                            cmdWil.Parameters.AddWithValue("@uid", uid);
                            using (MySqlDataReader r = cmdWil.ExecuteReader())
                            {
                                if (r.Read())
                                    wilayah = r["rt_rw"].ToString() + " - " + r["kelurahan"].ToString();
                            }
                        }

                        dgvUser.Rows.Add(row["id"].ToString(), row["nama"].ToString(), row["username"].ToString(), role, status, wilayah);
                    }
                    dgvUser.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvUser.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error memuat data user: " + ex.Message);
            }
        }

        private void ClearInput()
        {
            _selectedUserId = 0;
            txtNama.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtPassword.Visible = false;
            lblPassword.Visible = false;
            cmbRole.SelectedIndex = 0;
            if (cmbWilayah.Items.Count > 0)
                cmbWilayah.SelectedIndex = 0;
            btnSimpan.Text = "Simpan User";
            txtNama.Focus();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNama.Text) || string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Nama dan Username tidak boleh kosong.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string role = cmbRole.SelectedItem.ToString();

            if (role == "petugas_rtrw")
            {
                if (cmbWilayah.SelectedItem == null)
                {
                    MessageBox.Show("Pilih wilayah RT/RW untuk petugas.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            bool isEdit = _selectedUserId > 0;

            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    MySqlTransaction trans = conn.BeginTransaction();

                    if (isEdit)
                    {
                        // ── EDIT: UPDATE user ──
                        string query = "UPDATE users SET nama = @nama, username = @username, role = @role";
                        if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                            query += ", password_hash = @pass";
                        query += " WHERE id = @id";

                        MySqlCommand cmd = new MySqlCommand(query, conn, trans);
                        cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@role", role);
                        cmd.Parameters.AddWithValue("@id", _selectedUserId);
                        if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                            cmd.Parameters.AddWithValue("@pass", txtPassword.Text.Trim());
                        cmd.ExecuteNonQuery();

                        // Jika petugas, update penduduk record
                        if (role == "petugas_rtrw")
                        {
                            WilayahItem wil = (WilayahItem)cmbWilayah.SelectedItem;

                            MySqlCommand cek = new MySqlCommand(
                                "SELECT COUNT(*) FROM penduduk WHERE user_id = @uid", conn, trans);
                            cek.Parameters.AddWithValue("@uid", _selectedUserId);
                            int ada = Convert.ToInt32(cek.ExecuteScalar());

                            if (ada > 0)
                            {
                                MySqlCommand upd = new MySqlCommand(
                                    "UPDATE penduduk SET rt_rw = @rt, kelurahan = @kel WHERE user_id = @uid", conn, trans);
                                upd.Parameters.AddWithValue("@rt", wil.RtRw);
                                upd.Parameters.AddWithValue("@kel", wil.Kelurahan);
                                upd.Parameters.AddWithValue("@uid", _selectedUserId);
                                upd.ExecuteNonQuery();
                            }
                            else
                            {
                                string dummyNik = "9999" + _selectedUserId.ToString("D12");
                                MySqlCommand ins = new MySqlCommand(@"
                                    INSERT INTO penduduk
                                        (nik, nama_lengkap, alamat, rt_rw, kelurahan,
                                         tanggal_lahir, jenis_kelamin, status_ekonomi, user_id)
                                    VALUES
                                        (@nik, @nama, '-', @rt, @kel,
                                         '2000-01-01', 'laki_laki', 'miskin', @uid)", conn, trans);
                                ins.Parameters.AddWithValue("@nik", dummyNik);
                                ins.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                                ins.Parameters.AddWithValue("@rt", wil.RtRw);
                                ins.Parameters.AddWithValue("@kel", wil.Kelurahan);
                                ins.Parameters.AddWithValue("@uid", _selectedUserId);
                                ins.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        // ── INSERT: buat user baru ──
                        string query = "INSERT INTO users (nama, username, password_hash, role, is_active) VALUES (@nama, @username, @pass, @role, 1)";
                        MySqlCommand cmd = new MySqlCommand(query, conn, trans);
                        cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@pass", "Admin123");
                        cmd.Parameters.AddWithValue("@role", role);
                        cmd.ExecuteNonQuery();

                        MySqlCommand cmdId = new MySqlCommand("SELECT LAST_INSERT_ID()", conn, trans);
                        int newUserId = Convert.ToInt32(cmdId.ExecuteScalar());

                        if (role == "petugas_rtrw")
                        {
                            WilayahItem wil = (WilayahItem)cmbWilayah.SelectedItem;
                            string dummyNik = "9999" + newUserId.ToString("D12");
                            MySqlCommand ins = new MySqlCommand(@"
                                INSERT INTO penduduk
                                    (nik, nama_lengkap, alamat, rt_rw, kelurahan,
                                     tanggal_lahir, jenis_kelamin, status_ekonomi, user_id)
                                VALUES
                                    (@nik, @nama, '-', @rt, @kel,
                                     '2000-01-01', 'laki_laki', 'miskin', @uid)", conn, trans);
                            ins.Parameters.AddWithValue("@nik", dummyNik);
                            ins.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                            ins.Parameters.AddWithValue("@rt", wil.RtRw);
                            ins.Parameters.AddWithValue("@kel", wil.Kelurahan);
                            ins.Parameters.AddWithValue("@uid", newUserId);
                            ins.ExecuteNonQuery();
                        }
                    }

                    trans.Commit();

                    MessageBox.Show(isEdit ? "Akun berhasil diperbarui!" : "Akun berhasil ditambahkan! Password default: Admin123",
                        "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInput();
                    LoadDataFromDatabase();
                    LoadWilayahOptions();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (dgvUser.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih baris data di tabel terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string id = dgvUser.SelectedRows[0].Cells["ID"].Value.ToString();
            string username = dgvUser.SelectedRows[0].Cells["Username"].Value.ToString();
            DialogResult confirm = MessageBox.Show($"Reset password akun '{username}' ke default (Admin123)?", "Konfirmasi Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = DBHelper.GetConnection())
                    {
                        conn.Open();
                        string query = "UPDATE users SET password_hash = 'Admin123' WHERE id = @id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show($"Password untuk '{username}' berhasil direset ke Admin123.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnNonaktif_Click(object sender, EventArgs e)
        {
            if (dgvUser.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih baris data di tabel terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string id = dgvUser.SelectedRows[0].Cells["ID"].Value.ToString();
            string nama = dgvUser.SelectedRows[0].Cells["Nama Lengkap"].Value.ToString();
            string status = dgvUser.SelectedRows[0].Cells["Status"].Value.ToString();

            if (status == "Nonaktif")
            {
                MessageBox.Show("Akun ini sudah berstatus Nonaktif.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirm = MessageBox.Show($"Nonaktifkan akses sistem untuk akun '{nama}'?", "Konfirmasi Nonaktif", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = DBHelper.GetConnection())
                    {
                        conn.Open();
                        string query = "UPDATE users SET is_active = 0 WHERE id = @id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Status akun berhasil dinonaktifkan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataFromDatabase();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void dgvUser_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUser.SelectedRows.Count == 0 || dgvUser.SelectedRows[0].Cells["ID"].Value == null)
            {
                ClearInput();
                return;
            }

            DataGridViewRow row = dgvUser.SelectedRows[0];

            _selectedUserId = Convert.ToInt32(row.Cells["ID"].Value);
            txtNama.Text = row.Cells["Nama Lengkap"].Value?.ToString() ?? "";
            txtUsername.Text = row.Cells["Username"].Value?.ToString() ?? "";

            string roleDisplay = row.Cells["Role"].Value?.ToString() ?? "";
            string roleDb = roleDisplay == "Admin" ? "admin_pusat" :
                            roleDisplay == "Petugas" ? "petugas_rtrw" : "penerima_bantuan";
            cmbRole.SelectedItem = roleDb;

            txtPassword.Clear();
            txtPassword.Visible = true;
            lblPassword.Visible = true;

            btnSimpan.Text = "Update User";
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}
