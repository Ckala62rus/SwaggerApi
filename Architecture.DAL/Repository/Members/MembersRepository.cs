using Architecture.Core.Services.Members;
using Architecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Architecture.DAL.Repository.Members
{
    public class MembersRepository : IMembersRepository
    {
        private readonly ApplicationDbContext _context;

        public MembersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(Member newMember)
        {
            if (newMember == null) throw new ArgumentNullException(nameof(newMember));

            var newMemberEntity = new Member
            {
                Name = newMember.Name,
                YouTubeUserId = newMember.YouTubeUserId,
            };

            await _context.Members.AddAsync(newMemberEntity);
            await _context.SaveChangesAsync();

            return newMemberEntity.Id;
        }

        public async Task<Member[]> Get()
        {
            var memversEntities = await _context
                .Members
                .AsNoTracking()
                .Select(member => new Member
                {
                    Id = member.Id,
                    Name = member.Name,
                    YouTubeUserId= member.YouTubeUserId,
                })
                .ToArrayAsync();

            return memversEntities;
        }

        public async Task<Member> Get(string youTubeId)
        {
            return await _context
                .Members
                .Where(member => member.YouTubeUserId == youTubeId)
                .FirstOrDefaultAsync();
        }
    }
}
