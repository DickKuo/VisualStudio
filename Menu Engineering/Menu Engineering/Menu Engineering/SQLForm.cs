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
using System.Security.Cryptography;

namespace Menu_Engineering
{
    public partial class SQLForm : Form
    {
        string ConfigPath;
        ConfigManager configmanger = null;
        Byte[] key = { 12, 23, 34, 45, 56, 67, 78, 89 };
        Byte[] iv = { 120, 230, 10, 1, 10, 20, 30, 40 };

        public SQLForm()
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
            tbPw.Text = MyDESCryptoDe(tbPw.Text, key, iv);
         
        }

        private void btConnection_Click(object sender, EventArgs e)
        {
            try
            {
                SHelper._sqlconnection = string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3}",
                    tbIP.Text, tbDBName.Text, tbUser.Text, tbPw.Text);
                SHelper.DBIP = tbIP.Text;
                SHelper.DBName = tbDBName.Text;
                DataTable dt = SHelper.ExeDataTable("Select top 1 * from Food");
                MessageBox.Show("連線成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("連線失敗:" + ex.Message);
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            string des=MyDESCrypto(tbPw.Text, key, iv);  
            string connt =string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3}",
                    tbIP.Text, tbDBName.Text, tbUser.Text, des);
            configmanger.SetValue("BaseConncection", connt);
            MessageBox.Show("存檔完成");
        }



        //2、DES对字符串加密、解密  
        string MyDESCrypto(string str, byte[] keys, byte[] ivs)
        {
            //加密  
            byte[] strs = Encoding.Unicode.GetBytes(str);
            DESCryptoServiceProvider desc = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            ICryptoTransform transform = desc.CreateEncryptor(keys, ivs);//加密对象  
            CryptoStream cStream = new CryptoStream(mStream, transform, CryptoStreamMode.Write);
            cStream.Write(strs, 0, strs.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        string MyDESCryptoDe(string str, byte[] keys, byte[] ivs)
        {
            //解密  
            byte[] strs = Convert.FromBase64String(str);
            DESCryptoServiceProvider desc = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            ICryptoTransform transform = desc.CreateDecryptor(keys, ivs);//解密对象 
            CryptoStream cStream = new CryptoStream(mStream, transform, CryptoStreamMode.Write);
            cStream.Write(strs, 0, strs.Length);
            cStream.FlushFinalBlock();
            return Encoding.Unicode.GetString(mStream.ToArray());
        } 
    }
}
