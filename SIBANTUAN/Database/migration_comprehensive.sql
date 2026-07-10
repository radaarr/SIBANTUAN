-- =============================================
-- MIGRASI KOMPREHENSIF untuk SIBANTUAN
-- Jalanin file ini SEKALI setelah sibantuan.sql
-- =============================================
USE sibantuan;

-- =============================================
-- 1. Kolom verifikasi di tabel penduduk
-- =============================================
ALTER TABLE penduduk
  ADD COLUMN IF NOT EXISTS status_verifikasi ENUM('Belum Diverifikasi','Disetujui','Ditolak') DEFAULT 'Belum Diverifikasi',
  ADD COLUMN IF NOT EXISTS catatan_verifikasi TEXT DEFAULT NULL;

UPDATE penduduk SET status_verifikasi = 'Disetujui' WHERE status_verifikasi IS NULL;

-- =============================================
-- 2. View: permohonan_view (untuk form Permohonan)
-- =============================================
CREATE OR REPLACE VIEW permohonan_view AS
SELECT 
    pm.id AS id_permohonan,
    p.id AS id_penduduk,
    p.nik,
    p.nama_lengkap AS nama_warga,
    p.alamat,
    p.status_ekonomi,
    pb.nama_program AS program_bantuan,
    pm.tanggal_pengajuan AS tgl_pengajuan,
    pm.status_permohonan AS status
FROM permohonan pm
JOIN penduduk p ON pm.penduduk_id = p.id
JOIN program_bantuan pb ON pm.program_id = pb.id;

-- =============================================
-- 3. Ubah ENUM status_permohonan — tambah 'menunggu_penyaluran' dan 'disalurkan'
-- =============================================
ALTER TABLE permohonan
  MODIFY COLUMN status_permohonan
    ENUM('pending','disetujui','ditolak','menunggu_penyaluran','disalurkan')
    DEFAULT 'pending';

-- =============================================
-- 4. View: distribusi_view — LEFT JOIN agar pending penyaluran ikut tampil
-- =============================================
CREATE OR REPLACE VIEW distribusi_view AS
SELECT 
    COALESCE(d.id, 0) AS id_distribusi,
    pm.id AS permohonan_id,
    p.nik,
    p.nama_lengkap AS nama_warga,
    pb.nama_program AS program_bantuan,
    pm.tanggal_pengajuan AS tgl_disetujui,
    d.tanggal_distribusi AS tgl_distribusi,
    d.jumlah_bantuan,
    d.bentuk_bantuan,
    d.bukti_penerimaan,
    u.nama AS dicatat_oleh,
    d.keterangan,
    pm.status_permohonan AS status_permohonan
FROM permohonan pm
JOIN penduduk p ON pm.penduduk_id = p.id
JOIN program_bantuan pb ON pm.program_id = pb.id
LEFT JOIN distribusi d ON d.permohonan_id = pm.id
LEFT JOIN users u ON d.petugas_id = u.id
WHERE pm.status_permohonan IN ('menunggu_penyaluran', 'disalurkan');
