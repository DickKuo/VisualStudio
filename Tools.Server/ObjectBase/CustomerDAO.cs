using System;
using System.Collections.Generic;
using System.Data;

namespace ObjectBase {
    public class CustomerDAO : CommBase {

        private class SP {
            public const string AddCustomer = "AddCustomer";
            public const string GetCustomerBySN = "GetCustomerBySN";
            public const string GetCustomerByAccount = "GetCustomerByAccount";
            public const string LoginCheck = "LoginCheck";
            public const string UpdatePassWord = "UpdatePassWord";
            public const string GetCustomerListByPage = "GetCustomerListByPage";
            public const string UpdateCustomerByAccount = "UpdateCustomerByAccount";
            public const string SearchCustomerList = "SearchCustomerList";
            public const string UpdateCustomerChipsByAccount = "UpdateCustomerChipsByAccount";
        }

        private class SPParameter {
            public const string Account = "Account";
            public const string PassWord = "PassWord";
            public const string State = "State";
            public const string LockCount = "LockCount";
            public const string IsLock = "IsLock";
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string Gender = "Gender";
            public const string ID = "ID";
            public const string Email = "Email";
            public const string Phone = "Phone";
            public const string BirthDay = "BirthDay";
            public const string HomeAddr = "HomeAddr";
            public const string NickName = "NickName";
            public const string SN = "SN";
            public const string NowPassWord = "NowPassWord";
            public const string NewPassWord = "NewPassWord";
            public const string Range = "Range";
            public const string Page = "Page";
            public const string Audit = "Audit";
            public const string Remark = "Remark";
            public const string IsEnable = "IsEnable";
            public const string Name = "Name";
            public const string Commission = "Commission";
            public const string HelperSN = "HelperSN";
            public const string Chips = "Chips";
        }

        /// <summary>加入客戶</summary>
        /// <param name="_Customer"></param>
        /// <returns></returns>
        public int AddCustomer(Customer _Customer) {
            try {
                USP.AddParameter(SPParameter.PassWord, _Customer.PassWord);
                USP.AddParameter(CommBase.Remark, _Customer.Remark == null ? string.Empty : _Customer.Remark);
                USP.AddParameter(SPParameter.FirstName, _Customer.Member.FirstName);
                USP.AddParameter(SPParameter.LastName, _Customer.Member.LastName);
                USP.AddParameter(SPParameter.Gender, _Customer.Member.Gender);
                USP.AddParameter(SPParameter.ID, _Customer.Member.ID);
                USP.AddParameter(SPParameter.Email, _Customer.Member.Email);
                USP.AddParameter(SPParameter.Phone, _Customer.Member.Phone);
                USP.AddParameter(SPParameter.BirthDay, _Customer.Member.BirthDay);
                USP.AddParameter(SPParameter.HomeAddr, _Customer.Member.HomeAddr);
                USP.AddParameter(SPParameter.NickName, _Customer.Member.NickName == null ? string.Empty : _Customer.Member.NickName);
                DataTable dt = USP.ExeProcedureGetDataTable(SP.AddCustomer);
                if (dt != null && dt.Rows.Count > 0) {
                    return Convert.ToInt32(dt.Rows[0][0]);
                }
                else {
                    return -1;
                }
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return -1;
            }
        }

        /// <summary>由SN取得帳戶</summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public Customer GetCustomerBySN(int SN) {
            Customer _Customer = new Customer();
            try {
                USP.AddParameter(CommBase.SN, SN);
                _Customer = USP.ExeProcedureGetObject(SP.GetCustomerBySN, _Customer) as Customer;
                if (_Customer != null) {
                    _Customer = CombiCustomer(_Customer);
                }
                return _Customer;
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                _Customer.Member = new Member();
                return _Customer;
            }
        }

