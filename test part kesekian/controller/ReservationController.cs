

using Npgsql;
using test_part_kesekian.models;

namespace test_part_kesekian.controller
{


    public static class reservationController
    {
        public static bool IsTableAvailable(string tableNumber, DateTime reservationTime)
        {
            using (var conn = DatabaseHelper.GetConnection())
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
            using (var conn = DatabaseHelper.GetConnection())
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
            DatabaseHelper.ExecuteNonQuery(query, parameters);
        }
    }

}