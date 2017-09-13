using System.Collections.Generic;
using WebApplication1.Code.DAO;

namespace WebApplication1.Models
{
    public class BankAccountViewModels
    {

        public class BankAccountViewModel 
        {
            public TradeInfo Info { set; get; }

            public List<TradeInfo> InfoList { set; get; }
        }

    }
}