using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Code.DAO;
using WebRoutingTest.Models.Code;

namespace WebRoutingTest.Controllers
{
    public class MenuController : ApiController
    {
        private string ConnectionString =System.Web.Configuration.WebConfigurationManager.AppSettings["ConnectionString"].ToString();

        [Route("api/Menu/MenusLoading")]
        /// <summary>載入功能選單  </summary>
        /// <param name="Info"></param>
        /// <returns></returns>
        [HttpPost]
        public string MenusLoading(Operation Obj)
        {
            MenuDAO DAO = new MenuDAO(ConnectionString);
            WebApplication1.Models.MenuManagmentViewModels.MenuViewModel Model = new WebApplication1.Models.MenuManagmentViewModels.MenuViewModel();
            Model.MenuList= DAO.GetMenuViewModel();
            return JsonConvert.SerializeObject(Model);
        }//end SaveMoney


        /// <summary>新增功能 </summary>
        /// <param name="Main"></param>
        /// <returns></returns>
        [Route("api/Menu/AddMenu")]
        [HttpPost]
        public string AddMenu(Menu Main)
        {
            MenuDAO DAO = new MenuDAO(ConnectionString);
            Menu Menu=  DAO.AddMenu(Main);
            return JsonConvert.SerializeObject(Menu);
        }


        /// <summary>瀏覽Menu</summary>
        /// <returns></returns>
        public string ViewAll()
        {
            MenuDAO DAO = new MenuDAO(ConnectionString);
            WebApplication1.Models.MenuManagmentViewModels.MenuViewModel Model = new WebApplication1.Models.MenuManagmentViewModels.MenuViewModel();
              Model.MenuList=  DAO.GetBrowseView();
            return JsonConvert.SerializeObject(Model);
        }

    }
}
