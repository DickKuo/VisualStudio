using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WebApplication1.Code.DAO
{
    public class ApiOperation
    {
        public string Uri { set; get; }

        public string Action { set; get; }

        public string Controller { set; get; }

        public HttpMethod Methode { set; get; }

        public List<string> Parameters { set; get; }

        public object obj { set; get; }

    }
}