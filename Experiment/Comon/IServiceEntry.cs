using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comon
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
}
