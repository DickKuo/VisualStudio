using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommTool {

    public class ObjectBase : IDisposable {
        //private bool _ReLoad = false;
        private bool _Disposed = false;
        //Implement IDisposable.

        /// <summary>
        /// 設定ReLoad
        /// </summary>
        /// <param name="value"></param>
        //public void SetReLoad(bool value){
        //	this._ReLoad=value; 
        //}

        ///// <summary>
        ///// 取得ReLoad
        ///// </summary>
        ///// <returns></returns>
        //public bool  GetReLoad()
        //{
        //	return this._ReLoad;
        //}

        /// <summary>
        /// 回傳轉換Table Type欄位
        /// </summary>
        /// <returns></returns>
        public static string[] GetTableTypeColumn() { return new string[] { string.Empty }; }

        ///// <summary>
        ///// Disposed
        ///// </summary>
        protected bool Disposed {
            get {
                return _Disposed;
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///  Free other state (managed objects).
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (!_Disposed) {
                if (disposing) {
                    // Free other state (managed objects).
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                _Disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~ObjectBase() {
            // Simply call Dispose(false).
            Dispose(false);
        }
    }

}
