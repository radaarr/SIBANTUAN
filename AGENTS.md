# SIBANTUAN — Agent instructions

## Stack
- **.NET Framework 4.7.2** WinForms app (not .NET Core). Open `SIBANTUAN.slnx` in Visual Studio.
- **MySQL** backend via `MySql.Data` 9.7.0. Connection string hardcoded in `SIBANTUAN/DBHelper.cs`: `Server=localhost;Database=sibantuan;Uid=root;Pwd=;`.

## Build
```powershell
msbuild SIBANTUAN.slnx
# or open SIBANTUAN.slnx in Visual Studio and build
```

## Database setup
Run `SIBANTUAN/Database/sibantuan.sql` against a local MySQL instance. It creates the `sibantuan` DB, tables (`users`, `penduduk`, `program_bantuan`, `permohonan`, `distribusi`), and seed data.

## Default logins (plain text passwords, no hashing)
| username | password | role |
|----------|----------|------|
| admin | Admin123 | admin_pusat |
| petugas01 | Admin123 | petugas_rtrw |
| budi | Admin123 | penerima_bantuan |

## Entry point
`SIBANTUAN/Program.cs` → `Form1` (login form in `Forms/Login/`).

## Forms structure
- `Forms/Login/Form1.cs` — login with username/password against `users` table
- `Forms/Petugas/DashboardPetugas.cs` — petugas dashboard (stub, wired but empty)

## Known issue: .csproj is stale
`SIBANTUAN.csproj` references non-existent files `FormPetugas.cs`, `FormVerifikasi.cs`, `FormPenyaluran.cs` in `Forms/Petugas/`. The actual file is `DashboardPetugas.cs`. The `.csproj` needs updating before it will build.

## Tests
None — no test project exists in the repo.
