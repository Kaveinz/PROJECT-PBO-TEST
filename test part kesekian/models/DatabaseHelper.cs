using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace test_part_kesekian.models
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString = "Host=localhost;Username=farhan;Password=6yLwsk85;Database=MBOK_WO_RESERVE";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        public static DataTable GetData(string query)
        {
            DataTable dt = new DataTable();
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var adapter = new NpgsqlDataAdapter(query, conn))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public static void ExecuteNonQuery(string query, NpgsqlParameter[] parameters = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }

        

        public static void ExportToTextFile(DataTable dt, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine("Laporan Reservasi");
                sw.WriteLine("=================");
                foreach (DataRow row in dt.Rows)
                {
                    sw.WriteLine($"ID: {row["id"]}");
                    sw.WriteLine($"Username: {row["username"]}");
                    sw.WriteLine($"Nomor HP: {row["nomor_hp"]}");
                    sw.WriteLine($"Waktu: {Convert.ToDateTime(row["reservation_time"]).ToString("yyyy-MM-dd HH:mm")}");
                    sw.WriteLine($"Jumlah Orang: {row["jumlah_orang"]}");
                    sw.WriteLine($"Meja: {row["table_number"]}");
                    sw.WriteLine($"Status: {row["status"]}");
                    sw.WriteLine("-----------------");
                }
                sw.WriteLine($"Total Reservasi: {dt.Rows.Count}");
            }
        }
    }
}
