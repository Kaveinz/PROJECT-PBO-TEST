using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace test_part_kesekian
{
    public static class AuthController
    {
        public static User Login(string username, string password)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            string query = "SELECT id, username, role FROM users WHERE username = @u AND password_hash = crypt(@p, password_hash)";
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
                    Role = reader.GetString(2)
                };
            }
            return null;
        }
    }
}
