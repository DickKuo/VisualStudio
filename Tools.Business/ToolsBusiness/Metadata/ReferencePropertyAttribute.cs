using System;

namespace CommTool.Business.Metadata
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property)]
    public class ReferencePropertyAttribute : SimplePropertyAttribute
    {
        private string _referenceTo;
        public string ReferenceTo
        {
            get
            {
                return this._referenceTo;
            }
            set
            {
                if (value == null || value.Length == 0)
                {
                    throw new ArgumentNullException("ReferenceTo");
                }
                this._referenceTo = value;
            }
        }

        public ReferencePropertyAttribute(string pReferenceTo)
        {
            this.ReferenceTo = pReferenceTo;
        }
    }
}