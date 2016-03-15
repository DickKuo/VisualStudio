using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Code;
using WebApplication1.Code.DAO;
using WebApplication1.Code.Helpers;
using WebApplication1.Code.MenuHelper;
using WebApplication1.Models;
using WebApplication1.Models.Code;
using WebRoutingTest.Models.Code;

namespace WebApplication1.Controllers
{
    public class HomeController : BaseLoginController
    {
        private string Url = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl"].ToString();
        private const string ApiController = "Carousel";
        private const string ApiAction = "Uploading";
        
        /// <summary>
        /// 載入首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //讀取功能選單列表
            MenuManagmentViewModels.MenuViewModel Model = MenuHelper.LoadingMenu();
            MenuHelper.SetMenuSession(Model);

            return View();
        }
         

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";          
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }


        private ApiOperation Init(Operation Obj)
        {
            ApiOperation Oper = new ApiOperation();
            string _url = string.Format("{0}/{1}/{2}", Url, ApiController, ApiAction);
            Oper.Uri = _url;
            Oper.Methode = HttpMethod.Post;
            Oper.obj = Obj;
            return Oper;
        }



        public ActionResult LoadingCarousel()
        {
            Operation Obj = new Operation();
            Obj.Method = OperationMethod.Method.Select;
            Obj.Entity = Entities.EntitiesList.Photo;
            ApiOperation Oper = Init(Obj);
            WebApi Api = new WebApi();
            Operation Result = JsonConvert.DeserializeObject<Operation>(Api.ApiOperation(Oper));

            PhotoViewModels.PhotoViewModel Model = new PhotoViewModels.PhotoViewModel();
            Model.PhotoList = new List<Photo>();
            foreach (var Item in Result.ObjList)
            {
                Photo photo = JsonConvert.DeserializeObject<Photo>(Item.ToString());

                Model.PhotoList.Add(photo);
            }
            return PartialView("_CarouselPartialView", Model);
        }


       

    }
}