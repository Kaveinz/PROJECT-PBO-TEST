namespace test_part_kesekian.view
{
    partial class form_register
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
            Register_button = new Button();
            LoginTittleLabel = new Label();
            pass_label = new Label();
            username_label = new Label();
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            labelNama_lengkap = new Label();
            labelEmail = new Label();
            labellNoHp = new Label();
            labelAlamat = new Label();
            txtNama_lengkap = new TextBox();
            txtEmail = new TextBox();
            txtNoHp = new TextBox();
            txtAlamat = new TextBox();
            SuspendLayout();
            // 
            // Register_button
            // 
            Register_button.Location = new Point(37, 412);
            Register_button.Margin = new Padding(2);
            Register_button.Name = "Register_button";
            Register_button.Size = new Size(98, 27);
            Register_button.TabIndex = 11;
            Register_button.Text = "Simpan";
            Register_button.UseVisualStyleBackColor = true;
            Register_button.Click += Login_button_Click;
            // 
            // LoginTittleLabel
            // 
            LoginTittleLabel.AutoSize = true;
            LoginTittleLabel.Location = new Point(337, 37);
            LoginTittleLabel.Margin = new Padding(2, 0, 2, 0);
            LoginTittleLabel.Name = "LoginTittleLabel";
            LoginTittleLabel.Size = new Size(128, 20);
            LoginTittleLabel.TabIndex = 10;
            LoginTittleLabel.Text = "Mbok Wo Reserve";
            // 
            // pass_label
            // 
            pass_label.AutoSize = true;
            pass_label.Location = new Point(37, 212);
            pass_label.Margin = new Padding(2, 0, 2, 0);
            pass_label.Name = "pass_label";
            pass_label.Size = new Size(70, 20);
            pass_label.TabIndex = 9;
            pass_label.Text = "Password";
            // 
            // username_label
            // 
            username_label.AutoSize = true;
            username_label.Location = new Point(37, 121);
            username_label.Margin = new Padding(2, 0, 2, 0);
            username_label.Name = "username_label";
            username_label.Size = new Size(75, 20);
            username_label.TabIndex = 8;
            username_label.Text = "Username";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(37, 251);
            txtPassword.Margin = new Padding(2);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(305, 27);
            txtPassword.TabIndex = 7;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(37, 155);
            txtUsername.Margin = new Padding(2);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(305, 27);
            txtUsername.TabIndex = 6;
            txtUsername.TextChanged += txtUsername_TextChanged;
            // 
            // labelNama_lengkap
            // 
            labelNama_lengkap.AutoSize = true;
            labelNama_lengkap.Location = new Point(37, 296);
            labelNama_lengkap.Name = "labelNama_lengkap";
            labelNama_lengkap.Size = new Size(109, 20);
            labelNama_lengkap.TabIndex = 12;
            labelNama_lengkap.Text = "Nama Lengkap";
            labelNama_lengkap.Click += label1_Click;
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Location = new Point(439, 121);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(46, 20);
            labelEmail.TabIndex = 13;
            labelEmail.Text = "Email";
            // 
            // labellNoHp
            // 
            labellNoHp.AutoSize = true;
            labellNoHp.Location = new Point(439, 212);
            labellNoHp.Name = "labellNoHp";
            labellNoHp.Size = new Size(79, 20);
            labellNoHp.TabIndex = 14;
            labellNoHp.Text = "Nomor HP";
            // 
            // labelAlamat
            // 
            labelAlamat.AutoSize = true;
            labelAlamat.Location = new Point(439, 296);
            labelAlamat.Name = "labelAlamat";
            labelAlamat.Size = new Size(57, 20);
            labelAlamat.TabIndex = 15;
            labelAlamat.Text = "Alamat";
            // 
            // txtNama_lengkap
            // 
            txtNama_lengkap.Location = new Point(37, 330);
            txtNama_lengkap.Name = "txtNama_lengkap";
            txtNama_lengkap.Size = new Size(305, 27);
            txtNama_lengkap.TabIndex = 16;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(439, 155);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(302, 27);
            txtEmail.TabIndex = 17;
            // 
            // txtNoHp
            // 
            txtNoHp.Location = new Point(448, 251);
            txtNoHp.Name = "txtNoHp";
            txtNoHp.Size = new Size(293, 27);
            txtNoHp.TabIndex = 18;
            // 
            // txtAlamat
            // 
            txtAlamat.Location = new Point(448, 330);
            txtAlamat.Name = "txtAlamat";
            txtAlamat.Size = new Size(293, 27);
            txtAlamat.TabIndex = 19;
            // 
            // form_register
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtAlamat);
            Controls.Add(txtNoHp);
            Controls.Add(txtEmail);
            Controls.Add(txtNama_lengkap);
            Controls.Add(labelAlamat);
            Controls.Add(labellNoHp);
            Controls.Add(labelEmail);
            Controls.Add(labelNama_lengkap);
            Controls.Add(Register_button);
            Controls.Add(LoginTittleLabel);
            Controls.Add(pass_label);
            Controls.Add(username_label);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Name = "form_register";
            Text = "form_register";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Register_button;
        private Label LoginTittleLabel;
        private Label pass_label;
        private Label username_label;
        private TextBox txtPassword;
        private TextBox txtUsername;
        private Label labelNama_lengkap;
        private Label labelEmail;
        private Label labellNoHp;
        private Label labelAlamat;
        private TextBox txtNama_lengkap;
        private TextBox txtEmail;
        private TextBox txtNoHp;
        private TextBox txtAlamat;
    }
}