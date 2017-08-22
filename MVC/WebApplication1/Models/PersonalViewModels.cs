
namespace WebApplication1.Models {
    public class PersonalViewModels {

        public class PersonalViewModel {

            public string FirstName { set; get; }

            public string LastName { set; get; }

            public string NickName { set; get; }

            public string Phone { set; get; }

            public string HomeAddress { set; get; }

            public string Email { set; get; }                        
        }

        public class ForGetPassWordViewModel {

            public string NowPassWord { set; get; }

            public string NewPassWord { set; get; }

            public string PassWordComfirm { set; get; }
        }

    }

}