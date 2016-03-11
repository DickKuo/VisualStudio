using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Code.DAO;
using WebRoutingTest.Models.Code;
using WebRoutingTest.Models.Code.DAO;

namespace WebRoutingTest.Controllers
{
    public class CarouselController : ApiController
    {

        [Route("api/Carousel/Uploading")]
        [HttpPost]
        public object Uploading(Operation Oper)
        {
            FactoryOperation FactoryOp = new FactoryOperation();
            DAOBase BaseDao= FactoryOp.SelectFactory(Oper);
            Operation Result = FactoryOp.DoOperation(Oper, BaseDao);
            return Result;         
        }
    }
}
