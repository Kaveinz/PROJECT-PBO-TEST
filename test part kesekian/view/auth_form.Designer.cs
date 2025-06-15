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
            label1 = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(433, 168);
            txtUsername.Margin = new Padding(2);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(305, 27);
            txtUsername.TabIndex = 0;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(434, 249);
            txtPassword.Margin = new Padding(2);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(305, 27);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // username_label
            // 
            username_label.AutoSize = true;
            username_label.Location = new Point(433, 126);
            username_label.Margin = new Padding(2, 0, 2, 0);
            username_label.Name = "username_label";
            username_label.Size = new Size(118, 20);
            username_label.TabIndex = 2;
            username_label.Text = "Username          :";
            // 
            // pass_label
            // 
            pass_label.AutoSize = true;
            pass_label.Location = new Point(434, 213);
            pass_label.Margin = new Padding(2, 0, 2, 0);
            pass_label.Name = "pass_label";
            pass_label.Size = new Size(117, 20);
            pass_label.TabIndex = 3;
            pass_label.Text = "Password           :";
            // 
            // LoginTittleLabel
            // 
            LoginTittleLabel.AutoSize = true;
            LoginTittleLabel.Location = new Point(523, 79);
            LoginTittleLabel.Margin = new Padding(2, 0, 2, 0);
            LoginTittleLabel.Name = "LoginTittleLabel";
            LoginTittleLabel.Size = new Size(128, 20);
            LoginTittleLabel.TabIndex = 4;
            LoginTittleLabel.Text = "Mbok Wo Reserve";
            // 
            // Login_button
            // 
            Login_button.Location = new Point(433, 308);
            Login_button.Margin = new Padding(2);
            Login_button.Name = "Login_button";
            Login_button.Size = new Size(98, 27);
            Login_button.TabIndex = 5;
            Login_button.Text = "Login";
            Login_button.UseVisualStyleBackColor = true;
            Login_button.Click += btnLogin_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(434, 358);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(165, 20);
            label1.TabIndex = 6;
            label1.Text = "don't have account yet?";
            label1.Click += label1_Click;
            // 
            // button1
            // 
            button1.Location = new Point(619, 358);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(98, 27);
            button1.TabIndex = 7;
            button1.Text = "Register";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // auth_form
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Firebrick;
            ClientSize = new Size(1058, 567);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(Login_button);
            Controls.Add(LoginTittleLabel);
            Controls.Add(pass_label);
            Controls.Add(username_label);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Margin = new Padding(2);
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
        private Label label1;
        private Button button1;
    }
}