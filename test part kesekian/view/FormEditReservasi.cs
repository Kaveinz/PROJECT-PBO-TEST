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

            int id_reservasi = Convert.ToInt32(dgvReservations.SelectedRows[0].Cells["ID Reservasi"].Value);
            string status_baru = cbStatus.SelectedItem.ToString() ?? "";
            string nomor_meja = dgvReservations.SelectedRows[0].Cells["Nomor Meja"].Value?.ToString() ?? "";

            if (string.IsNullOrEmpty(status_baru) || string.IsNullOrEmpty(nomor_meja))
            {
                MessageBox.Show("Data tidak valid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "UPDATE reservations SET status = @status WHERE id = @id";
            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@status", status_baru),
                new NpgsqlParameter("@id", id_reservasi)
            };

            try
            {
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                if (status_baru == "Selesai" || status_baru == "Dibatalkan" || status_baru == "Tidak Datang")
                {
                    ReservationController.UpdateTableStatus(nomor_meja, "Available");
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
