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
    public static class AuthController
    {
        public static User Login(string username, string password)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            string query = "SELECT id, username, role, nomor_hp FROM users WHERE username = @u AND password_hash = crypt(@p, password_hash)";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("u", username);
            cmd.Parameters.AddWithValue("p", password);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Role = reader.GetString(2),
                    nomor_hp = reader.GetString(3)
                };
            }
            return null;
        }

        public static bool Register(User newUser)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();

            // Cek apakah username sudah dipakai
            string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @u";
            using (var checkCmd = new NpgsqlCommand(checkQuery, conn))
            {
                checkCmd.Parameters.AddWithValue("u", newUser.Username);
                long count = (long)checkCmd.ExecuteScalar();
                if (count > 0)
                    return false; // Username sudah dipakai
            }

            // Insert user baru
            string insertQuery = "INSERT INTO users(username, password_hash, role, nama_lengkap, email, nomor_hp, created_at, status) VALUES(@u, crypt(@p, gen_salt('bf')), @r, @n, @e, @h, NOW(), 'Aktif')";
;
            using var insertCmd = new NpgsqlCommand(insertQuery, conn);
            insertCmd.Parameters.AddWithValue("u", newUser.Username);
            insertCmd.Parameters.AddWithValue("p", newUser.Password);
            insertCmd.Parameters.AddWithValue("r", newUser.Role ?? "pengguna");
            insertCmd.Parameters.AddWithValue("n", newUser.nama_lengkap);
            insertCmd.Parameters.AddWithValue("e", newUser.email);
            insertCmd.Parameters.AddWithValue("h", newUser.nomor_hp);


            insertCmd.ExecuteNonQuery();
            return true;
        }



    }
}
