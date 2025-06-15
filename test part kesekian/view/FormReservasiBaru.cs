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
                // Validasi Input
                if (string.IsNullOrWhiteSpace(tbNomorHP.Text) || string.IsNullOrWhiteSpace(tbJumlahOrang.Text))
                {
                    MessageBox.Show("Maaf, Silahkan Isi Semua Data Reservasi.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(tbJumlahOrang.Text, out int jumlahOrang) || jumlahOrang <= 0)
                {
                    MessageBox.Show("Maaf, Jumlah Orang Tidak Boleh Kurang Dari 0.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DateTime reservationTime = dtpTanggal.Value.Date.Add(dtpWaktu.Value.TimeOfDay);

                // Waktu reservasi harus di hari yang beda
                if (reservationTime <= DateTime.Now)
                {
                    MessageBox.Show("Maaf, Reservasi Tidak Bisa Dilakukan Saat Ini. Silahkan Pilih Waktu Reservasi yang Sesuai. ", "Error", 
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
                            MessageBox.Show("Silahkan Pilih Kursi Terlebih Dahulu!", "Error", 
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


                        if (!reservation.ValidateReservation())
                        {
                            MessageBox.Show("Maaf, Reservasi Tidak Valid.", "Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Create reservation using the enhanced controller
                        bool success = ReservationController.Instance.CreateReservation(reservation);
                        
                        if (success)
                        {
                            
                            
                            MessageBox.Show($"Reservasi Berhasil!\n" +
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
