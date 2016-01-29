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
        string pType = string.Empty;
        string DatilyId = string.Empty;
        public DatilyShopping(string id, string type,string datilyId="")
        {
            InitializeComponent();
            FoodId = id;
            pType = type;
            DataTable dt = SQLHelper.SHelper.ExeDataTable(string.Format("Select Name from Food where FoodId ='{0}'", FoodId));

            if (dt.Rows.Count > 0)
            {
                textBox1.Text = dt.Rows[0][0].ToString();
                if (type == "Modify")
                {
                    DatilyId = datilyId;
                    DataTable dtTemp = SQLHelper.SHelper.ExeDataTable(string.Format("Select * from Datily where DatilyId ='{0}'", DatilyId));
                    if (dtTemp.Rows.Count > 0)
                    {
                        numericUpDownPrice.Value = Convert.ToDecimal(dtTemp.Rows[0][2]);
                        numericUpDownQuantity.Value = Convert.ToDecimal(dtTemp.Rows[0][3]);
                        rbRemark.Text = dtTemp.Rows[0][4].ToString();
                        dateTimePicker1.Value = Convert.ToDateTime(dtTemp.Rows[0][5]);
                        dateTimePicker1.Enabled = false;
                    }
                }
            }            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (numericUpDownPrice.Value < 0)
            {
                MessageBox.Show("價格不可以小於零");
            }
            else
            {
                if (numericUpDownQuantity.Value < 0)
                {
                    MessageBox.Show("重量不可以小於零");
                }
                else
                {
                    try
                    {
                        switch (pType)
                        {
                            case "Add":
                                #region  新增
                                DataTable dt = SQLHelper.SHelper.ExeDataTable(string.Format("Select * from Datily where FoodId='{0}' AND Day='{1}'", FoodId, dateTimePicker1.Value.Date.ToString("yyyy/MM/dd")));
                                if (dt.Rows.Count > 0)
                                {
                                    MessageBox.Show("當天已存在價格資料請確認。");
                                }
                                else
                                {
                                    Dictionary<string, object> dic = new Dictionary<string, object>();
                                    dic.Add("DatilyId", Guid.NewGuid());
                                    dic.Add("FoodId", FoodId);
                                    dic.Add("Price", numericUpDownPrice.Value);
                                    dic.Add("Quantity", numericUpDownQuantity.Value);
                                    dic.Add("Remark", rbRemark.Text);
                                    dic.Add("Day", dateTimePicker1.Value.Date);
                                    string sql = string.Format("Insert Into Datily (DatilyId,FoodId,Price,Quantity,Remark,Day)values(@DatilyId,@FoodId,@Price,@Quantity,@Remark,@Day)");
                                    SQLHelper.SHelper.ExeNoQueryUseParameter(sql, dic);
                                    MessageBox.Show("新增完成。");
                                }
                                #endregion
                                break;
                            case "Modify":
                                #region  修改
                                string thisSql = string.Format("Update Datily Set Price=@Price ,Quantity=@Quantity ,Remark=@Remark Where DatilyId=@DatilyId");
                                Dictionary<string, object> di = new Dictionary<string, object>();
                                di.Add("Price", numericUpDownPrice.Value);
                                di.Add("Quantity", numericUpDownQuantity.Value);
                                di.Add("DatilyId", DatilyId);
                                di.Add("Remark", rbRemark.Text);
                                SQLHelper.SHelper.ExeNoQueryUseParameter(thisSql, di);
                                MessageBox.Show("更新完成。");
                                #endregion
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("新增失敗。");
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
}
