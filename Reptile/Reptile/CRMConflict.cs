using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reptile
{

    public class CRMConflict
    {

        public string SN { set; get; }

        /// <summary>ConflictCustomerID table sn</summary>
        public string ApplyID { set; get; }

        /// <summary>姓名</summary>
        public string Name { set; get; }

        /// <summary>信箱</summary>
        public string Email { set; get; }

        /// <summary>電話</summary>
        public string Tel { set; get; }

        /// <summary>城市</summary>
        public string Country { set; get; }

        /// <summary>踩線審核結果</summary>
        public string ConflictResult { set; get; }

        /// <summary>審核結果原因</summary>
        public string ConflictResultReason { set; get; }
    }

    public class UsgCustomer
    {
        /// <summary>SN</summary>
        [UsgReflection(IsReflection = false)]
        public int Sn { set; get; }

        /// <summary>帳號</summary>
        [UsgReflection(IsReflection = false)]
        public string Account { set; get; }

        /// <summary>LastName(姓)</summary>
        public string LastName { set; get; }

        /// <summary>FirstName(名)</summary>
        public string FirstName { set; get; }

        /// <summary>性別</summary>
        [UsgReflection(IsReflection = false)]
        public int Sex { set; get; }

        /// <summary>生日</summary>
        [UsgReflection(IsReflection = false)]
        public string Birth { set; get; }

        /// <summary>國家(居住)</summary>
        public string HomeCountry { set; get; }

        /// <summary>城市(居住)</summary>
        public string HomeCity { set; get; }

        /// <summary>地址(居住)</summary>
        public string HomeAdd { set; get; }

        /// <summary>郵遞區號(居住)</summary>
        public string HomeCode { set; get; }

        /// <summary>國家(郵寄)</summary>
        public string MailCountry { set; get; }

        /// <summary>城市(郵寄)</summary>
        public string MailCity { set; get; }

        /// <summary>住址(郵寄)</summary>
        public string MailAdd { set; get; }

        /// <summary>郵遞區號(郵寄)</summary>
        public string MailCode { set; get; }

        /// <summary>國碼(居住電話)</summary>
        public string TelCountry { set; get; }

        /// <summary>顧問一 link to USGAdviser 
        /// AdviserNo
        /// </summary>
        [UsgReflection(IsReflection = false)]
        public string Empcode { set; get; }

        /// <summary>新增日期</summary>
        [UsgReflection(IsReflection = false)]
        public string Addtime { set; get; }

        /// <summary>修改日期</summary>
        [UsgReflection(IsReflection = false)]
        public string Edittime { set; get; }

        /// <summary>客戶身分</summary>
        [UsgReflection(IsReflection = false)]
        public string CustomerIdentity { set; get; }

        /// <summary>名(Company )</summary>
        [UsgReflection(IsReflection = false)]
        public string CompanyName { set; get; }

        /// <summary>區碼(居住電話)</summary>
        public string TelZone { set; get; }

        /// <summary>號碼(居住電話)</summary>
        public string TelCode { set; get; }

        /// <summary>電子信箱</summary>
        public string Email { set; get; }

        /// <summary>客戶來源</summary>
        [UsgReflection(IsReflection = false)]
        public int ClientSource { set; get; }

        /// <summary>USG認證狀態</summary>
        [UsgReflection(IsReflection = false)]
        public int ApproveStatus { set; get; }

        /// <summary>業務備註</summary>      
        [UsgReflection(IsReflection = false)]
        public string SalesRemark { set; get; }

        /// <summary>區域</summary>  
        [UsgReflection(IsReflection = false)]
        public int DistributionSupervisor { set; get; }
    }

    public class UsgUpdateStatus
    {
        public int SN { set; get; }

        public int ApproveStatus { set; get; }

        /// <summary>報聘單位代號</summary>
        public string Unit { set; get; }

        public string Remark { set; get; }

    }

    public class UsgAdviser
    {

        /// <summary>SN</summary>
        [UsgReflection(IsReflection = false)]
        public int Sn { set; get; }

        /// <summary>LastName(姓)</summary>
        public string Adv_LastName { set; get; }

        /// <summary>FirstName(名)</summary>
        public string Adv_FirstName { set; get; }

        /// <summary>性別</summary>
        [UsgReflection(IsReflection = false)]
        public int Adv_Sex { set; get; }

        /// <summary>英文名子</summary>
        public string Adv_Ename { set; get; }

        /// <summary>生日</summary>
        [UsgReflection(IsReflection = false)]
        public string Adv_Birthday { set; get; }

        /// <summary>身分證字號 or 護照</summary>
        public string Adv_ID { set; get; }

        /// <summary>顧問編號(IBCode)</summary>
        public string AdviserNo { set; get; }

        /// <summary>居住國家</summary>
        public string Adv_Country2 { set; get; }

        /// <summary>居住省分</summary>
        public string Adv_Sident2 { set; get; }

        /// <summary>居住城市</summary>
        public string Adv_City2 { set; get; }

        /// <summary>居住地址</summary>
        public string Adv_Add2 { set; get; }

        /// <summary>居住郵政編號</summary>
        public string Adv_MailCode2 { set; get; }

        /// <summary>電子信箱</summary>
        public string Adv_EMail { set; get; }

        /// <summary>英文(LastName)</summary>
        public string Adv_CLastName { set; get; }

        /// <summary>英文(FirstName)</summary>
        public string Adv_CFirstName { set; get; }

        /// <summary>顧問類別(Staff:員工 Business:業務 其餘為IB)</summary>
        public string Adv_Type { set; get; }

        /// <summary>居住電話</summary>
        public string Adv_TelCode2 { set; get; }

        /// <summary>行動電話</summary>
        public string Adv_TelCode3 { set; get; }

        /// <summary>戶籍國家</summary>
        public string Adv_Country1 { set; get; }

        /// <summary>戶籍城市</summary>
        public string Adv_City1 { set; get; }

        /// <summary>戶籍地址</summary>
        public string Adv_Add1 { set; get; }

        /// <summary>戶籍郵政編號</summary>
        public string Adv_Mailcode1 { set; get; }

        /// <summary>輔導者</summary>
        public string HelpNo { set; get; }

        /// <summary>報聘單位代號</summary>
        public string Unit { set; get; }

        /// <summary>顧問報聘生效日</summary>
        [UsgReflection(IsReflection = false)]
        public string Effectivedate { set; get; }

        /// <summary>資料變更日期</summary>
        [UsgReflection(IsReflection = false)]
        public string Datachangedate { set; get; }

        /// <summary>是否為公司戶</summary>
        [UsgReflection(IsReflection = false)]
        public bool Company { set; get; }

        /// <summary>公司名</summary>
        public string Company1 { set; get; }

        /// <summary>USG認證狀態</summary>
        [UsgReflection(IsReflection = false)]
        public int ApproveStatus { set; get; }
    }

    public class TempCustomer
    {

        public List<UsgCustomer> jsondata { set; get; }

        public string feature { set; get; }

        public string timestamp { set; get; }

        public TempCustomer()
        {
            jsondata = new List<UsgCustomer>();
        }
    }

    public class TempAdviser
    {
        private class Default
        {
            public const int BaseTimeYear = 1970;
            public const int BaseTimeMonth = 01;
            public const int BaseTimeDay = 01;
        }

        public UsgAdviser jsondata { set; get; }

        public string feature { set; get; }

        public string timestamp { set; get; }
        public void GetTimestamp()
        {
            DateTime BaseTime = new DateTime(Default.BaseTimeYear, Default.BaseTimeMonth, Default.BaseTimeDay);
            DateTime NewTime = DateTime.UtcNow;
            TimeSpan Span = NewTime.Subtract(BaseTime);
            this.timestamp = Convert.ToInt32(Span.TotalSeconds).ToString();
        }
    }

    public class ListTempAdviser
    {
        private class Default
        {
            public const int BaseTimeYear = 1970;
            public const int BaseTimeMonth = 01;
            public const int BaseTimeDay = 01;
        }

        public List<UsgAdviser> jsondata { set; get; }

        public string feature { set; get; }

        public string timestamp { set; get; }
        public void GetTimestamp()
        {
            DateTime BaseTime = new DateTime(Default.BaseTimeYear, Default.BaseTimeMonth, Default.BaseTimeDay);
            DateTime NewTime = DateTime.UtcNow;
            TimeSpan Span = NewTime.Subtract(BaseTime);
            this.timestamp = Convert.ToInt32(Span.TotalSeconds).ToString();
        }

        public ListTempAdviser()
        {
            jsondata = new List<UsgAdviser>();
        }
    }


    public class TempUpdateStatus
    {

        private class Default
        {
            public const int BaseTimeYear = 1970;
            public const int BaseTimeMonth = 01;
            public const int BaseTimeDay = 01;
        }

        public List<UsgUpdateStatus> Jsondata { set; get; }

        public string Feature { set; get; }

        public string Timestamp { set; get; }

        public void GetTimestamp()
        {
            DateTime BaseTime = new DateTime(Default.BaseTimeYear, Default.BaseTimeMonth, Default.BaseTimeDay);
            DateTime NewTime = DateTime.UtcNow;
            TimeSpan Span = NewTime.Subtract(BaseTime);
            this.Timestamp = Convert.ToInt32(Span.TotalSeconds).ToString();
        }

        public TempUpdateStatus()
        {
            Jsondata = new List<UsgUpdateStatus>();
        }
    }

    public class UsgReflectionAttribute : Attribute
    {
        /// <summary>跳過對應</summary>
        public bool IsReflection { get; set; }
    }

    public class UsgArrayCRMResponse
    {

        public List<UsgCRMResponse> Result { set; get; }
        public string FileName { set; get; }
    }

    public class UsgCRMResponse
    {

        public string SN { set; get; }

        public string Result { set; get; }

        public string ErrorCode { set; get; }

        public string FileName { set; get; }
    }

    public class CRMRequest
    {
        public string IBCode { set; get; }

        public string feature { set; get; }

        public string timestamp { set; get; }

    }

    /// <summary>來源</summary>
    public enum CRMSource
    {
        //快速註冊              Quick Regis.
        QuickReg = 1,
        //模擬帳戶申請          Demo Acc. Regis.
        Demo = 2,
        //開戶未成功            Imcomplete Acc. Open
        AccountOpen = 3,
        //等待介紹              To be referred
        Introduction = 4,
        //手動導入              Manually imported
        Manually = 5,
        //UsgFX Times           UsgFX Times
        UsgFXTimes = 6,
        //Landing Page          Landing Page
        LandingPage = 7,
        //NewsLetter            NewsLetter
        NewsLetter = 8,
        //E-Book                E-Book  
        EBook = 9
    }

    /// <summary>顧問審核狀態</summary>
    public enum AdviserApproveStatus
    {
        //正常                正常
        Normal = 1,
        //禁止出薪            Banned from Receiving Commission Payment
        Suspicious = 3,
        //文件不齊全          Documents are incomplete
        Incomplete = 4,
        //停權                停權
        Suspended = 5,
        //離職                Resigned
        Resigned = 6,
        //新建                Newly Registered
        NewlyRegistered = 7
    }

    /// <summary>地區</summary>
    public enum DistributionSupervisor
    {
        //澳洲                Australia
        Australia = 1,
        //台灣                Taiwan
        Taiwan = 2,
        //中國                China
        China = 3,
        //不分配              Not to assign
        Nottoassign = 4,
        //待分配              To be assigned
        Tobeassigned = 5,
        //其他                Other
        Other = 6,
        //澳洲(TW)            AU(TW)
        AU_TW = 8
    }

    /// <summary>性別</summary>
    public enum Sex
    {
        //男                 Male
        M = 1,
        //女                 Female
        F = 2
    }

    /// <summary>客戶審核狀態列舉</summary>
    public enum CustomerApproveStatus
    {
        //文件未上傳                   Documents Not Uploaded
        DocumentsNotUploaded = 1,
        //MT4已開戶                    MT4 Account Opened
        MT4AccountOpened = 2,
        //等待入金                     Awaiting Deposit
        AwaitingDeposit = 3,
        //正常戶                       Normal Account
        NormalAccount = 4,
        //靜止戶                       Dormant Account
        DormantAccount = 5,
        //結清戶                       Closed Account
        ClosedAccount = 6,
        //新開戶                       Newly Opened Account
        NewlyOpenedAccount = 7,
        //未檢查                       Account Unchecked
        AccountUnchecked = 9,
        //已檢查                       Account Checked
        AccountChecked = 10,
        //可疑帳戶                     Suspicious
        Suspicious = 11,
        //違規戶                       Disabled
        Disabled = 12
    }

    public enum CRMState
    {
        //新導入
        NewlyRegistered = 101,
        //已分配
        Assigned = 102,
        //已開戶
        AccOpened = 103,
        //已報聘
        Registered = 104,
        //有人報備
        Reported = 105,
        //重複登記
        DuplicateRegis = 106
    }
}