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
            
        }


    }
}
