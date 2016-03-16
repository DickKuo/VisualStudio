using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.Code;

namespace WebApplication1.Controllers
{
    public class ChartController : Controller
    {

        Dictionary<Currencies.Currency, Dictionary<Currencies.Currency, double[]>> MainCurrency = new Dictionary<Currencies.Currency, Dictionary<Currencies.Currency, double[]>>();

     


        private class Default {
            public const string DateFormat = "MM/dd";
        }

        //
        // GET: /Chart/
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>建立圖表資料</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult GenerateData(WebApplication1.Models.ChartViewModels.ChartViewMode Model)
        {
            InitCurrencyTable();          
            DateTime Temp = Model.BeginDate;          
            List<string> DateList = new List<string>();
            List<double[]> Exchange = new List<double[]>();
            Dictionary<Currencies.Currency, double[]> Currency = MainCurrency[Model.MainCurrency];
            Dictionary<Currencies.Currency, List<double>> Result = new Dictionary<Currencies.Currency, List<double>>();
            while (Temp <= Model.EndDate)
            {
                DateList.Add(Temp.ToString(Default.DateFormat));
                Temp = Temp.AddDays(1);

                foreach (var Item in Model.SubCurrency)
                {
                    if (!Result.ContainsKey(Item))
                    {
                        double[] temp = Currency[Item];
                        List<double> Values = new List<double>();
                        Values.Add(GetRandomNumber(temp[0], temp[1]));
                        Result.Add(Item, Values);
                    }
                    else
                    {
                        List<double> Values = Result[Item];
                        double[] temp = Currency[Item];
                        Values.Add(GetRandomNumber(temp[0], temp[1]));
                        Result[Item] = Values;
                    }
                }
            }
            Model.Naemes = new List<Currencies.Currency>();
            Model.Exchange = new List<double[]>();

            foreach (var item in Model.SubCurrency)
            {
                Model.Naemes.Add(item);
                Model.Exchange.Add(Result[item].ToArray());
            }
            Model.Categories = DateList.ToArray();    
            return Json(Model);
        }


        /// <summary>隨機資料數值</summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return Math.Round( random.NextDouble() * (maximum - minimum) + minimum,4);
        }


