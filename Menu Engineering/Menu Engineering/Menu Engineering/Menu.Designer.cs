namespace Menu_Engineering
{
    partial class Menu
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.新增食材ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新增食材類別ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新增食材ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.資料庫連線ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新增食材ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1003, 27);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 新增食材ToolStripMenuItem
            // 
            this.新增食材ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新增食材類別ToolStripMenuItem,
            this.新增食材ToolStripMenuItem1,
            this.資料庫連線ToolStripMenuItem});
            this.新增食材ToolStripMenuItem.Name = "新增食材ToolStripMenuItem";
            this.新增食材ToolStripMenuItem.Size = new System.Drawing.Size(51, 23);
            this.新增食材ToolStripMenuItem.Text = "設定";
            // 
            // 新增食材類別ToolStripMenuItem
            // 
            this.新增食材類別ToolStripMenuItem.Name = "新增食材類別ToolStripMenuItem";
            this.新增食材類別ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.新增食材類別ToolStripMenuItem.Text = "新增食材類別";
            this.新增食材類別ToolStripMenuItem.Click += new System.EventHandler(this.CollectionsToolStripMenuItem_Click);
            // 
            // 新增食材ToolStripMenuItem1
            // 
            this.新增食材ToolStripMenuItem1.Name = "新增食材ToolStripMenuItem1";
            this.新增食材ToolStripMenuItem1.Size = new System.Drawing.Size(168, 24);
            this.新增食材ToolStripMenuItem1.Text = "新增食材";
            this.新增食材ToolStripMenuItem1.Click += new System.EventHandler(this.IngredientsToolStripMenuItem1_Click);
            // 
            // 資料庫連線ToolStripMenuItem
            // 
            this.資料庫連線ToolStripMenuItem.Name = "資料庫連線ToolStripMenuItem";
            this.資料庫連線ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.資料庫連線ToolStripMenuItem.Text = "資料庫連線";
            this.資料庫連線ToolStripMenuItem.Click += new System.EventHandler(this.DBConntionToolStripMenuItem_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 444);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Menu";
            this.Text = "Menu";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 新增食材ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新增食材類別ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新增食材ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 資料庫連線ToolStripMenuItem;
    }
}

