using System;

namespace Adviser.Models.ViewModels
{
    public class BaseViewModel
    {
        public int SN { set; get; }

        public DateTime BeginTime { set; get; }

        public DateTime EndTime { set; get; }

        public int Page { set; get; }

        public int MaxPage { set; get; }

        public bool IsPartial { set; get; }
    }
}