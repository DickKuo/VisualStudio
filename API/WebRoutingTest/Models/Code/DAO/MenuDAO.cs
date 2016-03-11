using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using WebApplication1.Code.DAO;
using WebApplication1.Models;
using WebApplication1.Models.Code;
using WebRoutingTest.Models.Code.DAO;

namespace WebRoutingTest.Models.Code
{
    public class MenuDAO : DAOBase
    {

        private class SpName
        {
            public const string SpLoadingMenu = "SpLoadingMenu";            //預存_載入所有功能清單
            public const string SPAddMenu = "SPAddMenu";
            public const string SPEditMenu = "SPEditMenu";
            public const string SPDeleteMenu = "SPDeleteMenu";
        }

        private class DataRowName 
        {
            public const string MenuNo = "MenuNo";
            public const string Url = "Url";
            public const string Name = "Name";          
            public const string ParentNo = "ParentNo";
            public const string IsEnable = "IsEnable";
            public const string MenuOrder = "MenuOrder";
            public const string IsDone = "IsDone";
        }

        public MenuDAO()
        {
            
        }

        public MenuDAO(string ConnectionString)
        {
            base._DbIstance.ConnectiinString = ConnectionString;
        }


        /// <summary>將DB資料轉成物件</summary>
        /// <returns></returns>
        public List<Menu> GetMenuViewModel()
        {
            List<Menu> MenuList = new List<Menu>();
            Dictionary<string, Menu> DicMenu = new Dictionary<string, Menu>();
            DataTable dt = base._DbIstance.ExeProcedureGetDataTable(SpName.SpLoadingMenu);
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToBoolean(dr[DataRowName.IsEnable]))
                {
                    if (dr.IsNull(3))
                    {
                        #region 建立父結點
                        if (!DicMenu.ContainsKey(dr[DataRowName.MenuNo].ToString()))
                        {
                            Menu MenuParent = new Menu();
                            MenuParent.Name = dr[DataRowName.Name].ToString();
                            MenuParent.Url = dr[DataRowName.Url].ToString();                          
                            MenuParent.MenuNo = dr[DataRowName.MenuNo].ToString();
                            DicMenu.Add(dr[DataRowName.MenuNo].ToString(), MenuParent);
                        }
                        #endregion
                    }
                    else
                    {
                        #region  將子結點加入到父結點中
                        if (DicMenu.ContainsKey(dr[DataRowName.ParentNo].ToString()))
                        {
                            Menu MenuParent = DicMenu[dr[DataRowName.ParentNo].ToString()];
                            if (MenuParent.MenuList == null)
                            {
                                MenuParent.MenuList = new List<Menu>();
                            }
                            Menu ChildMenu = new Menu();
                            ChildMenu.Name = dr[DataRowName.Name].ToString();
                            ChildMenu.Url = dr[DataRowName.Url].ToString();
                            ChildMenu.MenuNo = dr[DataRowName.MenuNo].ToString();
                            MenuParent.MenuList.Add(ChildMenu);
                        }
                        #endregion
                    }
                }
            }
            foreach (Menu Item in DicMenu.Values)
            {
                MenuList.Add(Item);            
            }
            return MenuList;
        }//end GetMenuViewModel


       
        /// <summary>撈取主選單功能資料</summary>
        /// <returns></returns>
        public List<Menu> GetBrowseView()
        {
            List<Menu> MenuList = new List<Menu>();
            DataTable dt = base._DbIstance.ExeProcedureGetDataTable(SpName.SpLoadingMenu);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Menu MainMenu = new Menu();
                    MainMenu.Name = dr[DataRowName.Name].ToString();
                    MainMenu.Url = dr[DataRowName.Url].ToString();
                    MainMenu.MenuNo = dr[DataRowName.MenuNo].ToString();
                    MainMenu.IsEnable = Convert.ToBoolean(dr[DataRowName.IsEnable]);
                    MainMenu.ParentNo =dr[DataRowName.ParentNo].ToString();
                    MenuList.Add(MainMenu);
                }
            }
            return MenuList;
        }


        /// <summary>新增Menu </summary>
        /// <param name="MainMenu"></param>
        /// <returns></returns>
        public Menu AddMenu(Menu MainMenu)
        {
            base._DbIstance.AddParameter(DataRowName.Name, MainMenu.Name);
            base._DbIstance.AddParameter(DataRowName.Url,MainMenu.Url);
            base._DbIstance.AddParameter(DataRowName.ParentNo, MainMenu.ParentNo);
            base._DbIstance.AddParameter(DataRowName.IsEnable, MainMenu.IsEnable);
            base._DbIstance.ExeProcedureNotQuery(SpName.SPAddMenu);


            return MainMenu;
        }

        public override object Add<T>(T Obj)
        {
            Menu _Menu = JsonConvert.DeserializeObject<Menu>(Obj.ToString());
            Operation Oper = new Operation();
            Oper.Obj = _Menu;
            base._DbIstance.AddParameter(DataRowName.MenuNo, _Menu.MenuNo);
            base._DbIstance.AddParameter(DataRowName.Name, _Menu.Name);
            base._DbIstance.AddParameter(DataRowName.Url, _Menu.Url);
            base._DbIstance.AddParameter(DataRowName.ParentNo, _Menu.ParentNo);
            base._DbIstance.AddParameter(DataRowName.IsEnable, _Menu.IsEnable);
            base._DbIstance.AddParameter(DataRowName.MenuOrder, _Menu.MenuOrder);
            base._DbIstance.AddParameter(DataRowName.IsDone, 1, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureHasResult(SpName.SPAddMenu);
            string OutParameter = base._DbIstance.OutParameterValues[0].ToString();
            if (OutParameter == GlobalParameter.StoreProcedureResult.Error)
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Error;
            }
            else
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Success;
            }
            return Oper;
        }


        public override object Delete<T>(T Obj)
        {
            Menu _Menu = JsonConvert.DeserializeObject<Menu>(Obj.ToString());
            Operation Oper = new Operation();
            Oper.Obj = _Menu;
            base._DbIstance.AddParameter(DataRowName.MenuNo, _Menu.MenuNo);
            base._DbIstance.AddParameter(DataRowName.IsDone, 1, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureHasResult(SpName.SPDeleteMenu);
            string OutParameter = base._DbIstance.OutParameterValues[0].ToString();
            if (OutParameter == GlobalParameter.StoreProcedureResult.Error)
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Error;
            }
            else
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Success;
            }
            return Oper;
        }

        public override object Edit<T>(T Obj)
        {
            Menu _Menu = JsonConvert.DeserializeObject<Menu>(Obj.ToString());
            Operation Oper = new Operation();
            Oper.Obj = _Menu;
            base._DbIstance.AddParameter(DataRowName.MenuNo, _Menu.MenuNo);
            base._DbIstance.AddParameter(DataRowName.Name, _Menu.Name);
            base._DbIstance.AddParameter(DataRowName.Url, _Menu.Url);
            base._DbIstance.AddParameter(DataRowName.ParentNo, _Menu.ParentNo);
            base._DbIstance.AddParameter(DataRowName.IsEnable, _Menu.IsEnable);
            base._DbIstance.AddParameter(DataRowName.MenuOrder, _Menu.MenuOrder);
            base._DbIstance.AddParameter(DataRowName.IsDone, 1, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureHasResult(SpName.SPEditMenu);
            string OutParameter = base._DbIstance.OutParameterValues[0].ToString();
            if (OutParameter == GlobalParameter.StoreProcedureResult.Error)
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Error;
            }
            else
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Success;
            }
            return Oper;
        }
        

        /// <summary>取的角色可顯示清單 </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public override object Select<T>(T Obj)
        {
            LoginInfo Info = JsonConvert.DeserializeObject<LoginInfo>(Obj.ToString());
            Operation Oper = new Operation();
            Role _Role = new Role();
            _Role.RoleID = Info.User.RoleID;
            RolesMenuDAO RolesDAO = new RolesMenuDAO(base._DbIstance.ConnectiinString);
            Oper = RolesDAO.Select(JsonConvert.SerializeObject(_Role)) as Operation;
            _Role = Oper.Obj as Role;
       
         
            List<Menu> AllMenuList=new List<Menu>();

            Dictionary<string, Menu> DicMenu = new Dictionary<string, Menu>();
            DataTable dt = base._DbIstance.ExeProcedureGetDataTable(SpName.SpLoadingMenu);
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToBoolean(dr[DataRowName.IsEnable]))
                {                   
                    string MenuNo=dr[DataRowName.MenuNo].ToString();
                    //List<string> MenuList =new  List<string>();//Roles.MenuNoList.Where(X => X == MenuNo).ToList();
                    var MenuList =
                       from v in _Role.RoleMenuList
                       where v.MenuNo == MenuNo
                       orderby v
                       select v;


                    if (MenuList.ToList().Count > 0)//角色有的Menu才加入
                    {
                        if (string.IsNullOrEmpty(dr[3].ToString()))
                        {
                            #region 建立父結點
                            if (!DicMenu.ContainsKey(dr[DataRowName.MenuNo].ToString()))
                            {
                                Menu MenuParent = new Menu();
                                MenuParent.Name = dr[DataRowName.Name].ToString();
                                MenuParent.Url = dr[DataRowName.Url].ToString();
                                MenuParent.MenuNo = MenuNo;
                                DicMenu.Add(dr[DataRowName.MenuNo].ToString(), MenuParent);
                            }
                            #endregion
                        }
                        else
                        {
                            #region  將子結點加入到父結點中
                            if (DicMenu.ContainsKey(dr[DataRowName.ParentNo].ToString()))
                            {
                                Menu MenuParent = DicMenu[dr[DataRowName.ParentNo].ToString()];
                                if (MenuParent.MenuList == null)
                                {
                                    MenuParent.MenuList = new List<Menu>();
                                }
                                Menu ChildMenu = new Menu();
                                ChildMenu.Name = dr[DataRowName.Name].ToString();
                                ChildMenu.Url = dr[DataRowName.Url].ToString();
                                ChildMenu.MenuNo = MenuNo;
                                MenuParent.MenuList.Add(ChildMenu);
                            }
                            #endregion
                        }
                    }
                }
            }
            foreach (Menu Item in DicMenu.Values)
            {
                AllMenuList.Add(Item);
            }
            Oper.Obj = AllMenuList;
            return Oper;
        }
    }
}