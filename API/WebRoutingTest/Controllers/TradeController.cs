using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Code.DAO;
using WebRoutingTest.Models.Code;

namespace WebRoutingTest.Controllers
{
    public class TradeController : ApiController
    {

        [Route("api/Trade/SaveMoney")]
        /// <summary>儲存金額Api  </summary>
        /// <param name="Info"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveMoney(TradeInfo Info)
        {
            TradeDAO Trade = new TradeDAO();
            TradeInfo ResultInfo = Trade.CashOperation(Info);
            return JsonConvert.SerializeObject(ResultInfo);
        }//end SaveMoney

    }
}
