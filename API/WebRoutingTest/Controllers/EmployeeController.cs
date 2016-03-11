using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Code.DAO;
using WebApplication1.Models;

namespace WebRoutingTest.Controllers
{
    public class EmployeeController : ApiController
    {        

        /// <summary> 瀏覽員工資料 (全) /// </summary>
        /// <returns></returns>
        [Route("api/Employee/ViewAll")]
        [HttpGet]
        public string ViewAll()
        {
            EmployeeDAO EmpDAO = new EmployeeDAO();
            EmployeeMgrViewModels.EmployeeManamgerViewModel Model = new EmployeeMgrViewModels.EmployeeManamgerViewModel();
            Model = EmpDAO.ViewEmployees(null);         
            return JsonConvert.SerializeObject(Model);
        }

        
        /// <summary> 新增員工資料  走JSON /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        [Route("api/Employee/AddEmployee")]
        [HttpPost]
        public string AddEmployee(Employee emp)
        {
            EmployeeDAO EmpDAO = new EmployeeDAO();
            EmpDAO.Create(emp);
            return JsonConvert.SerializeObject(emp);
        }



        [Route("api/Employee/EditEmployee")]
        /// <summary>編輯員工資料 JSON /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        [HttpPost]
        public WebApplication1.Code.DAO.ResultCodeManager.ResultCode EditEmployee(Employee emp)
        {
            try
            {
                EmployeeDAO EmpDAO = new EmployeeDAO();
                 EmpDAO.Edit(emp);
                return WebApplication1.Code.DAO.ResultCodeManager.ResultCode.EditSucces;
            }
            catch (Exception ex)
            {
                LogDAO log = new LogDAO();
                log.LogMethod(ex.Message);
                return WebApplication1.Code.DAO.ResultCodeManager.ResultCode.EditError;
            }
        }

        

        /// <summary>刪除員工資料 丟一個參數 </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/Employee/DeleteEmployee")]
        [HttpPost]
        // DELETE: api/Employee/5
        public WebApplication1.Code.DAO.ResultCodeManager.ResultCode DeleteEmployee(Employee Emp)
        {
            try
            {
                EmployeeDAO EmpDao = new EmployeeDAO();
                EmpDao.Delete(Emp);
                return WebApplication1.Code.DAO.ResultCodeManager.ResultCode.DeleteSucces;
            }
            catch (Exception ex)
            {
                LogDAO log = new LogDAO();
                log.LogMethod(ex.Message);
                return WebApplication1.Code.DAO.ResultCodeManager.ResultCode.DeleteError;
            }
        }



    }
}
