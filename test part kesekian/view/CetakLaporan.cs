using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
              JOIN users u ON r.user_id = u.id
              WHERE r.status IN ('Selesai', 'Dibatalkan', 'Tidak Datang')";


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
            if (dtLaporan == null || dtLaporan.Rows.Count == 0)
            {
                MessageBox.Show("Untuk Menectak Laporan, Silahkan Filter Terlebih Dahulu (Bulanan/Mingguan)", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Text Files (*.txt)|*.txt";
                saveDialog.FileName = "laporan_reservasi_mbok_wo.txt";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        var columnNames = dtLaporan.Columns.Cast<DataColumn>()
                                                      .Select(column => column.ColumnName)
                                                      .ToList();

                        // Calculate column widths
                        List<int> columnWidths = new List<int>();
                        for (int i = 0; i < columnNames.Count; i++)
                        {
                            columnWidths.Add(columnNames[i].Length);
                        }

                        foreach (DataRow row in dtLaporan.Rows)
                        {
                            for (int i = 0; i < columnNames.Count; i++)
                            {
                                string cellValue = row[i]?.ToString() ?? string.Empty;
                                if (cellValue.Length > columnWidths[i])
                                {
                                    columnWidths[i] = cellValue.Length;
                                }
                            }
                        }

                        // Format Header
                        string formattedHeader = "";
                        for(int i=0; i< columnNames.Count; i++)
                        {
                            formattedHeader += columnNames[i].PadRight(columnWidths[i]);
                            if (i < columnNames.Count - 1)
                                formattedHeader += " | ";
                        }
                        sb.AppendLine(formattedHeader);

                        // Format Separator
                        string formattedSeparator = "";
                        for(int i=0; i< columnNames.Count; i++)
                        {
                            formattedSeparator += new string('-', columnWidths[i]);
                            if (i < columnNames.Count - 1)
                                formattedSeparator += "-+-"; 
                        }
                        sb.AppendLine(formattedSeparator);

                        // Format Data Rows
                        foreach (DataRow row in dtLaporan.Rows)
                        {
                            string formattedRow = "";
                            for (int i = 0; i < columnNames.Count; i++)
                            {
                                string cellValue = row[i]?.ToString() ?? string.Empty;
                                formattedRow += cellValue.PadRight(columnWidths[i]);
                                if (i < columnNames.Count - 1)
                                    formattedRow += " | ";
                            }
                            sb.AppendLine(formattedRow);
                        }

                        File.WriteAllText(saveDialog.FileName, sb.ToString());
                        MessageBox.Show("Laporan Berhasil Diekspor!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Gagal mengekspor laporan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MessageBox.Show("Silahkan Pilih Minggu Terlebih Dahuly", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    int mingguKe = Convert.ToInt32(comboBox3.SelectedItem);
                    controller = new LaporanMingguanController(mingguKe);
                    break;

                case "Bulanan":
                    if (comboBox2.SelectedItem == null)
                    {
                        MessageBox.Show("Silahkan Pilih Bulan Terlebih Dahuly", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
