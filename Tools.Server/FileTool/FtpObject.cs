using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CommTool {

    public class FtpObject : ObjectBase {

        string _Server;
        string _UserName;
        string _Password;
        string[] _FtpDirectoyArray = new string[] { "customer", "adviser", "other", "ea", "Account", "Import" };//("customer", "adviser", "other", "ea", "Account");//FtpDirectoy 轉字串用

        public FtpObject() {
            object FtpServer = string.Empty;
            object FtpUser = string.Empty;
            object FtpPassword = string.Empty;

            try {
                ObjectUtility.ReadRegistry("FTP", ref FtpServer);
            }
            catch (Exception ex) {
                FtpServer = "127.0.0.1";
            }

            try {
                ObjectUtility.ReadRegistry("FLoginID", ref FtpUser);
            }
            catch (Exception ex) {
                FtpUser = "ftp";
            }

            try {
                ObjectUtility.ReadRegistry("FPassword", ref  FtpPassword);
            }
            catch (Exception ex) {
                FtpPassword = "dick";
            }

            _Server = (string)FtpServer;
            _UserName = (string)FtpUser;
            _Password = (string)FtpPassword;
        }

        public FtpObject(string server, string userName, string password) {
            _Server = server;
            _UserName = userName;
            _Password = password;
        }

        ~FtpObject() {

        }

        /// <summary>UploadFileToFTP</summary>
        /// <param name="fileName">exists filename (ex:XXX.jpg)</param>
        /// <param name="filePath">filename's path</param>
        /// <param name="directory">UsgFx.FtpDirectoy.</param>
        /// <param name="ftpFileName">ftp filename (ex:XXX.jpg)</param>
        /// <returns>bool</returns>
        public bool UploadFileToFTP(string fileName, string filePath, FtpDirectory directory, string ftpFileName) {
            bool ReturnBool = false;
            try {
                /* Create an FTP Request */
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create("ftp://" + _Server + "/" + _FtpDirectoyArray[(int)directory] + "/" + ftpFileName);

                /* Log in to the FTP Server with the User Name and Password Provided */
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(_UserName, _Password);

                /* When in doubt, use these options */
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = true;

                //Load the file
                FileStream stream = File.OpenRead(filePath + "\\" + fileName);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();

                /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                try {
                    //Upload file
                    Stream reqStream = request.GetRequestStream();
                    reqStream.Write(buffer, 0, buffer.Length);
                    reqStream.Close();
                    ReturnBool = true;
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            }
            catch (WebException ex) {
                ToolLog.Log(ex);
                throw new Exception("FTP-Exception-" + ex.Message);                           //Error throw Exception
            }
            finally {
            }
            return ReturnBool;
        }

        public bool UploadFileToFTP(string fileName, string filePath, string directory, string ftpFileName) {
            bool ReturnBool = false;
            try {             
                /* Create an FTP Request */
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create("ftp://" + _Server + "/" + directory + "/" + ftpFileName);

                /* Log in to the FTP Server with the User Name and Password Provided */
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(_UserName, _Password);

                /* When in doubt, use these options */
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = true;
                request.EnableSsl = true;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

                //Load the file
                FileStream stream = File.OpenRead(filePath + "\\" + fileName);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();

                /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                try {
                    //Upload file                   
                    Stream reqStream = request.GetRequestStream();
                    reqStream.Write(buffer, 0, buffer.Length);
                    reqStream.Close();
                    ReturnBool = true;
                }
                catch (Exception ex) {
                    ToolLog.Log(ex);
                    Console.WriteLine(ex.ToString());
                }
            }
            catch (WebException ex) {
                ToolLog.Log(ex);
                throw new Exception("FTP-Exception-" + ex.Message);                           //Error throw Exception
            }
            finally {
            }
            return ReturnBool;
        }

        public static bool ValidateServerCertificate(object sender,X509Certificate certificate, X509Chain chain,SslPolicyErrors sslPolicyErrors) {
            if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors) {
                return false;
            }
            else if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateNameMismatch) {
                System.Security.Policy.Zone z = System.Security.Policy.Zone.CreateFromUrl
                   (((HttpWebRequest)sender).RequestUri.ToString());
                if (z.SecurityZone == System.Security.SecurityZone.Intranet || z.SecurityZone == System.Security.SecurityZone.MyComputer) {
                    return true;
                }
                return false;
            }
            return true;
        }

        /// <summary>DeleteFile </summary>
        /// <param name="directory">FtpDirectory</param>
        /// <param name="ftpFileName">ftpFileName</param>
        /// <returns></returns>
        public bool DeleteFile(FtpDirectory directory, string ftpFileName) {
            bool ReturnBool = false;
            FtpWebRequest FtpRequest = null;
            FtpWebResponse FtpResponse = null;
            try {
                //	Uri FileURL = new Uri("ftp://" + _Server + "/" + _FtpDirectoyArray[(int)directory] + "/" + ftpFileName);
                /* Create an FTP Request */
                FtpRequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + _Server + "/" + _FtpDirectoyArray[(int)directory] + "/" + ftpFileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                FtpRequest.Credentials = new NetworkCredential(_UserName, _Password);
                /* When in doubt, use these options */
                FtpRequest.UseBinary = true;
                FtpRequest.UsePassive = true;
                FtpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                FtpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                /* Establish Return Communication with the FTP Server */
                FtpResponse = (FtpWebResponse)FtpRequest.GetResponse();
                /* Resource Cleanup */
                ReturnBool = true;
            }
            catch (WebException ex) {
                ToolLog.Log(ex);
                throw new Exception("FTP-Exception-" + ex.Message);                           //Error throw Exception
            }
            finally {
                if (FtpResponse != null) {
                    FtpResponse.Close();
                }
                if (FtpRequest != null) {
                    FtpRequest = null;
                }
            }
            return ReturnBool;
        }
       
        /// <summary>DownloadFileFromFTP</summary>
        /// <param name="directory">UsgFx.FtpDirectoy.</param>
        /// <param name="ftpFileName">ftp filename (ex:XXX.jpg)</param>
        /// <param name="fileName">exists filename (ex:XXX.jpg)</param>
        /// <param name="filePath">filename's path</param>
        /// <returns>bool</returns>
        public bool DownloadFileFromFTP(FtpDirectory directory, string ftpFileName, string fileName, string filePath) {
            bool ReturnBool = false;
            FileStream TargetStream = null;
            Stream ResponseStream = null;
            FtpWebRequest FtpRequest = null;            
            try {
                FtpRequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + _Server + "/" + _FtpDirectoyArray[(int)directory] + "/" + ftpFileName);
                FtpRequest.Credentials = new NetworkCredential(_UserName, _Password);                
                if (fileName == string.Empty) {
                    fileName = ftpFileName;
                }

                FtpRequest.UseBinary = true;
                FtpRequest.UsePassive = true;
                FtpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                FtpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                
                /* Establish Return Communication with the FTP Server */
                FtpWebResponse FtpResponse = (FtpWebResponse)FtpRequest.GetResponse();
                /* Get the FTP Server's Response Stream */
                ResponseStream = FtpResponse.GetResponseStream();
                /* Open a File Stream to Write the Downloaded File */
                TargetStream = new FileStream(filePath + fileName, FileMode.Create);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[2048];
                int bytesRead = ResponseStream.Read(byteBuffer, 0, 2048);
                /* Download the File by Writing the Buffered Data Until the Transfer is Complete */
                try {
                    while (bytesRead > 0) {
                        TargetStream.Write(byteBuffer, 0, bytesRead);
                        bytesRead = ResponseStream.Read(byteBuffer, 0, 2048);
                    }
                }
                catch (Exception ex) {
                    ToolLog.Log(ex);
                    Console.WriteLine(ex.ToString()); 
                }                
            }
            catch (WebException ex) {
                //Error throw Exception
                throw new Exception("FTP-Exception-" + ex.Message);                         
            }
            finally {
                /* Resource Cleanup */
                if (TargetStream != null) {
                    TargetStream.Close();
                }
                if (ResponseStream != null) {
                    ResponseStream.Close();
                }
                if (FtpRequest != null) {
                    FtpRequest = null;
                }
            }
            ReturnBool = true;
            return ReturnBool;
        }

    }

    public enum FtpDirectory {
        Customer = 0,
        Adviser = 1,
        Other = 2,
        EA = 3,
        Account = 4,
        Import = 5,
    }

}