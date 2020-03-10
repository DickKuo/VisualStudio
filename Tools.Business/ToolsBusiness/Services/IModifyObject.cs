using System;

namespace CommTool.Business
{
    public interface IModifyObject
    {
        Guid CreateBy { set; get; }
        DateTime CreateDate { set; get; }
        Guid LastModifiedBy { set; get; }
        DateTime LastModifiedDate { set; get; }
    }
}