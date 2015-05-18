using System;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CommTool.Business
{
    public class DataEntity
    {
        private bool _dirty;
        [Browsable(false), XmlIgnore]
        public bool Dirty
        {
            get
            {
                return this._dirty;
            }
            set
            {
                if (this._dirty != value)
                {
                    this._dirty = value;
                    this.OnDirtyChanged(EventArgs.Empty);
                }
            }
        }
        private bool _isInitialized;

        [field: NonSerialized]
        public event EventHandler DirtyChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public PropertyCollection ExtendedProperties { get; set; }
        
        /// <summary>
        /// 取得欄位資料
        /// </summary>
        /// <param name="pPropertyName"></param>
        /// <returns></returns>
        public object GetPropertyValue(string pPropertyName)
        {
            PropertyInfo protinfo = this.GetType().GetProperty(pPropertyName);
            if (protinfo!=null)
            {
                return protinfo.GetValue(this,null);
            }
            return null;
        }

        /// <summary>
        /// 設定欄位資料
        /// </summary>
        /// <param name="pPropertyName"></param>
        /// <param name="pValue"></param>
        public void SetPropertyValue(string pPropertyName, object pValue)
        {
            PropertyInfo protinfo = this.GetType().GetProperty(pPropertyName);
            if (protinfo!=null)
            {
                protinfo.SetValue(pPropertyName, pValue,null);
            }
        }

        public virtual void OnPropertyChanged(string pPropertyName)
        {
            if (!this._isInitialized)
            {
                this.OnPropertyChanged(new PropertyChangedEventArgs(pPropertyName));
            }
        }
    
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (!this._isInitialized)
            {
                this.Dirty = true;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, e);
                }
            }
        }

        /// <summary>
        /// 實體被修改
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDirtyChanged(EventArgs e)
        {
            if (this.DirtyChanged != null)
            {
                this.DirtyChanged(this, e);
            }
        }

        //public void ClearErrors();
        //public object Clone();
        //public virtual string GetObjectDescription();
        //
        //protected virtual void OnDirtyChanged(EventArgs e);
        //protected virtual void OnInitialized(EventArgs e);
       
        //public void SetPropertyError(string pPropertyName, string pErrorInfo);
       
    }


}

