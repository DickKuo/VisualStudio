using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Net;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string text = "Use pixels to express measurements for padding and margins.";
            string from = "en";
            string to = "de";

            string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + text + "&from=" + from + "&to=" + to;
            string authToken = "Bearer" + " ";//+ admToken.access_token;

            HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Authorization", authToken);
        }

        protected override void OnStop()
        {
        }
    }
}
