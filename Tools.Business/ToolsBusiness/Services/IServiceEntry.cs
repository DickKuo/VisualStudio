using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommTool.Business.Services
{
    public interface IServiceEntry
    {
        System.Type ServiceInterface
        {
            get;
        }
        System.Type ServiceClass
        {
            get;
        }
        ServiceCreateType ServiceCreateType
        {
            get;
        }
    }
    public enum ServiceCreateType
    {
        Instance,
        Callback,
        Startup
    }
   
}
