namespace test_part_kesekian
{
    partial class FormReservasiBaru
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTanggal;
        private System.Windows.Forms.DateTimePicker dtpTanggal;
        private System.Windows.Forms.Label lblWaktu;
        private System.Windows.Forms.DateTimePicker dtpWaktu;
        private System.Windows.Forms.Label lblJumlahOrang;
        private System.Windows.Forms.TextBox tbJumlahOrang;
        private System.Windows.Forms.Button btnSimpan;

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
            lblTanggal = new Label();
            dtpTanggal = new DateTimePicker();
            lblWaktu = new Label();
            dtpWaktu = new DateTimePicker();
            lblJumlahOrang = new Label();
            tbJumlahOrang = new TextBox();
            btnSimpan = new Button();
            tbNomorHP = new TextBox();
            lblNomorHP = new Label();
            SuspendLayout();
            // 
            // lblTanggal
            // 
            lblTanggal.AutoSize = true;
            lblTanggal.Location = new Point(27, 77);
            lblTanggal.Margin = new Padding(4, 0, 4, 0);
            lblTanggal.Name = "lblTanggal";
            lblTanggal.Size = new Size(64, 20);
            lblTanggal.TabIndex = 4;
            lblTanggal.Text = "Tanggal:";
            // 
            // dtpTanggal
            // 
            dtpTanggal.Format = DateTimePickerFormat.Short;
            dtpTanggal.Location = new Point(160, 77);
            dtpTanggal.Margin = new Padding(4, 5, 4, 5);
            dtpTanggal.Name = "dtpTanggal";
            dtpTanggal.Size = new Size(132, 27);
            dtpTanggal.TabIndex = 5;
            // 
            // lblWaktu
            // 
            lblWaktu.AutoSize = true;
            lblWaktu.Location = new Point(27, 123);
            lblWaktu.Margin = new Padding(4, 0, 4, 0);
            lblWaktu.Name = "lblWaktu";
            lblWaktu.Size = new Size(53, 20);
            lblWaktu.TabIndex = 6;
            lblWaktu.Text = "Waktu:";
            // 
            // dtpWaktu
            // 
            dtpWaktu.Format = DateTimePickerFormat.Time;
            dtpWaktu.Location = new Point(160, 123);
            dtpWaktu.Margin = new Padding(4, 5, 4, 5);
            dtpWaktu.Name = "dtpWaktu";
            dtpWaktu.ShowUpDown = true;
            dtpWaktu.Size = new Size(132, 27);
            dtpWaktu.TabIndex = 7;
            // 
            // lblJumlahOrang
            // 
            lblJumlahOrang.AutoSize = true;
            lblJumlahOrang.Location = new Point(27, 169);
            lblJumlahOrang.Margin = new Padding(4, 0, 4, 0);
            lblJumlahOrang.Name = "lblJumlahOrang";
            lblJumlahOrang.Size = new Size(103, 20);
            lblJumlahOrang.TabIndex = 8;
            lblJumlahOrang.Text = "Jumlah Orang:";
            // 
            // tbJumlahOrang
            // 
            tbJumlahOrang.Location = new Point(160, 169);
            tbJumlahOrang.Margin = new Padding(4, 5, 4, 5);
            tbJumlahOrang.Name = "tbJumlahOrang";
            tbJumlahOrang.Size = new Size(65, 27);
            tbJumlahOrang.TabIndex = 9;
            // 
            // btnSimpan
            // 
            btnSimpan.BackColor = Color.MediumSeaGreen;
            btnSimpan.FlatStyle = FlatStyle.Flat;
            btnSimpan.ForeColor = Color.White;
            btnSimpan.Location = new Point(160, 215);
            btnSimpan.Margin = new Padding(4, 5, 4, 5);
            btnSimpan.Name = "btnSimpan";
            btnSimpan.Size = new Size(107, 46);
            btnSimpan.TabIndex = 10;
            btnSimpan.Text = "Simpan";
            btnSimpan.UseVisualStyleBackColor = false;
            btnSimpan.Click += btnSimpan_Click;
            // 
            // tbNomorHP
            // 
            tbNomorHP.Location = new Point(160, 31);
            tbNomorHP.Margin = new Padding(4, 5, 4, 5);
            tbNomorHP.Name = "tbNomorHP";
            tbNomorHP.Size = new Size(265, 27);
            tbNomorHP.TabIndex = 3;
            // 
            // lblNomorHP
            // 
            lblNomorHP.AutoSize = true;
            lblNomorHP.Location = new Point(27, 31);
            lblNomorHP.Margin = new Padding(4, 0, 4, 0);
            lblNomorHP.Name = "lblNomorHP";
            lblNomorHP.Size = new Size(82, 20);
            lblNomorHP.TabIndex = 2;
            lblNomorHP.Text = "Nomor HP:";
            // 
            // FormReservasiBaru
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(811, 502);
            ControlBox = false;
            Controls.Add(btnSimpan);
            Controls.Add(tbJumlahOrang);
            Controls.Add(lblJumlahOrang);
            Controls.Add(dtpWaktu);
            Controls.Add(lblWaktu);
            Controls.Add(dtpTanggal);
            Controls.Add(lblTanggal);
            Controls.Add(tbNomorHP);
            Controls.Add(lblNomorHP);
            Margin = new Padding(4, 5, 4, 5);
            Name = "FormReservasiBaru";
            Text = "Reservasi Baru";
            ResumeLayout(false);
            PerformLayout();
        }
        private TextBox tbNomorHP;
        private Label lblNomorHP;
    }
}