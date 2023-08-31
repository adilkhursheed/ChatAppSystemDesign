namespace ChatApp.API.Services
{
    public interface IDiscoverService
    {
        Task<string> DiscoverChanel(string userName);
    }
}