        /// <summary>初始化對照表</summary>
        private void InitCurrencyTable()
        {


            ////台幣
            Dictionary<Currencies.Currency, double[]> TWDCurrency = new Dictionary<Currencies.Currency, double[]>();
            TWDCurrency.Add(Currencies.Currency.USD, new double[] { 30, 35 });
            TWDCurrency.Add(Currencies.Currency.JPY, new double[] { 0.2, 0.4 });
            TWDCurrency.Add(Currencies.Currency.EUR, new double[] { 32, 45 });
            TWDCurrency.Add(Currencies.Currency.HKD, new double[] { 3, 6 });
            TWDCurrency.Add(Currencies.Currency.GBP, new double[] { 42, 50 });
            TWDCurrency.Add(Currencies.Currency.SGD, new double[] { 20, 30 });
            TWDCurrency.Add(Currencies.Currency.KRW, new double[] { 0.01, 0.05 });
            TWDCurrency.Add(Currencies.Currency.THB, new double[] { 0.08, 0.13 });            
            TWDCurrency.Add(Currencies.Currency.CNY, new double[] { 4, 5 });
            TWDCurrency.Add(Currencies.Currency.TWD, new double[] { 1, 1 });
            MainCurrency.Add(Currencies.Currency.TWD, TWDCurrency);


            ////日元
            Dictionary<Currencies.Currency, double[]> JPYCurrency = new Dictionary<Currencies.Currency, double[]>();
            JPYCurrency.Add(Currencies.Currency.USD, new double[] { 0.01, 0.03 });
            JPYCurrency.Add(Currencies.Currency.TWD, new double[] { 0.2, 0.4 });
            JPYCurrency.Add(Currencies.Currency.EUR, new double[] { 0.01, 0.03 });
            JPYCurrency.Add(Currencies.Currency.HKD, new double[] { 0.1, 0.3 });
            JPYCurrency.Add(Currencies.Currency.GBP, new double[] { 0.01, 0.03 });
            JPYCurrency.Add(Currencies.Currency.SGD, new double[] { 0.1, 0.3 });
            JPYCurrency.Add(Currencies.Currency.KRW, new double[] { 11,15 });
            JPYCurrency.Add(Currencies.Currency.THB, new double[] { 0.15, 0.3 });
            JPYCurrency.Add(Currencies.Currency.CNY, new double[] { 0.06,0.08 });
            JPYCurrency.Add(Currencies.Currency.JPY, new double[] { 0.06, 0.08 });
            MainCurrency.Add(Currencies.Currency.JPY, JPYCurrency);


            ////美元
            Dictionary<Currencies.Currency, double[]> USDCurrency = new Dictionary<Currencies.Currency, double[]>();
            USDCurrency.Add(Currencies.Currency.USD, new double[] { 1, 1 });
            USDCurrency.Add(Currencies.Currency.TWD, new double[] { 28, 35 });
            USDCurrency.Add(Currencies.Currency.EUR, new double[] { 0.1, 0.9 });
            USDCurrency.Add(Currencies.Currency.HKD, new double[] { 35, 40 });
            USDCurrency.Add(Currencies.Currency.GBP, new double[] { 1, 2 });
            USDCurrency.Add(Currencies.Currency.SGD, new double[] { 0.1, 0.3 });
            USDCurrency.Add(Currencies.Currency.KRW, new double[] { 11, 15 });
            USDCurrency.Add(Currencies.Currency.THB, new double[] { 30, 35 });
            USDCurrency.Add(Currencies.Currency.CNY, new double[] { 0.06, 0.08 });
            USDCurrency.Add(Currencies.Currency.JPY, new double[] {125, 150 });
            MainCurrency.Add(Currencies.Currency.USD, USDCurrency);


            ////人民幣
            Dictionary<Currencies.Currency, double[]> CNYCurrency = new Dictionary<Currencies.Currency, double[]>();
            CNYCurrency.Add(Currencies.Currency.USD, new double[] { 15, 25 });
            CNYCurrency.Add(Currencies.Currency.TWD, new double[] { 4, 6 });
            CNYCurrency.Add(Currencies.Currency.EUR, new double[] { 20,25 });
            CNYCurrency.Add(Currencies.Currency.HKD, new double[] { 10, 15 });
            CNYCurrency.Add(Currencies.Currency.GBP, new double[] { 1, 2 });
            CNYCurrency.Add(Currencies.Currency.SGD, new double[] { 1, 3 });
            CNYCurrency.Add(Currencies.Currency.KRW, new double[] { 11, 15 });
            CNYCurrency.Add(Currencies.Currency.THB, new double[] { 30, 35 });
            CNYCurrency.Add(Currencies.Currency.CNY, new double[] {1,1 });
            CNYCurrency.Add(Currencies.Currency.JPY, new double[] { 60, 70 });
            MainCurrency.Add(Currencies.Currency.CNY, CNYCurrency);


            ////港元
            Dictionary<Currencies.Currency, double[]> HKDCurrency = new Dictionary<Currencies.Currency, double[]>();
            HKDCurrency.Add(Currencies.Currency.USD, new double[] { 15, 25 });
            HKDCurrency.Add(Currencies.Currency.TWD, new double[] { 4, 6 });
            HKDCurrency.Add(Currencies.Currency.EUR, new double[] { 20, 25 });
            HKDCurrency.Add(Currencies.Currency.HKD, new double[] { 1,1 });
            HKDCurrency.Add(Currencies.Currency.GBP, new double[] { 1, 2 });
            HKDCurrency.Add(Currencies.Currency.SGD, new double[] { 1, 3 });
            HKDCurrency.Add(Currencies.Currency.KRW, new double[] { 11, 15 });
            HKDCurrency.Add(Currencies.Currency.THB, new double[] { 30, 35 });
            HKDCurrency.Add(Currencies.Currency.CNY, new double[] { 4, 6 });
            HKDCurrency.Add(Currencies.Currency.JPY, new double[] { 60, 70 });
            MainCurrency.Add(Currencies.Currency.HKD, HKDCurrency);


            ////歐元
            Dictionary<Currencies.Currency, double[]> EURCurrency = new Dictionary<Currencies.Currency, double[]>();
            EURCurrency.Add(Currencies.Currency.USD, new double[] { 15, 25 });
            EURCurrency.Add(Currencies.Currency.TWD, new double[] { 4, 6 });
            EURCurrency.Add(Currencies.Currency.EUR, new double[] { 1, 1 });
            EURCurrency.Add(Currencies.Currency.HKD, new double[] { 10, 15 });
            EURCurrency.Add(Currencies.Currency.GBP, new double[] { 1, 2 });
            EURCurrency.Add(Currencies.Currency.SGD, new double[] { 1, 3 });
            EURCurrency.Add(Currencies.Currency.KRW, new double[] { 11, 15 });
            EURCurrency.Add(Currencies.Currency.THB, new double[] { 30, 35 });
            EURCurrency.Add(Currencies.Currency.CNY, new double[] { 1, 1 });
            EURCurrency.Add(Currencies.Currency.JPY, new double[] { 60, 70 });
            MainCurrency.Add(Currencies.Currency.EUR, EURCurrency);


            ////泰銖
            Dictionary<Currencies.Currency, double[]> THBCurrency = new Dictionary<Currencies.Currency, double[]>();
            THBCurrency.Add(Currencies.Currency.USD, new double[] { 15, 25 });
            THBCurrency.Add(Currencies.Currency.TWD, new double[] { 4, 6 });
            THBCurrency.Add(Currencies.Currency.EUR, new double[] { 38, 46 });
            THBCurrency.Add(Currencies.Currency.HKD, new double[] { 10, 15 });
            THBCurrency.Add(Currencies.Currency.GBP, new double[] { 1, 2 });
            THBCurrency.Add(Currencies.Currency.SGD, new double[] { 1, 3 });
            THBCurrency.Add(Currencies.Currency.KRW, new double[] { 11, 15 });
            THBCurrency.Add(Currencies.Currency.THB, new double[] { 1, 1 });
            THBCurrency.Add(Currencies.Currency.CNY, new double[] { 4, 7 });
            THBCurrency.Add(Currencies.Currency.JPY, new double[] { 60, 70 });
            MainCurrency.Add(Currencies.Currency.THB, THBCurrency);


            ////韓元
            Dictionary<Currencies.Currency, double[]> KRWCurrency = new Dictionary<Currencies.Currency, double[]>();
            KRWCurrency.Add(Currencies.Currency.USD, new double[] { 15, 25 });
            KRWCurrency.Add(Currencies.Currency.TWD, new double[] { 4, 6 });
            KRWCurrency.Add(Currencies.Currency.EUR, new double[] { 38, 46 });
            KRWCurrency.Add(Currencies.Currency.HKD, new double[] { 10, 15 });
            KRWCurrency.Add(Currencies.Currency.GBP, new double[] { 1, 2 });
            KRWCurrency.Add(Currencies.Currency.SGD, new double[] { 1, 3 });
            KRWCurrency.Add(Currencies.Currency.KRW, new double[] { 1, 1 });
            KRWCurrency.Add(Currencies.Currency.THB, new double[] { 0.3, 0.8 });
            KRWCurrency.Add(Currencies.Currency.CNY, new double[] { 4, 7 });
            KRWCurrency.Add(Currencies.Currency.JPY, new double[] { 60, 70 });
            MainCurrency.Add(Currencies.Currency.KRW, KRWCurrency);


            ////英鎊
            Dictionary<Currencies.Currency, double[]> GBPCurrency = new Dictionary<Currencies.Currency, double[]>();
            GBPCurrency.Add(Currencies.Currency.USD, new double[] { 1, 5 });
            GBPCurrency.Add(Currencies.Currency.TWD, new double[] { 4, 6 });
            GBPCurrency.Add(Currencies.Currency.EUR, new double[] { 38, 46 });
            GBPCurrency.Add(Currencies.Currency.HKD, new double[] { 10, 15 });
            GBPCurrency.Add(Currencies.Currency.GBP, new double[] { 1,1 });
            GBPCurrency.Add(Currencies.Currency.SGD, new double[] { 1, 3 });
            GBPCurrency.Add(Currencies.Currency.KRW, new double[] { 2, 5 });
            GBPCurrency.Add(Currencies.Currency.THB, new double[] { 0.3, 0.8 });
            GBPCurrency.Add(Currencies.Currency.CNY, new double[] { 4, 7 });
            GBPCurrency.Add(Currencies.Currency.JPY, new double[] { 60, 70 });
            MainCurrency.Add(Currencies.Currency.GBP, GBPCurrency);


            ////新加坡元
            Dictionary<Currencies.Currency, double[]> SGDCurrency = new Dictionary<Currencies.Currency, double[]>();
            SGDCurrency.Add(Currencies.Currency.USD, new double[] { 1, 5 });
            SGDCurrency.Add(Currencies.Currency.TWD, new double[] { 0.1,0.8 });
            SGDCurrency.Add(Currencies.Currency.EUR, new double[] { 38, 46 });
            SGDCurrency.Add(Currencies.Currency.HKD, new double[] { 10, 15 });
            SGDCurrency.Add(Currencies.Currency.GBP, new double[] {1, 5 });
            SGDCurrency.Add(Currencies.Currency.SGD, new double[] { 1, 1 });
            SGDCurrency.Add(Currencies.Currency.KRW, new double[] { 2, 5 });
            SGDCurrency.Add(Currencies.Currency.THB, new double[] { 0.3, 0.8 });
            SGDCurrency.Add(Currencies.Currency.CNY, new double[] { 4, 7 });
            SGDCurrency.Add(Currencies.Currency.JPY, new double[] { 60, 70 });
            MainCurrency.Add(Currencies.Currency.SGD, SGDCurrency);

        }


    }
}
