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
using System.Net;
using Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Web;
using System.Diagnostics;
using System.Threading;

namespace CreateXML {
    public partial class Form1 : Form {

        #region private Member
        private StringBuilder _strUsing = new StringBuilder();
        private StringBuilder _nameSpace = new StringBuilder();
        private StringBuilder _class = new StringBuilder();
        private StringBuilder _classEnd = new StringBuilder();
        private StringBuilder _privateparameter = new StringBuilder();
        private StringBuilder _parameter = new StringBuilder();
        private string _xmlpath;
        private StringBuilder _stringBuilder = new StringBuilder();  //多語系儲存
        
        private int DetailTabBefore = 0; //20141002 add by Dick 明細ID
        private int DetailTabNew = 0;    //20141002 add by Dick 明細ID

        public string Xmlpath {
            get { return _xmlpath; }
            set { _xmlpath = value; }
        }

        private string _projectPath;

        public string ProjectPath {
            get { return _projectPath; }
            set { _projectPath = value; }
        }


        private Dictionary<string, List<string[]>> _DicCheckBox = new Dictionary<string, List<string[]>>();
        private System.Data.DataTable RefereceTable = new System.Data.DataTable();
        private Dictionary<string, string> RefereceDic = new Dictionary<string, string>();
        private Dictionary<string, QueryViewCondition> DicQueryView = new Dictionary<string, QueryViewCondition>(); //QueryView的字典

        //20141224 
        Dictionary<string, List<string>> Modules = new Dictionary<string, List<string>>();
        Dictionary<string, string> EnToCHT = new Dictionary<string, string>();
        Dictionary<string, string> CHTToEn = new Dictionary<string, string>();
        #endregion


        #region  Private Method
        /// <summary>
        /// 建立using部分
        /// </summary>
        private void CreateUsing() {
            _strUsing.Append("using System;\r\n");
            _strUsing.Append("using Dcms.Common.Torridity;\r\n");
            _strUsing.Append("using Dcms.Common.Torridity.Metadata;\r\n");
            _strUsing.Append("using System.ComponentModel;\r\n");
            _strUsing.Append("using System.Threading;\r\n");
            _strUsing.Append("using Dcms.Common.DataEntities;\r\n");
            _strUsing.Append("using Dcms.Common;\r\n");
        }

        /// <summary>
        /// 建立NameSpace 和描述
        /// </summary>
        private void CreateNameSpace(string ClasseName,string Description) {
            _nameSpace.Append("namespace Dcms.HR.DataEntities \r\n" +
            "{\r\n" +
            "    /// <summary>\r\n" +
            "    ///" + Description + "\r\n" +
            "    /// </summary>\r\n" +
            "    [DataEntity(PrimaryKey = \"" + ClasseName + "Id\")]\r\n" +
            "    [Serializable()]\r\n" +
            "    [Description(\"" + Description + "\")]\r\n");
        }

        /// <summary>
        /// 建立Class 開頭
        /// </summary>
        private void CreateClass(string ClassName) {
            _class.Append("    public class ");
            _class.Append(ClassName);
            _class.Append(" : ");
            if(cb_DataEntity.Checked) {
                _class.Append(cb_DataEntity.Text);
                _class.Append(",");
            }
            if(cb_IAuditObject.Checked) {
                _class.Append(cb_IAuditObject.Text);
                _class.Append(",");
            }
            if(cb_ICodeObject.Checked) {
                _class.Append(cb_ICodeObject.Text);
                _class.Append(",");
            }
            if(cb_IDataModifyObject.Checked) {
                _class.Append(cb_IDataModifyObject.Text);
                _class.Append(",");
            }
            if(cb_IFlagObject.Checked) {
                _class.Append(cb_IFlagObject.Text);
                _class.Append(",");
            }
            if(cb_INamedObject.Checked) {
                _class.Append(cb_INamedObject.Text);
                _class.Append(",");
            }
            if(cb_IOwnerObject.Checked) {
                _class.Append(cb_IOwnerObject.Text);
                _class.Append(",");
            }
            string temp = _class.ToString().Substring(0, _class.Length - 1);
            _class.Clear();
            _class.Append(temp);
            _class.Append("{\r\n");
            _class.Append("        public const string TYPE_KEY = \"" + ClassName + "\";\r\n");
        }

        private void EndClass() {
            _classEnd.Append("}");
        }

        /// <summary>
        /// 實體的參數PrivaterParameter   
        /// </summary>
        /// <param name="dataGrid">給予要使用的GridView</param>
        private void AddPrivateParameter(DataGridView dataGrid) {
            foreach (DataGridViewRow dr in dataGrid.Rows)
            {
                if(dr.Cells["Parameter"].Value != null) {
                    _privateparameter.Append("        private ");
                    string type =TypeSwap(dr.Cells["Type"].Value.ToString());
                    #region 20140925 add by Dick 加入Image型態
                    if(type != "Image") {
                        _privateparameter.Append(" System." + type);
                    }
                    else {
                        _privateparameter.Append(" byte[]");
                    }
                    #endregion                  
                    _privateparameter.Append(" _");
                    string temp = dr.Cells["Parameter"].Value.ToString();
                    _privateparameter.Append(temp.Substring(0, 1).ToLower());
                    _privateparameter.Append(temp.Substring(1, temp.Length - 1));
                    _privateparameter.Append(";\r\n");
                    AddParameter(temp, TypeSwap(dr.Cells["Type"].Value.ToString()), dr.Cells["ReferenceProperty"].Value.ToString(), dr.Cells["Describe"].Value.ToString());
                }
            }
        }

