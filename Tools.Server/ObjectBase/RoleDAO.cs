using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectBase {
    public class RoleDAO : CommBase {

        private class SP {
            public const string GetRoleByCustomerSN = "GetRoleByCustomerSN";
        }

        private class SPParameter {
            public const string SN = "SN";
            public const string CustomerSN = "CustomerSN";
            public const string Permission = "Permission";
        }

        /// <summary>依照客戶SN取得權限</summary>
        /// <param name="CustomerSn"></param>
        /// <returns></returns>
        public Role GetRoleByCustomerSN(int CustomerSN) {
            USP.AddParameter(SPParameter.CustomerSN, CustomerSN);
            Role _Role = USP.ExeProcedureGetObject(SP.GetRoleByCustomerSN, new Role()) as Role;
            return _Role;
        }

    }
}
