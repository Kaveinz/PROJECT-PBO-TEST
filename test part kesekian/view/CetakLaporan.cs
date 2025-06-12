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
    public partial class CetakLaporan : Form
    {
        private readonly LaporanReservasiController controller = new LaporanReservasiController();
        private readonly PrintDocument printDocument = new PrintDocument();
        private DataTable dtLaporan;

        public CetakLaporan()
        {
            InitializeComponent();
            Load += FormLaporanReservasi_Load;
        }

        private void FormLaporanReservasi_Load(object sender, EventArgs e)
        {
            LoadData();

        }
        private void LoadData()
        {
            string query = @"
            SELECT r.id, u.username, r.nomor_hp, r.reservation_time, 
                   r.jumlah_orang, r.table_number, r.status
            FROM reservations r
            JOIN users u ON r.user_id = u.id
            WHERE r.status IN ('Selesai', 'Dibatalkan', 'Tidak Datang')";

            DataTable dt = DatabaseHelper.GetData(query);
            dvgLaporan.DataSource = dt;
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            controller.CetakLaporan(e, dtLaporan);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Text Files (*.txt)|*.txt";
                saveDialog.FileName = "Laporan_Reservasi.txt";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    LaporanReservasiController.ExportToTextFile((DataTable)dvgLaporan.DataSource, saveDialog.FileName);
                    MessageBox.Show("Laporan telah diekspor!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dvgLaporan.DataSource is DataTable dt)
            {
                dtLaporan = dt; // simpan ke variabel yang akan digunakan saat print
                printDocument.PrintPage += PrintDocument_PrintPage;

                PrintPreviewDialog previewDialog = new PrintPreviewDialog
                {
                    Document = printDocument
                };

                previewDialog.ShowDialog();
            }
        }
    }


}
