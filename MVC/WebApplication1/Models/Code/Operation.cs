using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.Code;

namespace WebRoutingTest.Models.Code
{
    public class Operation
    {           
        public string Url { set; get; }

        public object Obj { set; get; }

        public List<object> ObjList { set; get; }

        public WebApplication1.Models.Code.OperationResultCode.OperationCode ResultCode { set; get; }    //Success Error

        public OperationMethod.Method Method { set; get; }   //Add Query  Edit  Delete

        public Entities.EntitiesList Entity { set; get; }    //Employee Department....

       

    }
}