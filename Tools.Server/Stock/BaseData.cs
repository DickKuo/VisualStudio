using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock {
    public class BaseData {
       protected  SQLHelper.UseStoreProcedure USP = new SQLHelper.UseStoreProcedure();

       protected class BaseSParameter {
           public const string BeginDate = "BeginDate";
           public const string EndDate = "EndDate";
           public const string SN = "SN";
           public const string DueMonth = "DueMonth";
           public const string DataTimeFormat = "yyyy-MM-dd";
       }
    }
}
