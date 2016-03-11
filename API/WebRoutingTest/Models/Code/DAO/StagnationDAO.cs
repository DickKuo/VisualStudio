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
    public class StagnationDAO : DAOBase
    {

        private class SP
        {
            public const string Select = "STView";
            public const string Add = "STAdd";
            public const string Edit = "STUpdate";
            public const string Delete = "STDelete";
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
            public const string SourceCompany = "SourceCompany";
        }

       

        public StagnationDAO(string ConnectionString)
        {
            base._DbIstance.ConnectiinString = ConnectionString;
        }

        public override object Select<T>(T Obj)
        {
            Operation Oper = new Operation();
            Oper.ObjList = new List<object>();
            base._DbIstance.AddParameter(DataRows.EmpNo,GlobalParameter.DefaultValues.Negative);
            DataTable dt = base._DbIstance.ExeProcedureGetDataTable(SP.Select);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Stagnation ST = new Stagnation();
                    ST.EmpNo = dr[DataRows.EmpNo].ToString();
                    ST.EmpFirstName = dr[DataRows.EmpFirstName].ToString();
                    ST.EmpLastName = dr[DataRows.EmpLastName].ToString();
                    ST.Tel = dr[DataRows.Tel].ToString();
                    ST.Addr = dr[DataRows.Addr].ToString();
                    ST.DepNo = dr[DataRows.DepNo].ToString();
                    ST.SourceCompany = dr[DataRows.SourceCompany].ToString();
                    Oper.ObjList.Add(ST);
                }
            }
            return Oper;
        }

        public override object Add<T>(T Obj)
        {
            Stagnation ST = JsonConvert.DeserializeObject<Stagnation>(Obj.ToString());
            Operation Oper = new Operation();
            ST.EmpNo = DateTime.Now.ToString(GlobalParameter.DefaultValues.DateTimeFormate);
            base._DbIstance.AddParameter(DataRows.EmpFirstName, ST.EmpFirstName);
            base._DbIstance.AddParameter(DataRows.EmpLastName, ST.EmpLastName);
            base._DbIstance.AddParameter(DataRows.Addr, ST.Addr);
            base._DbIstance.AddParameter(DataRows.Tel, ST.Tel);
            base._DbIstance.AddParameter(DataRows.DepNo, ST.DepNo);
            base._DbIstance.AddParameter(DataRows.SourceCompany, ST.SourceCompany);
            base._DbIstance.AddParameter(DataRows.IsDone, DataRows.InintValue, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.AddParameter(DataRows.EmpNo, ST.EmpNo, System.Data.SqlDbType.NVarChar, 20, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureHasResult(SP.Add);
            Oper.Obj = ST;
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
            Stagnation ST = JsonConvert.DeserializeObject<Stagnation>(Obj.ToString());
            Operation Oper = new Operation();
            Oper.Obj = ST;
            base._DbIstance.AddParameter(DataRows.EmpNo, ST.EmpNo);
            base._DbIstance.AddParameter(DataRows.EmpFirstName, ST.EmpFirstName);
            base._DbIstance.AddParameter(DataRows.EmpLastName, ST.EmpLastName);
            base._DbIstance.AddParameter(DataRows.Tel, ST.Tel);
            base._DbIstance.AddParameter(DataRows.Addr, ST.Addr);
            base._DbIstance.AddParameter(DataRows.SourceCompany, ST.SourceCompany);
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
            Stagnation ST = JsonConvert.DeserializeObject<Stagnation>(Obj.ToString());
            Operation Oper = new Operation();
            Oper.Obj = ST;
            base._DbIstance.AddParameter(DataRows.EmpNo, ST.EmpNo);
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