namespace DatabaseConnectionTask
{
    partial class DBConnection
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Title = new Label();
            Server = new Label();
            DatabaseName = new Label();
            LoginId = new Label();
            Password = new Label();
            ServerName = new TextBox();
            DBName = new TextBox();
            DBLoginID = new TextBox();
            DBPassword = new TextBox();
            submit = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // Title
            // 
            Title.AutoSize = true;
            Title.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            Title.Location = new Point(218, 42);
            Title.Name = "Title";
            Title.Size = new Size(386, 37);
            Title.TabIndex = 0;
            Title.Text = "Database Connection Details";
            Title.Click += Title_Click;
            // 
            // Server
            // 
            Server.AutoSize = true;
            Server.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Server.Location = new Point(263, 149);
            Server.Name = "Server";
            Server.Size = new Size(94, 20);
            Server.TabIndex = 1;
            Server.Text = "Server Name";
            // 
            // DatabaseName
            // 
            DatabaseName.AutoSize = true;
            DatabaseName.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            DatabaseName.Location = new Point(241, 353);
            DatabaseName.Name = "DatabaseName";
            DatabaseName.Size = new Size(116, 20);
            DatabaseName.TabIndex = 2;
            DatabaseName.Text = "Database Name";
            // 
            // LoginId
            // 
            LoginId.AutoSize = true;
            LoginId.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LoginId.Location = new Point(225, 249);
            LoginId.Name = "LoginId";
            LoginId.Size = new Size(132, 20);
            LoginId.TabIndex = 4;
            LoginId.Text = "Database Login ID";
            // 
            // Password
            // 
            Password.AutoSize = true;
            Password.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Password.Location = new Point(228, 302);
            Password.Name = "Password";
            Password.Size = new Size(129, 20);
            Password.TabIndex = 5;
            Password.Text = "Databse Password";
            // 
            // ServerName
            // 
            ServerName.Location = new Point(363, 146);
            ServerName.Name = "ServerName";
            ServerName.Size = new Size(192, 23);
            ServerName.TabIndex = 6;
            // 
            // DBName
            // 
            DBName.Location = new Point(363, 350);
            DBName.Name = "DBName";
            DBName.Size = new Size(192, 23);
            DBName.TabIndex = 7;
            // 
            // DBLoginID
            // 
            DBLoginID.Location = new Point(363, 250);
            DBLoginID.Name = "DBLoginID";
            DBLoginID.Size = new Size(192, 23);
            DBLoginID.TabIndex = 9;
            // 
            // DBPassword
            // 
            DBPassword.Location = new Point(363, 302);
            DBPassword.Name = "DBPassword";
            DBPassword.Size = new Size(192, 23);
            DBPassword.TabIndex = 10;
            // 
            // submit
            // 
            submit.BackColor = SystemColors.ScrollBar;
            submit.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            submit.Location = new Point(228, 414);
            submit.Name = "submit";
            submit.Size = new Size(327, 38);
            submit.TabIndex = 11;
            submit.Text = "Connect to server ";
            submit.UseVisualStyleBackColor = false;
            submit.Click += submit_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(228, 190);
            label1.Name = "label1";
            label1.Size = new Size(328, 30);
            label1.TabIndex = 12;
            label1.Text = "DataBase Authentication Details";
            // 
            // DBConnection
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 512);
            Controls.Add(label1);
            Controls.Add(submit);
            Controls.Add(DBPassword);
            Controls.Add(DBLoginID);
            Controls.Add(DBName);
            Controls.Add(ServerName);
            Controls.Add(Password);
            Controls.Add(LoginId);
            Controls.Add(DatabaseName);
            Controls.Add(Server);
            Controls.Add(Title);
            Name = "DBConnection";
            Text = "DatabaseConnectionForm";
            Load += DBConnection_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Title;
        private Label Server;
        private Label DatabaseName;
        private Label LoginId;
        private Label Password;
        private TextBox ServerName;
        private TextBox DBName;
        private TextBox DBLoginID;
        private TextBox DBPassword;
        private Button submit;
        private Label label1;
    }
}
