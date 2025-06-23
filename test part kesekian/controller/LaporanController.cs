using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using Npgsql;
using test_part_kesekian.models;

namespace test_part_kesekian.controller
{
    public interface Laporan
    {
        string JudulLaporan { get; }
        DataTable AmbilData();
        void CetakLaporan(PrintPageEventArgs e);


    }

    public class LaporanReservasi
    {
        public int totalReservasi { get; set; }
        public int reservasiPending { get; set; }
        public int reservasiSelesai { get; set; }
        public int reservasiDibatalkan { get; set; }
        public int reservasiTidakDatang { get; set; }
        public DateTime tanggalLaporan { get; set; }

        public LaporanReservasi()
        {
            tanggalLaporan = DateTime.Now;
        }

    }

    public abstract class LaporanReservasiController : Laporan
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
                string line = FormatData(row);
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

        protected virtual string FormatData(DataRow row)
        {
            return $"ID: {row["id"]}, User: {row["username"]}, HP: {row["nomor_hp"]}, " +
                   $"Waktu: {Convert.ToDateTime(row["reservation_time"]):yyyy-MM-dd HH:mm}, " +
                   $"Orang: {row["jumlah_orang"]}, Status: {row["status"]}";
        }

        public virtual void ExportFormat(string filePath)
        {
            DataTable dt = AmbilData();

            var columnNames = dt.Columns.Cast<DataColumn>()
                                  .Select(col => col.ColumnName)
                                  .ToList();

            List<int> columnWidths = new List<int>();
            for (int i = 0; i < columnNames.Count; i++)
                columnWidths.Add(columnNames[i].Length);

            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < columnNames.Count; i++)
                {
                    string val = row[i]?.ToString() ?? "";
                    if (val.Length > columnWidths[i])
                        columnWidths[i] = val.Length;
                }
            }

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                
                for (int i = 0; i < columnNames.Count; i++)
                {
                    sw.Write(columnNames[i].PadRight(columnWidths[i]));
                    if (i < columnNames.Count - 1) sw.Write(" | ");
                }
                sw.WriteLine();

                
                for (int i = 0; i < columnNames.Count; i++)
                {
                    sw.Write(new string('-', columnWidths[i]));
                    if (i < columnNames.Count - 1) sw.Write("-+-");
                }
                sw.WriteLine();


                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < columnNames.Count; i++)
                    {
                        sw.Write((row[i]?.ToString() ?? "").PadRight(columnWidths[i]));
                        if (i < columnNames.Count - 1) sw.Write(" | ");
                    }
                    sw.WriteLine();
                }

                
                var summary = HitungSummaryDariDataTable(dt);
                sw.WriteLine();
                sw.WriteLine("Ringkasan Status:");
                sw.WriteLine($"- Menunggu     : {summary.reservasiPending}");
                sw.WriteLine($"- Selesai      : {summary.reservasiSelesai}");
                sw.WriteLine($"- Dibatalkan   : {summary.reservasiDibatalkan}");
                sw.WriteLine($"- Tidak Datang : {summary.reservasiTidakDatang}");
                sw.WriteLine();
                sw.WriteLine($"Total Reservasi: {summary.totalReservasi}");
            }
        }




        protected virtual LaporanReservasi HitungSummaryDariDataTable(DataTable dt)
        {
            var summary = new LaporanReservasi
            {
                totalReservasi = dt.Rows.Count
            };

            foreach (DataRow row in dt.Rows)
            {
                string status = row["status"]?.ToString()?.Trim().ToLower() ?? "";

                switch (status)
                {
                    case "menunggu":
                        summary.reservasiPending++; break;
                    case "selesai":
                        summary.reservasiSelesai++; break;
                    case "dibatalkan":
                        summary.reservasiDibatalkan++; break;
                    case "tidak datang":
                        summary.reservasiTidakDatang++; break;
                }
            }

            return summary;
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
            hitungtanggalmingguan();
        }

        private void hitungtanggalmingguan()
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
                AND DATE(r.reservation_time) BETWEEN @startDate AND @endDate
                ORDER BY r.reservation_time DESC";

            var parameters = new NpgsqlParameter[]
            {
                new("@startDate", startDate.Date),
                new("@endDate", endDate.Date)
            };

            return _database.GetData(query, parameters);
        }

        //protected override string FormatData(DataRow row)
        //{
        //    DateTime reservationTime = Convert.ToDateTime(row["reservation_time"]);
        //    string dayOfWeek = reservationTime.ToString("dddd");
            
        //    return $"{dayOfWeek} - {base.FormatData(row)}";
        //}
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

    

    public class ReportException : Exception
    {
        public ReportException(string message) : base(message) { }
        public ReportException(string message, Exception innerException) : base(message, innerException) { }
    }
}