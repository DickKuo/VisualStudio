using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using HtmlAgilityPack;
using System.Text;
using System.Data;
using System.Linq;
using System;

namespace Stock
{
    public class CalendarDAO : BaseData
    {
        private class Default
        {
            public const string Sunday = "Sunday";
            public const string Saturday = "Saturday";
            public const string ContractTimeFormat = "yyyyMM";
            public const string Friday = "Friday";
            public const string Thursday = "Thursday";
            public const string Wednesday = "Wednesday";
        }

        private class SP
        {
            public const string GetCalendarDaily = "GetCalendarDaily";
            public const string SaveCalendar = "SaveCalendar";
            public const string UpdateCalendar = "UpdateCalendar";
            public const string GetDueMonthWeekStart = "GetDueMonthWeekStart";
            public const string GetDueMonthWeekLasDay = "GetDueMonthWeekLasDay";
            public const string GetWeeks = "GetWeeks";
            public const string GetCalendarByMonth = "GetCalendarByMonth";
            public const string StoreToHistory = "StoreToHistory";
        }

        private class SPParameter
        {
            public const string Today = "Today";
            public const string SN = "SN";
            public const string Daily = "Daily";
            public const string IsWorkDay = "IsWorkDay";
            public const string Week = "Week";
            public const string NearMonth1 = "NearMonth1";
            public const string NearMonth2 = "NearMonth2";
            public const string Remark = "Remark";
            public const string IsSettlement = "IsSettlement";
            public const string IsMaill = "IsMaill";
        }

        /// <summary>建立行事曆</summary>
        /// <param name="ThisMonth"></param>
        public void CreateNextMonthCalendar(DateTime ThisMonth)
        {
            List<Calendar> NodeList = new List<Calendar>();
            for (int i = 1; i <= 2; i++)
            {
                DateTime NextMonth = ThisMonth.AddMonths(i);
                WebInfo.WebInfo Web = new WebInfo.WebInfo();
                HtmlNodeCollection Collection = Web.GetWebHtmlDocumentNodeCollection(string.Format("http://www.beawms.com.tw/Futures/futures_3/{0}/{1}", NextMonth.Year, NextMonth.Month), "//div[@class='position']", Encoding.UTF8);
                if (Collection != null)
                {
                    string WeekOption = string.Empty;
                    foreach (var Item in Collection)
                    {
                        string DayTime = Item.ChildNodes[1].ChildNodes[1].InnerText;
                        DayTime = DayTime.Trim().Replace("(六)", string.Empty).Replace("(日)", string.Empty).Replace("(一)", string.Empty).Replace("(二)", string.Empty).Replace("(三)", string.Empty).Replace("(四)", string.Empty).Replace("(五)", string.Empty);
                        DateTime _CalendarDay = Convert.ToDateTime(DayTime);
                        Calendar _Calendar = GetCalendar(_CalendarDay);
                        if (Item.ChildNodes[1].ChildNodes[1].InnerText.IndexOf("(六)") != -1 || Item.ChildNodes[1].ChildNodes[1].InnerText.IndexOf("(日)") != -1)
                        {
                            _Calendar.IsWorkDay = false;
                        }
                        else
                        {
                            _Calendar.IsWorkDay = true;
                        }
                        if (!string.IsNullOrEmpty(Item.ChildNodes[1].ChildNodes[3].InnerText.Trim()))
                        {
                            WeekOption = Item.ChildNodes[1].ChildNodes[3].InnerText.Replace("結算日", string.Empty);
                            WeekOption = WeekOption.Replace("&nbsp;臺指選擇權一週到期契約及小型臺指期貨一週到期契約之最後交易日", string.Empty);
                            WeekOption = WeekOption.Replace("選擇權契約、股票期貨契約、股票選擇權契約之最後交易日", string.Empty);
                            WeekOption = WeekOption.Replace("股價指數期貨契約、股價指數", string.Empty);
                            WeekOption = WeekOption.Replace("其他市場", string.Empty);
                            WeekOption = WeekOption.Trim();
                            WeekOption = WeekOption.Replace("/", string.Empty).Trim();
                            WeekOption = WeekOption.Replace("-", string.Empty).Trim();
                            string Temp = string.Format("{0}&nbsp;摩根臺指期貨最後交易日", _Calendar.Daily.ToString(Default.ContractTimeFormat));
                            WeekOption = WeekOption.Replace(Temp, string.Empty);
                            WeekOption = WeekOption.Trim();
                            _Calendar.Remark = WeekOption;
                        }
                        NodeList.Add(_Calendar);
                    }
                }

                List<Calendar> NewNodeList = NodeList.OrderByDescending(o => o.Daily).ToList();
                string Week = string.Empty;
                string NearMonth1 = string.Empty;
                string NearMonth2 = string.Empty;
                foreach (Calendar Item in NewNodeList)
                {
                    if (Week.IndexOf("W1") == -1 && Week.IndexOf("W2") == -1)
                    {
                        NearMonth1 = Item.Daily.AddMonths(1).ToString(Default.ContractTimeFormat);
                        NearMonth2 = Item.Daily.AddMonths(2).ToString(Default.ContractTimeFormat);
                        Item.NearMonth1 = NearMonth1;
                        Item.NearMonth2 = NearMonth2;
                    }
                    else
                    {
                        Item.NearMonth1 = Week.Replace("W1", string.Empty).Replace("W2", string.Empty);
                        Item.NearMonth2 = Item.Daily.AddMonths(1).ToString(Default.ContractTimeFormat);
                    }

                    if (!string.IsNullOrEmpty(Item.Remark))
                    {
                        if (Item.Remark.IndexOf("休市") == -1 && Week.IndexOf("元但") == -1)
                        {
                            Item.NearMonth2 = Item.NearMonth1;
                            Item.NearMonth1 = Week;
                            Week = Item.Remark;
                        }
                        else
                        {
                            Item.IsWorkDay = false;
                        }
                    }

                    if (!string.IsNullOrEmpty(Week))
                    {
                        Item.Week = Week;
                    }

                    if (Item.SN > 0)
                    {
                        UpdateCalendar(Item);
                    }
                    else
                    {
                        SaveCalendar(Item);
                    }
                }
            }
        }

