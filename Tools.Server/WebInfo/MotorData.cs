using CommTool;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace WebInfo {
    public class MotorData {

        private List<string> Brand = new List<string>();
        private List<string> Modles = new List<string>();
        SQLHelper.UseStoreProcedure USP = new SQLHelper.UseStoreProcedure();

        private class Default {
            public const string Brand = "Brand";
            public const string MotorPostAddress = "MotorPostAddress";
            public const string JsonKey = "json";
            public const string JSonStringFormat = "{0}={1}";
            public const string DServiceConfig = "DService.exe.config";
            public const string DService = "DService";
        }

        private class SP {
            public const string AddMotor = "AddMotor";
            public const string GetMotorModle = "GetMotorModle";
            public const string GetMotorBrands = "GetMotorBrands";
            public const string AddMotorModle = "AddMotorModle";
            public const string AddMotorBrands = "AddMotorBrands";
        }

        private class SPParameter {
            public const string Key = "Key";
            public const string Title = "Title";
            public const string Url = "Url";
            public const string Price = "Price";
            public const string Context = "Context";
            public const string Location = "Location";
            public const string Img = "Img";
            public const string Milage = "Milage";
            public const string Years = "Years";
            public const string Brand = "Brand";
            public const string Model = "Model";
            public const string Remark = "Remark";
            public const string ModleName = "ModleName";
        }

        /// <summary>抓取所有網站的資料</summary>
        /// 20170207 addd by Dick
        public void GetMotorData() {
            DataTable BrandDT = USP.ExeProcedureGetDataTable(SP.GetMotorBrands);
            if (BrandDT != null && BrandDT.Rows.Count > 0) {
                foreach (DataRow dr in BrandDT.Rows) {
                    Brand.Add(dr[1].ToString());
                }
            }
            DataTable dt = USP.ExeProcedureGetDataTable(SP.GetMotorModle);
            if (dt != null && dt.Rows.Count > 0) {
                foreach (DataRow dr in dt.Rows) {
                    Modles.Add(dr[1].ToString());
                }
            }
            Getwebike();
            Getmuaban();
            Getrongbay();
        }

        /// <summary>抓取rongbay資料</summary>
        /// 20170207 addd by Dick
        /// <returns></returns>
        public List<Motor> Getrongbay(bool IsPostAndAdd=true) {
            List<Motor> ResultList = new List<Motor>();
            string DetailUrl = string.Empty;
            WebInfo Web = new WebInfo();
            for (int i = 1; i <= 20; i++) {
                string Url = string.Format("http://rongbay.com/TP-HCM/Xe-may-Xe-dap-c20-trang{0}.html", i);
                HtmlNodeCollection Anchors = Web.GetWebHtmlDocumentNodeCollection(Url, "//div[@class='NewsList']", Encoding.UTF8);
                if (Anchors != null) {
                    foreach (var Child in Anchors[0].ChildNodes) {
                        try {
                            if (Child.Name == CommTool.BaseConst.Div.ToLower()) {
                                if (!string.IsNullOrEmpty(Child.InnerText)) {
                                    Motor _Motor = new Motor();
                                    if (Child.ChildNodes[1] != null) {
                                        if (Child.ChildNodes[1].Attributes.Count < 2) {
                                            continue;
                                        }
                                        Uri TempUri = null;
                                        _Motor.Url = Child.ChildNodes[1].Attributes[2].Value;
                                        if (Uri.TryCreate(_Motor.Url, UriKind.Absolute, out TempUri)) {
                                            HtmlDocument _doc = Web.GetWebHtmlDocument(_Motor.Url, Encoding.UTF8);
                                            HtmlNodeCollection Detail = _doc.DocumentNode.SelectNodes("//div[@class='sub_detail_popup']");
                                            if (Detail != null) {
                                                _Motor.Title = Detail[0].ChildNodes[1].ChildNodes[1].InnerText.Trim();
                                                HtmlNode Node = _doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[6]/div[5]/div[1]/table[1]/tr[1]/td[1]/div[5]/div[1]/div[2]/div[1]/div[1]/ul[1]/li[1]");
                                                if (Node != null) {
                                                    _Motor.Price = Node.InnerText.Replace("Giá", string.Empty).Replace(":", string.Empty).Replace("vnđ", string.Empty).Trim();
                                                }
                                                _Motor.Key = "rongbay";
                                                HtmlNodeCollection Location = _doc.DocumentNode.SelectNodes("//p[@class='cl_666']");
                                                if (Location != null) {
                                                    _Motor.Location = Location[0].InnerText.Replace("Địa chỉ:", string.Empty).Trim();
                                                }
                                                HtmlNodeCollection Info_text = _doc.DocumentNode.SelectNodes("//div[@class='info_text']");
                                                if (Info_text != null) {
                                                    _Motor.Context = Info_text[0].InnerText.Trim();
                                                }
                                                HtmlNodeCollection ImagList = _doc.DocumentNode.SelectNodes("//div[@class='info_box']");
                                                if (ImagList != null) {
                                                    foreach (var Imag in ImagList[0].ChildNodes[1].ChildNodes) {
                                                        if (Imag.Name == "a") {
                                                            _Motor.Img += Imag.Attributes[0].Value + ",";
                                                        }
                                                    }
                                                }
                                                GetBrand(_Motor);
                                                GetYears(_Motor);
                                            }
                                            if (string.IsNullOrEmpty(_Motor.Model)) {
                                                GetModle(_Motor);
                                            }
                                            ResultList.Add(_Motor);
                                            if (IsPostAndAdd) {
                                                PostAndAddMotor(_Motor);
                                            }
                                        } 
                                    }
                                }
                            }
                        }
                        catch (Exception ex) {
                            CommTool.ToolLog.Log(ex);
                        }
                    }
                }
            }
            return ResultList;
        }             

        /// <summary>抓取muaban資料</summary>
        /// 20170207 addd by Dick
        /// <returns></returns>
        public List<Motor> Getmuaban(bool IsPostAndAdd=true) {
            List<Motor> ResultList = new List<Motor>();
            WebInfo Web = new WebInfo();
            for (int i = 1; i <= 20; i++) {
                string Url = string.Format("https://muaban.net/xe-may-toan-quoc-l0-c5?cp={0}", i);
                HtmlNodeCollection Anchors = Web.GetWebHtmlDocumentNodeCollection(Url, "//div[@class='mbn-box-list']", Encoding.UTF8);
                if (Anchors != null) {
                    foreach (var Child in Anchors[2].ChildNodes) {
                        try {
                            if (Child.Name == CommTool.BaseConst.Div.ToLower()) {
                                if (Child.ChildNodes.Count > 3) {
                                    Motor _Motor = new Motor();
                                    if (Child.ChildNodes[3].Attributes.Count > 2) {
                                        _Motor.Title = Child.ChildNodes[3].Attributes[1].Value;
                                        _Motor.Url = Child.ChildNodes[3].Attributes[2].Value;
                                    }
                                    else {
                                        _Motor.Title = Child.ChildNodes[3].Attributes[0].Value;
                                        _Motor.Url = Child.ChildNodes[3].Attributes[1].Value;
                                    }
                                    if (Child.ChildNodes[5].ChildNodes.Count > 1) {
                                        _Motor.Location = Child.ChildNodes[5].ChildNodes[1].ChildNodes[5].InnerText;
                                    }
                                    else {
                                        _Motor.Location = Child.ChildNodes[3].ChildNodes[1].ChildNodes[5].InnerText;
                                    }
                                    GetBrand(_Motor);
                                    GetYears(_Motor);

                                    _Motor.Key = "muaban";

                                    HtmlNodeCollection Context = Web.GetWebHtmlDocumentNodeCollection(_Motor.Url, "//div[@id='dvContent']", Encoding.UTF8);
                                    if (Context != null) {
                                        var ContextNodes = Context[0].SelectNodes("//div[@class='ct-body overflow clearfix']");
                                        if (ContextNodes.Count > 0) {
                                            _Motor.Context = ContextNodes[0].InnerText.Trim();
                                        }
                                        var PriceNodes = Context[0].SelectNodes("//div[@class='ct-price clearfix']");
                                        if (PriceNodes != null) {
                                            if (PriceNodes.Count > 0) {
                                                _Motor.Price = PriceNodes[0].InnerText.Replace("Giá:", string.Empty).Replace("đ", string.Empty).Trim();
                                            }
                                        }
                                        var Images = Context[0].SelectNodes("//div[@class='ct-image']");
                                        if (Images != null) {
                                            if (Images.Count > 0) {
                                                foreach (var ImageNode in Images[0].ChildNodes[1].ChildNodes) {
                                                    if (ImageNode.Name == BaseConst.Div.ToLower()) {
                                                        _Motor.Img += "," + ImageNode.ChildNodes[0].Attributes[2].Value;
                                                    }
                                                }
                                            }
                                            _Motor.Img = _Motor.Img.Substring(1, _Motor.Img.Length - 1);
                                        }
                                    }
                                    if (string.IsNullOrEmpty(_Motor.Model)) {
                                        GetModle(_Motor);
                                    }
                                    ResultList.Add(_Motor);
                                    if (IsPostAndAdd) {
                                        PostAndAddMotor(_Motor);
                                    }
                                }
                            }
                        }
                        catch (Exception ex) {
                            CommTool.ToolLog.Log(ex);
                        }
                    }
                }
            }
            return ResultList;
        }
        
        /// <summary>抓取Anchors資料</summary>
        /// 20170207 addd by Dick
        /// <returns></returns>
        public List<Motor> Getwebike(bool IsPostAndAdd=true) {
            List<Motor> ResultList = new List<Motor>();
            WebInfo Web = new WebInfo();
            for (int i = 1; i <= 20; i++) {
                string Url = string.Format("https://www.webike.vn/cho-xe-may/danh-sach-xe.html?page={0}", i);
                HtmlNodeCollection Anchors = Web.GetWebHtmlDocumentNodeCollection(Url, "//div[@class='product-grid']", Encoding.UTF8);
                if (Anchors != null) {
                    foreach (var MotorArray in Anchors[BaseConst.ArrayFirstItem].ChildNodes) {
                        try {
                            if (MotorArray.Name == BaseConst.Div.ToLower()) {
                                foreach (var Child in MotorArray.ChildNodes) {
                                    if (Child.Name == BaseConst.Div.ToLower()) {
                                        Motor NewMotor = new Motor();
                                        string TempStr = Child.ChildNodes[1].ChildNodes[3].InnerText;
                                        string[] TempArray = TempStr.Split(' ');
                                        if (TempArray.Length > 0) {
                                            NewMotor.Model = TempStr.Replace(TempArray[0], string.Empty).Replace(TempArray[TempArray.Length - 1], string.Empty).Trim();
                                            NewMotor.Brand = TempArray[0];
                                            NewMotor.Years = TempArray[TempArray.Length - 1];
                                            NewMotor.Price = Child.ChildNodes[1].ChildNodes[5].InnerText.Trim().Replace("VNĐ", string.Empty);
                                            if (Child.ChildNodes[1].ChildNodes[7].InnerText.IndexOf("&") != -1) {
                                                NewMotor.Location = Child.ChildNodes[1].ChildNodes[7].InnerText;
                                                while ((NewMotor.Location.IndexOf("&") != -1)) {
                                                    int Start = NewMotor.Location.IndexOf("&");
                                                    int End = NewMotor.Location.IndexOf(";");
                                                    int dec = End - Start;
                                                    string ReplaceString = NewMotor.Location.Substring(Start, dec + 1);
                                                    NewMotor.Location = NewMotor.Location.Replace(ReplaceString, string.Empty);
                                                }
                                            }
                                            NewMotor.Key = "webike";
                                            NewMotor.Milage = Child.ChildNodes[1].ChildNodes[9].ChildNodes[0].InnerHtml.ToLower().Replace("km", string.Empty).Replace("đi:", string.Empty).Replace("đ&atilde;", string.Empty).Trim();
                                            NewMotor.Url = Child.ChildNodes[1].Attributes[0].Value;
                                            NewMotor.Img = Child.ChildNodes[1].ChildNodes[1].ChildNodes[1].Attributes[0].Value;
                                            if (NewMotor.Url != null) {
                                                HtmlDocument _doc = Web.GetWebHtmlDocument(Url, Encoding.UTF8);
                                                HtmlNodeCollection Detail = _doc.DocumentNode.SelectNodes("//div[@id='center']");
                                                if (Detail != null) {
                                                    NewMotor.Title = Detail[0].ChildNodes[1].ChildNodes[1].ChildNodes[1].InnerText.Replace("\n", string.Empty).Replace("\t", string.Empty).Trim();
                                                }
                                                HtmlNode mdnode = _doc.DocumentNode.SelectSingleNode("//meta[@name='description']");
                                                if (mdnode != null) {
                                                    HtmlAttribute desc;
                                                    desc = mdnode.Attributes["content"];
                                                    NewMotor.Context = desc.Value;
                                                }
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(NewMotor.Model)) {
                                            AddMotorModle(NewMotor);
                                        }
                                        if (!string.IsNullOrEmpty(NewMotor.Brand)) {
                                            AddMotorBrands(NewMotor);
                                        }
                                        ResultList.Add(NewMotor);
                                        if (IsPostAndAdd) {
                                            PostAndAddMotor(NewMotor);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex) {
                            CommTool.ToolLog.Log(ex);
                        }
                    }
                }
            }
            return ResultList;
        }

        /// <summary>用Title簡易判斷品牌</summary>
        /// 20170207 addd by Dick
        /// <param name="_Motor"></param>
        private void GetBrand(Motor _Motor) {
            foreach (var str in Brand) {
                if (_Motor.Title.ToUpper().IndexOf(str.ToUpper()) != -1) {
                    _Motor.Brand = str;
                }
            }
        }

        /// <summary>取得年份</summary>
        /// 20170207 addd by Dick
        /// <param name="_Motor"></param>
        private void GetYears(Motor _Motor) {
            string[] sp = _Motor.Title.Split(' ');
            if (sp.Length > 0) {
                foreach (var str in sp) {
                    int result = 0;
                    if (int.TryParse(str, out result)) {
                        if (result > 1911 & result < 2030) {
                            _Motor.Years = str;
                        }
                    }
                }
            }
        }

        /// <summary>取得型號</summary>
        /// <param name="_Motor"></param>
        private void GetModle(Motor _Motor) {
            if (!string.IsNullOrEmpty(_Motor.Title)) {
                foreach (string str in Modles) {
                    if (_Motor.Title.ToUpper().IndexOf(str.ToUpper()) != -1) {
                        _Motor.Model = str;
                    }
                }
            }
        }

        /// <summary>寫入DB紀錄</summary>
        /// 20170207 addd by Dick
        /// <param name="_Motor"></param>
        public int AddMotor(Motor _Motor) {           
            try {
                USP.AddParameter(SPParameter.Brand, _Motor.Brand == null ? string.Empty : _Motor.Brand);
                USP.AddParameter(SPParameter.Context, _Motor.Context == null ? string.Empty : _Motor.Context);
                USP.AddParameter(SPParameter.Img, _Motor.Img == null ? string.Empty : _Motor.Img);
                USP.AddParameter(SPParameter.Key, _Motor.Key == null ? string.Empty : _Motor.Key);
                USP.AddParameter(SPParameter.Location, _Motor.Location == null ? string.Empty : _Motor.Location);
                USP.AddParameter(SPParameter.Milage, _Motor.Milage == null ? string.Empty : _Motor.Milage);
                USP.AddParameter(SPParameter.Model, _Motor.Model == null ? string.Empty : _Motor.Model);
                USP.AddParameter(SPParameter.Price, _Motor.Price == null ? string.Empty : _Motor.Price);
                USP.AddParameter(SPParameter.Remark, _Motor.Remark == null ? string.Empty : _Motor.Remark);
                USP.AddParameter(SPParameter.Title, _Motor.Title == null ? string.Empty : _Motor.Title);
                USP.AddParameter(SPParameter.Url, _Motor.Url == null ? string.Empty : _Motor.Url);
                USP.AddParameter(SPParameter.Years, _Motor.Years == null ? string.Empty : _Motor.Years);
                return USP.ExeProcedureReturnKey(SP.AddMotor);                
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return -1;
            }           
        }

        /// <summary>AddMotorModle</summary>
        /// <param name="_Motor"></param>
        public void AddMotorModle(Motor _Motor) {
            if (!string.IsNullOrEmpty(_Motor.Model)) {
                USP.AddParameter(SPParameter.ModleName, _Motor.Model);
                USP.ExeProcedureNotQuery(SP.AddMotorModle);
            }
        }

        /// <summary>AddMotorBrands</summary>
        /// <param name="_Motor"></param>
        public void AddMotorBrands(Motor _Motor) {
            if (!string.IsNullOrEmpty(_Motor.Brand)) {
                USP.AddParameter(SPParameter.Brand, _Motor.Brand);
                USP.ExeProcedureNotQuery(SP.AddMotorBrands);
            }
        }

        /// <summary>寫入資料庫，並同步過去</summary>
        /// 20170207 addd by Dick
        /// <param name="_Motor"></param>
        public void PostAndAddMotor(Motor _Motor) {
            if (_Motor.Price == "Li&ecirc;n hệ") {
                _Motor.Price = "0";
            }
            int SN = AddMotor(_Motor);
            if (SN != -1) {
                _Motor.SN = SN;
                string JsonData = Newtonsoft.Json.JsonConvert.SerializeObject(_Motor);
                CommTool.ToolLog.Log(JsonData);
                string configiPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Default.DServiceConfig);
                ConfigManager configmanage = new ConfigManager(configiPath, Default.DService);
                string PostAddress = configmanage.GetValue(Default.MotorPostAddress);                 
                WebInfo _WebInfo = new WebInfo();
                _WebInfo.HttpPostMethod(JsonData, PostAddress);
                Thread.Sleep(5000);
            }
        }

    }
}
