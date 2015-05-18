using System;

namespace CommTool.Business.Metadata
{
    [Serializable]
    public abstract class MetadataInfoAttribute :Attribute
    {
        //protected MetadataInfoAttribute();

        public string Alias { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public bool IsBrowsable { get; set; }
        public object Clone()
        {
            return base.MemberwiseClone();
        }
    }
    public enum GeneralDbType
    {
        Auto = 0,
        Boolean = 1,
        DateTime = 2,
        Decimal = 3,
        Guid = 4,
        Image = 5,
        Int32 = 6,
        String = 7,
        Text = 8,
        Date = 9,
        Int64 = 10,
        BigObject = 11,
    }
 
}
