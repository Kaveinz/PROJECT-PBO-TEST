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
                SELECT r.id, u.username, r.nomor_hp, r.reservation_time, r.jumlah_orang, r.table_number, r.status
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

            int reservationId = Convert.ToInt32(dgvReservations.SelectedRows[0].Cells["id"].Value);
            string tableNumber = dgvReservations.SelectedRows[0].Cells["table_number"].Value.ToString();

            string query = "UPDATE reservations SET status = 'Dibatalkan' WHERE id = @id";
            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@id", reservationId)
            };

            try
            {
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                DatabaseHelper.UpdateTableStatus(tableNumber, "Available");
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
