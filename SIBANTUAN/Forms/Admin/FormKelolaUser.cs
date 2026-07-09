using System;
using System.Drawing;
using System.Windows.Forms;

namespace SIBANTUAN.Forms
{
    public partial class FormKelolaUser : Form
    {
        // ==========================================
        // DEKLARASI WARNA TEMA (Serasi dengan Admin)
        // ==========================================
        private Color warnaBackground = Color.FromArgb(245, 246, 250); // Background keabu-abuan modern
        private Color warnaAksen = Color.FromArgb(52, 152, 219);       // Biru untuk Header Tabel
        private Color warnaPilih = Color.FromArgb(230, 242, 255);      // Biru muda untuk baris terpilih

        public FormKelolaUser()
        {
            InitializeComponent();
            SetupModernUI();
            LoadDataDummy(); // Nanti bisa diubah menjadi LoadDataFromDatabase() seperti form sebelumnya
        }

        // ==========================================
        // 1. PENGATURAN UI & TEMA MODERN
        // ==========================================
        private void SetupModernUI()
        {
            // Menyamakan warna background form dengan panel dashboard
            this.BackColor = warnaBackground;

            // Setup DataGridView agar clean dan selaras
            dgvUser.EnableHeadersVisualStyles = false;
            dgvUser.BackgroundColor = warnaBackground;
            dgvUser.GridColor = Color.LightGray;
            dgvUser.BorderStyle = BorderStyle.None;
            dgvUser.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Garis horizontal tipis
            dgvUser.AllowUserToAddRows = false; // Menghilangkan baris kosong di bawah

            // Setup Header Tabel
            dgvUser.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvUser.ColumnHeadersDefaultCellStyle.BackColor = warnaAksen;
            dgvUser.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUser.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvUser.ColumnHeadersHeight = 40;

            // Setup Baris Terpilih
            dgvUser.DefaultCellStyle.SelectionBackColor = warnaPilih;
            dgvUser.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvUser.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            dgvUser.RowTemplate.Height = 35;

            // Setup ComboBox Role
            cmbRole.Items.Clear();
            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("Petugas");
            cmbRole.SelectedIndex = 0;
        }

        // ==========================================
        // 2. DATA SEMENTARA (Menunggu Database)
        // ==========================================
        private void LoadDataDummy()
        {
            dgvUser.ColumnCount = 5;
            dgvUser.Columns[0].Name = "ID";
            dgvUser.Columns[1].Name = "Nama Lengkap";
            dgvUser.Columns[2].Name = "Username";
            dgvUser.Columns[3].Name = "Role";
            dgvUser.Columns[4].Name = "Status";

            dgvUser.Rows.Add("USR-001", "Bambang Bima", "bambang_admin", "Admin", "Aktif");
            dgvUser.Rows.Add("USR-002", "Chandra Pusat", "chandra_adm", "Admin", "Aktif");
            dgvUser.Rows.Add("USR-003", "Daffa", "daffa_ptg", "Petugas", "Aktif");

            dgvUser.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUser.ClearSelection();
        }

        private void ClearInput()
        {
            txtNama.Clear();
            txtUsername.Clear();
            cmbRole.SelectedIndex = 0;
            txtNama.Focus();
        }

        // ==========================================
        // 3. EVENT HANDLER TOMBOL
        // ==========================================
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNama.Text) || string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Nama dan Username tidak boleh kosong.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Generate ID Dummy Sederhana
            string newId = "USR-00" + (dgvUser.Rows.Count + 1);

            // Simulasi insert data ke tabel
            dgvUser.Rows.Add(newId, txtNama.Text, txtUsername.Text, cmbRole.Text, "Aktif");
            MessageBox.Show("Akun berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearInput();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (dgvUser.SelectedRows.Count > 0)
            {
                string username = dgvUser.SelectedRows[0].Cells["Username"].Value.ToString();
                DialogResult confirm = MessageBox.Show($"Apakah Anda yakin ingin mereset password akun '{username}' ke default (123456)?", "Konfirmasi Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    MessageBox.Show($"Password untuk '{username}' berhasil direset.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Pilih baris data di tabel terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnNonaktif_Click(object sender, EventArgs e)
        {
            if (dgvUser.SelectedRows.Count > 0)
            {
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
                    dgvUser.SelectedRows[0].Cells["Status"].Value = "Nonaktif";
                    MessageBox.Show("Status akun berhasil dinonaktifkan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Pilih baris data di tabel terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ==========================================
        // MENCEGAH ERROR DESIGNER
        // ==========================================
        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}