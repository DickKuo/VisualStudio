 
namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using WebApplication1.Code.DAO;
    using WebApplication1.Models.Code;

    public partial class LoginInfo
    {   
        public ObjectBase.Customer Customer { set; get; }

        public User User { set; get; }
        
        public List<Menu> MenuList { set; get; }

    }
}
