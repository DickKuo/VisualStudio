﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Stock {
    public class CalendarDAO :BaseData{

        private class Default {
            public const string Sunday = "Sunday";
            public const string Saturday = "Saturday";
        }

        private class SP {
            public const string GetCalendarDaily = "GetCalendarDaily";
            public const string SaveCalendar = "SaveCalendar";
            public const string UpdateCalendar = "UpdateCalendar";
            public const string GetDueMonthWeekStart = "GetDueMonthWeekStart";
        }

        private class SPParameter {
            public const string Today = "Today";
            public const string SN = "SN";
            public const string Daily = "Daily";
            public const string IsWorkDay = "IsWorkDay";
            public const string Week = "Week";
            public const string NearMonth1 = "NearMonth1";
            public const string NearMonth2 = "NearMonth2";
            public const string Remark = "Remark";
            public const string IsMaill = "IsMaill"; 
        }

        SQLHelper.UseStoreProcedure USP = new SQLHelper.UseStoreProcedure();

        /// <summary>取得當日的行事曆</summary>
        /// <param name="Today"></param>
        /// <returns></returns>
        public Calendar GetCalendar(DateTime Today) {
            USP.AddParameter(SPParameter.Today, Today);
            DataTable dt = USP.ExeProcedureGetDataTable(SP.GetCalendarDaily);
            Calendar _Calendar = new Calendar();
            if (dt != null && dt.Rows.Count > CommTool.BaseConst.MinItems) {
                DataRow Row = dt.Rows[CommTool.BaseConst.ArrayFirstItem];
                PropertyInfo[] infos = typeof(Calendar).GetProperties();
                foreach (PropertyInfo info in infos) {
                    _Calendar.GetType().GetProperty(info.Name).SetValue(_Calendar, Row[info.Name], null);
                }
                return _Calendar;
            }
            else {
                return null;
            }            
        }

        /// <summary>儲存行事曆</summary>
        /// <param name="_Calendar"></param>
        public void SaveCalendar(Calendar _Calendar) {
            try {
                USP.AddParameter(SPParameter.Daily, _Calendar.Daily);
                USP.AddParameter(SPParameter.IsWorkDay, _Calendar.IsWorkDay);
                USP.AddParameter(SPParameter.Week, _Calendar.Week);
                USP.AddParameter(SPParameter.NearMonth1, _Calendar.NearMonth1);
                USP.AddParameter(SPParameter.NearMonth2, _Calendar.NearMonth2);
                USP.AddParameter(SPParameter.Remark, _Calendar.Remark);
                USP.ExeProcedureNotQuery(SP.SaveCalendar);
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
            }
        }

        /// <summary>產生整年度的行事曆</summary>
        /// <param name="Year"></param>
        public void CreateYearsCalendar(int Year) {
            DateTime Startday = new DateTime(Year, 1, 1);
            DateTime EndDay = new DateTime(Year, 12, 31);
            DateTime NowDay =Startday;
            while (NowDay <= EndDay)
            {			
                Calendar _Calendar = new Calendar();
                _Calendar.Daily = NowDay; 
                if (NowDay.DayOfWeek.ToString() != Default.Sunday && NowDay.DayOfWeek.ToString() != Default.Saturday) {
                    _Calendar.IsWorkDay = true;
                }
                else {
                    _Calendar.IsWorkDay = false;
                } 
                _Calendar.Remark = string.Empty;
                if ((NowDay.Month == 1 && NowDay.Day == 1)) {
                    _Calendar.IsWorkDay = false;
                    _Calendar.Remark = "元但";
                }
                if ((NowDay.Month == 10 && NowDay.Day == 10) ) {
                    _Calendar.IsWorkDay = false;
                    _Calendar.Remark = "雙十國慶";
                }
                if ((NowDay.Month == 2 && NowDay.Day == 28)) {
                    _Calendar.IsWorkDay = false;
                    _Calendar.Remark = "228紀念日";
                }

                SaveCalendar(_Calendar);
                NowDay = NowDay.AddDays(1);
            }
        }

        /// <summary>更新行事曆</summary>
        /// <param name="_Calendar"></param>
        public void UpdateCalendar(Calendar _Calendar) {
            try {
                USP.AddParameter(SPParameter.SN, _Calendar.SN);
                USP.AddParameter(SPParameter.Daily, _Calendar.Daily);
                USP.AddParameter(SPParameter.IsWorkDay, _Calendar.IsWorkDay);
                USP.AddParameter(SPParameter.Week, _Calendar.Week);
                USP.AddParameter(SPParameter.NearMonth1, _Calendar.NearMonth1);
                USP.AddParameter(SPParameter.NearMonth2, _Calendar.NearMonth2);
                USP.AddParameter(SPParameter.Remark, _Calendar.Remark);
                USP.AddParameter(SPParameter.IsMaill, _Calendar.IsMaill);
                USP.ExeProcedureNotQuery(SP.UpdateCalendar);
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
            }
        }

        /// <summary>取得交易第一天</summary>
        /// <param name="DueMonth"></param>
        /// <returns></returns>
        public Calendar GetDueMonthWeekStart(string DueMonth) {
            USP.AddParameter(BaseData.BaseSParameter.DueMonth, DueMonth);
            Calendar _Calendar = USP.ExeProcedureGetObject(SP.GetDueMonthWeekStart,new Calendar ());
            return _Calendar;
        }
    }
}