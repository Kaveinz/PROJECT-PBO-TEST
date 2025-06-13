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
        bool ChangePassword(int userId, string oldPassword, string newPassword);
        bool ResetPassword(string username, string email);
        void Logout();
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

        // Property for current user (Encapsulation)
        public User CurrentUser 
        { 
            get => _currentUser; 
            protected set => _currentUser = value; 
        }

        // Abstract methods for derived classes to implement
        public abstract User Login(string username, string password);
        public abstract bool Register(User newUser);

        // Virtual methods that can be overridden (Polymorphism)
        public virtual bool ChangePassword(int userId, string oldPassword, string newPassword)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
                return false;

            if (newPassword.Length < 6)
                throw new ArgumentException("New password must be at least 6 characters long");

            // Verify old password first
            string verifyQuery = "SELECT COUNT(*) FROM users WHERE id = @id AND password_hash = crypt(@oldPassword, password_hash)";
            var verifyParams = new NpgsqlParameter[]
            {
                new("@id", userId),
                new("@oldPassword", oldPassword)
            };

            long count = (long)_database.ExecuteScalar(verifyQuery, verifyParams);
            if (count == 0)
                return false;

            // Update password
            string updateQuery = "UPDATE users SET password_hash = crypt(@newPassword, gen_salt('bf')) WHERE id = @id";
            var updateParams = new NpgsqlParameter[]
            {
                new("@newPassword", newPassword),
                new("@id", userId)
            };

            return _database.ExecuteNonQuery(updateQuery, updateParams) > 0;
        }

        public virtual bool ResetPassword(string username, string email)
        {
            // Generate temporary password
            string tempPassword = GenerateTemporaryPassword();
            
            string query = "UPDATE users SET password_hash = crypt(@tempPassword, gen_salt('bf')) WHERE username = @username AND email = @email";
            var parameters = new NpgsqlParameter[]
            {
                new("@tempPassword", tempPassword),
                new("@username", username),
                new("@email", email)
            };

            int rowsAffected = _database.ExecuteNonQuery(query, parameters);
            
            if (rowsAffected > 0)
            {
                // In a real application, you would send this password via email
                System.Windows.Forms.MessageBox.Show($"Temporary password: {tempPassword}", "Password Reset", 
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return true;
            }
            
            return false;
        }

        public virtual void Logout()
        {
            CurrentUser = null;
        }

        // Helper method to generate temporary password
        protected virtual string GenerateTemporaryPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Helper method to validate email format
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

        // Helper method to validate phone number
        protected virtual bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrWhiteSpace(phoneNumber) && 
                   phoneNumber.Length >= 10 && 
                   phoneNumber.All(c => char.IsDigit(c) || c == '+' || c == '-' || c == ' ');
        }
    }

    // Concrete implementation of AuthController (Inheritance)
    public class AuthController : BaseAuthController
    {
        private static AuthController _instance;
        private static readonly object _lock = new object();

        // Singleton pattern implementation
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

        // Private constructor for singleton
        private AuthController(IDatabaseOperations database) : base(database) { }        // Implementation of abstract Login method (Polymorphism)
        public override User Login(string username, string password)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username and password cannot be empty");

            try
            {
                Console.WriteLine($"🔐 Login attempt for username: '{username}'");
                
                string query = @"
                    SELECT id, username, role, nama_lengkap, email, nomor_hp, created_at, status 
                    FROM users 
                    WHERE username = @u AND password_hash = crypt(@p, password_hash) AND status = 'Aktif'";

                var parameters = new NpgsqlParameter[]
                {
                    new("@u", username.Trim().ToLower()),
                    new("@p", password)
                };

                Console.WriteLine($"🔍 Executing query with username: '{username.Trim().ToLower()}'");

                var result = DatabaseHelper.Instance.GetSingle(query, reader =>
                {
                    Console.WriteLine("✅ User found in database, creating User object...");
                    
                    var user = new User
                    {
                        Id = reader.GetInt32("id"),
                        Username = reader.GetString("username"),
                        NamaLengkap = reader.GetString("nama_lengkap"),
                        Email = reader.GetString("email"),
                        NomorHp = reader.GetString("nomor_hp"),
                        CreatedAt = reader.GetDateTime("created_at")
                    };

                    // Set role using enum
                    string roleStr = reader.GetString("role");
                    Console.WriteLine($"📋 User role from database: '{roleStr}'");
                    user.Role = roleStr.ToLower() == "admin" ? UserRole.Admin : UserRole.Pengguna;

                    // Set status using enum
                    string statusStr = reader.GetString("status");
                    Console.WriteLine($"📊 User status from database: '{statusStr}'");
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
                    Console.WriteLine($"✅ Login successful for user: {result.Username} (Role: {result.Role})");
                }
                else
                {
                    LogLoginAttempt(username, false);
                    Console.WriteLine($"❌ Login failed - no matching user found for username: '{username}'");
                    
                    // Additional debug: check if user exists at all
                    string checkUserQuery = "SELECT username, status FROM users WHERE username = @u";
                    var checkParams = new NpgsqlParameter[] { new("@u", username.Trim().ToLower()) };
                    
                    try
                    {
                        var existingUser = DatabaseHelper.Instance.GetSingle(checkUserQuery, reader =>
                        {
                            return new { 
                                Username = reader.GetString("username"),
                                Status = reader.GetString("status")
                            };
                        }, checkParams);
                        
                        if (existingUser != null)
                        {
                            Console.WriteLine($"🔍 User '{existingUser.Username}' exists with status: '{existingUser.Status}' - password might be incorrect");
                        }
                        else
                        {
                            Console.WriteLine($"🔍 No user found with username: '{username.Trim().ToLower()}'");
                        }
                    }
                    catch (Exception debugEx)
                    {
                        Console.WriteLine($"❌ Debug query failed: {debugEx.Message}");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Login exception: {ex.Message}");
                Console.WriteLine($"🔍 Stack trace: {ex.StackTrace}");
                throw new AuthenticationException($"Login failed: {ex.Message}", ex);
            }
        }

        // Implementation of abstract Register method (Polymorphism)
        public override bool Register(User newUser)
        {
            // Validate user data
            if (newUser == null)
                throw new ArgumentNullException(nameof(newUser));

            if (!newUser.ValidateData())
                throw new ArgumentException("Invalid user data provided");

            if (!IsValidEmail(newUser.Email))
                throw new ArgumentException("Invalid email format");

            if (!IsValidPhoneNumber(newUser.NomorHp))
                throw new ArgumentException("Invalid phone number format");

            try
            {
                // Use transaction for data consistency
                bool success = false;
                DatabaseHelper.ExecuteTransaction(transaction =>
                {
                    var conn = transaction.Connection;

                    // Check if username already exists
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @u OR email = @e";
                    using (var checkCmd = new NpgsqlCommand(checkQuery, conn, transaction))
                    {
                        checkCmd.Parameters.AddWithValue("@u", newUser.Username);
                        checkCmd.Parameters.AddWithValue("@e", newUser.Email);
                        long count = (long)checkCmd.ExecuteScalar();
                        
                        if (count > 0)
                            throw new InvalidOperationException("Username or email already exists");
                    }

                    // Insert new user
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
                throw new AuthenticationException($"Registration failed: {ex.Message}", ex);
            }
        }

        // Method to check user permissions
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

        // Method to log login attempts (for security)
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
                // Log login attempt failed, but don't throw exception
                // In production, you might want to log this to a file
            }
        }        // Static methods for backward compatibility
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

        public static void LogoutCurrentUser()
        {
            Instance.Logout();
        }
    }

    // Custom exception for authentication errors
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message) : base(message) { }
        public AuthenticationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
