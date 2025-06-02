using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace test_part_kesekian
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString = "Host=localhost;Username=postgres;Password=Renpersona5;Database=restaurant_db";

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

        public static bool IsTableAvailable(string tableNumber, DateTime reservationTime)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT COUNT(*) 
                    FROM tables t
                    WHERE t.table_number = @tableNumber
                    AND NOT EXISTS (
                        SELECT 1 FROM reservations r
                        WHERE r.table_number = t.table_number
                        AND r.status IN ('Menunggu', 'Dikonfirmasi')
                        AND r.reservation_time BETWEEN @startTime AND @endTime
                    )";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("tableNumber", tableNumber);
                    cmd.Parameters.AddWithValue("startTime", reservationTime.AddHours(-2));
                    cmd.Parameters.AddWithValue("endTime", reservationTime.AddHours(2));
                    long count = (long)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public static bool IsTableCapacitySufficient(string tableNumber, int jumlahOrang)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT capacity FROM tables WHERE table_number = @tableNumber";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("tableNumber", tableNumber);
                    object result = cmd.ExecuteScalar();
                    if (result == null) return false;
                    int capacity = (int)result;
                    return jumlahOrang <= capacity;
                }
            }
        }

        public static void UpdateTableStatus(string tableNumber, string status)
        {
            string query = "UPDATE tables SET status = @status WHERE table_number = @tableNumber";
            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@status", status),
                new NpgsqlParameter("@tableNumber", tableNumber)
            };
            ExecuteNonQuery(query, parameters);
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
