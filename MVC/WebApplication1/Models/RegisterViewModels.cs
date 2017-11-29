using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models {
    public class RegisterViewModels {

        public class RegisterViewModel {

            public string Account { set; get; }

            public string PassWord { set; get; }

            public string FirstName { set; get; }

            public string LastName { set; get; }

            public string NickName { set; get; }

            public int Gender { set; get; }

            public string ID { set; get; }
            
            public string Phone { set;get; }

            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime BirthDay { set; get; }

            public string HomeAddr { set; get; }

        }
    }
}