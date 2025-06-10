using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test_part_kesekian;
using test_part_kesekian.view;

namespace project_PBO.View
{
    public partial class layout_pelanggan : Form
    {
        public layout_pelanggan()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormReservasiBaru formReservasiBaru = new FormReservasiBaru();
            formReservasiBaru.MdiParent = this;
            formReservasiBaru.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lihat_reservasi Lihat_reservasi = new lihat_reservasi();
            Lihat_reservasi.MdiParent = this;
            Lihat_reservasi.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormBatalReservasi formBatalReservasi = new FormBatalReservasi();
            formBatalReservasi.MdiParent = this;
            formBatalReservasi.Show();
        }
    }
}
