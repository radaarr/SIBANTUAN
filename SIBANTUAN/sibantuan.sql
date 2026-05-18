-- Bikin database dulu
CREATE DATABASE IF NOT EXISTS sibantuan;
USE sibantuan;

-- =============================================
-- TABEL 1: users
-- Nyimpen semua akun login (admin, petugas, penerima)
-- =============================================
CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nama VARCHAR(100) NOT NULL,
    username VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    role ENUM('admin_pusat', 'petugas_rtrw', 'penerima_bantuan') NOT NULL,
    is_active TINYINT(1) DEFAULT 1,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- =============================================
-- TABEL 2: penduduk
-- Nyimpen data lengkap warga
-- =============================================
CREATE TABLE penduduk (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nik VARCHAR(16) NOT NULL UNIQUE,
    nama_lengkap VARCHAR(100) NOT NULL,
    alamat TEXT NOT NULL,
    rt_rw VARCHAR(10) NOT NULL,
    kelurahan VARCHAR(100) NOT NULL,
    tanggal_lahir DATE NOT NULL,
    jenis_kelamin ENUM('laki_laki', 'perempuan') NOT NULL,
    status_ekonomi ENUM('sangat_miskin', 'miskin', 'rentan') NOT NULL,
    user_id INT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE SET NULL
);

-- =============================================
-- TABEL 3: program_bantuan
-- Nyimpen daftar program bansos yang dibuat admin
-- =============================================
CREATE TABLE program_bantuan (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nama_program VARCHAR(150) NOT NULL,
    deskripsi TEXT,
    anggaran_total DECIMAL(15,2) NOT NULL,
    kuota_penerima INT NOT NULL,
    periode_mulai DATE NOT NULL,
    periode_selesai DATE NOT NULL,
    status_program ENUM('aktif', 'selesai', 'ditutup') DEFAULT 'aktif',
    created_by INT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (created_by) REFERENCES users(id) ON DELETE SET NULL
);

-- =============================================
-- TABEL 4: permohonan
-- Nyimpen pengajuan bantuan dari warga
-- =============================================
CREATE TABLE permohonan (
    id INT AUTO_INCREMENT PRIMARY KEY,
    penduduk_id INT NOT NULL,
    program_id INT NOT NULL,
    tanggal_pengajuan DATE NOT NULL,
    status_permohonan ENUM('pending', 'disetujui', 'ditolak') DEFAULT 'pending',
    catatan_petugas TEXT,
    alasan_tolak TEXT,
    verified_by INT,
    verified_at TIMESTAMP NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (penduduk_id) REFERENCES penduduk(id) ON DELETE CASCADE,
    FOREIGN KEY (program_id) REFERENCES program_bantuan(id) ON DELETE CASCADE,
    FOREIGN KEY (verified_by) REFERENCES users(id) ON DELETE SET NULL
);

-- =============================================
-- TABEL 5: distribusi
-- Bukti penyaluran bantuan ke warga
-- =============================================
CREATE TABLE distribusi (
    id INT AUTO_INCREMENT PRIMARY KEY,
    permohonan_id INT NOT NULL,
    petugas_id INT NOT NULL,
    tanggal_distribusi DATE NOT NULL,
    jumlah_bantuan DECIMAL(15,2) NOT NULL,
    bentuk_bantuan ENUM('uang_tunai', 'sembako', 'voucher') NOT NULL,
    bukti_penerimaan VARCHAR(255),
    keterangan TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (permohonan_id) REFERENCES permohonan(id) ON DELETE CASCADE,
    FOREIGN KEY (petugas_id) REFERENCES users(id) ON DELETE CASCADE
);

-- =============================================
-- DATA AWAL: akun default buat testing
-- Password semua akun: Admin123
-- =============================================
INSERT INTO users (nama, username, password_hash, role) VALUES
('Administrator Pusat', 'admin', 'Admin123', 'admin_pusat'),
('Petugas RT 01', 'petugas01', 'Admin123', 'petugas_rtrw'),
('Budi Santoso', 'budi', 'Admin123', 'penerima_bantuan');

-- =============================================
-- DATA AWAL: contoh program bantuan
-- =============================================
INSERT INTO program_bantuan (nama_program, deskripsi, anggaran_total, kuota_penerima, periode_mulai, periode_selesai, created_by) VALUES
('BLT Dana Desa 2026', 'Bantuan Langsung Tunai untuk warga kurang mampu', 500000000, 100, '2025-01-01', '2025-12-31', 1),
('Sembako Gratis 2026', 'Penyaluran paket sembako untuk warga miskin', 200000000, 50, '2025-01-01', '2025-06-30', 1);

-- =============================================
-- DATA AWAL: contoh data penduduk
-- =============================================
INSERT INTO penduduk (nik, nama_lengkap, alamat, rt_rw, kelurahan, tanggal_lahir, jenis_kelamin, status_ekonomi, user_id) VALUES
('3201010101010001', 'Budi Santoso', 'Jl. Mawar No. 1', '001/001', 'Sidanegara', '1990-01-01', 'laki_laki', 'miskin', 3),
('3201010101010002', 'Ahmad Maulana', 'Jl. Pisang No. 5', '001/001', 'Tambakreja', '1985-05-10', 'laki_laki', 'sangat_miskin', NULL),
('3201010101010003', 'Ahmad Fauzi', 'Jl. Kenanga No. 3', '002/001', 'Donan', '1978-08-17', 'laki_laki', 'rentan', NULL);