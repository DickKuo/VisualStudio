using System;


namespace CommTool.Business.Metadata
{
    public class SimplePropertyAttribute : MetadataInfoAttribute
    {
        private int _size = -2147483648;
        public int Size
        {
            get
            {
                return this._size;
            }
            set
            {
                if (value <= 0 && value != -2147483648)
                {
                    throw new ArgumentOutOfRangeException("Size");
                }
                this._size = value;
            }
        }

        public GeneralDbType DbType { get; set; }

        public SimplePropertyAttribute()
        { 
        
        }

        public SimplePropertyAttribute(GeneralDbType pDbType)
        {
            this.DbType = pDbType;
        }
        public SimplePropertyAttribute(GeneralDbType pDbType, int pSize)
        {
            this.DbType = pDbType;
            this.Size = pSize;
        }
    }
}
