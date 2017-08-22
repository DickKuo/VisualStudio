using System.Collections.Generic;
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