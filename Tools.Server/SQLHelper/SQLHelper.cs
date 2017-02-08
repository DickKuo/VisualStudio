using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SQLHelper {
    public static class SHelper {
        private class Default {
            public const string sqlconnection = "sqlconnection";
            public const string ExtendedProperties = "ExtendedProperties";
            public const string Dirty = "Dirty";
            public const int FirstItem = 0;
            public const string ConnectionFormat = "Data Source=.;Initial Catalog={0};User Id={1};Password={2};";
        }
              
        public static string _sqlconnection { set; get; }

        private static string _dbip;

        private static string _dbname;

        private static string _password;

        private static string _username;
   
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

        public static string UserName {
            set {
                _username = value;
            }
        }

        public static string PassWord {
            set {
                _password = value;
            }
        }         
              
        /// <summary>建構子</summary>
        /// 20150324 add by Dick
        /// <param name="DicSetting"></param>
        public static void InitSHelper(Dictionary<string, string> DicSetting) {
            if (DicSetting.ContainsKey(Default.sqlconnection)) {
                _sqlconnection = DicSetting[Default.sqlconnection];
            }
        }

        /// <summary>建立連線字串</summary>
        /// 20160808 add by Dick
        public static void NewConnectionString() {
            _sqlconnection = string.Format(Default.ConnectionFormat,DBName,_username,_password);
        }

        /// <summary>建立連線字串</summary>
        /// 20160808 add by Dick
        /// <param name="DbName"></param>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        public static void NewConnectionString(string DbName,string UserName,string PassWord) {
            _sqlconnection = string.Format(Default.ConnectionFormat, DbName, UserName, PassWord);
        }

        /// <summary>設定連線字串</summary>
        /// 20160808 add by Dick
        /// <param name="SqlConnectionString"></param>
        public static void SetConnectionString(string SqlConnectionString) {
            _sqlconnection = SqlConnectionString;
        }

        /// <summary>回傳連線字串</summary>
        /// 20160808 add by Dick
        /// <returns></returns>
        public static string GetConnectionString() {
            return _sqlconnection;
        }

        /// <summary>執行SQL語法 回傳DataTable</summary>
        /// 20150324 add by Dick
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

        /// <summary>撈取資料庫內Table 的所有欄位，返回String 型態。 #72</summary>
        /// 20150709 add by Dick
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

        /// <summary>輸入SQl指令，回單一資料</summary>
        /// 20150703 add by Dick 
        /// <param name="pSqlString"></param>
        /// <returns></returns>
        public static object ExeGetSingleResult(string pSqlString) {
            DataTable dt = ExeDataTable(pSqlString);
            if (dt != null && dt.Rows.Count > 0) {
                return dt.Rows[Default.FirstItem][Default.FirstItem];
            }
            else {
                return default(object);
            }
        }

        /// <summary>SQL執行使用Parameter 方式</summary>
        /// 20150324 add by Dick 
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

        /// <summary>SQL執行使用Parameter 方式</summary>
        /// 20150324 add by Dick 
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

        /// <summary>批量新增功能</summary>
        /// 20150724 add by Dick for #75  
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
    }

    public class UseStoreProcedure {

        private class Default {
            public const string MsUserName = "MsUserName";
            public const string MsPassWord = "MsPassWord";
            public const string DBName = "DBName";
            public const string ConnectionFormat = "Data Source=.;Initial Catalog={0};User Id={1};Password={2};";
        }

        //private  Dictionary<string, object>  _parameters    = new Dictionary<string,object>();
        private List<string> _OutParameter = new List<string>();

        SqlCommand Scmd = null;

        /// <summary>連線字串</summary>
        public string ConnectiinString {
            set {
                _ConnetionString = value;
            }
            get {
                return _ConnetionString;
            }
        }
        private string _ConnetionString = string.Empty;

        /// <summary>執行Store Procedure 後的結果</summary>
        public string Result { get { return CommandResult; } }
        private string CommandResult = string.Empty;

        /// <summary>最後一次執行Sql指令</summary>
        public string LastCommandString {
            get {
                return _LastCommandString;
            }
        }
        private string _LastCommandString = string.Empty;

        public List<string> OutParameterValues = new List<string>();

        public UseStoreProcedure() {
            object DbName = new object();
            object PassWord = new object();
            object UserName = new object();
            CommTool.ObjectUtility.ReadRegistry(Default.DBName, ref DbName);
            CommTool.ObjectUtility.ReadRegistry(Default.MsUserName, ref UserName);
            CommTool.ObjectUtility.ReadRegistry(Default.MsPassWord, ref PassWord);
            _ConnetionString = string.Format(Default.ConnectionFormat, DbName.ToString(), UserName.ToString(), PassWord.ToString());
        }

        /// <summary>執行預存不傳回值</summary>
        /// <param name="StoreProcedureName"></param>
        public void ExeProcedureNotQuery(string StoreProcedureName) {
            SqlConnection con;
            ExeInit(StoreProcedureName, out con);
            try {
                con.Open();
                Scmd.ExecuteNonQuery();
                if (_OutParameter.Count > 0) {
                    foreach (string str in _OutParameter) {
                        OutParameterValues.Add(Scmd.Parameters[str].Value.ToString());
                    }
                }
                CommandResult = MessageType.Sucess;
            }
            catch (Exception ex) {
                CommandResult = ex.Message;
            }
            finally {
                Scmd.Parameters.Clear();
                Scmd.Cancel();
                Scmd.Dispose();
                con.Close();
                con.Dispose();
                _OutParameter.Clear();
            }
        }

        /// <summary>執行預存回傳單一結果</summary>
        /// <param name="StoreProcedureName"></param>
        /// <param name="OutParameter"></param>
        /// <returns></returns>
        public string ExeProcedureHasResult(string StoreProcedureName) {
            SqlConnection con;
            ExeInit(StoreProcedureName, out con);
            try {
                con.Open();
                Scmd.ExecuteNonQuery();
                if (_OutParameter.Count > 0) {
                    foreach (string str in _OutParameter) {
                        OutParameterValues.Add(Scmd.Parameters[str].Value.ToString());
                    }
                }
                CommandResult = MessageType.Sucess;
            }
            catch (Exception ex) {
                CommandResult = ex.Message;
            }
            finally {
                Scmd.Parameters.Clear();
                Scmd.Cancel();
                Scmd.Dispose();
                con.Close();
                con.Dispose();
                _OutParameter.Clear();
            }
            return CommandResult;
        }

        /// <summary>取得資料 回傳DataTable</summary>
        /// <param name="StoreProcedureName">預存名稱</param>
        /// <returns></returns>
        public DataTable ExeProcedureGetDataTable(string StoreProcedureName) {
            SqlConnection con;
            ExeInit(StoreProcedureName, out con);
            DataTable dt = new DataTable();
            try {
                con.Open();
                dt.Load(Scmd.ExecuteReader());
                CommandResult = MessageType.Sucess;
                if (_OutParameter.Count > 0) {
                    foreach (string str in _OutParameter) {
                        OutParameterValues.Add(Scmd.Parameters[str].Value.ToString());
                    }
                }
            }
            catch (Exception ex) {
                CommandResult = ex.Message;
            }
            finally {
                Scmd.Parameters.Clear();
                Scmd.Cancel();
                Scmd.Dispose();
                con.Close();
                con.Dispose();
                _OutParameter.Clear();
            }
            return dt;
        }

        /// <summary>執行StoreProcedure並取得Key值</summary>
        /// 20170208 add by Dick
        /// <param name="StoreProcedureName"></param>
        /// <returns></returns>
        public int ExeProcedureReturnKey(string StoreProcedureName) {
            DataTable dt = this.ExeProcedureGetDataTable(StoreProcedureName);
            int Result = -1;
            if (dt != null && dt.Rows.Count > 0) {
                if (dt.Rows[0][0] != null && dt.Rows[0][0].ToString() != string.Empty) {
                    Result = Int32.Parse(dt.Rows[0][0].ToString());
                }
            }
            return Result;
        }

        private void ExeInit(string StoreProcedureName, out SqlConnection con) {
            if (Scmd == null) {
                Scmd = new SqlCommand();
            }
            con = new SqlConnection(this._ConnetionString);
            Scmd.Connection = con;
            Scmd.CommandText = StoreProcedureName;
            Scmd.CommandType = CommandType.StoredProcedure;
            OutParameterValues.Clear();
            CommandResult = string.Empty;
            _LastCommandString = Scmd.CommandText;
        }

        /// <summary>加入參數</summary>
        /// <param name="parameterName">參數名稱</param>
        /// <param name="value">參數值</param>
        public void AddParameter(string parameterName, object value, System.Data.SqlDbType? type = System.Data.SqlDbType.NVarChar, int? Len = 20, System.Data.ParameterDirection? Direction = System.Data.ParameterDirection.Input) {
            if (Scmd == null) {
                Scmd = new SqlCommand();
            }
            Scmd.Parameters.Add(parameterName, value);
            if (Direction == System.Data.ParameterDirection.Output) {
                _OutParameter.Add(parameterName);
            }
        }
    }

    public class SqlParameters {
        public string ParameterName { set; get; }
        public object Value { set; get; }
        public System.Data.SqlDbType Type { set; get; }
        public int Length { set; get; }
        public System.Data.ParameterDirection ParamDirection { set; get; }
    }

    public class MessageType {
        public const string LogInFail = "Login Failed";
        public const string LoginSucess = "Login Sucess";
        public const string Parameter = "Parameter Add Error";
        public const string Sucess = "Store Procedure Execute Sucess";
    }

    public struct iIndex {
        public string Name;  //索引名
        public bool IsUnique;  //是否唯一
        public bool IsClustered;  //是否聚集索引
        public string ColumnName;  //列名
        public bool IsDesc; //是否降序
    }

    public interface ISQLMataRepair {
    }

    public class SqlMataData : ISQLMataRepair {
    }

}
