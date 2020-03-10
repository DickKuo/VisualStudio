using CommTool.Business.Metadata;
using System.ComponentModel;

namespace DService.Business.Entities
{
    public class Gold
    {
        #region Private Member
        private System.Guid _goldId;
        private System.Decimal _bankSell;
        private System.Decimal _bankBuy;
        private System.DateTime _date;
        #endregion

        #region Parameter

        /// <summary>
        ///黃金存摺
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.Guid)]
        [Description("黃金存摺")]
        public System.Guid GoldId
        {
            get
            {
                return this._goldId;
            }
            set
            {
                if ((_goldId != value))
                {
                    this._goldId = value;
                    //this.OnPropertyChanged("GoldId");
                }
            }
        }

        /// <summary>
        ///銀行賣出
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.Decimal)]
        [Description("銀行賣出")]
        public System.Decimal BankSell
        {
            get
            {
                return this._bankSell;
            }
            set
            {
                if ((_bankSell != value))
                {
                    this._bankSell = value;
                    //this.OnPropertyChanged("BankSell");
                }
            }
        }

        /// <summary>
        ///銀行買入
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.Decimal)]
        [Description("銀行買入")]
        public System.Decimal BankBuy
        {
            get
            {
                return this._bankBuy;
            }
            set
            {
                if ((_bankBuy != value))
                {
                    this._bankBuy = value;
                    //this.OnPropertyChanged("BankBuy");
                }
            }
        }

        /// <summary>
        ///日期
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.DateTime)]
        [Description("日期")]
        public System.DateTime Date
        {
            get
            {
                return this._date;
            }
            set
            {
                if ((_date != value))
                {
                    this._date = value;
                    //this.OnPropertyChanged("Date");
                }
            }
        }
        #endregion 
    }
}