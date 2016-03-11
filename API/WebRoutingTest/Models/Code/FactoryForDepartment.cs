using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebRoutingTest.Models.Code.DAO;

namespace WebRoutingTest.Models.Code
{
    public class FactoryForDepartment
    {


        public DAOBranchBase SelectDAO(JsonObj Obj)
        {
            DAOBranchBase DAO = null;
            switch (Obj.Source)
            {
                case Sources.DAOSource.SP:
                    DAO = new DAOSPDepartment();
                    break;
                case Sources.DAOSource.Linq:
                    DAO = new DAOLinqDepartment();
                    break;
            }
            return DAO;
        }


        public JsonObj Operation(JsonObj Obj, DAOBranchBase DAO)
        {
            switch (Obj.Method)
            {
                case OperMethods.OperMethod.AddDepartment:
                    Obj = DAO.AddDepartment(Obj.JsonString) as JsonObj;
                    break;
                case OperMethods.OperMethod.DeleteDepartmentByDepNo:
                    Obj = DAO.DeleteDepartmentByDepNo(Obj.JsonString) as JsonObj;
                    break;
                case OperMethods.OperMethod.EditDepartment:
                    Obj = DAO.EditDepartment(Obj.JsonString) as JsonObj;
                    break;

                case OperMethods.OperMethod.GetALLDepartment:
                    Obj = DAO.GetALLDepartment() as JsonObj;
                    break;

                case OperMethods.OperMethod.GetALLEmployeeByDepNo:
                    Obj = DAO.GetALLEmployeeByDepNo(Obj.JsonString) as JsonObj;
                    break;

                case OperMethods.OperMethod.GetAllEmployeeNoDepartment:
                    Obj = DAO.GetAllEmployeeNoDepartment() as JsonObj;
                    break;
                //case OperMethods.OperMethod.GetDepEmployeeSlefAndNoDep:
                //    Obj = DAO.GetDepEmployeeSlefAndNoDep(Obj.JsonString) as JsonObj;
                //    break;
                case OperMethods.OperMethod.AddEmployeeByDepNo:
                    Obj = DAO.AddEmployeeByDepNo(Obj.JsonString) as JsonObj;
                    break;
                case OperMethods.OperMethod.DeleteDepartmentEmp:
                    Obj = DAO.DeleteDepartmentEmp(Obj.JsonString) as JsonObj;
                    break;
            }
            return Obj;
        }

    }
}