        /// <summary>
        /// 加入實際參數
        /// </summary>
        /// <param name="Parameter"></param>
        /// <param name="Type"></param>
        /// <param name="SampleOrReference"></param>
        private void AddParameter(string Parameter, string Type, string Reference, string Describe) {
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        ///");
            _parameter.Append(Describe);
            _parameter.Append("\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [");
            if(!string.IsNullOrEmpty(Reference) & Reference != "False") {
                _parameter.Append("ReferenceProperty(\"" + Reference + "\",");
            }
            else {
                _parameter.Append("SimpleProperty(");
            }
            _parameter.Append("DbType = GeneralDbType.");
            _parameter.Append(Type);
            _parameter.Append(")]\r\n");
            _parameter.Append("        [Description(\"");
            _parameter.Append(Describe + "\")]\r\n");
            #region 20140925 add by Dick 加入Image型態
            if(Type.Equals("Image")) {
                _parameter.Append("        public byte[] " + Parameter + "{\r\n");
            }
            else {
                _parameter.Append("        public System." + Type + " " + Parameter + "{\r\n");
            }
            #endregion            
            _parameter.Append("            get { \r\n");
            string temp = "_" + Parameter.Substring(0, 1).ToLower() + Parameter.Substring(1, Parameter.Length - 1);
            _parameter.Append("                return this." + temp + ";}\r\n");
            _parameter.Append("            set{ \r\n");
            _parameter.Append("                if ((" + temp + " != value))  {\r\n");
            _parameter.Append("                    this." + temp + " = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"" + Parameter.Substring(0, 1).ToUpper() + Parameter.Substring(1, Parameter.Length - 1) + "\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
        }


        private void CheckCheckBox() {
            if(cb_IAuditObject.Checked) {
                //AddAuditObject();
            }
            if(cb_IOwnerObject.Checked) {
                //AddOwnerObject();
            }
            if(cb_ICodeObject.Checked) {
                //AddCode();
            }
            if(cb_IDataModifyObject.Checked) {
                //AdddModify();
            }
            if(cb_IFlagObject.Checked) {
                //AddFlag();
            }
            if(cb_INamedObject.Checked) {
                //AddName();
            }
            ///20141028 modified by Dick for 修改成自動生成判斷
            //if(cb_Collection.Checked) {
            //    AddCollection();
            //}
        }

        /// <summary>
        /// 加入集合Collection
        /// </summary>
        private void AddCollection(string ClassName,string Description) {
            string temp = "_" + ClassName.Substring(0, 1).ToLower() + ClassName.Substring(1, ClassName.Length - 1);
            _parameter.Append(" #region Collection properties\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 返回 " + Description + "明細集合\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        private " + ClassName + "Collection " + temp + "Infos;\r\n");
            _parameter.Append("        [CollectionProperty(typeof(" + ClassName + "), Alias = \"" + ClassName + "\")]\r\n");
            _parameter.Append("        [Description(\"" + Description + "明細集合\")]\r\n");
            _parameter.Append("        public " + ClassName + "Collection " + ClassName + "Infos {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                if ((" + temp + "Infos == null)) {\r\n");
            _parameter.Append("                    System.Threading.Monitor.Enter(this);\r\n");
            _parameter.Append("                    if ((" + temp + "Infos == null)) {\r\n");
            _parameter.Append("                        this." + temp + "Infos = new " + ClassName + "Collection(this);\r\n");
            _parameter.Append("                    }\r\n");
            _parameter.Append("                    System.Threading.Monitor.Exit(this);\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("                return this." + temp + "Infos;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("        #endregion\r\n");
        }


        #region 產生固定選項函式

        /// <summary>
        /// 產生AddAuditObject
        /// 20140103 舊的產生審核方法
        /// </summary>
        private void AddAuditObject() {

            _parameter.Append(" #region  IAuditObject\r\n");
            _privateparameter.Append("        private System.Guid _approveEmployeeId;\r\n");
            _privateparameter.Append("\r\n");
            _privateparameter.Append("        private string _approveEmployeeName;\r\n");
            _privateparameter.Append("\r\n");
            _privateparameter.Append("        private string _approveRemark;\r\n");
            _privateparameter.Append("\r\n");
            _privateparameter.Append("        private System.DateTime _approveOperationDate;\r\n");
            _privateparameter.Append("\r\n");
            _privateparameter.Append("        private System.Guid _approveUserId;\r\n");
            _privateparameter.Append("\r\n");
            _privateparameter.Append("        private System.DateTime _repealOperationDate;\r\n");
            _privateparameter.Append("\r\n");
            _privateparameter.Append("        private System.Guid _repealUserId;\r\n");
            _privateparameter.Append("\r\n");
            _privateparameter.Append("        private string _approveResultId;\r\n");
            _privateparameter.Append("        private System.Guid _submitUserId;\r\n");
            _privateparameter.Append("        private System.DateTime _submitOperationDate;\r\n");
            _privateparameter.Append("        private string _stateId;\r\n");
            _privateparameter.Append("        private System.DateTime _approveDate;\r\n");
            _privateparameter.Append("        private System.DateTime _confirmOperationDate;\r\n");
            _privateparameter.Append("        private System.Guid _confirmUserId;\r\n");
            _privateparameter.Append("        private System.Guid _foundUserId;\r\n");
            _privateparameter.Append("        private System.DateTime _foundOperationDate;\r\n");




            _parameter.Append(" /// <summary>\r\n");
            _parameter.Append("        /// 返回/設定 審核人ID\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [ReferenceProperty(\"Employee\", DbType = GeneralDbType.Guid)]\r\n");
            _parameter.Append("        [Description(\"審核人\")]\r\n");
            _parameter.Append("        public System.Guid ApproveEmployeeId {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                return this._approveEmployeeId;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set {\r\n");
            _parameter.Append("                if ((_approveEmployeeId != value)) {\r\n");
            _parameter.Append("                    this._approveEmployeeId = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"ApproveEmployeeId\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 返回/設定 審核人名稱\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [SimpleProperty(DbType = GeneralDbType.String)]\r\n");
            _parameter.Append("        [Description(\"審核人名稱\")]\r\n");
            _parameter.Append("        public string ApproveEmployeeName {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                return this._approveEmployeeName;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set {\r\n");
            _parameter.Append("                if ((_approveEmployeeName != value)) {\r\n");
            _parameter.Append("                    this._approveEmployeeName = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"ApproveEmployeeName\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 返回/設定 審核批註\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [SimpleProperty(DbType = GeneralDbType.Text)]\r\n");
            _parameter.Append("        [Description(\"審核批註\")]\r\n");
            _parameter.Append("        public string ApproveRemark {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                return this._approveRemark;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set {\r\n");
            _parameter.Append("                if ((_approveRemark != value)) {\r\n");
            _parameter.Append("                    this._approveRemark = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"ApproveRemark\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 返回/設定 審核人操作日期\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [SimpleProperty(DbType = GeneralDbType.DateTime)]\r\n");
            _parameter.Append("        [Description(\"審核人操作日期\")]\r\n");
            _parameter.Append("        public System.DateTime ApproveOperationDate {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                return this._approveOperationDate;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set {\r\n");
            _parameter.Append("                if ((_approveOperationDate != value)) {\r\n");
            _parameter.Append("                    this._approveOperationDate = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"ApproveOperationDate\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 返回/設定 審核操作人ID\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [ReferenceProperty(\"User\", DbType = GeneralDbType.Guid)]\r\n");
            _parameter.Append("        [Description(\"審核操作人ID\")]\r\n");
            _parameter.Append("        public System.Guid ApproveUserId {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                return this._approveUserId;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set {\r\n");
            _parameter.Append("                if ((_approveUserId != value)) {\r\n");
            _parameter.Append("                    this._approveUserId = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"ApproveUserId\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 返回/設定 撤銷操作日期\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [SimpleProperty(DbType = GeneralDbType.DateTime)]\r\n");
            _parameter.Append("        [Description(\"撤銷操作日期\")]\r\n");
            _parameter.Append("        public System.DateTime RepealOperationDate {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                return this._repealOperationDate;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set {\r\n");
            _parameter.Append("                if ((_repealOperationDate != value)) {\r\n");
            _parameter.Append("                    this._repealOperationDate = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"RepealOperationDate\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 返回/設定 撤銷操作人ID\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [ReferenceProperty(\"User\", DbType = GeneralDbType.Guid)]\r\n");
            _parameter.Append("        [Description(\"撤銷操作人ID\")]\r\n");
            _parameter.Append("        public System.Guid RepealUserId {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                return this._repealUserId;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set {\r\n");
            _parameter.Append("                if ((_repealUserId != value)) {\r\n");
            _parameter.Append("                    this._repealUserId = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"RepealUserId\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 返回/設定 審核結果ID\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [ReferenceProperty(\"CodeInfo\", DbType = GeneralDbType.String)]\r\n");
            _parameter.Append("        [Description(\"審核結果ID\")]\r\n");
            _parameter.Append("        public string ApproveResultId {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                return this._approveResultId;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set {\r\n");
            _parameter.Append("                if ((_approveResultId != value)) {\r\n");
            _parameter.Append("                    this._approveResultId = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"ApproveResultId\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 返回/設定 提交操作人ID\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [ReferenceProperty(\"User\", DbType = GeneralDbType.Guid)]\r\n");
            _parameter.Append("        [Description(\"提交操作人ID\")]\r\n");
            _parameter.Append("        public System.Guid SubmitUserId {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                return this._submitUserId;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set {\r\n");
            _parameter.Append("                if ((_submitUserId != value)) {\r\n");
            _parameter.Append("                    this._submitUserId = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"SubmitUserId\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 返回/設定 提交操作日期\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [SimpleProperty(DbType = GeneralDbType.DateTime)]\r\n");
            _parameter.Append("        [Description(\"提交操作日期\")]\r\n");
            _parameter.Append("        public System.DateTime SubmitOperationDate {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                return this._submitOperationDate;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set {\r\n");
            _parameter.Append("                if ((_submitOperationDate != value)) {\r\n");
            _parameter.Append("                    this._submitOperationDate = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"SubmitOperationDate\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 返回/設定 狀態ID\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [ReferenceProperty(\"CodeInfo\", DbType = GeneralDbType.String)]\r\n");
            _parameter.Append("        [Description(\"狀態ID\")]\r\n");
            _parameter.Append("        [Index()]\r\n");
            _parameter.Append("        public string StateId {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                return this._stateId;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set {\r\n");
            _parameter.Append("                if ((_stateId != value)) {\r\n");
            _parameter.Append("                    this._stateId = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"StateId\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        ///返回/設定 審核日期\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [SimpleProperty(DbType = GeneralDbType.DateTime)]\r\n");
            _parameter.Append("        [Description(\"審核日期\")]\r\n");
            _parameter.Append("        public System.DateTime ApproveDate {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                return this._approveDate;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set {\r\n");
            _parameter.Append("                if ((_approveDate != value)) {\r\n");
            _parameter.Append("                    this._approveDate = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"ApproveDate\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 返回/設定 歸檔操作日期\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [SimpleProperty(DbType = GeneralDbType.DateTime)]\r\n");
            _parameter.Append("        [Description(\"歸檔操作日期\")]\r\n");
            _parameter.Append("        public System.DateTime ConfirmOperationDate {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                return this._confirmOperationDate;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set {\r\n");
            _parameter.Append("                if ((_confirmOperationDate != value)) {\r\n");
            _parameter.Append("                    this._confirmOperationDate = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"ConfirmOperationDate\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 返回/設定 歸檔操作人ID\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [ReferenceProperty(\"User\", DbType = GeneralDbType.Guid)]\r\n");
            _parameter.Append("        [Description(\"歸檔操作人ID\")]\r\n");
            _parameter.Append("        public System.Guid ConfirmUserId {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                return this._confirmUserId;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set {\r\n");
            _parameter.Append("                if ((_confirmUserId != value)) {\r\n");
            _parameter.Append("                    this._confirmUserId = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"ConfirmUserId\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 返回/設定 制定操作日期\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [SimpleProperty(DbType = GeneralDbType.DateTime)]\r\n");
            _parameter.Append("        [Description(\"制定操作日期\")]\r\n");
            _parameter.Append("        public System.DateTime FoundOperationDate {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                return this._foundOperationDate;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set {\r\n");
            _parameter.Append("                if ((_foundOperationDate != value)) {\r\n");
            _parameter.Append("                    this._foundOperationDate = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"FoundOperationDate\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 返回/設定 制定操作人ID\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [ReferenceProperty(\"User\", DbType = GeneralDbType.Guid)]\r\n");
            _parameter.Append("        [Description(\"制定操作人ID\")]\r\n");
            _parameter.Append("        public System.Guid FoundUserId {\r\n");
            _parameter.Append("            get {\r\n");
            _parameter.Append("                return this._foundUserId;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set {\r\n");
            _parameter.Append("                if ((_foundUserId != value)) {\r\n");
            _parameter.Append("                    this._foundUserId = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"FoundUserId\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        #endregion\r\n");

        }

        /// <summary>
        /// 產生 Flag
        /// </summary>
        private void AddFlag() {
            _privateparameter.Append("private bool _flag;\r\n");

            _parameter.Append("  /// <summary>\r\n");
            _parameter.Append("        /// 是否有效\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [SimpleProperty(DbType = GeneralDbType.Boolean)]\r\n");
            _parameter.Append("        public bool Flag\r\n");
            _parameter.Append("        {\r\n");
            _parameter.Append("            get\r\n");
            _parameter.Append("            {\r\n");
            _parameter.Append("                return this._flag;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set\r\n");
            _parameter.Append("            {\r\n");
            _parameter.Append("                if ((_flag != value))\r\n");
            _parameter.Append("                {\r\n");
            _parameter.Append("                    this._flag = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"Flag\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");


        }
        /// <summary>
        /// 產生 Ownerobject  實作
        /// </summary>
        private void AddOwnerObject() {
            _privateparameter.Append("#region   IOwnerObject 成员 \r\n");
            _privateparameter.Append("private string _assignReason;\r\n");
            _privateparameter.Append("private string _ownerId; \r\n");
            _privateparameter.Append("  /// <summary>\r\n");
            _privateparameter.Append("        /// 分配原因\r\n");
            _privateparameter.Append("        /// </summary>\r\n");
            _privateparameter.Append("        [SimpleProperty(DbType = GeneralDbType.String)]\r\n");
            _privateparameter.Append("        public string AssignReason\r\n");
            _privateparameter.Append("        {\r\n");
            _privateparameter.Append("            get\r\n");
            _privateparameter.Append("            {\r\n");
            _privateparameter.Append("                return this._assignReason;\r\n");
            _privateparameter.Append("            }\r\n");
            _privateparameter.Append("            set\r\n");
            _privateparameter.Append("            {\r\n");
            _privateparameter.Append("                if ((_assignReason != value))\r\n");
            _privateparameter.Append("                {\r\n");
            _privateparameter.Append("                    this._assignReason = value;\r\n");
            _privateparameter.Append("                    this.OnPropertyChanged(\"AssignReason\");\r\n");
            _privateparameter.Append("                }\r\n");
            _privateparameter.Append("            }\r\n");
            _privateparameter.Append("        }\r\n");


            _privateparameter.Append("/// <summary>\r\n");
            _privateparameter.Append("        /// 所有者ID\r\n");
            _privateparameter.Append("        /// </summary>\r\n");
            _privateparameter.Append("         [SimpleProperty(DbType = GeneralDbType.String)]\r\n");
            _privateparameter.Append("        public string OwnerId\r\n");
            _privateparameter.Append("        {\r\n");
            _privateparameter.Append("            get\r\n");
            _privateparameter.Append("            {\r\n");
            _privateparameter.Append("                return this._ownerId;\r\n");
            _privateparameter.Append("            }\r\n");
            _privateparameter.Append("            set\r\n");
            _privateparameter.Append("            {\r\n");
            _privateparameter.Append("                if ((_ownerId != value))\r\n");
            _privateparameter.Append("                {\r\n");
            _privateparameter.Append("                    this._ownerId = value;\r\n");
            _privateparameter.Append("                    this.OnPropertyChanged(\"OwnerId\");\r\n");
            _privateparameter.Append("                }\r\n");
            _privateparameter.Append("            }\r\n");
            _privateparameter.Append("        }\r\n");
            _privateparameter.Append(" #endregion\r\n");
        }


        /// <summary>
        /// 產生Code 實作
        /// </summary>
        private void AddCode() {
            _privateparameter.Append("private string _code; \r\n");
            _parameter.Append("    /// <summary>\r\n");
            _parameter.Append("        ///" + tb_scrib.Text + "\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [SimpleProperty(DbType = GeneralDbType.String)]\r\n");
            _parameter.Append("        [Description(\"" + tb_scrib.Text + "編碼\")]\r\n");
            _parameter.Append("        public string Code\r\n");
            _parameter.Append("        {\r\n");
            _parameter.Append("            get\r\n");
            _parameter.Append("            {\r\n");
            _parameter.Append("                return this._code;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set\r\n");
            _parameter.Append("            {\r\n");
            _parameter.Append("                if ((_code != value))\r\n");
            _parameter.Append("                {\r\n");
            _parameter.Append("                    this._code = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"Code\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
        }



        /// <summary>
        /// 產生Modify實作
        /// </summary>
        private void AdddModify() {
            _privateparameter.Append("private System.DateTime _createDate;\r\n");
            _privateparameter.Append("        private System.DateTime _lastModifiedDate;\r\n");
            _privateparameter.Append("        private System.Guid _createBy;\r\n");
            _privateparameter.Append("        private System.Guid _lastModifiedBy;\r\n");


            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 建立日期\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [SimpleProperty(DbType = GeneralDbType.DateTime)]\r\n");
            _parameter.Append("        public System.DateTime CreateDate\r\n");
            _parameter.Append("        {\r\n");
            _parameter.Append("            get\r\n");
            _parameter.Append("            {\r\n");
            _parameter.Append("                return this._createDate;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set\r\n");
            _parameter.Append("            {\r\n");
            _parameter.Append("                if ((_createDate != value))\r\n");
            _parameter.Append("                {\r\n");
            _parameter.Append("                    this._createDate = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"CreateDate\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 最後一次編輯日期\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [SimpleProperty(DbType = GeneralDbType.DateTime)]\r\n");
            _parameter.Append("        public System.DateTime LastModifiedDate\r\n");
            _parameter.Append("        {\r\n");
            _parameter.Append("            get\r\n");
            _parameter.Append("            {\r\n");
            _parameter.Append("                return this._lastModifiedDate;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set\r\n");
            _parameter.Append("            {\r\n");
            _parameter.Append("                if ((_lastModifiedDate != value))\r\n");
            _parameter.Append("                {\r\n");
            _parameter.Append("                    this._lastModifiedDate = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"LastModifiedDate\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 建立 用戶ID\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [ReferenceProperty(\"CostCenter\", DbType = GeneralDbType.Guid)]\r\n");
            _parameter.Append("        public System.Guid CreateBy\r\n");
            _parameter.Append("        {\r\n");
            _parameter.Append("            get\r\n");
            _parameter.Append("            {\r\n");
            _parameter.Append("                return this._createBy;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set\r\n");
            _parameter.Append("            {\r\n");
            _parameter.Append("                if ((_createBy != value))\r\n");
            _parameter.Append("                {\r\n");
            _parameter.Append("                    this._createBy = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"CreateBy\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append("        /// <summary>\r\n");
            _parameter.Append("        /// 最後一次編輯 用戶ID\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [ReferenceProperty(\"Employee\", DbType = GeneralDbType.Guid)]\r\n");
            _parameter.Append("        public System.Guid LastModifiedBy\r\n");
            _parameter.Append("        {\r\n");
            _parameter.Append("            get\r\n");
            _parameter.Append("            {\r\n");
            _parameter.Append("                return this._lastModifiedBy;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set\r\n");
            _parameter.Append("            {\r\n");
            _parameter.Append("                if ((_lastModifiedBy != value))\r\n");
            _parameter.Append("                {\r\n");
            _parameter.Append("                    this._lastModifiedBy = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"LastModifiedBy\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");


        }

        /// <summary>
        /// 產生Name
        /// </summary>
        private void AddName() {
            _privateparameter.Append(" private string _name;\r\n");

            _parameter.Append(" /// <summary>\r\n");
            _parameter.Append("        /// " + tb_scrib.Text + "名稱\r\n");
            _parameter.Append("        /// </summary>\r\n");
            _parameter.Append("        [SimpleProperty(DbType = GeneralDbType.String)]\r\n");
            _parameter.Append("        [Description(\"" + tb_scrib.Text + "名稱\")]\r\n");
            _parameter.Append("        public string Name\r\n");
            _parameter.Append("        {\r\n");
            _parameter.Append("            get\r\n");
            _parameter.Append("            {\r\n");
            _parameter.Append("                return this._name;\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("            set\r\n");
            _parameter.Append("            {\r\n");
            _parameter.Append("                if ((_name != value))\r\n");
            _parameter.Append("                {\r\n");
            _parameter.Append("                    this._name = value;\r\n");
            _parameter.Append("                    this.OnPropertyChanged(\"Name\");\r\n");
            _parameter.Append("                }\r\n");
            _parameter.Append("            }\r\n");
            _parameter.Append("        }\r\n");
            _parameter.Append("\r\n");
            _parameter.Append(" \r\n");

        }

        #endregion









        #endregion





        public Form1() {
            InitializeComponent();
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Parameter");
            //dt.Columns.Add("Type");
            dt.Columns.Add("Describe");
            dt.Columns.Add("ReferenceProperty");
            dt.Columns.Add("Necessary", typeof(bool));
            dt.Columns.Add("Order");
            //20140818 add by Dick
            dt.Columns.Add("UIOrder");
           
           

            RefereceTable.Columns.Add("Entity");



            dataGridView1.DataSource = dt;

            GridViewCellType(dataGridView1);

            System.Data.DataTable dtt11 = dataGridView1.DataSource as System.Data.DataTable;
            Xmlpath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "SaveFile.xml";
            XmlDocument doc = FileTool.XmlFile.LoadXml(Xmlpath);
            XmlNode root = doc.SelectSingleNode("root");
            foreach(XmlNode node in root.ChildNodes) {
                string ClassName = node.Attributes["ClassName"].Value.ToString();
                #region 20140827 modified by Dick for 修改成ComboBox
                EnitiesComboBox.Items.Add(ClassName);              
                //ToolStripMenuItem item = new ToolStripMenuItem(ClassName);
                //item.Click += new EventHandler(item_Click);               
                //載入上次作業ToolStripMenuItem.DropDownItems.Add(item);
                #endregion
                foreach(XmlNode child in node.ChildNodes) {
                    string temp = child.Attributes["ReferenceProperty"].Value;
                    if(!string.IsNullOrEmpty(temp)) {
                        RefereceDic[temp] = temp;
                    }
                }
            }
            ProjectPath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Path.xml";
            XmlDocument doc2 = FileTool.XmlFile.LoadXml(ProjectPath);
            XmlNode root2 = doc2.SelectSingleNode("root");
            foreach(XmlNode node in root2.ChildNodes) {
                if(node.Attributes["Set"].Value.ToString() == "true") {
                    ProjectPath = node.Attributes["Xpath"].Value.ToString();
                }
            }
            ProjectPath += @"\Dcms.HR.Support\Business.Implement\Resources\Dcms.HR.Business.Implement.Resources\Properties\";

            foreach(string str in RefereceDic.Values) {
                DataRow dr = RefereceTable.NewRow();
                dr[0] = str;
                RefereceTable.Rows.Add(dr);
            }
                    
            InitIAuditObject();
            InitIFlag();
            InitOwnerObject();
            InitCode();
            InitModify();
            InitName();

            ///加入條件視窗
            dataGridView1.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            var item = dataGridView1.ContextMenuStrip.Items.Add("加入條件");
            item.Click += new EventHandler(item_Click1);
            #region 20141029 add by Dick for 預設Browse
            QueryViewCondition QueryView =new QueryViewCondition();
            QueryView.BrowseName = "Browse";
            QueryView.Description = "瀏覽";
            QueryView.Type = "Browse";
            DicQueryView["Browse"] = QueryView;
            #endregion


            #region 20141224 add by Dick for 加入模組下拉方式           
            string ProgamPath = GetSettinhPath();
            DirectoryInfo DirInfo = new DirectoryInfo(ProgamPath);
            string Permission = DirInfo.FullName + Path.DirectorySeparatorChar + "Configuration" + Path.DirectorySeparatorChar + "Permission.xml";
            Modules = GetModules(Permission);
            string ModuleResource = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "ModResource.xml";
            XmlDocument docResource = FileTool.XmlFile.LoadXml(ModuleResource);
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


            #endregion
        }

        /// <summary>
        /// 20141224 add by Dick 取得模組架構 #6
        /// </summary>
        /// <param name="pXmlPath"></param>
        /// <returns></returns>
        private Dictionary<string, List<string>> GetModules(string pXmlPath)
        {
            Dictionary<string, List<string>> DicModules = new Dictionary<string, List<string>>();
            XmlDocument doc = FileTool.XmlFile.LoadXml(pXmlPath);
            XmlNode root = doc.ChildNodes[1];
            StringBuilder SB = new StringBuilder();
            FileTool.XmlFile XmlFile = new FileTool.XmlFile();
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


        /// <summary>
        /// 20141006 modified by Dick 
        /// </summary>
        /// <param name="Grid"></param>
        public void GridViewCellType(DataGridView Grid) {
            ///20140303 add by Dick  修改成下拉選單
            DataGridViewComboBoxColumn comboboxColumn = new DataGridViewComboBoxColumn();
            comboboxColumn.Name = "Type";
            System.Data.DataTable dtt = new System.Data.DataTable();
            dtt.Columns.Add("ID");
            dtt.Columns.Add("Item");
            InitType(ref dtt);
            comboboxColumn.DataSource = dtt;
            comboboxColumn.ValueMember = "Item";
            comboboxColumn.DisplayMember = "Item";
            Grid.Columns.Insert(1, comboboxColumn);
        }
     

        private void InitType(ref System.Data.DataTable dtt) {
            DataRow drr = dtt.NewRow();
            drr[0] = "1";
            drr[1] = "String";
            dtt.Rows.Add(drr);

            DataRow drr2 = dtt.NewRow();
            drr2[0] = "2";
            drr2[1] = "Int32";
            dtt.Rows.Add(drr2);

            DataRow drr3 = dtt.NewRow();
            drr3[0] = "3";
            drr3[1] = "Decimal";
            dtt.Rows.Add(drr3);

            DataRow drr4 = dtt.NewRow();
            drr4[0] = "4";
            drr4[1] = "DateTime";
            dtt.Rows.Add(drr4);

            DataRow drr5 = dtt.NewRow();
            drr5[0] = "5";
            drr5[1] = "Ntext";
            dtt.Rows.Add(drr5);

            DataRow drr6 = dtt.NewRow();
            drr6[0] = "6";
            drr6[1] = "Guid";
            dtt.Rows.Add(drr6);

            DataRow drr7 = dtt.NewRow();
            drr7[0] = "7";
            drr7[1] = "Bool";
            dtt.Rows.Add(drr7);

            DataRow drr8 = dtt.NewRow();
            drr8[0] = "8";
            drr8[1] = "Int64";

            DataRow drr9 = dtt.NewRow();
            drr8[0] = "9";
            drr8[1] = "Image";

            dtt.Rows.Add(drr8);
        }

        #region 初始化控制項  20140103

        /// <summary>
        /// 審核以參數方式列出
        /// </summary>
        private void InitIAuditObject() {
            List<string[]> IAuditObjectList = new List<string[]>();
            string[] IAuditObjectApproveEmployeeId = new string[] { "ApproveEmployeeId", "Guid", "審核人", "Employee" };
            string[] IAuditObjectApproveEmployeeName = new string[] { "ApproveEmployeeName", "String", "審核人" };
            string[] IAuditObjectApproveRemark = new string[] { "ApproveRemark", "nText", "審核批註" };
            string[] IAuditObjectApproveOperationDate = new string[] { "ApproveOperationDate", "DateTime", "審核人操作日期" };
            string[] IAuditObjectApproveUserId = new string[] { "ApproveUserId", "Guid", "審核操作人ID", "User" };
            string[] IAuditObjectRepealOperationDate = new string[] { "RepealOperationDate", "DateTime", "撤銷操作日期" };
            string[] IAuditObjectRRepealUserId = new string[] { "RepealUserId", "Guid", "撤銷操作人ID", "User" };
            string[] IAuditObjectApproveResultId = new string[] { "ApproveResultId", "String", "審核結果ID" };
            string[] IAuditObjectSubmitUserId = new string[] { "SubmitUserId", "Guid", "提交操作人ID", "User" };
            string[] IAuditObjectSubmitOperationDate = new string[] { "SubmitOperationDate", "DateTime", "提交操作日期" };
            string[] IAuditObjectStateId = new string[] { "StateId", "String", "狀態ID", "CodeInfo" };
            string[] IAuditObjectApproveDate = new string[] { "ApproveDate", "DateTime", "審核日期" };
            string[] IAuditObjectConfirmOperationDate = new string[] { "ConfirmOperationDate", "DateTime", "歸檔操作日期" };
            string[] IAuditObjectConfirmUserId = new string[] { "ConfirmUserId", "Guid", "歸檔操作人ID", "User" };
            string[] IAuditObjectFoundOperationDate = new string[] { "FoundOperationDate", "DateTime", "制定操作日期" };
            string[] IAuditObjectFoundUserId = new string[] { "FoundUserId", "Guid", "制定操作人ID", "User" };
            IAuditObjectList.Add(IAuditObjectApproveEmployeeId);
            IAuditObjectList.Add(IAuditObjectApproveEmployeeName);
            IAuditObjectList.Add(IAuditObjectApproveRemark);
            IAuditObjectList.Add(IAuditObjectApproveOperationDate);
            IAuditObjectList.Add(IAuditObjectApproveUserId);
            IAuditObjectList.Add(IAuditObjectRepealOperationDate);
            IAuditObjectList.Add(IAuditObjectRRepealUserId);
            IAuditObjectList.Add(IAuditObjectApproveResultId);
            IAuditObjectList.Add(IAuditObjectSubmitUserId);
            IAuditObjectList.Add(IAuditObjectSubmitOperationDate);
            IAuditObjectList.Add(IAuditObjectStateId);
            IAuditObjectList.Add(IAuditObjectApproveDate);
            IAuditObjectList.Add(IAuditObjectConfirmOperationDate);
            IAuditObjectList.Add(IAuditObjectConfirmUserId);
            IAuditObjectList.Add(IAuditObjectFoundOperationDate);
            IAuditObjectList.Add(IAuditObjectFoundUserId);
            _DicCheckBox["IAuditObject"] = IAuditObjectList;

        }

        /// <summary>
        /// Flag 以參數方式列出
        /// </summary>
        private void InitIFlag() {
            List<string[]> IFlagList = new List<string[]>();
            string[] IFlagListFlag = new string[] { "Flag", "Bool", "是否有效" };
            IFlagList.Add(IFlagListFlag);
            _DicCheckBox["IFlagObject"] = IFlagList;

        }

        /// <summary>
        /// InitOwnerObject 以參數方式列出
        /// </summary>
        private void InitOwnerObject() {

            List<string[]> InitOwnerObject = new List<string[]>();
            string[] IOwnerObjectAssignReason = new string[] { "AssignReason", "String", "分配原因" };
            string[] IOwnerObjectOwnerId = new string[] { "OwnerId", "String", "所有者ID", "User" };
            InitOwnerObject.Add(IOwnerObjectAssignReason);
            InitOwnerObject.Add(IOwnerObjectOwnerId);
            _DicCheckBox["IOwnerObject"] = InitOwnerObject;

        }

        /// <summary>
        /// ICode 以參數方式列出
        /// </summary>
        private void InitCode() {
            List<string[]> ICodeList = new List<string[]>();
            string[] ICode = new string[] { "Code", "String", "編碼" };
            ICodeList.Add(ICode);
            _DicCheckBox["ICodeObject"] = ICodeList;
        }

        /// <summary>
        /// ICode 以參數方式列出
        /// </summary>
        private void InitModify() {
            List<string[]> IModifyList = new List<string[]>();
            string[] IModifyCreateDate = new string[] { "CreateDate", "DateTime", "建立日期" };
            string[] IModifyLastModifiedDate = new string[] { "LastModifiedDate", "DateTime", "最後一次編輯日期" };
            string[] IModifyCreateBy = new string[] { "CreateBy", "Guid", "建立 用戶ID", "User" };
            string[] IModifyLastModifiedBy = new string[] { "LastModifiedBy", "Guid", "最後一次編輯 用戶ID", "User" };
            IModifyList.Add(IModifyCreateDate);
            IModifyList.Add(IModifyLastModifiedDate);
            IModifyList.Add(IModifyCreateBy);
            IModifyList.Add(IModifyLastModifiedBy);
            _DicCheckBox["IDataModifyObject"] = IModifyList;
        }



        /// <summary>
        /// ICode 以參數方式列出
        /// </summary>
        private void InitName() {
            List<string[]> INameList = new List<string[]>();
            string[] IName = new string[] { "Name", "String", "名稱" };
            INameList.Add(IName);
            _DicCheckBox["INamedObject"] = INameList;
        }



        #endregion




        private void item_Click(object sender, EventArgs e) {
            //throw new NotImplementedException();
            //20140827 modified by Dick 修改成ComboBox
            //ToolStripMenuItem item = sender as ToolStripMenuItem;
            if(EnitiesComboBox.SelectedIndex != -1) {
                string text = EnitiesComboBox.SelectedItem.ToString();
                LoadGridView(text, dataGridView1, true);
            }
            //dataGridView1.Rows.RemoveAt(dataGridView1.ColumnCount-1);
        }

        private void LoadGridView(string NodeName,DataGridView GridView,bool IsMain)
        {
            XmlDocument doc = FileTool.XmlFile.LoadXml(Xmlpath);
            XmlNode root = doc.GetElementById(NodeName);
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Parameter");           
            //   dt.Columns.Add("Type", typeof(DataGridViewComboBoxColumn));
            dt.Columns.Add("Describe");
            dt.Columns.Add("ReferenceProperty");
            dt.Columns.Add("Necessary", typeof(bool));
            dt.Columns.Add("Order");
            //20140818 add by Dick 加入UI排位子順序
            dt.Columns.Add("UIOrder");
            if(!IsMain) {
                dt.Columns.Add("Type");
            }
            //XmlNode root = doc.GetElementById(item.Text);
            if (IsMain)
            {
             this.tb_className.Text = root.Attributes["ClassName"].Value.ToString();
            this.tb_scrib.Text = root.Attributes["Describle"].Value.ToString();           
            cb_IAuditObject.Checked = Convert.ToBoolean(root.Attributes["IAuditObject"].Value.ToString());
            cb_DataEntity.Checked = Convert.ToBoolean(root.Attributes["DataEntity"].Value.ToString());
            cb_INamedObject.Checked = Convert.ToBoolean(root.Attributes["INamedObject"].Value.ToString());
            cb_IFlagObject.Checked = Convert.ToBoolean(root.Attributes["IFlagObject"].Value.ToString());
            cb_IOwnerObject.Checked = Convert.ToBoolean(root.Attributes["IOwnerObject"].Value.ToString());
            cb_ICodeObject.Checked = Convert.ToBoolean(root.Attributes["ICodeObject"].Value.ToString());
            cb_IDataModifyObject.Checked = Convert.ToBoolean(root.Attributes["IDataModifyObject"].Value.ToString());
            cb_Collection.Checked = Convert.ToBoolean(root.Attributes["Collection"].Value.ToString());            
            try {
                cb_ForCase.Checked = Convert.ToBoolean(root.Attributes["IsCase"].Value.ToString());
            }
            catch { }


            }

            List<string> li = new List<string>();
            foreach (XmlNode node in root.ChildNodes)
            {
                DataRow dr = dt.NewRow();
                dr[0] = node.Attributes["Name"].Value.ToString();
                //comboboxColumn.DataPropertyName = node.Attributes["Type"].Value.ToString();              
                li.Add(node.Attributes["Type"].Value.ToString());
                //dr[1] = node.Attributes["Type"].Value.ToString();
                dr[1] = node.Attributes["Describe"].Value.ToString();
                dr[2] = node.Attributes["ReferenceProperty"].Value;

                //20140904 加入Order 及 UIOrder 及 Necessary
                dr["Order"] = node.Attributes["Order"] != null ? node.Attributes["Order"].Value.ToString() : null;
                dr["UIOrder"] = node.Attributes["UIOrder"] != null ? node.Attributes["UIOrder"].Value.ToString() : null;
                bool IsNecessary = false;
                if (node.Attributes["Necessary"] != null)
                {
                    bool.TryParse(node.Attributes["Necessary"].Value, out IsNecessary);
                }
                dr["Necessary"] = IsNecessary;
                dt.Rows.Add(dr);
            }
          
            GridView.DataSource = dt;
           
            int count = 0;
            foreach (string str in li)
            {
                if (GridView.Rows.Count > count)
                {
                    GridView.Rows[count].Cells["Type"].Value = str == "Int" ? "Int32" : str;                   
                    count++;
                }
            }
        }

        /// <summary>
        /// WordAPI 繁體轉簡體
        /// </summary>
        /// <param name="pStr"></param>
        /// <param name="pIsTraditional"></param>
        /// <returns></returns>
        public static string translateEncodingByWord(string pStr, bool pIsTraditional) {
            Document doc = new Document();
            doc.Content.Text = pStr;
            if(pIsTraditional)
                doc.Content.TCSCConverter(WdTCSCConverterDirection.wdTCSCConverterDirectionTCSC, true, true);
            else
                doc.Content.TCSCConverter(WdTCSCConverterDirection.wdTCSCConverterDirectionSCTC, true, true);
            string ret = doc.Content.Text;
            object saveChanges = false;
            object originalFormat = Missing.Value;
            object routeDocument = Missing.Value;
            doc.Close(ref saveChanges, ref originalFormat, ref routeDocument);
            return ret;
        }

        /// <summary>
        /// 中翻英
        /// </summary>
        /// <param name="strToTranslate"></param>
        /// <param name="fromLanguage"></param>
        /// <param name="toLanguage"></param>
        /// <returns></returns>
        public string translateString(string strToTranslate, string fromLanguage, string toLanguage) {
            string translatedStr = "";
            string transRetHtml = "";

            ////following refer: http://python.u85.us/viewnews-335.html
            //string googleTranslateUrl = "http://translate.google.cn/translate_t";
            //Dictionary<string, string> postDict = new Dictionary<string, string>();
            //postDict.Add("hl", "zh-CN");
            //postDict.Add("ie", "UTF-8");
            //postDict.Add("text", strToTranslate);
            //postDict.Add("langpair", fromLanguage + "|" + toLanguage);
            //const string googleTransHtmlCharset = "UTF-8";
            //string transRetHtml = getUrlRespHtml(googleTranslateUrl, null,googleTransHtmlCharset, postDict);


            ////http://translate.google.cn/#zh-CN/en/%E4%BB%96%E4%BB%AC%E6%98%AF%E8%BF%99%E6%A0%B7%E8%AF%B4%E7%9A%84
            //string googleTransBaseUrl = "http://translate.google.cn/#";
            //strToTranslate = "他们是这样说的";
            //string encodedStr = HttpUtility.UrlEncode(strToTranslate);
            //string googleTransUrl = googleTransBaseUrl + fromLanguage + "/" + toLanguage + "/" + encodedStr;
            //string transRetHtml = getUrlRespHtml(googleTransUrl);


            //http://translate.google.cn/translate_a/t?client=t&text=%E4%BB%96%E4%BB%AC%E6%98%AF%E8%BF%99%E6%A0%B7%E8%AF%B4%E7%9A%84&hl=zh-CN&sl=zh-CN&tl=en&ie=UTF-8&oe=UTF-8&multires=1&ssel=0&tsel=0&sc=1
            //strToTranslate = "他们是这样说的";
            //string encodedStr = System. HttpUtility.UrlEncode(strToTranslate);
            //string googleTransBaseUrl = "http://translate.google.cn/translate_a/t?";
            //string googleTransUrl = googleTransBaseUrl;
            //googleTransUrl += "&client=" + "t";
            //googleTransUrl += "&text=" + encodedStr;
            //googleTransUrl += "&hl=" + "zh-CN";
            //googleTransUrl += "&sl=" + fromLanguage;// source   language
            //googleTransUrl += "&tl=" + toLanguage;  // to       language
            //googleTransUrl += "&ie=" + "UTF-8";     // input    encode
            //googleTransUrl += "&oe=" + "UTF-8";     // output   encode

            //try
            //{
            //    transRetHtml = getUrlRespHtml(googleTransUrl);
            //    //[[["They say","他们是这样说的","","Tāmen shì zhèyàng shuō de"]],,"zh-CN",,[["They",[5],0,0,1000,0,1,0],["say",[6],1,0,1000,1,2,0]],[["他们 是",5,[["They",1000,0,0],["they are",0,0,0],["they were",0,0,0],["that they are",0,0,0],["they are the",0,0,0]],[[0,3]],"他们是这样说的"],["这样 说",6,[["say",1000,1,0],["said",0,1,0],["say so",0,1,0],["says",0,1,0],["say this",0,1,0]],[[3,6]],""]],,,[["zh-CN"]],1]

            //    if (extractSingleStr(@"\[\[\[""(.+?)"","".+?"",", transRetHtml, out translatedStr))
            //    {
            //        //extrac out:They say
            //    }
            //}
            //catch
            //{
            //    // if pass some special string, such as "彭德怀", then will occur 500 error
            //    // here tmp not process the error, just omit it here
            //}

            return translatedStr;
        }

        public string transZhcnToEn(string strToTranslate) {
            return translateString(strToTranslate, "zh-CN", "en");
        }




        /// <summary>
        /// 生成多語系 XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateEntitiesXML() {
            XmlDocument doc = new XmlDocument();
            richTextBox1.Clear();
            StringBuilder SB = new StringBuilder();
            SB.Append("<!--" + tb_scrib.Text + "-->\r\n");
            SB.Append("<DataEntity TypeKey=\"" + tb_className.Text + "\" name=\"\"> \r\n");
            foreach(DataGridViewRow dr in dataGridView1.Rows) {
                if(dr.Cells["Parameter"].Value != null) {
                    SB.Append("\r\n<data name=\"");
                    SB.Append(dr.Cells["Parameter"].Value);
                    SB.Append("\">\r\n");
                    SB.Append("<EN>" + dr.Cells["Parameter"].Value.ToString() + "</EN>\r\n");
                    SB.Append("<CHS>" + translateEncodingByWord(dr.Cells["Describe"].Value.ToString(), true).Trim() + "</CHS> \r\n");
                    SB.Append("<CHT>" + dr.Cells["Describe"].Value.ToString() + "</CHT>\r\n");
                    SB.Append("</data>");
                }
            }
            SB.Append("\r\n  </DataEntity>");
            richTextBox1.AppendText(SB.ToString());
        }


        /// <summary>
        /// 顯示實體結構
        /// </summary>
        private void CreateEntities(DataGridView Grid,bool IsDetail,string ClaseeName,string Descrpion,List<Detail>li) {
            if(!cb_OnlyParameter.Checked) {
                CreateUsing();
                CreateNameSpace(ClaseeName, Descrpion);
                CreateClass(ClaseeName);
                EndClass();
            }
            AddPrivateParameter(Grid);
            if (!IsDetail)
            {
                CheckCheckBox();
            }
            foreach(Detail detail in li)
            {
                AddCollection(detail.Name, detail.Description);
            }
            richTextBox1.Clear();

            richTextBox1.AppendText(_strUsing.ToString());
            richTextBox1.AppendText(_nameSpace.ToString());
            richTextBox1.AppendText(_class.ToString());
            richTextBox1.AppendText("        #region Private Member \r\n");
            richTextBox1.AppendText(_privateparameter.ToString());
            richTextBox1.AppendText("        #endregion \r\n");
            richTextBox1.AppendText("        #region Parameter \r\n");
            richTextBox1.AppendText(_parameter.ToString());
            richTextBox1.AppendText("        #endregion \r\n");
            richTextBox1.AppendText(_classEnd.ToString());
            richTextBox1.AppendText("\r\n}");

            richTextBox1.Text = richTextBox1.Text.Replace("GeneralDbType.$$Text", "GeneralDbType.Text");
            richTextBox1.Text = richTextBox1.Text.Replace("$$Text", "String");


            _strUsing.Clear();
            _nameSpace.Clear();
            _class.Clear();
            _classEnd.Clear();
            _privateparameter.Clear();
            _parameter.Clear();
        }

        /// <summary>
        /// 將使用者輸入的型態做轉換
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string TypeSwap(string str) {
            string tempstr = string.Empty;
            switch(str.ToLower()) {
                case "ntext":
                    tempstr = "$$Text";
                    break;

                case "string":
                case "nvarchar":
                case "text":
                case "txt":
                    tempstr = "String";
                    break;
                case "int":
                case "int32":
                    tempstr = "Int32";
                    break;
                case "int64":
                    tempstr = "Int64";
                    break;
                case "guid":
                    tempstr = "Guid";
                    break;
                case "decimal":
                    tempstr = "Decimal";
                    break;
                case "datetime":
                    tempstr = "DateTime";
                    break;
                case "bool":
                case "boolean":
                    tempstr = "Boolean";
                    break;
                case "image": //20140925 add by Dick 加入檔案格式
                    tempstr = "Image";
                    break;
            }
            return tempstr;
        }


       

        /// <summary>
        /// 多語系製作
        /// </summary>
        private void SaveResource(string Xpath, string Name, string value, string comment) {
            XmlDocument doc = FileTool.XmlFile.LoadXml(Xpath);
            XmlNode root = doc.SelectSingleNode("root");
            XmlElement data = doc.CreateElement("data");
            data.SetAttribute("name", Name + "_" + value);
            data.SetAttribute("xml:space", "preserve");
            XmlElement val = doc.CreateElement("value");
            val.InnerText = value;
            data.AppendChild(val);
            XmlElement com = doc.CreateElement("comment");
            com.InnerText = comment;
            data.AppendChild(com);
            root.AppendChild(data);
            doc.Save(Xpath);
        }

        private void Form1_Load(object sender, EventArgs e) {

            //XmlDocument doc = Tools.XmlTool.LoadXml(@"C:\Users\Dick\Desktop\sql\Web2.config");
            //XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            //nsmgr.AddNamespace("ab", "http://schemas.microsoft.com/.NetConfiguration/v2.0");
            //XmlNode node = doc.SelectSingleNode("//ab:appSettings", nsmgr);
            //try {

            //    var xmlnsm = new XmlNamespaceManager(doc.NameTable);
            //    //XMLNode node = doc.SelectSingleNode("configuration", xmlnsm);
            //    //XmlNamespaceManager xml = doc.SelectSingleNode("configuration:PersonalInformation");

            //    //foreach(XmlNode node in doc.ChildNodes)
            //    //{
            //    //    if(node.Name == "configuration") {
            //    //        var b = 1;
            //    //    }
            //    //}
            //}
            //catch(Exception ex)
            //{
            //    var b = ex;
            //}


            String line = string.Empty;
            using(StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\SaveFile.xml")) {
                line = sr.ReadToEnd();
            }
            if(line != string.Empty) {
                line = line.Replace("Decimal", "decimal");
                line = line.Replace("String", "string");
                line = line.Replace("Boolean", "bool");
                line = line.Replace("Int32", "Int");
                line = line.Replace("int", "Int");
                line = line.Replace("GUID", "Guid");
                line = line.Replace("datetime", "DateTime");
                line = line.Replace("image", "Image");
                File.Delete(Directory.GetCurrentDirectory() + "\\SaveFile.xml");
                System.IO.File.WriteAllText(Directory.GetCurrentDirectory() + "\\SaveFile.xml", line);
            }
        }

        private void 生成生成EntitiesToolStripMenuItem_Click(object sender, EventArgs e) {
            CreateEntities(dataGridView1, false, tb_className.Text, tb_scrib.Text, new List<Detail> { });
        }

        private void 生成EntitiesXMLToolStripMenuItem_Click(object sender, EventArgs e) {
            CreateEntitiesXML();
        }

        private void 英文ToolStripMenuItem1_Click(object sender, EventArgs e) {
            ProductMutiLaguage("EN", "Browse_QueryView", false);
        }

        /// <summary>
        /// 建立多語系
        /// </summary>
        /// <param name="TypeName">Project/QueryView</param>
        /// <param name="ParameterName">參數名稱</param>
        /// <param name="Describe">描述</param>
        private void CreateResource(string TypeName, string ParameterName, string Describe) {

            richTextBox1.AppendText(TypeName);
            richTextBox1.AppendText(ParameterName);
            richTextBox1.AppendText(" ");
            richTextBox1.AppendText(ParameterName);
            richTextBox1.AppendText(" ");
            richTextBox1.AppendText(Describe);
            richTextBox1.AppendText("\r\n");
        }

        private void 存檔ToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveXml();
        }

        /// <summary>
        /// 儲存紀錄
        /// </summary>
        private void SaveXml() {
            try {
                XmlDocument doc = FileTool.XmlFile.LoadXml(Xmlpath);
                XmlNode root = doc.SelectSingleNode("root");
                XmlNode parent = doc.GetElementById(tb_className.Text);
                if(parent != null) {
                    root.RemoveChild(parent);
                }
                XmlElement ThisClass = doc.CreateElement("Class");
                ThisClass.SetAttribute("ClassName", this.tb_className.Text);
                ThisClass.SetAttribute("Describle", this.tb_scrib.Text);
                ThisClass.SetAttribute("IAuditObject", cb_IAuditObject.Checked.ToString());
                ThisClass.SetAttribute("DataEntity", cb_DataEntity.Checked.ToString());
                ThisClass.SetAttribute("INamedObject", cb_INamedObject.Checked.ToString());
                ThisClass.SetAttribute("IFlagObject", cb_IFlagObject.Checked.ToString());
                ThisClass.SetAttribute("IOwnerObject", cb_IOwnerObject.Checked.ToString());
                ThisClass.SetAttribute("ICodeObject", cb_ICodeObject.Checked.ToString());
                ThisClass.SetAttribute("IDataModifyObject", cb_IDataModifyObject.Checked.ToString());
                ThisClass.SetAttribute("Collection", cb_Collection.Checked.ToString());
                ThisClass.SetAttribute("IsCase", cb_ForCase.Checked.ToString());
                foreach(DataGridViewRow dr in dataGridView1.Rows) {
                    if(dr.Cells["Parameter"].Value != null && dr.Cells["Parameter"].Value.ToString() != "") {
                        XmlElement element = doc.CreateElement("Parameter");
                        element.SetAttribute("Name", dr.Cells["Parameter"].Value.ToString());
                        element.SetAttribute("Type", dr.Cells["Type"].Value.ToString());
                        element.SetAttribute("Describe", dr.Cells["Describe"].Value.ToString());

                        element.SetAttribute("ReferenceProperty", dr.Cells["ReferenceProperty"].Value.ToString());
                        // 20140904加入order 及  UIorder Nessery
                        element.SetAttribute("Order", dr.Cells["Order"].Value.ToString());
                        element.SetAttribute("UIOrder", dr.Cells["UIOrder"].Value.ToString());
                        element.SetAttribute("Necessary", dr.Cells["Necessary"].Value.ToString());

                        ThisClass.AppendChild(element);
                    }
                }
                root.AppendChild(ThisClass);
                doc.Save(Xmlpath);
            }
            catch(Exception ex) {

            }
        }

        private void 結束ToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
            this.Dispose();
        }

        /// <summary>
        /// 產生多語系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 英文ToolStripMenuItem_Click(object sender, EventArgs e) {
            ProductMutiLaguage("EN", "Browse_QueryProject", false);
        }


        /// <summary>
        /// 產生多語系
        /// </summary>
        /// <param name="type"></param>
        private void ProductMutiLaguage(string type, string front, bool IsSave) {
            StringBuilder sb = new StringBuilder();
            richTextBox1.Clear();
            sb = ResourceQueryView(type, front, IsSave, sb);
            richTextBox1.AppendText(sb.ToString());
        }


        /// <summary>
        /// QueryView 多語系
        /// </summary>
        /// <param name="type"></param>
        /// <param name="front"></param>
        /// <param name="IsSave"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        private StringBuilder ResourceQueryView(string type, string front, bool IsSave, StringBuilder sb) {
            foreach(DataGridViewRow dr in dataGridView1.Rows) {
                if(dr.Cells["Parameter"].Value != null) {
                    ProductMutiLanuageSubMethod(type, front, ref sb, dr.Cells["Parameter"].Value.ToString(), dr.Cells["Describe"].Value.ToString(), IsSave);
                }
            }
            if(dataGridView1.Rows.Count > 1) {
                PublicItem(type, front, ref sb, dataGridView1.Rows[0].Cells[2].Value.ToString());
            }
            else {
                PublicItem(type, front, ref sb, "");
            }
            return sb;
        }


        private void ProductMutiLanuageSubMethod(string type, string front, ref StringBuilder sb, string value1, string value2, bool IsSave) {
            sb.Append(front);
            sb.Append("_");
            sb.Append(tb_className.Text);
            sb.Append("_");
            sb.Append(value1);
            sb.Append("\t");
            switch(type) {
                case "EN":
                    sb.Append(value1);
                    sb.Append("\t");
                    sb.Append(value2);
                    if(IsSave) {
                        SaveResource(ProjectPath + "QueryResources.resx", "Browse_QueryProject", value1, value2);
                    }
                    break;
                case "CHT":
                    sb.Append(value2);
                    sb.Append("\t");
                    sb.Append(value2);
                    if(IsSave) {
                        SaveResource(ProjectPath + "QueryResources.zh-CHT.resx", "Browse_QueryProject", value1, value2);
                    }
                    break;
                case "CHS":
                    string temp = translateEncodingByWord(value2, true).Trim();
                    sb.Append(temp);
                    sb.Append("\t");
                    sb.Append(temp);
                    if(IsSave) {
                        SaveResource(ProjectPath + "QueryResources.zh-CHS.resx", "Browse_QueryProject", value1, value2);
                    }
                    break;
            }
            sb.Append("\r\n");
        }


        /// <summary>
        /// 加入審核多語系
        /// </summary>
        /// <param name="type"></param>
        /// <param name="front"></param>
        /// <param name="sb"></param>
        /// <param name="Entity"></param>
        private void PublicItem(string type, string front, ref StringBuilder sb, string Entity) {
            if(Entity.IndexOf("Id") != -1) {
                Entity = Entity.Replace("Id", "");
            }
            foreach(Control con in MainGroup.Controls) {
                switch(con.Name) {
                    case "cb_IAuditObject":
                        System.Windows.Forms.CheckBox cb1 = con as System.Windows.Forms.CheckBox;
                        if(cb1.Checked) {
                            ProductMutiLanuageSubMethod(type, front, ref  sb, "Audited", "待審核", false);
                            ProductMutiLanuageSubMethod(type, front, ref  sb, "Confirmed", "已審核", false);

                        }
                        break;
                    case "cb_ICodeObject":
                        System.Windows.Forms.CheckBox cb2 = con as System.Windows.Forms.CheckBox;
                        if(cb2.Checked) {
                            ProductMutiLanuageSubMethod(type, front, ref  sb, "Code", Entity + "編號", false);
                        }
                        break;
                    case "cb_INamedObject":
                        System.Windows.Forms.CheckBox cb3 = con as System.Windows.Forms.CheckBox;
                        if(cb3.Checked) {
                            ProductMutiLanuageSubMethod(type, front, ref  sb, "Name", Entity + "名稱", false);
                        }
                        break;
                }
            }
        }

        private void 繁中ToolStripMenuItem_Click(object sender, EventArgs e) {
            ProductMutiLaguage("CHT", "Browse_QueryProject", false);
        }

        private void 簡中ToolStripMenuItem_Click(object sender, EventArgs e) {
            ProductMutiLaguage("CHS", "Browse_QueryProject", false);
        }

        private void 繁中ToolStripMenuItem1_Click(object sender, EventArgs e) {
            ProductMutiLaguage("CHT", "Browse_QueryView", false);
        }

        private void 簡中ToolStripMenuItem1_Click(object sender, EventArgs e) {
            ProductMutiLaguage("CHS", "Browse_QueryView", false);
        }

        private void 路徑ToolStripMenuItem_Click(object sender, EventArgs e) {
            PathForm _pathForm = new PathForm();
            _pathForm.ShowDialog();
        }

        private void 一併修改ToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void 一併修改ToolStripMenuItem1_Click(object sender, EventArgs e) {
            ProductMutiLaguage("EN", "Browse_QueryView", true);
            ProductMutiLaguage("CHT", "Browse_QueryView", true);
            ProductMutiLaguage("CHS", "Browse_QueryView", true);
        }

        private void 個案打包ToolStripMenuItem_Click(object sender, EventArgs e) {
            Packet pk = new Packet();
            pk.ShowDialog();

        }

        private void sQL過濾ToolStripMenuItem_Click(object sender, EventArgs e) {
            SQLFilter sqlfiler = new SQLFilter();
            sqlfiler.Show();

        }

        private void 生成ServiceToolStripMenuItem_Click(object sender, EventArgs e) {
            CreateInterFace();
        }

        /// <summary>
        /// 生成接口
        /// </summary>
        private void CreateInterFace() {
            richTextBox1.Clear();
            string X = string.Empty;
            if(tb_className.Text.Substring(0, 1) == "X") {
                X = "I" + tb_className.Text.Substring(1, tb_className.Text.Length - 1) + "ServiceX";
            }
            else {
                X = "I" + tb_className.Text + "Service";
            }
            richTextBox1.AppendText("namespace Dcms.HR.Services {\r\n");
            richTextBox1.AppendText("    using System;\r\n");
            richTextBox1.AppendText("    using System.ComponentModel;\r\n");
            richTextBox1.AppendText("    using Dcms.Common;\r\n");
            richTextBox1.AppendText("    using Dcms.Common.Torridity;\r\n");
            richTextBox1.AppendText("    using Dcms.Common.Torridity.Metadata;\r\n");
            richTextBox1.AppendText("    using Dcms.HR.DataEntities;\r\n");
            richTextBox1.AppendText("    using System.Data;\r\n");
            richTextBox1.AppendText("    using Dcms.Common.Torridity.Query;\r\n");
            richTextBox1.AppendText("    using System.Collections.Generic;\r\n");
            richTextBox1.AppendText("    public interface " + X + @": IHRDocumentService<" + tb_className.Text + @">, IHRDocumentService {");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("    }\r\n");
            richTextBox1.AppendText("}");
        }

        private void 生成ToolStripMenuItem1_Click(object sender, EventArgs e) {

        }

        private void CreateServer(string pType) {
            richTextBox1.Clear();

            string X = string.Empty;
            if(tb_className.Text.Substring(0, 1) == "X") {
                X = "I" + tb_className.Text.Substring(1, tb_className.Text.Length - 1) + "ServiceX";
            }
            else {
                X = "I" + tb_className.Text + "Service";
            }
            StringBuilder str = new StringBuilder();
            foreach(DataGridViewRow dr in dataGridView1.Rows) {
                if(dr.Cells["Parameter"].Value != null) {
                    bool bo = dr.Cells["Necessary"].Value.ToString() == "" ? false : Convert.ToBoolean(dr.Cells["Necessary"].Value);
                    if(bo) {
                        string Parameter = dr.Cells["Parameter"].Value.ToString();
                        switch(dr.Cells["Type"].Value.ToString().ToLower()) {
                            case "decmail":
                                str.Append("            vh.GreaterThan<decimal>(e.DataEntity,\"" + Parameter + "\" , 0, true);\r\n");
                                break;
                            case "int32":
                                str.Append("            vh.GreaterThan<int>(e.DataEntity,\"" + Parameter + "\" , 0, true);\r\n");
                                break;
                            case "datetime":
                                str.Append("            vh.DateTimeNotIsEmpty(e.DataEntity, \"" + Parameter + "\" );\r\n");
                                break;
                            case "string":
                            case "guid":
                                str.Append("            vh.StringNotNullOrEmpty(e.DataEntity, \"" + Parameter + "\" );\r\n");
                                break;
                        }
                    }
                }
            }




            richTextBox1.AppendText(@"  namespace Dcms.HR.Services {
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using Dcms.Common;
    using Dcms.Common.Torridity;
    using Dcms.Common.Torridity.Metadata;
    using Dcms.Common.Torridity.Query;
    using Dcms.Common.Torridity.DataSource;
    using Dcms.Common.Core;
    using Dcms.Common.Business;
    using Dcms.Common.Services;
    using Dcms.HR.DataEntities;
    using Dcms.HR.Business.Implement.Properties;

    [ServiceClass(typeof(" + X + @"), ServiceCreateType.Callback)]
    public class " + X.Substring(1, X.Length - 1) + @" : HRBusinessServiceEx<" + tb_className.Text + @">, " + X + @" {
       " + pType + @"      
        protected override void OnSaveBefore(BusinessServiceBase<" + tb_className.Text + @">.SaveBeforeArgs e) {

            #region 数据检验
            e.DataEntity.ClearErrors();
            HRVerifyHelper vh = new HRVerifyHelper(e.Exceptions);
            " + str.ToString() + @"
            #endregion
            base.OnSaveBefore(e);
        }
    }

}");
        }



        private void 導入必填欄位ToolStripMenuItem_Click(object sender, EventArgs e) {
            richTextBox1.Clear();
            richTextBox1.AppendText("<FieldMapping TargetEntity=\"" + tb_className.Text + "\" ParentEntity=\"\"> \r\n");
            foreach(DataGridViewRow dr in dataGridView1.Rows) {
                if(dr.Cells["Parameter"].Value != null) {
                    bool bo = dr.Cells["Necessary"].Value.ToString() == "" ? false : Convert.ToBoolean(dr.Cells["Necessary"].Value);
                    if(bo) {
                        richTextBox1.AppendText("<Field Name=\"" + dr.Cells["Parameter"].Value + "\" EnName=\"" + dr.Cells["Parameter"].Value + "\" ScName=\"" + translateEncodingByWord(dr.Cells["Describe"].Value.ToString(), true).Trim() + "\" TcName=\"" + dr.Cells["Describe"].Value + "\" IsNotEmpty=\"true\" /> \r\n");
                    }
                    else {
                        richTextBox1.AppendText("<Field Name=\"" + dr.Cells["Parameter"].Value + "\" EnName=\"" + dr.Cells["Parameter"].Value + "\" ScName=\"" + translateEncodingByWord(dr.Cells["Describe"].Value.ToString(), true).Trim() + "\" TcName=\"" + dr.Cells["Describe"].Value + "\"  /> \r\n");
                    }
                }
            }
            richTextBox1.AppendText(" </FieldMapping>");
        }

        private void 英文ToolStripMenuItem2_Click(object sender, EventArgs e) {
            ProductMutiLaguage("EN", "DataEnityDisplayName", false);
        }

        private void 繁忠ToolStripMenuItem_Click(object sender, EventArgs e) {
            ProductMutiLaguage("CHT", "DataEnityDisplayName", false);
        }

        private void 簡忠ToolStripMenuItem_Click(object sender, EventArgs e) {
            ProductMutiLaguage("CHS", "DataEnityDisplayName", false);
        }


        #region CheckBox 選取動作

        /// <summary>
        /// 修改參數顯示
        /// </summary>
        /// <param name="ControlName"></param>
        /// <param name="bo"></param>
        private void ModiedGridView(string ControlName, bool bo) {
            System.Data.DataTable dt = this.dataGridView1.DataSource as System.Data.DataTable;
            int count = dt.Rows.Count ;
            if(bo) {
                List<string[]> Lis = _DicCheckBox[ControlName];
                foreach(string[] item in Lis) {
                    //DataRow dr = dt.NewRow();
                    //dr["Parameter"] =item[0];
                    //dr["Type"] = item[1];
                    //dr["Describe"] = item[2];
                    //if(item.Length > 2) {
                    //    dr["ReferenceProperty"] = item[3];
                    //}
                    //dt.Rows.Add(dr)
                    if(item.Length > 3) {
                        DataRow dr = dt.NewRow();
                        dr[0] = item[0];
                        dr[1] = item[2];
                        dr[2] = item[3];   
                        dt.Rows.Add(dr);   
                    }
                    else {
                        DataRow dr = dt.NewRow();
                        dr[0] = item[0];
                        dr[1] = item[2];
                        dt.Rows.Add(dr);
                    }
                    dataGridView1.Rows[count].Cells["Type"].Value = item[1] == "Int" ? "Int32" : item[1];
                    count++;
                }              
            }
            else {
                List<string[]> Lis = _DicCheckBox[ControlName];
                foreach(string[] item in Lis) {
                    DataRow[] dr = dt.Select("Parameter ='" + item[0] + "'");
                    if(dr.Length > 0) {
                        dt.Rows.Remove(dr[0]);
                    }
                }
            }
        }

        private void cb_IFlagObject_CheckedChanged(object sender, EventArgs e) {
            ModiedGridView(cb_IFlagObject.Text, cb_IFlagObject.Checked);
        }

        private void cb_IAuditObject_CheckedChanged(object sender, EventArgs e) {
            ModiedGridView(cb_IAuditObject.Text, cb_IAuditObject.Checked);
        }

        private void cb_IOwnerObject_CheckedChanged(object sender, EventArgs e) {
            ModiedGridView(cb_IOwnerObject.Text, cb_IOwnerObject.Checked);
        }

        private void cb_ICodeObject_CheckedChanged(object sender, EventArgs e) {
            ModiedGridView(cb_ICodeObject.Text, cb_ICodeObject.Checked);
        }

        private void cb_IDataModifyObject_CheckedChanged(object sender, EventArgs e) {
            ModiedGridView(cb_IDataModifyObject.Text, cb_IDataModifyObject.Checked);
        }

        private void cb_INamedObject_CheckedChanged(object sender, EventArgs e) {
            ModiedGridView(cb_INamedObject.Text, cb_INamedObject.Checked);
        }
        #endregion

        private void 生成VirtualEntitiesToolStripMenuItem_Click(object sender, EventArgs e) {
            richTextBox1.Clear();
            richTextBox1.AppendText("using System;\r\n");
            richTextBox1.AppendText("using System.Collections.Generic;\r\n");
            richTextBox1.AppendText("using System.Text;\r\n");
            richTextBox1.AppendText("using Dcms.Common.Torridity.Metadata;\r\n");
            richTextBox1.AppendText("using System.ComponentModel;\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("namespace Dcms.HR.DataEntities {\r\n");
            richTextBox1.AppendText("    [Description(\"" + tb_scrib.Text + "\")]\r\n");
            richTextBox1.AppendText("    [Serializable()]\r\n");
            richTextBox1.AppendText("    public class " + tb_className.Text + "Virtual : VirtualObjectBase<" + tb_className.Text + "> {\r\n");
            richTextBox1.AppendText("        public new const string TYPE_KEY = " + tb_className.Text + "Virtual;\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        [CollectionProperty(typeof(" + tb_className.Text + "))]\r\n");
            richTextBox1.AppendText("        public override ActualDataEntityList<" + tb_className.Text + "> ActualDataEntities {\r\n");
            richTextBox1.AppendText("            get {\r\n");
            richTextBox1.AppendText("                return base.ActualDataEntities;\r\n");
            richTextBox1.AppendText("            }\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("    }\r\n");
            richTextBox1.AppendText("}\r\n");
            richTextBox1.AppendText(" \r\n");

        }

        private void 生成VirtualServiceToolStripMenuItem_Click(object sender, EventArgs e) {
            richTextBox1.Clear();
            richTextBox1.AppendText("using System;\r\n");
            richTextBox1.AppendText("using System.Collections.Generic;\r\n");
            richTextBox1.AppendText("using System.Text;\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("using Dcms.Common;\r\n");
            richTextBox1.AppendText("using Dcms.HR.DataEntities;\r\n");
            richTextBox1.AppendText("using Dcms.HR.Services;\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("[ServiceClass(typeof(I" + tb_className.Text + "VirtualServiceX), ServiceCreateType.Callback)]\r\n");
            richTextBox1.AppendText("internal class " + tb_className.Text + "VirtualService : VirtualObjectServiceBase<" + tb_className.Text + ">, I" + tb_className.Text + "VirtualServiceX {\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("    /// <summary>\r\n");
            richTextBox1.AppendText("    /// 取實際實體服務\r\n");
            richTextBox1.AppendText("    /// </summary>\r\n");
            richTextBox1.AppendText("    protected override IDocumentService<" + tb_className.Text + "> ActualServiceType {\r\n");
            richTextBox1.AppendText("        get { return Factory.GetService<I" + tb_className.Text + "ServiceX>(); }\r\n");
            richTextBox1.AppendText("    }\r\n");
            richTextBox1.AppendText("    public override string TypeKey {\r\n");
            richTextBox1.AppendText("        get {\r\n");
            richTextBox1.AppendText("            return " + tb_className.Text + "Virtual.TYPE_KEY;\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("    }\r\n");
            richTextBox1.AppendText("}\r\n");
        }

        private void 生成VirtualUIToolStripMenuItem_Click(object sender, EventArgs e) {
            richTextBox1.Clear();
            string temp = tb_className.Text.Substring(0, 1).ToLower() + tb_className.Text.Substring(1, tb_className.Text.Length - 1);
            richTextBox1.AppendText("//---------------------------------------------------------------- \r\n");
            richTextBox1.AppendText("//Copyright (C) 2005-2005 Digital China Management System Co.,Ltd\r\n");
            richTextBox1.AppendText("//Http://www.Dcms.com.cn \r\n");
            richTextBox1.AppendText("// All rights reserved.\r\n");
            richTextBox1.AppendText("//<author>author</author>\r\n");
            richTextBox1.AppendText("//<createDate>date</createDate>\r\n");
            richTextBox1.AppendText("//<description>description</description>\r\n");
            richTextBox1.AppendText("//---------------------------------------------------------------- \r\n");
            richTextBox1.AppendText("using System;\r\n");
            richTextBox1.AppendText("using System.Data;\r\n");
            richTextBox1.AppendText("using System.Text;\r\n");
            richTextBox1.AppendText("using System.Drawing;\r\n");
            richTextBox1.AppendText("using System.Windows.Forms;\r\n");
            richTextBox1.AppendText("using System.ComponentModel;\r\n");
            richTextBox1.AppendText("using System.Collections.Generic;\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("using Dcms.Common;\r\n");
            richTextBox1.AppendText("using Dcms.Common.UI;\r\n");
            richTextBox1.AppendText("using Dcms.HR.Services;\r\n");
            richTextBox1.AppendText("using Dcms.HR.DataEntities;\r\n");
            richTextBox1.AppendText("using Dcms.HR.UI.Properties;\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("namespace Dcms.HR.UI {\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("    /// <summary>\r\n");
            richTextBox1.AppendText("    /// " + tb_className.Text + "Virtual實體編輯畫面\r\n");
            richTextBox1.AppendText("    /// </summary>\r\n");
            richTextBox1.AppendText("    public class " + tb_className.Text + "VirtualEditerView : HREditerView {\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        /// <summary>\r\n");
            richTextBox1.AppendText("        /// 初始化EditerView\r\n");
            richTextBox1.AppendText("        /// </summary>\r\n");
            richTextBox1.AppendText("        public " + tb_className.Text + "VirtualEditerView() {\r\n");
            richTextBox1.AppendText("            InitializeComponent();\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        #region Component Designer generated code\r\n");
            richTextBox1.AppendText("\r\n");

            richTextBox1.AppendText("        private BindingSource " + temp + "virtualBindingSource;\r\n");
            richTextBox1.AppendText("        private ErrorProvider errorProvider;\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        /// <summary> \r\n");
            richTextBox1.AppendText("        /// Required designer variable.\r\n");
            richTextBox1.AppendText("        /// </summary>\r\n");
            richTextBox1.AppendText("        private System.ComponentModel.IContainer components = null;\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        /// <summary> \r\n");
            richTextBox1.AppendText("        /// Clean up any resources being used.\r\n");
            richTextBox1.AppendText("        /// </summary>\r\n");
            richTextBox1.AppendText("        /// <param name=\"disposing\">true if managed resources should be disposed; otherwise, false.</param>\r\n");
            richTextBox1.AppendText("        protected override void Dispose(bool disposing) {\r\n");
            richTextBox1.AppendText("            if(disposing && (components != null)) {\r\n");
            richTextBox1.AppendText("                components.Dispose();\r\n");
            richTextBox1.AppendText("            }\r\n");
            richTextBox1.AppendText("            base.Dispose(disposing);\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        /// <summary> \r\n");
            richTextBox1.AppendText("        /// Required method for Designer support - do not modify \r\n");
            richTextBox1.AppendText("        /// the contents of this method with the code editor.\r\n");
            richTextBox1.AppendText("        /// </summary>\r\n");
            richTextBox1.AppendText("        private void InitializeComponent() {\r\n");
            richTextBox1.AppendText("            this.components = new System.ComponentModel.Container();\r\n");
            richTextBox1.AppendText("            this." + temp + "virtualBindingSource = new System.Windows.Forms.BindingSource(this.components);\r\n");
            richTextBox1.AppendText("            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);\r\n");
            richTextBox1.AppendText("            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();\r\n");
            richTextBox1.AppendText("            ((System.ComponentModel.ISupportInitialize)(this." + temp + "virtualBindingSource)).BeginInit();\r\n");
            richTextBox1.AppendText("            this.MainTabControl.SuspendLayout();\r\n");
            richTextBox1.AppendText("            this.SuspendLayout();\r\n");
            richTextBox1.AppendText("            // \r\n");
            richTextBox1.AppendText("            // errorProvider\r\n");
            richTextBox1.AppendText("            // \r\n");
            richTextBox1.AppendText("            this.errorProvider.ContainerControl = this;\r\n");
            richTextBox1.AppendText("            this.errorProvider.DataSource = this." + temp + "virtualBindingSource;\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("            // \r\n");
            richTextBox1.AppendText("            // " + temp + "virtualBindingSource\r\n");
            richTextBox1.AppendText("            // \r\n");
            richTextBox1.AppendText("            this." + temp + "virtualBindingSource.DataSource = typeof(Dcms.HR.DataEntities." + temp + "Virtual);\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("            // \r\n");
            richTextBox1.AppendText("            // " + temp + "virtualEditerView\r\n");
            richTextBox1.AppendText("            // \r\n");
            richTextBox1.AppendText("            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);\r\n");
            richTextBox1.AppendText("            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;\r\n");
            richTextBox1.AppendText("            this.Name = \"" + tb_className.Text + "VirtualEditerView\";\r\n");
            richTextBox1.AppendText("            this.MainTabControl.ResumeLayout(false);\r\n");
            richTextBox1.AppendText("            //this.DetailTabControlCollapsed = true;//如不需要明細區塊請取消註解本行\r\n");
            richTextBox1.AppendText("            ((System.ComponentModel.ISupportInitialize)(this." + temp + "virtualBindingSource)).EndInit();\r\n");
            richTextBox1.AppendText("            this.ResumeLayout(false);\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        #endregion\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        #region Override\r\n");
            richTextBox1.AppendText("        /// <summary>\r\n");
            richTextBox1.AppendText("        /// 觸發DataSource事件\r\n");
            richTextBox1.AppendText("        /// </summary>\r\n");
            richTextBox1.AppendText("        /// <param name=\"e\">事件需要的参數</param>\r\n");
            richTextBox1.AppendText("        protected override void OnDataSourceChanged(EventArgs e) {\r\n");
            richTextBox1.AppendText("            this." + temp + "virtualBindingSource.DataSource = this.DataSource;\r\n");
            richTextBox1.AppendText("            base.OnDataSourceChanged(e);\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        /// <summary>\r\n");
            richTextBox1.AppendText("        /// 结束得编辑\r\n");
            richTextBox1.AppendText("        /// </summary>\r\n");
            richTextBox1.AppendText("        public override void EndEdit() {\r\n");
            richTextBox1.AppendText("            base.EndEdit();\r\n");
            richTextBox1.AppendText("            this." + temp + "virtualBindingSource.EndEdit();\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("        #endregion\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        //儲存檔案\r\n");
            richTextBox1.AppendText("        public void SaveData() {\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        //數據驗證\r\n");
            richTextBox1.AppendText("        public void CheckData() {\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("    }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("    /// <summary>\r\n");
            richTextBox1.AppendText("    /// " + tb_className.Text + "Virtual文件窗口的訪問服務\r\n");
            richTextBox1.AppendText("    /// </summary>\r\n");
            richTextBox1.AppendText("    public class " + tb_className.Text + "VirtualDocumentProvider : VirtualProvider, IDocumentInfoProvider {\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        /// <summary>\r\n");
            richTextBox1.AppendText("        /// 返回" + tb_className.Text + "Virtual文件類型的關鍵字r\r\n");
            richTextBox1.AppendText("        /// </summary>\r\n");
            richTextBox1.AppendText("        public override string TypeKey {\r\n");
            richTextBox1.AppendText("            get { return " + tb_className.Text + "Virtual.TYPE_KEY; }\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        /// <summary>\r\n");
            richTextBox1.AppendText("        /// 對應主實體的TYPEKEY\r\n");
            richTextBox1.AppendText("        /// </summary>\r\n");
            richTextBox1.AppendText("        public override string BaseTypeKey {\r\n");
            richTextBox1.AppendText("            get { return " + tb_className.Text + ".TYPE_KEY; }\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        /// <summary>\r\n");
            richTextBox1.AppendText("        /// 建立DocumentWindow \r\n");
            richTextBox1.AppendText("        /// </summary>\r\n");
            richTextBox1.AppendText("        /// <returns>新的DocumentWindow</returns>\r\n");
            richTextBox1.AppendText("        public override DocumentWindow CreateDocumentWindow() {\r\n");
            richTextBox1.AppendText("            return new " + tb_className.Text + "VirtualDocument();\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        /// <summary>\r\n");
            richTextBox1.AppendText("        /// 獲取" + tb_className.Text + "Virtual的商業服務類型\r\n");
            richTextBox1.AppendText("        /// </summary>\r\n");
            richTextBox1.AppendText("        /// <param name=\"pCustom\"></param>\r\n");
            richTextBox1.AppendText("        /// <returns></returns>\r\n");
            richTextBox1.AppendText("        public override object GetBusinessService(object pCustom) {\r\n");
            richTextBox1.AppendText("            return Factory.GetService<I" + tb_className.Text + "VirtualService>();\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("    }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("    /// <summary>\r\n");
            richTextBox1.AppendText("    /// " + tb_className.Text + "Virtual文件的窗口\r\n");
            richTextBox1.AppendText("    /// </summary>\r\n");
            richTextBox1.AppendText("    public class " + tb_className.Text + "VirtualDocument : DcmsDocumentWindow {\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        /// <summary>\r\n");
            richTextBox1.AppendText("        /// 返回" + tb_className.Text + "Virtual窗口的文件關鍵字r\r\n");
            richTextBox1.AppendText("        /// </summary>\r\n");
            richTextBox1.AppendText("        public override string TypeKey {\r\n");
            richTextBox1.AppendText("            get { return " + tb_className.Text + "Virtual.TYPE_KEY; }\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        public override string HelpKey {\r\n");
            richTextBox1.AppendText("            get { return " + tb_className.Text + "Virtual.TYPE_KEY; }\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        #region InitializeComponent\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        /// <summary>\r\n");
            richTextBox1.AppendText("        /// 初始化" + tb_className.Text + "VirtualDocument\r\n");
            richTextBox1.AppendText("        /// </summary>\r\n");
            richTextBox1.AppendText("        public " + tb_className.Text + "VirtualDocument() {\r\n");
            richTextBox1.AppendText("            InitializeComponent();\r\n");
            richTextBox1.AppendText("            this.Controller.Saving += new EventHandler(Controller_Saving);\r\n");
            richTextBox1.AppendText("            this.SelectDocumentControl.Visible = false;\r\n");
            richTextBox1.AppendText("            this.Controller.SaveAndRead = false;\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        void Controller_Saving(object sender, EventArgs e) {\r\n");
            richTextBox1.AppendText("            this." + temp + "virtualEditerView1.CheckData();\r\n");
            richTextBox1.AppendText("            this." + temp + "virtualEditerView1.SaveData();\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        private " + tb_className.Text + "VirtualEditerView " + temp + "virtualEditerView1;\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        private void InitializeComponent() {\r\n");
            richTextBox1.AppendText("            this." + temp + "virtualEditerView1 = new Dcms.HR.UI." + tb_className.Text + "VirtualEditerView();\r\n");
            richTextBox1.AppendText("            this.SuspendLayout();\r\n");
            richTextBox1.AppendText("            // \r\n");
            richTextBox1.AppendText("            // " + temp + "virtualEditerView1\r\n");
            richTextBox1.AppendText("            // \r\n");
            richTextBox1.AppendText("            this." + temp + "virtualEditerView1.DataSource = null;\r\n");
            richTextBox1.AppendText("            this." + temp + "virtualEditerView1.Dock = System.Windows.Forms.DockStyle.Fill;\r\n");
            richTextBox1.AppendText("            this." + temp + "virtualEditerView1.Name = \"" + tb_className.Text + "VirtualEditerView1\";\r\n");
            richTextBox1.AppendText("            this." + temp + "virtualEditerView1.Readonly = false;\r\n");
            richTextBox1.AppendText("            this." + temp + "virtualEditerView1.TabIndex = 0;\r\n");
            richTextBox1.AppendText("            // \r\n");
            richTextBox1.AppendText("            // " + tb_className.Text + "VirtualDocument\r\n");
            richTextBox1.AppendText("            // \r\n");
            richTextBox1.AppendText("            this.Controls.Add(this." + temp + "virtualEditerView1);\r\n");
            richTextBox1.AppendText("            this.EditerView = this." + temp + "virtualEditerView1;\r\n");
            richTextBox1.AppendText("            this.Name = \"" + tb_className.Text + "VirtualDocument\";\r\n");
            richTextBox1.AppendText("            this.Controls.SetChildIndex(this." + temp + "virtualEditerView1, 0);\r\n");
            richTextBox1.AppendText("            this." + temp + "virtualEditerView1.ResumeLayout(false);\r\n");
            richTextBox1.AppendText("            this.Size = new System.Drawing.Size(442, 422);\r\n");
            richTextBox1.AppendText("            this.ResumeLayout(false);\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("        #endregion\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        protected override bool CanCreateButtion(string pCommandName) {\r\n");
            richTextBox1.AppendText("            //不可出現保存\r\n");
            richTextBox1.AppendText("  if(pCommandName.Equals(\"SaveAndNew\")) return false;\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("            return base.CanCreateButtion(pCommandName);\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        private IMenuService _service = Factory.GetService<IMenuService>();\r\n");
            richTextBox1.AppendText("        //關閉新增、删除、編輯的按鈕\r\n");
            richTextBox1.AppendText("        protected override void OnInitContextData(InitContextDataEventArgs e) {\r\n");
            richTextBox1.AppendText("            //定義工具欄\r\n");
            richTextBox1.AppendText("            base.OnInitContextData(e);\r\n");
            richTextBox1.AppendText("            foreach(ICommand command in this._service.Commands) {\r\n");
            richTextBox1.AppendText("                switch(command.Name) {\r\n");
            richTextBox1.AppendText("                    case \"DeleteRecord\":\r\n");
            richTextBox1.AppendText("                    case \"AddRecord\":\r\n");
            richTextBox1.AppendText("                    case \"EditRecord\":\r\n");
            richTextBox1.AppendText("                        e.Custom.SetData(command.GetType(), false); break; //不可删除編輯新增\r\n");
            richTextBox1.AppendText("                    default: break;\r\n");
            richTextBox1.AppendText("                }\r\n");
            richTextBox1.AppendText("            }\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        #region 重新設置標題\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        public override string Text {\r\n");
            richTextBox1.AppendText("            get {\r\n");
            richTextBox1.AppendText("                return this.ResetCaption(base.Text);\r\n");
            richTextBox1.AppendText("            }\r\n");
            richTextBox1.AppendText("            set {\r\n");
            richTextBox1.AppendText("                base.Text = value;\r\n");
            richTextBox1.AppendText("            }\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        private string ResetCaption(string pText) {\r\n");
            richTextBox1.AppendText("            if((this.Controller.EditerView != null) && (this.Controller.EditState != EditState.None) && (this.Controller.DataObserver != null)) {\r\n");
            richTextBox1.AppendText("                if(pText.IndexOf(\"-\") > 0) {\r\n");
            richTextBox1.AppendText("                    pText = pText.Substring(pText.IndexOf(\" - \") + 3) + string.Format(\" ({0}) \", Resources.Menu_Batch);\r\n");
            richTextBox1.AppendText("                }\r\n");
            richTextBox1.AppendText("            }\r\n");
            richTextBox1.AppendText("            return pText;\r\n");
            richTextBox1.AppendText("        }\r\n");
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("        #endregion\r\n");
            richTextBox1.AppendText("    }\r\n");
            richTextBox1.AppendText("}\r\n");
            richTextBox1.AppendText("\r\n");
        }


        /// <summary>
        /// 個案補X作法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_ForCase_CheckedChanged(object sender, EventArgs e) {
            if(cb_ForCase.Checked) {
                foreach(DataGridViewRow dr in dataGridView1.Rows) {
                    if(dr.Cells["Parameter"].Value != null) {
                        dr.Cells["Parameter"].Value = "X" + dr.Cells["Parameter"].Value;
                    }
                }
            }
            else {
                foreach(DataGridViewRow dr in dataGridView1.Rows) {
                    if(dr.Cells["Parameter"].Value != null) {
                        if(dr.Cells["Parameter"].Value.ToString().Substring(0, 1) == "X") {
                            dr.Cells["Parameter"].Value = dr.Cells["Parameter"].Value.ToString().Substring(1, dr.Cells["Parameter"].Value.ToString().Length - 1);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 20140827 節點多語系英文
        /// </summary>
        /// <returns></returns>
        private StringBuilder Pro_English() {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("Prog_");
            sb.Append(tb_className.Text);
            sb.Append("Browse");
            sb.Append("\t");
            sb.Append(tb_className.Text);
            sb.Append("\t");
            sb.Append(tb_scrib.Text);
            return sb;
        }


        /// <summary>
        /// 20140827 頁籤多語系英文
        /// </summary>
        /// <returns></returns>
        private StringBuilder Browse_English(string pPageName,string pDecription) {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append(string.Format("Browse_QueryProject_{0}_{1}\t{1}\t{2}", tb_className.Text, pPageName, pDecription));
            //sb.Append(tb_className.Text);
            //sb.Append("_Browse");
            //sb.Append("\t");
            //sb.Append("Browse");
            //sb.Append("\t");
            //sb.Append("瀏覽");
            return sb;
        }

        /// <summary>
        /// 20140827 頁籤多語系繁中
        /// </summary>
        /// <returns></returns>
        private StringBuilder Browse_CHT(string pPageName, string pDecription) {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append(string.Format("Browse_QueryProject_{0}_{1}\t{2}\t{2}", tb_className.Text, pPageName, pDecription));
            //sb.Append("Browse_QueryProject_");
            //sb.Append(tb_className.Text);
            //sb.Append("_Browse");
            //sb.Append("\t");
            //sb.Append("瀏覽");
            //sb.Append("\t");
            //sb.Append("瀏覽");
            return sb;
        }

        /// <summary>
        /// 20140827 頁籤多語系簡中
        /// </summary>
        /// <returns></returns>
        private StringBuilder Browse_CHS(string pPageName, string pDecription) {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append(string.Format("Browse_QueryProject_{0}_{1}\t{2}\t{2}", tb_className.Text, pPageName, translateEncodingByWord(pDecription, true).Trim()));
            //sb.Append("Browse_QueryProject_");
            //sb.Append(tb_className.Text);
            //sb.Append("_Browse");
            //sb.Append("\t");
            //sb.Append("浏览");
            //sb.Append("\t");
            //sb.Append("浏览");
            return sb;
        }


        /// <summary>
        /// 20140827 節點多語系繁體中文
        /// </summary>
        /// <returns></returns>
        private StringBuilder Pro_ChineseTraditional() {
            StringBuilder sb = new StringBuilder();
            sb.Append("Prog_");
            sb.Append(tb_className.Text);
            sb.Append("Browse");
            sb.Append("\t");
            sb.Append(tb_scrib.Text);
            sb.Append("\t");
            sb.Append(tb_scrib.Text);
            return sb;
        }

        /// <summary>
        /// 20140827 節點多語系簡體中文
        /// </summary>
        /// <returns></returns>
        private StringBuilder Pro_ChineseSimplified() {
            StringBuilder sb = new StringBuilder();
            sb.Append("Prog_");
            sb.Append(tb_className.Text);
            sb.Append("Browse");
            sb.Append("\t");
            sb.Append(translateEncodingByWord(tb_scrib.Text, true).Trim());
            sb.Append("\t");
            sb.Append(translateEncodingByWord(tb_scrib.Text, true).Trim());
            return sb;
        }

        /// <summary>
        /// 20140827 節點多語系英文按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 英文ToolStripMenuItem4_Click(object sender, EventArgs e) {
            richTextBox1.Clear();
            richTextBox1.AppendText("Prog_");       
            richTextBox1.AppendText(tb_className.Text);          
            richTextBox1.AppendText("Browse");          
            richTextBox1.AppendText("\t");            
            richTextBox1.AppendText(tb_className.Text);           
            richTextBox1.AppendText("\t");           
            richTextBox1.AppendText(tb_scrib.Text);            
        }

        /// <summary>
        /// 20140827 節點多語系 繁中按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 繁中ToolStripMenuItem2_Click(object sender, EventArgs e) {
            richTextBox1.Clear();
            richTextBox1.AppendText("Prog_");
            richTextBox1.AppendText(tb_className.Text);
            richTextBox1.AppendText("Browse");
            richTextBox1.AppendText("\t");
            richTextBox1.AppendText(tb_scrib.Text);
            richTextBox1.AppendText("\t");
            richTextBox1.AppendText(tb_scrib.Text);
        }

        /// <summary>
        /// 20140827 節點多語系 簡中按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 簡中ToolStripMenuItem3_Click(object sender, EventArgs e) {

            richTextBox1.Clear();
            richTextBox1.AppendText("Prog_");
            richTextBox1.AppendText(tb_className.Text);
            richTextBox1.AppendText("Browse");
            richTextBox1.AppendText("\t");
            richTextBox1.AppendText(translateEncodingByWord(tb_scrib.Text, true).Trim());
            richTextBox1.AppendText("\t");
            richTextBox1.AppendText(translateEncodingByWord(tb_scrib.Text, true).Trim());

        }


        /// <summary>
        /// 20140827 Title 多語系 英文按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 英文ToolStripMenuItem3_Click(object sender, EventArgs e) {
            richTextBox1.Clear();
            richTextBox1.AppendText(tb_className.Text);
            richTextBox1.AppendText("DisplayName");
            richTextBox1.AppendText("\t");
            richTextBox1.AppendText(tb_className.Text);
            richTextBox1.AppendText("\t");
            richTextBox1.AppendText(tb_scrib.Text);
        }

        /// <summary>
        /// 20140827 Title 多語系 繁中按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 繁忠ToolStripMenuItem1_Click(object sender, EventArgs e) {
            richTextBox1.Clear();
            richTextBox1.AppendText(tb_className.Text);
            richTextBox1.AppendText("DisplayName");
            richTextBox1.AppendText("\t");
            richTextBox1.AppendText(tb_scrib.Text);
            richTextBox1.AppendText("\t");
            richTextBox1.AppendText(tb_scrib.Text);
        }

        /// <summary>
        /// 20140827 Title 多語系 簡中按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 簡中ToolStripMenuItem2_Click(object sender, EventArgs e) {
            richTextBox1.Clear();
            richTextBox1.AppendText(tb_className.Text);
            richTextBox1.AppendText("DisplayName");
            richTextBox1.AppendText("\t");
            richTextBox1.AppendText(translateEncodingByWord(tb_scrib.Text, true).Trim());
            richTextBox1.AppendText("\t");
            richTextBox1.AppendText(translateEncodingByWord(tb_scrib.Text, true).Trim());
        }

        private void 生成ToolStripMenuItem1_Click_1(object sender, EventArgs e) {
            CreateServer();
        }

        private void CreateServer() {
            string str = string.Empty;

            #region 功能型
            if(radioButton1.Checked) {
                str = @"
             #region  功能型权限设置
            public override AccreditedPower PowerType {
             get {
                return AccreditedPower.Functional;
               }
            }
             #endregion
              ";
            }
            #endregion


            #region 人員型
            if(radioButton2.Checked) {
                str = @"
             #region 人员型权限设置
        public override AccreditedPolicy PolicyType {
            get {
                return AccreditedPolicy.EmployeePolicy;
            }
        }

        public override AccreditedPower PowerType {
            get {
                return AccreditedPower.Omnipotent;
            }
        }
        #endregion

              ";
            }

            #endregion

            #region 公司型权限设置
            if(radioButton3.Checked) {
                str = @"
               #region 公司型权限设置
        public override AccreditedPolicy PolicyType
        {
            get
            {
                return AccreditedPolicy.BusinessUnitPolicy;
            }
        }" +

         "public override string SourceName \r\n" +
          " {\r\n" +
              " get\r\n" +
               "{\r\n" +
                  " return \"CorporationId\";\r\n" +
               "}\r\n" +
          "}\r\n" +

          @" public override AccreditedPower PowerType
        {
            get
            {
                return AccreditedPower.Omnipotent;
            }
        }
        #endregion ";
            }
            #endregion
            CreateServer(str);
        }

        private void 一鍵生成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControlDetail.TabPages.Count <= 3)
            {
                SingleFile(); //單檔
            }
            else
            {
                MulitFile(); //雙檔
            }
        }

        /// <summary>
        /// 20141002 雙檔一鍵生成功能
        /// </summary>
        private void MulitFile() {
            string ProgamPath = GetSettinhPath();   //取得設定的資料夾位置
            string ParentPath = Directory.GetParent(ProgamPath).FullName;
            OneClick oneclick = new OneClick(ParentPath, ProgamPath);
            OneClickMultiFile multiclick = new OneClickMultiFile(ParentPath, ProgamPath);
            List<Detail> detailLis = new List<Detail>();
            #region 20141028 add by Dick for 建立明細實體
            foreach (TabPage page in tabControlDetail.TabPages)
            {
                if (!page.Name.Equals("tabPage4") & !page.Name.Equals("tabIncrease") & !page.Name.Equals("tabSub"))
                {                    
                    foreach(Control col in page.Controls)
                    {
                      if(col.GetType().Name.Equals("DataGridView"))
                      {
                          DataGridView grid =col as DataGridView;
                          CreateEntities(grid, true, page.Text, page.ToolTipText, new List<Detail> { });
                          Detail detail = new Detail();
                          detail.Name = page.Text;
                          detail.Description = page.ToolTipText;
                          detailLis.Add(detail);
                          oneclick.CSFileSave("DigiWin.HR.CustomBusiness", "DataEntities", page.Text,richTextBox1.Text);
                          string collection = multiclick.CreateCollection(page.Text);
                          oneclick.CSFileSave("DigiWin.HR.CustomBusiness", "CollectionClass", page.Text + "Collection", collection);
                      }
                    }
                    
                }
            }
            #endregion

            #region 20141028 add by Dick for 建立主表實體
            CreateEntities(dataGridView1, false, tb_className.Text, tb_scrib.Text, detailLis);
            oneclick.CSFileSave("DigiWin.HR.CustomBusiness", "DataEntities", tb_className.Text, richTextBox1.Text);
            #endregion

            string InterFace = "I" + tb_className.Text.Substring(1, tb_className.TextLength - 1) + "ServiceX";
            CreateInterFace();
            oneclick.CSFileSave("DigiWin.HR.CustomBusiness", "Services", InterFace, this.richTextBox1.Text);
            CreateServer();
            oneclick.CSFileSave("DigiWin.HR.CustomBusinessImplement", "Services", tb_className.Text.Substring(1, tb_className.TextLength - 1) + "ServiceX", this.richTextBox1.Text);
            #region 20141003 modified by Dick for 修改成多頁籤
            oneclick.CreateQueryView(tb_className.Text, this.richTextBox1.Text, this.dataGridView1);
            List<DataGridView> GridViewList = new List<DataGridView>();
            foreach (TabPage page in tabControl1.TabPages)
            {
                switch (page.Name)
                {
                    case "tabPage2":
                    case "tabPage3":
                        break;
                    default:
                        foreach (Control control in page.Controls)
                        {
                            if (control.GetType().Name.Equals("DataGridView"))
                            {
                                DataGridView Gridview = control as DataGridView;
                                Gridview.Name = page.Text;
                                GridViewList.Add(Gridview);
                            }
                        }
                        break;
                }
            }
            if (GridViewList.Count > 0)
            {
                oneclick.CreateQueryView(tb_className.Text, this.richTextBox1.Text, GridViewList, DicQueryView);
            }
            #endregion
            System.Data.DataTable dt = dataGridView1.DataSource as System.Data.DataTable;
            oneclick.AppendDataEntityDisplayInfo(dt, tb_className.Text);//加入自訂列多語系
            oneclick.RegisterEntity(tb_className.Text);//註冊實體

            #region QueryView 多語系
            QueryResource(oneclick, dt);
            #endregion

            #region 20140905 add by Dick  加入瀏覽頁籤多語系

            foreach (QueryViewCondition conditions in DicQueryView.Values)
            {
                if (!conditions.Type.Equals("Select"))
                { //20141007 add by Dick for Select 不需要多語系  
                    oneclick.AddResourceRow("DigiWin.HR.CustomBusinessImplement", Browse_English(conditions.BrowseName, conditions.BrowseName).ToString(), "QueryResourcesForCase", true);
                    oneclick.AddResourceRow("DigiWin.HR.CustomBusinessImplement", Browse_CHT(conditions.BrowseName, conditions.Description).ToString(), "QueryResourcesForCase.zh-CHT", false);
                    oneclick.AddResourceRow("DigiWin.HR.CustomBusinessImplement", Browse_CHS(conditions.BrowseName, conditions.Description).ToString(), "QueryResourcesForCase.zh-CHS", false);
                }
            }
            #endregion
            #region 樹節點多語系
            //tasks[0]= System.Threading.Tasks.Task.Factory.StartNew(() => TreeResource(oneclick));           
            //TreeResource(oneclick);
            #endregion


            #region  20141225 add by Dick for 加入Permission
            DirectoryInfo DirPro = new DirectoryInfo(ProgamPath);
            string PermissionPath = DirPro.Parent.FullName +Path.DirectorySeparatorChar+ "DigiWin.HR.CustomBusinessImplement" + Path.DirectorySeparatorChar + "Configuration" + Path.DirectorySeparatorChar + "PermissionForCase.xml";
            string ActionString = string.Empty;
            if (radioButton1.Checked)
            {
                ActionString = "BaseActions";
            }
            if (radioButton2.Checked)
            {
                ActionString = "CorpActions";
            }
            if (radioButton3.Checked)
            {
                ActionString = "EmpActions";
            }

            oneclick.WritePermissionFile(PermissionPath, tb_className.Text, CB_Modules.Text, CB_SubModule.Text, ActionString, Modules, EnToCHT, CHTToEn);
            #endregion

            MessageBox.Show("生成完成");
        }

        /// <summary>
        /// 20140919 modified by Dick 
        /// 切開單檔一鍵生成
        /// </summary>
        private void SingleFile() {
            string ProgamPath = GetSettinhPath();
            string ParentPath = Directory.GetParent(ProgamPath).FullName;
            OneClick oneclick = new OneClick(ParentPath, ProgamPath);
            CreateEntities(dataGridView1, false, tb_className.Text, tb_scrib.Text, new List<Detail> { });            
            oneclick.CSFileSave("DigiWin.HR.CustomBusiness", "DataEntities", tb_className.Text, this.richTextBox1.Text);
            string InterFace = "I" + tb_className.Text.Substring(1, tb_className.TextLength - 1) + "ServiceX";
            CreateInterFace();
            oneclick.CSFileSave("DigiWin.HR.CustomBusiness", "Services", InterFace, this.richTextBox1.Text);
            CreateServer();
            oneclick.CSFileSave("DigiWin.HR.CustomBusinessImplement", "Services", tb_className.Text.Substring(1, tb_className.TextLength - 1) + "ServiceX", this.richTextBox1.Text);
            #region 20141003 modified by Dick for 修改成多頁籤
            oneclick.CreateQueryView(tb_className.Text, this.richTextBox1.Text, this.dataGridView1); 
            List<DataGridView> GridViewList = new List<DataGridView>();            
            foreach(TabPage page in tabControl1.TabPages)
            {
                switch(page.Name)
                {                   
                    case "tabPage2":
                    case "tabPage3":
                        break;
                    default:
                        foreach(Control control in page.Controls) {
                            if(control.GetType().Name.Equals("DataGridView")) {
                                DataGridView Gridview = control as DataGridView;
                                Gridview.Name = page.Text;
                                GridViewList.Add(Gridview);
                            }
                        }
                    break;
                }
            }
            if(GridViewList.Count > 0) {
                oneclick.CreateQueryView(tb_className.Text, this.richTextBox1.Text, GridViewList, DicQueryView); 
            }            
            #endregion    
            System.Data.DataTable dt = dataGridView1.DataSource as System.Data.DataTable;
            oneclick.AppendDataEntityDisplayInfo(dt, tb_className.Text);
            oneclick.RegisterEntity(tb_className.Text);

            int mode = 0;
            foreach(Control co in groupBoxMode.Controls) {
                RadioButton cb = co as RadioButton;
                if(cb.Checked) {
                    mode = Convert.ToInt32(cb.Text.Replace("Mode", ""));
                }
            }
            ///20140905 建立檔單UI           
            oneclick.CreateentityNoDetailBrowseEditViewV5(tb_className.Text, dataGridView1, mode);

            ///20140827 多執行緒
            //System.Threading.Tasks.Task[] tasks = new System.Threading.Tasks.Task[1];

            #region 樹節點多語系
            //tasks[0]= System.Threading.Tasks.Task.Factory.StartNew(() => TreeResource(oneclick));           
            TreeResource(oneclick);
            #endregion


            #region 加入標題多語系

            TitleResource(oneclick);
            #endregion

            #region QueryView 多語系
            QueryResource(oneclick, dt);
            #endregion

            #region 20140905 add by Dick  加入瀏覽頁籤多語系

            foreach( QueryViewCondition conditions in   DicQueryView.Values)
            {
                if(!conditions.Type.Equals("Select")) { //20141007 add by Dick for Select 不需要多語系
                    oneclick.AddResourceRow("DigiWin.HR.CustomBusinessImplement", Browse_English(conditions.BrowseName, conditions.BrowseName).ToString(), "QueryResourcesForCase", true);
                    oneclick.AddResourceRow("DigiWin.HR.CustomBusinessImplement", Browse_CHT(conditions.BrowseName, conditions.Description).ToString(), "QueryResourcesForCase.zh-CHT", false);
                    oneclick.AddResourceRow("DigiWin.HR.CustomBusinessImplement", Browse_CHS(conditions.BrowseName, conditions.Description).ToString(), "QueryResourcesForCase.zh-CHS", false);
                }
            }
            #endregion


            #region  UI多語系
            string SourcePath = AppDomain.CurrentDomain.BaseDirectory + @"\SampleFile\Resource.zh-CHT.resx";
            string DestinationPath = ParentPath + "\\" + "DigiWin.HR.CustomUI" + Path.DirectorySeparatorChar + tb_className.Text + ".zh-CHT.resx";
            File.Copy(SourcePath, DestinationPath, true);
            SourcePath = AppDomain.CurrentDomain.BaseDirectory + @"\SampleFile\Resource.zh-CHS.resx";
            DestinationPath = ParentPath + "\\" + "DigiWin.HR.CustomUI" + Path.DirectorySeparatorChar + tb_className.Text + ".zh-CHS.resx";
            File.Copy(SourcePath, DestinationPath, true);
            UIResource(oneclick, tb_className.Text, ParentPath + "\\" + "DigiWin.HR.CustomUI" + Path.DirectorySeparatorChar);


            #endregion


            #region  20141225 add by Dick for 加入Permission
            DirectoryInfo DirPro = new DirectoryInfo(ProgamPath);
            string PermissionPath = DirPro.Parent.FullName + Path.DirectorySeparatorChar + "DigiWin.HR.CustomBusinessImplement" + Path.DirectorySeparatorChar + "Configuration" + Path.DirectorySeparatorChar + "PermissionForCase.xml";
            string ActionString=string.Empty;
            if (radioButton1.Checked)
            {
                ActionString = "BaseActions";
            }
            if(radioButton2.Checked)
            {
            ActionString="CorpActions";
            }
            if (radioButton3.Checked)
            {
            ActionString="EmpActions";
            }

            oneclick.WritePermissionFile(PermissionPath, tb_className.Text, CB_Modules.Text, CB_SubModule.Text, ActionString, Modules, EnToCHT, CHTToEn);
            #endregion

            //System.Threading.Tasks.Task.WaitAll(tasks);
            MessageBox.Show("完成!!");
        }

        /// <summary>
        /// 20141006 add by Dick 取得設定的路徑
        /// </summary>
        /// <returns></returns>
        private static string GetSettinhPath() {
            XmlDocument doc = FileTool.XmlFile.LoadXml(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Path.xml");
            XmlNode root = doc.SelectSingleNode("root");
            string ProgamPath = string.Empty;
            foreach(XmlNode node in root.ChildNodes) {
                if(Convert.ToBoolean(node.Attributes["Set"].Value)) {
                    ProgamPath = node.Attributes["Xpath"].Value.ToString();
                }
            }
            return ProgamPath;
        }

        private void TreeResource(OneClick oneclick) {          
            oneclick.AddResourceRow("DigiWin.HR.CustomUI", Pro_English().ToString(), "ResourcesForCase", true);      
            oneclick.AddResourceRow("DigiWin.HR.CustomUI", Pro_ChineseTraditional().ToString(), "ResourcesForCase.zh-CHT", false);
            oneclick.AddResourceRow("DigiWin.HR.CustomUI", Pro_ChineseSimplified().ToString(), "ResourcesForCase.zh-CHS", false);
        }

        private void QueryResource(OneClick oneclick, System.Data.DataTable dt) {
            StringBuilder English = new StringBuilder();
            English = ResourceQueryView("EN", "Browse_QueryView", false, English);
            string[] Enstr = English.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            oneclick.AddQueryView(dt, "DigiWin.HR.CustomBusinessImplement", Enstr, "QueryResourcesForCase", true);
            StringBuilder CHT = new StringBuilder();
            CHT = ResourceQueryView("CHT", "Browse_QueryView", false, CHT);
            string[] CHTstr = CHT.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);            
            oneclick.AddQueryView(dt, "DigiWin.HR.CustomBusinessImplement", CHTstr, "QueryResourcesForCase.zh-CHT", false);  
            StringBuilder CHS = new StringBuilder();
            CHS = ResourceQueryView("CHS", "Browse_QueryView", false, CHS);
            string[] CHSstr = CHS.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            oneclick.AddQueryView(dt, "DigiWin.HR.CustomBusinessImplement", CHSstr, "QueryResourcesForCase.zh-CHS", false);
        }

        private void TitleResource(OneClick oneclick) {
            英文ToolStripMenuItem3.PerformClick();
            oneclick.AddResourceRow("DigiWin.HR.CustomUI", richTextBox1.Text, "ResourcesForCase", true);
            繁忠ToolStripMenuItem1.PerformClick();
            oneclick.AddResourceRow("DigiWin.HR.CustomUI", richTextBox1.Text, "ResourcesForCase.zh-CHT", false);
            簡中ToolStripMenuItem2.PerformClick();
            oneclick.AddResourceRow("DigiWin.HR.CustomUI", richTextBox1.Text, "ResourcesForCase.zh-CHS", false);
        }

        /// <summary>
        /// 20140913 UI多語系
        /// </summary>
        /// <param name="oneclick"></param>
        /// <param name="FileName"></param>
        private void UIResource(OneClick oneclick,string EntityName,string FilePath) {
            繁中ToolStripMenuItem3.PerformClick();
            SubUIResource(EntityName, FilePath, ".zh-CHT.resx");        
            簡中ToolStripMenuItem4.PerformClick();
            SubUIResource(EntityName, FilePath, ".zh-CHS.resx");
        }

        /// <summary>
        /// 20140913 加入多語系
        /// </summary>
        /// <param name="EntityName"></param>
        /// <param name="FilePath"></param>
        /// <param name="Extend"></param>
        private void SubUIResource(string EntityName, string FilePath,string Extend) {
            string[]arry = richTextBox1.Text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if(arry.Length > 0) {
                FilePath += EntityName + Extend;
                foreach(string str in arry) {
                    string[] spl = str.Split('\t');
                    OneClick.AddResource(FilePath, spl);
                }
            }
        }



        /// <summary>
        /// 20140523 ReferenceProperty  自動帶入值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e) {
            if(dataGridView1.Rows[0].Cells[e.ColumnIndex].OwningColumn.Name.Equals("ReferenceProperty")) {
                if(e.RowIndex > 0) {
                    if(!string.IsNullOrEmpty(dataGridView1.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value.ToString())) {
                        DataRow[] dr = RefereceTable.Select(string.Format(@"Entity like '{0}%'", dataGridView1.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].FormattedValue));
                        if(dr.Length > 0) {
                            dataGridView1.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value = dr[0].ItemArray[0].ToString();
                        }
                    }
                }
            }
        }

        private void 開啟新檔ToolStripMenuItem_Click(object sender, EventArgs e) {
            tb_className.Text = string.Empty;
            tb_scrib.Text = string.Empty;
            System.Data.DataTable dt = dataGridView1.DataSource as System.Data.DataTable;
            dt.Clear();
        }

        private void 英文ToolStripMenuItem5_Click(object sender, EventArgs e) {
            foreach(DataGridViewRow dr in dataGridView1.Rows) {
                if(dr.Cells["UIOrder"] != null) {
                    if(!string.IsNullOrEmpty(dr.Cells["UIOrder"].ToString())) {
                        if(dr.Cells["UIOrder"].Value != null) { 
                        
                        }
                    }
                }
            }
        }
       

        private void 繁中ToolStripMenuItem3_Click(object sender, EventArgs e) {
            richTextBox1.Clear();
            foreach(DataGridViewRow dr in dataGridView1.Rows) {
                if(dr.Cells["UIOrder"] != null) {
                    if(!string.IsNullOrEmpty(dr.Cells["UIOrder"].ToString())) {
                        if(dr.Cells["UIOrder"].Value != null) {
                            int order =0;
                            if(int.TryParse(dr.Cells["UIOrder"].Value.ToString(),out order))
                            {                                
                                if(order != 0) {
                                    if(dr.Cells["Type"].Value.ToString().ToLower() != "bool") {
                                        richTextBox1.AppendText(string.Format("{0}Label1.Text\t{1}:\t{2} \r\n", dr.Cells["Parameter"].Value.ToString(), dr.Cells["Describe"].Value.ToString(), dr.Cells["Describe"].Value.ToString()));
                                    }
                                    else {
                                     //   richTextBox1.AppendText(string.Format("dcmsCheckEdit{0}.Text\t{1}:\t{2} \r\n", dr.Cells["Parameter"].Value.ToString(), dr.Cells["Describe"].Value.ToString(), dr.Cells["Describe"].Value.ToString()));
                                    }
                                }
                                if(order == -1) {
                                    richTextBox1.AppendText(string.Format("groupBox2.Text\t{0}:\t{1} \r\n", "備註", "備註"));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void 簡中ToolStripMenuItem4_Click(object sender, EventArgs e) {
            richTextBox1.Clear();
            foreach(DataGridViewRow dr in dataGridView1.Rows) {
                if(dr.Cells["UIOrder"] != null) {
                    if(!string.IsNullOrEmpty(dr.Cells["UIOrder"].ToString())) {
                        if(dr.Cells["UIOrder"].Value != null) {
                            int order = 0;
                            if(int.TryParse(dr.Cells["UIOrder"].Value.ToString(), out order)) {
                                if(order != 0) {
                                    string temp = translateEncodingByWord(dr.Cells["Describe"].Value.ToString(), true).Trim();
                                    if(dr.Cells["Type"].Value.ToString().ToLower() != "bool") {
                                        richTextBox1.AppendText(string.Format("{0}Label1.Text\t{1}:\t{2} \r\n", dr.Cells["Parameter"].Value.ToString(), temp, temp));
                                    }
                                    else {
                                      //  richTextBox1.AppendText(string.Format("dcmsCheckEdit{0}.Text\t{1}:\t{2} \r\n", dr.Cells["Parameter"].Value.ToString(), temp, temp));
                                    }
                                }
                                if(order == -1) {
                                    string temp = translateEncodingByWord("備註", true).Trim();
                                    richTextBox1.AppendText(string.Format("groupBox2.Text\t{0}:\t{1} \r\n", temp, temp));
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 20140917 add by Dick 
        /// 加入QueryView頁籤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_Click(object sender, EventArgs e) {
            TabControl tab = (TabControl)sender;
            if(tab.SelectedTab.Text =="+") {
                //加入Browse還有條件
                ConditionView PageName = new ConditionView();
                if(PageName.ShowDialog() == DialogResult.OK) {
                    tab.TabPages.Insert(tab.SelectedIndex, PageName.Result.BrowseName);
                    tab.SelectedIndex = tab.SelectedIndex - 1;
                    TabPage page = tab.SelectedTab;
                    TabAddGridView(tab.SelectedIndex - 1, page);
                    QueryViewCondition BroseCodition = PageName.Result;                    
                    DicQueryView[PageName.Result.BrowseName] = BroseCodition;
                }
            }
            if(tab.SelectedTab.Text == "-") {
                int index = TabSelectedBefore;
                if (TabSelectedBefore != 0)
                {
                    tab.SelectedIndex = TabSelectedBefore - 1;
                    TabPage page = tab.TabPages[index] as TabPage;
                    if (!page.Name.Equals("tabPage1"))
                    {
                        string name = page.Name;
                        tab.TabPages.RemoveAt(index);
                        if(DicQueryView.ContainsKey(name)) {
                            DicQueryView.Remove(name);
                        }                        
                    }
                }
                tab.SelectedIndex = TabSelectedBefore;
            }
        }

        private int TabSelectedIndexNow;
        private int TabSelectedBefore;
        /// <summary>
        /// 20140923 add by Dick 產生新的dataGridView
        /// </summary>
        private void TabAddGridView(int Index, TabPage pPage) {
            System.Data.DataTable dt = dataGridView1.DataSource as System.Data.DataTable;
            DataGridView NewView = new DataGridView();
            NewView.Name = "NewView" + Index.ToString();
            NewView.DataSource = dt;
            NewView.Width = 974;
            NewView.Height = 234;
            NewView.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            var item = NewView.ContextMenuStrip.Items.Add("加入條件");
            item.Click += new EventHandler(item_Click1);                    
            pPage.Controls.Add(NewView);
          
        }

        /// <summary>
        /// 20140923 add by Dick for
        /// 加入右鍵條件功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_Click1(object sender, EventArgs e) {
            TabPage Page = tabControl1.SelectedTab as TabPage;
            QueryViewCondition BroseCodition=null;
            if(DicQueryView.ContainsKey(Page.Text)) {
             BroseCodition  = DicQueryView[Page.Text];
            }
            if(BroseCodition != null) {
                ConditionView conditionview = new ConditionView(BroseCodition);
                if(conditionview.ShowDialog() == DialogResult.OK) {
                    DicQueryView[Page.Text] = conditionview.Result;
                }
            }
            else {
                ConditionView conditionview = new ConditionView();
                if(conditionview.ShowDialog() == DialogResult.OK) {
                    DicQueryView[Page.Text] = conditionview.Result;
                }
            }
        }      

        //20141128 modified by Dick for 修改模式。
        private void queryView排序ToolStripMenuItem_Click(object sender, EventArgs e) {           
            //string ThisPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"XmlOrder.exe");
            //string XMLPath =Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"New.xml");
            //if(!File.Exists(ThisPath)) {
            //    MessageBox.Show("找不到檔案XmlOrder.exe");
            //    return;
            //}
            //Process.Start(ThisPath);
            //Thread.Sleep(1000);
            //if(File.Exists(XMLPath)) {
            //    using(StreamReader sw = new StreamReader(XMLPath)) {
            //        StringBuilder sb = new StringBuilder();
            //        string line = string.Empty;
            //        while((line=sw.ReadLine())!=null)
            //        {
            //            if(line == "<?xml version=\"1.0\" encoding=\"utf-8\"?>") {
            //                continue;
            //            }
            //            if(line == "<root>") {
            //                continue;
            //            }
            //            if(line == "</root>") {
            //                continue;
            //            }
            //            sb.Append(string.Format("{0}\r\n", line));
            //        }
            ShowMessage show = new ShowMessage();
            show.ShowDialog();
            //    }
            //}
        }


        /// <summary>
        /// 20140919 add by Dick 
        /// 加入說明功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 說明ToolStripMenuItem_Click(object sender, EventArgs e) {           
            Description descrip = new Description(this.ProductVersion.ToString());                 
            descrip.ShowDialog();
         }
             

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            TabSelectedBefore = TabSelectedIndexNow;
            TabSelectedIndexNow = tabControl1.SelectedIndex;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.F11) {
                一鍵生成ToolStripMenuItem.PerformClick();
            }           
        }

        private void button2_Click(object sender, EventArgs e) {
            DetailItem detail = new DetailItem();
            detail.Show();
        }      

        private void dataGridView1_DragEnter(object sender, DragEventArgs e) {
            if(e.Data.GetDataPresent("System.Windows.Forms.TreeNode")) {
                e.Effect = DragDropEffects.Move;
            }
            else {
                e.Effect = DragDropEffects.None;
            }  
        }

        /// <summary>
        /// 20141001 add by Dick for  拖曳名細
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_DragDrop(object sender, DragEventArgs e) {
            TreeNode NewNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");            
            bool IsExist = false;
            foreach(TabPage page in tabControlDetail.TabPages) {
                if(page.Text.Equals(NewNode.Text)) {
                    IsExist = true;
                }
            }
            if(!IsExist) {
                tabControlDetail.TabPages.Insert(tabControlDetail.TabPages.Count - 2, NewNode.Text);
                tabControlDetail.SelectedIndex = tabControlDetail.TabPages.Count - 3;
                DataGridView GridView =new DataGridView();
                GridView.Width =1299;
                GridView.Height =292;  
                TabPage Page = tabControlDetail.SelectedTab;
                Page.ToolTipText = NewNode.ToolTipText;//20141028 加入描述
                Page.AutoScroll = true;
                Page.Controls.Add(GridView);
                LoadGridView(NewNode.Text, GridView, false);
            }
        }

       
        /// <summary>
        /// 20141001 add by Dick for  加入名細頁籤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            DetailTabBefore = DetailTabNew;
            DetailTabNew = tabControlDetail.SelectedIndex;
            TabPage Item =tabControlDetail.SelectedTab;           
            if (Item.Name.Equals("tabIncrease"))
            {
                PagesName PageName = new PagesName();
                if(PageName.ShowDialog() == DialogResult.OK) {
                    bool IsExist = false;
                    foreach(TabPage page in tabControlDetail.TabPages) {
                        if(page.Text.Equals(PageName.Name)) {
                            IsExist = true;
                        }
                    }
                    if(!IsExist) {
                        tabControlDetail.TabPages.Insert(tabControlDetail.TabPages.Count - 2, PageName.Name);
                        tabControlDetail.SelectedIndex = tabControlDetail.TabPages.Count - 3;
                    }
                }
                else {
                    tabControlDetail.SelectedIndex = DetailTabBefore;
                }
            }
            if(Item.Name.Equals("tabSub"))
            {
                int index = DetailTabBefore;
                if(DetailTabBefore != 0) {
                    tabControlDetail.SelectedIndex = DetailTabBefore - 1;
                    TabPage page = tabControlDetail.TabPages[DetailTabBefore - 1] as TabPage;
                    if(!page.Name.Equals("tabPage4") ) {
                        tabControlDetail.TabPages.RemoveAt(index);
                    }
                }
                tabControlDetail.SelectedIndex = TabSelectedBefore;
            }
        }

        private void 載入實體ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertEntities dialog = new InsertEntities();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                System.Data.DataTable dt = this.dataGridView1.DataSource as System.Data.DataTable;
                int count = dt.Rows.Count;
                string[] sp = dialog.context.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach(string str in sp)
                {
                    string[] arrary = str.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    DataRow dr = dt.NewRow();
                    dr[0] = arrary[0];                     
                    dr[1] = arrary[2];
                    dt.Rows.Add(dr);
                    dataGridView1.Rows[count].Cells["Type"].Value = ChangeType(arrary[1]);
                    count++;
                }
            }
        }

        private string ChangeType(string str)
        {
            string Tag = str.ToLower();
            if (Tag.IndexOf("nvarchar") != -1 | Tag.IndexOf("string") != -1)
            {
                return "String";
            }
            if (Tag.IndexOf("int") != -1)
            {
                return "Int32";
            }
            if (Tag.IndexOf("decimal") != -1)
            {
                return "Decimal";
            }
            if (Tag.IndexOf("datetime") != -1)
            {
                return "DateTime";
            }
            if (Tag.IndexOf("ntext") != -1)
            {
                return "Ntext";
            }

            if (Tag.IndexOf("guid") != -1 | Tag.IndexOf("uniqueidentifier") != -1)
            {
                return "Guid";
            }

            if (Tag.IndexOf("bool") != -1 | Tag.IndexOf("bit") != -1)
            {
                return "Bool";
            }
            return "";
        }

        /// <summary>
        /// 20141224 add by Dick for 可儲存"權限設定"及"模式設定"(一鍵生成加入Permission) #6
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_Modules_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_Modules.SelectedIndex != -1)
            {
                CB_SubModule.Items.Clear();
                string English = CHTToEn[CB_Modules.Text];
                if (Modules.ContainsKey(English))
                {
                    List<string> li = Modules[English];
                    foreach (string item in li)
                    {
                        CB_SubModule.Items.Add(EnToCHT[item]);
                    }
                }
                else
                {
                    English = English + "Module";
                    if (Modules.ContainsKey(English))
                    {
                        List<string> li = Modules[English];
                        foreach (string item in li)
                        {
                            if(EnToCHT.ContainsKey(item))
                            {
                                CB_SubModule.Items.Add(EnToCHT[item]);
                            }
                        }
                    }
                }
            }
        }
    }    
}