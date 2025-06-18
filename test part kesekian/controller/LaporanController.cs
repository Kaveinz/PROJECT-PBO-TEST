using System.Data;
using System.Drawing.Printing;
using Npgsql;
using test_part_kesekian.models;

namespace test_part_kesekian.controller
{
    public interface IReportService
    {
        string JudulLaporan { get; }
        DataTable AmbilData();
        void CetakLaporan(PrintPageEventArgs e);
        void ExportToFile(string filePath);
    }

    public class LaporanReservasi
    {
        public int totalReservasi { get; set; }
        public int reservasiPending { get; set; }
        public int reservasiDikonfirmasi { get; set; }
        public int reservasiSelesai { get; set; }
        public int reservasiDibatalkan { get; set; }
        public DateTime tanggalLaporan { get; set; }

        public LaporanReservasi()
        {
            tanggalLaporan = DateTime.Now;
        }

        public double rataRataPembatalan => totalReservasi > 0 ? (double)rataRataPembatalan / totalReservasi * 100 : 0;
        public double rataRataSelesai => totalReservasi > 0 ? (double)rataRataSelesai / totalReservasi * 100 : 0;
    }

    public abstract class LaporanReservasiController : IReportService
    {
        protected readonly IDatabaseOperations _database;
        protected DateTime startDate;
        protected DateTime endDate;

        public abstract string JudulLaporan { get; }

        protected LaporanReservasiController()
        {
            _database = DatabaseHelper.Instance;
        }

        protected LaporanReservasiController(DateTime _startDate, DateTime _endDate) : this()
        {
            startDate = _startDate;
            endDate = _endDate;
        }

        public abstract DataTable AmbilData();

        public virtual void CetakLaporan(PrintPageEventArgs e)
        {
            DataTable dt = AmbilData();
            float yPos = 100;
            string header = JudulLaporan;
            string footer = $"Total Reservasi: {dt.Rows.Count}";

            e.Graphics.DrawString(header, new Font("Arial", 16, FontStyle.Bold), Brushes.Black, new PointF(100, 50));
            e.Graphics.DrawString("".PadRight(50, '-'), new Font("Arial", 12), Brushes.Black, new PointF(100, 80));

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

            e.Graphics.DrawString("".PadRight(50, '-'), new Font("Arial", 12), Brushes.Black, new PointF(100, yPos));
            e.Graphics.DrawString(footer, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new PointF(100, yPos + 20));
        }

        protected virtual string FormatRowForPrint(DataRow row)
        {
            return $"ID: {row["id"]}, User: {row["username"]}, HP: {row["nomor_hp"]}, " +
                   $"Waktu: {Convert.ToDateTime(row["reservation_time"]):yyyy-MM-dd HH:mm}, " +
                   $"Orang: {row["jumlah_orang"]}, Status: {row["status"]}";
        }

        public virtual void ExportToFile(string filePath)
        {
            DataTable dt = AmbilData();
            ExportToTextFile(dt, filePath);
        }

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

        public virtual LaporanReservasi GetReportSummary()
        {
            DataTable dt = AmbilData();
            
            var summary = new LaporanReservasi
            {
                totalReservasi = dt.Rows.Count
            };

            foreach (DataRow row in dt.Rows)
            {
                string status = row["status"].ToString().ToLower();
                switch (status)
                {
                    case "menunggu":
                        summary.reservasiPending++;
                        break;
                    case "dikonfirmasi":
                        summary.reservasiDikonfirmasi++;
                        break;
                    case "selesai":
                        summary.reservasiSelesai++;
                        break;
                    case "dibatalkan":
                        summary.reservasiDibatalkan++;
                        break;
                }
            }

            return summary;
        }        
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

    public class LaporanMingguanController : LaporanReservasiController
    {
        private readonly int minggu;

        public override string JudulLaporan => $"Laporan Reservasi Mingguan - Minggu ke-{minggu}";

        public LaporanMingguanController(int mingguKe) : base()
        {
            minggu = mingguKe;
            CalculateWeekDates();
        }

        private void CalculateWeekDates()
        {
            DateTime startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
            startDate = startOfYear.AddDays((minggu - 1) * 7);
            endDate = startDate.AddDays(6);
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
                new("@startDate", startDate.Date),
                new("@endDate", endDate.Date)
            };

            return _database.GetData(query, parameters);
        }

        protected override string FormatRowForPrint(DataRow row)
        {
            DateTime reservationTime = Convert.ToDateTime(row["reservation_time"]);
            string dayOfWeek = reservationTime.ToString("dddd");
            
            return $"{dayOfWeek} - {base.FormatRowForPrint(row)}";
        }
    }

    public class LaporanBulananController : LaporanReservasiController
    {
        private readonly int bulan;
        private readonly int tahun;

        public override string JudulLaporan => $"Laporan Reservasi Bulanan - {GetNamaBulan(bulan)} {tahun}";

        public LaporanBulananController(int bulanKe) : this(bulanKe, DateTime.Now.Year) { }

        public LaporanBulananController(int bulanKe, int tahunKe) : base()
        {
            bulan  = bulanKe;
            tahun = tahunKe;
            KalkulasiBulan();
        }

        private void KalkulasiBulan()
        {
            startDate = new DateTime(tahun, bulan, 1);
            endDate = startDate.AddMonths(1).AddDays(-1);
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
                new("@startDate", startDate.Date),
                new("@endDate", endDate.Date)
            };

            return _database.GetData(query, parameters);
        }

        private string GetNamaBulan(int bulan)
        {
            string[] namaBulan = { "", "Januari", "Februari", "Maret", "April", "Mei", "Juni",
                                   "Juli", "Agustus", "September", "Oktober", "November", "Desember" };
            return namaBulan[bulan];
        }
    }

    public class LaporanStatusController : LaporanReservasiController
    {
        private readonly ReservationStatus status;

        public override string JudulLaporan => $"Laporan Reservasi - Status {status}";

        public LaporanStatusController(ReservationStatus Status) : base()
        {
            status = Status;
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
                new("@status", status.ToString())
            };

            return _database.GetData(query, parameters);
        }
    }

    public class ReportException : Exception
    {
        public ReportException(string message) : base(message) { }
        public ReportException(string message, Exception innerException) : base(message, innerException) { }
    }
}