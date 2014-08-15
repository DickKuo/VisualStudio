using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Tools {
    public class XmlTool {
        public static XmlDocument LoadXml(string Path) {
            XmlDocument doc = new XmlDocument();
            System.IO.StreamReader sr = new System.IO.StreamReader(Path);
            doc.Load(sr);
            sr.Close();
            sr.Dispose();
            return doc;
        }

        
    }
}
