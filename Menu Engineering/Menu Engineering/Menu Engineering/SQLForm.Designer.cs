namespace Menu_Engineering
{
    partial class SQLForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.tbPw = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDBName = new System.Windows.Forms.TextBox();
            this.btConnection = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.bt_Save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "使用者名稱";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "使用者密碼";
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(134, 35);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(255, 25);
            this.tbUser.TabIndex = 1;
            // 
            // tbPw
            // 
            this.tbPw.Location = new System.Drawing.Point(134, 90);
            this.tbPw.Name = "tbPw";
            this.tbPw.PasswordChar = '*';
            this.tbPw.Size = new System.Drawing.Size(255, 25);
            this.tbPw.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(69, 183);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "DBName";
            // 
            // tbDBName
            // 
            this.tbDBName.Location = new System.Drawing.Point(134, 180);
            this.tbDBName.Name = "tbDBName";
            this.tbDBName.Size = new System.Drawing.Size(255, 25);
            this.tbDBName.TabIndex = 1;
            // 
            // btConnection
            // 
            this.btConnection.Location = new System.Drawing.Point(148, 228);
            this.btConnection.Name = "btConnection";
            this.btConnection.Size = new System.Drawing.Size(103, 23);
            this.btConnection.TabIndex = 2;
            this.btConnection.Text = "連線測試";
            this.btConnection.UseVisualStyleBackColor = true;
            this.btConnection.Click += new System.EventHandler(this.btConnection_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(107, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "IP";
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(134, 130);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(255, 25);
            this.tbIP.TabIndex = 1;
            // 
            // bt_Save
            // 
            this.bt_Save.Location = new System.Drawing.Point(295, 228);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(75, 23);
            this.bt_Save.TabIndex = 3;
            this.bt_Save.Text = "存檔";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // SQLConntion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 272);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.btConnection);
            this.Controls.Add(this.tbPw);
            this.Controls.Add(this.tbDBName);
            this.Controls.Add(this.tbIP);
            this.Controls.Add(this.tbUser);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SQLConntion";
            this.Text = "SQLConntion";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.TextBox tbPw;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDBName;
        private System.Windows.Forms.Button btConnection;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.Button bt_Save;
    }
}