using System.ComponentModel;
using System;

namespace Stock
{
    public class Stock
    {
        #region 私有變數

        private string _stockNum;

        private DateTime _stockTime;

        private decimal _price;

        private decimal _buyPrice;

        private decimal _sellPrice;

        private string _change;

        private decimal _quantity;

        private decimal _yesterday;

        private decimal _start;

        private decimal _highest;

        private decimal _lowest;

        private bool _IsSucess;

        #endregion

        /// <summary>
        /// 是否成功擷取資料
        /// </summary>       
        [Description("Howest")]
        public System.Boolean IsSucess
        {
            get
            {
                return this._IsSucess;
            }
            set
            {
                if ((_IsSucess != value))
                {
                    this._IsSucess = value;
                }
            }
        }

        /// <summary>
        /// 最低
        /// </summary>       
        [Description("Howest")]
        public System.Decimal Lowest
        {
            get
            {
                return this._lowest;
            }
            set
            {
                if ((_lowest != value))
                {
                    this._lowest = value;
                }
            }
        }

        /// <summary>
        /// 最高
        /// </summary>       
        [Description("Highest")]
        public System.Decimal Highest
        {
            get
            {
                return this._highest;
            }
            set
            {
                if ((_highest != value))
                {
                    this._highest = value;
                }
            }
        }

        /// <summary>
        /// 開盤
        /// </summary>       
        [Description("Start")]
        public System.Decimal Start
        {
            get
            {
                return this._start;
            }
            set
            {
                if ((_start != value))
                {
                    this._start = value;
                }
            }
        }

        /// <summary>
        /// 昨收
        /// </summary>       
        [Description("Yesterday")]
        public System.Decimal Yesterday
        {
            get
            {
                return this._yesterday;
            }
            set
            {
                if ((_yesterday != value))
                {
                    this._yesterday = value;
                }
            }
        }

        /// <summary>
        /// 張數
        /// </summary>       
        [Description("Quantity")]
        public System.Decimal Quantity
        {
            get
            {
                return this._quantity;
            }
            set
            {
                if ((_quantity != value))
                {
                    this._quantity = value;
                }
            }
        }

        /// <summary>
        /// 漲跌
        /// </summary>       
        [Description("Change")]
        public System.String Change
        {
            get
            {
                return this._change;
            }
            set
            {
                if ((_change != value))
                {
                    this._change = value;
                }
            }
        }

        /// <summary>
        /// 賣價
        /// </summary>       
        [Description("SellPrice")]
        public System.Decimal SellPrice
        {
            get
            {
                return this._sellPrice;
            }
            set
            {
                if ((_sellPrice != value))
                {
                    this._sellPrice = value;
                }
            }
        }

        /// <summary>
        /// 買價
        /// </summary>       
        [Description("BuyPrice")]
        public System.Decimal BuyPrice
        {
            get
            {
                return this._buyPrice;
            }
            set
            {
                if ((_buyPrice != value))
                {
                    this._buyPrice = value;
                }
            }
        }

        /// <summary>
        /// 成交價
        /// </summary>       
        [Description("Price")]
        public System.Decimal Price
        {
            get
            {
                return this._price;
            }
            set
            {
                if ((_price != value))
                {
                    this._price = value;
                }
            }
        }

        /// <summary>
        /// 股票代號
        /// </summary>       
        [Description("StockNum")]
        public System.String StockNum
        {
            get
            {
                return this._stockNum;
            }
            set
            {
                if ((_stockNum != value))
                {
                    this._stockNum = value;
                }
            }
        }

        /// <summary>
        /// 時間
        /// </summary>       
        [Description("StockNum")]
        public System.DateTime StockTime
        {
            get
            {
                return this._stockTime;
            }
            set
            {
                if ((_stockTime != value))
                {
                    this._stockTime = value;
                }
            }
        }
    }
}