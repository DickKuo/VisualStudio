using Adviser.Helpers;
using ObjectBase;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adviser.Controllers
{
    public class BaseLoginController : Controller
    {
        /// <summary>
        /// 執行Action 之前會做的事情
        /// 驗證登入狀態
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (!LoginHelper.CheckIsLogin()) {
                filterContext.Result = RedirectToAction("LoginView", "Login");
            }
            else {
                MenuHelper.LoadingMenu();
            }
            base.OnActionExecuting(filterContext);
        } //end OnActionExecuting

        /// <summary>審核列舉</summary>
        /// <returns></returns>
        public List<SelectListItem> GetAuditItems(Customer _Customer) {
            List<SelectListItem> AuditItems = new List<SelectListItem>();
            AuditItems.Add(new SelectListItem { Text = Resources.ResourceCustomer.AuditTpyes_NotYet, Value = Convert.ToInt32(AuditTypes.NotYet).ToString(), Selected = (_Customer.Audit == AuditTypes.NotYet) });
            AuditItems.Add(new SelectListItem { Text = Resources.ResourceCustomer.AuditTpyes_OK, Value = Convert.ToInt32(AuditTypes.OK).ToString(), Selected = (_Customer.Audit == AuditTypes.OK) });
            AuditItems.Add(new SelectListItem { Text = Resources.ResourceCustomer.AuditTpyes_Cancel, Value = Convert.ToInt32(AuditTypes.Cancel).ToString(), Selected = (_Customer.Audit == AuditTypes.Cancel) });
            AuditItems.Add(new SelectListItem { Text = Resources.ResourceCustomer.AuditTpyes_NO, Value = Convert.ToInt32(AuditTypes.NO).ToString(), Selected = (_Customer.Audit == AuditTypes.NO) });
            return AuditItems;
        }//end  GetGenderItems    

        /// <summary>審核列舉</summary>
        /// <returns></returns>
        public List<SelectListItem> GetAuditItemsNoSelected() {
            List<SelectListItem> AuditItems = new List<SelectListItem>();
            AuditItems.Add(new SelectListItem { Text = Resources.ResourceCustomer.AuditTpyes_NotYet, Value = Convert.ToInt32(AuditTypes.NotYet).ToString() });
            AuditItems.Add(new SelectListItem { Text = Resources.ResourceCustomer.AuditTpyes_OK, Value = Convert.ToInt32(AuditTypes.OK).ToString() });
            AuditItems.Add(new SelectListItem { Text = Resources.ResourceCustomer.AuditTpyes_Cancel, Value = Convert.ToInt32(AuditTypes.Cancel).ToString() });
            AuditItems.Add(new SelectListItem { Text = Resources.ResourceCustomer.AuditTpyes_NO, Value = Convert.ToInt32(AuditTypes.NO).ToString() });
            return AuditItems;
        }//end  GetAuditItemsNoSelected  

        /// <summary>性別列舉</summary>
        /// <returns></returns>
        public List<SelectListItem> GetGenderItems() {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = Resources.Resource.Gender_Male, Value = Convert.ToInt32(GenderType.Male).ToString(), Selected = true });
            items.Add(new SelectListItem { Text = Resources.Resource.Gender_Fmale, Value = Convert.ToInt32(GenderType.Fmale).ToString() });
            return items;
        }//end  GetGenderItems   
	}
}