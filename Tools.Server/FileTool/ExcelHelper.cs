using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CommTool
{
    public class ExcelHelper
    {        
        private bool isHDR = true;

        public bool IsHDR {
            get {
                return isHDR;
            }
            set {
                isHDR = value;
            }
        }

        /// <summary>20150130 Excel連線字串</summary>
        /// <param name="pExcelFilePath"></param>
        /// <param name="isAdditional"></param>
        /// <returns></returns>
        private string GetConnectString(string pExcelFilePath, bool isAdditional) {

            string connStr = string.Empty;

            bool isOccupied = false;

            string msg = string.Empty;

            bool IsInstalled = true;

            connStr = "Provider = Microsoft.ACE.OLEDB.12.0 ; Data Source =" + pExcelFilePath + string.Format(";Extended Properties=\"Excel 12.0;HDR={0};IMEX=1\";", IsHDR ? "YES" : "No");
           
            try {
                using (OleDbConnection con = new OleDbConnection(connStr)) {
                    con.Open();
                    con.Close();
                    return connStr;
                }
            }
            catch (Exception ex) {
                if (ex is OleDbException) {
                    isOccupied = true;
                    msg += ex.Message + "\n";
                }
                else {
                    IsInstalled = false;
                }
            }
            return connStr;
        }

        public List<string> GetSheetsNameByOpenXml(string filePath) {
            List<string> sheetNames = new List<string>();
            FileStream pStream = File.OpenRead(filePath);
            pStream.Close();
            pStream.Dispose();
            return sheetNames;
        }
    }
}
