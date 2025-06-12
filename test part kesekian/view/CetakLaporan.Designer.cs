namespace test_part_kesekian.view
{
    partial class CetakLaporan
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
            dvgLaporan = new DataGridView();
            btnExport = new Button();
            btnPrint = new Button();
            ((System.ComponentModel.ISupportInitialize)dvgLaporan).BeginInit();
            SuspendLayout();
            // 
            // dvgLaporan
            // 
            dvgLaporan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dvgLaporan.Location = new Point(12, 12);
            dvgLaporan.Name = "dvgLaporan";
            dvgLaporan.RowHeadersWidth = 51;
            dvgLaporan.Size = new Size(794, 217);
            dvgLaporan.TabIndex = 0;
            dvgLaporan.CellContentClick += dataGridView1_CellContentClick;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(38, 272);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(94, 29);
            btnExport.TabIndex = 1;
            btnExport.Text = "Export";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // btnPrint
            // 
            btnPrint.Location = new Point(156, 272);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(94, 29);
            btnPrint.TabIndex = 2;
            btnPrint.Text = "Preview";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Click += btnPrint_Click;
            // 
            // CetakLaporan
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(818, 450);
            Controls.Add(btnPrint);
            Controls.Add(btnExport);
            Controls.Add(dvgLaporan);
            Name = "CetakLaporan";
            Text = "CetakLaporan";
            ((System.ComponentModel.ISupportInitialize)dvgLaporan).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dvgLaporan;
        private Button btnExport;
        private Button btnPrint;
    }
}