using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;


namespace CreateXML
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string UpdateFilePath =AppDomain.CurrentDomain.BaseDirectory  + "AutoUpData.exe";
            Process.Start(UpdateFilePath);
            Application.Run(new Form1());                      
        }
    }
}
