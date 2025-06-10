namespace test_part_kesekian
{
    partial class FormBatalReservasi
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dgvReservations;
        private System.Windows.Forms.Button btnBatal;

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
            dgvReservations = new DataGridView();
            btnBatal = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvReservations).BeginInit();
            SuspendLayout();
            // 
            // dgvReservations
            // 
            dgvReservations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReservations.Location = new Point(27, 31);
            dgvReservations.Margin = new Padding(4, 5, 4, 5);
            dgvReservations.Name = "dgvReservations";
            dgvReservations.ReadOnly = true;
            dgvReservations.RowHeadersWidth = 51;
            dgvReservations.Size = new Size(800, 308);
            dgvReservations.TabIndex = 0;
            // 
            // btnBatal
            // 
            btnBatal.BackColor = Color.IndianRed;
            btnBatal.FlatStyle = FlatStyle.Flat;
            btnBatal.ForeColor = Color.White;
            btnBatal.Location = new Point(27, 354);
            btnBatal.Margin = new Padding(4, 5, 4, 5);
            btnBatal.Name = "btnBatal";
            btnBatal.Size = new Size(107, 46);
            btnBatal.TabIndex = 1;
            btnBatal.Text = "Batalkan";
            btnBatal.UseVisualStyleBackColor = false;
            btnBatal.Click += btnBatal_Click;
            // 
            // FormBatalReservasi
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(867, 462);
            Controls.Add(btnBatal);
            Controls.Add(dgvReservations);
            Margin = new Padding(4, 5, 4, 5);
            Name = "FormBatalReservasi";
            Text = "Batalkan Reservasi";
            Load += FormBatalReservasi_Load;
            ((System.ComponentModel.ISupportInitialize)dgvReservations).EndInit();
            ResumeLayout(false);
        }
    }
}