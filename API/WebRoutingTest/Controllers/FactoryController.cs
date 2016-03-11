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
    public class FactoryController : ApiController
    {

        [Route("api/Factory/Factory")]
        [HttpPost]
        public Operation Factory(Operation Oper)
        {
            FactoryOperation FactoryOp = new FactoryOperation();
            DAOBase BaseDao =  FactoryOp.SelectFactory(Oper);
            Operation Result = FactoryOp.DoOperation(Oper,BaseDao);
            return Result;
        }


        [Route("api/Factory/DepartmentMangement")]
        [HttpPost]
        public JsonObj DepartmentMangement(JsonObj Obj)
        {
            FactoryForDepartment Factory = new FactoryForDepartment();
            DAOBranchBase BaseDAO = Factory.SelectDAO(Obj);
            JsonObj Result = Factory.Operation(Obj, BaseDAO);
            return Result;
        }

    }
}
