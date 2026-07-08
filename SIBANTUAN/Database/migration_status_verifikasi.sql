-- =============================================
-- MIGRASI: Tambah kolom status_verifikasi & catatan_verifikasi ke tabel penduduk
-- =============================================
USE sibantuan;
ALTER TABLE penduduk
  ADD COLUMN status_verifikasi ENUM('Belum Diverifikasi','Disetujui','Ditolak') DEFAULT 'Belum Diverifikasi',
  ADD COLUMN catatan_verifikasi TEXT DEFAULT NULL;

-- Set data existing sebagai sudah diverifikasi
UPDATE penduduk SET status_verifikasi = 'Disetujui';
