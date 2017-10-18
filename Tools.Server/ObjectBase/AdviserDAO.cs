using System;
using System.Data;

namespace ObjectBase {
    public class AdviserDAO : CommBase {

        private class SP {
            public const string AddAdviser = "AddAdviser";
            public const string LoginCheckAdviser = "LoginCheckAdviser";
            public const string GetAdviserBySN = "GetAdviserBySN";
        }

        private class SParameter {
            public const string SN = "SN";
            public const string Account = "Account";
            public const string PassWord = "PassWord";
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string ID = "ID";
            public const string Gender = "Gender";
            public const string Email = "Email";
            public const string Phone = "Phone";
            public const string BirthDay = "BirthDay";
            public const string Address = "Address";
            public const string AddTime = "AddTime";
            public const string EditTime = "EditTime";
            public const string IsEnable = "IsEnable";
            public const string Remark = "Remark";
        }

        /// <summary>建立顧問</summary>
        /// <param name="_Adviser"></param>
        /// <returns></returns>
        public int AddAdviser(Adviser _Adviser) {
            USP.AddParameter(SParameter.Account, _Adviser.Account);
            USP.AddParameter(SParameter.PassWord, _Adviser.PassWord);
            USP.AddParameter(SParameter.FirstName, _Adviser.FirstName);
            USP.AddParameter(SParameter.LastName, _Adviser.LastName);
            USP.AddParameter(SParameter.ID, _Adviser.ID);
            USP.AddParameter(SParameter.Gender, _Adviser.Gender);
            USP.AddParameter(SParameter.Email, _Adviser.Email);
            USP.AddParameter(SParameter.Phone, _Adviser.Phone);
            USP.AddParameter(SParameter.BirthDay, _Adviser.BirthDay);
            USP.AddParameter(SParameter.Address, _Adviser.Address);
            USP.AddParameter(SParameter.Remark, _Adviser.Remark == null ? string.Empty : _Adviser.Remark);
            DataTable dt = USP.ExeProcedureGetDataTable(SP.AddAdviser);
            if (dt != null && dt.Rows.Count > 0) {
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            else {
                return -1;
            }
        }//end AddAdviser

        /// <summary>顧問登入</summary>
        /// <param name="_Adviser"></param>
        /// <returns></returns>
        public Adviser LoginCheckAdviser(Adviser _Adviser) {
            USP.AddParameter(SParameter.Account, _Adviser.Account);
            USP.AddParameter(SParameter.PassWord, _Adviser.PassWord);
            Adviser _ResultAdviser = USP.ExeProcedureGetObject(SP.LoginCheckAdviser, new Adviser());
            return _ResultAdviser;
        }//end LoginCheckAdviser
        
        /// <summary>依據SN取得顧問</summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public Adviser GetAdviserBySN(int SN) {
            USP.AddParameter(CommBase.SN, SN);
            Adviser _ResultAdviser = USP.ExeProcedureGetObject(SP.GetAdviserBySN, new Adviser());
            return _ResultAdviser;
        }
    }
}
