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
        bool CekMejaKosong(string tableNumber, DateTime reservationTime);
        bool CekKapasitasMeja(string tableNumber, int jumlahOrang);
        bool CekNomorHP(string phoneNumber);
        bool JamOperasional(DateTime reservationTime);
        void UpdateStatusMeja(string tableNumber, string status);


    }

    public abstract class BaseReservationController : IReservationService
    {
        protected readonly IDatabaseOperations _database;

        protected BaseReservationController(IDatabaseOperations database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public abstract bool CreateReservation(Reservation reservation);
        public abstract bool CekMejaKosong(string tableNumber, DateTime reservationTime);
        public abstract bool CekKapasitasMeja(string tableNumber, int jumlahOrang);
        public abstract bool CekNomorHP(string phoneNumber);
        public abstract bool JamOperasional(DateTime reservationTime);
        public abstract void UpdateStatusMeja(string tableNumber, string status);




        protected virtual bool ValidateReservationData(Reservation reservation)
        {
            if (reservation == null)
                return false;

            return reservation.ValidateReservasi();
        }
    }

    public class ReservationController : BaseReservationController
    {
        private static ReservationController _instance;
        private static readonly object _lock = new object();

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

        public override bool CreateReservation(Reservation reservation)
        {
            if (!ValidateReservationData(reservation))
                throw new ArgumentException("Tanggal Reservasi Tidak Validz!");

            

           

            try
            {
                bool success = false;
                DatabaseHelper.ExecuteTransaction(transaction =>
                {
                    var conn = transaction.Connection;

                    // Masukin reservasi ke database
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

                    
                    UpdateStatusMeja(reservation.TableNumber, "Reserved");
                    

                    success = true;
                });

                return success;
            }
            catch (Exception ex)
            {
                throw new ReservationException($"Failed to create reservation: {ex.Message}", ex);
            }
        }
        
        
            public override bool CekMejaKosong(string tableNumber, DateTime reservationTime)
            {
                string query = @"
                    SELECT COUNT(*) 
                    FROM tables t
                    WHERE t.table_number = @tableNumber
                    AND NOT EXISTS (
                        SELECT 1 FROM reservations r
                        WHERE r.table_number = t.table_number
                        AND r.status IN ('Menunggu', 'Dikonfirmasi')
                        AND @startTime < r.reservation_time + interval '2 hour'
                        AND @endTime > r.reservation_time
                    )";

                var parameters = new NpgsqlParameter[]
                {
                    new("@tableNumber", tableNumber),
                    new("@startTime", reservationTime.AddHours(-1)),
                    new("@endTime", reservationTime.AddHours(1)) 
                };

                try
                {
                    long count = (long)_database.ExecuteScalar(query, parameters);
                    return count > 0;
                }
                catch (Exception ex)
                {
                    throw new ReservationException($"Gagal mengecek ketersediaan meja: {ex.Message}", ex);
                }
            }

        

        public override bool CekKapasitasMeja(string tableNumber, int jumlahOrang)
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

        

        public override bool CekNomorHP(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;
                
            
            return phoneNumber.Length >= 10 && 
                   phoneNumber.Length <= 15 && 
                   phoneNumber.All(char.IsDigit);
        }


        public override bool JamOperasional(DateTime reservationTime)
        {
            TimeSpan open = new TimeSpan(9, 0, 0);  // 09:00
            TimeSpan close = new TimeSpan(21, 0, 0); // 21:00

            TimeSpan waktu = reservationTime.TimeOfDay;
            return waktu >= open && waktu <= close;
        }

        

        public override void UpdateStatusMeja(string tableNumber, string status)
        {
            string query = "UPDATE tables SET status = @status WHERE table_number = @tableNumber";
            var parameters = new NpgsqlParameter[]
            {
                new("@status", status),
                new("@tableNumber", tableNumber)
            };
            DatabaseHelper.ExecuteNonQuery(query, parameters);
        }



        public static bool CreateReservationStatic(Reservation reservation)
        {
            return Instance.CreateReservation(reservation);
        }
        public static bool CekMejaKosongStatic(string tableNumber, DateTime reservationTime)
        {
            return Instance.CekMejaKosong(tableNumber, reservationTime);
        }

        public static bool CekKapasitasMejaStatic(string tableNumber, int jumlahOrang)
        {
            return Instance.CekKapasitasMeja(tableNumber, jumlahOrang);
        }

        public static bool CekNomorHPStatic(string phoneNumber)
        {
            return Instance.CekNomorHP(phoneNumber);
        }
        public static bool JamOperasionalStatic(DateTime reservationTime)
        {
            return Instance.JamOperasional(reservationTime);
        }
        public static void UpdateStatusMejaStatic(string tableNumber, string status)
        {
            Instance.UpdateStatusMeja(tableNumber, status);
        }
    }

    public class ReservationException : Exception
    {
        public ReservationException(string message) : base(message) { }
        public ReservationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
