﻿namespace Stock
{
    public class Option
    {
        public string OP { set; get; }

        public decimal Buy { set; get; }

        public decimal Sell { set; get; }

        public decimal Clinch { set; get; }

        public decimal Change { set; get; }

        public string Time { set; get; }

        public string Contract { set; get; }

        /// <summary>成交量</summary>
        public int Volume { set; get; }

        /// <summary>未沖銷契約數</summary>
        public int NumberOfContracts { set; get; }

        /// <summary>類別</summary>
        public string DueMonth { set; get; }

        /// <summary>時間戳章</summary>
        public string TradeTimestamp { set; get; }

        public static string[] GetTableTypeColumn()
        {
            return new string[] { "OP", "Buy", "Sell", "Clinch", "Change", "Time", "Contract", "DueMonth", "NumberOfContracts", "Volume", "TradeTimestamp" };
        }
    }
}