using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace AutoUpData {
    class Program {
        static void Main(string[] args) {
            string DocPath = AppDomain.CurrentDomain.BaseDirectory + "VersionConfig.xml";
            if(File.Exists(DocPath)) {
                XmlDocument doc = Tools.XmlTool.LoadXml(DocPath);
                XmlNode ServerPath = doc.SelectSingleNode("root/ServerPath");
                string SourcePath = ServerPath.Attributes["Path"].Value;
                if(Directory.Exists(SourcePath)) {
                    XmlNode DownLoadList = doc.SelectSingleNode("root/DownLoadList");
                    foreach(XmlNode node in DownLoadList.ChildNodes)
                    {
                        string FilePath = AppDomain.CurrentDomain.BaseDirectory + node.Attributes["Name"].Value;
                           string Sourcefile =SourcePath + node.Attributes["Name"].Value;
                        if(File.Exists(Sourcefile)) {
                            if(File.Exists(FilePath)) {
                                FileVersionInfo SourceFile = FileVersionInfo.GetVersionInfo(Sourcefile);
                                FileVersionInfo Info = FileVersionInfo.GetVersionInfo(FilePath);
                                if(SourceFile.FileVersion != null) {
                                    if(!SourceFile.FileVersion.Equals(Info.FileVersion)) {
                                        File.Copy(Sourcefile, FilePath, true);
                                    }
                                }
                                else {
                                    FileInfo info = new FileInfo(Sourcefile);
                                    if(info.Extension.Equals(".xml")) {
                                        XmlDocument SourceDoc = Tools.XmlTool.LoadXml(info.FullName);
                                        XmlNode root = SourceDoc.SelectSingleNode("root");
                                        XmlDocument DeInfo = Tools.XmlTool.LoadXml(FilePath);
                                        XmlNode deroot = DeInfo.SelectSingleNode("root");
                                        if(root != null && deroot != null) {
                                            if(!root.Attributes["Version"].Value.Equals(deroot.Attributes["Version"].Value)) {
                                                File.Copy(Sourcefile, FilePath, true);
                                            }
                                        }
                                    }
                                }
                            }
                            else {
                                File.Copy(Sourcefile, FilePath, true);
                            }
                        }
                    }
                }
            }
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + "CreateXML.exe");
        }
    }
}
