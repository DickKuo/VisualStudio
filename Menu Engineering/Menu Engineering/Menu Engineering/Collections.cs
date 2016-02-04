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
    public partial class Collections : Form
    {
        string Id = string.Empty;
        string Type = string.Empty;
        string IsMaster = string.Empty;
        string FoodId = string.Empty;

        public Collections(string Master="Master",string type = "Add", string id = "",string foodid="")
        {
            InitializeComponent();
            Id = id;
            Type = type;
            IsMaster=Master;
            FoodId = foodid;

            switch (Master)
            {
                case "Master":
                    label1.Text = "類別名稱";
                    if (type == "Modify")
                    {
                        DataTable dt = SQLHelper.SHelper.ExeDataTable(string.Format("Select * from Collections where CollectionsId='{0}'", Id));
                        if (dt.Rows.Count > 0)
                        {
                            textBox1.Text = dt.Rows[0][1].ToString();
                            richTextBox1.Text = dt.Rows[0][2].ToString();
                        }
                    }
                    break;

                case "Detail":
                    label1.Text = "食材名稱";
                    if (type == "Modify")
                    {
                        DataTable dt = SQLHelper.SHelper.ExeDataTable(string.Format("Select * from Food where CollectionsId='{0}' AND FoodId='{1}'", Id,FoodId));
                        if (dt.Rows.Count > 0)
                        {
                            textBox1.Text = dt.Rows[0][2].ToString();
                            richTextBox1.Text = dt.Rows[0][3].ToString();
                        }
                    }
                    break;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("請輸入食品類別");
            }
            else
            {
                try
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    string sql = string.Empty;
                    switch (IsMaster)
                    {
                        case "Master":                            
                            #region  Master
                            switch (Type)
                            {
                                case "Add"://新增  
                                    DataTable dt = SQLHelper.SHelper.ExeDataTable(string.Format("Select * from Collections where Name ='{0}' ", textBox1.Text));
                                    if (dt.Rows.Count > 0)
                                    {
                                        MessageBox.Show("已存在相同食材類別");
                                    }
                                    else
                                    {
                                        sql = "Insert into Collections (CollectionsId,Name,Remark)values(@CollectionsId,@Name,@Remark)";
                                        dic.Add("CollectionsId", Guid.NewGuid());
                                        dic.Add("Name", textBox1.Text);
                                        dic.Add("Remark", richTextBox1.Text);
                                        SQLHelper.SHelper.ExeNoQueryUseParameter(sql, dic);
                                    }
                                    break;
                                case "Modify"://修改
                                    sql = "UpDate  Collections Set  Name=@Name,Remark=@Remark Where CollectionsId= @CollectionsId";
                                    dic.Add("CollectionsId", Id);
                                    dic.Add("Name", textBox1.Text);
                                    dic.Add("Remark", richTextBox1.Text);
                                    SQLHelper.SHelper.ExeNoQueryUseParameter(sql, dic);
                                    break;
                            }
                            #endregion
                        break;
                        case "Detail":
                        #region Detail
                        switch (Type)
                        {
                            case "Add"://新增  
                                DataTable dt = SQLHelper.SHelper.ExeDataTable(string.Format("Select * From Food where Name ='{0}' AND CollectionsId ='{1}'", textBox1.Text, Id));
                                    if (dt.Rows.Count > 0)
                                    {
                                        MessageBox.Show("已存在相同食材");
                                    }
                                    else
                                    {
                                        sql = "Insert Into Food (FoodId,CollectionsId,Name,Remark) values (@FoodId,@CollectionsId,@Name,@Remark)";
                                        dic.Add("FoodId", Guid.NewGuid());
                                        dic.Add("CollectionsId", Id);
                                        dic.Add("Name", textBox1.Text);
                                        dic.Add("Remark", richTextBox1.Text);
                                        SQLHelper.SHelper.ExeNoQueryUseParameter(sql, dic);
                                    }
                                break;
                            case "Modify"://修改
                                    sql = "UpDate  Food Set  Name=@Name,Remark=@Remark Where FoodId= @FoodId";
                                    dic.Add("FoodId", FoodId);
                                    dic.Add("Name", textBox1.Text);
                                    dic.Add("Remark", richTextBox1.Text);
                                    SQLHelper.SHelper.ExeNoQueryUseParameter(sql, dic);
                                break;
                        }
                        #endregion
                        break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("新增失敗");
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
