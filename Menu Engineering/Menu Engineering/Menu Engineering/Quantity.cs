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
    public partial class Quantity : Form
    {
        public Quantity()
        {
            InitializeComponent();
        }
       public decimal Total = 0;
       public decimal Used = 0;
       public string FoodName;
       protected override void OnLoad(EventArgs e)
       {
           base.OnLoad(e);
           label1.Text = FoodName;
       }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Total = numericUpDownTotalQuantity.Value;
            Used = numericUpDownQuantityused.Value;
        }
    }
}
