using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StartTool {

    public class StockStart {
        private string _logpath;

        private string _stockxml;

        private string _connentionstring;

        private string _url;

        public string LogPath {
            set {
                _logpath = value;
            }
        }

        public string StockXML {
            set {
                _stockxml = value;
            }
        }

        public string ConnectionString {
            set {
                _connentionstring = value;
            }
        }

        public string URL {
            set {
                _url = value;
            }
        }

        public StockStart(string pLogPath, string pStockXML, string pConnetion, string pURL) {
            this.LogPath = pLogPath;
            this.StockXML = pStockXML;
            this.ConnectionString = pConnetion;
            this.URL = pURL;
        }

        /// <summary>股票服務的開始 </summary>
        public void Start() {
            CommTool.ToolLog tool = new CommTool.ToolLog(_logpath);
            CommTool.ToolLog.Log("抓取資料");
            CommTool.XmlFile xf = new CommTool.XmlFile(_stockxml);
            List<string> nodes = xf.XmlLoadList();
            foreach (string node in nodes) {
                Stock.StockData stockData = new Stock.StockData(node);
                Stock.Stock stock = stockData.GetStockData(_url, _connentionstring);
                stockData.SetkData(_connentionstring);
            }
        }
    }
}
