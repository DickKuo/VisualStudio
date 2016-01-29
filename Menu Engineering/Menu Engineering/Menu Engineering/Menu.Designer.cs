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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.新增食材ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.資料庫連線ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabFood = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridViewFood = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabDatily = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dataGridViewDatily = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerBegin = new System.Windows.Forms.DateTimePicker();
            this.tabMenu = new System.Windows.Forms.TabPage();
            this.contextMenuStripCollections = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripFood = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.新增ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刪除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.每日採購ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripDatily = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.修改ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.刪除ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cbCollections = new System.Windows.Forms.ComboBox();
            this.menuStrip1.SuspendLayout();
            this.tabFood.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFood)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabDatily.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDatily)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.contextMenuStripCollections.SuspendLayout();
            this.contextMenuStripFood.SuspendLayout();
            this.contextMenuStripDatily.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新增食材ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(899, 27);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 新增食材ToolStripMenuItem
            // 
            this.新增食材ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.資料庫連線ToolStripMenuItem});
            this.新增食材ToolStripMenuItem.Name = "新增食材ToolStripMenuItem";
            this.新增食材ToolStripMenuItem.Size = new System.Drawing.Size(51, 23);
            this.新增食材ToolStripMenuItem.Text = "設定";
            // 
            // 資料庫連線ToolStripMenuItem
            // 
            this.資料庫連線ToolStripMenuItem.Name = "資料庫連線ToolStripMenuItem";
            this.資料庫連線ToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.資料庫連線ToolStripMenuItem.Text = "資料庫連線";
            this.資料庫連線ToolStripMenuItem.Click += new System.EventHandler(this.DBConntionToolStripMenuItem_Click);
            // 
            // tabFood
            // 
            this.tabFood.Controls.Add(this.groupBox2);
            this.tabFood.Controls.Add(this.groupBox1);
            this.tabFood.Location = new System.Drawing.Point(4, 25);
            this.tabFood.Name = "tabFood";
            this.tabFood.Padding = new System.Windows.Forms.Padding(3);
            this.tabFood.Size = new System.Drawing.Size(861, 373);
            this.tabFood.TabIndex = 0;
            this.tabFood.Text = "食材";
            this.tabFood.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dataGridViewFood);
            this.groupBox2.Location = new System.Drawing.Point(24, 196);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(812, 171);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "食材";
            // 
            // dataGridViewFood
            // 
            this.dataGridViewFood.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewFood.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFood.Location = new System.Drawing.Point(17, 25);
            this.dataGridViewFood.MultiSelect = false;
            this.dataGridViewFood.Name = "dataGridViewFood";
            this.dataGridViewFood.RowTemplate.Height = 27;
            this.dataGridViewFood.Size = new System.Drawing.Size(779, 132);
            this.dataGridViewFood.TabIndex = 0;
            this.dataGridViewFood.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewFood_MouseDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Location = new System.Drawing.Point(24, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(812, 173);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "食材類別";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(17, 24);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(779, 140);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabFood);
            this.tabControl1.Controls.Add(this.tabDatily);
            this.tabControl1.Controls.Add(this.tabMenu);
            this.tabControl1.Location = new System.Drawing.Point(12, 30);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(869, 402);
            this.tabControl1.TabIndex = 1;
            // 
            // tabDatily
            // 
            this.tabDatily.Controls.Add(this.groupBox4);
            this.tabDatily.Controls.Add(this.groupBox3);
            this.tabDatily.Location = new System.Drawing.Point(4, 25);
            this.tabDatily.Name = "tabDatily";
            this.tabDatily.Padding = new System.Windows.Forms.Padding(3);
            this.tabDatily.Size = new System.Drawing.Size(861, 373);
            this.tabDatily.TabIndex = 2;
            this.tabDatily.Text = "每日";
            this.tabDatily.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.dataGridViewDatily);
            this.groupBox4.Location = new System.Drawing.Point(18, 86);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(823, 271);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "食材資料";
            // 
            // dataGridViewDatily
            // 
            this.dataGridViewDatily.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewDatily.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDatily.Location = new System.Drawing.Point(7, 25);
            this.dataGridViewDatily.Name = "dataGridViewDatily";
            this.dataGridViewDatily.RowTemplate.Height = 27;
            this.dataGridViewDatily.Size = new System.Drawing.Size(799, 228);
            this.dataGridViewDatily.TabIndex = 0;
            this.dataGridViewDatily.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewDatily_MouseDown);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.cbCollections);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.dateTimePickerEnd);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.dateTimePickerBegin);
            this.groupBox3.Location = new System.Drawing.Point(18, 18);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(823, 62);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "查詢條件";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(672, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 28);
            this.button1.TabIndex = 3;
            this.button1.Text = "查詢";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.Location = new System.Drawing.Point(267, 21);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(200, 25);
            this.dateTimePickerEnd.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(230, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "~";
            // 
            // dateTimePickerBegin
            // 
            this.dateTimePickerBegin.Location = new System.Drawing.Point(7, 21);
            this.dateTimePickerBegin.Name = "dateTimePickerBegin";
            this.dateTimePickerBegin.Size = new System.Drawing.Size(200, 25);
            this.dateTimePickerBegin.TabIndex = 0;
            // 
            // tabMenu
            // 
            this.tabMenu.Location = new System.Drawing.Point(4, 25);
            this.tabMenu.Name = "tabMenu";
            this.tabMenu.Padding = new System.Windows.Forms.Padding(3);
            this.tabMenu.Size = new System.Drawing.Size(861, 373);
            this.tabMenu.TabIndex = 1;
            this.tabMenu.Text = "菜單";
            this.tabMenu.UseVisualStyleBackColor = true;
            // 
            // contextMenuStripCollections
            // 
            this.contextMenuStripCollections.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.modifyToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStripCollections.Name = "contextMenuStrip1";
            this.contextMenuStripCollections.Size = new System.Drawing.Size(109, 76);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.addToolStripMenuItem.Text = "新增";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // modifyToolStripMenuItem
            // 
            this.modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
            this.modifyToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.modifyToolStripMenuItem.Text = "修改";
            this.modifyToolStripMenuItem.Click += new System.EventHandler(this.modifyToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.deleteToolStripMenuItem.Text = "刪除";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // contextMenuStripFood
            // 
            this.contextMenuStripFood.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新增ToolStripMenuItem,
            this.修改ToolStripMenuItem,
            this.刪除ToolStripMenuItem,
            this.每日採購ToolStripMenuItem});
            this.contextMenuStripFood.Name = "contextMenuStripFood";
            this.contextMenuStripFood.Size = new System.Drawing.Size(139, 100);
            // 
            // 新增ToolStripMenuItem
            // 
            this.新增ToolStripMenuItem.Name = "新增ToolStripMenuItem";
            this.新增ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.新增ToolStripMenuItem.Text = "新增";
            this.新增ToolStripMenuItem.Click += new System.EventHandler(this.FoodAddToolStripMenuItem_Click);
            // 
            // 修改ToolStripMenuItem
            // 
            this.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem";
            this.修改ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.修改ToolStripMenuItem.Text = "修改";
            this.修改ToolStripMenuItem.Click += new System.EventHandler(this.FoodModifyToolStripMenuItem_Click);
            // 
            // 刪除ToolStripMenuItem
            // 
            this.刪除ToolStripMenuItem.Name = "刪除ToolStripMenuItem";
            this.刪除ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.刪除ToolStripMenuItem.Text = "刪除";
            this.刪除ToolStripMenuItem.Click += new System.EventHandler(this.FoodDeleteToolStripMenuItem_Click);
            // 
            // 每日採購ToolStripMenuItem
            // 
            this.每日採購ToolStripMenuItem.Name = "每日採購ToolStripMenuItem";
            this.每日採購ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.每日採購ToolStripMenuItem.Text = "每日採購";
            this.每日採購ToolStripMenuItem.Click += new System.EventHandler(this.DatilyInToolStripMenuItem_Click);
            // 
            // contextMenuStripDatily
            // 
            this.contextMenuStripDatily.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.修改ToolStripMenuItem1,
            this.刪除ToolStripMenuItem1});
            this.contextMenuStripDatily.Name = "contextMenuStripDatily";
            this.contextMenuStripDatily.Size = new System.Drawing.Size(109, 52);
            // 
            // 修改ToolStripMenuItem1
            // 
            this.修改ToolStripMenuItem1.Name = "修改ToolStripMenuItem1";
            this.修改ToolStripMenuItem1.Size = new System.Drawing.Size(108, 24);
            this.修改ToolStripMenuItem1.Text = "修改";
            this.修改ToolStripMenuItem1.Click += new System.EventHandler(this.DatilyModifyToolStripMenuItem1_Click);
            // 
            // 刪除ToolStripMenuItem1
            // 
            this.刪除ToolStripMenuItem1.Name = "刪除ToolStripMenuItem1";
            this.刪除ToolStripMenuItem1.Size = new System.Drawing.Size(108, 24);
            this.刪除ToolStripMenuItem1.Text = "刪除";
            this.刪除ToolStripMenuItem1.Click += new System.EventHandler(this.DatilyDeleteToolStripMenuItem1_Click);
            // 
            // cbCollections
            // 
            this.cbCollections.FormattingEnabled = true;
            this.cbCollections.Location = new System.Drawing.Point(500, 25);
            this.cbCollections.Name = "cbCollections";
            this.cbCollections.Size = new System.Drawing.Size(121, 23);
            this.cbCollections.TabIndex = 4;
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 444);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Menu";
            this.Text = "Menu Engineering";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabFood.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFood)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabDatily.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDatily)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.contextMenuStripCollections.ResumeLayout(false);
            this.contextMenuStripFood.ResumeLayout(false);
            this.contextMenuStripDatily.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 新增食材ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 資料庫連線ToolStripMenuItem;
        private System.Windows.Forms.TabPage tabFood;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripCollections;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridViewFood;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFood;
        private System.Windows.Forms.ToolStripMenuItem 新增ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刪除ToolStripMenuItem;
        private System.Windows.Forms.TabPage tabMenu;
        private System.Windows.Forms.TabPage tabDatily;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerBegin;
        private System.Windows.Forms.DataGridView dataGridViewDatily;
        private System.Windows.Forms.ToolStripMenuItem 每日採購ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDatily;
        private System.Windows.Forms.ToolStripMenuItem 修改ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 刪除ToolStripMenuItem1;
        private System.Windows.Forms.ComboBox cbCollections;
    }
}

