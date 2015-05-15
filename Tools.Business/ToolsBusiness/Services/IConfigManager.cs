using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolsBusiness.Services
{
    public interface IConfigManager
    {        

        /// <summary>
        /// 設定參數的值
        /// </summary>
        /// <param name="pParameter">參數名稱</param>
        /// <param name="value">值</param>
        void SetValue(string pParameter, string value);


        /// <summary>
        /// 取得參數的值
        /// </summary>
        /// <param name="pParameter">參數名稱</param>
        /// <returns></returns>
        string GetValue(string pParameter);

        /// <summary>
        /// 取得參數字典
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetParamters();
    }
}
