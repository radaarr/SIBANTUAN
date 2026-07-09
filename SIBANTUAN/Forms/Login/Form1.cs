using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Drawing;
using System.Windows.Forms;
using SIBANTUAN.Forms;
using SIBANTUAN.Forms.Petugas;
using SIBANTUAN.Forms.Penerima;

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
                        int userId = reader.GetInt32("id");
                        string nama = reader.GetString("nama");
                        string role = reader.GetString("role");
                        reader.Close();

                        this.Hide();

                        Session.UserId = userId;
                        Session.Username = txtUsername.Text.Trim();
                        Session.Nama = nama;
                        Session.Role = role;

                        if (role == "petugas_rtrw")
                        {
                            MySqlCommand wilCmd = new MySqlCommand(
                                "SELECT id, rt_rw, kelurahan FROM penduduk " +
                                "WHERE user_id = @uid LIMIT 1", conn);
                            wilCmd.Parameters.AddWithValue("@uid", userId);
                            MySqlDataReader wilReader = wilCmd.ExecuteReader();
                            if (wilReader.Read())
                            {
                                Session.PendudukId = wilReader.GetInt32("id");
                                Session.WilayahRtRw = wilReader.GetString("rt_rw");
                                Session.WilayahKelurahan = wilReader.GetString("kelurahan");
                            }
                            else
                            {
                                MessageBox.Show("Akun petugas belum terhubung ke data penduduk.\r\n" +
                                    "Silakan hubungi admin untuk menautkan akun ini ke penduduk.",
                                    "Wilayah Tidak Ditemukan",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            wilReader.Close();
                        }

                        if (role == "admin_pusat")
                        {
                            FormAdmin formAdmin = new FormAdmin();
                            formAdmin.ShowDialog();
                        }
                        else if (role == "petugas_rtrw")
                        {
                            DashboardPetugas dashboardPetugas = new DashboardPetugas(userId, nama);
                            dashboardPetugas.ShowDialog();
                        }
                        else if (role == "penerima_bantuan")
                        {
                            DashboardPenerima formPenerima = new DashboardPenerima();
                            formPenerima.ShowDialog();
                        }

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

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void BtnDaftar_Click(object sender, EventArgs e)
        {
            FormDaftar daftar = new FormDaftar();
            daftar.ShowDialog();
        }
    }
}