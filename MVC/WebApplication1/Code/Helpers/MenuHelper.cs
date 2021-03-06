﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using WebApplication1.Code.DAO;
using WebApplication1.Code.Helpers;
using WebApplication1.Models;
using Newtonsoft.Json;
using WebRoutingTest.Models.Code;
using WebApplication1.Models.Code;

namespace WebApplication1.Code.MenuHelper
{
    public class MenuHelper : BaseModel
    {
        private static string ApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl"].ToString();    //Api 位置

        private class SessionNames
        {
            public const string SessionMenu = "SeesionMenu";
        }        

        private class ApiSetting
        {
            public const string Controller = "Factory";
            public const string Action = "Factory";   
        }

        private class Default {
            public const string MenuNo = "MenuNo";
            public const string Url = "Url";
            public const string Permission = "Permission";
        }

        /// <summary>取得該角色可顯示的功能列清單</summary>
        /// <returns></returns>
        public static MenuManagmentViewModels.MenuViewModel LoadingMenu()
        {
            LoginInfo Info = LoginHelper.GetLoginInfo();
            MenuManagmentViewModels.MenuViewModel Model = new MenuManagmentViewModels.MenuViewModel();
            Model.MenuList = new List<Menu>();
            System.Xml.XmlDocument XDoc = new System.Xml.XmlDocument();
            XDoc.Load(System.Web.HttpContext.Current.Server.MapPath(BaseModel.App_Data) + "\\bar.xml");
            System.Xml.XmlNodeList XNodeList = XDoc.GetElementsByTagName(BaseModel.Item);
            if (XNodeList != null) {
                foreach (System.Xml.XmlNode Item in XNodeList) {
                    Menu _Menu = new Menu();
                    _Menu.Seq = Convert.ToInt32(Item.Attributes[BaseModel.Seq].Value);
                    _Menu.MenuNo = Item.Attributes[Default.MenuNo].Value;
                    _Menu.Name =  Resources.Menu.ResourceManager.GetString("Menu_" + _Menu.MenuNo, System.Threading.Thread.CurrentThread.CurrentUICulture);
                    _Menu.Permission = Convert.ToInt32(Item.Attributes[Default.Permission].Value);
                    _Menu.Url = Item.Attributes[Default.Url].Value;
                    _Menu.MenuList = new List<Menu>();
                    foreach (System.Xml.XmlNode SubItem in Item.ChildNodes) {
                        Menu _SubMenu = new Menu();
                        _SubMenu.Seq = Convert.ToInt32(SubItem.Attributes[BaseModel.Seq].Value);
                        _SubMenu.Name = SubItem.Attributes[BaseModel.Name].Value;
                        _SubMenu.MenuNo = SubItem.Attributes[Default.MenuNo].Value;
                        _SubMenu.Permission = Convert.ToInt32(SubItem.Attributes[Default.Permission].Value);
                        _SubMenu.Url = SubItem.Attributes[Default.Url].Value;
                        if (_SubMenu.Permission >= Info.Customer.Role.Permission) {
                            _Menu.MenuList.Add(_SubMenu);
                        }
                    }
                    if (_Menu.Permission >= Info.Customer.Role.Permission) {
                        Model.MenuList.Add(_Menu);
                    }
                }
            } 
            Model.MenuList = Model.MenuList.OrderBy(x => x.Seq).ToList();
            Info.MenuList = Model.MenuList;
            return Model;
        }

        public static void SetMenuSession(object Obj)
        {
            HttpContext.Current.Session[SessionNames.SessionMenu] = Obj;
        }

        public static MenuManagmentViewModels.MenuViewModel GetSessionObj()
        {
            if (HttpContext.Current.Session[SessionNames.SessionMenu] != null)
            {
                return HttpContext.Current.Session[SessionNames.SessionMenu] as MenuManagmentViewModels.MenuViewModel;
            }
            else
            {
                return null;
            }
        }

    }
}