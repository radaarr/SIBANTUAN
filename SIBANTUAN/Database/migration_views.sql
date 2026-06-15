-- =============================================
-- MIGRASI: VIEW untuk kompatibilitas kode
-- =============================================

-- View: pendaftar (untuk form Verifikasi)
-- Menggabungkan data penduduk dengan status permohonan terakhir
CREATE OR REPLACE VIEW pendaftar AS
SELECT 
    p.id,
    p.nik,
    p.nama_lengkap,
    p.alamat,
    p.rt_rw,
    p.kelurahan,
    p.tanggal_lahir AS tgl_lahir,
    CASE WHEN p.jenis_kelamin = 'laki_laki' THEN 'Laki-laki' ELSE 'Perempuan' END AS jenis_kelamin,
    p.status_ekonomi,
    p.created_at AS tgl_daftar,
    COALESCE(
        (SELECT CASE 
            WHEN status_permohonan = 'pending' THEN 'Pending'
            WHEN status_permohonan = 'disetujui' THEN 'Disetujui'
            WHEN status_permohonan = 'ditolak' THEN 'Ditolak'
         END
         FROM permohonan WHERE penduduk_id = p.id ORDER BY created_at DESC LIMIT 1),
        'Belum Diverifikasi'
    ) AS status
FROM penduduk p;

-- View: distribusi_view (untuk form Distribusi)
-- Menggabungkan distribusi dengan data penduduk, program, petugas
CREATE OR REPLACE VIEW distribusi_view AS
SELECT 
    d.id,
    d.permohonan_id,
    pd.nik,
    pd.nama_lengkap AS nama_warga,
    pb.nama_program AS program_bantuan,
    p.tanggal_pengajuan AS tgl_ajuan,
    p.verified_at AS tgl_disetujui,
    CASE 
        WHEN p.status_permohonan = 'pending' THEN 'Pending'
        WHEN p.status_permohonan = 'disetujui' THEN 'Disetujui'
        WHEN p.status_permohonan = 'ditolak' THEN 'Ditolak'
    END AS status_permohonan,
    d.tanggal_distribusi AS tgl_distribusi,
    d.jumlah_bantuan,
    CASE 
        WHEN d.bentuk_bantuan = 'uang_tunai' THEN 'Uang Tunai'
        WHEN d.bentuk_bantuan = 'sembako' THEN 'Sembako'
        WHEN d.bentuk_bantuan = 'voucher' THEN 'Voucher'
    END AS bentuk_bantuan,
    d.bukti_penerimaan,
    d.keterangan,
    pt.nama AS dicatat_oleh
FROM distribusi d
JOIN permohonan p ON d.permohonan_id = p.id
JOIN penduduk pd ON p.penduduk_id = pd.id
JOIN program_bantuan pb ON p.program_id = pb.id
JOIN users pt ON d.petugas_id = pt.id;

-- View: permohonan_view (untuk form Permohonan)
-- Menggabungkan permohonan dengan data penduduk dan program
CREATE OR REPLACE VIEW permohonan_view AS
SELECT 
    p.id,
    pd.nik,
    pd.nama_lengkap AS nama_warga,
    pd.alamat,
    pd.status_ekonomi,
    pb.nama_program AS program_bantuan,
    pb.kuota_penerima AS kuota_program,
    CONCAT(pb.periode_mulai, ' - ', pb.periode_selesai) AS periode_program,
    p.tanggal_pengajuan AS tgl_pengajuan,
    p.tanggal_pengajuan AS tgl_ajuan,
    CASE 
        WHEN p.status_permohonan = 'pending' THEN 'Pending'
        WHEN p.status_permohonan = 'disetujui' THEN 'Disetujui'
        WHEN p.status_permohonan = 'ditolak' THEN 'Ditolak'
    END AS status,
    p.catatan_petugas,
    p.alasan_tolak,
    p.verified_by,
    p.verified_at
FROM permohonan p
JOIN penduduk pd ON p.penduduk_id = pd.id
JOIN program_bantuan pb ON p.program_id = pb.id;

-- View: program (untuk form Laporan)
CREATE OR REPLACE VIEW program AS
SELECT 
    id,
    nama_program,
    deskripsi,
    anggaran_total,
    kuota_penerima,
    periode_mulai,
    periode_selesai,
    CASE 
        WHEN status_program = 'aktif' THEN 'Aktif'
        WHEN status_program = 'selesai' THEN 'Selesai'
        WHEN status_program = 'ditutup' THEN 'Ditutup'
    END AS status
FROM program_bantuan;
