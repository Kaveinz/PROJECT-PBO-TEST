using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test_part_kesekian.models;

namespace test_part_kesekian.view
{
    public partial class layout_admin : Form
    {
        private PrintDocument printDocument;
        public layout_admin()
        {
            InitializeComponent();
            printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
        }

        private void closeAllMdiChild()
        {
            foreach (Form childform in this.MdiChildren.ToArray())
            {
                childform.Close();
                childform.Dispose();
            }
        }

        private void layout_admin_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            closeAllMdiChild();

        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {

            string query = @"
                SELECT r.id, u.username, r.nomor_hp, r.reservation_time, r.jumlah_orang, r.table_number, r.status
                FROM reservations r
                JOIN users u ON r.user_id = u.id
                WHERE r.status IN ('Selesai', 'Dibatalkan', 'Tidak Datang')";

            var dt = DatabaseHelper.GetData(query);

            float yPos = 100;
            int count = 0;
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
                count++;
                if (yPos > e.MarginBounds.Height)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            e.Graphics.DrawString("--------------------------------------------------", new Font("Arial", 12), Brushes.Black, new PointF(100, yPos));
            e.Graphics.DrawString(footer, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new PointF(100, yPos + 20));
        }

        private void btnCetakLaporan_Click(object sender, EventArgs e)
        {
            string query = @"
                SELECT r.id, u.username, r.nomor_hp, r.reservation_time, r.jumlah_orang, r.table_number, r.status
                FROM reservations r
                JOIN users u ON r.user_id = u.id
                WHERE r.status IN ('Selesai', 'Dibatalkan', 'Tidak Datang')";

            var dt = DatabaseHelper.GetData(query);

            using (Form form = new Form { Text = "Laporan Reservasi", Size = new System.Drawing.Size(800, 400) })
            {
                DataGridView dgv = new DataGridView
                {
                    Dock = DockStyle.Top,
                    Height = 300,
                    DataSource = dt
                };

                Button btnExport = new Button
                {
                    Text = "Export to Text",
                    Location = new System.Drawing.Point(10, 310),
                    Size = new System.Drawing.Size(120, 30)
                };
                btnExport.Click += (s, ev) =>
                {
                    DatabaseHelper.ExportToTextFile(dt, "Laporan_Reservasi.txt");
                    MessageBox.Show("Laporan telah diekspor ke Laporan_Reservasi.txt", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                Button btnPrint = new Button
                {
                    Text = "Print Preview",
                    Location = new System.Drawing.Point(140, 310),
                    Size = new System.Drawing.Size(120, 30)
                };
                btnPrint.Click += (s, ev) =>
                {
                    PrintPreviewDialog previewDialog = new PrintPreviewDialog();
                    previewDialog.Document = printDocument;
                    previewDialog.ShowDialog();
                };

                form.Controls.Add(dgv);
                form.Controls.Add(btnExport);
                form.Controls.Add(btnPrint);
                form.ShowDialog();
            }
    }   }
}
