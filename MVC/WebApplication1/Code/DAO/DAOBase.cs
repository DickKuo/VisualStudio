using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Code.DAO
{
    public abstract class DAOBase
    {
        protected UseStoreProcedure _DbIstance = new UseStoreProcedure();

        public DAOBase()
        {
        
        }


        public virtual void SetConnetionString(string ConnectionString)
        {
            _DbIstance.ConnectiinString = ConnectionString;
        }


        public abstract object Select<T>(T Obj);

        public abstract object Add<T>(T Obj);

        public abstract object Edit<T>(T Obj);

        public abstract object Delete<T>(T Obj);

    }
}