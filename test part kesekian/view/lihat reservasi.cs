using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test_part_kesekian.models;

namespace test_part_kesekian.view
{
    public partial class lihat_reservasi : Form
    {
        public lihat_reservasi()
        {
            InitializeComponent();
            LoadReservations();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void gridAllReservasi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadReservations()
        {
            string query = @"
                SELECT r.id, u.username, r.reservation_time,r.table_number
                FROM reservations r
                JOIN users u ON r.user_id = u.id
                WHERE r.status IN ('Menunggu', 'Dikonfirmasi')";
          

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        gridAllReservasi.DataSource = dt;
                    }
                }
            }
        }
    }
}
