-- =============================================
-- Migration: Hubungkan akun petugas ke data penduduk
-- Menambahkan record penduduk untuk user petugas01
-- =============================================

-- Tambah data penduduk untuk petugas01 (user_id=2) di wilayah 001/001 - Sidanegara
INSERT INTO penduduk (nik, nama_lengkap, alamat, nomor_telepon, rt_rw, kelurahan, 
                      tanggal_lahir, jenis_kelamin, status_ekonomi, status_verifikasi, user_id)
VALUES ('3201010101010099', 'Petugas RT 01', 'Kantor Kelurahan', '081234567891',
        '001/001', 'Sidanegara', '1990-01-01', 'laki_laki', 'miskin', 'Disetujui', 2)
ON DUPLICATE KEY UPDATE user_id = 2, rt_rw = '001/001', kelurahan = 'Sidanegara';
