using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DExecute
{

    public class DAnalysis
    {
        private const string PatternUrl = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
        private StructAnalysisResult _analysisresult;
        public StructAnalysisResult AnalysisResult { get { return _analysisresult; } }

        public enum AnalysisType
        {
            W = 1,   //網頁
            E = 2,   //錯誤 
            R = 3,   //重開服務
            S = 4,   //停止服務            
        }
               
        public StructAnalysisResult Start(string Query)
        {
            string result = string.Empty;
            string[] split = Query.Split(' ');
            if (split.Length > 0)
            {
                switch (split[0].ToLower())
                {
                    case "w": // 網頁的解析 
                        WebAnalysis(split);
                        break;
                    case "r":
                        _analysisresult.Type = AnalysisType.R;
                        break;
                    case "s":
                        _analysisresult.Type = AnalysisType.S;
                        break;
                }
            }
            else
            {
                _analysisresult.Type = AnalysisType.E;
                _analysisresult.Result = "輸入字串格式錯誤";
            }
            return _analysisresult;
        }

        /// <summary>
        /// 20150409 add by Dick for 解析網址
        /// </summary>
        /// <param name="split"></param>
        private void WebAnalysis(string[] split)
        {
            System.Text.RegularExpressions.Regex Re = new System.Text.RegularExpressions.Regex(PatternUrl, RegexOptions.IgnoreCase);
            if (split.Length > 1)
            {
                switch (split[1].ToLower())
                {
                    case "getbueaty":
                        GetBueaty(split, Re);
                        break;
                }
            }
        }

        private void GetBueaty(string[] split, System.Text.RegularExpressions.Regex Re)
        {
            if (split.Length > 2)
            {
                Match M = Re.Match(split[2]);
                if (M.Success)
                {
                    _analysisresult.Type = AnalysisType.W;
                    _analysisresult.Result = split[2];
                }
                else
                {
                    _analysisresult.Type = AnalysisType.E;
                    _analysisresult.Result = "解析失敗";
                }
            }
            else
            {
                _analysisresult.Type = AnalysisType.E;
                _analysisresult.Result = "解析失敗";
            }
        }

        public struct StructAnalysisResult
        {
            public AnalysisType Type;
            public string Result;
        }
    }

}
