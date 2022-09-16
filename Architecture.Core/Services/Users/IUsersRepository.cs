using Architecture.Domain.Entities;
using System.Threading.Tasks;

namespace Architecture.Core.Services.Users
{
    public interface IUsersRepository : IBaseRepository<User>
    {
        Task<User> GetByEmail(string email);
    }
}
