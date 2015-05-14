using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Collections;

namespace CommTool
{
   public  class ToolLog 
    {
       private static string _path = @"C:\Log";
      
       public ToolLog(string path)
       {
           ToolPath = path;           
       }

       /// <summary>
       /// 如果資料夾不存在的話，建立資料夾
       /// </summary>
       private static void CheckDirectoryIsExist()
       {           
           if (!Directory.Exists(_path))
           {
               Directory.CreateDirectory(_path);
               Log("資料夾不存在，建立資料夾");
           }
       }


       public ToolLog() { }

       public static string ToolPath
       {
           set { _path =value;}
           get { return _path; }
       }

       public static  void Log(Exception ex)
       {
           CheckDirectoryIsExist();
           StringBuilder message =new StringBuilder();
           message.AppendFormat("【{0}】 【{1}】 ",ex.Message,ex.StackTrace);
           Log(LogType.Error, message.ToString());
           Console.WriteLine(message.ToString());
       }

       public static  void Log(System.Net.Mail.SmtpException ex)
       {
           CheckDirectoryIsExist();
           StringBuilder message = new StringBuilder();
           message.AppendFormat("【{0}】 【{1}】 ", ex.Message, ex.StackTrace);
           Log(LogType.Error, message.ToString());
           Console.WriteLine(message.ToString());
       }


       public static  void Log(string str)
       {
           CheckDirectoryIsExist();
           DateTime dt = DateTime.Now;
           string TempPath = ToolPath + Path.DirectorySeparatorChar + dt.ToString("yyyy-MM-dd") + ".txt";
           StringBuilder message = new StringBuilder();
           using (StreamWriter sw2 = new StreamWriter(TempPath,true))
           {              
               message.AppendFormat("【{0}】 【Normal】 {1}\r\n", dt.ToString("HH:mm:ss"),str );
               //sw2.Write("【" +  + " " + "" + + "\r\n");
               sw2.Write(message.ToString());
               sw2.Close();
               sw2.Dispose();
           }
           Console.WriteLine(message.ToString());
       }

       public static  void Log(LogType type, string str)
       {
           CheckDirectoryIsExist();
           DateTime dt = DateTime.Now;           
           string TempPath = ToolPath + Path.DirectorySeparatorChar + dt.ToString("yyyy-MM-dd") + ".txt";
           StringBuilder message = new StringBuilder();
           using (StreamWriter sw2 = new StreamWriter(TempPath, true))
           {
               message.AppendFormat("【{0}】 {1}{2}\r\n", dt.ToString("HH:mm:ss"), typeconvert(type), str);
               //sw2.Write("【" + dt.ToString("HH:mm:ss") + "】 "+ typeconvert(type) + str + "\r\n");
               sw2.Write(message.ToString());
               sw2.Close();
               sw2.Dispose();
           }
           Console.WriteLine(message.ToString());
           if (type ==LogType.Error)
           {
               StringBuilder ErrorMessage = new StringBuilder();
               TempPath = ToolPath + Path.DirectorySeparatorChar + dt.ToString("yyyy-MM-dd") + "-Error.txt";
               using (StreamWriter sw2 = new StreamWriter(TempPath, true))
               {
                   //sw2.Write("【" + dt.ToString("HH:mm:ss") + "】 " + typeconvert(type) + str + "\r\n");
                   ErrorMessage.AppendFormat("【{0}】 {1}{2}\r\n", dt.ToString("HH:mm:ss"), typeconvert(type), str);
                   sw2.Close();
                   sw2.Dispose();
               }
           }
       }

       private static string typeconvert(LogType type)
       { 
           string temp ="";
           switch(type)
           {
             case LogType.Error :
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
           }
          return temp;
       }

       ~ToolLog()
       {

       }
   }
    
   public enum LogType {
      Error=0,
      Mail =1,
      Normal = 2,
      Delete =3
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

       /// <summary>
       /// 20150302 加入例外訊息
       /// </summary>
       /// <param name="ex"></param>
       public void Add(Exception ex)
       {
           if (!ExcetionCollectionList.Contains(ex))
           {
               ExcetionCollectionList.Add(ex);
           }
       }

       /// <summary>
       /// 20150302 列出所有例外訊息
       /// </summary>
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

           // Iterate the inner collection to 
           // find the box to be removed.
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
