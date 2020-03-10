using WebInfo.Business.DataEntities;
using System.Collections.Generic;
using System.Xml;
using System.IO;

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
        /// 抓取文章內容
        /// 20150616 擴充可以選擇是否避免相關指定網頁以外的網址 工具集 #62  
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="IsAvoid">true 如果不是指定的網站類型則跳過</param>
        /// <param name="PointUrl">指定網址類型</param>
        /// <returns></returns>
        StreamReader GetWebInfo(string Url, bool IsAvoid = false, string PointUrl = "www.ptt.cc");
                
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

        ///// <summary>
        ///// 工具集 #60
        ///// 將miupix的圖片轉換成實際圖片位置
        ///// </summary>
        ///// <param name="Url">欲轉換網址</param>
        ///// <returns></returns>
        //string GetMiupixImg(string Url);
    }
}