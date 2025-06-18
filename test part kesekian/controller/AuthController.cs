using Npgsql;
using System.Data;
using System.Text;
using test_part_kesekian.models;

namespace test_part_kesekian.controller
{
    // Interface buat login sama register
    public interface IAuthService
    {
        User Login(string username, string password);
        bool Register(User penggunaBaru);

    }

    public abstract class BaseAuthController : IAuthService
    {
        protected readonly IDatabaseOperations _database;
      

        protected BaseAuthController(IDatabaseOperations database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public User penggunaTerbaru
        {
            get => penggunaTerbaru;
            protected set => penggunaTerbaru = value;
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
        private static AuthController instance;
        private static readonly object lockInstance= new object();

   
        public static AuthController Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockInstance)
                    {
                        if (instance == null)
                            instance = new AuthController(DatabaseHelper.Instance);
                    }
                }
                return instance;
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
                  

                    var pengguna = new User
                    {
                        Id = reader.GetInt32("id"),
                        Username = reader.GetString("username"),
                        NamaLengkap = reader.GetString("nama_lengkap"),
                        Email = reader.GetString("email"),
                        NomorHp = reader.GetString("nomor_hp"),
                        CreatedAt = reader.GetDateTime("created_at")
                    };

                    
                    string roleStr = reader.GetString("role");
                    pengguna.Role = roleStr.ToLower() == "admin" ? UserRole.Admin : UserRole.Pengguna;

                 
                    string statusStr = reader.GetString("status");
                    pengguna.Status = statusStr.ToLower() switch
                    {
                        "aktif" => UserStatus.Aktif,
                        "nonaktif" => UserStatus.Nonaktif,
                        "suspended" => UserStatus.Suspended,
                        _ => UserStatus.Nonaktif
                    };

                    return pengguna;
                }, parameters);

              

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login exception: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw new AuthenticationException($"Login failed: {ex.Message}", ex);
            }
        }

        public override bool Register(User penggunaBaru)
        {
            if (penggunaBaru == null)
                throw new ArgumentNullException(nameof(penggunaBaru));

            if (!penggunaBaru.ValidateData())
                throw new ArgumentException("Invalid User Data");

            if (!IsValidEmail(penggunaBaru.Email))
                throw new ArgumentException("Format Email Tidak Valid");

            if (!IsValidPhoneNumber(penggunaBaru.NomorHp))
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
                        checkCmd.Parameters.AddWithValue("@u", penggunaBaru.Username);
                        checkCmd.Parameters.AddWithValue("@e", penggunaBaru.Email);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count > 0)
                            throw new InvalidOperationException("Username atau Email Sudah Digunakan!");
                    }

                    // Nambah user baru
                    string insertQuery = @"
                        INSERT INTO users(username, password_hash, role, nama_lengkap, email, nomor_hp, created_at, status)
                        VALUES(@u, crypt(@p, gen_salt('bf')), @r, @n, @e, @h, NOW(), 'Aktif')";

                    using (var insertCmd = new NpgsqlCommand(insertQuery, conn, transaction))
                    {
                        insertCmd.Parameters.AddWithValue("@u", penggunaBaru.Username);
                        insertCmd.Parameters.AddWithValue("@p", penggunaBaru.Password);
                        insertCmd.Parameters.AddWithValue("@r", penggunaBaru.Role.ToString().ToLower());
                        insertCmd.Parameters.AddWithValue("@n", penggunaBaru.NamaLengkap);
                        insertCmd.Parameters.AddWithValue("@e", penggunaBaru.Email);
                        insertCmd.Parameters.AddWithValue("@h", penggunaBaru.NomorHp);

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
    }

   
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message) : base(message) { }
        public AuthenticationException(string message, Exception innerException) : base(message, innerException) { }
    }
}