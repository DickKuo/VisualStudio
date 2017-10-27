using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class MessageNotLoginController : Controller
    {
        //
        // GET: /MessageNotLogin/
        public ActionResult Index()
        {
            return View();
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


        public ActionResult Messages(string MessageStr, string ReturnURL, WebApplication1.Models.Code.BaseCode.MessageType MessageType) {
            ViewBag.MessageStr = MessageStr;
            ViewBag.ReturnURL = ReturnURL;
            ViewBag.MessageType = MessageType;
            return View();
        }
	}
}