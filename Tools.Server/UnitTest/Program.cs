using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebInfo;
using System.IO;
using WebInfo.Business.DataEntities;
using System.Reflection;
using CommTool;
using CommTool.Business.Services;
using System.Text.RegularExpressions;

namespace UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //StreamReader sw = new StreamReader(@"C:\Users\Dick\Desktop\新增資料夾\111.txt",Encoding.Default);
            //StringBuilder sb = new StringBuilder();
            //string  context = sw.ReadToEnd();
            //MatchCollection matches = Regex.Matches(context, "</span></div>[^\"]+<span class=\"f2\">", RegexOptions.Multiline);
            //foreach (Match match in matches)
            //{
            //    string resul = match.Value;
            //    GetAnalysis(sb, context, resul);
            //}
            //ToolLog.Log("");
            //System.Attribute attribute = null;
            //Assembly assembly = Assembly.LoadFrom(@"C:\Users\Dick\Desktop\Black\CommTool.Service.dll");
            //object obj = assembly.CreateInstance("TestAddin");
            //ITestService Itest = CallServihce.GetService<ITestService>(); 
            //Itest.HelloWord();              <div id="main-container">   <span class="f2">
          //  System.Type[] types = ii.GetTypes();
          //object[] customAttributes=  types[6].GetCustomAttributes(true);
          //if (customAttributes != null)
          //{
          //    object[] array = customAttributes;
          //    for (int i = 0; i < array.Length; i++)
          //    {
          //        System.Attribute attribute = (System.Attribute)array[i];
          //        if (attribute.GetType() == this._searchAttribute)
          //        {
          //            pFindAttribute = attribute;
          //            //return true;
          //        }
          //    }
          //}
            //string ttt = sb.ToString();
        }

        private static void GetAnalysis(StringBuilder sb, string context, string resul)
        {
            if (resul.IndexOf("http") == -1)
            {
                if (resul.IndexOf("<a href") == -1)
                {
                    string[] array = context.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    bool Into = false;

                    foreach (string str in array)
                    {
                        if (str.IndexOf("class=\"article-meta-tag\">時間</span><span") != -1)
                        {
                            Into = true;
                            continue;
                        }
                        if (str.IndexOf("class=\"f2\">※") != -1)
                        {
                            Into = false;
                        }

                        if (Into)
                        {
                            sb.AppendFormat("{0}\r\n", str);
                        }
                    }
                }
            }
        }
    }

    public class TestAddin : IServiceProviderAddin
    {
        IServiceEntry[] _serviceEntries;
         public IServiceEntry[] ServiceEntries
        {
            get {
                return _serviceEntries;
            }
        }
    }
}
