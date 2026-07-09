using System;
using System.Data;
using System.Windows.Forms;

namespace SIBANTUAN.Forms.Admin
{
    public partial class FormProgramBantuan : Form
    {
        // Membuat DataTable global di dalam form agar data bisa ditambah, diedit, dan diubah statusnya
        private DataTable dtProgram;

        public FormProgramBantuan()
        {
            InitializeComponent();
            InisialisasiTabel();
        }

        // 1. Fungsi awal untuk membuat kolom tabel dan mengisi data default
        private void InisialisasiTabel()
        {
            dtProgram = new DataTable();
            dtProgram.Columns.Add("ID Program", typeof(string));
            dtProgram.Columns.Add("Nama Program Bantuan", typeof(string));
            dtProgram.Columns.Add("Sasaran Penerima", typeof(string));
            dtProgram.Columns.Add("Status Program", typeof(string));

            // Mengisi data awal (Dummy Data)
            dtProgram.Rows.Add("PRG-001", "Bantuan Langsung Tunai (BLT)", "Keluarga Miskin", "Aktif");
            dtProgram.Rows.Add("PRG-002", "Bantuan Sembako Pangan", "Lansia & Disabilitas", "Aktif");
            dtProgram.Rows.Add("PRG-003", "Beasiswa Pendidikan SIBANTUAN", "Anak Yatim / Pelajar", "Selesai");

            // Hubungkan DataTable ke DataGridView dgvProgram
            dgvProgram.DataSource = dtProgram;

            // Merapikan ukuran kolom agar memenuhi space tabel
            dgvProgram.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProgram.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Pilih langsung 1 baris penuh saat diklik
            dgvProgram.MultiSelect = false; // Hanya boleh pilih satu baris dalam satu waktu
        }

        // ====================================================
        // EVENT TOMBOL TAMBAH
        // ====================================================
        private void btnTambah_Click(object sender, EventArgs e)
        {

        }

        // ====================================================
        // EVENT TOMBOL EDIT
        // ====================================================
        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Validasi apakah user sudah memilih baris di tabel
            if (dgvProgram.SelectedRows.Count > 0)
            {
                // Ambil baris yang sedang aktif dipilih user
                int indexTerpilih = dgvProgram.SelectedRows[0].Index;
                DataRow baris = dtProgram.Rows[indexTerpilih];

                // Simulasi mengubah nama program bantuan yang dipilih
                string namaLama = baris["Nama Program Bantuan"].ToString();
                baris["Nama Program Bantuan"] = namaLama + " (Updated)";
                baris["Sasaran Penerima"] = "Keluarga Rentan";

                MessageBox.Show("Data program berhasil diperbarui!", "Sukses Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Silakan klik/pilih salah satu baris di tabel terlebih dahulu untuk diedit.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ====================================================
        // EVENT TOMBOL TUTUP PROGRAM
        // ====================================================
        private void btnTutup_Click(object sender, EventArgs e)
        {
            // Validasi apakah user sudah memilih baris di tabel
            if (dgvProgram.SelectedRows.Count > 0)
            {
                int indexTerpilih = dgvProgram.SelectedRows[0].Index;
                DataRow baris = dtProgram.Rows[indexTerpilih];

                // Cek jika statusnya memang masih aktif
                if (baris["Status Program"].ToString() == "Aktif")
                {
                    DialogResult dialog = MessageBox.Show($"Apakah Anda yakin ingin menutup program '{baris["Nama Program Bantuan"]}'?",
                        "Konfirmasi Tutup Program", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dialog == DialogResult.Yes)
                    {
                        // Mengubah status program dari 'Aktif' menjadi 'Selesai' / 'Nonaktif'
                        baris["Status Program"] = "Selesai";
                        MessageBox.Show("Program telah berhasil ditutup/dinonaktifkan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Program ini memang sudah berstatus Selesai/Nonaktif.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih program pada tabel yang ingin ditutup.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnTambah_Click_1(object sender, EventArgs e)
        {
            
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {

        }
    }
}