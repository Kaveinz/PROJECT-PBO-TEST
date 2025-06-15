
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using test_part_kesekian.models;

namespace test_part_kesekian.controller
{
    public interface IReservationService
    {
        bool CreateReservation(Reservation reservation);
        List<Reservation> GetUserReservations(int userId);
        List<Reservation> GetAllReservations();
        Reservation GetReservationById(int id);
        bool IsTableAvailable(string tableNumber, DateTime reservationTime);
    }

    public abstract class BaseReservationController : IReservationService
    {
        protected readonly IDatabaseOperations _database;

        protected BaseReservationController(IDatabaseOperations database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public abstract bool CreateReservation(Reservation reservation);
        public abstract bool IsTableAvailable(string tableNumber, DateTime reservationTime);

        
        public virtual List<Reservation> GetUserReservations(int userId)
        {
            string query = @"
                SELECT id, user_id, nomor_hp, reservation_time, jumlah_orang, table_number, status
                FROM reservations 
                WHERE user_id = @userId 
                ORDER BY reservation_time DESC";

            var parameters = new NpgsqlParameter[] { new("@userId", userId) };

            return DatabaseHelper.Instance.GetList(query, MapReservationFromReader, parameters);
        }

        public virtual List<Reservation> GetAllReservations()
        {
            string query = @"
                SELECT id, user_id, nomor_hp, reservation_time, jumlah_orang, table_number, status
                FROM reservations 
                ORDER BY reservation_time DESC";

            return DatabaseHelper.Instance.GetList(query, MapReservationFromReader);
        }

        public virtual Reservation GetReservationById(int id)
        {
            string query = @"
                SELECT id, user_id, nomor_hp, reservation_time, jumlah_orang, table_number, status
                FROM reservations 
                WHERE id = @id";

            var parameters = new NpgsqlParameter[] { new("@id", id) };

            return DatabaseHelper.Instance.GetSingle(query, MapReservationFromReader, parameters);
        }

        protected virtual Reservation MapReservationFromReader(NpgsqlDataReader reader)
        {
            var reservation = new Reservation
            {
                Id = reader.GetInt32("id"),
                UserId = reader.GetInt32("user_id"),
                NomorHP = reader.GetString("nomor_hp"),
                ReservationTime = reader.GetDateTime("reservation_time"),
                JumlahOrang = reader.GetInt32("jumlah_orang"),
                TableNumber = reader.GetString("table_number")
            };

            string statusStr = reader.GetString("status");
            reservation.Status = statusStr.ToLower() switch
            {
                "menunggu" => ReservationStatus.Menunggu,
                "dikonfirmasi" => ReservationStatus.Dikonfirmasi,
                "selesai" => ReservationStatus.Selesai,
                "dibatalkan" => ReservationStatus.Dibatalkan,
                _ => ReservationStatus.Menunggu
            };

            return reservation;
        }

       
        protected virtual bool ValidateReservationData(Reservation reservation)
        {
            if (reservation == null)
                return false;

            return reservation.ValidateReservation();
        }
    }

    // Concrete implementation of ReservationController
    public class ReservationController : BaseReservationController
    {
        private static ReservationController _instance;
        private static readonly object _lock = new object();

