using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace CheckVersion {
    public partial class CheckVersionForm : Form {
        public CheckVersionForm() {
            InitializeComponent();
        }

        private void CheckVersion_Load(object sender, EventArgs e) {
           foreach(string str in  Directory.GetFiles(@"\\10.40.60.169\hrm\HRM 教育訓練\HRM 工具\懶人機"))
           {
               string fileName  =str.Replace(@"\\10.40.60.169\hrm\HRM 教育訓練\HRM 工具\懶人機\","");

               FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo(str);
               
               if(!File.Exists(Directory.GetCurrentDirectory()+"\\"+fileName))
               {
               
               }

               // FileVersionInfo CurrentFileVersion = 
               
               //myFileVersion.FileVersion
           }
           Process.Start(Directory.GetCurrentDirectory() + "\\" + "CreateXML.exe");
        }
    }
}
