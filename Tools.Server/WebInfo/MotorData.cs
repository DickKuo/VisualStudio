using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebInfo {
    public class MotorData {

        

        public void Index() { 
        
        
        }


        public void Getmuaban() { 
            WebInfo Web =new WebInfo();
            string Url = "https://muaban.net/xe-may-toan-quoc-l0-c5";
            HtmlNodeCollection Anchors = Web.GetWebHtmlDocumentNodeCollection(Url, "//div[@class='mbn-box-list']",Encoding.UTF8);
            if (Anchors != null) {
                foreach (var Child in Anchors[2].ChildNodes) {
                    if (Child.Name ==CommTool.BaseConst.Div.ToLower()) {
                        Motor _Motor = new Motor();
                        _Motor.Title = Child.ChildNodes[3].Attributes[1].Value;
                        _Motor.Url = Child.ChildNodes[3].Attributes[2].Value;
                    }
                }
            }
        }

    }
}
