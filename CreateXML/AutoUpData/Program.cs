using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Threading;
using CommTool;

namespace AutoUpData {
    class Program {
        static void Main(string[] args) {
            string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CreateXML.exe.config");
            ConfigManager configmanger = new ConfigManager(ConfigPath, "CreateXML");
            if (Convert.ToBoolean(configmanger.GetValue("IsUpdated")))
            {
                configmanger.SetValue("IsUpdated", "true");
                return;
            }
            try
            {
                Console.WriteLine("更新開始....");
                Thread.Sleep(2000);
                string DocPath = AppDomain.CurrentDomain.BaseDirectory + "VersionConfig.xml";
                ToolLog.ToolPath = @"C:\Log";
                if (File.Exists(DocPath))
                {
                    XmlDocument doc = LoadXml(DocPath);
                    XmlNode ServerPath = doc.SelectSingleNode("root/ServerPath");
                    string SourcePath = ServerPath.Attributes["Path"].Value;
                    //Console.WriteLine(SourcePath);
                    //CommTool.ToolLog.Log(DocPath);
                    if (Directory.Exists(SourcePath))
                    {
                        XmlNode DownLoadList = doc.SelectSingleNode("root/DownLoadList");
                        ///20141210 取得更新清單
                        List<string> FileList = new List<string>();
                        List<string> NotExistFile = new List<string>();
                        foreach (XmlNode node in DownLoadList.ChildNodes)
                        {
                            FileList.Add(node.Attributes["Name"].Value);
                            NotExistFile.Add(node.Attributes["Name"].Value);
                        }
                        DirectoryInfo DirInfo = new DirectoryInfo(SourcePath);
                        DirectoryInfo DesInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                        #region 20141210 已存在檔案 判斷版號更新
                        foreach (FileInfo fi in DirInfo.GetFiles())
                        {
                            if (FileList.Contains(fi.Name))
                            {
                                foreach (FileInfo desfi in DesInfo.GetFiles())
                                {
                                    if (FileList.Contains(desfi.Name))
                                    {
                                        if (fi.Name.Equals(desfi.Name))
                                        {
                                            if (fi.Extension.Equals(".xml"))
                                            {
                                                try
                                                {
                                                    XmlDocument SourceDoc = LoadXml(fi.FullName);
                                                    XmlNode root = SourceDoc.SelectSingleNode("root");
                                                    XmlDocument DeInfo = LoadXml(desfi.FullName);
                                                    XmlNode deroot = DeInfo.SelectSingleNode("root");
                                                    if (root != null && deroot != null)
                                                    {
                                                        if (!root.Attributes["Version"].Value.Equals(deroot.Attributes["Version"].Value))
                                                        {
                                                            try
                                                            {
                                                                File.Copy(fi.FullName, desfi.FullName, true);
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                CommTool.ToolLog.Log(ex);
                                                            }
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    //Console.WriteLine(ex.Message);
                                                    CommTool.ToolLog.Log(ex);
                                                }
                                            }
                                            else
                                            {
                                                FileVersionInfo SourceFile = FileVersionInfo.GetVersionInfo(fi.FullName);
                                                FileVersionInfo Info = FileVersionInfo.GetVersionInfo(desfi.FullName);
                                                if (SourceFile.FileVersion != null)
                                                {
                                                    if (!SourceFile.FileVersion.Equals(Info.FileVersion))
                                                    {
                                                        File.Copy(fi.FullName, desfi.FullName, true);
                                                    }
                                                }
                                            }
                                            NotExistFile.Remove(desfi.Name);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 20141210 新檔案處理
                        if (NotExistFile.Count > 0)
                        {
                            foreach (FileInfo fi in DirInfo.GetFiles())
                            {
                                if (NotExistFile.Contains(fi.Name))
                                {
                                    File.Copy(fi.FullName, DesInfo.FullName + Path.DirectorySeparatorChar + fi.Name, true);
                                }
                            }
                        }
                        #endregion
                        #region 20141210 舊的寫法 註解掉
                        //foreach (XmlNode node in DownLoadList.ChildNodes)
                        ////{
                        //    string FilePath = AppDomain.CurrentDomain.BaseDirectory + node.Attributes["Name"].Value;
                        //       string Sourcefile =SourcePath + node.Attributes["Name"].Value;                     
                        //if (File.Exists(Sourcefile))
                        //{                       
                        //    if (File.Exists(FilePath))
                        //    {
                        //        FileVersionInfo SourceFile = FileVersionInfo.GetVersionInfo(Sourcefile);
                        //        FileVersionInfo Info = FileVersionInfo.GetVersionInfo(FilePath);
                        //        if (SourceFile.FileVersion != null)
                        //        {
                        //            if (!SourceFile.FileVersion.Equals(Info.FileVersion))
                        //            {
                        //                try
                        //                {
                        //                    File.Copy(Sourcefile, FilePath, true);
                        //                }
                        //                catch (Exception ex)
                        //                {
                        //                    Console.WriteLine(ex.Message);
                        //                }
                        //            }
                        //        }
                        //        else
                        //        {
                        //            FileInfo info = new FileInfo(Sourcefile);
                        //            if (info.Extension.Equals(".xml"))
                        //            {
                        //                XmlDocument SourceDoc = LoadXml(info.FullName);
                        //                XmlNode root = SourceDoc.SelectSingleNode("root");
                        //                XmlDocument DeInfo = LoadXml(FilePath);
                        //                XmlNode deroot = DeInfo.SelectSingleNode("root");
                        //                if (root != null && deroot != null)
                        //                {
                        //                    if (!root.Attributes["Version"].Value.Equals(deroot.Attributes["Version"].Value))
                        //                    {
                        //                        try
                        //                        {
                        //                            File.Copy(Sourcefile, FilePath, true);
                        //                        }
                        //                        catch (Exception ex)
                        //                        {
                        //                            Console.WriteLine(ex.Message);
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        try
                        //        {
                        //            File.Copy(Sourcefile, FilePath, true);
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            Console.WriteLine(ex.Message);
                        //        }
                        //    }
                        //}                           
                        //}

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {               
                configmanger.SetValue("IsUpdated", "true");
                Thread.Sleep(3000);
                string UpdateFilePath = AppDomain.CurrentDomain.BaseDirectory + "CreateXML.exe";
                Process.Start(UpdateFilePath);
            }
            //Console.WriteLine("請按任意建繼續....");
            //Console.Read();
            //Process.Start(AppDomain.CurrentDomain.BaseDirectory + "CreateXML.exe");
        }

        public static XmlDocument LoadXml(string Path)
        {
            XmlDocument doc = new XmlDocument();
            System.IO.StreamReader sr = new System.IO.StreamReader(Path);
            doc.Load(sr);
            sr.Close();
            sr.Dispose();
            return doc;
        }
    }
}
