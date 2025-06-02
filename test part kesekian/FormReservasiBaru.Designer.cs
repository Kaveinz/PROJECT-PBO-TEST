namespace test_part_kesekian
{
    partial class FormReservasiBaru
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblNomorHP;
        private System.Windows.Forms.TextBox tbNomorHP;
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
            this.lblNomorHP = new System.Windows.Forms.Label();
            this.tbNomorHP = new System.Windows.Forms.TextBox();
            this.lblTanggal = new System.Windows.Forms.Label();
            this.dtpTanggal = new System.Windows.Forms.DateTimePicker();
            this.lblWaktu = new System.Windows.Forms.Label();
            this.dtpWaktu = new System.Windows.Forms.DateTimePicker();
            this.lblJumlahOrang = new System.Windows.Forms.Label();
            this.tbJumlahOrang = new System.Windows.Forms.TextBox();
            this.btnSimpan = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblNomorHP
            this.lblNomorHP.AutoSize = true;
            this.lblNomorHP.Location = new System.Drawing.Point(20, 20);
            this.lblNomorHP.Name = "lblNomorHP";
            this.lblNomorHP.Size = new System.Drawing.Size(60, 13);
            this.lblNomorHP.TabIndex = 2;
            this.lblNomorHP.Text = "Nomor HP:";

            // tbNomorHP
            this.tbNomorHP.Location = new System.Drawing.Point(120, 20);
            this.tbNomorHP.Name = "tbNomorHP";
            this.tbNomorHP.Size = new System.Drawing.Size(200, 20);
            this.tbNomorHP.TabIndex = 3;

            // lblTanggal
            this.lblTanggal.AutoSize = true;
            this.lblTanggal.Location = new System.Drawing.Point(20, 50);
            this.lblTanggal.Name = "lblTanggal";
            this.lblTanggal.Size = new System.Drawing.Size(50, 13);
            this.lblTanggal.TabIndex = 4;
            this.lblTanggal.Text = "Tanggal:";

            // dtpTanggal
            this.dtpTanggal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTanggal.Location = new System.Drawing.Point(120, 50);
            this.dtpTanggal.Name = "dtpTanggal";
            this.dtpTanggal.Size = new System.Drawing.Size(100, 20);
            this.dtpTanggal.TabIndex = 5;

            // lblWaktu
            this.lblWaktu.AutoSize = true;
            this.lblWaktu.Location = new System.Drawing.Point(20, 80);
            this.lblWaktu.Name = "lblWaktu";
            this.lblWaktu.Size = new System.Drawing.Size(40, 13);
            this.lblWaktu.TabIndex = 6;
            this.lblWaktu.Text = "Waktu:";

            // dtpWaktu
            this.dtpWaktu.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpWaktu.Location = new System.Drawing.Point(120, 80);
            this.dtpWaktu.Name = "dtpWaktu";
            this.dtpWaktu.ShowUpDown = true;
            this.dtpWaktu.Size = new System.Drawing.Size(100, 20);
            this.dtpWaktu.TabIndex = 7;

            // lblJumlahOrang
            this.lblJumlahOrang.AutoSize = true;
            this.lblJumlahOrang.Location = new System.Drawing.Point(20, 110);
            this.lblJumlahOrang.Name = "lblJumlahOrang";
            this.lblJumlahOrang.Size = new System.Drawing.Size(80, 13);
            this.lblJumlahOrang.TabIndex = 8;
            this.lblJumlahOrang.Text = "Jumlah Orang:";

            // tbJumlahOrang
            this.tbJumlahOrang.Location = new System.Drawing.Point(120, 110);
            this.tbJumlahOrang.Name = "tbJumlahOrang";
            this.tbJumlahOrang.Size = new System.Drawing.Size(50, 20);
            this.tbJumlahOrang.TabIndex = 9;

            // btnSimpan
            this.btnSimpan.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnSimpan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimpan.ForeColor = System.Drawing.Color.White;
            this.btnSimpan.Location = new System.Drawing.Point(120, 140);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(80, 30);
            this.btnSimpan.TabIndex = 10;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.UseVisualStyleBackColor = false;
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);

            // FormReservasiBaru
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 200);
            this.Controls.Add(this.btnSimpan);
            this.Controls.Add(this.tbJumlahOrang);
            this.Controls.Add(this.lblJumlahOrang);
            this.Controls.Add(this.dtpWaktu);
            this.Controls.Add(this.lblWaktu);
            this.Controls.Add(this.dtpTanggal);
            this.Controls.Add(this.lblTanggal);
            this.Controls.Add(this.tbNomorHP);
            this.Controls.Add(this.lblNomorHP);
            this.Name = "FormReservasiBaru";
            this.Text = "Reservasi Baru";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}