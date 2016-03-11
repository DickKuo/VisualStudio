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
    public class RolesMenuDAO : DAOBase
    {

        public RolesMenuDAO(string ConnectionString)
        {
            base.SetConnetionString(ConnectionString);
        }

        private class SpName {
            public const string SpSelect = "SPRolesMenuQuery";
            public const string SpDelete = "SPRolesMenuDelete";
            public const string SpAdd = "SPRolesMenuAdd";
        }

        private class RowNames {
            public const string RoleID = "RoleID";
            public const string MenuNo = "MenuNo";
            public const string IsDone = "IsDone";
        }


        public override object Select<T>(T Obj)
        {
            Operation Oper = new Operation();
            Role _Role=JsonConvert.DeserializeObject<Role>(Obj.ToString());
            base._DbIstance.AddParameter(RowNames.RoleID, _Role.RoleID);
            base._DbIstance.AddParameter(RowNames.IsDone, 1, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            DataTable dt = base._DbIstance.ExeProcedureGetDataTable(SpName.SpSelect);
            foreach(DataRow dr in dt.Rows)
            {
                RolesMenu RoleMenu = new RolesMenu();
                RoleMenu.RoleID = _Role.RoleID;
                RoleMenu.MenuNo = dr[RowNames.MenuNo].ToString();
                _Role.RoleMenuList.Add(RoleMenu);
            }
            Oper.Obj = _Role;
            return Oper;
        }

        public override object Add<T>(T Obj)
        {
            Role Roles = JsonConvert.DeserializeObject<Role>(Obj.ToString());
            Operation Oper = new Operation();
            foreach (RolesMenu Item in Roles.RoleMenuList)
            {
                base._DbIstance.AddParameter(RowNames.RoleID, Roles.RoleID);
                base._DbIstance.AddParameter(RowNames.MenuNo, Item.MenuNo);
                base._DbIstance.AddParameter(RowNames.IsDone, 1, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
                base._DbIstance.ExeProcedureGetDataTable(SpName.SpAdd);
            }
            Oper.Obj = Roles;
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
            Operation Oper = new Operation();
            Role _Role = JsonConvert.DeserializeObject<Role>(Obj.ToString());
            base._DbIstance.AddParameter(RowNames.RoleID, _Role.RoleID);
            base._DbIstance.AddParameter(RowNames.IsDone, 1, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            DataTable dt = base._DbIstance.ExeProcedureGetDataTable(SpName.SpDelete);
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