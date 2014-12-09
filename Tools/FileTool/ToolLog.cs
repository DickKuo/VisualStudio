using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.ComponentModel;

namespace FileTool
{
   public  class ToolLog 
    {
       private static string _path = @"D:\Log";
      
       public ToolLog(string path)
       {
           ToolPath = path;
       }

       public ToolLog() { }

       public static string ToolPath
       {
           set { _path =value;}
           get { return _path; }
       }

       public static  void Log(Exception ex)
       {
           Log(LogType.Error,"【"+ex.Message+"】  " +"【"+ex.StackTrace+"】" );           
       }

       public static  void Log(System.Net.Mail.SmtpException ex)
       {
           Log(LogType.Error,"【" + ex.Message + "】  " + "【" + ex.StackTrace + "】");   
       }


       public static  void Log(string str)
       {
           DateTime dt = DateTime.Now;
           string TempPath = ToolPath + Path.DirectorySeparatorChar + dt.ToString("yyyy-MM-dd") + ".txt";        
           using (StreamWriter sw2 = new StreamWriter(TempPath,true))
           {
               sw2.Write("【" + dt.ToString("HH:mm:ss") + "】 " + " 【Normal】 " + str + "\r\n");
               sw2.Close();
               sw2.Dispose();
           }  
       }

       public static  void Log(LogType type, string str)
       {
           DateTime dt = DateTime.Now;           
           string TempPath = ToolPath + Path.DirectorySeparatorChar + dt.ToString("yyyy-MM-dd") + ".txt";
           using (StreamWriter sw2 = new StreamWriter(TempPath, true))
           {
               sw2.Write("【" + dt.ToString("HH:mm:ss") + "】 "+ typeconvert(type) + str + "\r\n");
               sw2.Close();
               sw2.Dispose();
           }

           if (type ==LogType.Error)
           {
               TempPath = ToolPath + Path.DirectorySeparatorChar + dt.ToString("yyyy-MM-dd") + "-Error.txt";
               using (StreamWriter sw2 = new StreamWriter(TempPath, true))
               {
                   sw2.Write("【" + dt.ToString("HH:mm:ss") + "】 " + typeconvert(type) + str + "\r\n");
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

}
