using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIBANTUAN.Forms.Penerima
{
    public partial class AjukanPermohonan : Form
    {
        private DataTable _dtProgram;
        public AjukanPermohonan()
        {
            InitializeComponent();
        }

        // ================================================================
        // LOAD — isi NIK, Nama, dan dropdown program saat form dibuka
        // ================================================================
        private void AjukanPermohonan_Load(object sender, EventArgs e)
        {
            IsiDataDiri();
            LoadProgram();
        }

        // ================================================================
        // 1. ISI NIK & NAMA LENGKAP (otomatis dari Session)
        // ================================================================
        private void IsiDataDiri()
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    var cmd = new MySqlCommand(
                        "SELECT nik, nama_lengkap FROM penduduk WHERE user_id = @uid", conn);
                    cmd.Parameters.AddWithValue("@uid", Session.UserId);

                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            txtNIK.Text = r["nik"].ToString();
                            txtNama.Text = r["nama_lengkap"].ToString();

                            // ReadOnly supaya tidak bisa diedit manual
                            txtNIK.ReadOnly = true;
                            txtNama.ReadOnly = true;
                        }
                        else
                        {
                            txtNIK.Text = "Data tidak ditemukan";
                            txtNama.Text = "Data tidak ditemukan";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data diri:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================================================================
        // 2. ISI DROPDOWN PROGRAM BANTUAN
        // ================================================================
        private void LoadProgram()
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    // Hitung sisa kuota = kuota - jumlah yang sudah disetujui
                    var cmd = new MySqlCommand(@"
                        SELECT pb.id,
                               pb.nama_program,
                               pb.deskripsi,
                               pb.kuota_penerima,
                               pb.periode_mulai,
                               pb.periode_selesai,
                               (pb.kuota_penerima - 
                                IFNULL((
                                    SELECT COUNT(*) FROM permohonan
                                    WHERE program_id = pb.id
                                    AND status_permohonan = 'disetujui'
                                ), 0)
                               ) AS sisa_kuota
                        FROM program_bantuan pb
                        WHERE pb.status_program = 'aktif'
                        AND pb.periode_selesai >= CURDATE()", conn);

                    _dtProgram = new DataTable();
                    new MySqlDataAdapter(cmd).Fill(_dtProgram);

                    cmbProgram.Items.Clear();
                    foreach (DataRow row in _dtProgram.Rows)
                    {
                        int sisa = Convert.ToInt32(row["sisa_kuota"]);
                        string tampil = sisa > 0
                            ? row["nama_program"] + " — sisa " + sisa + " kuota"
                            : row["nama_program"] + " — KUOTA PENUH";
                        cmbProgram.Items.Add(tampil);
                    }

                    if (cmbProgram.Items.Count > 0)
                        cmbProgram.SelectedIndex = 0;
                    else
                        cmbProgram.Items.Add("Tidak ada program aktif saat ini");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat program bantuan:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================================================================
        // 3. TOMBOL KIRIM PERMOHONAN
        // ================================================================
        private void btnKirim_Click(object sender, EventArgs e)
        {
            // Validasi: harus pilih program
            if (cmbProgram.SelectedIndex < 0 || _dtProgram == null ||
                _dtProgram.Rows.Count == 0)
            {
                MessageBox.Show("Pilih program bantuan terlebih dahulu.",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi: data diri harus ada
            if (txtNIK.Text == "Data tidak ditemukan" || string.IsNullOrWhiteSpace(txtNIK.Text))
            {
                MessageBox.Show(
                    "Data kependudukan kamu belum ada.\n" +
                    "Hubungi Petugas RT/RW untuk menginput data kamu.",
                    "Data Tidak Ditemukan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = _dtProgram.Rows[cmbProgram.SelectedIndex];
            int progId = Convert.ToInt32(row["id"]);
            int sisa = Convert.ToInt32(row["sisa_kuota"]);

            // Validasi: kuota tidak boleh penuh
            if (sisa <= 0)
            {
                MessageBox.Show("Kuota program ini sudah penuh.",
                    "Kuota Penuh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    // Ambil penduduk_id dari NIK
                    var cmdPenduduk = new MySqlCommand(
                        "SELECT id FROM penduduk WHERE user_id = @uid", conn);
                    cmdPenduduk.Parameters.AddWithValue("@uid", Session.UserId);
                    int pendudukId = Convert.ToInt32(cmdPenduduk.ExecuteScalar());

                    if (pendudukId == 0)
                    {
                        MessageBox.Show("Data penduduk tidak ditemukan.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Cek duplikat — sudah pernah ajukan program yang sama?
                    var cmdCek = new MySqlCommand(@"
                        SELECT COUNT(*) FROM permohonan
                        WHERE penduduk_id = @pid
                        AND program_id   = @progid
                        AND status_permohonan IN ('pending', 'disetujui')", conn);
                    cmdCek.Parameters.AddWithValue("@pid", pendudukId);
                    cmdCek.Parameters.AddWithValue("@progid", progId);
                    int sudahAda = Convert.ToInt32(cmdCek.ExecuteScalar());

                    if (sudahAda > 0)
                    {
                        MessageBox.Show(
                            "Kamu sudah pernah mengajukan permohonan\n" +
                            "untuk program bantuan ini.",
                            "Permohonan Duplikat",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Insert permohonan baru
                    var cmdInsert = new MySqlCommand(@"
                        INSERT INTO permohonan
                            (penduduk_id, program_id, tanggal_pengajuan,
                             status_permohonan, catatan_petugas)
                        VALUES
                            (@pid, @progid, CURDATE(), 'pending', @ket)", conn);
                    cmdInsert.Parameters.AddWithValue("@pid", pendudukId);
                    cmdInsert.Parameters.AddWithValue("@progid", progId);
                    cmdInsert.Parameters.AddWithValue("@ket", txtKeterangan.Text.Trim());
                    cmdInsert.ExecuteNonQuery();

                    MessageBox.Show(
                        "Permohonan berhasil dikirim!\n\n" +
                        "Status: Pending\n" +
                        "Petugas RT/RW akan segera memverifikasi.",
                        "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset form
                    txtKeterangan.Clear();
                    LoadProgram();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal mengirim permohonan:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================================================================
        // TOMBOL NAVIGASI SIDEBAR
        // ================================================================
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }

        private void btnAjukanPermohonan_Click(object sender, EventArgs e)
        {
            var form = new AjukanPermohonan();
            if (form.ShowDialog() == DialogResult.Abort)
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        private void btnStatusPermohonan_Click(object sender, EventArgs e)
        {
            var form = new StatusPermohonan();
            if (form.ShowDialog() == DialogResult.Abort)
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        private void btnRiwayatBantuan_Click(object sender, EventArgs e)
        {
            var form = new RiwayatBantuan();
            if (form.ShowDialog() == DialogResult.Abort)
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var konfirmasi = MessageBox.Show(
                "Yakin ingin logout?", "Konfirmasi Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (konfirmasi == DialogResult.Yes)
            {
                Session.UserId = 0;
                Session.Username = null;
                Session.Nama = null;
                Session.Role = null;
                this.Close(); // tutup form saat ini
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
