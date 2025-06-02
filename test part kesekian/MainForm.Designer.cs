namespace test_part_kesekian
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button btnReservasiBaru;
        private System.Windows.Forms.Button btnEditReservasi;
        private System.Windows.Forms.Button btnBatalReservasi;
        private System.Windows.Forms.Button btnCetakLaporan;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnReservasiBaru = new System.Windows.Forms.Button();
            this.btnEditReservasi = new System.Windows.Forms.Button();
            this.btnBatalReservasi = new System.Windows.Forms.Button();
            this.btnCetakLaporan = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // btnReservasiBaru
            this.btnReservasiBaru.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnReservasiBaru.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReservasiBaru.ForeColor = System.Drawing.Color.White;
            this.btnReservasiBaru.Location = new System.Drawing.Point(50, 50);
            this.btnReservasiBaru.Name = "btnReservasiBaru";
            this.btnReservasiBaru.Size = new System.Drawing.Size(200, 40);
            this.btnReservasiBaru.TabIndex = 0;
            this.btnReservasiBaru.Text = "Reservasi Baru";
            this.btnReservasiBaru.UseVisualStyleBackColor = false;
            this.btnReservasiBaru.Click += new System.EventHandler(this.btnReservasiBaru_Click);

            // btnEditReservasi
            this.btnEditReservasi.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnEditReservasi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditReservasi.ForeColor = System.Drawing.Color.White;
            this.btnEditReservasi.Location = new System.Drawing.Point(50, 100);
            this.btnEditReservasi.Name = "btnEditReservasi";
            this.btnEditReservasi.Size = new System.Drawing.Size(200, 40);
            this.btnEditReservasi.TabIndex = 1;
            this.btnEditReservasi.Text = "Edit Reservasi";
            this.btnEditReservasi.UseVisualStyleBackColor = false;
            this.btnEditReservasi.Click += new System.EventHandler(this.btnEditReservasi_Click);

            // btnBatalReservasi
            this.btnBatalReservasi.BackColor = System.Drawing.Color.IndianRed;
            this.btnBatalReservasi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatalReservasi.ForeColor = System.Drawing.Color.White;
            this.btnBatalReservasi.Location = new System.Drawing.Point(50, 100);
            this.btnBatalReservasi.Name = "btnBatalReservasi";
            this.btnBatalReservasi.Size = new System.Drawing.Size(200, 40);
            this.btnBatalReservasi.TabIndex = 2;
            this.btnBatalReservasi.Text = "Batalkan Reservasi";
            this.btnBatalReservasi.UseVisualStyleBackColor = false;
            this.btnBatalReservasi.Click += new System.EventHandler(this.btnBatalReservasi_Click);

            // btnCetakLaporan
            this.btnCetakLaporan.BackColor = System.Drawing.Color.Orange;
            this.btnCetakLaporan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCetakLaporan.ForeColor = System.Drawing.Color.White;
            this.btnCetakLaporan.Location = new System.Drawing.Point(50, 150);
            this.btnCetakLaporan.Name = "btnCetakLaporan";
            this.btnCetakLaporan.Size = new System.Drawing.Size(200, 40);
            this.btnCetakLaporan.TabIndex = 3;
            this.btnCetakLaporan.Text = "Cetak Laporan";
            this.btnCetakLaporan.UseVisualStyleBackColor = false;
            this.btnCetakLaporan.Click += new System.EventHandler(this.btnCetakLaporan_Click);

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.btnCetakLaporan);
            this.Controls.Add(this.btnBatalReservasi);
            this.Controls.Add(this.btnEditReservasi);
            this.Controls.Add(this.btnReservasiBaru);
            this.Name = "MainForm";
            this.Text = "Sistem Reservasi";
            this.ResumeLayout(false);
        }
    }
}