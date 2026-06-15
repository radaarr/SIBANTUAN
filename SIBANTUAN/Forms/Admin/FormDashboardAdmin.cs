using System;
using System.Windows.Forms;

namespace SIBANTUAN.Forms.Admin
{
    public partial class FormDashboardAdmin : Form
    {
        private int userId;
        private string nama;

        public FormDashboardAdmin(int userId, string nama)
        {
            InitializeComponent();
            this.userId = userId;
            this.nama = nama;
        }

        private void FormDashboardAdmin_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "Selamat datang, " + nama;
            lblDate.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
