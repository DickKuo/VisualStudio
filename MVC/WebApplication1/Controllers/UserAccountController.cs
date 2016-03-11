using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Code.DAO;
using WebApplication1.Models;
using WebApplication1.Models.Code;
using WebRoutingTest.Models.Code;

namespace WebApplication1.Controllers
{
    public class UserAccountController : BaseLoginController
    {
        //
        // GET: /UserAccount/
        public ActionResult Index(UserAccountViewModels.UserAccountViewModel Model)
        {
           
            return View();
        }


        public ActionResult LoadingAccountPartialView(UserAccountViewModels.UserAccountViewModel Model)
        {
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Select;
            Obj.Entity = Entities.EntitiesList.User;
            WebApi Api = new WebApi();
            ApiOperation Oper = Api.Init(Obj);
            string ReslutString = Api.ApiOperation(Oper);
            Operation Result = JsonConvert.DeserializeObject<Operation>(ReslutString);
            Model.UserList = JsonConvert.DeserializeObject<List<User>>(Result.Obj.ToString());
            GetRoleSelectItem(Model);           
            return PartialView("_AccountPartialView", Model);
        }


        /// <summary>載入編輯操作畫面</summary>
        /// <returns></returns>
        public ActionResult LoadingEditPartialView(UserAccountViewModels.UserAccountViewModel Model)
        {
            if (Model.JsonString != null)
            {
                Operation Obj = new Operation();
                Obj.Method = OperationMethod.Method.Select;
                Obj.Entity = Entities.EntitiesList.User;
                Obj.Obj = Model.JsonString;
                WebApi Api = new WebApi();
                ApiOperation Oper = Api.Init(Obj);
                string ReslutString = Api.ApiOperation(Oper);
                Operation Result = JsonConvert.DeserializeObject<Operation>(ReslutString);
                List<User> UserList = JsonConvert.DeserializeObject<List<User>>(Result.Obj.ToString());
                if (UserList.Count > 0)
                {
                    Model.User.RoleID = UserList[0].RoleID;
                }
            }

            GetRoleSelectItem(Model);
            return PartialView("_EditPartialView");
        }

        private void GetRoleSelectItem(UserAccountViewModels.UserAccountViewModel Model)
        {
            WebApi Api = new WebApi();
            Operation Obj = new Operation();
            Obj.Entity = Entities.EntitiesList.Role;
            ApiOperation Oper = Api.Init(Obj);
            string ReslutString = Api.ApiOperation(Oper);
            Operation Result = JsonConvert.DeserializeObject<Operation>(ReslutString);
            List<Role> RoleList = JsonConvert.DeserializeObject<List<Role>>(Result.Obj.ToString());
            List<SelectListItem> MySelectItemList = new List<SelectListItem>();
            foreach (Role Item in RoleList)
            {
                MySelectItemList.Add(new SelectListItem()
                {
                    Text = Item.Name,
                    Value = Item.RoleID
                });
            }
            if (Model.User != null)
            {
                if (!string.IsNullOrEmpty(Model.User.RoleID))
                {
                    var Arrary = from v in MySelectItemList
                                 where v.Value == Model.User.RoleID
                                 select v;
                    if (Arrary.ToList().Count > 0)
                    {
                        foreach (SelectListItem Item in Arrary.ToList())
                        {
                            Item.Selected = true;
                        }
                    }
                }
            }
            ViewBag.RoleList = MySelectItemList;
        }



        public ActionResult AddAction(UserAccountViewModels.UserAccountViewModel Model)
        {
            WebApi Api = new WebApi();
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Add;
            Obj.Entity = Entities.EntitiesList.User;
            Obj.Obj = Model.User;
            ApiOperation Oper = Api.Init(Obj);
            string ReslutString = Api.ApiOperation(Oper);
            Operation Result = JsonConvert.DeserializeObject<Operation>(ReslutString);
            User User = JsonConvert.DeserializeObject<User>(Result.Obj.ToString());
            return Json(User);
        }
        


        public ActionResult EditAction(UserAccountViewModels.UserAccountViewModel Model)
        {
            WebApi Api = new WebApi();
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Edit;
            Obj.Entity = Entities.EntitiesList.User;
            Obj.Obj = Model.User;
            ApiOperation Oper = Api.Init(Obj);
            string ReslutString = Api.ApiOperation(Oper);
            Operation Result = JsonConvert.DeserializeObject<Operation>(ReslutString);
            User User = JsonConvert.DeserializeObject<User>(Result.Obj.ToString());
            return Json(User);                
        }



        public ActionResult DeleteMothd(UserAccountViewModels.UserAccountViewModel Model)
        {
            User User = JsonConvert.DeserializeObject<User>(Model.JsonString);
            WebApi Api = new WebApi();
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Delete;
            Obj.Entity = Entities.EntitiesList.User;
            Obj.Obj = User;
            ApiOperation Oper = Api.Init(Obj);
            string ReslutString = Api.ApiOperation(Oper);
            Operation Result = JsonConvert.DeserializeObject<Operation>(ReslutString);
         
            return Json(User);
        }

    }
}