        /// <summary>取得當日的行事曆</summary>
        /// <param name="Today"></param>
        /// <returns></returns>
        public Calendar GetCalendar(DateTime Today)
        {
            USP.AddParameter(SPParameter.Today, Today);
            DataTable dt = USP.ExeProcedureGetDataTable(SP.GetCalendarDaily);
            Calendar _Calendar = new Calendar();
            if (dt != null && dt.Rows.Count > CommTool.BaseConst.MinItems)
            {
                DataRow Row = dt.Rows[CommTool.BaseConst.ArrayFirstItem];
                PropertyInfo[] infos = typeof(Calendar).GetProperties();
                foreach (PropertyInfo info in infos)
                {
                    _Calendar.GetType().GetProperty(info.Name).SetValue(_Calendar, Row[info.Name], null);
                }
                return _Calendar;
            }
            else
            {
                return null;
            }
        }

        /// <summary>儲存行事曆</summary>
        /// <param name="_Calendar"></param>
        public void SaveCalendar(Calendar _Calendar)
        {
            try
            {
                USP.AddParameter(SPParameter.Daily, _Calendar.Daily);
                USP.AddParameter(SPParameter.IsWorkDay, _Calendar.IsWorkDay);
                USP.AddParameter(SPParameter.Week, _Calendar.Week);
                USP.AddParameter(SPParameter.NearMonth1, _Calendar.NearMonth1);
                USP.AddParameter(SPParameter.NearMonth2, _Calendar.NearMonth2);
                USP.AddParameter(SPParameter.Remark, _Calendar.Remark);
                USP.AddParameter(SPParameter.IsSettlement, _Calendar.IsSettlement);
                USP.ExeProcedureNotQuery(SP.SaveCalendar);
            }
            catch (Exception ex)
            {
                CommTool.ToolLog.Log(ex);
            }
        }

