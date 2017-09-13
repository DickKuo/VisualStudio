using System.Collections.Generic;
using WebApplication1.Models.Code;

namespace WebApplication1.Models
{
    public class UserAccountViewModels
    {
        public class UserAccountViewModel
        {
            public User User { set; get; }

            private List<User> _UserList = new List<User>();

            public List<User> UserList { get { return _UserList; } set { _UserList = value; } }

            public string JsonString { set; get; }
        }
    }
}