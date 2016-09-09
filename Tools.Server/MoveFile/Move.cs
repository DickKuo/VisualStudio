using System;
using System.Collections.Generic;
using System.IO;

namespace MoveFile
{
    public class Move
    {
        private class Default {
            public const string CPath = @"C:\Share\";
            public const string CHS = "zh-CHS";
            public const string CHT = "zh-CHT";
        }

        /// <summary>List 移動檔案</summary>
        /// <param name="pStart"></param>
        /// <param name="pDestination"></param>
        /// <param name="pli">檔案名稱列表</param>
        /// <param name="pName"></param>
        /// <returns></returns>
        public virtual string MoveEachFile(string pStart, string pDestination, List<string> pli, string pType) {
            try {
                if (!Directory.Exists(Default.CPath + pDestination)) {
                    Directory.CreateDirectory(Default.CPath + pDestination);
                }
                foreach (string str in pli) {
                    if (File.Exists(pStart + Path.DirectorySeparatorChar + str)) {
                        File.Copy(pStart + Path.DirectorySeparatorChar + str, Default.CPath + pDestination + Path.DirectorySeparatorChar + str);
                    }
                }
                switch (pType.ToLower()) {
                    case "client":
                        if (!Directory.Exists(Default.CPath + pDestination + Path.DirectorySeparatorChar + Default.CHS)) {
                            Directory.CreateDirectory(Default.CPath + pDestination + Path.DirectorySeparatorChar + Default.CHS);
                        }
                        File.Copy(pStart + Path.DirectorySeparatorChar + Default.CHS + Path.DirectorySeparatorChar + "Dcms.HR.UI.CaseUI.resources.dll", Default.CPath + pDestination + Path.DirectorySeparatorChar + Default.CHS + Path.DirectorySeparatorChar + "Dcms.HR.UI.CaseUI.resources.dll");

                        if (!Directory.Exists(Default.CPath + pDestination + Path.DirectorySeparatorChar + Default.CHT)) {
                            Directory.CreateDirectory(Default.CPath + pDestination + Path.DirectorySeparatorChar + Default.CHT);
                        }
                        File.Copy(pStart + Path.DirectorySeparatorChar + Default.CHT + Path.DirectorySeparatorChar + "Dcms.HR.UI.CaseUI.resources.dll", Default.CPath + pDestination + Path.DirectorySeparatorChar + Default.CHT + Path.DirectorySeparatorChar + "Dcms.HR.UI.CaseUI.resources.dll");
                        break;
                    case "server":
                        if (!Directory.Exists(Default.CPath + pDestination + Path.DirectorySeparatorChar + Default.CHS)) {
                            Directory.CreateDirectory(Default.CPath + pDestination + Path.DirectorySeparatorChar + Default.CHS);
                        }
                        File.Copy(pStart + Path.DirectorySeparatorChar + Default.CHS + Path.DirectorySeparatorChar + "DigiWin.HR.CaseBusinessImplement.resources.dll", Default.CPath + pDestination + Path.DirectorySeparatorChar + Default.CHS + Path.DirectorySeparatorChar + "DigiWin.HR.CaseBusinessImplement.resources.dll");

                        if (!Directory.Exists(Default.CPath + pDestination + Path.DirectorySeparatorChar + Default.CHT)) {
                            Directory.CreateDirectory(Default.CPath + pDestination + Path.DirectorySeparatorChar + Default.CHT);
                        }
                        File.Copy(pStart + Path.DirectorySeparatorChar + Default.CHT + Path.DirectorySeparatorChar + "DigiWin.HR.CaseBusinessImplement.resources.dll", Default.CPath + pDestination + Path.DirectorySeparatorChar + Default.CHT + Path.DirectorySeparatorChar + "DigiWin.HR.CaseBusinessImplement.resources.dll");
                        break;
                }
                return "完成";
            }
            catch (Exception ex) {
                return ex.ToString();
            }
        }

        /// <summary>單支檔案移動</summary>
        /// <param name="pStart"></param>
        /// <param name="pDestination"></param>
        /// <param name="pName"></param>
        /// <returns></returns>
        public virtual string MoveEachFile(string pStart, string pDestination, string pName) {
            try {
                if (!Directory.Exists(Default.CPath + pDestination)) {
                    Directory.CreateDirectory(Default.CPath + pDestination);
                }
                File.Copy(pStart + Path.DirectorySeparatorChar + pName, Default.CPath + pDestination + Path.DirectorySeparatorChar + pName);
                return string.Empty;
            }
            catch (Exception ex) {
                return ex.ToString();
            }
        }

        #region

        /// <summary>移動檔案</summary>
        /// <param name="SourcePath"></param>
        /// <param name="DestinationPath"></param>
        /// <param name="flag"></param>
        public virtual void RecurrceMove(string SourcePath, string DestinationPath, bool flag) {
            DirectoryInfo srcDirectory = new DirectoryInfo(SourcePath);
            DestinationPath = DestinationPath + Path.DirectorySeparatorChar + srcDirectory.Name;
            CopyDirectory(SourcePath, DestinationPath, flag);
        }

        private void CopyDirectory(string SourceFolder, string DestinationFolder, bool flag) {
            if (Directory.Exists(SourceFolder) == true) {
                if (Directory.Exists(DestinationFolder) == false) {
                    Directory.CreateDirectory(DestinationFolder);
                }
                DirectoryInfo srcDirectory = new DirectoryInfo(SourceFolder);
                foreach (FileInfo fi in srcDirectory.GetFiles()) {
                    try {
                        //FullName是檔案的完整路徑位址喔，Name才是檔案的名字
                        //Path.DirectorySeparatorChar是路徑裡的斜線啦
                        //最後面的true是是否覆蓋相同的檔案
                        if (flag == true) {
                            if (fi.Name.IndexOf("ForCase") != -1) {
                                File.Copy(fi.FullName, DestinationFolder + Path.DirectorySeparatorChar + fi.Name, true);
                            }
                        }
                        else {
                            File.Copy(fi.FullName, DestinationFolder + Path.DirectorySeparatorChar + fi.Name, true);
                        }
                    }
                    catch (Exception e) {
                    }
                }
                foreach (DirectoryInfo di in srcDirectory.GetDirectories()) {
                    try {
                        CopyDirectory(di.FullName, DestinationFolder + Path.DirectorySeparatorChar + di.Name, flag);
                    }
                    catch (Exception e) {
                    }
                }
            }
        }
        #endregion
    }
}
