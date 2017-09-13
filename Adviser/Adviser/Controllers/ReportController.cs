using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adviser.Controllers
{
    public class ReportController : BaseLoginController
    {
        
        public ActionResult TransRecord()
        {
            return View();
        }
	}
}