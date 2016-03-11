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
    public class PTimeDAO : DAOBase
    {
        private class SP
        {
            public const string Select = "PTView";
            public const string Add = "PTAdd";
            public const string Edit = "PTUpdate";
            public const string Delete = "PTDelete";
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
            public const string WorkDays = "WorkDays";
        }


        public PTimeDAO(string ConnectionString)
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
                    PTime Pt = new PTime();
                    Pt.EmpNo = dr[DataRows.EmpNo].ToString();
                    Pt.EmpFirstName = dr[DataRows.EmpFirstName].ToString();
                    Pt.EmpLastName = dr[DataRows.EmpLastName].ToString();
                    Pt.Tel = dr[DataRows.Tel].ToString();
                    Pt.Addr = dr[DataRows.Addr].ToString();
                    Pt.DepNo = dr[DataRows.DepNo].ToString();
                    Pt.WorkDays = Convert.ToInt32(dr[DataRows.WorkDays]);
                    Oper.ObjList.Add(Pt);
                }
            }
            return Oper;
        }

        public override object Add<T>(T Obj)
        {
            PTime Pt = JsonConvert.DeserializeObject<PTime>(Obj.ToString());
            Operation Oper = new Operation();
            Pt.EmpNo = DateTime.Now.ToString(GlobalParameter.DefaultValues.DateTimeFormate);
            base._DbIstance.AddParameter(DataRows.EmpFirstName, Pt.EmpFirstName);
            base._DbIstance.AddParameter(DataRows.EmpLastName, Pt.EmpLastName);
            base._DbIstance.AddParameter(DataRows.Addr, Pt.Addr);
            base._DbIstance.AddParameter(DataRows.Tel, Pt.Tel);
            base._DbIstance.AddParameter(DataRows.DepNo, Pt.DepNo);
            base._DbIstance.AddParameter(DataRows.WorkDays, Pt.WorkDays);
            base._DbIstance.AddParameter(DataRows.IsDone, DataRows.InintValue, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.AddParameter(DataRows.EmpNo, Pt.EmpNo, System.Data.SqlDbType.NVarChar, 20, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureHasResult(SP.Add);
            Oper.Obj = Pt;
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
            PTime PT = JsonConvert.DeserializeObject<PTime>(Obj.ToString());
            Operation Oper = new Operation();
            Oper.Obj = PT;
            base._DbIstance.AddParameter(DataRows.EmpNo, PT.EmpNo);
            base._DbIstance.AddParameter(DataRows.EmpFirstName, PT.EmpFirstName);
            base._DbIstance.AddParameter(DataRows.EmpLastName, PT.EmpLastName);
            base._DbIstance.AddParameter(DataRows.Tel, PT.Tel);
            base._DbIstance.AddParameter(DataRows.Addr, PT.Addr);
            base._DbIstance.AddParameter(DataRows.WorkDays, PT.WorkDays);
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
            PTime PT = JsonConvert.DeserializeObject<PTime>(Obj.ToString());
            Operation Oper = new Operation();
            Oper.Obj = PT;
            base._DbIstance.AddParameter(DataRows.EmpNo, PT.EmpNo);
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