using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRoutingTest.Models.Code
{
    public class JsonObj
    {
        public WebRoutingTest.Models.Code.Sources.DAOSource Source { set; get; }

        public WebRoutingTest.Models.Code.OperMethods.OperMethod Method { set; get; }

        public string JsonString { set; get; }
    }
}