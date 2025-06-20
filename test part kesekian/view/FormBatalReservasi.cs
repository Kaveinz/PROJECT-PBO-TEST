using System.Data;
using Npgsql;
using test_part_kesekian.controller;
using test_part_kesekian.models;

namespace test_part_kesekian
{
    public partial class FormBatalReservasi : Form
    {
        public FormBatalReservasi()
        {
            InitializeComponent();
            LoadReservations();
        }

        private void LoadReservations()
        {
            string query = @"
                SELECT r.id as ""ID Reservasi"", u.username as ""Username"", r.nomor_hp as ""Nomor HP"", r.reservation_time as ""Waktu Reservasi"", r.jumlah_orang as ""Jumlah Orang"", r.table_number as ""Nomor Meja"", r.status as ""Status""
                FROM reservations r
                JOIN users u ON r.user_id = u.id
                WHERE r.user_id = @user_id AND r.status IN ('Menunggu', 'Dikonfirmasi')";
            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@user_id", auth_form.CurrentUser.Id)
            };

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvReservations.DataSource = dt;
                    }
                }
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            if (dgvReservations.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih reservasi yang akan dibatalkan!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int reservationId = Convert.ToInt32(dgvReservations.SelectedRows[0].Cells["ID Reservasi"].Value);
            string tableNumber = dgvReservations.SelectedRows[0].Cells["Nomor Meja"].Value.ToString();

            string query = "UPDATE reservations SET status = 'Dibatalkan' WHERE id = @id";
            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@id", reservationId)
            };

            try
            {
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                ReservationController.UpdateStatusMejaStatic(tableNumber, "Available");
                MessageBox.Show("Reservasi berhasil dibatalkan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadReservations();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormBatalReservasi_Load(object sender, EventArgs e)
        {

        }
    }
}
