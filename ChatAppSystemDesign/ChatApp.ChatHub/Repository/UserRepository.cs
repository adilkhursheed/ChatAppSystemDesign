using ChatApp.API.DTO;

namespace ChatApp.API.Repository
{
    public class UserRepository : IUserRepository
    {
        private List<UserDto> ActiveUsers { get; set; }
        public UserRepository()
        {
            this.ActiveUsers = new List<UserDto>();
        }

        public async Task AddActiveUser(UserDto user)
        {
            user.UserId = this.ActiveUsers.Count + 1;
            await Task.Run(() => this.ActiveUsers.Add(user));
        }
        public async Task<UserDto> GetActiveUser(string username)
        {
            return await Task.Run(() => this.ActiveUsers.FirstOrDefault(x => x.UserName == username));
        }
    }
}
