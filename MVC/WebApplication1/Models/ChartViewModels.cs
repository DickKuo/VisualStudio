using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class ChartViewModels
    {
        public class ChartViewMode
        {
            public DateTime BeginDate { set; get; }
            public DateTime EndDate { set; get; }

            public  WebApplication1.Models.Code.Currencies.Currency MainCurrency { set; get; }

            public List<WebApplication1.Models.Code.Currencies.Currency> SubCurrency { set; get; }

            public string Currencies { set; get; }

            public List<WebApplication1.Models.Code.Currencies.Currency> Naemes { set; get; }
            public List<double[]>Exchange { set; get; }
            
           
            public string[] Categories { set; get; }
          


        }

    }
}