using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

namespace ServiceLog
{
    public class ToolLog
    {
        private class Default
        {
            public const string ErrorFile = "_Error.txt";
            public const string MonthAndDateStringFormat = "00";
            public const string MessageFormat = "【{0}】 {1}{2}\r\n";
            public const string Record = "_Record";
            public const string FileNameFormat = "{0}-{1}-{2}";
            public const string BasePath = @"C:\SLog";
            public const string TimeFormat = "yyyy-MM-dd";
            public const string Txt = ".txt";
            public const string TimeFormat_HHmmss = "HH:mm:ss";
        }

        private static string _path = Default.BasePath;

        /// <summary>建構子</summary>
        /// <param name="path"></param>
        public ToolLog(string path)
        {
            ToolPath = path;
        }

        /// <summary>如果資料夾不存在的話，建立資料夾 </summary>
        private static void CheckDirectoryIsExist()
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
                Log("資料夾不存在，建立資料夾");
            }
            ArramgeLog();
        }

        /// <summary>檔案每月一號將上個月檔案整理成一個資料夾 
        /// 20150604 整理Log檔案每月一號將上個月檔案整理成一個資料夾 #61 
        /// 20160909 整理資料夾改成共用Function  by Dick  #61 
        /// </summary>
        private static void ArramgeLog()
        {
            if (DateTime.Now.Day == 1)
            {
                DateTime LastMonth = DateTime.Now.AddMonths(-1);
                StringBuilder sb = new StringBuilder();
                sb.Append(LastMonth.Year.ToString());
                sb.Append(LastMonth.Month.ToString(Default.MonthAndDateStringFormat));
                string DirectotyPath = Path.Combine(ToolPath, sb.ToString());
                if (!Directory.Exists(DirectotyPath))
                {
                    Directory.CreateDirectory(DirectotyPath);
                }
                DateTime Begin = new DateTime(LastMonth.Year, LastMonth.Month, 1);
                DateTime End = Begin.AddMonths(1).AddDays(-1);
                DateTime Time = Begin;
                while (Time <= End)
                {
                    MoveFile(DirectotyPath, Time, Default.Txt);
                    MoveFile(DirectotyPath, Time, Default.ErrorFile);
                    MoveFile(DirectotyPath, Time, Default.Record + Default.Txt);
                    Time = Time.AddDays(1);
                }
            }
        }

        /// <summary>搬移紀錄檔</summary>
        /// 20160909 紀錄檔案歸檔 by Dick #61 
        /// <param name="DirectotyPath"></param>
        /// <param name="Time"></param>
        /// <param name="MoveFileName"></param>
        private static void MoveFile(string DirectotyPath, DateTime Time, string MoveFileName)
        {
            string FileName = string.Format(Default.FileNameFormat + MoveFileName, Time.Year.ToString(), Time.Month.ToString(Default.MonthAndDateStringFormat), Time.Day.ToString(Default.MonthAndDateStringFormat));
            string ResourceFilePath = Path.Combine(ToolPath, FileName);
            string DestFilePath = Path.Combine(DirectotyPath, FileName);
            if (File.Exists(ResourceFilePath))
            {
                File.Move(ResourceFilePath, DestFilePath);
            }
        }

        public ToolLog()
        {
        }

        public static string ToolPath
        {
            set { _path = value; }
            get { return _path; }
        }

        public static void Log(Exception ex)
        {
            CheckDirectoryIsExist();
            StringBuilder message = new StringBuilder();
            message.AppendFormat("【{0}】 【{1}】 ", ex.Message, ex.StackTrace);
            Log(LogType.Error, message.ToString());
            Console.WriteLine(message.ToString());
        }

        public static void Log(System.Net.Mail.SmtpException ex)
        {
            CheckDirectoryIsExist();
            StringBuilder message = new StringBuilder();
            message.AppendFormat("【{0}】 【{1}】 ", ex.Message, ex.StackTrace);
            Log(LogType.Error, message.ToString());
            Console.WriteLine(message.ToString());
        }

        public static void Log(string str)
        {
            CheckDirectoryIsExist();
            DateTime dt = DateTime.Now;
            string TempPath = ToolPath + Path.DirectorySeparatorChar + dt.ToString(Default.TimeFormat) + Default.Txt;
            StringBuilder message = new StringBuilder();
            using (StreamWriter sw2 = new StreamWriter(TempPath, true))
            {
                message.AppendFormat("【{0}】 【Normal】 {1}\r\n", dt.ToString(Default.TimeFormat_HHmmss), str);
                sw2.Write(message.ToString());
                sw2.Close();
                sw2.Dispose();
            }
            Console.WriteLine(message.ToString());
        }

        /// <summary>紀錄抓取的JSON格式資料</summary>
        /// 20160909 加入新功能  By Dick 
        /// <param name="RecordString"></param>
        public static void Record(string RecordString)
        {
            DateTime dt = DateTime.Now;
            string TempPath = ToolPath + Path.DirectorySeparatorChar + dt.ToString(Default.TimeFormat) + Default.Record + Default.Txt;
            using (StreamWriter SW = new StreamWriter(TempPath, true))
            {
                SW.WriteLine(RecordString);
                SW.Close();
                SW.Dispose();
            }
            Console.WriteLine(RecordString);
        }

        public static void Log(LogType type, string str)
        {
            CheckDirectoryIsExist();
            DateTime dt = DateTime.Now;
            string TempPath = ToolPath + Path.DirectorySeparatorChar + dt.ToString(Default.TimeFormat) + Default.Txt;
            StringBuilder message = new StringBuilder();
            using (StreamWriter sw2 = new StreamWriter(TempPath, true))
            {
                message.AppendFormat(Default.MessageFormat, dt.ToString(Default.TimeFormat_HHmmss), typeconvert(type), str);
                sw2.Write(message.ToString());
                sw2.Close();
                sw2.Dispose();
            }
            Console.WriteLine(message.ToString());
            if (type == LogType.Error)
            {
                StringBuilder ErrorMessage = new StringBuilder();
                TempPath = ToolPath + Path.DirectorySeparatorChar + dt.ToString(Default.TimeFormat) + Default.ErrorFile;
                using (StreamWriter sw2 = new StreamWriter(TempPath, true))
                {
                    ErrorMessage.AppendFormat(Default.MessageFormat, dt.ToString(Default.TimeFormat_HHmmss), typeconvert(type), str);
                    sw2.Write(ErrorMessage.ToString());
                    sw2.Close();
                    sw2.Dispose();
                }
            }
        }

        private static string typeconvert(LogType type)
        {
            string temp = string.Empty;
            switch (type)
            {
                case LogType.Error:
                    temp = " 【Error】 ";
                    break;
                case LogType.Mail:
                    temp = " 【Mail】 ";
                    break;
                case LogType.Normal:
                    temp = " 【Normal】 ";
                    break;
                case LogType.Delete:
                    temp = " 【Delete】 ";
                    break;
                case LogType.Exclude:   //工具集 #67   
                    temp = " 【Exclude】 ";
                    break;
            }
            return temp;
        }
        ~ToolLog()
        {
        }
    }

    public enum LogType
    {
        Error = 0,
        Mail = 1,
        Normal = 2,
        Delete = 3,
        Exclude = 4
    }

    public class ExcetionCollection
    {
        // For IsReadOnly
        private bool isRO = false;
        List<Exception> ExcetionCollectionList = null;

        public ExcetionCollection()
        {
            ExcetionCollectionList = new List<Exception>();
        }

        /// <summary>20150302 加入例外訊息</summary>
        /// <param name="ex"></param>
        public void Add(Exception ex)
        {
            if (!ExcetionCollectionList.Contains(ex))
            {
                ExcetionCollectionList.Add(ex);
            }
        }

        /// <summary> 20150302 列出所有例外訊息</summary>
        /// <returns></returns>
        public string ShowException()
        {
            StringBuilder Error = new StringBuilder();
            foreach (Exception ex in ExcetionCollectionList)
            {
                Error.AppendLine(ex.Message);
            }
            return Error.ToString();
        }

        public int Count
        {
            get
            {
                return ExcetionCollectionList.Count;
            }
        }

        public void Clear()
        {
            ExcetionCollectionList.Clear();
        }

        public bool IsReadOnly
        {
            get { return isRO; }
        }

        public bool Remove(Exception item)
        {
            bool result = false;
            for (int i = 0; i < ExcetionCollectionList.Count; i++)
            {
                Exception Ex = (Exception)ExcetionCollectionList[i];
                if (Ex.Message.Equals(item.Message))
                {
                    ExcetionCollectionList.RemoveAt(i);
                    result = true;
                    break;
                }
            }
            return result;
        }

        public bool Contains(Exception item, EqualityComparer<Exception> comp)
        {
            bool found = false;
            foreach (Exception bx in ExcetionCollectionList)
            {
                if (comp.Equals(bx, item))
                {
                    found = true;
                }
            }
            return found;
        }
    }
}