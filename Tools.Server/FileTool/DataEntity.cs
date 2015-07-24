using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommTool.Business.Services;
using System.Data;
using System.Reflection;

namespace CommTool
{
    public class DataEntity :IDataEntity
    {

        //public string TypeKey { get; }

        public  ExcetionCollection Excections {  get;  set; }

        public PropertyCollection ExtendedProperties { set; get; }

        public object GetPropertyValue(string pPropertyName)
        {
            PropertyInfo property = base.GetType().GetProperty(pPropertyName);
            if (property != null)
            {
                return property.GetValue(this, null);
            }
            return null;
        }

        public void SetPropertyValue(string pPropertyName, object pValue)
        {
            PropertyInfo property = base.GetType().GetProperty(pPropertyName);
            if (property != null)
            {
                base.GetType().GetProperty(pPropertyName).SetValue(this, pValue, null);
            }
        }


    }
}
