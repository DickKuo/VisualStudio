using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ComponentModel;

namespace FileTool
{
    public class XmlFile
    {
        #region 私有屬性
        private string _path;
        private string _result;
        #endregion

        #region 屬性

        public string Path
        {
            set
            {
                _path = value;
            }
            get
            {
                return _path;
            }
        }

        public string Result
        {
            set
            {
                _result = value;
            }
            get
            {
                return _result;
            }
        }
        #endregion


        #region 建構函式
        public XmlFile()
        {
        }

        public XmlFile(string path)
        {
            Path = path;
        }

        #endregion

        #region 公開函式

        /// <summary>
        /// 讀取Xml檔案
        /// </summary>
        /// <returns></returns>
        public virtual XmlDocument XmlLoad()
        {
            XmlDocument doc = new XmlDocument();
            System.IO.StreamReader sr = new System.IO.StreamReader(Path);
            doc.Load(sr);
            sr.Close();
            sr.Dispose();
            return doc;
        }

        /// <summary>
        /// 讀取所有節點
        /// </summary>
        /// <param name="Att">屬性名稱</param>
        /// <returns></returns>
        public virtual List<string> XmlLoadList(string Att)
        {
            List<string> li = new List<string>();
            XmlDocument doc = XmlLoad();
            XmlNode root = doc.SelectSingleNode("root");
            foreach (XmlNode child in root.ChildNodes)
            {
                li.Add(child.Attributes[Att].Value);
            }
            return li;
        }

        /// <summary>
        /// 讀取所有節點
        /// </summary>     
        /// <returns></returns>
        public virtual List<string> XmlLoadList()
        {
            List<string> li = new List<string>();
            XmlDocument doc = XmlLoad();
            XmlNode root = doc.SelectSingleNode("root");
            foreach (XmlNode child in root.ChildNodes)
            {
                li.Add(child.Attributes[0].Value);
            }
            return li;
        }

        /// <summary>
        /// 加入單一屬性的節點
        /// </summary>
        /// <param name="NodeName"></param>
        /// <param name="Att"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual void XmlAddNode(string NodeName, string Att, string value)
        {
            XmlDocument doc = XmlLoad();
            XmlNode root = doc.SelectSingleNode("root");
            XmlElement element = doc.CreateElement(NodeName);
            element.SetAttribute(Att, value);
            root.AppendChild(element);
            doc.Save(Path);
        }


        public virtual XmlNode GetNodeByName(string proot,string Node)
        {
            XmlDocument doc = XmlLoad();
            XmlNode root = doc.SelectSingleNode(proot);
            foreach (XmlNode child in root.ChildNodes)
            {
                if (child.Name == Node)
                {
                    return child;
                }
                else
                {
                    GetNodeByName(child.Name, Node);
                }
            }
            return null;
        }

        /// <summary>
        ///  加入多屬性節點
        /// </summary>
        /// <param name="NodeName">結點名稱</param>
        /// <param name="Att">屬性</param>
        /// <returns></returns>
        [Description("新增節點，多屬性方法")]
        public virtual void XmlAddNode(string NodeName, Dictionary<string, string> Att)
        {
            XmlDocument doc = XmlLoad();
            XmlNode root = doc.SelectSingleNode("root");
            XmlElement element = doc.CreateElement(NodeName);
            foreach (var key in Att)
            {
                element.SetAttribute(key.Key, key.Value);
            }
            root.AppendChild(element);
            doc.Save(Path);
        }

        /// <summary>
        /// 刪除指定Node
        /// </summary>
        /// <param name="ID">NodeID</param>
        public virtual string XmlDeleteNode(string ID)
        {
            try
            {
                XmlDocument doc = XmlLoad();
                XmlNode root = doc.SelectSingleNode("root");
                XmlNode element = doc.GetElementById(ID);
                root.RemoveChild(element);
                doc.Save(Path);
                ToolLog tools = new ToolLog();
                tools.Log(LogType.Delete, "XmlNode ID：" + ID);
                return "ok";
            }
            catch (Exception ex)
            {
                ToolLog tools = new ToolLog();
                tools.Log(ex);
                return ex.ToString();
            }
        }

        /// <summary>
        /// 刪除List
        /// </summary>
        /// <param name="ID">NodeID</param>
        public virtual string XmlDeleteNode(List<string> li)
        {
            try
            {
                XmlDocument doc = XmlLoad();
                XmlNode root = doc.SelectSingleNode("root");
                StringBuilder SB = new StringBuilder();
                foreach (string str in li)
                {
                    XmlNode element = doc.GetElementById(str);
                    SB.Append(str);
                    SB.Append(",");
                    root.RemoveChild(element);
                }
                doc.Save(Path);
                ToolLog tools = new ToolLog();
                tools.Log(LogType.Delete, "XmlNode ID：" + SB.ToString().Substring(0, SB.Length - 2));
                return "ok";
            }
            catch (Exception ex)
            {
                ToolLog tools = new ToolLog();
                tools.Log(ex);
                return ex.ToString();
            }
        }

        /// <summary>
        /// 刪除arry
        /// </summary>
        /// <param name="ID">NodeID</param>
        public virtual void XmlDeleteNodeArry(string arry)
        {
            XmlDocument doc = XmlLoad();
            XmlNode root = doc.SelectSingleNode("root");
            string[] li = arry.Split(',');
            StringBuilder SB = new StringBuilder();
            foreach (string str in li)
            {
                XmlNode element = doc.GetElementById(str);
                SB.Append(str);
                SB.Append(",");
                root.RemoveChild(element);
            }
            doc.Save(Path);
            ToolLog tools = new ToolLog();
            tools.Log(LogType.Delete, "XmlNode ID：" + SB.ToString().Substring(0, SB.Length - 2));
        }


        /// <summary>
        /// 檢查節點是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public virtual bool XmlNodeIsExist(string ID)
        {
            bool IsExist = false;
            try
            {
                XmlDocument doc = XmlLoad();
                XmlNode element = doc.GetElementById(ID);
                if (element != null)
                {
                    IsExist = true;
                }
            }
            catch { }

            return IsExist;
        }

        #endregion


        ~XmlFile()
        {
        }
    }

}
