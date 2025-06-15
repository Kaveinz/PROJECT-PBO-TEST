using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using Npgsql;
using test_part_kesekian.models;

namespace test_part_kesekian.controller
{
    // Interface for report operations (Abstraction)
    public interface IReportService
    {
        string JudulLaporan { get; }
        DataTable AmbilData();
        void CetakLaporan(PrintPageEventArgs e);
        void ExportToFile(string filePath);
    }

    // Data class for report summary (Encapsulation)
    public class ReportSummary
    {
        public int TotalReservations { get; set; }
        public int PendingReservations { get; set; }
        public int ConfirmedReservations { get; set; }
        public int CompletedReservations { get; set; }
        public int CancelledReservations { get; set; }
        public decimal TotalRevenue { get; set; }
        public DateTime ReportDate { get; set; }

        public ReportSummary()
        {
            ReportDate = DateTime.Now;
        }

        public double CancellationRate => TotalReservations > 0 ? (double)CancelledReservations / TotalReservations * 100 : 0;
        public double CompletionRate => TotalReservations > 0 ? (double)CompletedReservations / TotalReservations * 100 : 0;
    }

    // Abstract base class for report management (Abstraction & Inheritance)
    public abstract class LaporanReservasiController : IReportService
    {
        // Encapsulation: Protected fields
        protected readonly IDatabaseOperations _database;
        protected DateTime _startDate;
        protected DateTime _endDate;

        // Abstract property for report title
        public abstract string JudulLaporan { get; }

        // Constructor
        protected LaporanReservasiController()
        {
            _database = DatabaseHelper.Instance;
        }

        protected LaporanReservasiController(DateTime startDate, DateTime endDate) : this()
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        // Abstract method for derived classes to implement
        public abstract DataTable AmbilData();

        // Virtual method for printing (Polymorphism)
        public virtual void CetakLaporan(PrintPageEventArgs e)
        {
            DataTable dt = AmbilData();
            float yPos = 100;
            string header = JudulLaporan;
            string footer = $"Total Reservasi: {dt.Rows.Count}";

            // Print header
            e.Graphics.DrawString(header, new Font("Arial", 16, FontStyle.Bold), Brushes.Black, new PointF(100, 50));
            e.Graphics.DrawString("".PadRight(50, '-'), new Font("Arial", 12), Brushes.Black, new PointF(100, 80));

            // Print data rows
            foreach (DataRow row in dt.Rows)
            {
                string line = FormatRowForPrint(row);
                e.Graphics.DrawString(line, new Font("Arial", 10), Brushes.Black, new PointF(100, yPos));
                yPos += 20;
                
                if (yPos > e.MarginBounds.Height - 100)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            // Print footer
            e.Graphics.DrawString("".PadRight(50, '-'), new Font("Arial", 12), Brushes.Black, new PointF(100, yPos));
            e.Graphics.DrawString(footer, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new PointF(100, yPos + 20));
        }

        // Virtual method for formatting row data (Polymorphism)
        protected virtual string FormatRowForPrint(DataRow row)
        {
            return $"ID: {row["id"]}, User: {row["username"]}, HP: {row["nomor_hp"]}, " +
                   $"Waktu: {Convert.ToDateTime(row["reservation_time"]):yyyy-MM-dd HH:mm}, " +
                   $"Orang: {row["jumlah_orang"]}, Status: {row["status"]}";
        }

        // Method to export data to text format only (no CSV/HTML)
        public virtual void ExportToFile(string filePath)
        {
            DataTable dt = AmbilData();
            ExportToTextFile(dt, filePath);
        }

        // Protected method for text export only, tanpa info meja
        protected virtual void ExportToTextFile(DataTable dt, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine(JudulLaporan);
                sw.WriteLine("".PadRight(JudulLaporan.Length, '='));
                sw.WriteLine($"Tanggal Laporan: {DateTime.Now:yyyy-MM-dd HH:mm}");
                sw.WriteLine();

                foreach (DataRow row in dt.Rows)
                {
                    sw.WriteLine($"ID: {row["id"]}");
                    sw.WriteLine($"Username: {row["username"]}");
                    sw.WriteLine($"Nomor HP: {row["nomor_hp"]}");
                    sw.WriteLine($"Waktu: {Convert.ToDateTime(row["reservation_time"]):yyyy-MM-dd HH:mm}");
                    sw.WriteLine($"Jumlah Orang: {row["jumlah_orang"]}");
                    sw.WriteLine($"Status: {row["status"]}");
                    sw.WriteLine("".PadRight(30, '-'));
                }
                sw.WriteLine($"Total Reservasi: {dt.Rows.Count}");
            }
        }

