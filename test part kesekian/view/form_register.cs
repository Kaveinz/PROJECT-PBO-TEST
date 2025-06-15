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

        private void Login_button_Click(object sender, EventArgs e)
        {
            try
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
                    MessageBox.Show("Semua field wajib diisi!", "Validasi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                
                var newUser = new User(username, password, namaLengkap, email, nomorHP)
                {
                    Role = UserRole.Pengguna
                };

                
                if (!newUser.ValidateData())
                {
                    MessageBox.Show("Data yang dimasukkan tidak valid!\n" +
                                  "- Username minimal 3 karakter\n" +
                                  "- Password minimal 6 karakter\n" +
                                  "- Email harus valid\n" +
                                  "- Nomor HP minimal 10 digit", 
                                  "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                
                bool success = AuthController.Instance.Register(newUser);
                if (success)
                {
                    MessageBox.Show($"Registrasi berhasil!\nSelamat datang, {newUser.NamaLengkap}!", 
                        "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    this.Hide();
                    var authForm = new auth_form();
                    authForm.Show();
                    this.Close();
                }
            }
            catch (AuthenticationException ex)
            {
                MessageBox.Show($"Registrasi gagal: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Data tidak valid: {ex.Message}", "Validasi Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
