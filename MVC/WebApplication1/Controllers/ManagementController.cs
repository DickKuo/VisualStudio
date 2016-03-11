using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Code.DAO;
using WebApplication1.Models;
using WebApplication1.Models.Code;
using WebRoutingTest.Models.Code;

namespace WebApplication1.Controllers
{
    public class ManagementController : BaseLoginController
    {

        private  string Url = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl"].ToString();
        private const string ApiController = "Factory";
        private const string ApiAction = "Factory";


        private  Sources.DAOSource UseSource = Sources.DAOSource.Linq;
        private string NowUrl;
        private const string Controller = "Factory";
        private const string Action = "DepartmentMangement";

        public ManagementController()
        {
            NowUrl = string.Format("{0}/{1}/{2}", Url, Controller, Action);
            if (Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings["IsLinqApi"]))
            {
                UseSource = Sources.DAOSource.Linq;
            }
            else
            {
                UseSource = Sources.DAOSource.SP;
            }
        }

        private class EmpTypes
        {
            public const string FullTime = "全職";
            public const string PTime = "臨時工";
            public const string Stagnation = "駐點";
        }

        /// <summary>頁籤瀏覽畫面 </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            return View();
        }
        
        
        /// <summary>載入員工列表(PartialView)</summary>
        /// <returns></returns>
        public ActionResult LoadingEmployee()
        {
            WebApi Api = new WebApi();
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Select;
            Obj.Entity = Entities.EntitiesList.Employee;
            ApiOperation Oper = Api.Init(Obj);            
           
            Operation Result = JsonConvert.DeserializeObject<Operation>(Api.ApiOperation(Oper));
            EmployeeMgrViewModels.EmployeeManamgerViewModel Model = new EmployeeMgrViewModels.EmployeeManamgerViewModel();
            Model.EmpList = new List<Employee>();
            foreach (var Item in Result.ObjList)
            {
                Employee Emp = JsonConvert.DeserializeObject<Employee>(Item.ToString());
                Model.EmpList.Add(Emp);
            }
            return PartialView("_EmployeePartialView", Model);
        }


        /// <summary>載入部門PartialView</summary>
        /// <returns></returns>
        public ActionResult LoadingDepartment()
        {
            DepartmentViewModels.DepartmentViewModel Model = LoadingDep();
            return PartialView("_DepartmentPartialView", Model);
        }


        /// <summary>載入部門的資料</summary>
        /// <returns></returns>
        private DepartmentViewModels.DepartmentViewModel LoadingDep()
        {          
            DepartmentViewModels.DepartmentViewModel Model = new DepartmentViewModels.DepartmentViewModel();
            WebApi Api = new WebApi();
            JsonObj Json = new JsonObj();
            Json.Method = OperMethods.OperMethod.GetALLDepartment;
            Json.Source = UseSource;
            ApiOperation Oper = new ApiOperation();
            Oper.Methode = HttpMethod.Post;
            Oper.obj = Json;
            Oper.Uri = NowUrl;
            JsonObj Result = JsonConvert.DeserializeObject<JsonObj>(Api.ApiOperation(Oper));
            Model.DepList = JsonConvert.DeserializeObject<List<Department>>(Result.JsonString);
            return Model;
        }


        /// <summary>載入職位</summary>
        /// <returns></returns>
        public ActionResult LoadingPosition()
        {
            WebApi Api = new WebApi();
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Select;
            Obj.Entity = Entities.EntitiesList.Position;
            ApiOperation Oper = Api.Init(Obj);

          
            Operation Result = JsonConvert.DeserializeObject<Operation>(Api.ApiOperation(Oper));
            PositionViewModels.PositionViewModel Model = new PositionViewModels.PositionViewModel();
            Model.PositionList = new List<Position>();
            foreach (var Item in Result.ObjList)
            {
                Position Pos = JsonConvert.DeserializeObject<Position>(Item.ToString());

                Model.PositionList.Add(Pos);
            }

            return PartialView("_PositionPartialView", Model);
        }


        /// <summary>刪除部們所屬的員工</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult DeleteDepartmentEmp(DepartmentViewModels.DepartmentViewModel Model)
        {
            WebApi Api = new WebApi();
            if (Model.QueryString != null)
            {
                Model.Dep.Remark = Model.QueryString;
            }
            JsonObj Jsono = new JsonObj();
            Jsono.Method = OperMethods.OperMethod.DeleteDepartmentEmp;
            Jsono.Source = UseSource;
            Jsono.JsonString = JsonConvert.SerializeObject(Model.Dep);
            ApiOperation Oper = new ApiOperation();
            Oper.Methode = HttpMethod.Post;
            Oper.obj = Jsono;
            Oper.Uri = NowUrl;
            JsonObj Result = JsonConvert.DeserializeObject<JsonObj>(Api.ApiOperation(Oper));
            Model.Dep = JsonConvert.DeserializeObject<Department>(Result.JsonString);
            return Json(Model);

        }

        /// <summary>載入員工新增頁面</summary>
        /// <returns></returns>
        public ActionResult AddEmpView()
        {
            WebApplication1.Models.EmployeeMgrViewModels.EmployeeManamgerViewModel Model = new EmployeeMgrViewModels.EmployeeManamgerViewModel();
            WebApi Api = new WebApi();
            JsonObj Jsonobj = new JsonObj();
            Jsonobj.Method = OperMethods.OperMethod.GetAllEmployeeNoDepartment;
            Jsonobj.Source = UseSource;          
            ApiOperation Oper = new ApiOperation();
            Oper.Methode = HttpMethod.Post;
            Oper.obj = Jsonobj;
            Oper.Uri = NowUrl;
            JsonObj Result = JsonConvert.DeserializeObject<JsonObj>(Api.ApiOperation(Oper));
            Model.EmpList = JsonConvert.DeserializeObject<List<Employee>>(Result.JsonString);
            return PartialView("_AllEmployeePartialView", Model);
        }


        /// <summary>部門加入員工</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult AddEmployeeByDepNo(DepartmentViewModels.DepartmentViewModel Model)
        {
            Department Dep = Model.Dep;
            Dep.Remark = Model.QueryString;
            WebApi Api = new WebApi();
            JsonObj Jsonobj = new JsonObj();
            Jsonobj.Method = OperMethods.OperMethod.AddEmployeeByDepNo;
            Jsonobj.Source = UseSource;
            Jsonobj.JsonString = JsonConvert.SerializeObject(Dep);
            ApiOperation Oper = new ApiOperation();
            Oper.Methode = HttpMethod.Post;
            Oper.obj = Jsonobj;
            Oper.Uri = NowUrl;
            JsonObj Result = JsonConvert.DeserializeObject<JsonObj>(Api.ApiOperation(Oper));


            return Json(Model);
        }

        /// <summary>員工新增操作</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult AddEmpAction(WebApplication1.Models.EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            WebApi Api = new WebApi();
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Add;
            Obj.Entity = GetEmpTypeToEntity(Model);           
            Obj.Obj = GetEmpTypeToObject(Model);
            ApiOperation Oper = Api.Init(Obj);           
          
            string temp = Api.ApiOperation(Oper);
            ApiOperation Result = JsonConvert.DeserializeObject<ApiOperation>(temp);
            Employee Emp = JsonConvert.DeserializeObject<Employee>(Result.obj.ToString());
            Model.Emp = Emp;
            return Json(Model.Emp);
        }


        /// <summary>選擇員工類型</summary>
        /// <returns></returns>
        private Entities.EntitiesList GetEmpTypeToEntity(WebApplication1.Models.EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            Entities.EntitiesList result =Entities.EntitiesList.Employee ;
            switch (Model.EmployeeType)
            {
                case "0":
                    result= Entities.EntitiesList.FullTime;
                    break;
                case"1":
                    result = Entities.EntitiesList.PTime;
                    break;
                case"2":
                    result = Entities.EntitiesList.Stagnation;
                    break;
            }
            return result;
        }


        /// <summary>塞入Class的擴充資料</summary>
        /// <returns></returns>
        private string GetEmpTypeToObject(WebApplication1.Models.EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            string result = null;
            switch (Model.EmployeeType)
            {
                case "0":
                    FullTime FT =new FullTime();
                    FT = JsonConvert.DeserializeObject<FullTime>(Model.EmployeeJson);
                    FT.Years = Convert.ToInt32(Model.Extend);
                    result = JsonConvert.SerializeObject(FT);
                    break;
                case "1":
                    PTime PT = new PTime();
                    PT=   JsonConvert.DeserializeObject<PTime>(Model.EmployeeJson);
                    PT.WorkDays = Convert.ToInt32(Model.Extend);
                    result = JsonConvert.SerializeObject( PT);
                    break;
                case "2":
                    Stagnation ST = new Stagnation();
                   ST= JsonConvert.DeserializeObject<Stagnation>(Model.EmployeeJson);
                    ST.SourceCompany = Model.Extend;
                    result =  JsonConvert.SerializeObject(ST);
                    break;
            }
            return result;
        }
                

        /// <summary>載入員工編輯畫面</summary>
        /// <returns></returns>
        /// 
        public ActionResult EditEmployeeView()
        {
            return PartialView("_EditEmployeePartialView");
        }


        /// <summary>員工編輯動作</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult EditEmpAction(WebApplication1.Models.EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            WebApi Api = new WebApi();
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Edit;
            Obj.Entity = GetEmpTypeToEntity(Model);
            Obj.Obj = GetEmpTypeToObject(Model);
            ApiOperation Oper = Api.Init(Obj);
          
            string temp = Api.ApiOperation(Oper);
            ApiOperation Result = JsonConvert.DeserializeObject<ApiOperation>(temp);
            Model.Emp = JsonConvert.DeserializeObject<Employee>(Result.obj.ToString());
            return Json(Model.Emp);
        }

        /// <summary>員工刪除操作</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult DeleteEmpAction(WebApplication1.Models.EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            Employee Emp = new Employee();
            Emp.EmpNo = Model.EmployeeJson;
            Model.Emp = Emp;
            EmpMethod(Model, OperationMethod.Method.Delete);
            return Json(Model.Emp);
        }


        /// <summary>傳送API資料設訂</summary>
        /// <param name="Model"></param>
        /// <param name="Method"></param>
        private void EmpMethod(EmployeeMgrViewModels.EmployeeManamgerViewModel Model, OperationMethod.Method Method)
        {
            WebApi Api = new WebApi();
            Operation Obj = new Operation();
            Obj.Method = Method;
            Obj.Entity = GetDeleteEntity(Model);
            Obj.Obj = Model.Emp;
            ApiOperation Oper = Api.Init(Obj);
           
            string temp = Api.ApiOperation(Oper);
            ApiOperation Result = JsonConvert.DeserializeObject<ApiOperation>(temp);
            Model.Emp = JsonConvert.DeserializeObject<Employee>(Result.obj.ToString());
        }


        /// <summary>取得要Delete的Class </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        private Entities.EntitiesList GetDeleteEntity(WebApplication1.Models.EmployeeMgrViewModels.EmployeeManamgerViewModel Model)
        {
            Entities.EntitiesList result = Entities.EntitiesList.Employee;
            switch (Model.EmployeeType)
            {
                case EmpTypes.FullTime:
                    result = Entities.EntitiesList.FullTime;
                    break;
                case EmpTypes.PTime:
                    result = Entities.EntitiesList.PTime;
                    break;
                case EmpTypes.Stagnation:
                    result = Entities.EntitiesList.Stagnation;
                    break;
            }
            return result;
        }



        /// <summary>載入部門新增畫面</summary>
        /// <returns></returns>
        /// 
        public ActionResult AddDepartmentView()
        {
            return PartialView("_AddDepartmentPartialView");
        }


        /// <summary>新增部門操作</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
         [HttpPost]
        public ActionResult AddDepAction(DepartmentViewModels.DepartmentViewModel Model)
        {
            WebApi Api = new WebApi();
            JsonObj Jsonobj = new JsonObj();
            Jsonobj.Method = OperMethods.OperMethod.AddDepartment;
            Jsonobj.Source = UseSource;
            Jsonobj.JsonString = JsonConvert.SerializeObject(Model.Dep);
            ApiOperation Oper = new ApiOperation();
            Oper.Methode = HttpMethod.Post;
            Oper.obj = Jsonobj;
            Oper.Uri = NowUrl;
            JsonObj Result = JsonConvert.DeserializeObject<JsonObj>(Api.ApiOperation(Oper));
            Model.Dep = JsonConvert.DeserializeObject<Department>(Result.JsonString);
            return Json(Model.Dep);
        }


        /// <summary>刪除前設定 </summary>
        /// <param name="Model"></param>
        /// <param name="Method"></param>
        /// <param name="Entity"></param>
         private void DepMethod(DepartmentViewModels.DepartmentViewModel Model , OperationMethod.Method Method,Entities.EntitiesList Entity)
         {
             WebApi Api = new WebApi();
             Operation Obj = new Operation();
             Obj.Method = Method;
             Obj.Entity = Entity;
             Obj.Obj = Model.Dep;
             ApiOperation Oper = Api.Init(Obj);
            
             string temp = Api.ApiOperation(Oper);
             ApiOperation Result = JsonConvert.DeserializeObject<ApiOperation>(temp);
             Model.Dep = JsonConvert.DeserializeObject<Department>(Result.obj.ToString());
         }



        /// <summary>載入部門編輯畫面</summary>
        /// <returns></returns>
        public ActionResult EditDepartmentView()
        {
            return PartialView("_EditDepartmentPartialView");
        }


        /// <summary>部門編輯動作</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult EditDepAction(DepartmentViewModels.DepartmentViewModel Model)
        {
            WebApi Api = new WebApi();
            JsonObj Jsonobj = new JsonObj();
            Jsonobj.Method = OperMethods.OperMethod.EditDepartment;
            Jsonobj.Source = UseSource;
            Jsonobj.JsonString = JsonConvert.SerializeObject(Model.Dep);
            ApiOperation Oper = new ApiOperation();
            Oper.Methode = HttpMethod.Post;
            Oper.obj = Jsonobj;
            Oper.Uri = NowUrl;
            JsonObj Result = JsonConvert.DeserializeObject<JsonObj>(Api.ApiOperation(Oper));
            return Json(Model.Dep);
        }


        /// <summary> 部門刪除動作</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
         [HttpPost]
        public ActionResult DeleteAction(DepartmentViewModels.DepartmentViewModel Model)
        {           
            WebApi Api = new WebApi();
            JsonObj Jsonobj = new JsonObj();
            Jsonobj.Method = OperMethods.OperMethod.DeleteDepartmentByDepNo;
            Jsonobj.Source = UseSource;
            Jsonobj.JsonString = Model.QueryString;
                //JsonConvert.SerializeObject(Model.Dep);
            ApiOperation Oper = new ApiOperation();
            Oper.Methode = HttpMethod.Post;
            Oper.obj = Jsonobj;
            Oper.Uri = NowUrl;
            JsonObj Result = JsonConvert.DeserializeObject<JsonObj>(Api.ApiOperation(Oper));
            Model.Dep = JsonConvert.DeserializeObject<Department>(Result.JsonString);

            // Dep.DepNo = Model.DepNo;
            //Model.Dep = Dep;
            //DepMethod(Model, OperationMethod.Method.Delete, Entities.EntitiesList.Department);
            return Json(Model.Dep);
        }
                        

        /// <summary> 查詢該公司的所有員工</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
         public ActionResult GetDepartmentEmnpsAction(DepartmentViewModels.DepartmentViewModel Model)
         {
             EmployeeMgrViewModels.EmployeeManamgerViewModel EmpModel = new EmployeeMgrViewModels.EmployeeManamgerViewModel();
             WebApi Api = new WebApi();
             JsonObj Json = new JsonObj();
             Json.Method = OperMethods.OperMethod.GetALLEmployeeByDepNo;
             Json.Source = UseSource;
             Json.JsonString = JsonConvert.SerializeObject(Model.Dep);
             ApiOperation Oper = new ApiOperation();
             Oper.Methode = HttpMethod.Post;
             Oper.obj = Json;
             Oper.Uri = NowUrl;
             JsonObj Result = JsonConvert.DeserializeObject<JsonObj>(Api.ApiOperation(Oper));
             EmpModel.EmpList =JsonConvert.DeserializeObject<List<Employee>>(Result.JsonString);

             //DepartmentViewModels.DepartmentViewModel Temp = LoadingDep();
          
             //EmpModel.EmpList = new List<Employee>();
             //if (Model.DepNo != null)
             //{
             //    var getresult = Temp.DepList.Where(e => e.DepNo.Contains(Model.DepNo)).ToList();
             //    EmpModel.EmpList = getresult[0].EmpList;
             if (Model.QueryString != null)
             {
                 EmpModel.EmpList = EmpModel.EmpList.Where(e => e.EmpNo.Contains(Model.QueryString) ||
                      e.EmpFirstName.Contains(Model.QueryString) ||
                       e.EmpLastName.Contains(Model.QueryString) ||
                       e.Tel.Contains(Model.QueryString)
                     ).ToList();
             }
             //}
             return PartialView("_EmployeePartialView", EmpModel);
         }




    }
}