        // Method to get report summary
        public virtual ReportSummary GetReportSummary()
        {
            DataTable dt = AmbilData();
            
            var summary = new ReportSummary
            {
                TotalReservations = dt.Rows.Count
            };

            foreach (DataRow row in dt.Rows)
            {
                string status = row["status"].ToString().ToLower();
                switch (status)
                {
                    case "menunggu":
                        summary.PendingReservations++;
                        break;
                    case "dikonfirmasi":
                        summary.ConfirmedReservations++;
                        break;
                    case "selesai":
                        summary.CompletedReservations++;
                        summary.TotalRevenue += Convert.ToInt32(row["jumlah_orang"]) * 50000;
                        break;
                    case "dibatalkan":
                        summary.CancelledReservations++;
                        break;
                }
            }

            return summary;
        }        // Static method for backward compatibility
        public static void ExportToTextFileStatic(DataTable dt, string filePath)
        {
            var tempController = new LaporanUmumController();
            tempController.ExportToTextFile(dt, filePath);
        }        public static DataTable GetLaporanReservasi()
        {
            const string query = @"
                SELECT r.id AS ""ID Reservasi"", 
                       u.username AS ""Nama Pengguna"", 
                       r.nomor_hp AS ""Nomor HP"", 
                       r.reservation_time AS ""Waktu Reservasi"", 
                       r.jumlah_orang AS ""Jumlah Orang"", 
                       r.table_number AS ""Nomor Meja"", 
                       r.status AS ""Status""
                FROM reservations r
                JOIN users u ON r.user_id = u.id
                WHERE r.status IN ('Selesai', 'Dibatalkan', 'Tidak Datang')";
            
            return DatabaseHelper.GetData(query);
        }
    }

    // Concrete implementation for general reports
    public class LaporanUmumController : LaporanReservasiController
    {
        public override string JudulLaporan => "Laporan Reservasi Umum";

        public override DataTable AmbilData()
        {
            const string query = @"
                SELECT r.id, u.username, r.nomor_hp, r.reservation_time, r.jumlah_orang, r.table_number, r.status
                FROM reservations r
                JOIN users u ON r.user_id = u.id
                WHERE r.status IN ('Selesai', 'Dibatalkan', 'Tidak Datang')
                ORDER BY r.reservation_time DESC";

            return _database.GetData(query);
        }
    }

    // Concrete implementation for weekly reports (Inheritance)
    public class LaporanMingguanController : LaporanReservasiController
    {
        private readonly int _mingguKe;

        public override string JudulLaporan => $"Laporan Reservasi Mingguan - Minggu ke-{_mingguKe}";

        public LaporanMingguanController(int mingguKe) : base()
        {
            _mingguKe = mingguKe;
            CalculateWeekDates();
        }

        private void CalculateWeekDates()
        {
            DateTime startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
            _startDate = startOfYear.AddDays((_mingguKe - 1) * 7);
            _endDate = _startDate.AddDays(6);
        }

        public override DataTable AmbilData()
        {
            string query = @"
                SELECT r.id, u.username, r.nomor_hp, r.reservation_time, 
                       r.jumlah_orang, r.table_number, r.status
                FROM reservations r
                JOIN users u ON r.user_id = u.id
                WHERE r.status IN ('Selesai', 'Dibatalkan', 'Tidak Datang')
                AND DATE(r.reservation_time) BETWEEN @startDate AND @endDate
                ORDER BY r.reservation_time DESC";

            var parameters = new NpgsqlParameter[]
            {
                new("@startDate", _startDate.Date),
                new("@endDate", _endDate.Date)
            };

            return _database.GetData(query, parameters);
        }

