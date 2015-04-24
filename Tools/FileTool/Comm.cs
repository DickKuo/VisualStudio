using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommTool.Properties;

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

    public static class Initiator
    {
        private const string DefaulrLanguage = "1";

        //private static IServiceProvider _curentprovider;

        //public IServiceProvider curentprovider { get { return _curentprovider; } }



        //public static object GetSevice(Type ServiceType)
        //{
        //    if (ServiceType == null)
        //    {
        //        throw new System.ArgumentNullException("ServiceType");
        //    }
        //    if (Initiator._curentprovider == null)
        //    {
        //        throw new ArgumentNullException(Resource.ProviderIsNull);
        //    }
        //    object service = Initiator._curentprovider.GetService(ServiceType);
        //    if (service == null)
        //    {
        //        throw new DesignException(string.Format(Resources.FailGetService, serviceType.FullName));
        //    }
        //    return service;
        //}

        //public static T  GetSevice<T>(object ServiceName)        
        //{
        //    if (Initiator._curentprovider == null)
        //    {
        //        throw new ArgumentNullException(Resource.ProviderIsNull);
        //    }
        //    object service = Initiator._curentprovider.GetService(typeof(T));
        //    if (service == null)
        //    {
        //        throw new ArgumentNullException(string.Format(Resources.FailGetService, typeof(T).FullName));
        //    }
        //    return (T)((object)service);
        //}


      
    }

    public static class CommTool {
        /// <summary>
        /// 20150108 add by Dick for 取得固定長度的字串 單位為Byte
        /// </summary>
        /// <param name="pString">輸入字串</param>
        /// <param name="pLen">取值長度</param>
        /// <param name="IsLeft">預設true從左算起;false為從右算起</param>
        /// <param name="AddChar">字串長度不足時，補足字元</param>
        /// <returns></returns>
        private static string GetLenString(string pString, int pLen, bool IsLeft = true, char AddChar = ' ')
        {
            if (IsLeft)
            {
                pString = pString.PadRight(pLen, AddChar);
            }
            else
            {
                pString = pString.PadLeft(pLen, AddChar);
            }
            string hopestring = string.Empty;  //希望取得的字串
            string spp = string.Empty;
            int byteLength;
            if (IsLeft)
            {
                for (int i = 0; i <= pString.Length; i++)
                {
                    spp = pString.Substring(0, i);
                    byteLength = System.Text.Encoding.Default.GetBytes(spp).Length;
                    if (byteLength <= pLen)
                    { hopestring = spp; }
                    else
                    {
                        hopestring = pString.Substring(0, i - 1);
                        break;
                    }
                }
            }
            else
            {
                int getcount = 1;
                for (int i = pString.Length - 1; i >= 0; i--)
                {
                    spp = pString.Substring(i, getcount);
                    byteLength = System.Text.Encoding.Default.GetBytes(spp).Length;
                    if (byteLength <= pLen)
                    { hopestring = spp; }
                    else
                    {
                        hopestring = pString.Substring(i, getcount);
                        break;
                    }
                    if (byteLength == pLen)
                    {
                        break;
                    }
                    getcount++;
                }
            }
            return hopestring;
        }

    }
}
