using MessengerApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApp.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user);
    }
}
