using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommTool.Business.Metadata;
using CommTool.Service;
using CommTool.Business;

namespace CreateXML
{
    public class RecordCode :DataEntity
    {
        #region Private Member
        private System.String _typeKey = "RecordCode";
        private System.Guid _recordCodeId;
        private System.String _context;
        private System.DateTime _recordDate;
        private System.String _author;
        private System.String _fileName;
        private System.String _customer;
        #endregion
        #region Parameter

        /// <summary>
        ///
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.String)]
        public System.String TypeKey
        {
            get
            {
                return this._typeKey;
            }          
        }

        /// <summary>
        ///
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.Guid)]
        public System.Guid RecordCodeId
        {
            get
            {
                return this._recordCodeId;
            }
            set
            {
                if ((_recordCodeId != value))
                {
                    this._recordCodeId = value;
                }
            }
        }
        /// <summary>
        ///
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.String)]
        public System.String Context
        {
            get
            {
                return this._context;
            }
            set
            {
                if ((_context != value))
                {
                    this._context = value;
                }
            }
        }
        /// <summary>
        ///
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.DateTime)]
        public System.DateTime RecordDate
        {
            get
            {
                return this._recordDate;
            }
            set
            {
                if ((_recordDate != value))
                {
                    this._recordDate = value;
                }
            }
        }
        /// <summary>
        ///
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.String)]
        public System.String Author
        {
            get
            {
                return this._author;
            }
            set
            {
                if ((_author != value))
                {
                    this._author = value;
                }
            }
        }
        /// <summary>
        ///
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.String)]
        public System.String FileName
        {
            get
            {
                return this._fileName;
            }
            set
            {
                if ((_fileName != value))
                {
                    this._fileName = value;
                }
            }
        }
        /// <summary>
        ///
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.String)]
        public System.String Customer
        {
            get
            {
                return this._customer;
            }
            set
            {
                if ((_customer != value))
                {
                    this._customer = value;
                }
            }
        }
        #endregion 

    }

    public class TempC : DataEntity
    {
        private System.Guid _recordCodeId;
        private System.String _context;
        [SimpleProperty(DbType = GeneralDbType.Guid)]
        public System.Guid RecordCodeId
        {
            get
            {
                return this._recordCodeId;
            }
            set
            {
                if ((_recordCodeId != value))
                {
                    this._recordCodeId = value;
                }
            }
        }
        /// <summary>
        ///
        /// </summary>
        [SimpleProperty(DbType = GeneralDbType.String)]
        public System.String Context
        {
            get
            {
                return this._context;
            }
            set
            {
                if ((_context != value))
                {
                    this._context = value;
                }
            }
        }
    }
}
