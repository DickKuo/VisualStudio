using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class ForGetPassWordController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendGetPassWordEmail (){ 
        
            return RedirectToAction("Index","Home");
        }
	}
}