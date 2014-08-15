using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace Mail
{
    public class Mail
    {
        private string _logPath=@"D:\Log";

        private string _account="erpbank.dick";
        private string _password = "erp59107";

        public string PassWord {
            set {
                _password = value;
            }
        }

        public string Account
        {
            set
            {
                _account = value;
            }
        }

        public string LogPath {
            set {
                _logPath = value;
            }
            get {
                return _logPath;
            }
        }

        public Mail()
        { 
        
        }

        public Mail(string path)
        {
            _logPath = path;
        }

        public Mail(string path, string account, string password)
        {
            Account = account;
            PassWord = password;
            _logPath = path;
        }

        public void Send(string receiver,string subject,string body)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(receiver);//收件者，以逗號分隔不同收件者
            //msg.CC.Add("c@msn.com");//副本
            //msg.Bcc.Add("d@yahoo.com");//密件副本

            //3個參數分別是發件人地址（可以隨便寫），發件人姓名，編碼
            msg.From = new MailAddress("erpbank.dick@gmail.com", "Dick股神系統", System.Text.Encoding.UTF8);

            msg.Subject = subject;//郵件標題 
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//郵件標題編碼 
            msg.Body = body;//郵件內容 
            msg.BodyEncoding = System.Text.Encoding.UTF8;//郵件內容編碼 
            msg.IsBodyHtml = false;//是否是HTML郵件 
            msg.Priority = MailPriority.Normal;//郵件優先級 
            FileTool.ToolLog tool = new FileTool.ToolLog(_logPath);
            try
            {
                //建立 SmtpClient 物件 並設定 Gmail的smtp主機及Port 
                SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);

                //設定你的帳號密碼

                //Gmial 的 smtp 使用 SSL
                MySmtp.Credentials = new System.Net.NetworkCredential("erpbank.dick@gmail.com", "erp59107");

                MySmtp.EnableSsl = true;
                //發送Email
                //MySmtp.Send("", "", "C# Gmail發信測試", "文件內容");
                MySmtp.Send(msg);

                tool.Log(FileTool.LogType.Mail,"發送信件");
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                tool.Log(ex);                
            }
        }

    }
}
