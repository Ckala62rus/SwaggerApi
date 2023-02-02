using Architecture.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Architecture.Core.Services.Members
{
    public interface IMembersService
    {
        Task<int> Create(Member member);
        Task<List<Member>> Get(int page);
        Task<Member> Get(string youTubeId);
        Task<int> Count();
        //Task<MemberStatistic> GetStatistics(int memberId);
    }
}
