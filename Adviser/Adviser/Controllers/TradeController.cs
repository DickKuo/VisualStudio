using System.Collections.Generic;
using Adviser.Models.ViewModels;
using Adviser.Models.Code;
using Adviser.Helpers;
using System.Web.Mvc;
using System.Linq;
using System;
using Stock;

namespace Adviser.Controllers
{
    public class TradeController : BaseLoginController
    {
        /// <summary>報價表</summary>
        /// <returns></returns>
        public ActionResult Quotes(TradeViewModels.TradeViewModel ViewModel)
        {
            if (ViewModel == null)
            {
                ViewModel = new TradeViewModels.TradeViewModel();
            }
            DateTime NowTime = DateTime.Now;
            DateTime TimeEnd = NowTime.AddDays(1);
            CalendarDAO _CalendarDAO = new CalendarDAO();
            Calendar _Calendar = _CalendarDAO.GetCalendar(DateTime.Now);
            ViewBag.DueMonthList = GetDueMonthItems(_Calendar);
            ViewModel.BeginTime = NowTime;
            ViewModel.EndTime = TimeEnd;
            ViewModel.Page = 1;
            ViewModel.DueMonth = string.IsNullOrEmpty(ViewModel.DueMonth) == false ? ViewModel.DueMonth : _Calendar.Week;
            ViewModel = SearchQuotes(ViewModel);
            if (ViewModel.IsPartial)
            {
                if (ChkIsMobile())
                {
                    return PartialView("../Trade/_QuotesTableM", ViewModel);
                }
                else
                {
                    return PartialView("../Trade/_QuotesTable", ViewModel);
                }
            }
            else
            {
                return Views(ViewModel);
            }
        }

        /// <summary></summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public TradeViewModels.TradeViewModel SearchQuotes(TradeViewModels.TradeViewModel ViewModel)
        {
            StockDAO Stock = new StockDAO();
            List<Option> OptionList = Stock.GetOptionQuotesByDuMonthAndTime(ViewModel.BeginTime, ViewModel.EndTime, ViewModel.DueMonth);
            ViewModel.QuotesList = new List<OptionQuotes>();
            foreach (Option OP in OptionList)
            {
                List<OptionQuotes> Quotes = ViewModel.QuotesList.Where(x => x.Contract == OP.Contract).ToList();
                if (Quotes.Count > 0)
                {
                    Quotes[0].Contract = OP.Contract;
                    Quotes[0].Time = OP.Time;
                    Quotes[0].DueMonth = OP.DueMonth;
                    if (OP.OP == "Call")
                    {
                        Quotes[0].CallBuy = OP.Buy.ToString();
                        Quotes[0].CallSell = OP.Sell.ToString();
                        Quotes[0].CallClinch = OP.Clinch.ToString();
                        Quotes[0].CallChagne = OP.Change.ToString();
                        Quotes[0].CallVolume = OP.Volume.ToString();
                    }
                    else
                    {
                        Quotes[0].PutBuy = OP.Buy.ToString();
                        Quotes[0].PutSell = OP.Sell.ToString();
                        Quotes[0].PutClinch = OP.Clinch.ToString();
                        Quotes[0].PutChagne = OP.Change.ToString();
                        Quotes[0].PutVolume = OP.Volume.ToString();
                    }
                }
                else
                {
                    OptionQuotes OPQuotes = new OptionQuotes();
                    OPQuotes.Contract = OP.Contract;
                    OPQuotes.Time = OP.Time;
                    OPQuotes.DueMonth = OP.DueMonth;
                    if (OP.OP == "Call")
                    {
                        OPQuotes.CallBuy = OP.Buy.ToString();
                        OPQuotes.CallSell = OP.Sell.ToString();
                        OPQuotes.CallClinch = OP.Clinch.ToString();
                        OPQuotes.CallChagne = OP.Change.ToString();
                        OPQuotes.CallVolume = OP.Volume.ToString();
                    }
                    else
                    {
                        OPQuotes.PutBuy = OP.Buy.ToString();
                        OPQuotes.PutSell = OP.Sell.ToString();
                        OPQuotes.PutClinch = OP.Clinch.ToString();
                        OPQuotes.PutChagne = OP.Change.ToString();
                        OPQuotes.PutVolume = OP.Volume.ToString();
                    }
                    ViewModel.QuotesList.Add(OPQuotes);
                }
            }
            return ViewModel;
        }

        /// <summary>下單</summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public dynamic AddTrade(TradeViewModels.TradeViewModel ViewModel)
        {
            TradeRecordDAO _TradDAO = new TradeRecordDAO();
            TradeRecord _TradeRecord = new TradeRecord();
            _TradeRecord.Contract = ViewModel.Contract;
            _TradeRecord.DueMonth = ViewModel.DueMonth;
            _TradeRecord.Lot = ViewModel.Lot;
            _TradeRecord.OP = ViewModel.Op;
            _TradeRecord.Type = ViewModel.Type;
            _TradeRecord.Price = Convert.ToDecimal(ViewModel.Clinch);
            _TradeRecord.AdviserSN = LoginHelper.GetLoginInfo().Adviser.SN;
            int Result = 0;
            Result = _TradDAO.AddTradeRecord(_TradeRecord);
            if (Result > 0)
            {
                return "OK";
            }
            else
            {
                return "Error";
            }
        }

        /// <summary>取得交易結算週期</summary>
        /// <returns></returns>
        public List<SelectListItem> GetDueMonthItems(Calendar _Calendar)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = _Calendar.Week, Value = _Calendar.Week, Selected = true });
            items.Add(new SelectListItem { Text = _Calendar.NearMonth1, Value = _Calendar.NearMonth1 });
            items.Add(new SelectListItem { Text = _Calendar.NearMonth2, Value = _Calendar.NearMonth2 });
            return items;
        }
    }
}