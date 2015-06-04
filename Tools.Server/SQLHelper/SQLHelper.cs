using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SQLHelper
{
    public static class SHelper
    {
        #region 變數宣告
        public static string _sqlconnection { set; get; }
        #endregion

        #region 共用函式

        /// <summary>
        /// 20150324 建構子
        /// </summary>
        /// <param name="DicSetting"></param>
        public static void InitSHelper(Dictionary<string, string> DicSetting)
        {
            if (DicSetting.ContainsKey("sqlconnection"))
            {
                _sqlconnection = DicSetting["sqlconnection"];
            }
        }

        /// <summary>
        /// 20150324 執行SQL語法 回傳DataTable
        /// </summary>
        /// <param name="SqlSrting">輸入SQL指令</param>
        /// <returns>回傳撈取資料</returns>
        public static DataTable ExeDataTable(string SqlSrting)
        {
            DataTable dt = new DataTable();
            SqlCommand scm = null;
            try
            {
                using (SqlConnection scon = new SqlConnection(_sqlconnection))
                {
                    scon.Open();
                    scm = new SqlCommand(SqlSrting, scon);
                    dt.Load(scm.ExecuteReader());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }

        /// <summary>
        /// 20150324 add by Dick for SQL執行使用Parameter 方式。
        /// </summary>
        /// <param name="SqlSrting"></param>
        /// <param name="DicParameters"></param>
        /// <returns></returns>
        public static DataTable ExeDataTableUseParameter(string SqlSrting,Dictionary<string,object> DicParameters)
        {
            DataTable dt = new DataTable();
            SqlCommand scm = null;
            try
            {
                using (SqlConnection scon = new SqlConnection(_sqlconnection))
                {
                    scon.Open();
                    scm = new SqlCommand(SqlSrting, scon);
                    foreach (string ParamName in DicParameters.Keys)
                    {
                        scm.Parameters.Add(ParamName, DicParameters[ParamName]);
                    }
                    dt.Load(scm.ExecuteReader());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }

        /// <summary>
        /// 20150324 執行SQL 無傳回值
        /// </summary>
        /// <param name="SqlSrting">輸入SQL指令</param>
        public static void ExeNoQuery(string SqlSrting)
        {
            SqlCommand scm = null;
            try
            {
                using (SqlConnection scon = new SqlConnection(_sqlconnection))
                {
                    scon.Open();
                    scm = new SqlCommand(SqlSrting, scon);
                    scm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 20150324 add by Dick for SQL執行使用Parameter 方式。
        /// </summary>
        /// <param name="SqlSrting"></param>
        /// <param name="DicParameters"></param>
        /// <returns></returns>
        public static void ExeNoQueryUseParameter(string SqlSrting, Dictionary<string, object> DicParameters)
        {
            try
            {
                using (SqlConnection scon = new SqlConnection(_sqlconnection))
                {
                    scon.Open();
                    SqlCommand scm = new SqlCommand(SqlSrting, scon);
                    foreach (string ParamName in DicParameters.Keys)
                    {
                        scm.Parameters.Add(ParamName, DicParameters[ParamName]);
                    }
                    scm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }          
        }
        
        #endregion

    }

    public struct iIndex {
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
