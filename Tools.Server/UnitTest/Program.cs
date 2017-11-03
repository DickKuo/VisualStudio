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
            //TranscationDAO DAO = new TranscationDAO();
            //CustomerDAO _CustDAO = new CustomerDAO();
            //Customer _Customer = new Customer();

            TradeRecordDAO TradDB = new TradeRecordDAO();

            TradDB.CalculateResultReport();
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
