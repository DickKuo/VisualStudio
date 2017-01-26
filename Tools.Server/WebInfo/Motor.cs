using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebInfo {
    public class Motor {

        /// <summary>來源的編碼</summary>
        public string Key { set; get; }

        /// <summary>標題</summary>
        public string Title { set; get; }

        /// <summary>網址</summary>
        public string Url { set; get; }

        /// <summary>價格</summary>
        public double Price { set; get; }

        /// <summary>內文</summary>
        public string Context { set; get; }
        
        /// <summary>地區</summary>
        public string Location { set; get; }

        /// <summary>圖片</summary>
        public string Img { set; get; }

        /// <summary>里程數</summary>
        public string Milage { set; get; }

        /// <summary>車齡</summary>
        public string Years { set; get; }

        /// <summary>品牌</summary>
        public string Brand { set; get; }

        /// <summary>型號</summary>
        public string Model { set; get; }

        /// <summary>備註</summary>
        public string Remark { set; get; }

    }
}
