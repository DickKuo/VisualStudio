using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommTool.Business.Services
{
    public interface IServiceProviderAddin
    {
        IServiceEntry[] ServiceEntries
        {
            get;
        }
    }
}
