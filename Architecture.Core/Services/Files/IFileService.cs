using Architecture.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Architecture.Core.Services.Files
{
    public interface IFileService
    {
        Task<List<File>> GetFiles();
        Task<int> Create(File user);
        Task<File> GetFile(int id);
        Task<bool> Delete(File user);
        Task<File> Update(File user);
    }
}
