using System.Data;
using System;

namespace ObjectBase
{
    public class ExtraTagDAO : CommBase
    {
        private class SP
        {
            public const string UpdateExtraTag = "UpdateExtraTag";
            public const string GetExtraTag = "GetExtraTag";
        }

        private class SSParameter
        {
            public const string SN = "SN";
            public const string Class = "Class";
            public const string ExtraUserType = "ExtraUserType";
            public const string ExtraUserSN = "ExtraUserSN";
            public const string Tag = "Tag";
        }

        /// <summary>更新ExtraTag</summary>
        /// <param name="UserType"></param>
        /// <param name="Class"></param>
        /// <param name="UserSN"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public int UpdateExtraTag(ExtraUserType UserType, ExtraClass Class, int UserSN, int Tag)
        {
            USP.AddParameter(SSParameter.Class, Class);
            USP.AddParameter(SSParameter.ExtraUserType, UserType);
            USP.AddParameter(SSParameter.ExtraUserSN, UserSN);
            USP.AddParameter(SSParameter.Tag, Tag);
            DataTable dt = USP.ExeProcedureGetDataTable(SP.UpdateExtraTag);
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>取得ExtraTag</summary>
        /// <param name="UserType"></param>
        /// <param name="Class"></param>
        /// <param name="UserSN"></param>
        /// <returns></returns>
        public int GetExtraTag(ExtraUserType UserType, ExtraClass Class, int UserSN)
        {
            USP.AddParameter(SSParameter.Class, Class);
            USP.AddParameter(SSParameter.ExtraUserType, UserType);
            USP.AddParameter(SSParameter.ExtraUserSN, UserSN);
            DataTable dt = USP.ExeProcedureGetDataTable(SP.GetExtraTag);
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }
    }
}