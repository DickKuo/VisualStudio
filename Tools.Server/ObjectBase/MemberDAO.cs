using System;

namespace ObjectBase {
    public class MemberDAO : CommBase {

        private class SP {
            public const string GetMemberByCustomerSN = "GetMemberByCustomerSN";
        }

        private class SPParamter {
            public const string CustomerSN = "CustomerSN";
        }

        /// <summary></summary>
        /// <param name="CustomerSN"></param>
        /// <returns></returns>
        public Member GetMemberByCustomerSN(int CustomerSN) {
            try {
                USP.AddParameter(SPParamter.CustomerSN, CustomerSN);
                Member _Member = USP.ExeProcedureGetObject(SP.GetMemberByCustomerSN, new Member());
                return _Member;
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return new Member();
            }
        }

    }
    
    enum GenderType { 
      Male =1,
      Fmale =2
    }
}
