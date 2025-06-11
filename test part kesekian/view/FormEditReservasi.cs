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
        }

        private void LoadReservations()
        {
            string query = @"
                SELECT r.id, u.username, r.nomor_hp, r.reservation_time, r.jumlah_orang, r.table_number, r.status
                FROM reservations r
                JOIN users u ON r.user_id = u.id
                WHERE r.status IN ('Menunggu', 'Dikonfirmasi')";
            var dt = DatabaseHelper.GetData(query);
            dgvReservations.DataSource = dt;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (dgvReservations.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih reservasi yang akan diedit!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbStatus.SelectedItem == null)
            {
                MessageBox.Show("Pilih status baru!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int reservationId = Convert.ToInt32(dgvReservations.SelectedRows[0].Cells["id"].Value);
            string newStatus = cbStatus.SelectedItem.ToString();
            string tableNumber = dgvReservations.SelectedRows[0].Cells["table_number"].Value.ToString();

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
                    reservationController.UpdateTableStatus(tableNumber, "Available");
                }
                MessageBox.Show("Status reservasi berhasil diperbarui!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadReservations();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
