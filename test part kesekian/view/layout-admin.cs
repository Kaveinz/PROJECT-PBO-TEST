using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test_part_kesekian.models;

namespace test_part_kesekian.view
{
    public partial class layout_admin : Form
    {
        private PrintDocument printDocument;
        public layout_admin()
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

        private void layout_admin_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            closeAllMdiChild();
            FormEditReservasi formEditReservasi = new FormEditReservasi();
            formEditReservasi.MdiParent = this;
            formEditReservasi.WindowState = FormWindowState.Maximized;
            formEditReservasi.Show();

        }

       
        private void btnCetakLaporan_Click(object sender, EventArgs e)
        {

            closeAllMdiChild();
            CetakLaporan laporanForm = new CetakLaporan();
            laporanForm.MdiParent = this; // Penting: tetapkan parent MDI-nya
            laporanForm.WindowState = FormWindowState.Maximized; // Opsional
            laporanForm.Show();
        }   }
}
