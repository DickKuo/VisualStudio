using System.Text.RegularExpressions;
using System.Collections.Generic;
using Adviser.Helpers;
using System.Web.Mvc;
using ObjectBase;
using System;

namespace Adviser.Controllers
{
    public class BaseLoginController : Controller
    {
        protected const string TimeFormat = "yyyy-MM-dd"; 

        /// <summary>
        /// 執行Action 之前會做的事情
        /// 驗證登入狀態
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!LoginHelper.CheckIsLogin())
            {
                filterContext.Result = RedirectToAction("LoginView", "Login");
            }
            else
            {
                MenuHelper.LoadingMenu();
            }
            base.OnActionExecuting(filterContext);
        }

        /// <summary>審核列舉</summary>
        /// <returns></returns>
        public List<SelectListItem> GetAuditItems(Customer _Customer)
        {
            List<SelectListItem> AuditItems = new List<SelectListItem>();
            AuditItems.Add(new SelectListItem { Text = Resources.ResourceCustomer.AuditTpyes_NotYet, Value = Convert.ToInt32(AuditTypes.NotYet).ToString(), Selected = (_Customer.Audit == AuditTypes.NotYet) });
            AuditItems.Add(new SelectListItem { Text = Resources.ResourceCustomer.AuditTpyes_OK, Value = Convert.ToInt32(AuditTypes.OK).ToString(), Selected = (_Customer.Audit == AuditTypes.OK) });
            AuditItems.Add(new SelectListItem { Text = Resources.ResourceCustomer.AuditTpyes_Cancel, Value = Convert.ToInt32(AuditTypes.Cancel).ToString(), Selected = (_Customer.Audit == AuditTypes.Cancel) });
            AuditItems.Add(new SelectListItem { Text = Resources.ResourceCustomer.AuditTpyes_NO, Value = Convert.ToInt32(AuditTypes.NO).ToString(), Selected = (_Customer.Audit == AuditTypes.NO) });
            return AuditItems;
        }

        /// <summary>審核列舉</summary>
        /// <returns></returns>
        public List<SelectListItem> GetAuditItemsNoSelected()
        {
            List<SelectListItem> AuditItems = new List<SelectListItem>();
            AuditItems.Add(new SelectListItem { Text = Resources.ResourceCustomer.AuditTpyes_NotYet, Value = Convert.ToInt32(AuditTypes.NotYet).ToString() });
            AuditItems.Add(new SelectListItem { Text = Resources.ResourceCustomer.AuditTpyes_OK, Value = Convert.ToInt32(AuditTypes.OK).ToString() });
            AuditItems.Add(new SelectListItem { Text = Resources.ResourceCustomer.AuditTpyes_Cancel, Value = Convert.ToInt32(AuditTypes.Cancel).ToString() });
            AuditItems.Add(new SelectListItem { Text = Resources.ResourceCustomer.AuditTpyes_NO, Value = Convert.ToInt32(AuditTypes.NO).ToString() });
            return AuditItems;
        }

        /// <summary>性別列舉</summary>
        /// <returns></returns>
        public List<SelectListItem> GetGenderItems()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = Resources.Resource.Gender_Male, Value = Convert.ToInt32(GenderType.Male).ToString(), Selected = true });
            items.Add(new SelectListItem { Text = Resources.Resource.Gender_Fmale, Value = Convert.ToInt32(GenderType.Fmale).ToString() });
            return items;
        }

        /// <summary>顧問列舉</summary>
        /// <returns></returns>
        public List<SelectListItem> GetAdviserItems(int AdviserSN)
        {
            AdviserDAO AdviserDB = new AdviserDAO();
            List<SelectListItem> items = new List<SelectListItem>();
            List<ObjectBase.Adviser> ListAdviser = AdviserDB.GetListAdviser();
            foreach (var Item in ListAdviser)
            {
                if (Item.SN == AdviserSN)
                {
                    items.Add(new SelectListItem { Text = Item.LastName + Item.FirstName, Value = Item.SN.ToString(), Selected = true });
                }
                else
                {
                    items.Add(new SelectListItem { Text = Item.LastName + Item.FirstName, Value = Item.SN.ToString() });
                }
            }
            return items;
        }

        #region 替換原始ViewResult，偵測是否有Mobile的View
        public ViewResult Views()
        {
            string NewViewName = CheckViewIsExist(string.Empty);
            return View(NewViewName);
        }

        public ViewResult Views(IView view)
        {
            return View();
        }

        public ViewResult Views(object model)
        {
            string NewViewName = CheckViewIsExist(string.Empty);
            return View(NewViewName, model);
        }

        public ViewResult Views(string OriViewName)
        {
            string ViewName = CheckViewIsExist(OriViewName);
            return View(ViewName);
        }

        public ViewResult Views(string OriViewName, object model)
        {
            string ViewName = CheckViewIsExist(OriViewName);
            return View(ViewName, model);
        }

        public ViewResult Views(string OriViewName, string OriMasterName)
        {
            string ViewName = CheckViewIsExist(OriViewName);
            string MasterViewName = CheckViewIsExist(OriMasterName);
            return View(ViewName, MasterViewName);
        }

        public enum MessageTypeCode : int
        {
            info = 0,
            danger = 1,
        };

        /// <summary>
        /// 針對View 去做檢查，順便針對裝置偵測做顯示切換
        /// </summary>
        /// <param name="CurrentViewName"></param>
        /// <returns></returns>
        public string CheckViewIsExist(string CurrentViewName)
        {
            /* 檢查裝置來源是否為手持裝置 */
            string OrginalViewName = string.Empty;

            if (string.IsNullOrEmpty(CurrentViewName))
            {
                if (ControllerContext.RouteData != null)
                {
                    if (ControllerContext.RouteData.Values["action"] != null)
                    {
                        OrginalViewName = ControllerContext.RouteData.Values["action"].ToString();
                    }
                }
            }
            else
            {
                OrginalViewName = CurrentViewName.Trim();
            }

            if (ChkIsMobile())
            {
                string MobileViewName2 = OrginalViewName + "M";
                ViewEngineResult result2 = ViewEngines.Engines.FindView(ControllerContext, MobileViewName2, null);
                if (result2.View != null)
                {
                    return MobileViewName2;
                }
            }
            return OrginalViewName;
        }

        /// <summary>檢測是否為手持裝置</summary>
        /// <returns></returns>
        public bool ChkIsMobile()
        {
            bool RetVal = false;
            string UserAgent = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (UserAgent.Length >= 4)
            {
                if (b.IsMatch(UserAgent) || v.IsMatch(UserAgent.Substring(0, 4)))
                {
                    RetVal = true;
                }
            }
            return RetVal;
        }
        #endregion
    }
}