using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comon
{
    public interface IServiceProviderAddin
    {
        IServiceEntry[] ServiceEntries
        {
            get;
        }
    }
}
