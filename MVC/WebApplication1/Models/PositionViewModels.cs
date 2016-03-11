using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebRoutingTest.Models.Code;

namespace WebApplication1.Models
{
    public class PositionViewModels
    {
        public class PositionViewModel
        {

            public Position Position { set; get; }

            public List<Position> PositionList { set; get; }


        }
    }
}