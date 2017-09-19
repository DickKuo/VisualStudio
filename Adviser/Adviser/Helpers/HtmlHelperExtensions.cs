using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace System.Web.Mvc.Html {
    public class ScriptVarTag {
        public string Key { get; set; }
        public string Val { get; set; }
    }

    public static class HtmlHelperExtensions {

        private const string _jSViewDataName = "~/Bundles/RenderJavaScript{0}";
        private const string _styleViewDataName = "~/Content/RenderStyle{0}";

        public static void AddJavaScriptVar(this HtmlHelper htmlHelper, string key, string value) {

            List<ScriptVarTag> scriptList = htmlHelper.ViewContext.HttpContext
              .Items["RenderJavaScriptVar"] as List<ScriptVarTag>;
            ScriptVarTag _ScriptVarTag = new ScriptVarTag() { Key = key, Val = value };
            if (scriptList != null) {

                if (!scriptList.Contains(_ScriptVarTag)) {
                    scriptList.Add(_ScriptVarTag);
                }
                else {


                }
            }
            else {
                scriptList = new List<ScriptVarTag>();
                scriptList.Add(_ScriptVarTag);
                htmlHelper.ViewContext.HttpContext
                  .Items.Add("RenderJavaScriptVar", scriptList);
            }

        }
        public static IHtmlString RenderJavaScriptVars(this HtmlHelper htmlHelper) {
            StringBuilder result = new StringBuilder();
            List<ScriptVarTag> scriptList = htmlHelper.ViewContext.HttpContext
                                 .Items["RenderJavaScriptVar"] as List<ScriptVarTag>;
            if (scriptList != null) {

                foreach (ScriptVarTag scriptVarTag in scriptList) {
                    result.Append(string.Format("var {0}=\"{1}\";", scriptVarTag.Key, scriptVarTag.Val.Replace("\"", "\\\"")));

                }

                return MvcHtmlString.Create("<script type=\"text/javascript\">" + result.ToString() + "</script>");
            }
            else {
                return new HtmlString(null);
            }
        }


        public static void AddJavaScript(this HtmlHelper htmlHelper,
                                         string scriptURL) {

            List<string> scriptList = htmlHelper.ViewContext.HttpContext
              .Items["RenderJavaScript"] as List<string>;

            if (scriptList != null) {

                if (!scriptList.Contains(scriptURL)) {
                    scriptList.Add(scriptURL);
                }
            }
            else {
                scriptList = new List<string>();
                scriptList.Add(scriptURL);
                htmlHelper.ViewContext.HttpContext
                  .Items.Add("RenderJavaScript", scriptList);
            }

        }

        public static IHtmlString RenderJavaScripts(this HtmlHelper htmlHelper) {
            string _Value = "";
            List<string> ScriptList = htmlHelper.ViewContext.HttpContext.Items["RenderJavaScript"] as List<string>;
            if (ScriptList != null) {
                _Value = string.Join(",", ScriptList.ToArray()).GetHashCode().ToString();
            }
            else {
                _Value = "";
            }

            //string _Value = GetIdentification(htmlHelper);
            if (!String.IsNullOrEmpty(_Value)) {
                var I = BundleTable.Bundles.GetBundleFor(String.Format(_jSViewDataName, _Value));
                if (I != null) {
                    return Scripts.Render(String.Format(_jSViewDataName, _Value));
                }
                else {
                    List<string> scriptList = htmlHelper.ViewContext.HttpContext
                      .Items["RenderJavaScript"] as List<string>;
                    if (scriptList != null) {
                        var _ScriptBundle = new ScriptBundle(String.Format(_jSViewDataName, _Value));
                        foreach (string script in scriptList) {

                            _ScriptBundle.Include(script.Trim());

                        }
                        BundleTable.Bundles.Add(_ScriptBundle);
                        return Scripts.Render(String.Format(_jSViewDataName, _Value));
                    }
                    else {
                        return new HtmlString(null);
                    }
                }
            }
            else {
                return new HtmlString(null);

            }
        }

        public static void AddStyle(this HtmlHelper htmlHelper, string styleURL) {

            List<string> styleList = htmlHelper.ViewContext.HttpContext
              .Items["RenderStyle"] as List<string>;

            if (styleList != null) {
                if (!styleList.Contains(styleURL)) {
                    styleList.Add(styleURL);
                }
            }
            else {
                styleList = new List<string>();
                styleList.Add(styleURL);
                htmlHelper.ViewContext.HttpContext
                  .Items.Add("RenderStyle", styleList);
            }

        }
        public static IHtmlString RenderStyles(this HtmlHelper htmlHelper) {

            string _Value = "";
            List<string> StyletList = htmlHelper.ViewContext.HttpContext.Items["RenderStyle"] as List<string>;
            if (StyletList != null) {
                _Value = string.Join(",", StyletList.ToArray()).GetHashCode().ToString();
            }
            else {
                _Value = "";
            }

            if (!String.IsNullOrEmpty(_Value)) {
                var I = BundleTable.Bundles.GetBundleFor(String.Format(_styleViewDataName, _Value));
                if (I != null) {
                    return Styles.Render(String.Format(_styleViewDataName, _Value));
                }
                else {
                    List<string> styleList = htmlHelper.ViewContext.HttpContext
                      .Items["RenderStyle"] as List<string>;

                    if (styleList != null) {
                        var _StyleBundle = new StyleBundle(String.Format(_styleViewDataName, _Value));
                        foreach (string style in styleList) {
                            _StyleBundle.Include(style);
                        }
                        BundleTable.Bundles.Add(_StyleBundle);
                        return Styles.Render(String.Format(_styleViewDataName, _Value));
                    }
                    else {
                        return new HtmlString(null);
                    }
                }
            }
            else {
                return new HtmlString(null);
            }
        }
    }
}