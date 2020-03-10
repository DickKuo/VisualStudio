using System.Collections.Generic;

namespace Adviser.Models.Code
{
    public partial class LoginInfo
    {
        public ObjectBase.Adviser Adviser { set; get; }
        public List<Menu> MenuList { set; get; }
    }
}