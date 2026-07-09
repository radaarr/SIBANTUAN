-- =============================================
-- MIGRASI: VIEW untuk kompatibilitas kode
-- =============================================
-- View ini disesuaikan dengan struktur database di sibantuan.sql
-- (tabel: penduduk, program_bantuan, permohonan, distribusi, users)

-- View: permohonan_view (untuk form Permohonan)
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

-- View: distribusi_view (untuk form Distribusi)
CREATE OR REPLACE VIEW distribusi_view AS
SELECT 
    d.id AS id_distribusi,
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
FROM distribusi d
JOIN permohonan pm ON d.permohonan_id = pm.id
JOIN penduduk p ON pm.penduduk_id = p.id
JOIN program_bantuan pb ON pm.program_id = pb.id
JOIN users u ON d.petugas_id = u.id;
