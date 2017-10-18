using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Code
{
    public class BaseCode
    {

        public enum MessageType : int
        {
            danger = 0,
            warning = 1,
            info = 2,
            success = 3,
        }
    }
}