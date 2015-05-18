using System;

namespace CommTool.Business
{
    public interface IOwnerObject
    {
        string AssignReason { set; get; }
        string OwnerId { set; get; }
    }
}
