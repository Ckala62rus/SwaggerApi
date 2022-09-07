using Architecture.Domain.Entities;
using System.Threading.Tasks;

namespace Architecture.Core.Services.Members
{
    public interface IMembersRepository
    {
        Task<int> Add(Member member);
        Task<Member[]> Get();
        Task<Member> Get(string youTubeId);
    }
}
