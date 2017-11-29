using ObjectBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Code.Helpers; 

namespace WebApplication1.Controllers
{
    public class AjaxFuctionsController : BaseLoginController
    {
        
        
        /// <summary></summary>
        /// <returns></returns>
        [HttpPost]
        public string Index()
        {           
            List<Bank> ListBank = new List<Bank>();
            BankDAO BankDB=new BankDAO();
            ListBank = BankDB.GetBankByCustomerSN(LoginHelper.GetLoginInfo().Customer.SN);
            return Newtonsoft.Json.JsonConvert.SerializeObject(ListBank);
        }       

    }
}