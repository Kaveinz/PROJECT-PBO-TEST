using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_part_kesekian.models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace test_part_kesekian.controller
{
    // Interface for authentication operations (Abstraction)
    public interface IAuthenticationService
    {
        User Login(string username, string password);
        bool Register(User newUser);
       
    }

    // Abstract base class for authentication (Abstraction & Inheritance)
    public abstract class BaseAuthController : IAuthenticationService
    {
        protected readonly IDatabaseOperations _database;
        protected User _currentUser;

        protected BaseAuthController(IDatabaseOperations database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public User CurrentUser
        {
            get => _currentUser;
            protected set => _currentUser = value;
        }

     
        public abstract User Login(string username, string password);
        public abstract bool Register(User newUser);

   

   
        protected virtual bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        
        protected virtual bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrWhiteSpace(phoneNumber) &&
                           phoneNumber.Length >= 10 &&
                           phoneNumber.All(c => char.IsDigit(c) || c == '+' || c == '-' || c == ' ');
        }
    }

  
    public class AuthController : BaseAuthController
    {
        private static AuthController _instance;
        private static readonly object _lock = new object();

   
        public static AuthController Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new AuthController(DatabaseHelper.Instance);
                    }
                }
                return _instance;
            }
        }

       
        private AuthController(IDatabaseOperations database) : base(database) { }       
        public override User Login(string username, string password)
        {
           
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username atau Password Tidak Boleh Kosong!");

            try
            {
               

                string query = @"
                    SELECT id, username, role, nama_lengkap, email, nomor_hp, created_at, status
                    FROM users
                    WHERE username = @u AND password_hash = crypt(@p, password_hash) AND status = 'Aktif'";

                var parameters = new NpgsqlParameter[]
                {
                    new("@u", username.Trim().ToLower()),
                    new("@p", password)
                };

               

                var result = DatabaseHelper.Instance.GetSingle(query, reader =>
                {
                  

                    var user = new User
                    {
                        Id = reader.GetInt32("id"),
                        Username = reader.GetString("username"),
                        NamaLengkap = reader.GetString("nama_lengkap"),
                        Email = reader.GetString("email"),
                        NomorHp = reader.GetString("nomor_hp"),
                        CreatedAt = reader.GetDateTime("created_at")
                    };

                    
                    string roleStr = reader.GetString("role");
                    user.Role = roleStr.ToLower() == "admin" ? UserRole.Admin : UserRole.Pengguna;

                 
                    string statusStr = reader.GetString("status");
                    user.Status = statusStr.ToLower() switch
                    {
                        "aktif" => UserStatus.Aktif,
                        "nonaktif" => UserStatus.Nonaktif,
                        "suspended" => UserStatus.Suspended,
                        _ => UserStatus.Nonaktif
                    };

                    return user;
                }, parameters);

                if (result != null)
                {
                    CurrentUser = result;
                    LogLoginAttempt(username, true);
                }
                else
                {
                    LogLoginAttempt(username, false);

                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login exception: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw new AuthenticationException($"Login failed: {ex.Message}", ex);
            }
        }

        public override bool Register(User newUser)
        {
            // Validate user data
            if (newUser == null)
                throw new ArgumentNullException(nameof(newUser));

            if (!newUser.ValidateData())
                throw new ArgumentException("Invalid User Data");

            if (!IsValidEmail(newUser.Email))
                throw new ArgumentException("Format Email Tidak Valid");

            if (!IsValidPhoneNumber(newUser.NomorHp))
                throw new ArgumentException("Nomor HP Tidak Valid");

            try
            {
                
                bool success = false;
                DatabaseHelper.ExecuteTransaction(transaction =>
                {
                    var conn = transaction.Connection;

                    //cek apakah username tersedia
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @u OR email = @e";
                    using (var checkCmd = new NpgsqlCommand(checkQuery, conn, transaction))
                    {
                        checkCmd.Parameters.AddWithValue("@u", newUser.Username);
                        checkCmd.Parameters.AddWithValue("@e", newUser.Email);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count > 0)
                            throw new InvalidOperationException("Username atau Email Sudah Digunakan!");
                    }

                    // nambah user baru
                    string insertQuery = @"
                        INSERT INTO users(username, password_hash, role, nama_lengkap, email, nomor_hp, created_at, status)
                        VALUES(@u, crypt(@p, gen_salt('bf')), @r, @n, @e, @h, NOW(), 'Aktif')";

                    using (var insertCmd = new NpgsqlCommand(insertQuery, conn, transaction))
                    {
                        insertCmd.Parameters.AddWithValue("@u", newUser.Username);
                        insertCmd.Parameters.AddWithValue("@p", newUser.Password);
                        insertCmd.Parameters.AddWithValue("@r", newUser.Role.ToString().ToLower());
                        insertCmd.Parameters.AddWithValue("@n", newUser.NamaLengkap);
                        insertCmd.Parameters.AddWithValue("@e", newUser.Email);
                        insertCmd.Parameters.AddWithValue("@h", newUser.NomorHp);

                        int rowsAffected = insertCmd.ExecuteNonQuery();
                        success = rowsAffected > 0;
                    }
                });

                return success;
            }
            catch (Exception ex)
            {
                throw new AuthenticationException($"Registrasi Gagal: {ex.Message}", ex);
            }
        }

       
        public bool HasPermission(string permission)
        {
            if (CurrentUser == null)
                return false;

            return CurrentUser.Role switch
            {
                UserRole.Admin => true, // Admin has all permissions
                UserRole.Pengguna => permission.ToLower() switch
                {
                    "make_reservation" => true,
                    "view_own_reservations" => true,
                    "cancel_own_reservation" => true,
                    _ => false
                },
                _ => false
            };
        }

       
        private void LogLoginAttempt(string username, bool success)
        {
            try
            {
                string query = @"
                    INSERT INTO login_logs(username, success, attempt_time, ip_address)
                    VALUES(@username, @success, NOW(), @ip)";

                var parameters = new NpgsqlParameter[]
                {
                    new("@username", username),
                    new("@success", success),
                    new("@ip", "127.0.0.1") // In real app, get actual IP
                };

                _database.ExecuteNonQuery(query, parameters);
            }
            catch
            {
                
            }
        }
        
        public static User LoginStatic(string username, string password)
        {
            return Instance.Login(username, password);
        }

        public static bool RegisterStatic(User newUser)
        {
            return Instance.Register(newUser);
        }

        public static User GetCurrentUser()
        {
            return Instance.CurrentUser;
        }

        
    }

   
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message) : base(message) { }
        public AuthenticationException(string message, Exception innerException) : base(message, innerException) { }
    }
}