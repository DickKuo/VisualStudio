using ObjectBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Models.Code
{
    public class BaseCode
    {

        /// <summary>
        /// 取得ClientIP
        /// </summary>
        /// <returns></returns>
        public static string GetUserIP() {
            string VisitorsIPAddr = string.Empty;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null) {
                VisitorsIPAddr = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0) {
                VisitorsIPAddr = System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            return VisitorsIPAddr;
        }


        public enum MessageType : int
        {
            danger = 0,
            warning = 1,
            info = 2,
            success = 3,
        }

        public enum PageAction : int {
            None = 0,
            Add = 1,
            Edit = 2,
            Delete = 3,
            View =4 
        }

        /// <summary>審核列舉</summary>
        /// <returns></returns>
        public static List<SelectListItem> GetAuditItems(Customer _Customer) {
            List<SelectListItem> AuditItems = new List<SelectListItem>();
            AuditItems.Add(new SelectListItem { Text = Resources.Resource.AuditTpyes_NotYet, Value = Convert.ToInt32(AuditTypes.NotYet).ToString(), Selected = (_Customer.Audit == AuditTypes.NotYet) });
            AuditItems.Add(new SelectListItem { Text = Resources.Resource.AuditTpyes_OK, Value = Convert.ToInt32(AuditTypes.OK).ToString(), Selected = (_Customer.Audit == AuditTypes.OK) });
            AuditItems.Add(new SelectListItem { Text = Resources.Resource.AuditTpyes_Cancel, Value = Convert.ToInt32(AuditTypes.Cancel).ToString(), Selected = (_Customer.Audit == AuditTypes.Cancel) });
            AuditItems.Add(new SelectListItem { Text = Resources.Resource.AuditTpyes_NO, Value = Convert.ToInt32(AuditTypes.NO).ToString(), Selected = (_Customer.Audit == AuditTypes.NO) });
            return AuditItems;
        }//end  GetGenderItems   
       
        /// <summary>交易類型</summary>
        /// <returns></returns>
        public static List<SelectListItem> GetTradTypeItems(){
            List<SelectListItem> Items = new List<SelectListItem>();
            Items.Add(new SelectListItem { Text = Resources.Resource.TransTypes_Deposit, Value = "1" });
            Items.Add(new SelectListItem { Text = Resources.Resource.TransTypes_Withdrawal, Value ="2"});
            Items.Add(new SelectListItem { Text = Resources.Resource.TransTypes_Bonus, Value = "3" });
            Items.Add(new SelectListItem { Text = Resources.Resource.TransTypes_Dividend, Value = "4" });
            return Items;
        }//end  GetTradTypeItems  

    }
}