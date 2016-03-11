using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Code.DAO;
using WebApplication1.Code.MenuHelper;

namespace WebApplication1.Models.Code
{
    public class MyDynamicNodeProvider:DynamicNodeProviderBase
    {

        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var ReturnValue = new List<DynamicNode>();

            MenuManagmentViewModels.MenuViewModel Model =    MenuHelper.LoadingMenu();
            foreach (Menu menu in Model.MenuList)
            {
                DynamicNode DyNode = new DynamicNode();
                if (menu.MenuList != null)
                {
                    DyNode.Title = menu.Name;
                    DyNode.ParentKey = "";
                    DyNode.Key = menu.MenuNo;
                    DyNode.Url = menu.Url;
                    ReturnValue.Add(DyNode);
                }
                else
                {
                    foreach (Menu ChildMenu in menu.MenuList)
                   {
                       DyNode.Title = menu.Name;
                       DyNode.ParentKey = ChildMenu.ParentNo;
                       DyNode.Key = menu.MenuNo;
                       DyNode.Url = menu.Url;
                       ReturnValue.Add(DyNode);
                   }
                }
            }
            return ReturnValue;
        }
    }
}