using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommTool.Business;
using System.IO;
using CommTool;
using System.Xml;

namespace CommTool
{
    public class ConfigManager : IConfigManager
    {
        private class Default {
            public const string name = "name";
        }

        private Dictionary<string, string> _dicParameter;

        private string _typeKey;
        
        /// <summary>Config檔案路徑</summary>
        public string ConfigPath { set; get; }

        /// <summary>應用程式名稱</summary>
        public string TypeKey { set { _typeKey = value; } get { return _typeKey; } }

        /// <summary>建立結構</summary>
        /// <param name="pPath">路徑</param>
        /// <param name="AppName">應用程式名稱</param>
        public ConfigManager(string pPath, string AppName, bool IsStandard = false) {
            ConfigPath = pPath;
            _dicParameter = new Dictionary<string, string>();
            _typeKey = AppName;
            if (!IsStandard) {
                Init();
            }
            else {
                Initkey();
            }
        }

        /// <summary>初始化參數</summary>
        private void Initkey() {
            if (File.Exists(ConfigPath)) {
                XmlDocument doc = XmlFile.LoadXml(ConfigPath);
                XmlNode nodelist = doc.SelectSingleNode(string.Format("appSettings", _typeKey));
                foreach (XmlNode node in nodelist) {
                    try {
                        string ParameterName = node.Attributes[0].Value;
                        //XmlNode Child = node.FirstChild;
                        //string value = Child.InnerText;
                        if (!_dicParameter.ContainsKey(ParameterName)) {
                            _dicParameter.Add(ParameterName, node.Attributes[1].Value);
                        }
                    }
                    catch { }

                }
            }
            else {
                ToolLog.Log("Config檔案不存在。");
            }
        }

        /// <summary>初始化參數</summary>
        private void Init() {
            if (File.Exists(ConfigPath)) {
                XmlDocument doc = XmlFile.LoadXml(ConfigPath);
                XmlNode nodelist = doc.SelectSingleNode(string.Format("configuration/userSettings/{0}.Settings1", _typeKey));
                foreach (XmlNode node in nodelist) {
                    string ParameterName = node.Attributes[Default.name].Value;
                    XmlNode Child = node.FirstChild;
                    string value = Child.InnerText;
                    if (!_dicParameter.ContainsKey(ParameterName)) {
                        _dicParameter.Add(ParameterName, value);
                    }
                }
            }
            else {
                ToolLog.Log("Config檔案不存在。");
            }
        }

        /// <summary>取得參數的值</summary>
        /// <param name="pParameter">參數名稱</param>
        /// <returns></returns>
        public string GetValue(string pParameter) {
            if (_dicParameter.ContainsKey(pParameter)) {
                return _dicParameter[pParameter];
            }
            else {
                return string.Empty;
            }
        }

        /// <summary>設定參數的值</summary>
        /// <param name="pParameter">參數名稱</param>
        /// <param name="value">值</param>
        public void SetValue(string pParameter, string value) {
            XmlDocument Config = XmlFile.LoadXml(ConfigPath);
            XmlNode node = Config.SelectSingleNode(string.Format("configuration/userSettings/{1}.Settings1/setting[@name='{0}']", pParameter, _typeKey));
            XmlNode child = node.ChildNodes[0];
            child.InnerText = value;
            Config.Save(ConfigPath);
        }

        /// <summary>取得參數字典</summary>
        /// <returns></returns>
        public Dictionary<string, string> GetParamters() {
            return _dicParameter;
        }
    }
}
