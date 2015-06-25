namespace CreateXML
{
    partial class AddSalaryKey
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textCode = new System.Windows.Forms.TextBox();
            this.CB_SalaryKeyBigType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textEnName = new System.Windows.Forms.TextBox();
            this.textdescription = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.comboKeyType = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textCode);
            this.groupBox1.Controls.Add(this.comboKeyType);
            this.groupBox1.Controls.Add(this.CB_SalaryKeyBigType);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textEnName);
            this.groupBox1.Controls.Add(this.textdescription);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(796, 355);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "薪資計算";
            // 
            // textCode
            // 
            this.textCode.Location = new System.Drawing.Point(493, 60);
            this.textCode.Name = "textCode";
            this.textCode.Size = new System.Drawing.Size(215, 22);
            this.textCode.TabIndex = 27;
            // 
            // CB_SalaryKeyBigType
            // 
            this.CB_SalaryKeyBigType.FormattingEnabled = true;
            this.CB_SalaryKeyBigType.Items.AddRange(new object[] {
            "人事資訊",
            "薪資福利類",
            "考勤信息",
            "績效資訊",
            "獎懲資訊",
            "其它資訊"});
            this.CB_SalaryKeyBigType.Location = new System.Drawing.Point(123, 63);
            this.CB_SalaryKeyBigType.Name = "CB_SalaryKeyBigType";
            this.CB_SalaryKeyBigType.Size = new System.Drawing.Size(179, 20);
            this.CB_SalaryKeyBigType.TabIndex = 26;
            this.CB_SalaryKeyBigType.SelectedIndexChanged += new System.EventHandler(this.CB_SalaryKeyBigType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(27, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "SalaryBigType：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(435, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "Code：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(420, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "EnName：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(62, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "ScName：";
            // 
            // textEnName
            // 
            this.textEnName.Location = new System.Drawing.Point(493, 21);
            this.textEnName.Name = "textEnName";
            this.textEnName.Size = new System.Drawing.Size(215, 22);
            this.textEnName.TabIndex = 1;
            // 
            // textdescription
            // 
            this.textdescription.Location = new System.Drawing.Point(123, 21);
            this.textdescription.Name = "textdescription";
            this.textdescription.Size = new System.Drawing.Size(179, 22);
            this.textdescription.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(595, 436);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 25);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(718, 436);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 25);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(26, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "SalaryKeyType：";
            // 
            // comboKeyType
            // 
            this.comboKeyType.FormattingEnabled = true;
            this.comboKeyType.Location = new System.Drawing.Point(124, 96);
            this.comboKeyType.Name = "comboKeyType";
            this.comboKeyType.Size = new System.Drawing.Size(179, 20);
            this.comboKeyType.TabIndex = 26;
            // 
            // AddSalaryKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 486);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Name = "AddSalaryKey";
            this.Text = "新增薪資計算";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textEnName;
        private System.Windows.Forms.TextBox textdescription;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox CB_SalaryKeyBigType;
        private System.Windows.Forms.TextBox textCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboKeyType;
        private System.Windows.Forms.Label label7;
    }
}