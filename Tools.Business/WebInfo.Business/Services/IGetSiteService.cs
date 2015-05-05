using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WebInfo.Business.DataEntities;
using System.Xml;

namespace WebInfo.Business.Services
{
    public interface IGetSiteService
    {
        /// <summary>
        /// 抓取文章內容
        /// </summary>
        /// <param name="Url">Url</param>
        /// <returns></returns>
        StreamReader GetWebInfo(string Url);


        /// <summary>
        /// 抓取PTT文章列表         
        /// </summary>
        /// <param name="BaseUrl">起始網址Url</param>
        /// <returns></returns>
        SitePlus GetUrlList(string BaseUrl);


        /// <summary>
        /// 20150410 針對網址解析
        /// 解析表特版文章內容。
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        SiteInfo GetInfo(string Url);

        /// <summary>
        /// 遞回所有頁面
        /// </summary>
        /// <param name="index">起始索引列</param>
        /// <param name="pSiteplus">輔助類</param>
        /// <param name="li">Post列</param>
        /// <param name="Site">Ptt版</param>
        /// <param name="Formate">網頁清單清除條件</param>
        /// <param name="pCondition">撈取資料條件</param>
        /// <param name="doc">存檔Xml Document</param>
        /// <param name="root">跟索引</param>     
        void Recursive(ref int index, SitePlus pSiteplus, List<SiteInfo> li, string Site, string Formate, string pCondition, XmlDocument doc, XmlNode root);


    }
}
