using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Code.DAO;
using WebApplication1.Models.Code;
using WebRoutingTest.Models.Code.DAO;

namespace WebRoutingTest.Models.Code
{

    public class FactoryOperation
    {
        private string ConnectionString =  System.Web.Configuration.WebConfigurationManager.AppSettings["ConnectionString"].ToString();

        

        /// <summary>選擇模式</summary>
        /// <param name="Oper"></param>
        public DAOBase SelectFactory(Operation Oper)
        {
            DAOBase DAO = null;
            switch (Oper.Entity)
            {
                case Entities.EntitiesList.Position:
                    DAO = new PositionDAO(ConnectionString);
                    break;
                case Entities.EntitiesList.Department:
                    DAO = new DepartmentDAO(ConnectionString);
                    break;
                case Entities.EntitiesList.Employee:
                    DAO = new WebRoutingTest.Models.Code.DAO.EmployeeDAO(ConnectionString);
                    break;
                case Entities.EntitiesList.Photo:
                    DAO = new PhotoDAO(ConnectionString);                  
                    break;
                case Entities.EntitiesList.PTime:
                    DAO = new PTimeDAO(ConnectionString);
                    break;
                case Entities.EntitiesList.FullTime:
                    DAO = new FullTimeDAO(ConnectionString);
                    break;
                case Entities.EntitiesList.Stagnation:
                    DAO = new StagnationDAO(ConnectionString);
                    break;
                case Entities.EntitiesList.Menu:
                    DAO = new MenuDAO(ConnectionString);
                    break;
                case Entities.EntitiesList.Role:
                    DAO = new RoleDAO(ConnectionString);
                    break;
                case Entities.EntitiesList.RolesMenu:
                    DAO = new RolesMenuDAO(ConnectionString);
                    break;
                case Entities.EntitiesList.User:
                    DAO = new UserAccountDAO(ConnectionString);
                    break;
            }
            return DAO;
        }


        /// <summary>操作方法</summary>
        /// <param name="Oper"></param>
        /// <returns></returns>
        public Operation DoOperation(Operation Oper, DAOBase DAO)
        {
            switch (Oper.Method)
            {
                case OperationMethod.Method.Select:
                    Oper = DAO.Select(Oper.Obj) as Operation;
                    break;
                case OperationMethod.Method.Add:
                    Oper = DAO.Add(Oper.Obj) as Operation;
                    break;
                case OperationMethod.Method.Edit:
                    Oper = DAO.Edit(Oper.Obj) as Operation;
                    break;
                case OperationMethod.Method.Delete:
                    Oper = DAO.Delete(Oper.Obj) as Operation;
                    break;
            }
            return Oper;
        }

    }

}