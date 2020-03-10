namespace CommTool.Business.Services
{
    public interface IServiceProviderAddin
    {
        IServiceEntry[] ServiceEntries
        {
            get;
        }
    }
}