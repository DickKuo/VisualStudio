using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectBase {
    public class Adviser {

        public int SN { set; get; }

        public string Account { set; get; }

        public string PassWord { set; get; }

        public string FirstName { set; get; }

        public string LastName { set; get; }

        public string ID { set; get; }

        public GenderType Gender { set; get; }

        public string Email { set; get; }

        public string Phone { set; get; }

        public DateTime BirthDay { set; get; }

        public string Address { set; get; }

        public DateTime AddTime { set; get; }

        public DateTime EditTime { set; get;  }

        public bool IsEnable { set; get; }

        public string Remark { set; get; }

    }
}
