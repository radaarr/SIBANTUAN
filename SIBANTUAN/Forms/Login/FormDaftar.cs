using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SIBANTUAN
{
    public class FormDaftar : Form
    {
        private TextBox txtNik, txtNama, txtAlamat, txtRtRw, txtKelurahan, txtTglLahir, txtNoTelp, txtUsername, txtPassword;
        private ComboBox cmbJk;
        private Button btnDaftar, btnBatal;

        public FormDaftar()
        {
            this.Text = "SIBANTUAN - Daftar Warga Baru";
            this.Size = new Size(500, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Lavender;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Label lblTitle = new Label();
            lblTitle.Text = "Pendaftaran Warga Baru";
            lblTitle.Font = new Font("Microsoft YaHei", 14, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 20);
            this.Controls.Add(lblTitle);

            Label lblSub = new Label();
            lblSub.Text = "Isi data diri untuk mendaftar sebagai penerima bantuan";
            lblSub.Font = new Font("Microsoft Sans Serif", 9);
            lblSub.AutoSize = true;
            lblSub.Location = new Point(20, 55);
            this.Controls.Add(lblSub);

            int y = 90;
            int labelW = 120;
            int inputW = 280;
            int xLabel = 30;
            int xInput = 160;
            int gap = 35;

            AddField("NIK:", xLabel, y, labelW, ref txtNik, xInput, y, inputW); y += gap;
            AddField("Nama Lengkap:", xLabel, y, labelW, ref txtNama, xInput, y, inputW); y += gap;
            AddField("Alamat:", xLabel, y, labelW, ref txtAlamat, xInput, y, inputW); y += gap;
            AddField("RT/RW:", xLabel, y, labelW, ref txtRtRw, xInput, y, inputW); y += gap;
            AddField("Kelurahan:", xLabel, y, labelW, ref txtKelurahan, xInput, y, inputW); y += gap;
            AddField("Tgl Lahir:", xLabel, y, labelW, ref txtTglLahir, xInput, y, inputW);
            Label hint = new Label(); hint.Text = "(yyyy-MM-dd)"; hint.AutoSize = true; hint.ForeColor = Color.Gray; hint.Location = new Point(xInput + inputW + 5, y + 3); hint.Font = new Font("Microsoft Sans Serif", 8); this.Controls.Add(hint);
            y += gap;

            Label lblJk = new Label(); lblJk.Text = "Jenis Kelamin:"; lblJk.Location = new Point(xLabel, y); lblJk.Size = new Size(labelW, 25); this.Controls.Add(lblJk);
            cmbJk = new ComboBox(); cmbJk.Items.AddRange(new[] { "laki_laki", "perempuan" }); cmbJk.SelectedIndex = 0;
            cmbJk.Location = new Point(xInput, y); cmbJk.Size = new Size(inputW, 25); cmbJk.DropDownStyle = ComboBoxStyle.DropDownList; this.Controls.Add(cmbJk);
            y += gap;

            AddField("No. Telepon:", xLabel, y, labelW, ref txtNoTelp, xInput, y, inputW); y += gap;

            Label lblLine = new Label();
            lblLine.Text = "—————————— Data Akun ——————————";
            lblLine.AutoSize = true; lblLine.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
            lblLine.Location = new Point(20, y + 5); this.Controls.Add(lblLine);
            y += 35;

            AddField("Username:", xLabel, y, labelW, ref txtUsername, xInput, y, inputW); y += gap;
            AddField("Password:", xLabel, y, labelW, ref txtPassword, xInput, y, inputW);
            Label passHint = new Label(); passHint.Text = "(min. 6 karakter)"; passHint.AutoSize = true; passHint.ForeColor = Color.Gray; passHint.Location = new Point(xInput + inputW + 5, y + 3); passHint.Font = new Font("Microsoft Sans Serif", 8); this.Controls.Add(passHint);
            y += gap + 20;

            btnDaftar = new Button();
            btnDaftar.Text = "Daftar";
            btnDaftar.BackColor = Color.FromArgb(46, 204, 113);
            btnDaftar.ForeColor = Color.White;
            btnDaftar.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);
            btnDaftar.Location = new Point(xInput, y);
            btnDaftar.Size = new Size(130, 40);
            btnDaftar.Click += BtnDaftar_Click;
            this.Controls.Add(btnDaftar);

            btnBatal = new Button();
            btnBatal.Text = "Batal";
            btnBatal.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);
            btnBatal.Location = new Point(xInput + 150, y);
            btnBatal.Size = new Size(130, 40);
            btnBatal.Click += (s, e) => this.Close();
            this.Controls.Add(btnBatal);
        }

        private void AddField(string label, int xLabel, int y, int w, ref TextBox tb, int xInput, int yInput, int wInput)
        {
            Label lbl = new Label();
            lbl.Text = label;
            lbl.Location = new Point(xLabel, yInput + 3);
            lbl.Size = new Size(w, 25);
            this.Controls.Add(lbl);

            tb = new TextBox();
            tb.Location = new Point(xInput, yInput);
            tb.Size = new Size(wInput, 25);
            this.Controls.Add(tb);
        }

        private void BtnDaftar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNik.Text) || string.IsNullOrEmpty(txtNama.Text) || string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("NIK, Nama, Username, dan Password wajib diisi.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Password minimal 6 karakter.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    // Cek NIK sudah terdaftar
                    MySqlCommand cekNik = new MySqlCommand("SELECT COUNT(*) FROM penduduk WHERE nik = @nik", conn);
                    cekNik.Parameters.AddWithValue("@nik", txtNik.Text.Trim());
                    if (Convert.ToInt32(cekNik.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("NIK sudah terdaftar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Cek username sudah dipakai
                    MySqlCommand cekUser = new MySqlCommand("SELECT COUNT(*) FROM users WHERE username = @username", conn);
                    cekUser.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                    if (Convert.ToInt32(cekUser.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("Username sudah dipakai.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Insert user dulu
                    MySqlCommand insertUser = new MySqlCommand(
                        "INSERT INTO users (nama, username, password_hash, role, is_active) VALUES (@nama, @username, @pass, 'penerima_bantuan', 1)",
                        conn);
                    insertUser.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                    insertUser.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                    insertUser.Parameters.AddWithValue("@pass", txtPassword.Text);
                    insertUser.ExecuteNonQuery();

                    // Ambil id user yang baru dibuat
                    MySqlCommand lastId = new MySqlCommand("SELECT LAST_INSERT_ID()", conn);
                    long userId = Convert.ToInt64(lastId.ExecuteScalar());

                    // Insert penduduk
                    string tglLahir = string.IsNullOrEmpty(txtTglLahir.Text) ? "2000-01-01" : txtTglLahir.Text;
                    MySqlCommand insertPenduduk = new MySqlCommand(
                        @"INSERT INTO penduduk (nik, nama_lengkap, alamat, rt_rw, kelurahan, tanggal_lahir, jenis_kelamin, status_ekonomi, nomor_telepon, user_id, status_verifikasi)
                          VALUES (@nik, @nama, @alamat, @rtrw, @kel, @tgl, @jk, 'miskin', @telp, @uid, 'Belum Diverifikasi')",
                        conn);
                    insertPenduduk.Parameters.AddWithValue("@nik", txtNik.Text.Trim());
                    insertPenduduk.Parameters.AddWithValue("@nama", txtNama.Text.Trim());
                    insertPenduduk.Parameters.AddWithValue("@alamat", txtAlamat.Text);
                    insertPenduduk.Parameters.AddWithValue("@rtrw", txtRtRw.Text);
                    insertPenduduk.Parameters.AddWithValue("@kel", txtKelurahan.Text);
                    insertPenduduk.Parameters.AddWithValue("@tgl", tglLahir);
                    insertPenduduk.Parameters.AddWithValue("@jk", cmbJk.SelectedItem.ToString());
                    insertPenduduk.Parameters.AddWithValue("@telp", txtNoTelp.Text);
                    insertPenduduk.Parameters.AddWithValue("@uid", userId);
                    insertPenduduk.ExecuteNonQuery();

                    MessageBox.Show("Pendaftaran berhasil! Silakan login dengan akun yang baru dibuat.\nStatus verifikasi: Belum Diverifikasi — hubungi petugas untuk verifikasi.",
                        "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
