using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DExecute
{

    public class DAnalysis
    {
        public const string PatternUrl = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
        private StructAnalysisResult _analysisresult;
        public StructAnalysisResult AnalysisResult { get { return _analysisresult; } }

        public enum AnalysisType
        {
            W = 1,  //網頁
            E = 2,  //錯誤         
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
                Match M = Re.Match(split[1]);
                if (M.Success)
                {
                    _analysisresult.Type = AnalysisType.W;
                    _analysisresult.Result = split[1];
                }
                else
                {
                    _analysisresult.Type = AnalysisType.E;
                    _analysisresult.Result = "解析失敗";
                }
            }
        }

        public struct StructAnalysisResult
        {
            public AnalysisType Type;
            public string Result;
        }
    }

}
