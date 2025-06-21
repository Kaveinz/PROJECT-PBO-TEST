using System.Data;
using System.Drawing.Printing;
using System.Text;
using test_part_kesekian.models;
using test_part_kesekian.controller;

namespace test_part_kesekian.view
{
    public partial class CetakLaporan : Form
    {
        private LaporanReservasiController controller;
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
              SELECT r.id AS ""ID Reservasi"", 
                     u.username AS ""Nama Pengguna"", 
                     r.nomor_hp AS ""Nomor HP"", 
                     r.reservation_time AS ""Waktu Reservasi"", 
                     r.jumlah_orang AS ""Jumlah Orang"", 
                     r.table_number AS ""Nomor Meja"", 
                     r.status AS ""Status""
              FROM reservations r
              JOIN users u ON r.user_id = u.id";


            DataTable dt = DatabaseHelper.GetData(query);
            dvgLaporan.DataSource = dt;
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            controller.CetakLaporan(e);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (controller == null)
            {
                MessageBox.Show("Laporan belum dipilih atau belum diproses.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text Files (*.txt)|*.txt";
                sfd.FileName = "laporan_reservasi.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        controller.ExportFormat(sfd.FileName);
                        MessageBox.Show("Laporan berhasil diekspor!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Gagal ekspor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            string jenis = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(jenis))
            {
                MessageBox.Show("Atur Jenis Laporan Terlebih Dahulu! (Mingguan/Bulanan)", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            switch (jenis)
            {
                case "Mingguan":
                    if (comboBox3.SelectedItem == null)
                    {
                        MessageBox.Show("Silahkan Pilih Minggu Terlebih Dahulu", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    int mingguKe = Convert.ToInt32(comboBox3.SelectedItem);
                    controller = new LaporanMingguanController(mingguKe);
                    break;

                case "Bulanan":
                    if (comboBox2.SelectedItem == null)
                    {
                        MessageBox.Show("Silahkan Pilih Bulan Terlebih Dahulu", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    int bulanKe = Convert.ToInt32(comboBox2.SelectedItem);
                    controller = new LaporanBulananController(bulanKe);
                    break;
            }

            dtLaporan = controller.AmbilData();
            dvgLaporan.DataSource = dtLaporan;

            printDocument.PrintPage -= PrintDocument_PrintPage;
            printDocument.PrintPage += PrintDocument_PrintPage;

            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDocument
            };

            previewDialog.ShowDialog();
        }
    }


}
