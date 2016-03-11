using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApplication1.Code.DAO;
using WebApplication1.Models.Code;

namespace WebRoutingTest.Models.Code.DAO
{
    public class FullTimeDAO : DAOBase
    {
        private class SP
        {
            public const string Select = "FTView";
            public const string Add = "FTAdd";
            public const string Edit = "FTUpdate";
            public const string Delete = "FTDelete";
        }

        private class DataRows
        {
            public const string DepNo = "DepNo";
            public const string EmpNo = "EmpNo";
            public const string EmpFirstName = "EmpFirstName";
            public const string EmpLastName = "EmpLastName";
            public const string Tel = "Tel";
            public const string Addr = "Addr";
            public const string IsDone = "IsDone";
            public const string InintValue = "0";
            public const string Years = "Years";
        }


        public FullTimeDAO(string ConnectionString)
        {
            base._DbIstance.ConnectiinString = ConnectionString;
        }

        public override object Select<T>(T Obj)
        {
            Operation Oper = new Operation();
            Oper.ObjList = new List<object>();
            base._DbIstance.AddParameter(DataRows.EmpNo, GlobalParameter.DefaultValues.Negative);
            DataTable dt = base._DbIstance.ExeProcedureGetDataTable(SP.Select);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    FullTime FT = new FullTime();
                    FT.EmpNo = dr[DataRows.EmpNo].ToString();
                    FT.EmpFirstName = dr[DataRows.EmpFirstName].ToString();
                    FT.EmpLastName = dr[DataRows.EmpLastName].ToString();
                    FT.Tel = dr[DataRows.Tel].ToString();
                    FT.Addr = dr[DataRows.Addr].ToString();
                    FT.DepNo = dr[DataRows.DepNo].ToString();
                    FT.Years = Convert.ToInt32(dr[DataRows.Years]);
                    Oper.ObjList.Add(FT);
                }
            }
            return Oper;
        }


        public override object Add<T>(T Obj)
        {
            FullTime FT = JsonConvert.DeserializeObject<FullTime>(Obj.ToString());
            Operation Oper = new Operation();
            FT.EmpNo = DateTime.Now.ToString(GlobalParameter.DefaultValues.DateTimeFormate);
            base._DbIstance.AddParameter(DataRows.EmpFirstName, FT.EmpFirstName);
            base._DbIstance.AddParameter(DataRows.EmpLastName, FT.EmpLastName);
            base._DbIstance.AddParameter(DataRows.Addr, FT.Addr);
            base._DbIstance.AddParameter(DataRows.Tel, FT.Tel);
            base._DbIstance.AddParameter(DataRows.DepNo, FT.DepNo);
            base._DbIstance.AddParameter(DataRows.Years, FT.Years);
            base._DbIstance.AddParameter(DataRows.IsDone, DataRows.InintValue, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.AddParameter(DataRows.EmpNo, FT.EmpNo, System.Data.SqlDbType.NVarChar, 20, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureHasResult(SP.Add);
            Oper.Obj = FT;
            string OutParameter = base._DbIstance.OutParameterValues[0].ToString();
            if (OutParameter == GlobalParameter.StoreProcedureResult.Error)
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Error;
            }
            else
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Success;
            }
            return Oper;
        }

        public override object Edit<T>(T Obj)
        {
            FullTime FT = JsonConvert.DeserializeObject<FullTime>(Obj.ToString());
            Operation Oper = new Operation();
            Oper.Obj = FT;
            base._DbIstance.AddParameter(DataRows.EmpNo, FT.EmpNo);
            base._DbIstance.AddParameter(DataRows.EmpFirstName, FT.EmpFirstName);
            base._DbIstance.AddParameter(DataRows.EmpLastName, FT.EmpLastName);
            base._DbIstance.AddParameter(DataRows.Tel, FT.Tel);
            base._DbIstance.AddParameter(DataRows.Addr, FT.Addr);
            base._DbIstance.AddParameter(DataRows.Years, FT.Years);
            base._DbIstance.AddParameter(DataRows.IsDone, DataRows.InintValue, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureHasResult(SP.Edit);
            string OutParameter = base._DbIstance.OutParameterValues[0].ToString();
            if (OutParameter == GlobalParameter.StoreProcedureResult.Error)
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Error;
            }
            else
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Success;
            }
            return Oper;
        }


        public override object Delete<T>(T Obj)
        {

            FullTime FT = JsonConvert.DeserializeObject<FullTime>(Obj.ToString());
            Operation Oper = new Operation();
            Oper.Obj = FT;
            base._DbIstance.AddParameter(DataRows.EmpNo, FT.EmpNo);
            base._DbIstance.AddParameter(DataRows.IsDone, DataRows.InintValue, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureHasResult(SP.Delete);
            string OutParameter = base._DbIstance.OutParameterValues[0].ToString();
            if (OutParameter == GlobalParameter.StoreProcedureResult.Error)
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Error;
            }
            else
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Success;
            }
            return Oper;
        }

    }
}