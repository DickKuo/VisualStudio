using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Diagnostics;
using ToolsBusiness.Services;


namespace CommTool
{
    public class Files : IFileService
    {

        /// <summary>
        /// 20141226 直接寫入檔案方法
        /// </summary>
        /// <param name="sb">內容</param>
        /// <param name="SaveFile">存檔位置</param>
        public static void WritFile(StringBuilder sb, string SaveFile)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(SaveFile))
                {
                    sw.Write(sb.ToString());
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
            }
        }

        /// <summary>
        /// 20141226 直接寫入檔案方法
        /// </summary>
        /// <param name="sb">內容</param>
        /// <param name="SaveFile">存檔位置</param>
        public static void WritFile(StringBuilder sb, string SaveFile, bool pAppend)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(SaveFile, pAppend))
                {
                    sw.Write(sb.ToString());
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
            }
        }



        /// <summary>
        /// 20141226 直接寫入檔案方法
        /// </summary>
        /// <param name="pContent">內容</param>
        /// <param name="SaveFile">存檔位置</param>
        public static void WritFile(string pContent, string SaveFile)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(SaveFile))
                {
                    sw.Write(pContent);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
            }
        }



        /// <summary>
        /// 20141226 直接寫入檔案方法
        /// </summary>
        /// <param name="sb">內容</param>
        /// <param name="SaveFile">存檔位置</param>
        public static void WritFile(string pContent, string SaveFile, bool pAppend)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(SaveFile, pAppend))
                {
                    sw.Write(pContent);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
            }
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
                                XmlDocument SourceDoc = XmlFile.LoadXml(FiInfo.FullName);
                                XmlNode root = SourceDoc.SelectSingleNode("root");
                                XmlDocument DeInfo = XmlFile.LoadXml(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + FiInfo.Name);
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
               ToolLog.Log(ex.Message);
            }
        }


        /// <summary>
        ///  設定資料夾路徑底下檔案是否唯獨
        /// </summary>
        /// <param name="pFilePath">資料夾路徑</param>
        /// <param name="IsReadOnly">是否唯獨 true:唯獨 ; false :不唯獨</param>
        public void FileReadOnly(DirectoryInfo dirInfo, bool IsReadOnly)
        {
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                if (IsReadOnly)
                {
                    file.Attributes = FileAttributes.ReadOnly;
                }
                else
                {
                    file.Attributes = FileAttributes.Normal;
                }
            }
            foreach (DirectoryInfo subDir in dirInfo.GetDirectories())
            {
                FileReadOnly(subDir, IsReadOnly);
            }
        }

    }
}