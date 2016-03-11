using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApplication1.Code.DAO;

namespace WebRoutingTest.Models.Code.DAO
{
    public class PositionDAO : DAOBase
    {
        /// <summary>StoreProcedure名稱</summary>
        private class SP
        {
            public const string Select = "SpSelectPosition";
            public const string Add = "SpAddPosition";
            public const string Edit = "SpEditPosition";
            public const string Delete = "SpDeletePosition";
        }

        /// <summary>資料欄位名稱</summary>
        private class DataRows
        {
            public const string PostNo = "PostNo";
            public const string QA = "QA";
            public const string RD = "RD";
            public const string IT = "IT";
            public const string HR = "HR";
            public const string IsSuccess = "IsSuccess";
        }

        public PositionDAO(string ConnectionString)
        {
            base.SetConnetionString(ConnectionString);
        }


        /// <summary>撈取資料庫所有員工列表</summary>
        /// <returns></returns>
        public override object Select<T>(T Obj)
        {
            Operation Oper = new Operation();
            Oper.ObjList = new List<object>();
            DataTable dt = base._DbIstance.ExeProcedureGetDataTable(SP.Select);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Position Po = new Position();
                    Po.PositionNo = dr[DataRows.PostNo].ToString();
                    Po.QA = dr[DataRows.QA].ToString();
                    Po.RD = dr[DataRows.RD].ToString();
                    Po.IT = dr[DataRows.IT].ToString();
                    Po.HR = dr[DataRows.HR].ToString();
                    Oper.ObjList.Add(Po);
                }
            }            
            return Oper;
        }


        /// <summary>新增</summary>
        /// <param name="Oper"></param>
        /// <returns></returns>
        public override object Add<T>(T Obj)
        {
            Position Po = Obj as Position;
            Operation Oper = new Operation();
            Oper.Obj = Obj;
            base._DbIstance.AddParameter(DataRows.PostNo, Po.PositionNo);
            base._DbIstance.AddParameter(DataRows.QA, Po.QA);
            base._DbIstance.AddParameter(DataRows.RD, Po.RD);
            base._DbIstance.AddParameter(DataRows.IT, Po.IT);
            base._DbIstance.AddParameter(DataRows.HR, Po.HR);
            base._DbIstance.AddParameter(DataRows.IsSuccess, 0, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureHasResult(SP.Add);
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


        /// <summary>刪除</summary>
        /// <param name="Oper"></param>
        /// <returns></returns>
        public override object Delete<T>(T Obj)
        {
            Position Po = Obj as Position;
            Operation Oper = new Operation();
            Oper.Obj = Obj;
            base._DbIstance.AddParameter(DataRows.PostNo, Po.PositionNo);
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


        /// <summary>編輯</summary>
        /// <param name="Oper"></param>
        /// <returns></returns>
        public override object Edit<T>(T Obj)
        {
            Position Po = Obj as Position;
            Operation Oper = new Operation();
            Oper.Obj = Obj;
            base._DbIstance.AddParameter(DataRows.PostNo, Po.PositionNo);
            base._DbIstance.AddParameter(DataRows.QA, Po.QA);
            base._DbIstance.AddParameter(DataRows.RD, Po.RD);
            base._DbIstance.AddParameter(DataRows.IT, Po.IT);
            base._DbIstance.AddParameter(DataRows.HR, Po.HR);
            base._DbIstance.AddParameter(DataRows.IsSuccess, 0, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
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

    }
}