namespace CommTool.Business.Services
{
    public interface IServiceNotification
    {
        void Start(string[] args);
        void Pause();
        void Continue();
        void Stop();
    }
}