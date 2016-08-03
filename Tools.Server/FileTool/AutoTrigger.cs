using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommTool
{
    public interface IAutoTrigger
    {
        /// <summary>Log路徑</summary>
        string LogPath { set; get; }

        /// <summary>類型名稱</summary>
        string KeyName { get; }

        /// <summary>執行觸發</summary>
        /// <param name="pCurrentTime"></param>
        void Execute(string pCurrentTime);

        /// <summary>寫Log紀錄</summary>
        /// <param name="ex"></param>
        void Log(object ex);

    }

    public abstract class AutoTrigger : IAutoTrigger
    {
        private const string _defaultLogPath = @"C:\SLog";  //預設Log位置
        private string _logpaht;

        /// <summary>Log路徑</summary>
        public string LogPath {
            set {
                _logpaht = value;
            }
            get {
                if (string.IsNullOrEmpty(_logpaht)) {
                    return _defaultLogPath;
                }
                else {
                    return _logpaht;
                }
            }
        }

        /// <summary>類型名稱</summary>
        public virtual string KeyName {
            get {
                return this.GetType().Name;
            }
        }

        /// <summary>執行觸發</summary>
        /// <param name="pCurrentTime">當前時間</param>
        public abstract void Execute(string pCurrentTime);

        /// <summary>寫log</summary>
        /// <param name="ex"></param>
        public virtual void Log(object ex) {
            ToolLog.Log(ex.ToString());
        }

    }


    public interface ITriggerService
    {
        /// <summary>執行自動觸發</summary>
        /// <param name="pCurrentTime"></param>
        void Run(string pCurrentTime);
    }

    public class TriggerService : ITriggerService
    {

        Dictionary<string, IAutoTrigger> _dicTriggers = new Dictionary<string, IAutoTrigger>();
        
        public TriggerService() {
        }

        public virtual TriggerService GetAutoTriggerService() {
            return new TriggerService();
        }

        /// <summary>加入自動觸發</summary>
        /// <param name="tigger"></param>
        public void AddTriggers(IAutoTrigger tigger) {
            if (!_dicTriggers.ContainsKey(tigger.KeyName)) {
                _dicTriggers.Add(tigger.KeyName, tigger);
            }
        }

        /// <summary>執行自動觸發 </summary>
        /// <param name="pCurrentTime">當前時間</param>
        public void Run(string pCurrentTime) {
            foreach (IAutoTrigger tigger in _dicTriggers.Values) {
                tigger.Execute(pCurrentTime);
            }
        }
        
        public int Count {
            get {
                return _dicTriggers.Count;           
            }
        }

    }
}
