using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CommTool.Business.Services;

namespace SQLHelper
{
    public static class SHelper
    {
        private class Default {
            public const string sqlconnection = "sqlconnection";
            public const string ExtendedProperties = "ExtendedProperties";
            public const string Dirty = "Dirty";
        }

        #region 變數宣告

        public static string _sqlconnection { set; get; }
        private static string _dbip;
        private static string _dbname;
        #endregion
        
        #region public

        public static string DBIP {
            set {
                _dbip = value;
            }
            get {
                return _dbip;
            }
        }
        public static string DBName {
            set {
                _dbname = value;
            }
            get {
                return _dbname;
            }
        }

        #endregion

        #region 共用函式

        /// <summary>20150324 建構子</summary>
        /// <param name="DicSetting"></param>
        public static void InitSHelper(Dictionary<string, string> DicSetting) {
            if (DicSetting.ContainsKey(Default.sqlconnection)) {
                _sqlconnection = DicSetting[Default.sqlconnection];
            }
        }

        /// <summary>20150324 執行SQL語法 回傳DataTable</summary>
        /// <param name="SqlSrting">輸入SQL指令</param>
        /// <returns>回傳撈取資料</returns>
        public static DataTable ExeDataTable(string SqlSrting) {
            DataTable dt = new DataTable();
            SqlCommand scm = null;
            try {
                using (SqlConnection scon = new SqlConnection(_sqlconnection)) {
                    scon.Open();
                    scm = new SqlCommand(SqlSrting, scon);
                    dt.Load(scm.ExecuteReader());
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return dt;
        }

        /// <summary>20150709 新增功能，撈取資料庫內Table 的所有欄位，返回String 型態。 #72</summary>
        /// <param name="pTableName"></param>
        /// <returns></returns>
        public static string GetTableColumns(string pTableName) {
            StringBuilder result = new StringBuilder();
            string sql = string.Format(@"SELECT COLUMN_NAME
                            FROM {0}.INFORMATION_SCHEMA.COLUMNS
                            WHERE TABLE_NAME = N'{1}'", DBName, pTableName);
            DataTable dt = ExeDataTable(sql);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows) {
                sb.AppendLine(string.Format(",[{0}]", dr[0].ToString()));
            }
            if (sb.Length > 0) {
                result.AppendLine("Select ");
                result.Append(sb.ToString().Remove(0, 1));
                result.AppendFormat(" From {0}", pTableName);
            }
            return result.ToString();
        }

        /// <summary>20150703 輸入SQl指令，回單一資料。</summary>
        /// <param name="pSqlString"></param>
        /// <returns></returns>
        public static object ExeGetSingleResult(string pSqlString) {
            DataTable dt = ExeDataTable(pSqlString);
            if (dt != null && dt.Rows.Count > 0) {
                return dt.Rows[0][0];
            }
            else {
                return default(object);
            }
        }

        /// <summary>20150324 add by Dick for SQL執行使用Parameter 方式。</summary>
        /// <param name="SqlSrting"></param>
        /// <param name="DicParameters"></param>
        /// <returns></returns>
        public static DataTable ExeDataTableUseParameter(string SqlSrting, Dictionary<string, object> DicParameters) {
            DataTable dt = new DataTable();
            SqlCommand scm = null;
            try {
                using (SqlConnection scon = new SqlConnection(_sqlconnection)) {
                    scon.Open();
                    scm = new SqlCommand(SqlSrting, scon);
                    foreach (string ParamName in DicParameters.Keys) {
                        scm.Parameters.Add(ParamName, DicParameters[ParamName]);
                    }
                    dt.Load(scm.ExecuteReader());
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return dt;
        }

        /// <summary>20150324 執行SQL 無傳回值</summary>
        /// <param name="SqlSrting">輸入SQL指令</param>
        public static void ExeNoQuery(string SqlSrting) {
            SqlCommand scm = null;
            try {
                using (SqlConnection scon = new SqlConnection(_sqlconnection)) {
                    scon.Open();
                    scm = new SqlCommand(SqlSrting, scon);
                    scm.ExecuteNonQuery();
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>20150324 add by Dick for SQL執行使用Parameter 方式。</summary>
        /// <param name="SqlSrting"></param>
        /// <param name="DicParameters"></param>
        /// <returns></returns>
        public static void ExeNoQueryUseParameter(string SqlSrting, Dictionary<string, object> DicParameters) {
            try {
                using (SqlConnection scon = new SqlConnection(_sqlconnection)) {
                    scon.Open();
                    SqlCommand scm = new SqlCommand(SqlSrting, scon);
                    foreach (string ParamName in DicParameters.Keys) {
                        scm.Parameters.Add(ParamName, DicParameters[ParamName]);
                    }
                    scm.ExecuteNonQuery();
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>20150724 add by Dick for #75  批量新增功能。</summary>
        /// <param name="dt"></param>
        public static void SqlBulkCopy(DataTable dt) {
            try {
                using (SqlConnection scon = new SqlConnection(_sqlconnection)) {
                    scon.Open();
                    using (SqlBulkCopy sqlBC = new SqlBulkCopy(scon)) {
                        //設定一個批次量寫入多少筆資料
                        sqlBC.BatchSize = 1000;
                        //設定逾時的秒數
                        sqlBC.BulkCopyTimeout = 60;
                        //設定 NotifyAfter 屬性，以便在每複製 10000 個資料列至資料表後，呼叫事件處理常式。
                        sqlBC.NotifyAfter = 10000;
                        //sqlBC.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                        //設定要寫入的資料庫
                        sqlBC.DestinationTableName = dt.TableName;
                        //對應資料行
                        foreach (DataColumn dc in dt.Columns) {
                            switch (dc.ColumnName) {
                                case Default.ExtendedProperties:
                                case Default.Dirty:
                                    continue;
                            }
                            sqlBC.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                        }
                        //開始寫入
                        dt.Columns.Remove(Default.ExtendedProperties);
                        dt.Columns.Remove(Default.Dirty);
                        sqlBC.WriteToServer(dt);
                    }
                    scon.Dispose();
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }

    public struct iIndex
    {
        public string Name;  //索引名
        public bool IsUnique;  //是否唯一
        public bool IsClustered;  //是否聚集索引
        public string ColumnName;  //列名
        public bool IsDesc; //是否降序
    }

    public interface ISQLMataRepair
    {
    }

    public class SqlMataData : ISQLMataRepair
    {
    }

}