        // Singleton pattern
        public static ReservationController Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new ReservationController(DatabaseHelper.Instance);
                    }
                }
                return _instance;
            }
        }

        private ReservationController(IDatabaseOperations database) : base(database) { }

        // Implementation of abstract CreateReservation method
        public override bool CreateReservation(Reservation reservation)
        {
            if (!ValidateReservationData(reservation))
                throw new ArgumentException("Invalid reservation data");

            if (!IsTableAvailable(reservation.TableNumber, reservation.ReservationTime))
                throw new InvalidOperationException("Table is not available at the selected time");

            if (!IsTableCapacitySufficient(reservation.TableNumber, reservation.JumlahOrang))
                throw new InvalidOperationException("Table capacity is insufficient for the number of people");

            try
            {
                bool success = false;
                DatabaseHelper.ExecuteTransaction(transaction =>
                {
                    var conn = transaction.Connection;

                    // Insert reservation
                    string insertQuery = @"
                        INSERT INTO reservations (user_id, nomor_hp, reservation_time, jumlah_orang, table_number, status)
                        VALUES (@user_id, @nomor_hp, @reservation_time, @jumlah_orang, @table_number, @status)";

                    using (var cmd = new NpgsqlCommand(insertQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@user_id", reservation.UserId);
                        cmd.Parameters.AddWithValue("@nomor_hp", reservation.NomorHP);
                        cmd.Parameters.AddWithValue("@reservation_time", reservation.ReservationTime);
                        cmd.Parameters.AddWithValue("@jumlah_orang", reservation.JumlahOrang);
                        cmd.Parameters.AddWithValue("@table_number", reservation.TableNumber);
                        cmd.Parameters.AddWithValue("@status", reservation.Status.ToString());

                        cmd.ExecuteNonQuery();
                    }

                    // Update table status
                    string updateTableQuery = "UPDATE tables SET status = @status WHERE table_number = @tableNumber";
                    using (var cmd = new NpgsqlCommand(updateTableQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@status", "Reserved");
                        cmd.Parameters.AddWithValue("@tableNumber", reservation.TableNumber);
                        cmd.ExecuteNonQuery();
                    }

                    success = true;
                });

                return success;
            }
            catch (Exception ex)
            {
                throw new ReservationException($"Failed to create reservation: {ex.Message}", ex);
            }
        }

        // Implementation of abstract UpdateReservation method
        
        // Implementation of abstract IsTableAvailable method
        public override bool IsTableAvailable(string tableNumber, DateTime reservationTime)
        {
            string query = @"
                SELECT COUNT(*) 
                FROM tables t
                WHERE t.table_number = @tableNumber
                AND t.status = 'Available'
                AND NOT EXISTS (
                    SELECT 1 FROM reservations r
                    WHERE r.table_number = t.table_number
                    AND r.status IN ('Menunggu', 'Dikonfirmasi')
                    AND r.reservation_time BETWEEN @startTime AND @endTime
                )";

            var parameters = new NpgsqlParameter[]
            {
                new("@tableNumber", tableNumber),
                new("@startTime", reservationTime.AddHours(-2)),
                new("@endTime", reservationTime.AddHours(2))
            };

            try
            {
                long count = (long)_database.ExecuteScalar(query, parameters);
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new ReservationException($"Failed to check table availability: {ex.Message}", ex);
            }
        }

        public bool IsTableCapacitySufficient(string tableNumber, int jumlahOrang)
        {
            string query = "SELECT capacity FROM tables WHERE table_number = @tableNumber";
            var parameters = new NpgsqlParameter[] { new("@tableNumber", tableNumber) };

            try
            {
                object result = _database.ExecuteScalar(query, parameters);
                if (result == null || result == DBNull.Value)
                    return false;

                int capacity = Convert.ToInt32(result);
                return jumlahOrang <= capacity;
            }
            catch (Exception ex)
            {
                throw new ReservationException($"Failed to check table capacity: {ex.Message}", ex);
            }
        }

        

        // Method to get upcoming reservations
        

        // Method to confirm reservation (admin only)
        public bool ConfirmReservation(int reservationId)
        {
            string query = "UPDATE reservations SET status = 'Dikonfirmasi' WHERE id = @id AND status = 'Menunggu'";
            var parameters = new NpgsqlParameter[] { new("@id", reservationId) };

            try
            {
                int rowsAffected = _database.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new ReservationException($"Failed to confirm reservation: {ex.Message}", ex);
            }
        }

        public static bool IsTableAvailableStatic(string tableNumber, DateTime reservationTime)
        {
            return Instance.IsTableAvailable(tableNumber, reservationTime);
        }

        public static bool IsTableCapacitySufficientStatic(string tableNumber, int jumlahOrang)
        {
            return Instance.IsTableCapacitySufficient(tableNumber, jumlahOrang);
        }

        public static void UpdateTableStatus(string tableNumber, string status)
        {
            string query = "UPDATE tables SET status = @status WHERE table_number = @tableNumber";
            var parameters = new NpgsqlParameter[]
            {
                new("@status", status),
                new("@tableNumber", tableNumber)
            };
            DatabaseHelper.ExecuteNonQuery(query, parameters);
        }
    }

   
    public class ReservationException : Exception
    {
        public ReservationException(string message) : base(message) { }
        public ReservationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
