using System.Collections.Generic;
using WebApplication1.Models.Code;

namespace WebApplication1.Models {
    public class TradeViewModels {

        public class TradeViewModel : BaseViewModel {

            public string DueMonth { set; get; }

            public List<OptionQuotes> QuotesList { set; get; }

            public string Op { set; get; }

            public string Contract { set; get; }

            public string Type { set; get; }

            public string Clinch { set; get; }

            public string Lot { set; get; }
        }
    }
}