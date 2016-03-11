using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Code.DAO
{
    public class MenuApi:WebApi
    {
        public MenuApi()
        {
            this.ModelName = "Menu";
        }
        
        
        public string View = "ViewAll";

        public string Delete = "DeleteMenu";

        public string Add = "AddMenu";

        public string Eidt = "EditMenu";



        /// <summary>新增Menu資訊</summary>
       /// <param name="MainMenu"></param>
       /// <returns></returns>
        public Menu AddMenu(Menu MainMenu)
        {
            string Str = string.Format("{0}/{1}/{2}", this._uri, this.ModelName, Add);
            ApiOperation Operation = new ApiOperation();
            Operation.Uri = Str;
            Operation.obj = MainMenu;
            string temp = base.ApiOperation(Operation);
            Menu resultemp = JsonConvert.DeserializeObject<Menu>(temp);
            return resultemp;
        }



        /// <summary>撈取主選單</summary>
        /// <param name="Operation"></param>
        /// <returns></returns>
        public MenuManagmentViewModels.MenuViewModel MenusLoading(ApiOperation Operation)
        {
            string temp = ApiOperation(Operation);
            WebApplication1.Models.MenuManagmentViewModels.MenuViewModel Model = JsonConvert.DeserializeObject<WebApplication1.Models.MenuManagmentViewModels.MenuViewModel>(temp);
            return Model;
        }


        /// <summary> 主功能單列表</summary>
        /// <returns></returns>
        public MenuManagmentViewModels.MenuViewModel ViewBrowse()
        {
            string Str = string.Format("{0}/{1}/{2}", this._uri, this.ModelName, View);
            ApiOperation Operation = new ApiOperation();
            Operation.Uri = Str;            
            string temp = base.ApiOperation(Operation);
            MenuManagmentViewModels.MenuViewModel Model = JsonConvert.DeserializeObject<WebApplication1.Models.MenuManagmentViewModels.MenuViewModel>(temp);
            return Model;
        }

    }
}