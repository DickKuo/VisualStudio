using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApplication1.Code;
using WebApplication1.Code.DAO;

namespace WebRoutingTest.Models.Code.DAO
{
    public class PhotoDAO : DAOBase
    {
        private class SP
        {
            public const string Select = "SpSelectPhoto";
            public const string Add = "SpAddPhoto";
            public const string Edit = "SpEditPhoto";
            public const string Delete = "SpDeletePhoto";
        }

        /// <summary>資料欄位名稱</summary>
        private class DataRows
        {
            public const string ImageNo = "ImageNo";
            public const string Name = "Name";
            public const string AttachedFileName = "AttachedFileName";
            public const string ImageOrder = "ImageOrder";
            public const string Remark = "Remark";
            public const string IsSuccess = "IsSuccess";
        }


        public PhotoDAO(string ConnectionString)
        {
            base.SetConnetionString(ConnectionString);
        }


        public override object Select<T>(T Obj)
        {
            Operation Oper = new Operation();
            Oper.ObjList = new List<object>();
            base._DbIstance.AddParameter(DataRows.IsSuccess, 0, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
            DataTable dt = base._DbIstance.ExeProcedureGetDataTable(SP.Select);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Photo photo = new Photo();
                    photo.ImageNo = dr[DataRows.ImageNo].ToString();
                    photo.Remark = dr[DataRows.Remark].ToString();
                    photo.AttachedFileName = dr[DataRows.AttachedFileName].ToString();
                    Oper.ObjList.Add(photo);
                }
            }
            return Oper;
        }



        public override object Add<T>(T Obj)
        {
            Photo photo = JsonConvert.DeserializeObject<Photo>(Obj.ToString());
            Operation Oper = new Operation();
            //Photo photo = JsonConvert.DeserializeObject<Photo>(Oper.Obj.ToString());
            Oper.Obj = Obj;
            base._DbIstance.AddParameter(DataRows.ImageNo, photo.ImageNo);
            base._DbIstance.AddParameter(DataRows.Remark, photo.Remark);
            base._DbIstance.AddParameter(DataRows.AttachedFileName, photo.AttachedFileName);
            //base.Procedure.AddParameter(DataRows.Remark, photo.Remark);
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


        public override object Edit<T>(T Obj)
        {
            throw new NotImplementedException();
        }


        public override object Delete<T>(T Obj)
        {
            Photo photo = JsonConvert.DeserializeObject<Photo>(Obj.ToString());
            Operation Oper = new Operation();
            Oper.Obj = Obj;
            //Photo photo = JsonConvert.DeserializeObject<Photo>(Oper.Obj.ToString());
            base._DbIstance.AddParameter(DataRows.ImageNo, photo.ImageNo);
            base._DbIstance.AddParameter(DataRows.IsSuccess, 0, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
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