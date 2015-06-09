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
            StreamReader sw = new StreamReader(@"C:\Users\Dick\Desktop\新增資料夾\111.txt",Encoding.Default);
            string  context = sw.ReadToEnd();
            MatchCollection matches = Regex.Matches(context, "^bbs-content[^\"]+<span class=\"f2\">", RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string resul = match.Value;
            }
            //ToolLog.Log("");
            //System.Attribute attribute = null;
            //Assembly assembly = Assembly.LoadFrom(@"C:\Users\Dick\Desktop\Black\CommTool.Service.dll");
            //object obj = assembly.CreateInstance("TestAddin");
            //ITestService Itest = CallServihce.GetService<ITestService>();
            //Itest.HelloWord();
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
