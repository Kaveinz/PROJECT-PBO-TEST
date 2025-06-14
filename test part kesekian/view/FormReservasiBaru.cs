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
using test_part_kesekian.controller;
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


            if (!int.TryParse(tbJumlahOrang.Text, out int jumlahOrang) || jumlahOrang <= 0)
            {
                MessageBox.Show("Jumlah orang harus berupa angka positif!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime reservationTime = dtpTanggal.Value.Date.Add(dtpWaktu.Value.TimeOfDay);

            FormPilihKursi formPilihKursi = new FormPilihKursi(reservationTime, jumlahOrang);
            formPilihKursi.MdiParent = this.MdiParent; // atur parent-nya agar jadi MDI Child
            formPilihKursi.OnKursiDipilih += (sender2, kursi) =>
            {
                if (string.IsNullOrEmpty(kursi))
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
        new NpgsqlParameter("@nomor_hp", auth_form.CurrentUser.nomor_hp),
        new NpgsqlParameter("@reservation_time", reservationTime),
        new NpgsqlParameter("@jumlah_orang", jumlahOrang),
        new NpgsqlParameter("@table_number", kursi)
                };

                try
                {
                    DatabaseHelper.ExecuteNonQuery(query, parameters);
                    reservationController.UpdateTableStatus(kursi, "Reserved");
                    MessageBox.Show("Reservasi berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            formPilihKursi.Show();
        }
    }
}
