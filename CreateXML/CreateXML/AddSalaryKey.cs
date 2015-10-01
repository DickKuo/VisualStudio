using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using CommTool;

namespace CreateXML
{
    public partial class AddSalaryKey : Form
    {
        public AddSalaryKey()
        {
            InitializeComponent();
        }

        Dictionary<string, List<string>> Modules = new Dictionary<string, List<string>>();
        Dictionary<string, string> EnToCHT = new Dictionary<string, string>();
        Dictionary<string, string> CHTToEn = new Dictionary<string, string>();


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        
        }

        /// <summary>
        /// 20141006 add by Dick 取得設定的路徑
        /// </summary>
        /// <returns></returns>
        private static string GetSettinhPath()
        {
            XmlDocument doc = CommTool.XmlFile.LoadXml(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Path.xml");
            XmlNode root = doc.SelectSingleNode("root");
            string ProgamPath = string.Empty;
            foreach (XmlNode node in root.ChildNodes)
            {
                if (Convert.ToBoolean(node.Attributes["Set"].Value))
                {
                    ProgamPath = node.Attributes["Xpath"].Value.ToString();
                }
            }
            return ProgamPath;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrEmpty(textdescription.Text))
            {
                ErrorMessage.AppendLine("ScName 不能為空。");
            }
            if (string.IsNullOrEmpty(textEnName.Text))
            {
                ErrorMessage.AppendLine("EnName 不能為空。");
            }
            if (CB_SalaryKeyBigType.SelectedIndex==-1)
            {
                ErrorMessage.AppendLine("SalaryBigType 不能為空。");
            }
            if (string.IsNullOrEmpty(textCode.Text))
            {
                ErrorMessage.AppendLine("Code 不能為空。");
            }
            
          
            if (ErrorMessage.Length > 0)
            {
                MessageBox.Show(ErrorMessage.ToString());
                return;
            }
            Add();
            MessageBox.Show("新增完成。");
            this.Close();
        }

