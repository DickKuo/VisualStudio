using Adviser.Helpers;
using System.Web.Mvc;
using ObjectBase;

namespace Adviser.Controllers
{
    public class AdviserController : BaseLoginController
    {
        public ActionResult AdviserEWallet()
        {
            Models.ViewModels.AdviserViewModels.EWalletViewModel ViewModel = new Models.ViewModels.AdviserViewModels.EWalletViewModel();
            int AdviserSN = LoginHelper.GetLoginInfo().Adviser.SN;
            AdviserEWalletDAO EWalletDB = new AdviserEWalletDAO();
            ViewModel.EWallte = EWalletDB.GetAdviserEWalletByAdviserSN(AdviserSN);
            return View(ViewModel);
        }
    }
}