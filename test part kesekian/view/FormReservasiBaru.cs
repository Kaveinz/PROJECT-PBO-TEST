using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using test_part_kesekian.models;

namespace test_part_kesekian
{
    public partial class FormReservasiBaru : Form
    {
        public FormReservasiBaru()
        {
            InitializeComponent();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbNomorHP.Text) || string.IsNullOrWhiteSpace(tbJumlahOrang.Text))
            {
                MessageBox.Show("Harap lengkapi semua kolom!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(tbJumlahOrang.Text, out int jumlahOrang) || jumlahOrang <= 0)
            {
                MessageBox.Show("Jumlah orang harus berupa angka positif!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime reservationTime = dtpTanggal.Value.Date.Add(dtpWaktu.Value.TimeOfDay);

            using (var formPilihKursi = new FormPilihKursi(reservationTime, jumlahOrang))
            {
                if (formPilihKursi.ShowDialog() == DialogResult.OK)
                {
                    string tableNumber = formPilihKursi.SelectedKursi;
                    if (string.IsNullOrEmpty(tableNumber))
                    {
                        MessageBox.Show("Pilih kursi terlebih dahulu!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string query = @"
                        INSERT INTO reservations (user_id, nomor_hp, reservation_time, jumlah_orang, table_number, status)
                        VALUES (@user_id, @nomor_hp, @reservation_time, @jumlah_orang, @table_number, 'Menunggu')";

                    var parameters = new NpgsqlParameter[]
                    {
                        new NpgsqlParameter("@user_id", auth_form.CurrentUser.Id),
                        new NpgsqlParameter("@nomor_hp", tbNomorHP.Text),
                        new NpgsqlParameter("@reservation_time", reservationTime),
                        new NpgsqlParameter("@jumlah_orang", jumlahOrang),
                        new NpgsqlParameter("@table_number", tableNumber)
                    };

                    try
                    {
                        DatabaseHelper.ExecuteNonQuery(query, parameters);
                        DatabaseHelper.UpdateTableStatus(tableNumber, "Reserved");
                        MessageBox.Show("Reservasi berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
