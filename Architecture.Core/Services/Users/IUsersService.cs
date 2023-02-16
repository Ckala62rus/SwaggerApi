using Architecture.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Architecture.Core.Services.Users
{
    public interface IUsersService
    {
        Task<List<User>> GetUsers();
        Task<int> Create(User user);
        Task<User> GetUser(int id);
        Task<bool> Delete(User user);
        Task<User> Update(User user);
        Task<User> GetUserByEmail(string email);
        public string hashPassword(string password);
    }
}
