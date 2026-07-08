using System;
using System.Drawing;
using System.Windows.Forms;

namespace SIBANTUAN.Forms
{
    public partial class FormKelolaUser : Form
    {
        // Deklarasi 2 warna utama
        private Color colorSlate = Color.FromArgb(248, 250, 252);
        private Color colorLavender = Color.FromArgb(230, 230, 250);

        public FormKelolaUser()
        {
            InitializeComponent();
            SetupUI();
            LoadDataDummy();
        }

        // ==========================================
        // 1. PENGATURAN UI & TEMA
        // ==========================================
        private void SetupUI()
        {
            this.BackColor = colorSlate;

            // Setup DataGridView agar clean dan minimalis
            dgvUser.EnableHeadersVisualStyles = false;
            dgvUser.BackgroundColor = colorSlate;
            dgvUser.GridColor = colorLavender;
            dgvUser.BorderStyle = BorderStyle.None;

            // Setup Header Tabel
            dgvUser.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvUser.ColumnHeadersDefaultCellStyle.BackColor = colorLavender;
            dgvUser.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvUser.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvUser.ColumnHeadersHeight = 40;

            // Setup Baris Terpilih
            dgvUser.DefaultCellStyle.SelectionBackColor = colorLavender;
            dgvUser.DefaultCellStyle.SelectionForeColor = Color.Black;
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
            dgvUser.ColumnCount = 4;
            dgvUser.Columns[0].Name = "ID";
            dgvUser.Columns[1].Name = "Nama Lengkap";
            dgvUser.Columns[2].Name = "Username";
            dgvUser.Columns[3].Name = "Role";

            dgvUser.Rows.Add("USR-001", "Bambang Bima", "bambang_admin", "Admin");
            dgvUser.Rows.Add("USR-002", "Chandra Pusat", "chandra_adm", "Admin");
            dgvUser.Rows.Add("USR-003", "Daffa", "daffa_ptg", "Petugas");

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

            // Simulasi insert data ke tabel
            dgvUser.Rows.Add("USR-NEW", txtNama.Text, txtUsername.Text, cmbRole.Text);
            MessageBox.Show("Akun berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearInput();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (dgvUser.SelectedRows.Count > 0)
            {
                string username = dgvUser.SelectedRows[0].Cells[2].Value.ToString();
                DialogResult confirm = MessageBox.Show($"Reset password akun '{username}' ke default?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    MessageBox.Show("Password berhasil direset.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                string nama = dgvUser.SelectedRows[0].Cells[1].Value.ToString();
                DialogResult confirm = MessageBox.Show($"Nonaktifkan akun '{nama}'?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    MessageBox.Show("Status akun berhasil dinonaktifkan.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Pilih baris data di tabel terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}