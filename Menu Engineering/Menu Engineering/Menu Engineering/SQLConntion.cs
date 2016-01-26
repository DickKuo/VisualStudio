using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using SQLHelper;
using System.IO;
using CommTool;

namespace Menu_Engineering
{
    public partial class SQLConntion : Form
    {
        string ConfigPath;
        ConfigManager configmanger = null;

        public SQLConntion()
        {
            InitializeComponent();
            ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Menu Engineering.exe.config");
            configmanger = new ConfigManager(ConfigPath, "Menu_Engineering");
            string BaseConncection = configmanger.GetValue("BaseConncection");
            string[] sp = BaseConncection.Split(';');
            tbIP.Text =sp[0].Replace("Data Source=","");
            tbDBName.Text = sp[1].Replace("Initial Catalog=", "");
            tbUser.Text = sp[2].Replace("User Id=", "");
            tbPw.Text = sp[3].Replace("Password=", ""); 
        }

        private void btConnection_Click(object sender, EventArgs e)
        {
            try
            {
                SHelper._sqlconnection = string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};Application Name=DcmsHr",
                    tbIP.Text, tbDBName.Text, tbUser.Text, tbPw.Text);
                DataTable dt = SHelper.ExeDataTable("Select top 1 FoodId from Food");
                MessageBox.Show("連線成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("連線失敗:"+ex.Message);
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {

        }
    }
}
