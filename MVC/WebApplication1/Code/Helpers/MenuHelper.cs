using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using WebApplication1.Code.DAO;
using WebApplication1.Code.Helpers;
using WebApplication1.Models;
using Newtonsoft.Json;
using WebRoutingTest.Models.Code;
using WebApplication1.Models.Code;

namespace WebApplication1.Code.MenuHelper
{
    public  class MenuHelper
    {

        private static string ApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl"].ToString();    //Api 位置

        private class SessionNames
        {
            public const string SessionMenu = "SeesionMenu";

        }
        

        private class ApiSetting
        {
            //public const string Controller = "Menu";           
            //public const string Action = "MenusLoading";
            public const string Controller = "Factory";
            public const string Action = "Factory";   
        }

        

        /// <summary>取得該角色可顯示的功能列清單</summary>
        /// <returns></returns>
        public static MenuManagmentViewModels.MenuViewModel LoadingMenu()
        {
            LoginInfo Info = LoginHelper.GetLoginInfo();
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Select;
            Obj.Entity = Entities.EntitiesList.Menu;
            Obj.Obj=  JsonConvert.SerializeObject(Info);
            ApiOperation Oper = new ApiOperation();
            string _url = string.Format("{0}/{1}/{2}", ApiUrl, ApiSetting.Controller, ApiSetting.Action);
            Oper.Uri = _url;
            Oper.Methode = HttpMethod.Post;
            Oper.obj = Obj;           
            WebApi Api = new WebApi();
            Operation Result = JsonConvert.DeserializeObject<Operation>(Api.ApiOperation(Oper));
            MenuManagmentViewModels.MenuViewModel Model = new MenuManagmentViewModels.MenuViewModel();
            if (Result.Obj != null)
            {
                Model.MenuList = JsonConvert.DeserializeObject<List<Menu>>(Result.Obj.ToString());
            }
            Model.MenuList = Model.MenuList.OrderBy(x => x.MenuNo).ToList();
            return Model;
        }


        public static void SetMenuSession(object Obj)
        {
            HttpContext.Current.Session[SessionNames.SessionMenu] = Obj;
        }

        public static MenuManagmentViewModels.MenuViewModel GetSessionObj()
        {
            if (HttpContext.Current.Session[SessionNames.SessionMenu] != null)
            {
                return HttpContext.Current.Session[SessionNames.SessionMenu] as MenuManagmentViewModels.MenuViewModel;
            }
            else
            {
                return null;
            }
        }


    }
}