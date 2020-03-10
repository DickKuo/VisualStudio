using System;

namespace ObjectBase
{
    public class MemberDAO : CommBase
    {
        private class SP
        {
            public const string GetMemberByCustomerSN = "GetMemberByCustomerSN";
            public const string UpdateMemberBySN = "UpdateMemberBySN";
        }

        private class SPParamter
        {
            public const string CustomerSN = "CustomerSN";
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string NickName = "NickName";
            public const string Gender = "Gender";
            public const string Email = "Email";
            public const string Phone = "Phone";
            public const string BirthDay = "BirthDay";
            public const string HomeAddr = "HomeAddr";
            public const string Remark = "Remark";
        }

        /// <summary></summary>
        /// <param name="CustomerSN"></param>
        /// <returns></returns>
        public Member GetMemberByCustomerSN(int CustomerSN)
        {
            try
            {
                USP.AddParameter(SPParamter.CustomerSN, CustomerSN);
                Member _Member = USP.ExeProcedureGetObject(SP.GetMemberByCustomerSN, new Member());
                return _Member;
            }
            catch (Exception ex)
            {
                CommTool.ToolLog.Log(ex);
                return new Member();
            }
        }

        /// <summary>更新會員資料</summary>
        /// <param name="_Member"></param>
        /// <returns></returns>
        public int UpdateMember(Member _Member)
        {
            try
            {
                USP.AddParameter(CommBase.SN, _Member.SN);
                USP.AddParameter(SPParamter.FirstName, _Member.FirstName);
                USP.AddParameter(SPParamter.LastName, _Member.LastName);
                USP.AddParameter(SPParamter.NickName, _Member.NickName);
                USP.AddParameter(SPParamter.Gender, _Member.Gender);
                USP.AddParameter(SPParamter.Email, _Member.Email);
                USP.AddParameter(SPParamter.Phone, _Member.Phone);
                USP.AddParameter(SPParamter.BirthDay, _Member.BirthDay);
                USP.AddParameter(SPParamter.HomeAddr, _Member.HomeAddr);
                USP.AddParameter(SPParamter.Remark, _Member.Remark == null ? string.Empty : _Member.Remark);
                return USP.ExeProcedureHasResultReturnCode(SP.UpdateMemberBySN);
            }
            catch (Exception ex)
            {
                CommTool.ToolLog.Log(ex);
                return -1;
            }
        }
    }
}