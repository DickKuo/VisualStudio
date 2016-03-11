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

        public abstract object Add(Operation Oper);

        public abstract object Edit(Operation Oper);

        public abstract object Delete(Operation Oper);


    }
}