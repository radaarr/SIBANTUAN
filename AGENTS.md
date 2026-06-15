# SIBANTUAN — Agent instructions

## Stack
- **.NET Framework 4.7.2** WinForms app (not .NET Core). Open `SIBANTUAN.slnx` in Visual Studio.
- **MySQL** backend via `MySql.Data` 9.7.0. Connection string hardcoded in `SIBANTUAN/DBHelper.cs`: `Server=localhost;Database=sibantuan;Uid=root;Pwd=;`.

## Build
```powershell
msbuild SIBANTUAN.slnx
# or open SIBANTUAN.slnx in Visual Studio and build
```
NuGet packages already restored in `packages/`.

## Database setup
Run `SIBANTUAN/Database/sibantuan.sql` against a local MySQL instance. Creates `sibantuan` DB, 5 tables (`users`, `penduduk`, `program_bantuan`, `permohonan`, `distribusi`), foreign keys, and seed data.

## Default logins (plain text passwords, no hashing)
| username | password | role |
|----------|----------|------|
| admin | Admin123 | admin_pusat |
| petugas01 | Admin123 | petugas_rtrw |
| budi | Admin123 | penerima_bantuan |

## Entry point
`SIBANTUAN/Program.cs` → `Form1` (login form in `Forms/Login/Form1.cs`). Authenticates against `users` table with plain-text password comparison.

## Forms structure
- `Forms/Login/Form1.cs` — login. Only `petugas_rtrw` role navigates to a real form (`DashboardPetugas`); `admin_pusat` and `penerima_bantuan` show a MessageBox then exit.
- `Forms/Petugas/` — all petugas forms:
  - `DashboardPetugas.cs` — dashboard with nav buttons, statistik cards. **Code-behind is empty** (all event handlers are stubs).
  - `Verifikasi.cs` — verifikasi warga CRUD. Queries `pendaftar` view/table.
  - `Permohonan.cs` — permohonan CRUD. Queries `permohonan` view/table.
  - `Distribusi.cs` — distribusi CRUD. Queries `distribusi` view/table.
  - `Laporan.cs` — **empty stub** (constructor only).

## Known issues
- **SQL injection in all Petugas CRUD forms**: `Verifikasi.cs:78`, `Permohonan.cs:90`, `Distribusi.cs:119` use string concatenation for filters and search terms instead of parameterized queries.
- **admin_pusat and penerima_bantuan dashboards not wired**: Login only shows a MessageBox and exits for these roles — no `FormDashboardAdmin` or `FormDashboardPenerima` exists.
- **Distribusi.Designer.cs missing**: Referenced in `.csproj` but does not exist on disk (only `Distribusi.cs` and `Distribusi.resx` exist).
- **DBHelper.cs imports `System.Data.SqlClient`** — unused import (`MySql.Data.MySqlClient` is the actual provider used).

## Tests
None.
