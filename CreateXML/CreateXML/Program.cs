using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using CommTool;
using System.Threading;


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
            AutoUpdate();
        }

        /// <summary>
        /// 自動更新程式
        /// </summary>
        private static void AutoUpdate()
        {
            string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CreateXML.exe.config");
            ConfigManager configmanger = new ConfigManager(ConfigPath, "CreateXML");
            if (!Convert.ToBoolean(configmanger.GetValue("IsUpdated")))
            {
                string UpdateFilePath = AppDomain.CurrentDomain.BaseDirectory + "AutoUpData.exe";
                Process.Start(UpdateFilePath);
            }
            else
            {
                configmanger.SetValue("IsUpdated", "false");
                Thread.Sleep(2000);
                Application.Run(new Form1());    
            }
        }
    }
}
