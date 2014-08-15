using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MoveFile {
    public class Move {

        /// <summary>
        /// List 移動檔案
        /// </summary>
        /// <param name="pStart"></param>
        /// <param name="pDestination"></param>
        /// <param name="pli">檔案名稱列表</param>
        /// <param name="pName"></param>
        /// <returns></returns>
        public virtual string MoveEachFile(string pStart,string pDestination,List<string> pli) {
            try {
                if(!Directory.Exists(@"C:\Share\" + pDestination)) {
                    Directory.CreateDirectory(@"C:\Share\" + pDestination);
                }
                foreach(string str in pli) {
                    if(File.Exists(pStart + "\\" + str)) {
                        File.Copy(pStart + "\\" + str, @"C:\Share\" + pDestination + "\\" + str);
                    }
                }
                if(!Directory.Exists(@"C:\Share\" + pDestination+"\\zh-CHS"))
                {
                    Directory.CreateDirectory(@"C:\Share\" + pDestination + "\\zh-CHS");
                }
                File.Copy(pStart + "\\zh-CHS" + "\\" + "Dcms.HR.UI.CaseUI.resources.dll", @"C:\Share\" + pDestination + "\\zh-CHS" + "\\" + "Dcms.HR.UI.CaseUI.resources.dll");

                if(!Directory.Exists(@"C:\Share\" + pDestination + "\\zh-CHT")) {
                    Directory.CreateDirectory(@"C:\Share\" + pDestination + "\\zh-CHT");
                }
                File.Copy(pStart + "\\zh-CHT" + "\\" + "Dcms.HR.UI.CaseUI.resources.dll", @"C:\Share\" + pDestination + "\\zh-CHT" + "\\" + "Dcms.HR.UI.CaseUI.resources.dll");


                return "完成";
            }
            catch(Exception ex) {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 單支檔案移動
        /// </summary>
        /// <param name="pStart"></param>
        /// <param name="pDestination"></param>
        /// <param name="pName"></param>
        /// <returns></returns>
        public virtual string MoveEachFile(string pStart, string pDestination, string pName) {
            try {
                if(!Directory.Exists(@"C:\Share\" + pDestination)) {
                    Directory.CreateDirectory(@"C:\Share\" + pDestination);
                }
                File.Copy(pStart + "\\" + pName, @"C:\Share\" + pDestination + "\\" + pName);
                return "";
            }
            catch(Exception ex) {
                return ex.ToString();
            }
        }

        #region


        public virtual void RecurrceMove(string SourcePath, string DestinationPath,bool flag)
        {
            DirectoryInfo srcDirectory = new DirectoryInfo(SourcePath);

            DestinationPath = DestinationPath + Path.DirectorySeparatorChar + srcDirectory.Name;

            CopyDirectory(SourcePath, DestinationPath,flag);
        }
        private void CopyDirectory(string SourceFolder, string DestinationFolder,bool flag) {
            if(Directory.Exists(SourceFolder) == true) {
                if(Directory.Exists(DestinationFolder) == false) {
                    Directory.CreateDirectory(DestinationFolder);
                }


                DirectoryInfo srcDirectory = new DirectoryInfo(SourceFolder);


                foreach(FileInfo fi in srcDirectory.GetFiles()) {
                    try {
                        //FullName是檔案的完整路徑位址喔，Name才是檔案的名字
                        //Path.DirectorySeparatorChar是路徑裡的斜線啦
                        //最後面的true是是否覆蓋相同的檔案
                        if(flag == true) {
                            if(fi.Name.IndexOf("ForCase") != -1) {
                                File.Copy(fi.FullName, DestinationFolder + Path.DirectorySeparatorChar + fi.Name, true);
                            }
                        }
                        else {
                            File.Copy(fi.FullName, DestinationFolder + Path.DirectorySeparatorChar + fi.Name, true);
                        }
                    }
                    catch(Exception e) {

                    }
                }
                foreach(DirectoryInfo di in srcDirectory.GetDirectories()) {
                    try {
                        CopyDirectory(di.FullName, DestinationFolder + Path.DirectorySeparatorChar + di.Name,flag);
                    }
                    catch(Exception e) {

                    }
                }
            }
        }

      

        #endregion


    }
}
