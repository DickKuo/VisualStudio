using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ObjectBase {
    public class BounsDAO : CommBase {

        private class SP {
            public const string GetListBouns = "GetListBouns";
            public const string AddBouns = "AddBouns";
        }

        private class SParamter {
            public const string Draw = "Draw";
            public const string TradeDate = "TradeDate";
            public const string BounsType = "BounsType";
            public const string AdviserSN = "AdviserSN";
            public const string CustomerSN = "CustomerSN";
        }

        /// <summary>加入獎金</summary>
        /// <param name="_Bouns"></param>
        /// <returns></returns>
        public int AddBouns(Bouns _Bouns) {
            USP.AddParameter(CommBase.SN, 0);
            USP.AddParameter(SParamter.Draw,_Bouns.Draw);
            USP.AddParameter(SParamter.BounsType, _Bouns.BounsType);
            USP.AddParameter(SParamter.AdviserSN, _Bouns.AdviserSN);
            USP.AddParameter(SParamter.CustomerSN, _Bouns.CustomerSN);
            int ResCode = 0;
            int SN = 0;
            ResCode = USP.ExeProcedureHasResultReturnCode(SP.AddBouns);
            if (ResCode == 99) {
                SN =Convert.ToInt32 (USP.OutParameterValues[0]);
            }
            return SN;
        }

        /// <summary>取得時間區間的獎金狀況</summary>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="AdviserSN"></param>
        /// <returns></returns>
        public List<Bouns> GetListBouns(string BeginDate, string EndDate, int AdviserSN, int Page, out int PageCount) {
            PageCount = 0;
            USP.AddParameter(CommBase.SParamter_PageCount, 0,SqlDbType.Int,20,ParameterDirection.Output);
            USP.AddParameter(CommBase.SParamter_BeginDate, BeginDate);
            USP.AddParameter(CommBase.SParamter_EndDate, EndDate);
            USP.AddParameter(SParamter.AdviserSN ,AdviserSN);
            USP.AddParameter(CommBase.SParamter_Page, Page);
            USP.AddParameter(CommBase.SParamter_PageSize, 20);      
            List<Bouns> ListBouns = USP.ExeProcedureGetObjectList(SP.GetListBouns,new Bouns());
            PageCount = Convert.ToInt32(USP.OutParameterValues[0]);
            return ListBouns;
        }


    }
}
