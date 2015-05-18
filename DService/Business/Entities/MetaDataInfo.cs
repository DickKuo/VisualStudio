using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DService.Business.Entities
{
    public abstract class MetaDataInfo :Attribute
    {

        public string Alias { set; get; }
        public string DisplayName { set; get; }

    }
}
