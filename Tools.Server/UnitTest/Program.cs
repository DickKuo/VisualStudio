using CommTool.Business.Services;
using ObjectBase;
using Stock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace UnitTest {
    class Program {

        [DllImport("user32.dll", SetLastError = true)]
        public static extern Int32 SendInput(Int32 cInputs, ref INPUT pInputs, Int32 cbSize);
        private class Default {
            public const string TimeFormat = "yyyy/MM/dd";
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 28)]
        public struct INPUT {
            [FieldOffset(0)]
            public INPUTTYPE dwType;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBOARDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MOUSEINPUT {
            public Int32 dx;
            public Int32 dy;
            public Int32 mouseData;
            public MOUSEFLAG dwFlags;
            public Int32 time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct KEYBOARDINPUT {
            public Int16 wVk;
            public Int16 wScan;
            public KEYBOARDFLAG dwFlags;
            public Int32 time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct HARDWAREINPUT {
            public Int32 uMsg;
            public Int16 wParamL;
            public Int16 wParamH;
        }

        public enum INPUTTYPE : int {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }

        [Flags()]
        public enum MOUSEFLAG : int {
            MOVE = 0x1,
            LEFTDOWN = 0x2,
            LEFTUP = 0x4,
            RIGHTDOWN = 0x8,
            RIGHTUP = 0x10,
            MIDDLEDOWN = 0x20,
            MIDDLEUP = 0x40,
            XDOWN = 0x80,
            XUP = 0x100,
            VIRTUALDESK = 0x400,
            WHEEL = 0x800,
            ABSOLUTE = 0x8000
        }

        [Flags()]
        public enum KEYBOARDFLAG : int {
            EXTENDEDKEY = 1,
            KEYUP = 2,
            UNICODE = 4,
            SCANCODE = 8
        }

        static void Main(string[] args) {
            //MainMethod();

           // Reward _Reward = new Reward();
           // RewardDAO Dao = new RewardDAO();
           // _Reward.SN = 3;
           // _Reward.PayCustomerSN = 1;
           // //_Reward.CustomerSN = 1;
           // //_Reward.Money = 21m;
           // //_Reward.RewardTime = DateTime.Now;
           //int trtt=  Dao.PayReward(_Reward);

           // EWalletDAO DAO = new EWalletDAO();
           //decimal result = DAO.ReCalculateEWallet(2);

            //CreateCustomer();
            //StockDAO DAO = new StockDAO();
            ////DAO.GetOptionEveryDay("https://tw.screener.finance.yahoo.net/future/aa03?fumr=futurepart&amp;opmr=optionpart&amp;opcm=WTXO&amp;opym=");
            ////DAO.ControlPrice();  
            //WebInfo.WebInfo Web = new WebInfo.WebInfo();

            //string TempCode = System.Web.HttpUtility.UrlEncode("_台選", System.Text.Encoding.GetEncoding("BIG5")).ToUpper();//将繁体汉字转换为Url

            //string Temp ="201705W4".Replace(DateTime.Now.Year.ToString(),string.Empty);
            //string []arr =Temp.Split('W');
            //string GetTemp =string.Empty;
            //if(arr.Length>0)
            //{
            //    GetTemp =string.Format("TX{0}{1}{2}W{0}{1}",arr[1],arr[0],TempCode);
            //}

            //WebInfo.WebInfo we = new WebInfo.WebInfo();
            //we.HttpPostMethod("http://www666.usgfx.com/usgapiserver/DepositPage/SafechargeNotifications?ppp_status=OK&cardCompany=Visa&nameOnCard=www&country=TW&currency=USD&merchant_site_id=135323&merchant_id=439896639487429442&merchantLocale=zh_TW&requestVersion=3.0.0&PPP_TransactionID=221212373&productId=usgfx&customData=Union+Standard+Test+Multi&payment_method=cc_card&responseTimeStamp=2017-06-06.06%3A05%3A56&message=Success&Error=Success&userPaymentOptionId=1558793&Status=APPROVED&ExErrCode=0&ErrCode=0&AuthCode=111876&ReasonCode=0&Token=ZQBLAEYAUgBnAHMATABHAG0AMgAtAE8AUAA4AHIALgBaAFsAPgBhAD0AWABpACsAUABaAFAANwA1ACkALABZAF0AYgBkAEcAdQAlADMAbgBgAHoAMwA%3D&tokenId=7355934&responsechecksum=345433cb5bb9a14722ec447cf197d5c4&advanceResponseChecksum=46f2b4805dd5c3d7d524937f2f427d83&totalAmount=7.00&TransactionID=101508687190&dynamicDescriptor=Union+Standard+Test&uniqueCC=2X6q1jdQkdgSLk49u671NnyQWVQ%3D&orderTransactionId=1005396063&item_amount_1=7.00&item_quantity_1=1&merchant_unique_id=20170606140440-10011618", "http://www666.usgfx.com/usgapiserver/DepositPage/SafechargeNotifications");

            //Web.HttpPostMethod(string.Empty, "http://www.capitalfutures.com.tw/option/default.asp?Sname=TX405_台選W405&xy=1:7", Encoding.Default); 

            //var anchors = DAO.GetOptionDailyCapitalfutures("http://www.capitalfutures.com.tw/option/default.asp?", "201705W4", Encoding.Default, "20170518001155");

            //WeightedDAO WDAO = new WeightedDAO();

            //DAO.GetOptionEveryDay("http://www.capitalfutures.com.tw/option/default.asp?", "https://tw.screener.finance.yahoo.net/future/aa03?fumr=futurepart&opmr=optionpart&opcm=WTXO&opym=");
            //TradeRecordDAO RecordDAO = new TradeRecordDAO();

            //RecordDAO.CalculateResultReport();

            //CalendarDAO DAR = new CalendarDAO();
            

            //DAR.CreateNextMonthCalendar(DateTime.Now);

            //EWallet Wallet = new EWallet();
            //Wallet.CustomerSN = 2;
            //Wallet.Balance = 1000;
            //Wallet.Pyeongchang = 3222;
            //Wallet.Available = 3454;


            TranscationDAO DAO = new TranscationDAO();

            //Transaction Trans = new Transaction();
            //TransactionDetail Detail = new TransactionDetail();

            //Trans.CustomerSN = 1;
            //Trans.TradeType = TranscationTypes.Deposit;
            //Detail.BankAccount = "dsdsadsadsa";
            //Detail.BankName = "ERRR9453";
            //Detail.BranchName = "PKKDDK";
            //Detail.Draw = 70;
            //Detail.Remark = string.Empty;
            //Trans.Detail = Detail;

          List<Transaction> ListTran =   DAO.GetTransactionsByTradeType("2017-07-01", "2017-07-31",10,1,TranscationTypes.Deposit);

            //DAO.GetTransactionByCustomerSN("2017-07-10", "2017-07-11", 1);

            CustomerDAO _CustDAO = new CustomerDAO();
            Customer _Customer = new Customer();
            //ExtraTagDAO _ExtDAO = new ExtraTagDAO();
            //_Customer = _CustDAO.GetCustomerByAccount("dick22707@gmail.com");
            //_Customer.PassWord = "987654321";
            //int result = _ExtDAO.UpdateExtraTag(ExtraUserType.Customer, ExtraClass.MinimunLot, _Customer.SN, (int)MinimunLotLimit.NotLimit);

            //AdviserDAO _AdviserDAO = new AdviserDAO();
            //Adviser _adviser = new Adviser();
            //_adviser.Account = "55441234";
            //_adviser.PassWord = "1234567";
            //_adviser.Address = "cfcdfdggdfgdf";
            //_adviser.BirthDay = DateTime.Now;
            //_adviser.Email = "dick22707@gmail.com";
            //_adviser.FirstName = "First";
            //_adviser.Gender = GenderType.Male;
            //_adviser.ID = "A123456789";
            //_adviser.LastName = "Last";
            //_adviser.Phone = "0953359025";
            //_adviser.Remark = "第一位";

            //int result = _AdviserDAO.AddAdviser(_adviser);

           //Adviser Result =  _AdviserDAO.LoginCheckAdviser(_adviser );


            //TradeRecordDAO _TradeRecordDAO = new TradeRecordDAO();
            //TradeRecord Red = new TradeRecord();
            //Red.Contract = "10400";
            //Red.DueMonth = "201707W4";
            //Red.Lot = "2";
            //Red.OP = "Put";
            //Red.Price = 33.5m;
            //Red.Type = "Sell";


            //_TradeRecordDAO.AddTradeRecord(Red);



            //_CustDAO.UpdatePassWord(_Customer,"123456789");

        }


        private void SendMailByAmazon() {
            string FROM = "dick22707@gmail.com";
            string TO = "erpbank.dick@gmail.com";

            string SUBJECT = "Amazon SES test (SMTP interface accessed using C#)";
            string BODY = "This email was sent through the Amazon SES SMTP interface by using C#.";


        //AKIAIMFFECVXEROV6CZQ
        }
        


        private static void CreateCustomer() {
            CustomerDAO Dao = new CustomerDAO();
            Customer _Customer = new Customer();
            _Customer.PassWord = "9999";
            _Customer.Member = new Member();
            _Customer.Member.FirstName = "DWDWDDW";
            _Customer.Member.LastName = "21221";
            _Customer.Member.NickName = "21cdfds221";
            _Customer.Member.HomeAddr = "231231221";
            _Customer.Member.ID = "A123456789";
            _Customer.Member.Phone = "333333";
            _Customer.Member.BirthDay = DateTime.Now;
            _Customer.Member.Email = "dsadsd@gmail.com";
            Dao.AddCustomer(_Customer);
        }

        private static bool IsQeury { set; get; }

        private static void MainMethod() {
            bool IsGo = true;
            while (IsGo) {
                Console.WriteLine(" Query :q , Add :b ,Sell: s  ,Stop : t , Exit : e ");
                string ReadString = Console.ReadLine();
                string Commad = string.Empty;
                if (!string.IsNullOrEmpty(ReadString)) {
                    string[] Sp = ReadString.Split(' ');
                    if (Sp.Length > 0) {
                        switch (Sp[0].ToLower()) {
                            case "t":
                                IsQeury = false;
                                break;
                            case "e":
                                IsGo = false;
                                break;
                            case "q":
                                IsQeury = true;
                                Task Task1 = Task.Factory.StartNew(() => {
                                    WeekPoint A = new WeekPoint();
                                    WeekPoint B = new WeekPoint();
                                    A.DueMonth = Sp[1];
                                    B.DueMonth = Sp[1];
                                    A.OP = Change(Sp[2]);
                                    A.Contract = Sp[3];
                                    B.OP = Change(Sp[4]);
                                    B.Contract = Sp[5];
                                    ShowNowPrice(A, B);
                                });
                                break;
                            case "b":
                                Console.WriteLine(" DueMonth OP Contract Type Lot Price ");
                                Commad = Console.ReadLine();
                                if (!string.IsNullOrEmpty(Commad)) {
                                    string[] SpCommand = Commad.Split(' ');
                                    if (SpCommand.Length > 0) {
                                        TradeRecord Record = new TradeRecord();
                                        Record.TradeDate = DateTime.Now;
                                        Record.DueMonth = SpCommand[0];
                                        Record.OP = Change(SpCommand[1]);
                                        Record.Contract = SpCommand[2];
                                        Record.Type = Change(SpCommand[3]);
                                        Record.Lot = SpCommand[4];
                                        Record.Price =Convert.ToDecimal(SpCommand[5]);
                                        TradeRecordDAO DAO = new TradeRecordDAO();
                                        DAO.AddTradeRecord(Record);
                                    }
                                }
                                break;
                            case "s":
                                Console.WriteLine(" DueMonth OP Contract Type Lot Price ");
                                 Commad = Console.ReadLine();
                                if (!string.IsNullOrEmpty(Commad)) {
                                    string[] SpCommand = Commad.Split(' ');
                                    if (SpCommand.Length > 0) {
                                        TradeRecordDAO DAO = new TradeRecordDAO();
                                        List<TradeRecord> ListRecord = new List<TradeRecord>();
                                        ListRecord = DAO.GetTradeRecord();
                                        foreach (TradeRecord Record in ListRecord)
                                        {
                                            if (Record.DueMonth.ToLower().Equals(SpCommand[0].ToLower()) && Record.OP.ToLower().Equals(Change( SpCommand[1]).ToLower()) && Record.Contract.Equals(SpCommand[2])) {
                                                Record.IsPyeongchang = true;
                                                Record.StopPrice = Convert.ToDecimal(SpCommand[5]);
                                                Record.PyeongchangTime = DateTime.Now;
                                                DAO.UpdateTradeRecord(Record);
                                            }
                                        }  
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }
                  
        private static string GetSHA256Enc(string Value) {
            byte[] source = Encoding.Default.GetBytes(Value);//將字串轉為Byte[]
            using (SHA256 sha256 = SHA256Managed.Create()) {
                byte[] crypto = sha256.ComputeHash(source);//進行SHA256加密
                return Convert.ToBase64String(crypto);//把加密後的字串從Byte[]轉為字串
            }
        }

        private static string Change(string Str) {
            switch (Str.ToLower()) {
                case "c":
                case "call":
                    return "Call";
                    
                case "put":
                case "p":
                    return "Put";                    
                case "s":
                case "sell":
                    return "Sell";
                case "b":
                case "buy":
                    return "Buy";
                default:
                    return string.Empty;                    
           }
        }

        private static void ShowNowPrice(WeekPoint APoint, WeekPoint BPoint) {
            StockDAO _StockDAO = new StockDAO();
            WeightedDAO _WeightDAO = new WeightedDAO();
            while (IsQeury) {
                DateTime TimeStamp = DateTime.Now;
                TimeSpan StartTimeSpan = TimeStamp.Subtract(new DateTime(TimeStamp.Year, TimeStamp.Month, TimeStamp.Day, 8, 44, 59));
                TimeSpan EndTimeSpan = TimeStamp.Subtract(new DateTime(TimeStamp.Year, TimeStamp.Month, TimeStamp.Day, 23, 45, 15));
                if (StartTimeSpan.TotalSeconds >= 0 && EndTimeSpan.TotalSeconds <= 0) {
                    Console.WriteLine(TimeStamp.ToString("yyyy/MM/dd HH:mm:ss"));
                    Option CallOption = _StockDAO.GetOptionByMonthAndContractAndOP(APoint);
                    Weighted NowWeighted = _WeightDAO.GetWeighted();
                    string StrWeighted = string.Format("Price :{0} , Futurs: {1} , Change : {2} , Volume : {3} ", NowWeighted.Price, NowWeighted.Futures, NowWeighted.Change, NowWeighted.Volume);
                    Console.WriteLine(StrWeighted);
                    string CallString = string.Format("OP :{0} , Contract :{1} , Price :{2} ,Volume :{3} ,Chanage :{4} ", CallOption.OP, CallOption.Contract, CallOption.Clinch, CallOption.Volume, CallOption.Change);
                    Console.WriteLine(CallString);
                    Option PutOption = _StockDAO.GetOptionByMonthAndContractAndOP(BPoint);
                    string PutString = string.Format("OP :{0} , Contract :{1} , Price :{2} ,Volume :{3} ,Chanage :{4} ", PutOption.OP, PutOption.Contract, PutOption.Clinch, PutOption.Volume, PutOption.Change);
                    Console.WriteLine(PutString);
                    System.Threading.Thread.Sleep(1000 * 10);
                }
                else {
                    Console.WriteLine("Not Trade Time");
                    break;
                }
            }
        }
         
        //private static void TaskTest(string DueMonth) {
        //   StockData StockDAO = new StockData();
        //   List<Option> OptionList = new List<Option>();
        //   for (int i = 0; i <= 50;i++ ) {
        //       OptionList.AddRange(StockDAO.GetOptionDaily("https://tw.screener.finance.yahoo.net/future/aa03?fumr=futurepart&amp;opmr=optionpart&amp;opcm=WTXO&amp;opym=", DueMonth, Encoding.UTF8, DateTime.Now.ToString("yyyyMMddHHmmss")));
        //       Console.WriteLine(string.Format("{0}   {1} ",DueMonth,OptionList.Count));
        //   }            
        //}
        
        //private static void TaskTestBBB() {
        //    for (int i = 0; i <= 50; i++) {
        //        Console.WriteLine(string.Format("{0}     {1}   BBB", DateTime.Now.ToString(),i));
        //        Random red = new Random();
        //        System.Threading.Thread.Sleep(100 * red.Next(1, 50));
        //    }
        //}

        /// <summary>取得網站的HtmlDocument</summary>
        /// <param name="ppweburl"></param>
        /// <returns></returns>
        //private static  HtmlDocument GetWebHtmlDocument(string ppweburl, Encoding Encode) {
        //    HttpWebRequest MyHttpWebRequest = HttpWebRequest.Create(ppweburl) as HttpWebRequest;
        //    HttpWebResponse MyHttpWebResponse = MyHttpWebRequest.GetResponse() as HttpWebResponse;
        //    StreamReader myStreamReader = new StreamReader(MyHttpWebResponse.GetResponseStream(), Encode);
        //    HtmlAgilityPack.HtmlDocument _HtmlDocument = new HtmlAgilityPack.HtmlDocument();
        //    _HtmlDocument.LoadHtml(myStreamReader.ReadToEnd());
        //    myStreamReader.Close();
        //    MyHttpWebResponse = null;
        //    MyHttpWebRequest = null;
        //    return _HtmlDocument;
        //}
        
        private static void GetAnalysis(StringBuilder sb, string context, string resul) {
            if (resul.IndexOf("http") == -1) {
                if (resul.IndexOf("<a href") == -1) {
                    string[] array = context.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    bool Into = false;

                    foreach (string str in array) {
                        if (str.IndexOf("class=\"article-meta-tag\">時間</span><span") != -1) {
                            Into = true;
                            continue;
                        }
                        if (str.IndexOf("class=\"f2\">※") != -1) {
                            Into = false;
                        }

                        if (Into) {
                            sb.AppendFormat("{0}\r\n", str);
                        }
                    }
                }
            }
        }

    }

    public class TestAddin : IServiceProviderAddin {
        IServiceEntry[] _serviceEntries;
        public IServiceEntry[] ServiceEntries {
            get {
                return _serviceEntries;
            }
        }
    }

    public class LineMessage {

        public string to { set; get; }

        public List<Message> messages = new List<Message>();
    }
    
    public class Message {
        public string type { set; get; }

        public string text { set; get; }
    }

    public class CRMRequest {
        public string IBCode { set; get; }

        public string feature { set; get; }

        public string timestamp { set; get; }

    }
    
}
