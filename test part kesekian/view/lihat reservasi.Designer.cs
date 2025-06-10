namespace test_part_kesekian.view
{
    partial class lihat_reservasi
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            gridAllReservasi = new DataGridView();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)gridAllReservasi).BeginInit();
            SuspendLayout();
            // 
            // gridAllReservasi
            // 
            gridAllReservasi.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridAllReservasi.Location = new Point(0, 81);
            gridAllReservasi.Name = "gridAllReservasi";
            gridAllReservasi.RowHeadersWidth = 51;
            gridAllReservasi.Size = new Size(801, 357);
            gridAllReservasi.TabIndex = 0;
            gridAllReservasi.CellContentClick += gridAllReservasi_CellContentClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(291, 33);
            label1.Name = "label1";
            label1.Size = new Size(205, 20);
            label1.TabIndex = 1;
            label1.Text = "DAFTAR RESERVASI MBO WO";
            label1.Click += label1_Click;
            // 
            // lihat_reservasi
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(gridAllReservasi);
            Name = "lihat_reservasi";
            Text = "lihat_reservasi";
            ((System.ComponentModel.ISupportInitialize)gridAllReservasi).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView gridAllReservasi;
        private Label label1;
    }
}