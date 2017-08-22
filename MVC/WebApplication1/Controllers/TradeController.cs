using Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Code.Helpers;
using WebApplication1.Models;
using WebApplication1.Models.Code;

namespace WebApplication1.Controllers
{
    public class TradeController : BaseLoginController
    {
        /// <summary></summary>
        /// <returns></returns>
        public ActionResult Quotes(string DueMonth)
        {            
            DateTime NowTime = DateTime.Now;
            DateTime TimeEnd = NowTime.AddDays(1);
            TradeViewModels.TradeViewModel ViewModel = new TradeViewModels.TradeViewModel();
            CalendarDAO _CalendarDAO = new CalendarDAO();
            Calendar _Calendar = _CalendarDAO.GetCalendar(DateTime.Now);
            ViewBag.DueMonthList = GetDueMonthItems(_Calendar);
            ViewModel.BeginTime = NowTime;
            ViewModel.EndTime = TimeEnd;
            ViewModel.Page = 1;
            ViewModel.DueMonth = string.IsNullOrEmpty(DueMonth) == false ? DueMonth : _Calendar.Week;
            ViewModel = SearchQuotes(ViewModel);
            return View(ViewModel);
        }//end Quotes

        public ActionResult QuotesSearch(TradeViewModels.TradeViewModel ViewModel) {
           TradeViewModels.TradeViewModel ResultView = new TradeViewModels.TradeViewModel();
           DateTime NowTime = DateTime.Now;
           DateTime TimeEnd = NowTime.AddDays(1);
           ViewModel.BeginTime = NowTime;
           ViewModel.EndTime = TimeEnd;
           ViewModel.Page = 1;
           ResultView = SearchQuotes(ViewModel);
           return PartialView("_QuotesTable", ResultView);
        }


        /// <summary></summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public TradeViewModels.TradeViewModel SearchQuotes(TradeViewModels.TradeViewModel ViewModel) {
            Stock.StockDAO Stock = new Stock.StockDAO();
            List<Option> OptionList = Stock.GetOptionQuotesByDuMonthAndTime(ViewModel.BeginTime, ViewModel.EndTime, ViewModel.DueMonth);
            ViewModel.QuotesList = new List<Models.Code.OptionQuotes>();
            foreach (Option OP in OptionList) {
                List<OptionQuotes> Quotes = ViewModel.QuotesList.Where(x => x.Contract == OP.Contract).ToList();
                if (Quotes.Count > 0) {
                    Quotes[0].Contract = OP.Contract;
                    Quotes[0].Time = OP.Time;
                    Quotes[0].DueMonth = OP.DueMonth;
                    if (OP.OP == "Call") {
                        Quotes[0].CallBuy = OP.Buy.ToString();
                        Quotes[0].CallSell = OP.Sell.ToString();
                        Quotes[0].CallClinch = OP.Clinch.ToString();
                        Quotes[0].CallChagne = OP.Change.ToString();
                        Quotes[0].CallVolume = OP.Volume.ToString();
                    }
                    else {
                        Quotes[0].PutBuy = OP.Buy.ToString();
                        Quotes[0].PutSell = OP.Sell.ToString();
                        Quotes[0].PutClinch = OP.Clinch.ToString();
                        Quotes[0].PutChagne = OP.Change.ToString();
                        Quotes[0].PutVolume = OP.Volume.ToString();
                    }
                }
                else {
                    OptionQuotes OPQuotes = new OptionQuotes();
                    OPQuotes.Contract = OP.Contract;
                    OPQuotes.Time = OP.Time;
                    OPQuotes.DueMonth = OP.DueMonth;
                    if (OP.OP == "Call") {
                        OPQuotes.CallBuy = OP.Buy.ToString();
                        OPQuotes.CallSell = OP.Sell.ToString();
                        OPQuotes.CallClinch = OP.Clinch.ToString();
                        OPQuotes.CallChagne = OP.Change.ToString();
                        OPQuotes.CallVolume = OP.Volume.ToString();
                    }
                    else {
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
        }//end SearchQuotes

        /// <summary>下單</summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public dynamic AddTrade(TradeViewModels.TradeViewModel ViewModel) {
            TradeRecordDAO _TradDAO = new TradeRecordDAO();
            TradeRecord _TradeRecord = new TradeRecord();
            _TradeRecord.Contract = ViewModel.Contract;
            _TradeRecord.DueMonth = ViewModel.DueMonth;
            _TradeRecord.Lot = ViewModel.Lot;
            _TradeRecord.OP = ViewModel.Op;
            _TradeRecord.Type = ViewModel.Type;
            _TradeRecord.Price = Convert.ToDecimal(ViewModel.Clinch);
            _TradeRecord.CustomerSN = LoginHelper.GetLoginInfo().Customer.SN;
            int Result = 0;
            Result = _TradDAO.AddTradeRecord(_TradeRecord);
            if (Result > 0) {
                return "OK";
            }
            else {
                return "Error";
            }
        }//end AddTrade


        /// <summary>取得交易結算週期</summary>
        /// <returns></returns>
        public List<SelectListItem> GetDueMonthItems(Calendar _Calendar) { 
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = _Calendar.Week, Value = _Calendar.Week, Selected = true });
            items.Add(new SelectListItem { Text = _Calendar.NearMonth1, Value = _Calendar.NearMonth1 });
            items.Add(new SelectListItem { Text = _Calendar.NearMonth2, Value = _Calendar.NearMonth2 });
            return items;
        }//end  GetDueMonthItems   

    }
}
