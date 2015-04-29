using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading;
using System.Configuration;
using WebInfo;
using CommTool;


namespace DService
{
    public partial class Service1 : ServiceBase
    {
        private List<string> _timelist = new List<string>();
        Dictionary<string, string> DicParameters = new Dictionary<string, string>();   //參數設定 
        public Service1()
        {
            InitializeComponent();            
        }

        private void Init()
        {
            CommTool.ToolLog.ToolPath = Settings1.Default.LogPath;
            DateTime BaseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime FlagTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day, 0, 0, 0);
            int interval = Convert.ToInt32(Settings1.Default.Interval);
            while (BaseTime <= FlagTime)
            {
                _timelist.Add(BaseTime.ToString("HH:mm:ss"));
                BaseTime = BaseTime.AddSeconds(GetTime() * interval);
            }
            InitParamter();
        }

        private void InitParamter()
        {
            try
            {
                foreach (SettingsProperty PropertyName in Settings1.Default.Properties)
                {
                    DicParameters.Add(PropertyName.Name, Settings1.Default.PropertyValues[PropertyName.Name].PropertyValue.ToString());
                }
            }
            catch (Exception ex)
            {
                CommTool.ToolLog.Log(ex);
            }
        }


        protected override void OnStart(string[] args)
        {
            Init();
            DServerLog("服務啟動");
            DServerLog("服務啟動更新開始...");
            UpdateDll(Settings1.Default.UpDateGradPath);
            DServerLog("服務啟動更新結束...");
            Thread t1 = new Thread(Lessner);
            t1.Start();
            System.Timers.Timer time = new System.Timers.Timer();
            time.Elapsed += new System.Timers.ElapsedEventHandler(time_Elapsed);
            time.Interval = 1000;
            time.Start();
        }

        void time_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            DExecute.DExecute execute = new DExecute.DExecute(DicParameters);
            if (DateTime.Now.ToString("HH:mm") == Settings1.Default.UpDateTime)
            {
                DServerLog("更新開始");
                UpdateDll(Settings1.Default.UpDateGradPath);
                DServerLog("更新結束");
            }
            GetPTTBueaty(time);
        }

        private void GetPTTBueaty(string time)
        {
            if (_timelist.Contains(time))
            {
                DServerLog("執行" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                string LogPath = Settings1.Default.LogPath;
                GetSite site = new GetSite(LogPath);
                string RecordXml = Path.Combine(LogPath,"Record.xml");
                if (!File.Exists(RecordXml))
                {
                    XmlFile xml = new XmlFile();
                    xml.CreateBaseXml(RecordXml, string.Empty, true);
                }
                XmlDocument doc = XmlFile.LoadXml(RecordXml);
                XmlNode root = doc.SelectSingleNode("root");   
                site.PostAddress = Convert.ToBoolean(Settings1.Default.IsTest)  ==true?  Settings1.Default.TestPostAddress :   site.PostAddress = Settings1.Default.PostAddress;
                site.PushCount = Settings1.Default.PushCount;
                site.Tag = Settings1.Default.StartTag;          
                string Site = Settings1.Default.Theme;
                SitePlus siteplus = site.GetUrlList("https://www.ptt.cc/bbs/" + Site + "/index" + site.Tag + ".html");
                DServerLog("取得表特列表" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                List<SiteInfo> SiteInfoList = new List<SiteInfo>();
                site.Recursive(1150, siteplus, SiteInfoList, Site, "/bbs/" + Site + "/index", Settings1.Default.Condition, doc, root);
            }
        }

        protected override void OnStop()
        {
            DServerLog("服務停止");            
        }


        private  void Lessner()
        {
            DExecute.DExecute execute = new DExecute.DExecute(DicParameters);
            execute.Start();
        }
        
        /// <summary>
        /// 20141219 add by Dick for 取得時間單位轉換
        /// </summary>
        /// <returns></returns>
        private int GetTime()
        { 
            int Result =0;
            switch(Settings1.Default.IntervalUnit.ToLower())
            {
                case "s":
                    Result = 1;
                    break;
                case"m":
                    Result = 60;
                    break;
                case "h":
                    Result = 3600;
                    break;
                default:

                    break;
            }
            return Result;
        }


        /// <summary>
        /// 20141219 add by Dick for 更新檔案
        /// </summary>
        /// <param name="UpdatePath"></param>
        public virtual void UpdateDll(string UpdatePath)
        {
            try
            {
                DirectoryInfo LocalDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                List<string> LocalFileList = new List<string>();
                foreach (FileInfo FiInfo in LocalDir.GetFiles())
                {
                    LocalFileList.Add(FiInfo.Name);
                }
                DirectoryInfo RemoveDir = new DirectoryInfo(UpdatePath);
                foreach (FileInfo FiInfo in RemoveDir.GetFiles())
                {
                    if (FiInfo.Name.Equals("FileTool.dll"))
                    {
                        continue;
                    }
                    if (!LocalFileList.Contains(FiInfo.Name))
                    {
                        File.Copy(FiInfo.FullName, AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + FiInfo.Name, true);
                    }
                    else
                    {
                        if (FiInfo.Extension.Equals(".xml"))
                        {
                            try
                            {
                                XmlDocument SourceDoc = LoadXml(FiInfo.FullName);
                                XmlNode root = SourceDoc.SelectSingleNode("root");
                                XmlDocument DeInfo = LoadXml(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + FiInfo.Name);
                                XmlNode deroot = DeInfo.SelectSingleNode("root");
                                if (root != null && deroot != null)
                                {
                                    if (!root.Attributes["Version"].Value.Equals(deroot.Attributes["Version"].Value))
                                    {
                                        try
                                        {
                                            File.Copy(FiInfo.FullName, AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + FiInfo.Name, true);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            FileVersionInfo SourceFile = FileVersionInfo.GetVersionInfo(FiInfo.FullName); //遠端檔案
                            FileVersionInfo Info = FileVersionInfo.GetVersionInfo(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + FiInfo.Name); //本地檔案
                            if (SourceFile.FileVersion != null)
                            {
                                if (!SourceFile.FileVersion.Equals(Info.FileVersion))
                                {
                                    File.Copy(SourceFile.FileName, Info.FileName, true);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DServerLog(ex.Message);
            }
        }

        /// <summary>
        /// 讀取Xml檔案
        /// </summary>
        /// <param name="pPath">輸入讀取路徑</param>
        /// <returns></returns>
        public  XmlDocument LoadXml(string pPath)
        {
            XmlDocument doc = new XmlDocument();
            System.IO.StreamReader sr = new System.IO.StreamReader(pPath);
            doc.Load(sr);
            sr.Close();
            sr.Dispose();
            return doc;
        }

        public static void DServerLog(string Message)
        {
             CommTool.ToolLog.Log(Message);          
        }
    }
}
