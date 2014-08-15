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
            this.cob_Path.Location = new System.Drawing.Point(25, 35);
            this.cob_Path.Name = "cob_Path";
            this.cob_Path.Size = new System.Drawing.Size(318, 20);
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
            // PathForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 170);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.cob_Path);
            this.Controls.Add(this.button1);
            this.Name = "PathForm";
            this.Text = "設定路徑";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cob_Path;
        private System.Windows.Forms.Button button2;
    }
}