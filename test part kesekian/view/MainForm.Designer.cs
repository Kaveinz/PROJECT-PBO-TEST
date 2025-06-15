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
            btnReservasiBaru = new Button();
            btnEditReservasi = new Button();
            btnBatalReservasi = new Button();
            btnCetakLaporan = new Button();
            SuspendLayout();
            // 
            // btnReservasiBaru
            // 
            btnReservasiBaru.BackColor = Color.MediumSeaGreen;
            btnReservasiBaru.FlatStyle = FlatStyle.Flat;
            btnReservasiBaru.ForeColor = Color.White;
            btnReservasiBaru.Location = new Point(67, 77);
            btnReservasiBaru.Margin = new Padding(4, 5, 4, 5);
            btnReservasiBaru.Name = "btnReservasiBaru";
            btnReservasiBaru.Size = new Size(267, 62);
            btnReservasiBaru.TabIndex = 0;
            btnReservasiBaru.Text = "Reservasi Baru";
            btnReservasiBaru.UseVisualStyleBackColor = false;
            btnReservasiBaru.Click += btnReservasiBaru_Click;
            // 
            // btnEditReservasi
            // 
            btnEditReservasi.BackColor = Color.DodgerBlue;
            btnEditReservasi.FlatStyle = FlatStyle.Flat;
            btnEditReservasi.ForeColor = Color.White;
            btnEditReservasi.Location = new Point(67, 154);
            btnEditReservasi.Margin = new Padding(4, 5, 4, 5);
            btnEditReservasi.Name = "btnEditReservasi";
            btnEditReservasi.Size = new Size(267, 62);
            btnEditReservasi.TabIndex = 1;
            btnEditReservasi.Text = "Edit Reservasi";
            btnEditReservasi.UseVisualStyleBackColor = false;
            btnEditReservasi.Click += btnEditReservasi_Click;
            // 
            // btnBatalReservasi
            // 
            btnBatalReservasi.BackColor = Color.IndianRed;
            btnBatalReservasi.FlatStyle = FlatStyle.Flat;
            btnBatalReservasi.ForeColor = Color.White;
            btnBatalReservasi.Location = new Point(67, 154);
            btnBatalReservasi.Margin = new Padding(4, 5, 4, 5);
            btnBatalReservasi.Name = "btnBatalReservasi";
            btnBatalReservasi.Size = new Size(267, 62);
            btnBatalReservasi.TabIndex = 2;
            btnBatalReservasi.Text = "Batalkan Reservasi";
            btnBatalReservasi.UseVisualStyleBackColor = false;
            btnBatalReservasi.Click += btnBatalReservasi_Click;
            // 
            // btnCetakLaporan
            // 
            btnCetakLaporan.BackColor = Color.Orange;
            btnCetakLaporan.FlatStyle = FlatStyle.Flat;
            btnCetakLaporan.ForeColor = Color.White;
            btnCetakLaporan.Location = new Point(67, 231);
            btnCetakLaporan.Margin = new Padding(4, 5, 4, 5);
            btnCetakLaporan.Name = "btnCetakLaporan";
            btnCetakLaporan.Size = new Size(267, 62);
            btnCetakLaporan.TabIndex = 3;
            btnCetakLaporan.Text = "Cetak Laporan";
            btnCetakLaporan.UseVisualStyleBackColor = false;
            btnCetakLaporan.Click += btnCetakLaporan_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 462);
            Controls.Add(btnCetakLaporan);
            Controls.Add(btnBatalReservasi);
            Controls.Add(btnEditReservasi);
            Controls.Add(btnReservasiBaru);
            Margin = new Padding(4, 5, 4, 5);
            Name = "MainForm";
            Text = "Sistem Reservasi";
            Load += MainForm_Load;
            ResumeLayout(false);
        }
    }
}