using System;

namespace ObjectBase {
    public class CustomerDAO :CommBase{

        private class SP{
            public const string AddCustomer = "AddCustomer";
            public const string GetCustomerBySN = "GetCustomerBySN";
            public const string GetCustomerByAccount = "GetCustomerByAccount";
            public const string LoginCheck = "LoginCheck";
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
                return USP.ExeProcedureHasResultReturnCode(SP.AddCustomer);                
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
                 Customer _Cus  = USP.ExeProcedureGetObject(SP.GetCustomerBySN, _Customer) as Customer;
                 MemberDAO _MemberDB = new MemberDAO();
                 _Cus.Member = _MemberDB.GetMemberByCustomerSN(_Cus.SN);
                 return _Cus;
             }
             catch (Exception ex) {
                 CommTool.ToolLog.Log(ex);
                 _Customer.Member =new Member();
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
                MemberDAO _MemberDB = new MemberDAO();
                _Customer.Member = _MemberDB.GetMemberByCustomerSN(_Customer.SN);
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
        public Customer LoginCheck(string Account,string PassWord) {
            USP.AddParameter(SPParameter.Account, Account);
            USP.AddParameter(SPParameter.PassWord, PassWord);
            Customer _Customer =  USP.ExeProcedureGetObject(SP.LoginCheck, new Customer());
            if (_Customer != null) {
                MemberDAO _MemberDAO = new MemberDAO();
                _Customer.Member =  _MemberDAO.GetMemberByCustomerSN(_Customer.SN);
            }
            return _Customer;
        }

    }
}
