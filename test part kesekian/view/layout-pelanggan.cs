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

        private void closeAllMdiChild()
        {
            foreach (Form childform in this.MdiChildren.ToArray())
            {
                childform.Close();
                childform.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            closeAllMdiChild();
            FormReservasiBaru formReservasiBaru = new FormReservasiBaru();
            formReservasiBaru.MdiParent = this;
            formReservasiBaru.Show();
            formReservasiBaru.WindowState = FormWindowState.Maximized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            closeAllMdiChild();
            lihat_reservasi Lihat_reservasi = new lihat_reservasi();
            Lihat_reservasi.MdiParent = this;
            Lihat_reservasi.Show();
            Lihat_reservasi.WindowState = FormWindowState.Maximized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            closeAllMdiChild();
            FormBatalReservasi formBatalReservasi = new FormBatalReservasi();
            formBatalReservasi.MdiParent = this;
            formBatalReservasi.Show();
            formBatalReservasi.WindowState = FormWindowState.Maximized;
        }

        private void layout_pelanggan_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
