using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test_part_kesekian.controller;
using test_part_kesekian.models;

namespace test_part_kesekian.view
{
    public partial class form_register : Form
    {
        public form_register()
        {
            InitializeComponent();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void Login_button_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string namaLengkap = txtNama_lengkap.Text.Trim();
            string email = txtEmail.Text.Trim();
            string nomorHP = txtNoHp.Text.Trim();

            if (string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(namaLengkap) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(nomorHP))
            {
                MessageBox.Show("Semua field wajib diisi!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newUser = new User
            {
                Username = username,
                Password = password,
                Role = "pengguna",
                nama_lengkap = namaLengkap,
                email = email,
                nomor_hp = nomorHP
            };

            bool success = AuthController.Register(newUser);
            if (success)
            {
                MessageBox.Show("Registrasi berhasil!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); 
                var auth_form = new auth_form();
                auth_form.Show();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
