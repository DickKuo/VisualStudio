using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Code.DAO;
using WebApplication1.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

namespace WebApplication1.Controllers
{
    public class EmployeeController : BaseLoginController
    {
        UseStoreProcedure SP = new UseStoreProcedure();

        
      
        private  bool IsUseApi = Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings["IsUseApi"]);       //設定是否Api
        private  string Apiurl = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl"].ToString();     //Api 位置   Employee
        private  string Bankuri = System.Web.Configuration.WebConfigurationManager.AppSettings["TradeUri"].ToString();    //Api 位置   Trade
       

        
        
        /// <summary>員工列表/// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            EmployeeDAO EmpDAO = new EmployeeDAO();
            return View(EmpDAO.ViewEmployees(null));
        }



        public ActionResult CreateView(EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            

            return View();
        }


        [HttpPost]
        public ActionResult Create(EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            EmployeeDAO EmpDAO = new EmployeeDAO();
            EmpDAO.Create(Model.Emp);
            return RedirectToAction("Index");
        }
        

        /// <summary> 用流覽的畫面帶過來</summary>
        /// <param name="Emp"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditView(EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            //用畫面資料代           
            Model.Emp = JsonConvert.DeserializeObject<Employee>(Model.EmployeeJson);
            return View(Model);
        }


        /// <summary>編輯功能處理</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            EmployeeDAO EmpDAO = new EmployeeDAO();
            EmpDAO.Edit(Model.Emp);
            return RedirectToAction("Index");
        }


        public ActionResult BDelete(EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            Model.Emp = JsonConvert.DeserializeObject<Employee>(Model.EmployeeJson);
            EmployeeDAO EmpDAO = new EmployeeDAO();
            EmpDAO.Delete(Model.Emp);
            return RedirectToAction("Index");
        }


        /// <summary>金流載入選擇畫面</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult Account(EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            return PartialView("_BankAccountViewPartial");
        }


        /// <summary> 金流操作</summary>
        /// <returns></returns>
        public ActionResult BankAccountPartial(TradeInfo Info)
        {
            ApiOperation Operation = new ApiOperation();     
            string Uri = string.Format("{0}/{1}", Bankuri, "SaveMoney");
            Operation.Uri = Uri;
            Operation.obj = Info;
            WebApi Api = new WebApi();
            string temp = Api.ApiOperation(Operation);
            TradeInfo ResultInfo = JsonConvert.DeserializeObject<TradeInfo>(temp);                     
            return Json(ResultInfo);
        }


        public ActionResult CashFlow()
        {
            if (IsUseApi)
            {
                return View(IndexByAjaxWithApi());
            }
            else
            {
                return View();
            }
        }


        /// <summary> 用Ajax方式修改頁面的方式 </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult IndexByAjax()
        {
            if (IsUseApi)
            {               
                return View(IndexByAjaxWithApi());
            }
            else
            {
                EmployeeDAO EmpDAO = new EmployeeDAO();
                return View(EmpDAO.ViewEmployees(null));
            }
        }  //end IndexByAjax


        /// <summary> 員工列表使用Api的方法 /// </summary>
        /// <returns></returns>
        private EmployeeMgrViewModels.EmployeeManamgerViewModel IndexByAjaxWithApi()
        {
            EmployeeMgrViewModels.EmployeeManamgerViewModel Model =new EmployeeMgrViewModels.EmployeeManamgerViewModel ();
            EmployeeWebApi empApi = new EmployeeWebApi();
            ApiOperation Operation = new ApiOperation();          
            Operation.Uri = Apiurl;
            Operation.Action=empApi.View;
            Operation.Parameters=new List<string> ();
            Operation.Methode=HttpMethod.Get;
            var temp = JsonConvert.DeserializeObject<dynamic>(empApi.ApiMVC(Operation));
            Model = JsonConvert.DeserializeObject<EmployeeMgrViewModels.EmployeeManamgerViewModel>(temp);

            return Model;
        }//end IndexByAjaxWithApi


        /// <summary>新增的表單畫面</summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PartialCreateView()
        {
            return View();
        }


        /// <summary>編輯畫面使用PartialView</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPartialViewAction(EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            Model.Emp = JsonConvert.DeserializeObject<Employee>(Model.EmployeeJson);
            return PartialView("_EditViewPartial", Model);
        }//end EditPartialViewAction



        
        /// <summary> 金流控制畫面</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CashFlowPartialViewAction(EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            Model.Emp = JsonConvert.DeserializeObject<Employee>(Model.EmployeeJson);
            return PartialView("_CaseFlowViewPartial");
        }//end CashFlowPartialViewAction




        /// <summary>  ajax編輯 </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult EditAction(EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            if (IsUseApi)
            {
                ///走Api                
                EmployeeWebApi EmpApi = new EmployeeWebApi();
                EmpApi.SetApiUrl(Apiurl);
                EmpApi.EditEmployee(Model.Emp);
                return Json(Model.Emp);
            }
            else
            {
                EmployeeDAO DAO = new EmployeeDAO();
                DAO.Edit(Model.Emp);
                return Json(Model.Emp);
            }
        }//end EditAction



        /// <summary> 顯示新增畫面的PartialView </summary>
        /// <returns></returns>
        public ActionResult ADDPartialViewAction()
        {
            return PartialView("_CreateViewPartial");
        }//end ADDPartialViewAction
        

        /// <summary> 新增員工Action </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAction(EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            if (IsUseApi)
            {
                ///走Api     
                EmployeeWebApi WebApi = new EmployeeWebApi();
                WebApi.SetApiUrl(Apiurl);
                return Json(WebApi.AddEmployee(Model.Emp));
            }
            else
            {
                EmployeeDAO DAO = new EmployeeDAO();
                DAO.Create(Model.Emp);
                return Json(Model.Emp);
            }
        } //end AddAction


        /// <summary> 員工刪除動作 /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult DeleteAction(EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            Model.Emp = JsonConvert.DeserializeObject<Employee>(Model.EmployeeJson);
            if (IsUseApi)
            {
                ///走Api     
                EmployeeWebApi Api = new EmployeeWebApi();
                Api.SetApiUrl(Apiurl);
                Api.DeleteEmployee(Model.Emp);
                string EmpNoId = String.Format("Emp_{0}", Model.Emp.EmpNo);
                return Json(EmpNoId);
            }
            else
            {
                EmployeeDAO EmpDAO = new EmployeeDAO();
                string result = EmpDAO.Delete(Model.Emp);
                string ReturnId = string.Empty;
                if (result == MessageType.Sucess)
                {
                    string EmpNoId = String.Format("Emp_{0}", Model.Emp.EmpNo);
                    return Json(EmpNoId);
                }
                else
                {
                    return Json(ResultCodeManager.ResultCode.DeleteError);
                }
            }
        } //end DeleteAction



        public ActionResult GoToIndexAction(EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
       
            return RedirectToAction("Index");
        }


      
    }
}