        // Override format method for weekly specific information
        protected override string FormatRowForPrint(DataRow row)
        {
            DateTime reservationTime = Convert.ToDateTime(row["reservation_time"]);
            string dayOfWeek = reservationTime.ToString("dddd");
            
            return $"{dayOfWeek} - {base.FormatRowForPrint(row)}";
        }
    }

    // Concrete implementation for monthly reports (Inheritance)
    public class LaporanBulananController : LaporanReservasiController
    {
        private readonly int _bulanKe;
        private readonly int _tahun;

        public override string JudulLaporan => $"Laporan Reservasi Bulanan - {GetMonthName(_bulanKe)} {_tahun}";

        public LaporanBulananController(int bulanKe) : this(bulanKe, DateTime.Now.Year) { }

        public LaporanBulananController(int bulanKe, int tahun) : base()
        {
            _bulanKe = bulanKe;
            _tahun = tahun;
            CalculateMonthDates();
        }

        private void CalculateMonthDates()
        {
            _startDate = new DateTime(_tahun, _bulanKe, 1);
            _endDate = _startDate.AddMonths(1).AddDays(-1);
        }

        public override DataTable AmbilData()
        {
            string query = @"
                SELECT r.id, u.username, r.nomor_hp, r.reservation_time, 
                       r.jumlah_orang, r.table_number, r.status
                FROM reservations r
                JOIN users u ON r.user_id = u.id
                WHERE r.status IN ('Selesai', 'Dibatalkan', 'Tidak Datang')
                AND DATE(r.reservation_time) BETWEEN @startDate AND @endDate
                ORDER BY r.reservation_time DESC";

            var parameters = new NpgsqlParameter[]
            {
                new("@startDate", _startDate.Date),
                new("@endDate", _endDate.Date)
            };

            return _database.GetData(query, parameters);
        }

        private string GetMonthName(int month)
        {
            string[] monthNames = { "", "Januari", "Februari", "Maret", "April", "Mei", "Juni",
                                   "Juli", "Agustus", "September", "Oktober", "November", "Desember" };
            return monthNames[month];
        }
    }

    // // Concrete implementation for daily reports
    // public class LaporanHarianController : LaporanReservasiController
    // {
    //     private readonly DateTime _tanggal;

    //     public override string JudulLaporan => $"Laporan Reservasi Harian - {_tanggal:dd MMMM yyyy}";

    //     public LaporanHarianController(DateTime tanggal) : base()
    //     {
    //         _tanggal = tanggal.Date;
    //     }

    //     public override DataTable AmbilData()
    //     {
    //         string query = @"
    //             SELECT r.id, u.username, r.nomor_hp, r.reservation_time, 
    //                    r.jumlah_orang, r.table_number, r.status
    //             FROM reservations r
    //             JOIN users u ON r.user_id = u.id
    //             WHERE DATE(r.reservation_time) = @tanggal
    //             ORDER BY r.reservation_time";

    //         var parameters = new NpgsqlParameter[]
    //         {
    //             new("@tanggal", _tanggal)
    //         };

    //         return _database.GetData(query, parameters);
    //     }
    // }

    public class LaporanStatusController : LaporanReservasiController
    {
        private readonly ReservationStatus _status;

        public override string JudulLaporan => $"Laporan Reservasi - Status {_status}";

        public LaporanStatusController(ReservationStatus status) : base()
        {
            _status = status;
        }

        public override DataTable AmbilData()
        {
            string query = @"
                SELECT r.id, u.username, r.nomor_hp, r.reservation_time, 
                       r.jumlah_orang, r.table_number, r.status
                FROM reservations r
                JOIN users u ON r.user_id = u.id
                WHERE r.status = @status
                ORDER BY r.reservation_time DESC";

            var parameters = new NpgsqlParameter[]
            {
                new("@status", _status.ToString())
            };

            return _database.GetData(query, parameters);
        }
    }

    // Custom exception for report operations
    public class ReportException : Exception
    {
        public ReportException(string message) : base(message) { }
        public ReportException(string message, Exception innerException) : base(message, innerException) { }
    }
}