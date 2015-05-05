using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WebInfo.Business.DataEntities;

namespace WebInfo.Business
{
    public interface IWebInfoService
    {
        /// <summary>
        /// 抓取網頁資訊
        /// </summary>
        /// <param name="pUrl">網址</param>
        /// <returns></returns>
        StreamReader GetResponse(string pUrl);

        /// <summary>
        /// 抓取網頁資訊To String
        /// </summary>
        /// <param name="pUrl">網址</param>
        /// <returns></returns>
        string GetStringResponse(string pUrl);


        /// <summary>
        /// Post功能
        /// </summary>
        /// <param name="Address">網址</param>
        /// <param name="SiteInfoList">文章列</param>
        /// <returns></returns>
        long POST(string Address, List<SiteInfo> SiteInfoList);

        /// <summary>
        /// 20150410 add by Dick for 直接針對網址進行解析內容及傳輸。
        /// </summary>
        /// <param name="Url">指定網址</param>
        /// <param name="pCondition">標題限制必須包含字串</param>
        /// <param name="PushCount">推文數量</param>
        /// <param name="PostAdress">分析後傳送位址</param>
        void GetBueatyDirtory(string Url, string pCondition, int PushCount, string PostAdress);

        /// <summary>
        /// 20150410 add by Dick for 直接針對網址進行解析內容及傳輸。
        /// 20150428 modified by Dick for 不需要過濾標題條件
        /// </summary>
        /// <param name="Url">指定網址</param>
        /// <param name="PushCount">推文數量</param>
        /// <param name="PostAdress">分析後傳送位址</param>
        void GetBueatyDirtory(string Url, int PushCount, string PostAdress);
    }
}
