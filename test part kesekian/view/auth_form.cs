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
                // Debug info
                Console.WriteLine("Login button clicked");
                
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text;

                // Input validation
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Username dan password tidak boleh kosong!", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Show loading cursor
                this.Cursor = Cursors.WaitCursor;
                
                Console.WriteLine($"Attempting login for user: {username}");

                var user = AuthController.Instance.Login(username, password);
                
                // Reset cursor
                this.Cursor = Cursors.Default;
                
                if (user != null)
                {
                    CurrentUser = user;
                    
                    Console.WriteLine($"Login successful for: {user.Username}, Role: {user.Role}");
                    
                    // Show welcome message
                    MessageBox.Show($"Selamat datang, {user.NamaLengkap}!\nLogin sebagai {user.Role}", 
                        "Login Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Navigate to appropriate form based on role
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
                    // Login failed
                    Console.WriteLine("Login failed - user is null");
                    MessageBox.Show("Username atau password salah!\nSilakan coba lagi.", 
                        "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    // Clear password field
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (AuthenticationException ex)
            {
                this.Cursor = Cursors.Default;
                Console.WriteLine($"Authentication error: {ex.Message}");
                MessageBox.Show($"Login gagal: {ex.Message}", "Error", 
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
            // Test database connection on form load
            TestDatabaseConnection();
        }

        private void TestDatabaseConnection()
        {
            try
            {
                Console.WriteLine("Testing database connection...");
                bool isConnected = DatabaseHelper.TestConnection();
                  if (isConnected)
                {
                    Console.WriteLine("✅ Database connection successful!");
                    
                    // Test if we can query users table
                    var testQuery = "SELECT COUNT(*) FROM users";
                    var result = DatabaseHelper.ExecuteScalar(testQuery);
                    Console.WriteLine($"✅ Found {result} users in database");
                    
                    // Fix sequence synchronization issue
                    Console.WriteLine("🔧 Checking and fixing sequence synchronization...");
                    DatabaseHelper.FixReservationSequence();
                }
                else
                {
                    Console.WriteLine("❌ Database connection failed!");
                    MessageBox.Show("Warning: Cannot connect to database.\nPlease check your PostgreSQL connection.", 
                        "Database Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Database test error: {ex.Message}");
                MessageBox.Show($"Database Error: {ex.Message}\n\nPlease check:\n" +
                              "1. PostgreSQL is running\n" +
                              "2. Database 'MBOK_WO_RESERVE' exists\n" +
                              "3. Connection string is correct", 
                              "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
