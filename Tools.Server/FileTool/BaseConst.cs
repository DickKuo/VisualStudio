using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CommTool {
    public class BaseConst {
        public const string Root = "root";
        public const string HtmlFile = ".html";
        public const string XmlFile = ".xml";
        public const string LocalHostIP = "127.0.0.1";
        public const string TxtFile = ".txt";
        public const string DateFormat = "yyyy-MM-dd";
        public const string TimeFormat = "HH:mm:ss";
        public const string TimeFormatComplete = "yyyy/MM/dd HH:mm:ss";
        public const string TimeFormatToMinute = "yyyy/MM/dd HH:mm";
        public const string Title="Title";
        public const string Table = "Table";
        public const string Tr = "tr";
        public const string Dot = ",";
        public const string Div = "Div";
        public const string EmptySpace = "&nbsp;";
        public const string BaseLogPath = "";
        public const int MinItems = 0;
        public const int ArrayMinItems = 0;
        public const int ArrayFirstItem = 0;
        public const int ArraySecondItem = 1;
        public const int ArrayThridItem = 2;
        public const int ArrayFourthItem = 3;
        public const int ArrayFifthItem = 4;
        public const int ArraySixthItem = 5;
        public const int ArraySeventhItem = 6;
        public const int ArrayEithItem = 7;
        public const int ArrayNinethItem = 8;
        public const int ArrayTenthItem = 9;
        public const string DServiceConfig = "DService.exe.config";
        public const string Wednesday = "Wednesday";


        [MethodImpl(MethodImplOptions.NoInlining)]
        protected string GetCurrentMethod() {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            return sf.GetMethod().Name;
        }


    }
}
