using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace WebInfo
{
    public class WebInfo
    {




        public StreamReader GetResponse(string pUrl)
        {
            WebRequest myWebRequest = WebRequest.Create(pUrl);
            myWebRequest.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)myWebRequest.GetResponse();
            Stream DataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(DataStream, Encoding.Default);
            return reader;
        }



    }

    
}
