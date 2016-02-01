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
    public partial class MenuCollection : Form
    {
        string MenuCollectionId = string.Empty;
        string PType = string.Empty;

        public MenuCollection(string ptype = "Add", string collectionId="")
        {
            InitializeComponent();
            MenuCollectionId = collectionId;
            PType = ptype;
            if (ptype == "Modify")
            {
                string sql = string.Format("Select * from MenuCollections Where MenuCollectionsId ='{0}'", MenuCollectionId);
                DataTable dt = SQLHelper.SHelper.ExeDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    TbCollectionName.Text = dt.Rows[0][1].ToString();
                    richTextBox1.Text = dt.Rows[0][2].ToString();
                }
            }
            else
            {
                MenuCollectionId = Guid.NewGuid().ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TbCollectionName.Text))
            {
                MessageBox.Show("類別名稱不得為空。");
            }
            else
            {
                string sql = string.Empty;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                try
                {
                    sql = string.Format("Select * from MenuCollections Where Name =@Name AND MenuCollectionsId<>@MenuCollectionsId");
                            dic.Add("Name", TbCollectionName.Text);
                            dic.Add("MenuCollectionsId", MenuCollectionId);                    
                            DataTable dt = SQLHelper.SHelper.ExeDataTableUseParameter(sql, dic);
                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show("已存在相同的類別名稱。");
                            }
                            else
                            {
                                switch (PType)
                                {
                                    case "Add":
                                        #region 新增
                                        dic.Clear();
                                        sql = string.Format(" Insert Into MenuCollections (MenuCollectionsId,Name,Remark)values(@MenuCollectionsId,@Name,@Remark)");
                                        dic.Add("MenuCollectionsId", MenuCollectionId);
                                        dic.Add("Name", TbCollectionName.Text);
                                        dic.Add("Remark", richTextBox1.Text);
                                        SQLHelper.SHelper.ExeNoQueryUseParameter(sql, dic);
                                        MessageBox.Show("新增成功。");
                                        #endregion                                        
                                        break;
                                    case "Modify":
                                        #region 修改
                                        dic.Clear();
                                        sql = "Update MenuCollections Set Name=@Name,Remark=@Remark Where MenuCollectionsId=@MenuCollectionsId";
                                        dic.Add("Name", TbCollectionName.Text);
                                        dic.Add("Remark", richTextBox1.Text);
                                        dic.Add("MenuCollectionsId", MenuCollectionId);
                                        SQLHelper.SHelper.ExeNoQueryUseParameter(sql, dic);
                                        MessageBox.Show("修改成功。");
                                        #endregion
                                        break;
                                }
                            }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("變更失敗。");
                }
                finally
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}
