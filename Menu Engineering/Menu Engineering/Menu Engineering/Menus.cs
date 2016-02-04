using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Menu_Engineering
{
     public  class Menus
    {
         public Guid MenuId { set; get; }

         public string MenuCollectionId { set; get; }

         public string Name{set;get;}

         public decimal SalePrice { set; get; }

         public decimal NumberSold { set; get; }

         public byte[] Photo { set; get; }

         public string Remark { set; get; }
    }
}
