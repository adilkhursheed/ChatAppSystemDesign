using ChatApp.API.DTO;

namespace ChatApp.API.Repository
{
    public interface IUserRepository
    {
        Task AddActiveUser(UserDto user);
        Task<UserDto> GetActiveUser(string username);
    }
}