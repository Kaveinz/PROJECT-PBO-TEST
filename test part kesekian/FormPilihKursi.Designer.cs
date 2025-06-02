namespace test_part_kesekian
{
    partial class FormPilihKursi
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button meja7;
        private System.Windows.Forms.Button meja6;
        private System.Windows.Forms.Button meja5;
        private System.Windows.Forms.Button meja4;
        private System.Windows.Forms.Button meja3;
        private System.Windows.Forms.Button meja2;
        private System.Windows.Forms.Button meja1;
        private System.Windows.Forms.Button btnKonfirmasi;

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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.meja7 = new System.Windows.Forms.Button();
            this.meja6 = new System.Windows.Forms.Button();
            this.meja5 = new System.Windows.Forms.Button();
            this.meja4 = new System.Windows.Forms.Button();
            this.meja3 = new System.Windows.Forms.Button();
            this.meja2 = new System.Windows.Forms.Button();
            this.meja1 = new System.Windows.Forms.Button();
            this.btnKonfirmasi = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();

            // groupBox1
            this.groupBox1.Controls.Add(this.meja7);
            this.groupBox1.Controls.Add(this.meja6);
            this.groupBox1.Controls.Add(this.meja5);
            this.groupBox1.Controls.Add(this.meja4);
            this.groupBox1.Controls.Add(this.meja3);
            this.groupBox1.Controls.Add(this.meja2);
            this.groupBox1.Controls.Add(this.meja1);
            this.groupBox1.Location = new System.Drawing.Point(20, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 200);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pilih Kursi";
            this.groupBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox1_Paint);

            // meja7
            this.meja7.Location = new System.Drawing.Point(240, 150);
            this.meja7.Name = "meja7";
            this.meja7.Size = new System.Drawing.Size(50, 40);
            this.meja7.TabIndex = 6;
            this.meja7.Text = "A7";
            this.meja7.UseVisualStyleBackColor = true;

            // meja6
            this.meja6.Location = new System.Drawing.Point(180, 150);
            this.meja6.Name = "meja6";
            this.meja6.Size = new System.Drawing.Size(50, 40);
            this.meja6.TabIndex = 5;
            this.meja6.Text = "A6";
            this.meja6.UseVisualStyleBackColor = true;

            // meja5
            this.meja5.Location = new System.Drawing.Point(120, 150);
            this.meja5.Name = "meja5";
            this.meja5.Size = new System.Drawing.Size(50, 40);
            this.meja5.TabIndex = 4;
            this.meja5.Text = "A5";
            this.meja5.UseVisualStyleBackColor = true;

            // meja4
            this.meja4.Location = new System.Drawing.Point(240, 100);
            this.meja4.Name = "meja4";
            this.meja4.Size = new System.Drawing.Size(50, 40);
            this.meja4.TabIndex = 3;
            this.meja4.Text = "A4";
            this.meja4.UseVisualStyleBackColor = true;

            // meja3
            this.meja3.Location = new System.Drawing.Point(180, 100);
            this.meja3.Name = "meja3";
            this.meja3.Size = new System.Drawing.Size(50, 40);
            this.meja3.TabIndex = 2;
            this.meja3.Text = "A3";
            this.meja3.UseVisualStyleBackColor = true;

            // meja2
            this.meja2.Location = new System.Drawing.Point(120, 100);
            this.meja2.Name = "meja2";
            this.meja2.Size = new System.Drawing.Size(50, 40);
            this.meja2.TabIndex = 1;
            this.meja2.Text = "A2";
            this.meja2.UseVisualStyleBackColor = true;

            // meja1
            this.meja1.Location = new System.Drawing.Point(120, 50);
            this.meja1.Name = "meja1";
            this.meja1.Size = new System.Drawing.Size(50, 40);
            this.meja1.TabIndex = 0;
            this.meja1.Text = "A1";
            this.meja1.UseVisualStyleBackColor = true;

            // btnKonfirmasi
            this.btnKonfirmasi.Location = new System.Drawing.Point(130, 230);
            this.btnKonfirmasi.Name = "btnKonfirmasi";
            this.btnKonfirmasi.Size = new System.Drawing.Size(100, 30);
            this.btnKonfirmasi.TabIndex = 1;
            this.btnKonfirmasi.Text = "Konfirmasi";
            this.btnKonfirmasi.UseVisualStyleBackColor = true;
            this.btnKonfirmasi.Click += new System.EventHandler(this.btnKonfirmasi_Click);

            // FormPilihKursi
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 300);
            this.Controls.Add(this.btnKonfirmasi);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormPilihKursi";
            this.Text = "Pilih Kursi";
            this.Load += new System.EventHandler(this.FormPilihKursi_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = (GroupBox)sender;
            e.Graphics.Clear(SystemColors.Control);
            ControlPaint.DrawBorder(e.Graphics, box.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
            SizeF stringSize = e.Graphics.MeasureString(box.Text, box.Font);
            float textX = (box.Width - stringSize.Width) / 2;
            e.Graphics.DrawString(box.Text, box.Font, Brushes.Black, textX, 0);
        }
    }
}