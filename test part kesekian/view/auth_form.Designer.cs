namespace test_part_kesekian
{
    partial class auth_form
    {
        private System.ComponentModel.IContainer components = null;

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
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            username_label = new Label();
            pass_label = new Label();
            LoginTittleLabel = new Label();
            Login_button = new Button();
            SuspendLayout();
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(220, 97);
            txtUsername.Margin = new Padding(2, 2, 2, 2);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(305, 27);
            txtUsername.TabIndex = 0;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(220, 165);
            txtPassword.Margin = new Padding(2, 2, 2, 2);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(305, 27);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // username_label
            // 
            username_label.AutoSize = true;
            username_label.Location = new Point(99, 97);
            username_label.Margin = new Padding(2, 0, 2, 0);
            username_label.Name = "username_label";
            username_label.Size = new Size(118, 20);
            username_label.TabIndex = 2;
            username_label.Text = "Username          :";
            // 
            // pass_label
            // 
            pass_label.AutoSize = true;
            pass_label.Location = new Point(99, 167);
            pass_label.Margin = new Padding(2, 0, 2, 0);
            pass_label.Name = "pass_label";
            pass_label.Size = new Size(117, 20);
            pass_label.TabIndex = 3;
            pass_label.Text = "Password           :";
            // 
            // LoginTittleLabel
            // 
            LoginTittleLabel.AutoSize = true;
            LoginTittleLabel.Location = new Point(286, 29);
            LoginTittleLabel.Margin = new Padding(2, 0, 2, 0);
            LoginTittleLabel.Name = "LoginTittleLabel";
            LoginTittleLabel.Size = new Size(128, 20);
            LoginTittleLabel.TabIndex = 4;
            LoginTittleLabel.Text = "Mbok Wo Reserve";
            // 
            // Login_button
            // 
            Login_button.Location = new Point(220, 224);
            Login_button.Margin = new Padding(2, 2, 2, 2);
            Login_button.Name = "Login_button";
            Login_button.Size = new Size(98, 27);
            Login_button.TabIndex = 5;
            Login_button.Text = "Login";
            Login_button.UseVisualStyleBackColor = true;
            Login_button.Click += btnLogin_Click;
            // 
            // auth_form
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(640, 360);
            Controls.Add(Login_button);
            Controls.Add(LoginTittleLabel);
            Controls.Add(pass_label);
            Controls.Add(username_label);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Margin = new Padding(2, 2, 2, 2);
            Name = "auth_form";
            Text = "Login";
            Load += auth_form_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private TextBox txtUsername;
        private TextBox txtPassword;
        private Label username_label;
        private Label pass_label;
        private Label LoginTittleLabel;
        private Button Login_button;
    }
}