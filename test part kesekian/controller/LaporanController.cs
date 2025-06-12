

using System.Data;
using System.Drawing.Printing;
using test_part_kesekian.models;

namespace test_part_kesekian
{
    public class LaporanReservasiController
    {
        private const string Query = @"
        SELECT r.id, u.username, r.nomor_hp, r.reservation_time, r.jumlah_orang, r.table_number, r.status
        FROM reservations r
        JOIN users u ON r.user_id = u.id
        WHERE r.status IN ('Selesai', 'Dibatalkan', 'Tidak Datang')";

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

        public DataTable GetLaporanReservasi()
        {
            return DatabaseHelper.GetData(Query);
        }


        public void CetakLaporan(PrintPageEventArgs e, DataTable dt)
        {
            float yPos = 100;
            string header = "Laporan Reservasi";
            string footer = $"Total Reservasi: {dt.Rows.Count}";

            e.Graphics.DrawString(header, new Font("Arial", 16, FontStyle.Bold), Brushes.Black, new PointF(100, 50));
            e.Graphics.DrawString("--------------------------------------------------", new Font("Arial", 12), Brushes.Black, new PointF(100, 80));

            foreach (DataRow row in dt.Rows)
            {
                string line = $"ID: {row["id"]}, Username: {row["username"]}, HP: {row["nomor_hp"]}, " +
                              $"Waktu: {Convert.ToDateTime(row["reservation_time"]).ToString("yyyy-MM-dd HH:mm")}, " +
                              $"Orang: {row["jumlah_orang"]}, Meja: {row["table_number"]}, Status: {row["status"]}";
                e.Graphics.DrawString(line, new Font("Arial", 12), Brushes.Black, new PointF(100, yPos));
                yPos += 20;
                if (yPos > e.MarginBounds.Height)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            e.Graphics.DrawString("--------------------------------------------------", new Font("Arial", 12), Brushes.Black, new PointF(100, yPos));
            e.Graphics.DrawString(footer, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new PointF(100, yPos + 20));
        }
    }
}