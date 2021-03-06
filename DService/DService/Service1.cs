﻿using System.Collections.Generic;
using System.ServiceProcess;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using DStandardServer;
using System.Xml;
using System.IO;
using CommTool;
using System;

namespace DService
{
    public partial class Service1 : ServiceBase
    {
        private class Default
        {
            public const int Second = 1000;
            public const int Interval = Second * 1;
            public const string Root = "root";
            public const string Version = "Version";
            public const string ShortTimeFormat = "ShortTimeFormat";
            public const string XML = ".xml";
            public const string UpDateGradPath = "UpDateGradPath";
            public const string ServerBakPath = "ServerBakPath";
        }

        private CommTool.TriggerService server = null;
        private CommTool.ConfigManager configmanage;
        private List<string> _timelist = new List<string>();
        Dictionary<string, string> DicParameters = new Dictionary<string, string>();   //參數設定 

        public Service1()
        {
            InitializeComponent();
        }

        private void Init()
        {
            ServiceLog.ToolLog.ToolPath = Settings1.Default.LogPath;
            InitParamter();
        }

        private void InitParamter()
        {
            try
            {
                foreach (SettingsProperty PropertyName in Settings1.Default.Properties)
                {
                    ServiceLog.ToolLog.Log(string.Format("初始化參數：{0}", PropertyName.Name));
                    DicParameters.Add(PropertyName.Name, Settings1.Default.PropertyValues[PropertyName.Name].PropertyValue.ToString());
                }
            }
            catch (Exception ex)
            {
                ServiceLog.ToolLog.Log(ex);
            }
        }

        protected override void OnStart(string[] args)
        {
            ServiceLog.ToolLog.Log("服務啟動");
            Init();
            string configiPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DService.exe.config");
            System.Timers.Timer time = new System.Timers.Timer();
            configmanage = new ConfigManager(configiPath, "DService");
            if (Convert.ToBoolean(configmanage.GetValue("IsAutoUpdate")))
            {
                ServiceLog.ToolLog.Log("服務啟動更新開始...");
                UpdateDll(configmanage.GetValue(Default.UpDateGradPath));
                ServiceLog.ToolLog.Log("服務啟動更新結束...");
                time.Elapsed += new System.Timers.ElapsedEventHandler(time_Elapsed);
                time.Interval = Default.Interval;
                time.Start();
            }

            server = GetTriggerServices();
            Thread t1 = new Thread(Lessner);
            t1.Start();
            System.Timers.Timer time2 = new System.Timers.Timer();
            time2.Elapsed += new System.Timers.ElapsedEventHandler(time2_Elapsed);
            time2.Interval = Default.Interval;
            time2.Start();
        }

        private TriggerService GetTriggerServices()
        {
            ServerImplement service = new ServerImplement();
            return service.GetAutoTriggerService();
        }

        private void time_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string time = DateTime.Now.ToString(configmanage.GetValue(Default.ShortTimeFormat));
            if (DateTime.Now.ToString(configmanage.GetValue(Default.ShortTimeFormat)) == configmanage.GetValue("UpDateTime"))
            {
                ServiceLog.ToolLog.Log("更新開始");
                UpdateDll(Settings1.Default.UpDateGradPath);
                ServiceLog.ToolLog.Log("更新結束");
            }
        }

        private void time2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string time = DateTime.Now.ToString(configmanage.GetValue(Default.ShortTimeFormat));
            server.Run(time);
        }

        protected override void OnStop()
        {
            ServiceLog.ToolLog.Log("服務停止");
        }
        private void Lessner()
        {
            DExecute.DExecute execute = new DExecute.DExecute(DicParameters);
            execute.Start();
        }

        /// <summary>20141219 add by Dick for 更新DLL</summary>
        /// 20170210 修改讓就算一個檔案無法搬移其他檔案還是可以正常運作 modified by Dick
        /// <param name="UpdatePath"></param>
        public virtual void UpdateDll(string UpdatePath)
        {
            DirectoryInfo LocalDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string configiPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DService.exe.config");
            configmanage = new ConfigManager(configiPath, "DService");
            List<string> LocalFileList = new List<string>();
            foreach (FileInfo FiInfo in LocalDir.GetFiles())
            {
                LocalFileList.Add(FiInfo.Name);
            }

            DirectoryInfo RemoveDir = new DirectoryInfo(UpdatePath);
            foreach (FileInfo FiInfo in RemoveDir.GetFiles())
            {
                try
                {
                    if (FiInfo.Name.Equals("FileTool.dll"))
                    {
                        continue;
                    }
                    if (!FiInfo.Extension.Equals(".dll"))
                    {
                        continue;
                    }
                    if (!LocalFileList.Contains(FiInfo.Name))
                    {
                        File.Copy(FiInfo.FullName, AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + FiInfo.Name, true);
                    }
                    else
                    {
                        if (FiInfo.Extension.Equals(Default.XML))
                        {
                            try
                            {
                                XmlDocument SourceDoc = XmlFile.LoadXml(FiInfo.FullName);
                                XmlNode root = SourceDoc.SelectSingleNode(Default.Root);
                                XmlDocument DeInfo = XmlFile.LoadXml(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + FiInfo.Name);
                                XmlNode deroot = DeInfo.SelectSingleNode(Default.Root);
                                if (root != null && deroot != null)
                                {
                                    if (!root.Attributes[Default.Version].Value.Equals(deroot.Attributes[Default.Version].Value))
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
                            string ServerBakPath = configmanage.GetValue(Default.ServerBakPath) + Path.DirectorySeparatorChar + FiInfo.Name;
                            if (SourceFile.FileVersion != null)
                            {
                                if (!SourceFile.FileVersion.Equals(Info.FileVersion))
                                {
                                    File.Copy(Info.FileName, ServerBakPath, true);//先備份
                                    File.Copy(SourceFile.FileName, Info.FileName, true);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ServiceLog.ToolLog.Log(ex);
                }
            }
        }
    }
}