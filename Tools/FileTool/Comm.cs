using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommTool
{
    public static class StringExtension
    {
        /// <summary>
        /// 將GUID轉換成String 
        /// </summary>
        /// <param name="pID"></param>
        /// <returns></returns>
        public static string ConvertToString(this System.Guid pID)
        {
            if (pID.Equals(System.Guid.Empty))
            {
                return string.Empty;
            }
            return pID.ToString("D");
        }

        /// <summary>
        /// 將String轉換成GUID
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Guid ConvertToGuid(this String str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return System.Guid.Empty;
            }
            System.Guid empty = System.Guid.Empty;
            System.Guid.TryParse(str, out empty);
            return empty;
        }

        /// <summary>
        /// 檢查是否為空值或是Null。
        /// </summary>
        /// <param name="pValue"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object pValue)
        {
            return pValue == null || (pValue is System.Guid && System.Guid.Empty.Equals(pValue)) || (pValue is string && (string.Empty.Equals(pValue) || pValue.Equals(System.Guid.Empty.ToString())));
        }

        /// <summary>
        /// 檢查是否為空值或Null帶入欄位及錯誤訊息。        
        /// </summary>
        /// <param name="pValue"></param>
        /// <param name="pCoumnName"></param>
        /// <param name="pError"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object pValue,string pCoumnName ,StringBuilder pError)
        {
            bool  IsNull = pValue == null || (pValue is System.Guid && System.Guid.Empty.Equals(pValue)) || (pValue is string && (string.Empty.Equals(pValue) || pValue.Equals(System.Guid.Empty.ToString())));
            if (!IsNull)
            {
                pError.AppendFormat("欄位'{0}'不可為空。",pCoumnName);
            }
            return IsNull;
        }
    }
}
