using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Diagnostics;
using CommTool.Business;
using System.Runtime.InteropServices;
using System.Data;
using System.Net;


namespace CommTool
{
    public class Files : IFileService
    {
        private class Default { 
            public const string Version = "Version";
            public const string FileTool = "FileTool.dll";
            public const string kernel32 = "kernel32";
            public const char SplitChar = ',';
            public const string ReplaceString = ",\"";
            public const char SplitLine = '|';
            public const string ChangeChar = "|";
            public const string DateTimeFormat = "yyyy/MM/dd";
        }

        /// <summary>直接寫入檔案方法</summary>
        /// 20141226 add by Dick
        /// <param name="sb">內容</param>
        /// <param name="SaveFile">存檔位置</param>
        public static void WritFile(StringBuilder sb, string SaveFile) {
            try {
                using (StreamWriter sw = new StreamWriter(SaveFile)) {
                    sw.Write(sb.ToString());
                    sw.Close();
                }
            }
            catch (Exception ex) {
                ToolLog.Log(ex);
            }
        }

        /// <summary>直接寫入檔案方法 </summary>
        ///  20141226 add by Dick
        /// <param name="sb">內容</param>
        /// <param name="SaveFile">存檔位置</param>
        public static void WritFile(StringBuilder sb, string SaveFile, bool pAppend) {
            try {
                using (StreamWriter sw = new StreamWriter(SaveFile, pAppend)) {
                    sw.Write(sb.ToString());
                    sw.Close();
                }
            }
            catch (Exception ex) {
                ToolLog.Log(ex);
            }
        }

        /// <summary>直接寫入檔案方法</summary>
        ///  20141226 add by Dick
        /// <param name="pContent">內容</param>
        /// <param name="SaveFile">存檔位置</param>
        public static void WritFile(string pContent, string SaveFile) {
            try {
                using (StreamWriter sw = new StreamWriter(SaveFile)) {
                    sw.Write(pContent);
                    sw.Close();
                }
            }
            catch (Exception ex) {
                ToolLog.Log(ex);
            }
        }        

        /// <summary>直接寫入檔案方法 </summary>
        ///  20141226 add by Dick 
        /// <param name="sb">內容</param>
        /// <param name="SaveFile">存檔位置</param>
        public static void WritFile(string pContent, string SaveFile, bool pAppend) {
            try {
                using (StreamWriter sw = new StreamWriter(SaveFile, pAppend)) {
                    sw.Write(pContent);
                    sw.Close();
                }
            }
            catch (Exception ex) {
                ToolLog.Log(ex);
            }
        }

        /// <summary>更新檔案</summary>
        /// 20141219 add by Dick 
        /// <param name="UpdatePath"></param>
        public virtual void UpdateDll(string UpdatePath) {
            try {
                DirectoryInfo LocalDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                List<string> LocalFileList = new List<string>();
                foreach (FileInfo FiInfo in LocalDir.GetFiles()) {
                    LocalFileList.Add(FiInfo.Name);
                }
                DirectoryInfo RemoveDir = new DirectoryInfo(UpdatePath);
                foreach (FileInfo FiInfo in RemoveDir.GetFiles()) {
                    if (FiInfo.Name.Equals(Default.FileTool)) {
                        continue;
                    }
                    if (!LocalFileList.Contains(FiInfo.Name)) {
                        File.Copy(FiInfo.FullName, AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + FiInfo.Name, true);
                    }
                    else {
                        if (FiInfo.Extension.Equals(BaseConst.XmlFile)) {
                            try {
                                XmlDocument SourceDoc = XmlFile.LoadXml(FiInfo.FullName);
                                XmlNode root = SourceDoc.SelectSingleNode(BaseConst.Root);
                                XmlDocument DeInfo = XmlFile.LoadXml(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + FiInfo.Name);
                                XmlNode deroot = DeInfo.SelectSingleNode(BaseConst.Root);
                                if (root != null && deroot != null) {
                                    if (!root.Attributes[Default.Version].Value.Equals(deroot.Attributes[Default.Version].Value)) {
                                        try {
                                            File.Copy(FiInfo.FullName, AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + FiInfo.Name, true);
                                        }
                                        catch (Exception ex) {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex) {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else {
                            FileVersionInfo SourceFile = FileVersionInfo.GetVersionInfo(FiInfo.FullName); //遠端檔案
                            FileVersionInfo Info = FileVersionInfo.GetVersionInfo(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + FiInfo.Name); //本地檔案
                            if (SourceFile.FileVersion != null) {
                                if (!SourceFile.FileVersion.Equals(Info.FileVersion)) {
                                    File.Copy(SourceFile.FileName, Info.FileName, true);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                ToolLog.Log(ex.Message);
            }
        }

        /// <summary>設定資料夾路徑底下檔案是否唯獨 </summary>
        /// <param name="pFilePath">資料夾路徑</param>
        /// <param name="IsReadOnly">是否唯獨 true:唯獨 ; false :不唯獨</param>     
        public void FileReadOnly(DirectoryInfo dirInfo, bool IsReadOnly) {
            foreach (FileInfo file in dirInfo.GetFiles()) {
                if (IsReadOnly) {
                    file.Attributes = FileAttributes.ReadOnly;
                }
                else {
                    file.Attributes = FileAttributes.Normal;
                }
            }
            foreach (DirectoryInfo subDir in dirInfo.GetDirectories()) {
                FileReadOnly(subDir, IsReadOnly);
            }
        }
        
        /// <summary>寫入ini檔案</summary>
        /// 20160808 加入方法 by Dick
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="SaveFile"></param>
        public static void WriteIniFile(string Section, string Key, string Value, string SaveFile) {
            WritePrivateProfileString(Section, Key, Value, SaveFile);
        }

        /// <summary>讀取ini檔案</summary>
        /// 20160808 加入方法 by Dick
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="SaveFile"></param>
        /// <returns></returns>
        public static string ReadIniFile(string Section, string Key, string SaveFile) {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, string.Empty, temp, 255, SaveFile);
            return temp.ToString();
        }

        /// <summary>讀取CSV檔案，轉成DataTable資料流</summary>
        /// <param name="FilePath"></param>
        /// <param name="Table">Table的欄位順序要與CSV順序一致</param>
        /// <returns></returns>
        public static DataTable ReadCSV(string FilePath,DataTable Table) {
            StreamReader SR = new StreamReader(FilePath,Encoding.Default);
            string  line =string.Empty;
            line = SR.ReadLine();
            while((line=SR.ReadLine())!=null)
            {
                string[] sp = line.Replace(Default.ReplaceString,Default.ChangeChar).Replace("\"",string.Empty).Split(Default.SplitChar);
                DataRow dr = Table.NewRow();
                int i=0;
                foreach (string result in sp) {   
                    dr[i] = result;
                    i++;                    
                }
                Table.Rows.Add(dr);
            }
            return Table;
        }

        /// <summary>下載檔案到指定位置</summary>
        /// <param name="Url"></param>
        /// <param name="Destination"></param>
        public static void DownLoadFile(string Url,string Destination) {
            WebClient wc = new WebClient();
            wc.DownloadFile(Url, Destination);
        }

        [DllImport(Default.kernel32, CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport(Default.kernel32, CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,int size, string filePath);
    }
}