using System;

namespace Adviser.Models.Code
{
    public class ReportRequest
    {
        public DateTime BeginTime { set; get; }

        public DateTime EndTime { set; get; }

        public int Page { set; get; }

        public int PreRecord { set; get; }
    }
}