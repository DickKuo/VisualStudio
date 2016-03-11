using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Code.DAO;

namespace WebRoutingTest.Models
{
    public  abstract class BaseBank
    {
        protected string _BankName { set; get; }

        /// <summary>　</summary>
        /// <param name="BankName"></param>
        public void SetBankName(string BankName)
        {
            this._BankName = BankName;          
        }


        public virtual TradeInfo Opeartion(TradeInfo Info)
        {
            TradeInfo Result = null;
            switch (Info.Operation)
            {
                case TradeInfo.OperationCode.Add:
                    Result = this.Add(Info);
                    break;
                case TradeInfo.OperationCode.Reduce:
                    Result = this.Reduce(Info);
                    break;
            }
            return Result;
        }

        /// <summary>  加錢計算/// </summary>
        /// <param name="Emp"></param>
        /// <param name="Money"></param>
        /// <returns></returns>
        public abstract TradeInfo Add(TradeInfo Info);


        /// <summary>減錢計算/// </summary>
        /// <param name="Emp"></param>
        /// <param name="Money">金額用負數給予</param>
        /// <returns></returns>
        public abstract TradeInfo Reduce(TradeInfo Info);

          
    }
}