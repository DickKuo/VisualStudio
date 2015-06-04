using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebInfo;
using System.IO;
using WebInfo.Business.DataEntities;
using System.Reflection;

namespace UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Attribute attribute = null;
            Assembly ii = Assembly.LoadFile(@"C:\Users\Dick\Desktop\Black\CommTool.Service.dll");
            //ITestService Itest = CallService.GetService<ITestService>();
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
}
