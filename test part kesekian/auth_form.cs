using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_part_kesekian
{
    public partial class auth_form : Form
    {
        public static User CurrentUser { get; private set; }

        public auth_form()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            var user = AuthController.Login(username, password);
            if (user != null)
            {
                CurrentUser = user;
                MessageBox.Show($"{user.Role} berhasil login!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                using (var mainForm = new MainForm())
                {
                    mainForm.ShowDialog();
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Login gagal! Username atau password salah.", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
