using Adviser.Helpers;
using ObjectBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adviser.Controllers
{
    public class AdviserController : BaseLoginController
    {
        //
        // GET: /Adviser/
        public ActionResult AdviserEWallet()
        {
            Adviser.Models.ViewModels.AdviserViewModels.EWalletViewModel ViewModel = new Models.ViewModels.AdviserViewModels.EWalletViewModel();
            int AdviserSN = LoginHelper.GetLoginInfo().Adviser.SN;
            AdviserEWalletDAO EWalletDB = new AdviserEWalletDAO();
            ViewModel.EWallte = EWalletDB.GetAdviserEWalletByAdviserSN(AdviserSN);
            return View(ViewModel);
        }

      
	}
}