using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileTool
{
    public class Files
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
                FileTool.ToolLog.Log(ex);
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
                FileTool.ToolLog.Log(ex);
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
                FileTool.ToolLog.Log(ex);
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
                FileTool.ToolLog.Log(ex);
            }
        }
    }
}