using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using GetSite;
using System.Data;
//using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading;

namespace Reptile
{
    class Program
    {
        static int Tag { set; get; }

        static int PushCount { set; get; }
        
        static DateTime RecordTime {set;get;}

        static List<string> RecordList = new List<string>();

        static void Main(string[] args)
        {
            XmlDocument doc = Tools.XmlTool.LoadXml(Directory.GetCurrentDirectory() + "\\" + "Record.xml");
            XmlNodeList NodeList = doc.SelectNodes("/root/Title");
            foreach(XmlNode node in NodeList)
            {
                RecordList.Add(node.InnerText);
            }
            //while(true) {
               // Console.WriteLine("啟動請按r");
            //int key = Console.Read();
               //if(key.Equals(114))
                   Start();
                   //Console.Read();
            //}
        }

        public static string Address { set; get; }
        private static void Start() {
            GetSite.GetSite site = new GetSite.GetSite(@"C:\Users\Dick\Desktop\22");
            XmlDocument doc = Tools.XmlTool.LoadXml(Directory.GetCurrentDirectory() + "\\" + "Config.xml");
            XmlNode PostAddress = doc.SelectSingleNode(@"/root/PostAddress");
            XmlNode NodeCondition = doc.SelectSingleNode(@"/root/Condition");
            XmlNode NodeSite = doc.SelectSingleNode(@"/root/Site");
            XmlNode NodeStart = doc.SelectSingleNode(@"/root/Start");
            XmlNode NodePushCount = doc.SelectSingleNode(@"/root/PushCount");
            XmlNode NodeRecordTime = doc.SelectSingleNode(@"/root/ReCordTime");

            RecordTime = Convert.ToDateTime(NodeRecordTime.InnerText);
            PushCount = Convert.ToInt32(NodePushCount.InnerText);
            string Condition = NodeCondition.InnerText;
             Address = PostAddress.InnerText;
             Tag = Convert.ToInt32(NodeStart.InnerText);
            string Site = NodeSite.InnerText;
            SitePlus siteplus = site.GetUrlList("https://www.ptt.cc/bbs/" + Site + "/index" + Tag + ".html");
            List<SiteInfo> SiteInfoList = new List<SiteInfo>();
            Recursive(Tag, siteplus, SiteInfoList, Site, "/bbs/" + Site + "/index", Condition);             //"SiteInfoList=" + 
                     
            //List<SiteInfo> temp = new List<SiteInfo>();          
            //foreach (SiteInfo info in SiteInfoList)
            //{
            //    temp.Add(info);

            //    //if (temp.Count > 9)
            //    //{
            //        //Console.WriteLine(info.Title);
            //        //Console.WriteLine(info.PostDate);
            //        //Console.Write(info.Context);
            //        long length = POST(Address, temp);
             
            //        temp.Clear();
            //    //}
            //}

            //long length = POST(Address, SiteInfoList);

            //if(length > 0)
                //Console.WriteLine("OK");
                //NodeStart.InnerText = Tag.ToString();
                //NodeRecordTime.InnerText = RecordTime.ToString("yyyy/MM/dd HH:mm:ss");
            XmlDocument Recorddoc = Tools.XmlTool.LoadXml(Directory.GetCurrentDirectory() + "\\" + "Record.xml");
              XmlNode root =doc.SelectSingleNode("root");
              foreach (string str in RecordList)
              {
                  XmlElement element = Recorddoc.CreateElement("Title");
                  element.InnerText = str;
                  root.AppendChild(element);
              }
              Recorddoc.Save(Directory.GetCurrentDirectory() + "\\" + "Record.xml");

             doc.Save(Directory.GetCurrentDirectory() + "\\" + "Config.xml");
        }

        private static long POST(string Address, List<SiteInfo> SiteInfoList) {
            //"SiteInfoList =" +
            string strJson = "SiteInfoList=" + JsonConvert.SerializeObject(SiteInfoList, Newtonsoft.Json.Formatting.Indented);
            //byte[] postBytes = Encoding.ASCII.GetBytes(strJson);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Address);
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://dickguo.net63.net/chat/getInfo/");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            //System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] byteArray = Encoding.UTF8.GetBytes(strJson);

            //byte[] byteArray = Encoding.ASCII.GetBytes(strJson);
            request.ContentLength = byteArray.Length;


            //System.Net.WebResponse resp = request.GetResponse();
            //if(resp == null) {
            //    Console.WriteLine("Error");
            //}
            //else {
            //    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            //    Console.Write(sr.ReadToEnd().Trim());
            //}

            //request.ContentType = @"application/json";

            using(Stream dataStream = request.GetRequestStream()) {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try {
                using(HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                    Stream receiveStream = response.GetResponseStream();
                    Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                    StreamReader readStream = new StreamReader(receiveStream, encode);
                    length = response.ContentLength;
                    string st= readStream.ReadToEnd();
                    if (st.IndexOf("Dis") != -1)
                    {
                        DateTime date = DateTime.Now;
                        StreamWriter sw = new StreamWriter(@"C:\Users\Dick\Desktop\22\" + date.ToString("yyyymmddHHmmss") + ".txt");
                        sw.Write(strJson);
                        sw.Close();
                    }
                }
            }
            catch(WebException ex) {
                Console.WriteLine(ex.ToString());
                // Log exception and throw as for GET example above
             
            }

            
            return length;
        }

        /// <summary>
        /// 遞回所有頁面
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pSiteplus"></param>
        /// <param name="li"></param>
        /// <param name="Formate"></param>       
        static void Recursive(int index, SitePlus pSiteplus,List<SiteInfo> li,string Site,string Formate,string pCondition)
        {
            GetSite.GetSite site = new GetSite.GetSite(@"C:\Users\Dick\Desktop\22");
            Console.WriteLine(index.ToString());
            foreach (string str in pSiteplus.Context)
            {
                GetSite.SiteInfo info = site.GetInfo(@"https://www.ptt.cc" + str);
                if (info.Title != null)
                {
                    if(info.Title.IndexOf(pCondition) != -1 && info.PushList.Count > PushCount)
                    {
                        Thread.Sleep(1000);
                        if (info.Title.IndexOf("Re: ") == -1)
                        { 
                       info.PushList.Clear();
                       li.Add(info);
                       //if (info.PostDate > RecordTime)
                       //{                           
                       if (!RecordList.Contains(info.Title))
                       {
                           long length = POST(Address, li);
                        
                           RecordList.Add(info.Title);
                           RecordTime = info.PostDate;
                       }
                       //}
                       li.Clear();
                        }
                    }
                }
            }

            foreach (string str in pSiteplus.Index)
            {
                string temp = str.Replace(Formate, "").Replace(".html", "");
                int result = 0;
                if (int.TryParse(temp, out result))
                {
                    if (result > index)
                    {
                        Thread.Sleep(1000);
                        Tag = result;

                        pSiteplus = site.GetUrlList("https://www.ptt.cc/bbs/" + Site + "/index" + result + ".html");
                        Recursive(result, pSiteplus, li, Site, Formate, pCondition);
                    }
                }
            }
        }

    }

    
}
