using ObjectBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

namespace WebApplication1.Controllers
{
    public class RegisterController : Controller
    {
        public const string App_Data = "~\\App_Data";

        /// <summary>註冊畫面</summary>
        /// <param name="RegisterView"></param>
        /// <returns></returns>
        public ActionResult Index(WebApplication1.Models.RegisterViewModels.RegisterViewModel RegisterView)
        {
            ViewBag.GenderList = GetGenderItems();
            ViewBag.Role = RoleList();            
            if (RegisterView.Account != null) {
                int Result = 0;
                CustomerDAO _CustomerDAO = new CustomerDAO();
                Customer _Customer = new Customer();
                _Customer.Account = RegisterView.Account;
                _Customer.PassWord = RegisterView.PassWord;
                _Customer.Member = new Member();
                _Customer.Member.BirthDay = RegisterView.BirthDay == DateTime.MinValue ? DateTime.Now : RegisterView.BirthDay;
                _Customer.Member.Email = RegisterView.Account;
                _Customer.Member.FirstName = RegisterView.FirstName == null ? "Default" : RegisterView.FirstName;
                _Customer.Member.LastName = RegisterView.LastName == null ? "Default" : RegisterView.LastName;
                _Customer.Member.NickName = RegisterView.NickName;
                _Customer.Member.Phone = RegisterView.Phone == null ? "Default" : RegisterView.Phone;
                _Customer.Member.ID = RegisterView.ID == null ? "Default" : RegisterView.ID;
                _Customer.Member.HomeAddr = RegisterView.HomeAddr == null ? "Default" : RegisterView.HomeAddr;
                _Customer.Member.Gender = RegisterView.Gender;              
                Result = _CustomerDAO.AddCustomer(_Customer);
                if (Result > 0) {
                    CommTool.MailData Data = new CommTool.MailData();
                    Data.RegistrySend(_Customer.Account,"註冊信件","感謝您的註冊"+
                        "<br/>您的帳號:" + _Customer.Account +
                        "<br/>您的密碼:" + _Customer.PassWord +
                        "<br/>請您以這組帳號登入系統");
                    return RedirectToAction("Messages", "MessageNotLogin", new {
                        MessageStr = Resources.Resource.Register_Success,
                        returnUrl = "../Login/LoginView",
                        MessageType= WebApplication1.Models.Code.BaseCode.MessageType.success
                    });
                }
                else {
                    string ErrorMessage =string.Empty;
                    switch (Result)
                    {
                        case -1:
                            ErrorMessage = Resources.Resource.Register_Error_ExsitAccount;
                            break;
                        default:
                            ErrorMessage =Resources.Resource.Register_Error;
                            break;
                    }
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                    return View(RegisterView);
                }
            }
            else {
                return View(RegisterView);
            }         
        }//end Index

        /// <summary>性別列舉</summary>
        /// <returns></returns>
        public List<SelectListItem> GetGenderItems() {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = Resources.Resource.Gender_Male, Value = Convert.ToInt32(GenderType.Male).ToString(), Selected = true });
            items.Add(new SelectListItem { Text = Resources.Resource.Gender_Fmale, Value = Convert.ToInt32(GenderType.Fmale).ToString() });
            return items;
        }//end  GetGenderItems           

        /// <summary>取得規則說明</summary>
        /// <returns></returns>
        public string RoleList() {
            StringBuilder SB = new StringBuilder();
            System.Xml.XmlDocument XDoc = new System.Xml.XmlDocument();
            XDoc.Load(System.Web.HttpContext.Current.Server.MapPath(App_Data) + "\\Roles.xml");
            System.Xml.XmlNodeList XNodeList = XDoc.GetElementsByTagName("Role");
            if (XNodeList != null) {
                foreach (System.Xml.XmlNode Item in XNodeList) {
                    SB.AppendFormat("<li style='padding：10px；'>{0} ", Item.InnerText);
                }
            }
            return SB.ToString();
        }
	}
}