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
    public partial class AddCodeInfo : Form
    {
        public AddCodeInfo()
        {
            InitializeComponent();
        }

        Dictionary<string, List<string>> Modules = new Dictionary<string, List<string>>();
        Dictionary<string, string> EnToCHT = new Dictionary<string, string>();
        Dictionary<string, string> CHTToEn = new Dictionary<string, string>();


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            #region 20141224 add by Dick for 加入模組下拉方式
            //20150115 modified by Dick for 加入未設定位置時的提示訊息 #30
            string ProgamPath = GetSettinhPath();
            if (!Directory.Exists(ProgamPath))
            {
                MessageBox.Show("Export未設定，請設定好後重新啟動工具");
            }
            else
            {
                DirectoryInfo DirInfo = new DirectoryInfo(ProgamPath);
                string Permission = DirInfo.FullName + Path.DirectorySeparatorChar + "Configuration" + Path.DirectorySeparatorChar + "Permission.xml";
                if (!File.Exists(Permission))
                {
                    MessageBox.Show("Permission.xml不存在");
                }
                else
                {
                    Modules = GetModules(Permission);
                    string ModuleResource = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "ModResource.xml";
                    XmlDocument docResource = CommTool.XmlFile.LoadXml(ModuleResource);
                    XmlNode ResourceRoot = docResource.SelectSingleNode("root");
                    foreach (XmlNode node in ResourceRoot.ChildNodes)
                    {
                        //XmlNode ch = node.InnerText.Trim();
                        string innerText = node.ChildNodes[1].InnerXml.Trim();
                        string Attributes = node.Attributes[0].Value.Replace("Mod_", "");
                        EnToCHT.Add(Attributes, innerText);

                        if (!CHTToEn.ContainsKey(innerText))
                        {
                            CHTToEn.Add(innerText, Attributes);
                        }
                    }

                    foreach (string key in Modules.Keys)
                    {
                        string name = EnToCHT[key];
                        CB_Modules.Items.Add(name);
                    }

                }
            }
            #endregion
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

        /// <summary>
        /// 20141224 add by Dick 取得模組架構 #6
        /// </summary>
        /// <param name="pXmlPath"></param>
        /// <returns></returns>
        private Dictionary<string, List<string>> GetModules(string pXmlPath)
        {
            Dictionary<string, List<string>> DicModules = new Dictionary<string, List<string>>();
            XmlDocument doc = CommTool.XmlFile.LoadXml(pXmlPath);
            XmlNode root = doc.ChildNodes[1];
            StringBuilder SB = new StringBuilder();
            CommTool.XmlFile XmlFile = new CommTool.XmlFile();
            XmlNode Module = root.ChildNodes[1];
            foreach (XmlNode child in root.ChildNodes)
            {
                if (child.Name.Equals("Modules"))
                {
                    foreach (XmlNode node in child.ChildNodes)
                    {
                        string ModuleName = string.Empty;
                        if (node.Name.Equals("Module"))
                        {
                            ModuleName = node.Attributes[0].Value;
                            List<string> ChildList = new List<string>();
                            foreach (XmlNode SubNode in node.FirstChild.ChildNodes)
                            {
                                if (SubNode.Name.Equals("Module"))
                                {
                                    if (!ChildList.Contains(SubNode.Attributes[0].Value))
                                    {
                                        ChildList.Add(SubNode.Attributes[0].Value);
                                    }
                                }
                            }
                            DicModules[ModuleName] = ChildList;
                        }
                    }
                }
            }
            return DicModules;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder ErrorMessage = new StringBuilder();
            if (string.IsNullOrEmpty(textdescription.Text))
            {
                ErrorMessage.AppendLine("ScName 不能為空。");
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                ErrorMessage.AppendLine("EnName 不能為空。");
            }
            if (CB_Modules.SelectedIndex==-1)
            {
                ErrorMessage.AppendLine("BelongsModule 不能為空。");
            }
            if (string.IsNullOrEmpty(textCode.Text))
            {
                ErrorMessage.AppendLine("Code 不能為空。");
            }
            
            if (dataGridView1.Rows.Count == 1)
            {
                ErrorMessage.AppendLine("明細 不能為空。");
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
                if (File.Exists(DefaultDataForCasePath))
                {
                    CheckNode(DefaultDataForCasePath);
                    XmlDocument doc = XmlFile.LoadXml(DefaultDataForCasePath);
                    XmlNodeList nodelist = doc.SelectNodes("/Root/DataEntity");
                    XmlNode CodeKind = null;
                    XmlNode CodeInfo = null;
                    foreach (XmlNode node in nodelist)
                    {
                        if (node.Attributes["TypeKey"].Value == "CodeKind")
                        {
                            CodeKind = node;
                        }
                        if (node.Attributes["TypeKey"].Value == "CodeInfo")
                        {
                            CodeInfo = node;
                        }
                    }

                    #region 建立資料主表
                  
                    string Eng = Swap(CHTToEn[CB_Modules.SelectedItem.ToString()]);                    
                    XmlElement data = doc.CreateElement("data");
                    data.SetAttribute("name", "CodeKind_" + textCode.Text);
                    data.SetAttribute("description", textdescription.Text);
                    StringBuilder StrEN = new StringBuilder();
                    XmlElement EN = doc.CreateElement("EN");
                    StrEN.AppendFormat("<![CDATA[<CodeKind><CodeKindId>{0}</CodeKindId><ScName>{1}</ScName><EnName>{1}</EnName><IsSystem>{3}</IsSystem><Increasedly>{4}</Increasedly><Remark>{5}</Remark><Code>{0}</Code><BelongsModule>{2}</BelongsModule><Flag>true</Flag></CodeKind>]]>", textCode.Text, textBox2.Text, Eng, cbSystem.Checked.ToString().ToLower(), cb_increase.Checked.ToString().ToLower(), cb_increase.Checked == true ? "Code can be added" : "Code cannot be added");
                    EN.InnerXml = StrEN.ToString();
                    data.AppendChild(EN);
                    StringBuilder StrCHS = new StringBuilder();
                    XmlElement CHS = doc.CreateElement("CHS");
                    string temp =Form1.translateEncodingByWord(CB_Modules.SelectedItem.ToString(), true).Trim();
                    StrCHS.AppendFormat("<![CDATA[<CodeKind><CodeKindId>{0}</CodeKindId><ScName>{1}</ScName><EnName>{6}</EnName><IsSystem>{3}</IsSystem><Increasedly>{4}</Increasedly><Remark>{5}</Remark><Code>{0}</Code><BelongsModule>{2}</BelongsModule><Flag>true</Flag></CodeKind>]]>", textCode.Text, textdescription.Text, temp, cbSystem.Checked.ToString().ToLower(), cb_increase.Checked.ToString().ToLower(), cb_increase.Checked == true ? "用户可新增代码项" : "用户不可新增代码项", textBox2.Text);
                    CHS.InnerXml = StrCHS.ToString();
                    data.AppendChild(CHS);
                    StringBuilder StrCHT = new StringBuilder();
                    XmlElement CHT = doc.CreateElement("CHT");
                    StrCHT.AppendFormat("<![CDATA[<CodeKind><CodeKindId>{0}</CodeKindId><ScName>{1}</ScName><EnName>{6}</EnName><IsSystem>{3}</IsSystem><Increasedly>{4}</Increasedly><Remark>{5}</Remark><Code>{0}</Code><BelongsModule>{2}</BelongsModule><Flag>true</Flag></CodeKind>]]>", textCode.Text, textdescription.Text, CB_Modules.SelectedItem.ToString(), cbSystem.Checked.ToString().ToLower(), cb_increase.Checked.ToString().ToLower(), cb_increase.Checked == true ? "用戶可新增代碼項" : "用戶不可新增代碼項", textBox2.Text);
                    CHT.InnerXml = StrCHT.ToString();
                    data.AppendChild(CHT);
                    CodeKind.AppendChild(data);     
                    #endregion
                    
                    #region 建立資料明細
                    int count = 1;
                    foreach (DataGridViewRow dv in dataGridView1.Rows)
                    {
                        if (dv.Cells[1].Value != null)
                        {
                            XmlElement Infodata = doc.CreateElement("data");
                            string InfoId = textBox2.Text+"_"+ count.ToString("000");
                            Infodata.SetAttribute("name", "CodeInfo_" + InfoId);
                            Infodata.SetAttribute("description", dv.Cells[0].Value.ToString());
                            StringBuilder InfoStrEN = new StringBuilder();
                            InfoStrEN.AppendFormat("<![CDATA[<CodeInfo><CodeInfoId>{0}</CodeInfoId><KindCode>{1}</KindCode><KindName>{2}</KindName><InfoCode>{3}</InfoCode><ScName>{4}</ScName><IsSystem>{5}</IsSystem><Flag>true</Flag><Enabled>{6}</Enabled></CodeInfo>]]>",
                                InfoId, textCode.Text, textBox2.Text, count.ToString("000"), dv.Cells[1].Value.ToString(), cbSystem.Checked.ToString().ToLower(), dv.Cells[2].Value == null ? false.ToString() : dv.Cells[2].Value.ToString().ToLower());
                            XmlElement InfoEN = doc.CreateElement("EN");
                            InfoEN.InnerXml = InfoStrEN.ToString();
                            Infodata.AppendChild(InfoEN);
                            StringBuilder InfoStrCHS = new StringBuilder();
                            string Ctemp = Form1.translateEncodingByWord(dv.Cells[0].Value.ToString(), true).Trim();
                            InfoStrCHS.AppendFormat("<![CDATA[<CodeInfo><CodeInfoId>{0}</CodeInfoId><KindCode>{1}</KindCode><KindName>{2}</KindName><InfoCode>{3}</InfoCode><ScName>{4}</ScName><IsSystem>{5}</IsSystem><Flag>true</Flag><Enabled>{6}</Enabled></CodeInfo>]]>",
                                InfoId, textCode.Text, textBox2.Text, count.ToString("000"), Ctemp, cbSystem.Checked.ToString().ToLower(), dv.Cells[2].Value == null ? false.ToString() : dv.Cells[2].Value.ToString().ToLower());
                            XmlElement InfoCHS = doc.CreateElement("CHS");
                            InfoCHS.InnerXml = InfoStrCHS.ToString();
                            Infodata.AppendChild(InfoCHS);
                            StringBuilder InfoStrCHT = new StringBuilder();
                            InfoStrCHT.AppendFormat("<![CDATA[<CodeInfo><CodeInfoId>{0}</CodeInfoId><KindCode>{1}</KindCode><KindName>{2}</KindName><InfoCode>{3}</InfoCode><ScName>{4}</ScName><IsSystem>{5}</IsSystem><Flag>true</Flag><Enabled>{6}</Enabled></CodeInfo>]]>",
                                InfoId, textCode.Text, textBox2.Text, count.ToString("000"), dv.Cells[0].Value.ToString(), cbSystem.Checked.ToString().ToLower(), dv.Cells[2].Value == null ? false.ToString() : dv.Cells[2].Value.ToString().ToLower());
                            XmlElement InfoCHT = doc.CreateElement("CHT");
                            InfoCHT.InnerXml = InfoStrCHT.ToString();
                            Infodata.AppendChild(InfoCHT);
                            CodeInfo.AppendChild(Infodata);
                            count++;
                        }
                    }
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
                case "HumantrackManagerModule":         //人事跟蹤管理
                    result ="Personnel Tracking";
                    break;
                case "WorkTimeManagement":              //考勤時間管理
                    result = "Attendance Checking";
                    break;
                case "PayBoonManagement":               //薪資福利管理
                    result = "Payroll/Benefits";
                    break;
                case "TrainEmpolderManagement":         //培訓開發管理
                    result = "Training and Development";
                    break;
                case "PerformanceAssessManagement":     //績效考核管理
                    result = "Performance Appraisal";
                    break;
                case "DinnerManagement":                //員工就餐
                    result = "Employee Meals";
                    break;
                case "IeciuiuentSelectManagement":      //招募甄選管理
                    result = "Recruitment and Selection";
                    break;
                case "HumanResourcePlanModule":         //組織架構管理
                    result = "Organization Structure";
                    break;
                case "SystemSettingModule":             //系統配置管理
                    result = "System Layout";
                    break;
                case "ResourceApplyManagement":         //資源申領管理
                    result = "Resources Request Management";
                    break;
                case "MyWorkPlaceModule":               //我的主工作臺
                    result = "My Main Dashboard";
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
            }
            if (DataEntityCodeKind == null)
            {
                CreateNew(DefaultDataForCasePath, "CodeKind");
            }
            if (DataEntityCodeInfo == null)
            {
                CreateNew(DefaultDataForCasePath, "CodeInfo");
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
    }
}
