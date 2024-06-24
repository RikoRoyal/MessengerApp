using MessengerApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApp.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task<User> GetByUsernameAsync(string username);
    }
}
