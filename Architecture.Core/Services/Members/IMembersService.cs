using Architecture.Domain.Entities;
using System.Threading.Tasks;

namespace Architecture.Core.Services.Members
{
    public interface IMembersService
    {
        Task<int> Create(Member member);
        Task<Member[]> Get();
        Task<Member> Get(string youTubeId);
        //Task<MemberStatistic> GetStatistics(int memberId);
    }
}
