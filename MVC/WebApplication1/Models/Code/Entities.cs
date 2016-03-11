using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Code
{
    public class Entities
    {

        public enum EntitiesList
        {
            Employee = 0,
            Department = 1,
            Position = 2,
            Photo = 3,
            /// <summary>全職</summary>
            FullTime=4,
            /// <summary>兼職</summary>
            PTime=5,
            /// <summary>駐點</summary>
            Stagnation=6,
            /// <summary>功能清單</summary>
            Menu=7,
            /// <summary>角色</summary>
            Role=8,
            /// <summary>角色可用功能</summary>
            RolesMenu=9,
            /// <summary>使用者</summary>
            User=10



        }
    }
}