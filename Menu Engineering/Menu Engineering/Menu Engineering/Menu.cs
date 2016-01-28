using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SQLHelper;
using CommTool;
using System.IO;
using System.Security.Cryptography;

namespace Menu_Engineering
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        string ConfigPath;
        ConfigManager configmanger = null;
        Byte[] key = { 12, 23, 34, 45, 56, 67, 78, 89 };
        Byte[] iv = { 120, 230, 10, 1, 10, 20, 30, 40 };


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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                
                //ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Menu Engineering.exe.config");
                //configmanger = new ConfigManager(ConfigPath, "Menu_Engineering");
                //string BaseConncection = configmanger.GetValue("BaseConncection");
                ////string[] sp = AppConfig.Default.BaseConncection.ToString().Split(';');
                ////string tbIP = sp[0].Replace("Data Source=", "");
                ////string tbDBName = sp[1].Replace("Initial Catalog=", "");
                ////string tbUser = sp[2].Replace("User Id=", "");
                ////string tbPw = sp[3].Replace("Password=", "");
                //////tbPw = MyDESCryptoDe(tbPw, key, iv);
                ////SHelper._sqlconnection = string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3}",
                ////      tbIP, tbDBName, tbUser, tbPw);
                SHelper._sqlconnection = AppConfig.Default.BaseConncection.ToString();

                DataTable dt = SHelper.ExeDataTable("Select top 1 * from Food");
                CollectionsView();
                //AppDomain.CurrentDomain.SetData("BaseConncection", "Data Source=127.0.0.2;Initial Catalog=MenuEngineering;User Id=sa;Password=dcms;Application Name=DcmsHr");
                //AppConfig.Default.BaseConncection = "Data Source=127.0.0.5;Initial Catalog=MenuEngineering;User Id=sa;Password=dcms;Application Name=DcmsHr"; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }           
           }


        private void CollectionsView()
        {
            DataTable dt =new DataTable();
            dt = SHelper.ExeDataTable("Select Name,Remark from Collections");
            dataGridView1.DataSource = dt;

        }

        


        private void IngredientsToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void DBConntionToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SQLForm SQLCon = new SQLForm();
            SQLCon.ShowDialog();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Collections coll = new Collections();
            coll.ShowDialog();
            if (coll.DialogResult == DialogResult.OK)
                CollectionsView();
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow view = dataGridView1.SelectedRows[0];
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Name", view.Cells[0].Value);
                DataTable dt = SQLHelper.SHelper.ExeDataTableUseParameter("Select CollectionsId from Collections Where Name =@Name", dic);
                string Id = string.Empty;
                Id = dt.Rows[0][0].ToString();
                Collections coll = new Collections("Master","Modify",Id);
                coll.ShowDialog();
                if (coll.DialogResult == DialogResult.OK)
                    CollectionsView();
            }
            else
            {
                MessageBox.Show("未選擇資料列");
            }          
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("確定要刪除此類別嗎?", "提示", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DataGridViewRow view = dataGridView1.SelectedRows[0];
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("Name", view.Cells[0].Value);
                    SQLHelper.SHelper.ExeNoQueryUseParameter("Delete Collections Where Name =@Name", dic);
                    CollectionsView();
                }        
            }
            else
            {
                MessageBox.Show("未選擇資料列");
            }
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point po =new Point(30,50);
                contextMenuStripCollections.Show(MousePosition);    
            }
        }

        private void FoodAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationDetail("Add");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        private void OperationDetail(string Type)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow view = dataGridView1.SelectedRows[0];
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Name", view.Cells[0].Value);
                DataTable dt = SQLHelper.SHelper.ExeDataTableUseParameter("Select CollectionsId from Collections Where Name =@Name", dic);
                string Id = string.Empty;
                Id = dt.Rows[0][0].ToString();
                Collections coll = new Collections("Detail", Type, Id);
                coll.ShowDialog();
                if (coll.DialogResult == DialogResult.OK)
                    FreshDetail();
            }
            else
            {
                MessageBox.Show("未選擇資料列");
            }
        }

        private void FoodModifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && dataGridViewFood.SelectedRows.Count>0) 
            {
                string Id;
                string FoodId;
                GetDetail(out Id, out FoodId);

                Collections coll = new Collections("Detail", "Modify", Id, FoodId);
                coll.ShowDialog();
                if (coll.DialogResult == DialogResult.OK)
                    FreshDetail();
            }
            else
            {
                MessageBox.Show("未選擇資料列");
            }
        }

        /// <summary>
        /// 取得明細資料ID
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="FoodId"></param>
        private void GetDetail(out string Id, out string FoodId)
        {
            //Master
            DataGridViewRow view = dataGridView1.SelectedRows[0];
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Name", view.Cells[0].Value);
            DataTable dt = SQLHelper.SHelper.ExeDataTableUseParameter("Select CollectionsId from Collections Where Name =@Name", dic);
            Id = string.Empty;
            Id = dt.Rows[0][0].ToString();

            //Detail
            DataGridViewRow Detailview = dataGridViewFood.SelectedRows[0];
            Dictionary<string, object> dicdetail = new Dictionary<string, object>();
            dicdetail.Add("Name", Detailview.Cells[0].Value);
            dicdetail.Add("CollectionsId", Id);
            DataTable dtdetail = SQLHelper.SHelper.ExeDataTableUseParameter("Select FoodId from Food Where Name =@Name AND CollectionsId=@CollectionsId", dicdetail);
            FoodId = string.Empty;
            FoodId = dtdetail.Rows[0][0].ToString();
        }



        private void FoodDeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && dataGridViewFood.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("確定要刪除此食材嗎?", "提示", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string Id;
                    string FoodId;
                    GetDetail(out Id, out FoodId);
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("FoodId", FoodId);
                    SQLHelper.SHelper.ExeNoQueryUseParameter("Delete Food Where FoodId =@FoodId", dic);
                      FreshDetail();
                }  
            }
            else
            {
                MessageBox.Show("未選擇資料列");
            }
        }

        private void dataGridViewFood_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point po = new Point(30, 50);
                contextMenuStripFood.Show(MousePosition);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            FreshDetail();
        }

        private void FreshDetail()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow view = dataGridView1.SelectedRows[0];
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Name", view.Cells[0].Value);
                DataTable dtM = SQLHelper.SHelper.ExeDataTableUseParameter("Select CollectionsId from Collections Where Name =@Name", dic);
                string Id = string.Empty;
                Id = dtM.Rows[0][0].ToString();
                string sql = string.Format("Select Name,Remark from Food Where CollectionsId ='{0}'", Id);
                DataTable dtdetail = SQLHelper.SHelper.ExeDataTable(sql);
                dataGridViewFood.DataSource = dtdetail;
            }
        }
       
    }
}
