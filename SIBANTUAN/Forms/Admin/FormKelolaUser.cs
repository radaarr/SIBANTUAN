using MySql.Data.MySqlClient;
using System;
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

        public FormKelolaUser()
        {
            InitializeComponent();
            SetupModernUI();
            LoadDataFromDatabase();
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
            cmbRole.SelectedIndex = 0;
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

                    dgvUser.ColumnCount = 5;
                    dgvUser.Columns[0].Name = "ID";
                    dgvUser.Columns[1].Name = "Nama Lengkap";
                    dgvUser.Columns[2].Name = "Username";
                    dgvUser.Columns[3].Name = "Role";
                    dgvUser.Columns[4].Name = "Status";

                    dgvUser.Rows.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        string role = row["role"].ToString() == "admin_pusat" ? "Admin" : "Petugas";
                        string status = Convert.ToInt32(row["is_active"]) == 1 ? "Aktif" : "Nonaktif";
                        dgvUser.Rows.Add(row["id"].ToString(), row["nama"].ToString(), row["username"].ToString(), role, status);
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
            txtNama.Clear();
            txtUsername.Clear();
            cmbRole.SelectedIndex = 0;
            txtNama.Focus();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNama.Text) || string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Nama dan Username tidak boleh kosong.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string role = cmbRole.SelectedItem.ToString();
                    string query = "INSERT INTO users (nama, username, password_hash, role, is_active) VALUES (@nama, @username, @pass, @role, 1)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@pass", "Admin123");
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Akun berhasil ditambahkan! Password default: Admin123", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInput();
                    LoadDataFromDatabase();
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

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}
