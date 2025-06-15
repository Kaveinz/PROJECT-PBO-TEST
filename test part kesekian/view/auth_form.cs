using project_PBO.View;
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
using test_part_kesekian.view;

namespace test_part_kesekian
{
    public partial class auth_form : Form
    {
        public static User CurrentUser { get; private set; }

        public auth_form()
        {
            InitializeComponent();
        }        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                
                // Console.WriteLine("Klik button tess");

                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Username atau Password Tidak Boleh Kosong!", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                
              

                var user = AuthController.Instance.Login(username, password);
                
               
           
                
                if (user != null)
                {
                    CurrentUser = user;
                    
                  
                    MessageBox.Show($"Selamat Datang, {user.NamaLengkap}!\nKamu telah berhasil Login sebagai {user.Role}", 
                        "Login Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                   
                    this.Hide();
                    
                    if (user.Role == UserRole.Pengguna)
                    {
                        var mainForm = new layout_pelanggan();
                        mainForm.ShowDialog();
                    }
                    else if (user.Role == UserRole.Admin)
                    {
                        var mainForm = new layout_admin();
                        mainForm.ShowDialog();
                    }
                    
                    this.Close();
                }
                else
                {
                
                    Console.WriteLine("Login Gagal");
                    MessageBox.Show("Username atau Password Salah!\nSilakan coba lagi.", 
                        "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (AuthenticationException ex)
            {
                this.Cursor = Cursors.Default;
                Console.WriteLine($"Authentication error: {ex.Message}");
                MessageBox.Show($"Login Gagal: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                txtPassword.Clear();
                txtPassword.Focus();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Console.WriteLine($"Unexpected error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}\n\nSilakan periksa koneksi database.", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private void auth_form_Load(object sender, EventArgs e)
        {
            
        }

       
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var registerForm = new form_register();
            registerForm.Show();
            
        }
    }
}
