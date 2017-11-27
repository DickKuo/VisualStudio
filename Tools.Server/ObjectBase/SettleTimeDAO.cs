using System;
using System.Collections.Generic;
using System.Data;

namespace ObjectBase {
    public class SettleTimeDAO : CommBase {

        private class SP {
            public const string AddSettleTime = "AddSettleTime";
            public const string UpdateSettleTime = "UpdateSettleTime";
            public const string GetListSettleTime = "GetListSettleTime";
            public const string GetSettleTimeBySN = "GetSettleTimeBySN";
            public const string GetNearlySettleTime = "GetNearlySettleTime";
        }

        private class SParamter {
            public const string BeginTime = "BeginTime";
            public const string EndTime = "EndTime";
            public const string IsSettle = "IsSettle";
        }

        /// <summary></summary>
        /// <param name="_SettleTime"></param>
        /// <returns></returns>
        public int AddSettleTime(SettleTime _SettleTime) {
            USP.AddParameter(CommBase.SN, 0,SqlDbType.Int,20,ParameterDirection.Output);
            USP.AddParameter(SParamter.BeginTime,_SettleTime.BeginTime);
            USP.AddParameter(SParamter.EndTime, _SettleTime.EndTime);          
            int Resut = USP.ExeProcedureHasResultReturnCode(SP.AddSettleTime);             
            return Resut;
        }
        
        /// <summary></summary>
        /// <param name="_SettleTime"></param>
        /// <returns></returns>
        public int UpdateSettleTime(SettleTime _SettleTime) {
            USP.AddParameter(CommBase.SN, _SettleTime.SN);
            USP.AddParameter(SParamter.BeginTime, _SettleTime.BeginTime);
            USP.AddParameter(SParamter.EndTime, _SettleTime.EndTime);
            USP.AddParameter(SParamter.IsSettle, _SettleTime.IsSettle);
            int Resut = USP.ExeProcedureHasResultReturnCode(SP.UpdateSettleTime);
            return Resut;
        }

        /// <summary>取得結清時間</summary>
        /// <returns></returns>
        public List<SettleTime> GetListSettleTime() {
            return USP.ExeProcedureGetObjectList(SP.GetListSettleTime, new SettleTime()) as List<SettleTime>;
        }

        /// <summary></summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public SettleTime GetSettleTimeBySN(int SN) {
            USP.AddParameter(CommBase.SN, SN);
            SettleTime Resut = USP.ExeProcedureGetObject(SP.GetSettleTimeBySN, new SettleTime());
            return Resut;
        }
        
        /// <summary>取得目前的結算時間區間</summary>
        /// <returns></returns>
        public SettleTime GetNearlySettleTime() {
            SettleTime Resut = USP.ExeProcedureGetObject(SP.GetNearlySettleTime, new SettleTime());
            return Resut;
        }

        /// <summary>計算結清</summary>
        public void CalculateSettle() {
            SettleTime Currcy = GetNearlySettleTime();


        }

    }
}
