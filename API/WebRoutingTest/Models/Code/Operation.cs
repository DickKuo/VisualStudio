using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebRoutingTest.Models.Code.DAO;

namespace WebRoutingTest.Models.Code
{
    public class Operation
    {
        public enum OperationCode
        {
            Error = 0,
            Success = 1
        }

        public enum OperationMethod
        {
            Select = 0,
            Add = 1,
            Edit = 2,
            Delete = 3
        }

        public enum Entities
        {
            Employee = 0,
            Department = 1,
            Company = 2
        }

        public object Obj { set; get; }

        public List<object> ObjList { set; get; }

        public OperationCode ResultCode { set; get; }

        public OperationMethod Method { set; get; }

        public Entities Entity { set; get; }

        public BaseDAO Dao { set; get; }


    }
}