        /// <summary>由帳號取得客戶資料</summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public Customer GetCustomerByAccount(string Account) {
            try {
                USP.AddParameter(SPParameter.Account, Account);
                Customer _Customer = USP.ExeProcedureGetObject(SP.GetCustomerByAccount, new Customer());
                if (_Customer != null) {
                    _Customer = CombiCustomer(_Customer);
                }
                return _Customer;
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return new Customer();
            }
        }

        /// <summary>檢查登入帳號</summary>
        /// <param name="Account"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        public Customer LoginCheck(string Account, string PassWord) {
            USP.AddParameter(SPParameter.Account, Account);
            USP.AddParameter(SPParameter.PassWord, PassWord);
            Customer _Customer = USP.ExeProcedureGetObject(SP.LoginCheck, new Customer());
            if (_Customer != null) {
                _Customer = CombiCustomer(_Customer);
            }
            return _Customer;
        }

        /// <summary>整合客戶的所有物件</summary>
        /// <param name="_Customer"></param>
        /// <returns></returns>
        private Customer CombiCustomer(Customer _Customer) {
            MemberDAO _MemberDAO = new MemberDAO();
            RoleDAO _RoleDAO = new RoleDAO();
            ExtraTagDAO _ExtraTagDAO = new ExtraTagDAO();
            _Customer.Member = _MemberDAO.GetMemberByCustomerSN(_Customer.SN);
            _Customer.Role = _RoleDAO.GetRoleByCustomerSN(_Customer.SN);
            _Customer.MinimunLotLimit = _ExtraTagDAO.GetExtraTag(ExtraUserType.Customer, ExtraClass.MinimunLot, _Customer.SN);
            return _Customer;
        }

        /// <summary>設定最小交易組數</summary>
        /// <param name="_Customer"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public int SetMinimunLotLimit(Customer _Customer, int Tag) {
            ExtraTagDAO _ExtraTagDAO = new ExtraTagDAO();
            return _ExtraTagDAO.UpdateExtraTag(ExtraUserType.Customer, ExtraClass.MinimunLot, _Customer.SN, Tag);
        }

        /// <summary>變更密碼</summary>
        /// <param name="_Customer"></param>
        /// <param name="NewPassWord"></param>
        /// <returns></returns>
        public int UpdatePassWord(Customer _Customer, string NewPassWord) {
            USP.AddParameter(SPParameter.SN, _Customer.SN);
            USP.AddParameter(SPParameter.NowPassWord, _Customer.PassWord);
            USP.AddParameter(SPParameter.NewPassWord, NewPassWord);
            DataTable dt = USP.ExeProcedureGetDataTable(SP.UpdatePassWord);
            if (dt != null && dt.Rows.Count > 0) {
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            else {
                return -1;
            }
        }

        /// <summary>列出指定頁的客戶</summary>
        /// <param name="Page">指定頁</param>
        /// <param name="Records">一頁列出筆數</param>
        /// <returns></returns>
        public List<Customer> GetCustomerListByPage(int Page, int Range) {
            List<Customer> CustomerList = new List<Customer>();
            USP.AddParameter(SPParameter.Range, Range);
            USP.AddParameter(SPParameter.Page, Page);
            List<Customer> ResultList = new List<Customer>();
            CustomerList = USP.ExeProcedureGetObjectList(SP.GetCustomerListByPage, new Customer());
            foreach (Customer Item in CustomerList) {
                ResultList.Add(CombiCustomer(Item));
            }
            return ResultList;
        }

        /// <summary>變更客戶資料</summary>
        /// <param name="_Customer"></param>
        /// <returns></returns>
        public int UpdateCustomerByAccount(Customer _Customer) {
            USP.AddParameter(SPParameter.Account, _Customer.Account);
            USP.AddParameter(SPParameter.Audit, _Customer.Audit);
            USP.AddParameter(SPParameter.FirstName, _Customer.Member.FirstName);
            USP.AddParameter(SPParameter.LastName, _Customer.Member.LastName);
            USP.AddParameter(SPParameter.NickName, _Customer.Member.NickName);
            USP.AddParameter(SPParameter.Gender, _Customer.Member.Gender);
            USP.AddParameter(SPParameter.ID, _Customer.Member.ID);
            USP.AddParameter(SPParameter.Email, _Customer.Member.Email);
            USP.AddParameter(SPParameter.Phone, _Customer.Member.Phone);
            USP.AddParameter(SPParameter.BirthDay, _Customer.Member.BirthDay);
            USP.AddParameter(SPParameter.HomeAddr, _Customer.Member.HomeAddr);
            USP.AddParameter(SPParameter.IsEnable, _Customer.IsEnable);
            USP.AddParameter(SPParameter.Commission, _Customer.Commission);
            USP.AddParameter(SPParameter.HelperSN, _Customer.HelperSN);
            USP.AddParameter(SPParameter.Remark, _Customer.Member.Remark == null ? string.Empty : _Customer.Member.Remark);
            DataTable dt = USP.ExeProcedureGetDataTable(SP.UpdateCustomerByAccount);
            if (dt != null && dt.Rows.Count > 0) {
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            else {
                return -1;
            }
        }

        /// <summary></summary>
        /// <param name="_Customer"></param>
        /// <returns></returns>
        public int UpdateCustomerChipsByAccount(Customer _Customer)
        {
            USP.AddParameter(SPParameter.Account, _Customer.Account);
            USP.AddParameter(SPParameter.Chips, _Customer.Chips);
            int Result = USP.ExeProcedureHasResultReturnCode(SP.UpdateCustomerChipsByAccount);
            return Result;
        }

        /// <summary>依條件搜尋客戶列表</summary>
        /// <param name="_Customer"></param>
        /// <returns></returns>
        public List<Customer> SearchCustomerList(Customer _Customer) {
            USP.AddParameter(SPParameter.Account, _Customer.Account == null ? string.Empty : _Customer.Account);
            USP.AddParameter(SPParameter.Name, _Customer.Member.FirstName == null ? string.Empty : _Customer.Member.FirstName);
            USP.AddParameter(SPParameter.NickName, _Customer.Member.NickName == null ? string.Empty : _Customer.Member.NickName);
            USP.AddParameter(SPParameter.Phone, _Customer.Member.Phone == null ? string.Empty : _Customer.Member.Phone);
            List<Customer> CustomerList = USP.ExeProcedureGetObjectList(SP.SearchCustomerList,new Customer());
            List<Customer> ResultCustomer =new List<Customer>();
            foreach (Customer Item in CustomerList)
            {
                ResultCustomer.Add(CombiCustomer(Item));
            }
            return ResultCustomer;
        }

    }
}
