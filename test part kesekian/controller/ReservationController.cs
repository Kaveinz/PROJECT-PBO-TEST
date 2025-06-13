
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using test_part_kesekian.models;

namespace test_part_kesekian.controller
{
    // Interface for reservation operations (Abstraction)
    public interface IReservationService
    {
        bool CreateReservation(Reservation reservation);
        bool UpdateReservation(Reservation reservation);
        bool CancelReservation(int reservationId, int userId);
        List<Reservation> GetUserReservations(int userId);
        List<Reservation> GetAllReservations();
        Reservation GetReservationById(int id);
        bool IsTableAvailable(string tableNumber, DateTime reservationTime);
    }

    // Abstract base class for reservation management
    public abstract class BaseReservationController : IReservationService
    {
        protected readonly IDatabaseOperations _database;

        protected BaseReservationController(IDatabaseOperations database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        // Abstract methods for derived classes
        public abstract bool CreateReservation(Reservation reservation);
        public abstract bool UpdateReservation(Reservation reservation);
        public abstract bool IsTableAvailable(string tableNumber, DateTime reservationTime);

        // Virtual methods that can be overridden (Polymorphism)
        public virtual bool CancelReservation(int reservationId, int userId)
        {
            try
            {
                // First, get the reservation to check if it can be cancelled
                var reservation = GetReservationById(reservationId);
                if (reservation == null)
                    return false;

                // Check if user owns this reservation (security check)
                if (reservation.UserId != userId)
                    throw new UnauthorizedAccessException("User can only cancel their own reservations");

                // Check if reservation can be cancelled
                if (!reservation.CanBeCancelled())
                    throw new InvalidOperationException("This reservation cannot be cancelled");

                // Update reservation status and table status
                bool success = false;
                DatabaseHelper.ExecuteTransaction(transaction =>
                {
                    var conn = transaction.Connection;

                    // Update reservation status
                    string updateReservationQuery = "UPDATE reservations SET status = 'Dibatalkan' WHERE id = @id";
                    using (var cmd = new NpgsqlCommand(updateReservationQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id", reservationId);
                        cmd.ExecuteNonQuery();
                    }

                    // Update table status back to available
                    string updateTableQuery = "UPDATE tables SET status = 'Available' WHERE table_number = @tableNumber";
                    using (var cmd = new NpgsqlCommand(updateTableQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@tableNumber", reservation.TableNumber);
                        cmd.ExecuteNonQuery();
                    }

                    success = true;
                });

                return success;
            }
            catch (Exception ex)
            {
                throw new ReservationException($"Failed to cancel reservation: {ex.Message}", ex);
            }
        }

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

        // Helper method to map database reader to Reservation object
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

            // Map status string to enum
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

        // Helper method to validate reservation data
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
        public override bool UpdateReservation(Reservation reservation)
        {
            if (!ValidateReservationData(reservation))
                throw new ArgumentException("Invalid reservation data");

            // Get current reservation to check if it can be modified
            var currentReservation = GetReservationById(reservation.Id);
            if (currentReservation == null)
                return false;

            if (!currentReservation.CanBeModified())
                throw new InvalidOperationException("This reservation cannot be modified");

            try
            {
                bool success = false;
                DatabaseHelper.ExecuteTransaction(transaction =>
                {
                    var conn = transaction.Connection;

                    // If table number changed, check availability and update both tables
                    if (currentReservation.TableNumber != reservation.TableNumber)
                    {
                        if (!IsTableAvailable(reservation.TableNumber, reservation.ReservationTime))
                            throw new InvalidOperationException("New table is not available at the selected time");

                        // Release old table
                        string releaseOldTableQuery = "UPDATE tables SET status = 'Available' WHERE table_number = @oldTableNumber";
                        using (var cmd = new NpgsqlCommand(releaseOldTableQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@oldTableNumber", currentReservation.TableNumber);
                            cmd.ExecuteNonQuery();
                        }

                        // Reserve new table
                        string reserveNewTableQuery = "UPDATE tables SET status = 'Reserved' WHERE table_number = @newTableNumber";
                        using (var cmd = new NpgsqlCommand(reserveNewTableQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@newTableNumber", reservation.TableNumber);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Update reservation
                    string updateQuery = @"
                        UPDATE reservations 
                        SET nomor_hp = @nomor_hp, reservation_time = @reservation_time, 
                            jumlah_orang = @jumlah_orang, table_number = @table_number, status = @status
                        WHERE id = @id";

                    using (var cmd = new NpgsqlCommand(updateQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id", reservation.Id);
                        cmd.Parameters.AddWithValue("@nomor_hp", reservation.NomorHP);
                        cmd.Parameters.AddWithValue("@reservation_time", reservation.ReservationTime);
                        cmd.Parameters.AddWithValue("@jumlah_orang", reservation.JumlahOrang);
                        cmd.Parameters.AddWithValue("@table_number", reservation.TableNumber);
                        cmd.Parameters.AddWithValue("@status", reservation.Status.ToString());

                        cmd.ExecuteNonQuery();
                    }

                    success = true;
                });

                return success;
            }
            catch (Exception ex)
            {
                throw new ReservationException($"Failed to update reservation: {ex.Message}", ex);
            }
        }

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

        // Method to check if table capacity is sufficient
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

        // Method to get reservations by status
        public List<Reservation> GetReservationsByStatus(ReservationStatus status)
        {
            string query = @"
                SELECT id, user_id, nomor_hp, reservation_time, jumlah_orang, table_number, status
                FROM reservations 
                WHERE status = @status 
                ORDER BY reservation_time DESC";

            var parameters = new NpgsqlParameter[] { new("@status", status.ToString()) };

            return DatabaseHelper.Instance.GetList(query, MapReservationFromReader, parameters);
        }

        // Method to get upcoming reservations
        public List<Reservation> GetUpcomingReservations()
        {
            string query = @"
                SELECT id, user_id, nomor_hp, reservation_time, jumlah_orang, table_number, status
                FROM reservations 
                WHERE reservation_time >= NOW() AND status IN ('Menunggu', 'Dikonfirmasi')
                ORDER BY reservation_time ASC";

            return DatabaseHelper.Instance.GetList(query, MapReservationFromReader);
        }

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

        // Method to complete reservation
        public bool CompleteReservation(int reservationId)
        {
            try
            {
                bool success = false;
                DatabaseHelper.ExecuteTransaction(transaction =>
                {
                    var conn = transaction.Connection;

                    // Get reservation details
                    string getReservationQuery = "SELECT table_number FROM reservations WHERE id = @id";
                    string tableNumber;
                    using (var cmd = new NpgsqlCommand(getReservationQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id", reservationId);
                        tableNumber = cmd.ExecuteScalar()?.ToString();
                    }

                    if (string.IsNullOrEmpty(tableNumber))
                        throw new InvalidOperationException("Reservation not found");

                    // Update reservation status
                    string updateReservationQuery = "UPDATE reservations SET status = 'Selesai' WHERE id = @id";
                    using (var cmd = new NpgsqlCommand(updateReservationQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id", reservationId);
                        cmd.ExecuteNonQuery();
                    }

                    // Release table
                    string releaseTableQuery = "UPDATE tables SET status = 'Available' WHERE table_number = @tableNumber";
                    using (var cmd = new NpgsqlCommand(releaseTableQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@tableNumber", tableNumber);
                        cmd.ExecuteNonQuery();
                    }

                    success = true;
                });

                return success;
            }
            catch (Exception ex)
            {
                throw new ReservationException($"Failed to complete reservation: {ex.Message}", ex);
            }
        }        // Static methods for backward compatibility
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

    // Custom exception for reservation operations
    public class ReservationException : Exception
    {
        public ReservationException(string message) : base(message) { }
        public ReservationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
