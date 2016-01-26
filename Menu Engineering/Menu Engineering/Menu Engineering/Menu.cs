using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Menu_Engineering
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void CollectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void IngredientsToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void DBConntionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLConntion SQLCon = new SQLConntion();
            SQLCon.ShowDialog();
        }
    }
}
