using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebInfo {
    public class MotorData {

        private List<string> Brand = new List<string>();

        SQLHelper.UseStoreProcedure USP = new SQLHelper.UseStoreProcedure();

        private class SP {
            public const string AddMotor = "AddMotor";
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
        }


        public void Index() {
            Brand.Add("Honda");
            Brand.Add("Piaggio");
            Brand.Add("Piagio");
            Brand.Add("Nouvo");
            Brand.Add("Yamaha");
            Brand.Add("Vespa");
            Brand.Add("Suzuki");
            Brand.Add("Wave");
            Brand.Add("Spacy");


            //Brand.Add("wave");
            Getmuaban();
        }

        /// <summary>抓取muaban資料</summary>
        public void Getmuaban() {         
            WebInfo Web = new WebInfo();
            for (int i = 1; i <= 20; i++) {
                string Url = string.Format("https://muaban.net/xe-may-toan-quoc-l0-c5?cp={0}", i);
                HtmlNodeCollection Anchors = Web.GetWebHtmlDocumentNodeCollection(Url, "//div[@class='mbn-box-list']", Encoding.UTF8);               
                if (Anchors != null) {
                    foreach (var Child in Anchors[2].ChildNodes) {
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

                                foreach (var str in Brand) {
                                    if (_Motor.Title.ToUpper().IndexOf(str.ToUpper()) != -1) {
                                        _Motor.Brand = str;
                                    }
                                }
                                string[] sp = _Motor.Title.Split(' ');
                                if (sp.Length > 0) {
                                    foreach(var str in sp)
                                    {
                                        int result = 0;
                                        if (int.TryParse(str, out result)) {
                                            if (result > 1911  & result <2030) {
                                                _Motor.Years = str;
                                            }
                                        }
                                    }                                      
                                }

                                if (_Motor.Url.IndexOf("-id") != -1) {
                                    int Leng = _Motor.Url.IndexOf("-id");
                                    _Motor.Key = _Motor.Url.Substring(Leng + 3, _Motor.Url.Length - 1 - Leng - 3);
                                }
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
                                                if (ImageNode.Name == CommTool.BaseConst.Div.ToLower()) {
                                                    _Motor.Img += "," + ImageNode.ChildNodes[0].Attributes[2].Value;
                                                }
                                            }
                                        }
                                        _Motor.Img = _Motor.Img.Substring(1, _Motor.Img.Length - 1);
                                    }
                                }
                                AddMotor(_Motor);
                            }
                        }
                    }
                }
            }
        }


        public void AddMotor(Motor _Motor) {
            USP.AddParameter(SPParameter.Brand, _Motor.Brand==null?string.Empty :_Motor.Brand);
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
            USP.ExeProcedureNotQuery(SP.AddMotor);
        }

    }
}
