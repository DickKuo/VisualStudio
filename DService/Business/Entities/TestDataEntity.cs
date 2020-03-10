using CommTool.Business.Metadata;
using System.ComponentModel;
using CommTool.Business;

namespace DService.Business.Entities
{
    [Description("第一個Class")]
    public class TestDataEntity : DataEntity, ICodeObject, IFlagObject, IModifyObject
    {
        #region Private Member 
        private string _assignReason;
        private string _ownerId;
        private string _code;
        private System.DateTime _createDate;
        private System.DateTime _lastModifiedDate;
        private System.Guid _createBy;
        private System.Guid _lastModifiedBy;
        private bool _flag;
        #endregion 
        
        #region Parameter 
        /// <summary>
        ///分配原因
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.String)]
        [Description("分配原因")]
        public System.String AssignReason
        {
            get
            {
                return this._assignReason;
            }
            set
            {
                if ((_assignReason != value))
                {
                    this._assignReason = value;
                    this.OnPropertyChanged("AssignReason");
                }
            }
        }

        /// <summary>
        ///所有者ID
        /// </summary>
        [ReferenceProperty("User", DbType = GeneralDbType.String)]
        [Description("所有者ID")]
        public System.String OwnerId
        {
            get
            {
                return this._ownerId;
            }
            set
            {
                if ((_ownerId != value))
                {
                    this._ownerId = value;
                    this.OnPropertyChanged("OwnerId");
                }
            }
        }

        /// <summary>
        ///編碼
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.String)]
        [Description("編碼")]
        public System.String Code
        {
            get
            {
                return this._code;
            }
            set
            {
                if ((_code != value))
                {
                    this._code = value;
                    this.OnPropertyChanged("Code");
                }
            }
        }

        /// <summary>
        ///建立日期
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.DateTime)]
        [Description("建立日期")]
        public System.DateTime CreateDate
        {
            get
            {
                return this._createDate;
            }
            set
            {
                if ((_createDate != value))
                {
                    this._createDate = value;
                    this.OnPropertyChanged("CreateDate");
                }
            }
        }

        /// <summary>
        ///最後一次編輯日期
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.DateTime)]
        [Description("最後一次編輯日期")]
        public System.DateTime LastModifiedDate
        {
            get
            {
                return this._lastModifiedDate;
            }
            set
            {
                if ((_lastModifiedDate != value))
                {
                    this._lastModifiedDate = value;
                    this.OnPropertyChanged("LastModifiedDate");
                }
            }
        }

        /// <summary>
        ///建立 用戶ID
        /// </summary>
        [ReferenceProperty("User", DbType = GeneralDbType.Guid)]
        [Description("建立 用戶ID")]
        public System.Guid CreateBy
        {
            get
            {
                return this._createBy;
            }
            set
            {
                if ((_createBy != value))
                {
                    this._createBy = value;
                    this.OnPropertyChanged("CreateBy");
                }
            }
        }

        /// <summary>
        ///最後一次編輯 用戶ID
        /// </summary>
        [ReferenceProperty("User", DbType = GeneralDbType.Guid)]
        [Description("最後一次編輯 用戶ID")]
        public System.Guid LastModifiedBy
        {
            get
            {
                return this._lastModifiedBy;
            }
            set
            {
                if ((_lastModifiedBy != value))
                {
                    this._lastModifiedBy = value;
                    this.OnPropertyChanged("LastModifiedBy");
                }
            }
        }

        /// <summary>
        ///是否有效
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.Boolean)]
        [Description("是否有效")]
        public System.Boolean Flag
        {
            get
            {
                return this._flag;
            }
            set
            {
                if ((_flag != value))
                {
                    this._flag = value;
                    this.OnPropertyChanged("Flag");
                }
            }
        }
        #endregion 
    }
}