        private  void Add()
        {
            string SettingPath = GetSettinhPath();
            if (!string.IsNullOrEmpty(SettingPath))
            {
                DirectoryInfo dir = new DirectoryInfo(SettingPath);
                string ParentPath = dir.Parent.FullName;
                string DefaultDataForCasePath = Path.Combine(ParentPath, "DigiWin.HR.CustomBusinessImplement");
                DefaultDataForCasePath = Path.Combine(DefaultDataForCasePath, "Configuration");
                DefaultDataForCasePath = Path.Combine(DefaultDataForCasePath, "DefaultData");
                DefaultDataForCasePath = Path.Combine(DefaultDataForCasePath, "DefaultDataForCase.xml");
                #region 20150727 add by Dick for #70
                System.IO.FileInfo batchItemAttribute = new FileInfo(DefaultDataForCasePath);
                batchItemAttribute.Attributes = FileAttributes.Normal;
                #endregion              
                if (File.Exists(DefaultDataForCasePath))
                {
                    CheckNode(DefaultDataForCasePath);
                    XmlDocument doc = XmlFile.LoadXml(DefaultDataForCasePath);
                    XmlNodeList nodelist = doc.SelectNodes("/Root/DataEntity");
                    XmlNode CodeKind = null;
                    foreach (XmlNode node in nodelist)
                    {
                        if (node.Attributes["TypeKey"].Value == "SalaryKey")
                        {
                            CodeKind = node;
                        }
                    }

                    #region 建立資料主表

                    string SalaryKeyBigType = Swap(CB_SalaryKeyBigType.SelectedItem.ToString());                    
                    XmlElement data = doc.CreateElement("data");
                    data.SetAttribute("name", "SalaryKey" + textCode.Text);
                    data.SetAttribute("description", textdescription.Text);
                    StringBuilder StrEN = new StringBuilder();
                    XmlElement EN = doc.CreateElement("EN");
                    StrEN.AppendFormat("<![CDATA[<SalaryKey><SalaryKeyId>{0}</SalaryKeyId><KeyId>{0}</KeyId><SalaryKeyBigTypeId>{1}</SalaryKeyBigTypeId><SalaryKeyTypeId>{2}</SalaryKeyTypeId><DataField>DataField_{0}</DataField><Remark>System default data</Remark><Flag>1</Flag><Name>{3}</Name><CorporationId>688564ce-c44c-4e1b-a58d-a10091b6e77b</CorporationId><OwnerId>System</OwnerId><IsUse>true</IsUse></SalaryKey>]]>", textCode.Text, SalaryKeyBigType, SwapSalary(comboKeyType.SelectedItem != null ? comboKeyType.SelectedItem.ToString() : string.Empty), textEnName.Text);
                    EN.InnerXml = StrEN.ToString();
                    data.AppendChild(EN);
                    StringBuilder StrCHS = new StringBuilder();
                    XmlElement CHS = doc.CreateElement("CHS");
                    string temp =Form1.translateEncodingByWord(textdescription.Text, true).Trim();
                    StrCHS.AppendFormat("<![CDATA[<SalaryKey><SalaryKeyId>{0}</SalaryKeyId><KeyId>{0}</KeyId><SalaryKeyBigTypeId>{1}</SalaryKeyBigTypeId><SalaryKeyTypeId>{2}</SalaryKeyTypeId><DataField>DataField_{0}</DataField><Remark>系统默认数据</Remark><Flag>1</Flag><Name>{3}</Name><CorporationId>688564ce-c44c-4e1b-a58d-a10091b6e77b</CorporationId><OwnerId>System</OwnerId><IsUse>true</IsUse></SalaryKey>]]>", textCode.Text, SalaryKeyBigType, SwapSalary(comboKeyType.SelectedItem != null ? comboKeyType.SelectedItem.ToString() : string.Empty), temp);
                    CHS.InnerXml = StrCHS.ToString();
                    data.AppendChild(CHS);
                    StringBuilder StrCHT = new StringBuilder();
                    XmlElement CHT = doc.CreateElement("CHT");
                    StrCHT.AppendFormat("<![CDATA[<SalaryKey><SalaryKeyId>{0}</SalaryKeyId><KeyId>{0}</KeyId><SalaryKeyBigTypeId>{1}</SalaryKeyBigTypeId><SalaryKeyTypeId>{2}</SalaryKeyTypeId><DataField>DataField_{0}</DataField><Remark>系統預設資料</Remark><Flag>1</Flag><Name>{3}</Name><CorporationId>688564ce-c44c-4e1b-a58d-a10091b6e77b</CorporationId><OwnerId>System</OwnerId><IsUse>true</IsUse></SalaryKey>]]>", textCode.Text, SalaryKeyBigType, SwapSalary(comboKeyType.SelectedItem != null ? comboKeyType.SelectedItem.ToString() : string.Empty), textdescription.Text);
                    CHT.InnerXml = StrCHT.ToString();
                    data.AppendChild(CHT);
                    CodeKind.AppendChild(data);     
                    #endregion
                    
                   
                    doc.Save(DefaultDataForCasePath);
                }
                else
                {
                    MessageBox.Show("DefaultDataForCase.xml 檔案不存在。");
                    return; 
                }
            }
        }
       
        private string Swap(string p)
        {
            string result = string.Empty;
            switch( p)
            {
                case "人事資訊":
                    result = "SalaryKeyBigType_001";
                    break;
                case "薪資福利類":
                    result = "SalaryKeyBigType_002";
                    break;
                case "考勤信息":
                    result = "SalaryKeyBigType_003";
                    break;
                case "績效資訊":
                    result = "SalaryKeyBigType_004";
                    break;
                case "獎懲資訊":
                    result = "SalaryKeyBigType_005";
                    break;
                case "其它資訊":
                    result = "SalaryKeyBigType_006";
                    break;
            }
            return result;
        }


