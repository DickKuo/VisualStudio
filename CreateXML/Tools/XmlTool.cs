using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

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

        /// <summary>
        /// 20141128 add by Dick for ru8
        /// </summary>
        /// <param name="pInput"></param>
        private static void WriteXml(string pSavePath, string pInput)
        {
            using (StreamWriter sw = new StreamWriter(pSavePath,false))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n");
                sb.Append("<root>\r\n");
                sb.Append(pInput);
                sb.Append("</root>\r\n");
                sw.Write(sb.ToString());
                sw.Close();
            }
        }
    }
}