        /// <summary>產生整年度的行事曆</summary>
        /// <param name="Year"></param>
        public void CreateYearsCalendar(int Year)
        {
            DateTime Startday = new DateTime(Year, 01, 01);
            DateTime EndDay = new DateTime(Year, 12, 31);
            DateTime NowDay = Startday;
            bool IsNextWorkDay = true;
            bool IsNextSettlement = false;
            string WeekString = string.Empty;

            while (NowDay <= EndDay)
            {
                Calendar _Calendar = new Calendar();
                _Calendar.Daily = NowDay;
                _Calendar.Remark = string.Empty;               
                
                if (NowDay.DayOfWeek.ToString() != Default.Sunday && NowDay.DayOfWeek.ToString() != Default.Saturday)
                {
                    _Calendar.IsWorkDay = true;
                }
                else
                {
                    _Calendar.IsWorkDay = false;
                }

                if (IsNextWorkDay == false)
                {
                    _Calendar.IsWorkDay = false;
                    IsNextWorkDay = true;
                    _Calendar.Remark = "彈性放假";
                }

                _Calendar.Remark = string.Empty;
                if ((NowDay.Month == 1 && NowDay.Day == 1))
                {
                    _Calendar.IsWorkDay = false;
                    _Calendar.Remark = "元但";
                    if (NowDay.DayOfWeek.ToString() == Default.Thursday)
                    {
                        IsNextWorkDay = false;
                    }
                }

                if ((NowDay.Month == 2 && NowDay.Day == 28))
                {
                    _Calendar.IsWorkDay = false;
                    _Calendar.Remark = "228紀念日";
                    if (NowDay.DayOfWeek.ToString() == Default.Thursday)
                    {
                        IsNextWorkDay = false;
                    }
                }

                if ((NowDay.Month == 5 && NowDay.Day == 1))
                {
                    _Calendar.IsWorkDay = false;
                    _Calendar.Remark = "勞動節";
                    if (NowDay.DayOfWeek.ToString() == Default.Thursday)
                    {
                        IsNextWorkDay = false;
                    }
                }

                if ((NowDay.Month == 10 && NowDay.Day == 10))
                {
                    _Calendar.IsWorkDay = false;
                    _Calendar.Remark = "雙十國慶";
                    if (NowDay.DayOfWeek.ToString() == Default.Thursday)
                    {
                        IsNextWorkDay = false;
                    }
                }

                if (IsNextSettlement)
                {
                    _Calendar.IsSettlement = true;
                    IsNextSettlement = false;
                }

                if (NowDay.DayOfWeek.ToString() == Default.Wednesday)
                {
                    if (!_Calendar.IsWorkDay)
                    {
                        _Calendar.IsSettlement = false;
                        IsNextSettlement = true;
                    }
                    else
                    {
                        _Calendar.IsSettlement = true;
                        IsNextSettlement = false;
                    }
                }
                                                
                int Weeks = getWeekNumInMonth(NowDay); 

                if (string.IsNullOrEmpty(WeekString))
                {
                    switch (Weeks)
                    {
                        case 3:
                            _Calendar.Week = string.Format("{0}{1}", NowDay.Year, NowDay.Month.ToString("00"));
                            break;
                        case 5:
                            WeekString = string.Format("{0}{1}W{2}", NowDay.Year, NowDay.AddMonths(1).Month.ToString("00"), 1);
                            break;
                        default:
                            WeekString = string.Format("{0}{1}W{2}", NowDay.Year, NowDay.Month.ToString("00"), Weeks);
                            break;
                    }
                }

                _Calendar.Week = WeekString;
                DateTime NextMonth = NowDay.AddMonths(1);
                DateTime NextTwoMonth = NowDay.AddMonths(2);
                string[] arr = WeekString.Split('W');

                switch (Weeks)
                {
                    case 1:
                        if ((_Calendar.IsSettlement || IsNextSettlement) & _Calendar.IsWorkDay)
                        {
                            _Calendar.NearMonth1 = string.Format("{0}{1}W{2}", NowDay.Year, NowDay.Month.ToString("00"), (Weeks + 1).ToString());
                            _Calendar.NearMonth2 = string.Format("{0}{1}", NowDay.Year, NowDay.Month.ToString("00"));
                            WeekString = _Calendar.NearMonth1;
                        }
                        else
                        {
                            _Calendar.NearMonth1 = string.Format("{0}{1}", NowDay.Year, NowDay.Month.ToString("00"));
                            _Calendar.NearMonth2 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                        }
                        break;

                    case 2:                    
                        if ((_Calendar.IsSettlement || IsNextSettlement) & _Calendar.IsWorkDay)
                        {                          
                            if (arr.Length > 1)
                            {
                                if (Convert.ToInt32(arr[1]) == 1)
                                {
                                    _Calendar.NearMonth1 = string.Format("{0}{1}W{2}", NowDay.Year, NowDay.Month.ToString("00"), Weeks.ToString());
                                    _Calendar.NearMonth2 = string.Format("{0}{1}", NowDay.Year, NowDay.Month.ToString("00"));
                                    WeekString = _Calendar.NearMonth1;
                                }
                                else
                                {
                                    _Calendar.NearMonth1 = string.Format("{0}{1}", NowDay.Year, NowDay.Month.ToString("00"));
                                    _Calendar.NearMonth2 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                                    WeekString = _Calendar.NearMonth1;
                                }
                            }
                            else
                            {
                                _Calendar.NearMonth1 = string.Format("{0}{1}W{2}", NowDay.Year, NowDay.Month.ToString("00"), (Weeks + 1).ToString());
                                _Calendar.NearMonth2 = string.Format("{0}{1}", NowDay.Year, NowDay.Month.ToString("00"));
                            }
                        }
                        else
                        {
                            _Calendar.NearMonth1 = string.Format("{0}{1}", NowDay.Year, NowDay.Month.ToString("00"));
                            _Calendar.NearMonth2 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                            if (_Calendar.Week == _Calendar.NearMonth1)
                            {
                                _Calendar.NearMonth1 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                                _Calendar.NearMonth2 = string.Format("{0}{1}", NextTwoMonth.Year, NextTwoMonth.Month.ToString("00"));
                            }                            
                        }
                        break;

                    case 3:
                      
                        if ((_Calendar.IsSettlement || IsNextSettlement) & _Calendar.IsWorkDay)
                        {                         
                            if (arr.Length > 1)
                            {
                                if (Convert.ToInt32(arr[1]) == 2)
                                {
                                    _Calendar.NearMonth1 = string.Format("{0}{1}", NowDay.Year, NowDay.Month.ToString("00"));
                                    _Calendar.NearMonth2 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                                    WeekString = _Calendar.NearMonth1;
                                }
                                else
                                {
                                    _Calendar.NearMonth1 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                                    _Calendar.NearMonth2 = string.Format("{0}{1}", NextTwoMonth.Year, NextTwoMonth.Month.ToString("00"));
                                    WeekString = _Calendar.NearMonth1;
                                }
                            }                           
                            else
                            {
                                _Calendar.NearMonth1 = string.Format("{0}{1}W{2}", NowDay.Year, NowDay.Month.ToString("00"), (Weeks + 1).ToString());
                                _Calendar.NearMonth2 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                                WeekString = _Calendar.NearMonth1;
                            }
                        }
                        else
                        {
                            if (arr.Length > 1)
                            {
                                if (Convert.ToInt32(arr[1]) == 2)
                                {
                                    _Calendar.NearMonth1 = string.Format("{0}{1}", NowDay.Year, NowDay.Month.ToString("00"));
                                    _Calendar.NearMonth2 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                                }
                                else
                                {
                                    _Calendar.NearMonth1 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                                    _Calendar.NearMonth2 = string.Format("{0}{1}", NextTwoMonth.Year, NextTwoMonth.Month.ToString("00"));
                                }                                  
                            }
                            else
                            {
                                _Calendar.NearMonth1 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                                _Calendar.NearMonth2 = string.Format("{0}{1}", NextTwoMonth.Year, NextTwoMonth.Month.ToString("00"));
                                if (_Calendar.Week == _Calendar.NearMonth1)
                                {
                                    _Calendar.NearMonth1 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                                    _Calendar.NearMonth2 = string.Format("{0}{1}", NextTwoMonth.Year, NextTwoMonth.Month.ToString("00"));
                                }
                            }
                        }
                        break;

                    case 4:
                        if ((_Calendar.IsSettlement || IsNextSettlement) & _Calendar.IsWorkDay)
                        {
                            if (arr.Length > 1)
                            {
                                if (Convert.ToInt32(arr[1]) == 4)
                                {
                                    if (_Calendar.Daily.Month == _Calendar.Daily.AddDays(7).Month)
                                    {
                                        _Calendar.NearMonth1 = string.Format("{0}{1}W{2}", NowDay.Year, NowDay.Month.ToString("00"), (Weeks + 1).ToString());
                                        _Calendar.NearMonth2 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                                        WeekString = _Calendar.NearMonth1;
                                    }
                                    else
                                    {
                                        _Calendar.NearMonth1 = string.Format("{0}{1}W{2}", NextMonth.Year, NextMonth.Month.ToString("00"), (1).ToString());
                                        _Calendar.NearMonth2 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                                        WeekString = _Calendar.NearMonth1;
                                    }
                                }
                            }
                            else
                            {
                                _Calendar.NearMonth1 = string.Format("{0}{1}W{2}", NowDay.Year, NowDay.Month.ToString("00"), Weeks.ToString());
                                _Calendar.NearMonth2 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                                WeekString = _Calendar.NearMonth1;
                            }
                        }
                        else
                        {
                            _Calendar.NearMonth1 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                            _Calendar.NearMonth2 = string.Format("{0}{1}", NextTwoMonth.Year, NextTwoMonth.Month.ToString("00"));
                            if (_Calendar.Week == _Calendar.NearMonth1)
                            {
                                _Calendar.NearMonth1 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                                _Calendar.NearMonth2 = string.Format("{0}{1}", NextTwoMonth.Year, NextTwoMonth.Month.ToString("00"));
                            }
                        }
                        break;

                    default:
                        if ((_Calendar.IsSettlement || IsNextSettlement) & _Calendar.IsWorkDay)
                        {
                           
                            _Calendar.NearMonth1 = string.Format("{0}{1}W{2}", NextMonth.Year, NextMonth.Month.ToString("00"), (1).ToString());
                            _Calendar.NearMonth2 = string.Format("{0}{1}", NextTwoMonth.Year, NextTwoMonth.Month.ToString("00"));
                            WeekString = _Calendar.NearMonth1;
                        }
                        else
                        {
                            _Calendar.NearMonth1 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                            _Calendar.NearMonth2 = string.Format("{0}{1}", NextTwoMonth.Year, NextTwoMonth.Month.ToString("00"));
                            if (_Calendar.Week == _Calendar.NearMonth1)
                            {
                                _Calendar.NearMonth1 = string.Format("{0}{1}", NextMonth.Year, NextMonth.Month.ToString("00"));
                                _Calendar.NearMonth2 = string.Format("{0}{1}", NextTwoMonth.Year, NextTwoMonth.Month.ToString("00"));
                            }
                        }
                        break;
                }                                

                SaveCalendar(_Calendar);
                NowDay = NowDay.AddDays(1);
            }
        }
        
