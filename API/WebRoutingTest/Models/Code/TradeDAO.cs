using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Code.DAO;

namespace WebRoutingTest.Models.Code
{
    public class TradeDAO : DAOBase
    {
       private  string LogPath= System.Web.Configuration.WebConfigurationManager.AppSettings["LogPath"];
        public TradeDAO()
        {
            base._DbIstance.ConnectiinString = System.Web.Configuration.WebConfigurationManager.AppSettings["ConnectionString"].ToString();
        }

        private class SpName
        {
            public const string SpSaveMoney = "SpSaveMoney";
        }

        private class ParameterName
        {
            public const string EmpNo    = "EmpNo";
            public const string Money    = "Money";
            public const string BankNo   = "BankNo";
            public const string IsSucces = "IsSucces";
        }

        /// <summary>金額回存DB</summary>
        /// <param name="Info"></param>
        /// <returns></returns>
        public TradeInfo CashOperation(TradeInfo Info)
        {
            Factory   FactoryBank = new Factory();
            BaseBank  Bank = FactoryBank.GetBank(Info);
            TradeInfo BankInfo = Bank.Opeartion(Info);         
            BankInfo = SpSaveMoney(BankInfo);
            return BankInfo;
        }//end SaveMoney


        /// <summary> 儲存金錢異動 </summary>
        /// <param name="Info"></param>
        /// <returns></returns>
        public TradeInfo SpSaveMoney(TradeInfo Info)
        {     
            base._DbIstance.AddParameter(ParameterName.EmpNo, Info.EmpNo);
            base._DbIstance.AddParameter(ParameterName.Money, Info.Money);
            base._DbIstance.AddParameter(ParameterName.BankNo, Info.BankNo);
            base._DbIstance.AddParameter(ParameterName.IsSucces, 1, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureNotQuery(SpName.SpSaveMoney);           
            Info.ApiTradeResult = base._DbIstance.Result;
            if (base._DbIstance.OutParameterValues[0] == GlobalParameter.StoreProcedureResult.Error)
            {
                Info.ApiResultCode = TradeInfo.ResultCode.Error;
            }
            else
            {
                Info.ApiResultCode = TradeInfo.ResultCode.Sucess;
                LogDAO Log = new LogDAO();
                Log.LogFilePath = LogPath;
                Log.LogMethod(Info.ApiTradeResult);
            }
            return Info;
        }//end SpSaveMoney

        public override object Add<T>(T Obj)
        {
            throw new NotImplementedException();
        }

        public override object Delete<T>(T Obj)
        {
            throw new NotImplementedException();
        }


        public override object Edit<T>(T Obj)
        {
            throw new NotImplementedException();
        }


        public override object Select<T>(T Obj)
        {
            throw new NotImplementedException();
        }
    }
}