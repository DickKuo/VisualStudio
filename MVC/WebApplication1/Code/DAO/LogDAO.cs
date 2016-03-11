using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication1.Code.DAO
{
    public class LogDAO
    {

        private class TimeFormate {
            public const string Format = "yyyy/MM/dd HH:mm:ss";
        }

        public string LogFilePath { set; get; }


        /// <summary>寫入Log檔案</summary>
        /// <param name="Message"></param>
        public void LogMethod(string Message)
        {
            if (!File.Exists(LogFilePath))
            {
                using (StreamWriter sw = new StreamWriter(LogFilePath))
                {
                    sw.WriteLine(string.Format("{0} ,{1}", DateTime.Now.ToString(TimeFormate.Format), Message));
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(LogFilePath))
                {
                    sw.WriteLine(string.Format("{0} ,{1}", DateTime.Now.ToString(TimeFormate.Format), Message));
                }
            }
        }




    }
}