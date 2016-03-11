using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication1.Code.DAO;

namespace WebApplication1.Models
{
    public class UseStoreProcedure 
    {     
        
       //private  Dictionary<string, object>  _parameters    = new Dictionary<string,object>();
       private List<string> _OutParameter = new List<string>();
       SqlCommand Scmd = null;

       /// <summary>
       /// 連線字串
       /// </summary>
       public string ConnectiinString { set {
           _ConnetionString = value;
       }
           get {
               return _ConnetionString;
           }
       }
       private string _ConnetionString = string.Empty;

       /// <summary>
       /// 執行Store Procedure 後的結果
       /// </summary>
       public string Result { get { return CommandResult; } }
       private string CommandResult = string.Empty;

       /// <summary>
       /// 最後一次執行Sql指令
       /// </summary>
       public string LastCommandString
       {
           get {
               return _LastCommandString;
           }        
       }
       private string _LastCommandString = string.Empty;


       public List<string> OutParameterValues = new List<string>();


       /// <summary>
       /// 執行預存部傳回值
       /// </summary>
       /// <param name="StoreProcedureName"></param>
       public void ExeProcedureNotQuery(string StoreProcedureName)
       {           
           SqlConnection con;
           ExeInit(StoreProcedureName, out con);
           ////SetParameter(scmd);
           try
           {
               con.Open();
               Scmd.ExecuteNonQuery();               
               if (_OutParameter.Count > 0)
               {
                   foreach (string str in _OutParameter)
                   {
                       OutParameterValues.Add(Scmd.Parameters[str].Value.ToString());
                   }
               }
               CommandResult = MessageType.Sucess;              
           }
           catch (Exception ex)
           {
               CommandResult = ex.Message;
           }
           finally
           {
               Scmd.Parameters.Clear();
               Scmd.Cancel();
               Scmd.Dispose();
               con.Close();
               con.Dispose();
               _OutParameter.Clear();
           }
       }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="StoreProcedureName"></param>
       /// <param name="OutParameter"></param>
       /// <returns></returns>
       public string ExeProcedureHasResult(string StoreProcedureName)
       {          
           SqlConnection con;           
           ExeInit(StoreProcedureName, out con);                     
           try
           {
               con.Open();
               Scmd.ExecuteNonQuery();   
               if(_OutParameter.Count>0)
               {
                   foreach(string str in _OutParameter)
                   {
                       OutParameterValues.Add(Scmd.Parameters[str].Value.ToString());
                   }
               }
               CommandResult = MessageType.Sucess;           
           }
           catch (Exception ex)
           {
               CommandResult = ex.Message;
           }
           finally
           {
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
       public DataTable ExeProcedureGetDataTable(string StoreProcedureName)
       {
           SqlConnection con;
           
           ExeInit(StoreProcedureName, out con);
           //SetParameter(scmd);
           DataTable dt =new DataTable();           
           try
           {
               con.Open();
               dt.Load(Scmd.ExecuteReader());
               CommandResult = MessageType.Sucess;
               if (_OutParameter.Count > 0)
               {
                   foreach (string str in _OutParameter)
                   {
                       OutParameterValues.Add(Scmd.Parameters[str].Value.ToString());
                   }
               }
           }
           catch (Exception ex)
           {
               CommandResult = ex.Message;
           }
           finally
           {
               Scmd.Parameters.Clear();
               Scmd.Cancel();
               Scmd.Dispose();
               con.Close();
               con.Dispose();
               _OutParameter.Clear();
           }
           return dt;
       }


       private void ExeInit(string StoreProcedureName, out SqlConnection con)
       {
           if (Scmd == null)
           {
               Scmd = new SqlCommand();
           }
           con = new SqlConnection(this._ConnetionString);
           Scmd.Connection = con;
           Scmd.CommandText = StoreProcedureName;
               //new SqlCommand(StoreProcedureName, con);
           Scmd.CommandType = CommandType.StoredProcedure;
           OutParameterValues.Clear();
           CommandResult = string.Empty;
           _LastCommandString = Scmd.CommandText;
       }





       /// <summary>
       /// 加入參數
       /// </summary>
       /// <param name="parameterName">參數名稱</param>
       /// <param name="value">參數值</param>
       public void AddParameter(string parameterName, object value, System.Data.SqlDbType? type = System.Data.SqlDbType.NVarChar, int? Len = 20, System.Data.ParameterDirection? Direction = System.Data.ParameterDirection.Input)
       {
           if (Scmd == null)
           {
               Scmd = new SqlCommand();
           }
           Scmd.Parameters.Add(parameterName, value);
           if (Direction == System.Data.ParameterDirection.Output)
           {             
               _OutParameter.Add(parameterName);
           }
       }

       // /// <summary>
       // /// 設定已加入的參數進去
       // /// </summary>
       // /// <param name="scmd"></param>
       //private void SetParameter(SqlCommand scmd)
       //{
       //    foreach (var item in _parameters)
       //    {               
       //        scmd.Parameters.Add(item.Key,item.Value);
       //    }
       //}


    }


    public class SqlParameters {
        public string ParameterName { set; get; }
        public object Value { set; get; }
        public System.Data.SqlDbType Type { set; get; }
        public int Length { set; get; }
        public System.Data.ParameterDirection ParamDirection { set; get; }

    }


}