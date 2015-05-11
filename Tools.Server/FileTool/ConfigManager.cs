using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommTool
{
    public class ConfigManager
    {
        private  string _default =@"C\SLog\AppConfig.exe";
        private  string _path;

        public string ConfigPath {
            set {
                _path = value;
            }
            get {
                return _default;
            }
        }

    }
}
