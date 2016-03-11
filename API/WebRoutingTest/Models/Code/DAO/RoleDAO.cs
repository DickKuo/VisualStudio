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
    public class RoleDAO : DAOBase
    {
        public RoleDAO(string ConnectionString)
        {
            base.SetConnetionString(ConnectionString);
        }

        private class SP {
            public const string SpSelect = "SpRoleSelect";
            public const string SpAdd = "SpRoleAdd";
            public const string SpDelete = "SpRoleDelete";
            public const string SpEdit = "SpRoleEdit";
        }

        private class DataRows
        {
            public const string RoleNo = "RoleNo";
            public const string RoleID = "RoleID";
            public const string Name = "Name";
            public const string IsEnable = "IsEnable"; 
            public const string IsDone = "IsDone";
        }


        public override object Select<T>(T Obj)
        {
            Operation Oper = new Operation();                       
            List<Role> RoleList = new List<Role>();
            string RoleNo = GlobalParameter.DefaultValues.Negative;
            if (Obj != null)
            {
                Role _Role = JsonConvert.DeserializeObject<Role>(Obj.ToString());
                RoleNo = _Role.RoleNo;
            }
            base._DbIstance.AddParameter(DataRows.RoleNo, RoleNo);
            base._DbIstance.AddParameter(DataRows.IsDone, 1, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            DataTable dt = base._DbIstance.ExeProcedureGetDataTable(SP.SpSelect);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Role _Role = new Role();
                    _Role.RoleNo = dr[DataRows.RoleNo].ToString();
                    _Role.RoleID = dr[DataRows.RoleID].ToString();
                    _Role.Name = dr[DataRows.Name].ToString();
                    _Role.IsEnable = Convert.ToBoolean(dr[DataRows.IsEnable]);
                    RoleList.Add(_Role);
                }
            }
            if (Obj != null)
            {
                Role Role = JsonConvert.DeserializeObject<Role>(Obj.ToString());
                if (Role.RoleNo != null)
                {
                    var SelectArray = from v in RoleList
                                where v.RoleNo == Role.RoleNo
                                select v;
                    RoleList = SelectArray.ToList();
                }
            }
            Oper.Obj = RoleList;
            return Oper;
        }



        public override object Add<T>(T Obj)
        {
            Operation Oper = new Operation();            
            Role _Role = JsonConvert.DeserializeObject<Role>(Obj.ToString());
            _Role.RoleID = Guid.NewGuid().ToString();
            base._DbIstance.AddParameter(DataRows.RoleNo, _Role.RoleNo);
            base._DbIstance.AddParameter(DataRows.Name, _Role.Name);
            base._DbIstance.AddParameter(DataRows.RoleID, _Role.RoleID);
            base._DbIstance.AddParameter(DataRows.IsEnable, _Role.IsEnable);
            base._DbIstance.AddParameter(DataRows.IsDone, 1, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureGetDataTable(SP.SpAdd);
            //#region 加入名細
            ////RolesMenuDAO RoleMenuDao = new RolesMenuDAO(base._DbIstance.ConnectiinString);
            ////RoleMenuDao.Add(Obj.ToString());
            //#endregion
            Oper.Obj = _Role;
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
            Operation Oper = new Operation();
            if (Obj != null)
            {
               Oper= this.Select(Obj) as Operation;
                List<Role> RoleList = Oper.Obj as List<Role>;
                if (RoleList.Count > 0)
                {
                    Role _Role = JsonConvert.DeserializeObject<Role>(Obj.ToString());
                    _Role.RoleID = RoleList[0].RoleID;
                    RolesMenuDAO DAO = new RolesMenuDAO(base._DbIstance.ConnectiinString);
                    string Josn = JsonConvert.SerializeObject(_Role);
                    DAO.Delete(Josn);
                    DAO.Add(Josn);
                    base._DbIstance.AddParameter(DataRows.Name, _Role.Name);
                    base._DbIstance.AddParameter(DataRows.IsDone, 1, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
                    base._DbIstance.ExeProcedureGetDataTable(SP.SpEdit);
                     Oper.Obj = _Role;
            string OutParameter = base._DbIstance.OutParameterValues[0].ToString();
            if (OutParameter == GlobalParameter.StoreProcedureResult.Error)
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Error;
            }
            else
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Success;
            }
                }
            }
            return Oper;
        }

        public override object Delete<T>(T Obj)
        {
            Operation Oper = new Operation();
            Role _Role = JsonConvert.DeserializeObject<Role>(Obj.ToString());

            #region  刪除名細
            string Json = JsonConvert.SerializeObject(_Role);
            Oper = this.Select(Json) as Operation;
            List<Role> RoleList = Oper.Obj as List<Role>;
            string RoleJson = JsonConvert.SerializeObject(RoleList[0]);
            RolesMenuDAO _RolesMenuDAO = new RolesMenuDAO(base._DbIstance.ConnectiinString);
            _RolesMenuDAO.Delete(RoleJson);
            #endregion

            #region  刪除主表
            base._DbIstance.AddParameter(DataRows.RoleNo, _Role.RoleNo);
            base._DbIstance.AddParameter(DataRows.IsDone, 1, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureGetDataTable(SP.SpDelete);
            #endregion

            Oper.Obj = _Role;
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