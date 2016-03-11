using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebApplication1.Models;
using Newtonsoft.Json;
using WebRoutingTest.Models.Code;

namespace WebApplication1.Code.DAO
{
    public class WebApi
    {
        private string ApiUrl = "";   //Api 位置   Bootstrap
        
        public WebApi()
        {
            bool IsLinqAPI = Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings["IsLinqApi"]);
            //if (IsLinqAPI)
            //{
            //    ApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["LinqApiUrl"].ToString();          
            //}
            //else
            //{
                ApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl"].ToString();
            //}
        }


        private class ApiSetting
        {
            public const string Controller = "Factory";
            public const string Action = "Factory";
        }


        protected string _uri { set; get; }                 //Api路徑


        protected string ModelName { set; get; }
        

        /// <summary>設定ApiUrl/// </summary>
        /// <param name="Uri"></param>
        public virtual void SetApiUrl(string Uri)
        {
            _uri = Uri;
        }


        /// <summary>APi使用方法 </summary>
        /// <param name="Operation"></param>
        /// <returns></returns>
        public string ApiMVC(ApiOperation Operation)
        {
            string Finalurl = string.Empty;
            StringBuilder sb = new StringBuilder();
            Finalurl = Operation.Uri;
            foreach (var pa in Operation.Parameters)
            {
                sb.AppendFormat("/{0}", pa);
            }

            if (this.ModelName != string.Empty)
            {
                Finalurl += "/" + this.ModelName;
            }
            if (Operation.Action != string.Empty)
            {
                Finalurl += "/" + Operation.Action;
            }
            if (sb.Length > 0)
            {
                Finalurl += sb.ToString();
            }
            string s = string.Empty;
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(Finalurl);
                webRequest.Method = Operation.Methode.Method;
                webRequest.ContentLength = 0;
                webRequest.ContentType = "application/json";
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                var reader = new StreamReader(webResponse.GetResponseStream());
                 s = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                LogDAO log = new LogDAO();
                log.LogMethod(s);
                s = ex.Message;
            }
         
            return s;
        }
        

        /// <summary> 共用Api </summary>
        /// <param name="Operation"></param>
        /// <returns></returns>
        public string ApiOperation(ApiOperation Operation)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Operation.Uri);
                    var result = client.PostAsJsonAsync(Operation.Uri, Operation.obj).Result;
                    var temp = JsonConvert.DeserializeObject<dynamic>(result.Content.ReadAsStringAsync().Result);
                    return Convert.ToString( temp);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary>初始化API</summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public ApiOperation Init(Operation Obj)
        {
            ApiOperation Oper = new ApiOperation();
            string _url = string.Format("{0}/{1}/{2}", ApiUrl, ApiSetting.Controller, ApiSetting.Action);
            Oper.Uri = _url;
            Oper.Methode = HttpMethod.Post;
            Oper.obj = Obj;
            return Oper;
        }

        /// <summary>串接金流Api</summary>
        /// <param name="Operation"></param>
        /// <returns></returns>
        public TradeInfo SaveMoney(ApiOperation Operation)
        {
            var temp = ApiOperation(Operation);
            TradeInfo Info = JsonConvert.DeserializeObject<TradeInfo>(temp);
            return Info;
        }


      

       



    }
}