using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Code.DAO
{
    public class ResultCodeManager
    {        
        public enum ResultCode
        {
            DeleteError = 0,            //刪除失敗
            DeleteSucces = 1,           //刪除成功
            AddError=2,                 //新增失敗
            AddSucces=3,                //新增成功
            EditError=4,                //編輯失敗  
            EditSucces=5,               //編輯成功
            LoginSucces=6,              //登入成功
            LoginError=7                //登入失敗
        }
    }
}