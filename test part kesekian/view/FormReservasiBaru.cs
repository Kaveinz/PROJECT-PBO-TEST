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
    public partial class FormReservasiBaru : Form
    {
        public FormReservasiBaru()
        {
            InitializeComponent();
        }        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                // Input validation
                if (string.IsNullOrWhiteSpace(tbNomorHP.Text) || string.IsNullOrWhiteSpace(tbJumlahOrang.Text))
                {
                    MessageBox.Show("Harap lengkapi semua kolom!", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(tbJumlahOrang.Text, out int jumlahOrang) || jumlahOrang <= 0)
                {
                    MessageBox.Show("Jumlah orang harus berupa angka positif!", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DateTime reservationTime = dtpTanggal.Value.Date.Add(dtpWaktu.Value.TimeOfDay);

                // Check if reservation time is in the future
                if (reservationTime <= DateTime.Now)
                {
                    MessageBox.Show("Waktu reservasi harus di masa depan!", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Show table selection form
                FormPilihKursi formPilihKursi = new FormPilihKursi(reservationTime, jumlahOrang);
                formPilihKursi.MdiParent = this.MdiParent;
                formPilihKursi.OnKursiDipilih += (sender2, kursi) =>
                {
                    try
                    {
                        if (string.IsNullOrEmpty(kursi))
                        {
                            MessageBox.Show("Pilih kursi terlebih dahulu!", "Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Create reservation using enhanced model
                        var reservation = new Reservation(
                            auth_form.CurrentUser.Id,
                            tbNomorHP.Text.Trim(),
                            reservationTime,
                            jumlahOrang,
                            kursi
                        );

                        // Validate reservation data
                        if (!reservation.ValidateReservation())
                        {
                            MessageBox.Show("Data reservasi tidak valid!", "Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Create reservation using the enhanced controller
                        bool success = ReservationController.Instance.CreateReservation(reservation);
                        
                        if (success)
                        {
                            
                            
                            MessageBox.Show($"Reservasi berhasil dibuat!\n" +
                                          $"Nomor Meja: {kursi}\n" +
                                          $"Waktu: {reservationTime:dd/MM/yyyy HH:mm}\n" +
                                          $"Jumlah Orang: {jumlahOrang}\n" +
                                          $"Status: {reservation.GetStatusDescription()}", 
                                          "Reservasi Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            this.Close();
                        }
                    }
                    catch (ReservationException ex)
                    {
                        MessageBox.Show($"Gagal membuat reservasi: {ex.Message}", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                formPilihKursi.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
