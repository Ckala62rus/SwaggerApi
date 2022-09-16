using System.Collections.Generic;
using System.Threading.Tasks;

namespace Architecture.Core.Services
{
    public interface IBaseRepository<T>
    {
        Task<int> Create(T entity);
        Task<T> Get(int id);
        Task<List<T>> Select();
        bool Delete(T entity);
        Task<T> Update(T entity);
    }
}
