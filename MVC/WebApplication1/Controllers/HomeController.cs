using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using WebApplication1.Code;
using WebApplication1.Code.DAO;
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

        /// <summary>訊息</summary>
        /// <param name="info"></param>
        /// <param name="returnUrl"></param>
        /// <param name="coworker"></param>
        /// <returns></returns>
        public ActionResult Message(string info, string returnUrl, string coworker) {
            ViewBag.info = info;
            ViewBag.ReturnURL = returnUrl;
            TempData["coworker"] = coworker;
            return View();
        }


        public ActionResult Messages(string MessageStr, string ReturnURL, WebApplication1.Models.Code.BaseCode.MessageType MessageType)
        {
            ViewBag.MessageStr = MessageStr;
            ViewBag.ReturnURL = ReturnURL;
            ViewBag.MessageType = MessageType;
            return View();
        }
    }
}