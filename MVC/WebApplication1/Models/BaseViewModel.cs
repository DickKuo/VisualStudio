using System;

namespace WebApplication1.Models {
    public class BaseViewModel {

        public DateTime BeginTime { set; get; }

        public DateTime EndTime { set;get; }

        public int Page {set;get;}

        public int MaxPage { set; get; }

    }
}