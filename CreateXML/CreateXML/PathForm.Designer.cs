namespace CreateXML {
    partial class PathForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.button1 = new System.Windows.Forms.Button();
            this.cob_Path = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TB_DBIP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_DBName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.Location = new System.Drawing.Point(349, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 32);
            this.button1.TabIndex = 1;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cob_Path
            // 
            this.cob_Path.FormattingEnabled = true;
            this.cob_Path.Location = new System.Drawing.Point(85, 35);
            this.cob_Path.Name = "cob_Path";
            this.cob_Path.Size = new System.Drawing.Size(258, 20);
            this.cob_Path.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(410, 33);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(39, 32);
            this.button2.TabIndex = 3;
            this.button2.Text = "Set";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "資料庫IP：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "個案路徑：";
            // 
            // TB_DBIP
            // 
            this.TB_DBIP.Enabled = false;
            this.TB_DBIP.Location = new System.Drawing.Point(85, 109);
            this.TB_DBIP.Name = "TB_DBIP";
            this.TB_DBIP.Size = new System.Drawing.Size(258, 22);
            this.TB_DBIP.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 179);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "資料庫名稱：";
            // 
            // tb_DBName
            // 
            this.tb_DBName.Enabled = false;
            this.tb_DBName.Location = new System.Drawing.Point(85, 169);
            this.tb_DBName.Name = "tb_DBName";
            this.tb_DBName.Size = new System.Drawing.Size(258, 22);
            this.tb_DBName.TabIndex = 7;
            // 
            // PathForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 256);
            this.Controls.Add(this.tb_DBName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TB_DBIP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.cob_Path);
            this.Controls.Add(this.button1);
            this.Name = "PathForm";
            this.Text = "設定路徑";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cob_Path;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TB_DBIP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_DBName;
    }
}