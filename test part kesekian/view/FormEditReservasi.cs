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
    public partial class FormEditReservasi : Form
    {
        public FormEditReservasi()
        {
            InitializeComponent();
            LoadReservations();
        }        private void LoadReservations()
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
                WHERE r.status IN ('Menunggu', 'Dikonfirmasi')";
            var dt = DatabaseHelper.GetData(query);
            dgvReservations.DataSource = dt;
        }        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (dgvReservations.SelectedRows.Count == 0)
            {
                MessageBox.Show("Silahkan Pilih Reservasi yang Ingin Diubah Statusnya!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbStatus.SelectedItem == null)
            {
                MessageBox.Show("Pilih Status Reservasi yang Baru!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int reservationId = Convert.ToInt32(dgvReservations.SelectedRows[0].Cells["ID Reservasi"].Value);
            string newStatus = cbStatus.SelectedItem.ToString() ?? "";
            string tableNumber = dgvReservations.SelectedRows[0].Cells["Nomor Meja"].Value?.ToString() ?? "";

            if (string.IsNullOrEmpty(newStatus) || string.IsNullOrEmpty(tableNumber))
            {
                MessageBox.Show("Data tidak valid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "UPDATE reservations SET status = @status WHERE id = @id";
            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@status", newStatus),
                new NpgsqlParameter("@id", reservationId)
            };

            try
            {
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                if (newStatus == "Selesai" || newStatus == "Dibatalkan" || newStatus == "Tidak Datang")
                {
                    ReservationController.UpdateTableStatus(tableNumber, "Available");
                }
                MessageBox.Show("Status Reservasi Berhasil Diperbarui!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadReservations();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
