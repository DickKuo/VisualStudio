using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebRoutingTest.Models.Code.DAO
{
    public abstract class BaseDAO 
    {
        protected UseStoreProcedure Procedure = new UseStoreProcedure();

        public BaseDAO()
        {

        }

        public virtual void SetConnetionString(string ConnectionString)
        {
            Procedure.ConnectiinString = ConnectionString;
        }

        public abstract object Select();

        public abstract object Add<T>(T Obj);

        public abstract object Edit<T>(T Obj);

        public abstract object Delete<T>(T Obj);


    }
}