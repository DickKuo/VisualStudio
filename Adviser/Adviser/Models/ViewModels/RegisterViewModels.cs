using ObjectBase;
using System;
namespace Adviser.Models.ViewModels {
    public class RegisterViewModels {

        public class RegisterViewModel {

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

            public string Remark { set; get; }
        }

    }
}