        private string SwapSalary(string p)
        {
            string result = string.Empty;
            switch (p)
            {
                case"系統":
                    result = "SalaryKeyType_001";
                    break; 
                case"浮動":
                    result = "SalaryKeyType_002";
                    break; 
                case"指定":
                    result = "SalaryKeyType_003";
                    break; 
                case"穩定":
                    result = "SalaryKeyType_004";
                    break; 
                case"統籌":
                    result = "SalaryKeyType_005";
                    break; 
                case"計件":
                    result = "SalaryKeyType_006";
                    break;
                case"累計次數":
                    result = "SalaryStatKeyType_001";
                    break; 
                case"累計時間":
                    result = "SalaryStatKeyType_002";
                    break; 
                case"折算次數":
                    result = "SalaryStatKeyType_003";
                    break;  
                case"每月住宿費":
                    result = "SalaryOtherType_001";
                    break; 
                case"每月工服費用":
                    result = "SalaryOtherType_002";
                    break; 
                case"就餐次數":
                    result = "SalaryOtherType_003";
                    break;
            }
            return result;
        }

        /// <summary>
        /// 檢查是否存在CodeInfo && CodeKind 沒有則建立新的。
        /// </summary>
        /// <param name="DefaultDataForCasePath"></param>
        private static void CheckNode(string DefaultDataForCasePath)
        {
            XmlDocument doc = XmlFile.LoadXml(DefaultDataForCasePath);
            XmlNodeList nodelist = doc.SelectNodes("/Root/DataEntity");
            XmlNode DataEntityCodeKind = null;
            XmlNode DataEntityCodeInfo = null;
            XmlNode DataEntitySalaryKey = null;
            foreach (XmlNode node in nodelist)
            {
                if (node.Attributes["TypeKey"].Value == "CodeKind")
                {
                    DataEntityCodeKind = node;
                }
                if (node.Attributes["TypeKey"].Value == "CodeInfo")
                {
                    DataEntityCodeInfo = node;
                }
                if (node.Attributes["TypeKey"].Value == "SalaryKey")
                {
                    DataEntitySalaryKey = node;
                }
            }
            if (DataEntityCodeKind == null)
            {
                CreateNew(DefaultDataForCasePath, "CodeKind");
            }
            if (DataEntityCodeInfo == null)
            {
                CreateNew(DefaultDataForCasePath, "CodeInfo");
            }
            if (DataEntitySalaryKey == null)
            {
                CreateNew(DefaultDataForCasePath, "SalaryKey");
            }
        }

        /// <summary>
        /// 建立基本XML結構
        /// </summary>
        /// <param name="DefaultDataForCasePath"></param>
        /// <param name="Name"></param>
        /// <param name="doc"></param>
        private static void CreateNew(string DefaultDataForCasePath, string Name)
        {
            XmlDocument doc = XmlFile.LoadXml(DefaultDataForCasePath);
            XmlElement DataEntity = doc.CreateElement("DataEntity");
            XmlNode Root = doc.SelectSingleNode("/Root");
            DataEntity.SetAttribute("TypeKey", Name);
            DataEntity.SetAttribute("name", Name);
            Root.AppendChild(DataEntity);
            doc.Save(DefaultDataForCasePath);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CB_SalaryKeyBigType_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboKeyType.Items.Clear();
            switch (CB_SalaryKeyBigType.SelectedItem.ToString())
            {                  
                case "人事資訊":
                    //result = "SalaryKeyBigType_001";
                    break;
                case "薪資福利類":
                    comboKeyType.Items.Add("系統");
                    comboKeyType.Items.Add("浮動");
                    comboKeyType.Items.Add("指定");
                    comboKeyType.Items.Add("穩定");
                    comboKeyType.Items.Add("統籌");
                    comboKeyType.Items.Add("計件");
                    break;
                case "考勤信息":
                    comboKeyType.Items.Add("累計次數");
                    comboKeyType.Items.Add("累計時間");
                    comboKeyType.Items.Add("折算次數");
                    break;
                case "績效資訊":
                    //result = "SalaryKeyBigType_004";
                    break;
                case "獎懲資訊":
                    //result = "SalaryKeyBigType_005";
                    break;
                case "其它資訊":
                    comboKeyType.Items.Add("每月住宿費");
                    comboKeyType.Items.Add("每月工服費用");
                    comboKeyType.Items.Add("就餐次數");
                    break;
            }
        }
    }
}