        private int getWeekNumInMonth(DateTime daytime)
        {
            int dayInMonth = daytime.Day;
            //本月第一天  
            DateTime firstDay = daytime.AddDays(1 - daytime.Day);
            //本月第一天是周幾  
            int weekday = (int)firstDay.DayOfWeek == 0 ? 7 : (int)firstDay.DayOfWeek;
            //本月第一周有幾天  
            int firstWeekEndDay = 7 - (weekday - 1);
            //當前日期和第一周之差  
            int diffday = dayInMonth - firstWeekEndDay;
            diffday = diffday > 0 ? diffday : 1;
            //當前是第幾周,如果整除7就減一天  
            int WeekNumInMonth = ((diffday % 7) == 0
             ? (diffday / 7 - 1)
             : (diffday / 7)) + 1 + (dayInMonth > firstWeekEndDay ? 1 : 0);
            return WeekNumInMonth ;
        }

        /// <summary>更新行事曆</summary>
        /// <param name="_Calendar"></param>
        public void UpdateCalendar(Calendar _Calendar)
        {
            try
            {
                USP.AddParameter(SPParameter.SN, _Calendar.SN);
                USP.AddParameter(SPParameter.Daily, _Calendar.Daily);
                USP.AddParameter(SPParameter.IsWorkDay, _Calendar.IsWorkDay);
                USP.AddParameter(SPParameter.Week, _Calendar.Week);
                USP.AddParameter(SPParameter.NearMonth1, _Calendar.NearMonth1);
                USP.AddParameter(SPParameter.NearMonth2, _Calendar.NearMonth2);
                USP.AddParameter(SPParameter.Remark, _Calendar.Remark);
                USP.AddParameter(SPParameter.IsMaill, _Calendar.IsMaill);
                USP.AddParameter(SPParameter.IsSettlement, _Calendar.IsSettlement);
                USP.ExeProcedureNotQuery(SP.UpdateCalendar);
            }
            catch (Exception ex)
            {
                CommTool.ToolLog.Log(ex);
            }
        }

