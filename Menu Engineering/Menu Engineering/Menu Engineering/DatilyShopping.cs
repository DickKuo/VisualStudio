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
    public partial class DatilyShopping : Form
    {
        string FoodId = string.Empty;

        public DatilyShopping(string id)
        {
            InitializeComponent();
            FoodId = id;
           DataTable dt = SQLHelper.SHelper.ExeDataTable(string.Format("Select Name from Food where FoodId ='{0}'", FoodId));

           if (dt.Rows.Count > 0)
           {
               textBox1.Text = dt.Rows[0][0].ToString();
           }
        }


    }
}
