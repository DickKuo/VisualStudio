using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{

    public class DataTableMappingInfo
    {
        private IDataEntityType _dataEntityType;
        private DataFieldDictionary _fields;
        public IDataEntityType DataEntityType
        {
            get
            {
                return this._dataEntityType;
            }
            set
            {
                this._dataEntityType = value;
            }
        }
        public DataFieldDictionary Fields
        {
            get
            {
                return this._fields;
            }
            set
            {
                this._fields = value;
            }
        }
        public DataTableMappingInfo(IDataEntityType entityType)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException("entityType");
            }
            this.DataEntityType = entityType;
            this.Fields = new DataFieldDictionary();
        }
    }
}