        /// <summary>取得交易契約的第一天</summary>
        /// <param name="DueMonth"></param>
        /// <returns></returns>
        public Calendar GetDueMonthWeekStart(string DueMonth)
        {
            USP.AddParameter(BaseData.BaseSParameter.DueMonth, DueMonth);
            Calendar _Calendar = USP.ExeProcedureGetObject(SP.GetDueMonthWeekStart, new Calendar());
            return _Calendar;
        }

        /// <summary>取得交易契約的最後一天</summary>
        /// <param name="DueMonth"></param>
        /// <returns></returns>
        public Calendar GetDueMonthWeekLasDay(string DueMonth)
        {
            USP.AddParameter(BaseData.BaseSParameter.DueMonth, DueMonth);
            Calendar _Calendar = USP.ExeProcedureGetObject(SP.GetDueMonthWeekLasDay, new Calendar());
            return _Calendar;
        }

        /// <summary></summary>
        /// <returns></returns>
        public List<string> GetWeeks()
        {
            List<string> ListResult = new List<string>();
            USP.AddParameter(BaseData.BaseSParameter.EndDate, DateTime.Now.ToString(BaseData.BaseSParameter.DataTimeFormat));
            DataTable dt = USP.ExeProcedureGetDataTable(SP.GetWeeks);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string re = dr[0].ToString();
                    ListResult.Add(re);
                }
            }
            return ListResult;
        }

        /// <summary>取得行事曆月份</summary>
        /// <param name="DayTime"></param>
        /// <returns></returns>
        public List<Calendar> GetCalendarByMonth(DateTime DayTime)
        {
            DateTime BeginDate = new DateTime(DayTime.Year, DayTime.Month, 1);
            DateTime EndDate = BeginDate.AddMonths(1).AddDays(-1);
            USP.AddParameter(BaseData.BaseSParameter.BeginDate, BeginDate);
            USP.AddParameter(BaseData.BaseSParameter.EndDate, EndDate);
            List<Calendar> _ListCalendar = USP.ExeProcedureGetObjectList(SP.GetCalendarByMonth, new Calendar());
            return _ListCalendar;
        }

        /// <summary>將資料儲存到歷史資料庫內</summary>
        public void StoreToHistory()
        {
            USP.ExeProcedureGetDataTable(SP.StoreToHistory);
        }
    }
}