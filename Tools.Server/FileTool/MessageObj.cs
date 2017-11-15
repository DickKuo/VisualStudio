using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CommTool {
    public class MessageObj {

        private class Default {
            public const string LineUserID = "LineUserID";
            public const string LineAccessToken = "LineAccessToken";
            public const string LineServer = "LineServer";
        }

        /// <summary>發送Line訊息</summary>
        /// <param name="message"></param>
        public void SendLineMessage(string message) {
            object LineUserID = new object();
            object LineAccessToken = new object();
            object LineServer = new object();
            
            ObjectUtility.ReadRegistry(Default.LineUserID, ref LineUserID);
            ObjectUtility.ReadRegistry(Default.LineAccessToken, ref LineAccessToken);
            ObjectUtility.ReadRegistry(Default.LineServer, ref LineServer);

            LineMessage line = new LineMessage();
            Message mmm = new Message();
            mmm.type = "text";
            mmm.text = message;
            line.to = LineUserID.ToString();
            line.messages.Add(mmm);

            WebRequest req = WebRequest.Create(LineServer.ToString());
            req.Method = "POST";
            req.ContentType = "application/json";
            req.Headers["Authorization"] = "Bearer " + LineAccessToken.ToString();
            try {              
                string Data = Newtonsoft.Json.JsonConvert.SerializeObject(line);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Data);
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)req.GetResponse();
                string responseStr = string.Empty;
                if (response.StatusCode == HttpStatusCode.OK) {
                    Stream responseStream = response.GetResponseStream();
                    responseStr = new StreamReader(responseStream).ReadToEnd();
                }
            }
            catch (Exception ex) {
                ToolLog.Log(ex);
            }
        }
    }

    public class LineMessage {

        public string to { set; get; }

        public List<Message> messages = new List<Message>();
    }

    public class Message {
        public string type { set; get; }

        public string text { set; get; }
    }
}
