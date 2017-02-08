using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace CommTool {
    public class MailData {
        private class Default {           
            public const string MailAccount = "MailAccount";
            public const string MailPassWord = "MailPassWord";
        }

        private class SP {
            public const string GetSendMail = "GetSendMail";
        }

        SQLHelper.UseStoreProcedure USP = new SQLHelper.UseStoreProcedure();

        private string _logPath=@"D:\Log";

        private string _account= string.Empty;

        private string _password = string.Empty;

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

        public MailData() {
        }

        public MailData(string _LogPath)
        {
            _logPath = _LogPath;
        }

        public MailData(string _LogPath, string account, string password)
        {
            Account = account;
            PassWord = password;
            _logPath = _LogPath;
        }

        public void Send(string receiver,string subject,string body)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(receiver);//收件者，以逗號分隔不同收件者
            //msg.CC.Add("c@msn.com");//副本
            //msg.Bcc.Add("d@yahoo.com");//密件副本
            //3個參數分別是發件人地址（可以隨便寫），發件人姓名，編碼
            msg.From = new MailAddress(_account, "Dick股神系統", System.Text.Encoding.UTF8);
            msg.Subject = subject;//郵件標題 
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//郵件標題編碼 
            msg.Body = body;//郵件內容 
            msg.BodyEncoding = System.Text.Encoding.UTF8;//郵件內容編碼 
            msg.IsBodyHtml = false;//是否是HTML郵件 
            msg.Priority = MailPriority.Normal;//郵件優先級 
            try
            {
                //建立 SmtpClient 物件 並設定 Gmail的smtp主機及Port 
                SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
                //設定你的帳號密碼
                //Gmial 的 smtp 使用 SSL
                MySmtp.Credentials = new System.Net.NetworkCredential(_account, _password);
                MySmtp.EnableSsl = true;
                //發送Email
                //MySmtp.Send("", "", "C# Gmail發信測試", "文件內容");
                MySmtp.Send(msg);
                ToolLog.Log(LogType.Mail, "發送信件");
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                ToolLog.Log(ex);           
            }
        }

        /// <summary>抓Registry進行發送Mail</summary>
        /// <param name="receiver"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public void RegistrySend(string receiver, string subject, string body) {
            MailMessage msg = new MailMessage();
            msg.To.Add(receiver);//收件者，以逗號分隔不同收件者
            //msg.CC.Add("c@msn.com");//副本
            //msg.Bcc.Add("d@yahoo.com");//密件副本

            //3個參數分別是發件人地址（可以隨便寫），發件人姓名，編碼          

            msg.Subject = subject;//郵件標題 
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//郵件標題編碼 
            msg.Body = body;//郵件內容 
            msg.BodyEncoding = System.Text.Encoding.UTF8;//郵件內容編碼 
            msg.IsBodyHtml = false;//是否是HTML郵件 
            msg.Priority = MailPriority.Normal;//郵件優先級 
            try {
                //建立 SmtpClient 物件 並設定 Gmail的smtp主機及Port 
                SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
                //設定你的帳號密碼
                object Account = new object();
                object PassWord = new object();
                ObjectUtility.ReadRegistry(Default.MailAccount, ref Account);
                ObjectUtility.ReadRegistry(Default.MailPassWord, ref PassWord);
                msg.From = new MailAddress(Account.ToString(), "Dick股神系統", System.Text.Encoding.UTF8);
                //Gmial 的 smtp 使用 SSL
                MySmtp.Credentials = new System.Net.NetworkCredential(Account.ToString(), PassWord.ToString());
                MySmtp.EnableSsl = true;
                //發送Email
                //MySmtp.Send("", "", "C# Gmail發信測試", "文件內容");
                MySmtp.Send(msg);
                ToolLog.Log(LogType.Mail, "發送信件");
            }
            catch (System.Net.Mail.SmtpException ex) {
                ToolLog.Log(ex);
            }           
        }

        /// <summary>取得要發送的Maill</summary>
        /// <returns></returns>
        public DataTable GetSendMail() {
            return USP.ExeProcedureGetDataTable(SP.GetSendMail);
        }

    }
}
