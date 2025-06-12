using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test_part_kesekian.controller;
using test_part_kesekian.models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace test_part_kesekian
{
    public partial class FormPilihKursi : Form
    {
        private readonly DateTime reservationTime;
        private readonly int jumlahOrang;
        private string selectedKursi = null;

        public string SelectedKursi => selectedKursi;
        public event EventHandler<string> OnKursiDipilih;


        public FormPilihKursi(DateTime reservationTime, int jumlahOrang)
        {
            InitializeComponent();
            this.reservationTime = reservationTime;
            this.jumlahOrang = jumlahOrang;
        }

        private void FormPilihKursi_Load(object sender, EventArgs e)
        {
            CheckKursiAvailability(meja1, "A1");
            CheckKursiAvailability(meja2, "A2");
            CheckKursiAvailability(meja3, "A3");
            CheckKursiAvailability(meja4, "A4");
            CheckKursiAvailability(meja5, "A5");
            CheckKursiAvailability(meja6, "A6");
            CheckKursiAvailability(meja7, "A7");
        }

        private void CheckKursiAvailability(Button meja, string kodeKursi)
        {
            bool isAvailable = reservationController.IsTableAvailable(kodeKursi, reservationTime);
            bool isCapacitySufficient = reservationController.IsTableCapacitySufficient(kodeKursi, jumlahOrang);

            if (!isAvailable || !isCapacitySufficient)
            {
                meja.BackColor = Color.Red;
                meja.Enabled = false;
            }
            else
            {
                meja.BackColor = SystemColors.Control;
                meja.Click += Kursi_Click;
            }
        }

        private void Kursi_Click(object sender, EventArgs e)
        {
            Button meja = sender as Button;
            if (meja.BackColor == Color.Green)
            {
                meja.BackColor = SystemColors.Control;
                selectedKursi = null;
            }
            else
            {
                foreach (Control ctrl in groupBox1.Controls)
                {
                    if (ctrl is Button btn && btn.BackColor == Color.Green)
                    {
                        btn.BackColor = SystemColors.Control;
                    }
                }
                meja.BackColor = Color.Green;
                selectedKursi = meja.Text;
            }
        }

        private void btnKonfirmasi_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedKursi))
            {
                OnKursiDipilih?.Invoke(this, SelectedKursi);
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Silakan pilih kursi terlebih dahulu.");
            }
        }
    }
}
