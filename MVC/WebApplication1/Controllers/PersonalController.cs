using ObjectBase;
using System.Web.Mvc;
using WebApplication1.Code.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PersonalController : BaseLoginController
    {
        public ActionResult ChangePassWord()
        {          
            return View();
        }//end ChangePassWord

        public ActionResult ActionChangePassWord(WebApplication1.Models.PersonalViewModels.ForGetPassWordViewModel ViewModel) {
            LoginInfo Info = LoginHelper.GetLoginInfo();
            Info.Customer.PassWord = ViewModel.NowPassWord;
            CustomerDAO _CustomerDAO = new CustomerDAO();
            if (_CustomerDAO.UpdatePassWord(Info.Customer, ViewModel.NewPassWord)==1) {
                return RedirectToAction("LogOut", "Login");
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        }//end ActionChangePassWord


        public ActionResult Personal() {
            WebApplication1.Models.PersonalViewModels.PersonalViewModel ViewModel = new Models.PersonalViewModels.PersonalViewModel();
            LoginInfo Info = LoginHelper.GetLoginInfo();
            ViewModel.FirstName = Info.Customer.Member.FirstName;
            ViewModel.LastName = Info.Customer.Member.LastName;
            ViewModel.Email = Info.Customer.Member.Email;
            ViewModel.HomeAddress = Info.Customer.Member.HomeAddr;
            ViewModel.NickName = Info.Customer.Member.NickName;
            ViewModel.Phone = Info.Customer.Member.Phone;
            return View(ViewModel);
        }//end Personal

        /// <summary></summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public ActionResult PersonalChange(WebApplication1.Models.PersonalViewModels.PersonalViewModel ViewModel) {
            LoginInfo Info = LoginHelper.GetLoginInfo();
            Info.Customer.Member.FirstName = ViewModel.FirstName;
            Info.Customer.Member.LastName = ViewModel.LastName;
            Info.Customer.Member.HomeAddr = ViewModel.HomeAddress;
            Info.Customer.Member.NickName = ViewModel.NickName;
            Info.Customer.Member.Phone = ViewModel.Phone;            
            MemberDAO _MemberDAO = new MemberDAO();
            if( _MemberDAO.UpdateMember(Info.Customer.Member)>0)
            {
                LoginHelper.SetSeesion(Info);
            }            
            return RedirectToAction("Personal", "Personal");
        }//end PersonalChange

	}
}