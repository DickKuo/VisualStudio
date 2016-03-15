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
    public class MenuController : BaseLoginController
    {
        private string ApiUrl =System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl"].ToString();    //Api 位置  


        private class ApiSetting {
            public const string Menu = "Menu";                      //Controller
            public const string MenusLoading = "MenusLoading";      //Method 

            public const string Controller = "Factory";
            public const string Action = "Factory";
        }


        public ActionResult Index()
        {
            return View();
        }

        
        /// <summary> 瀏覽列表 </summary>
        /// <returns></returns>
        public ActionResult ManagementPage()
        {           
            return View();
        }


        public ActionResult LoadingRoleView()
        {
            return View("_LoadRolePartialView");
        }


        public ActionResult EditPartialView(MenuManagmentViewModels.MenuViewModel Model)
        {
            return PartialView("_EditMenuPartialView");
        }

        public ActionResult ChildEditPartialView()
        {
            return PartialView("_ChildEditMenuPartialView");
        }


        /// <summary>功能列清單---上層節點</summary>
        /// <returns></returns>
        public ActionResult MenuList()
        {
            MenuManagmentViewModels.MenuViewModel Model = GetMenuList();

            var ParentNodes = from v in Model.MenuList
                              where v.ParentNo == string.Empty
                              select v;
            Model.MenuList = ParentNodes.ToList();
            return PartialView("_ManagementPagePartialView", Model);
        }


        /// <summary>功能列清單---下層節點</summary>
        /// <returns></returns>
        public ActionResult ChildMenuList( MenuManagmentViewModels.MenuViewModel Model)
        {
            MenuManagmentViewModels.MenuViewModel Temp = GetMenuList();
                       

            var ChildNodes = from v in Temp.MenuList
                             where v.ParentNo == Model.MainMenu.MenuNo
                              select v;
            Model.MenuList = ChildNodes.ToList();
            return PartialView("_ChildMenuPartialView", Model);
        }



        /// <summary>取得所有Menu</summary>
        /// <returns></returns>
        private MenuManagmentViewModels.MenuViewModel GetMenuList()
        {
            MenuManagmentViewModels.MenuViewModel Model = new MenuManagmentViewModels.MenuViewModel();
            MenuApi Api = new MenuApi();
            Api.SetApiUrl(ApiUrl);
            ApiOperation Operation = new ApiOperation();
            Operation.Uri = ApiUrl;
            Operation.Action = Api.View;
            Operation.Parameters = new List<string>();
            Operation.Methode = HttpMethod.Get;
            Model = Api.ViewBrowse();
            Model.MenuList = Model.MenuList.OrderBy(X => X.MenuNo).ToList();
            return Model;
        }


        /// <summary>編輯的Action</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult AddMethod(MenuManagmentViewModels.MenuViewModel Model)
        {
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Add;
            Obj.Entity = Entities.EntitiesList.Menu;
            Obj.Obj = Model.JsonString;
            ApiOperation Oper = Init(Obj);
            WebApi Api = new WebApi();
            string temp = Api.ApiOperation(Oper);
            ApiOperation Result = JsonConvert.DeserializeObject<ApiOperation>(temp);
            Menu _Menu = JsonConvert.DeserializeObject<Menu>(Result.obj.ToString());
            Model.MainMenu = _Menu;
            return PartialView("_EditMenuPartialView", Model);
        }


        /// <summary>初始化API</summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        private ApiOperation Init(Operation Obj)
        {
            ApiOperation Oper = new ApiOperation();
            string _url = string.Format("{0}/{1}/{2}", ApiUrl, ApiSetting.Controller, ApiSetting.Action);
            Oper.Uri = _url;
            Oper.Methode = HttpMethod.Post;
            Oper.obj = Obj;
            return Oper;
        }


        /// <summary>編輯的Action</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult EditMothd(MenuManagmentViewModels.MenuViewModel Model)
        {
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Edit;
            Obj.Entity = Entities.EntitiesList.Menu;
            Obj.Obj = Model.JsonString;
            Menu Menu = JsonConvert.DeserializeObject<Menu>(Model.JsonString);
            Model.MainMenu = Menu;
            ApiOperation Oper = Init(Obj);
            WebApi Api = new WebApi();
            string temp = Api.ApiOperation(Oper);
            return Json(Model.MainMenu);
            //return PartialView("_EditMenuPartialView");
        }

          
        /// <summary>刪除功能清單</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult DeleteMothd(MenuManagmentViewModels.MenuViewModel Model)
        {
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Delete;
            Obj.Entity = Entities.EntitiesList.Menu;
            Obj.Obj = Model.JsonString;
            ApiOperation Oper = Init(Obj);
            WebApi Api = new WebApi();
            string temp = Api.ApiOperation(Oper);
            return PartialView("_EditMenuPartialView");
        }


        /// <summary>撈取角色清單</summary>
        /// <returns></returns>
        public ActionResult LoadingRoles()
        {
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Select;
            Obj.Entity = Entities.EntitiesList.Role;
            ApiOperation Oper = Init(Obj);
            WebApi Api = new WebApi();
            string temp = Api.ApiOperation(Oper);
            Operation Result = JsonConvert.DeserializeObject<Operation>(temp);
            List<Role> RoleList = JsonConvert.DeserializeObject<List<Role>>(Result.Obj.ToString());
            WebApplication1.Models.RoleViewModels.RoleViewModel Model = new RoleViewModels.RoleViewModel();
            Model.RoleList = RoleList;
            return PartialView("_RolesPartialView", Model);
        }


        /// <summary>角色修改畫面</summary>
        /// <returns></returns>
        public ActionResult EditRolePartialView()
        {
            return PartialView("_EditRolePartialView");
        }


        /// <summary>新增角色動作</summary>
        /// <returns></returns>
        public ActionResult AddRoleAction(WebApplication1.Models.RoleViewModels.RoleViewModel Model)
        {
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Add;
            Obj.Entity = Entities.EntitiesList.Role;
            Obj.Obj = Model.Role;
            ApiOperation Oper = Init(Obj);
            WebApi Api = new WebApi();
            string ReslutString = Api.ApiOperation(Oper);
            Operation Result = JsonConvert.DeserializeObject<Operation>(ReslutString);
            if (Result.Obj != null)
            {
                Role _Role = JsonConvert.DeserializeObject<Role>(Result.Obj.ToString());
            }           
            return PartialView("_EditRolePartialView");
        }


        /// <summary>編輯角色動作</summary>
        /// <returns></returns>
        public ActionResult EditRoleAction(WebApplication1.Models.RoleViewModels.RoleViewModel Model)
        {
            WebApi Api = new WebApi();
            Operation Obj = new Operation();
            if (Model.JsonString != null)
            {
                string[] sp = Model.JsonString.Split(',');
                if (sp.Length > 0)
                {
                    foreach (string str in sp)
                    {
                        RolesMenu Roles = new RolesMenu();
                        if (!string.IsNullOrEmpty(str))
                        {
                            Roles.MenuNo = str;
                            Model.Role.RoleMenuList.Add(Roles);
                        }
                    }
                }
            }
            Obj.Method = OperationMethod.Method.Edit;
            Obj.Entity = Entities.EntitiesList.Role;
            Obj.Obj = Model.Role;
            ApiOperation Oper = Init(Obj);
            string ReslutString = Api.ApiOperation(Oper);
            Operation Result = JsonConvert.DeserializeObject<Operation>(ReslutString);          

            return PartialView("_EditRolePartialView");
        }


        /// <summary>刪除角色動作</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult DeleteAction(WebApplication1.Models.RoleViewModels.RoleViewModel Model)
        {
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Delete;
            Obj.Entity = Entities.EntitiesList.Role;
            Obj.Obj = Model.Role;
            ApiOperation Oper = Init(Obj);
            WebApi Api = new WebApi();
            string temp = Api.ApiOperation(Oper);
            Operation Result = JsonConvert.DeserializeObject<Operation>(temp);
            return PartialView("_EditRolePartialView");
        }


        /// <summary>展開編擊畫面，功能列表</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult MenuListMatchRolesMenu(WebApplication1.Models.RoleViewModels.RoleViewModel Model)
        {            
            MenuManagmentViewModels.MenuViewModel ViewModel = GetMenuList();
            foreach (Menu ItemMenu in ViewModel.MenuList)
            {
                ItemMenu.IsEnable = false;
            }

            if (Model.Role != null)
            {
                #region 抓取Role
                Operation Obj = new Operation();
                Obj.Method = OperationMethod.Method.Select;
                Obj.Entity = Entities.EntitiesList.Role;
                Obj.Obj = Model.Role;
                ApiOperation Oper = Init(Obj);
                WebApi Api = new WebApi();
                string ResultString = Api.ApiOperation(Oper);
                Operation Result = JsonConvert.DeserializeObject<Operation>(ResultString);
                List<Role> RoleList = JsonConvert.DeserializeObject<List<Role>>(Result.Obj.ToString());
                #endregion

                #region 抓取MenuNo
                Obj.Method = OperationMethod.Method.Select;
                Obj.Entity = Entities.EntitiesList.RolesMenu;
                if (RoleList.Count > 0)
                {
                    Obj.Obj = RoleList[0];
                    Oper = Init(Obj);
                    ResultString = Api.ApiOperation(Oper);
                    Result = JsonConvert.DeserializeObject<Operation>(ResultString);
                    Role _Role = JsonConvert.DeserializeObject<Role>(Result.Obj.ToString());

                    foreach (RolesMenu Item in _Role.RoleMenuList)
                    {
                        var MenuArray = from v in ViewModel.MenuList
                                        where v.MenuNo == Item.MenuNo
                                        select v;
                        foreach (Menu ArraryItem in MenuArray)
                        {
                            ArraryItem.IsEnable = true;
                        }
                    }
                }
                #endregion
            }
            return PartialView("_RolesMenuPartialView", ViewModel);
        }


    }
}
