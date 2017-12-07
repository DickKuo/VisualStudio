using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock {
    public class WeekPointDAO : BaseData {

        private class SP {
            public const string GetListWeekPointByYear = "GetListWeekPointByYear";
        }

        private class SParamter {
            public const string BeginDate = "BeginDate";
            public const string EndDate = "EndDate";
        }
        
        /// <summary>取得每周的結算結果</summary>
        /// <param name="Year"></param>
        /// <returns></returns>
        public List<WeekPoint> GetListWeekPointByYear(int Year) {
            DateTime BeginDate = new DateTime(Year, 1, 1);
            DateTime EndDate = new DateTime(Year, 12, 31);
            USP.AddParameter(SParamter.BeginDate, BeginDate);
            USP.AddParameter(SParamter.EndDate, EndDate);
            List<WeekPoint> Result = USP.ExeProcedureGetObjectList(SP.GetListWeekPointByYear, new WeekPoint());
            return Result;
        }

    }
}
