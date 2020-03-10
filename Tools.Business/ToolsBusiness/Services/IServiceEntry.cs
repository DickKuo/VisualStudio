using System;

namespace CommTool.Business.Services
{
    public interface IServiceEntry
    {
        Type ServiceInterface
        {
            get;
        }
        Type ServiceClass
        {
            get;
        }
        ServiceCreateType ServiceCreateType
        {
            get;
        }
    }
    public enum ServiceCreateType
    {
        Instance,
        Callback,
        Startup
    }   
}