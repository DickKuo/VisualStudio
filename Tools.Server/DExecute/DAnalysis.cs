using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebInfo;

namespace DExecute {

    public class DAnalysis {
        private const string PatternUrl = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";

        private StructAnalysisResult _analysisresult;

        public StructAnalysisResult AnalysisResult { get { return _analysisresult; } }

        public enum AnalysisType {
            W = 1,     //網頁
            E = 2,     //錯誤 
            R = 3,     //重開服務
            S = 4,     //停止服務
            U = 5,     //更新
            Select = 6, //選取
            Save = 7,    //設定
            Get = 8,    //取得
            Clear = 9,   //清除
        }

        /// <summary>
        /// 20150710 modifeid for #73
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        public StructAnalysisResult Start(string Query) {
            string result = string.Empty;
            string[] split = Query.Split(' ');
            if (split.Length > 0) {
                switch (split[0].ToLower()) {
                    case "w": // 網頁的解析 
                        _analysisresult.Type = AnalysisType.W;
                        WebAnalysis(split);
                        break;
                    case "r":
                        _analysisresult.Type = AnalysisType.R;
                        break;
                    case "s":
                        _analysisresult.Type = AnalysisType.S;
                        break;
                    case "u":
                        _analysisresult.Type = AnalysisType.U;
                        break;
                    case "select":
                        if (split.Length > 1) {
                            switch (split[1].ToLower()) {
                                case "table":
                                    if (split[2].Length > 2) {
                                        _analysisresult.Type = AnalysisType.Select;
                                        _analysisresult.Result = SQLHelper.SHelper.GetTableColumns(split[2]);
                                    }
                                    break;
                            }
                        }
                        else {
                            _analysisresult.Type = AnalysisType.E;
                            _analysisresult.Result = "目前不支援該指令";
                        }
                        break;
                    case "clear":
                        _analysisresult.Type = AnalysisType.Clear;
                        _analysisresult.Result = "clear";
                        break;
                    case "get":
                        if (split.Length > 1) {
                            switch (split[1].ToLower()) {
                                case "record":
                                    break;
                            }
                        }
                        break;
                    default:
                        _analysisresult.Type = AnalysisType.E;
                        _analysisresult.Result = "目前不支援該指令";
                        break;
                }
            }
            else {
                _analysisresult.Type = AnalysisType.E;
                _analysisresult.Result = "輸入字串格式錯誤";
            }
            return _analysisresult;
        }

        /// <summary>解析網址</summary>
        /// 20150409 add by Dick 
        /// <param name="split"></param>
        private void WebAnalysis(string[] split) {
            System.Text.RegularExpressions.Regex Re = new System.Text.RegularExpressions.Regex(PatternUrl, RegexOptions.IgnoreCase);
            if (split.Length > 1) {
                switch (split[1].ToLower()) {
                    case "getbueaty":
                        GetBueaty(split, Re);
                        break;
                }
            }
        }

        /// <summary>解析網頁資訊</summary>
        /// <param name="split"></param>
        /// <param name="Re"></param>
        private void GetBueaty(string[] split, System.Text.RegularExpressions.Regex Re) {
            if (split.Length > 2) {
                Match M = Re.Match(split[2]);
                if (M.Success) {
                    _analysisresult.Type = AnalysisType.W;
                    _analysisresult.Result = split[2];
                }
                else {
                    _analysisresult.Type = AnalysisType.E;
                    _analysisresult.Result = "解析失敗";
                }
            }
            else {
                _analysisresult.Type = AnalysisType.E;
                _analysisresult.Result = "解析失敗";
            }
        }

        public struct StructAnalysisResult {
            public AnalysisType Type;
            public string Result;
        }
    }
}
