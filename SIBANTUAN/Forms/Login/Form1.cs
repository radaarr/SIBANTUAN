using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SIBANTUAN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Validasi input kosong
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username tidak boleh kosong!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password tidak boleh kosong!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // Cek ke database
            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT id, nama, role FROM users " +
                                   "WHERE username = @username " +
                                   "AND password_hash = @password " +
                                   "AND is_active = 1";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Login berhasil, ambil datanya
                        int userId = reader.GetInt32("id");
                        string nama = reader.GetString("nama");
                        string role = reader.GetString("role");
                        reader.Close();
                        conn.Close();

                        // Arahkan ke dashboard sesuai role
                        this.Hide();

                        if (role == "admin_pusat")
                        {
                            // FormDashboardAdmin formAdmin = new FormDashboardAdmin(userId, nama);
                            // formAdmin.ShowDialog();
                            MessageBox.Show("Selamat datang Admin: " + nama, "Login Berhasil",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (role == "petugas_rtrw")
                        {
                            // FormDashboardPetugas formPetugas = new FormDashboardPetugas(userId, nama);
                            // formPetugas.ShowDialog();
                            MessageBox.Show("Selamat datang Petugas: " + nama, "Login Berhasil",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (role == "penerima_bantuan")
                        {
                            // FormDashboardPenerima formPenerima = new FormDashboardPenerima(userId, nama);
                            // formPenerima.ShowDialog();
                            MessageBox.Show("Selamat datang: " + nama, "Login Berhasil",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        // Kalau form dashboard ditutup, tutup juga aplikasinya
                        Application.Exit();
                    }
                    else
                    {
                        reader.Close();
                        MessageBox.Show("Username atau password salah!", "Login Gagal",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPassword.Clear();
                        txtPassword.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal konek ke database!\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            DialogResult konfirmasi = MessageBox.Show(
                "Apakah kamu yakin ingin keluar?", "Konfirmasi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (konfirmasi == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // Biar bisa tekan Enter untuk login
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}