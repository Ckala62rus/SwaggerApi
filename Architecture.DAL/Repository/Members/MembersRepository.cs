using Architecture.Core.Services.Members;
using Architecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architecture.DAL.Repository.Members
{
    public class MembersRepository : IMembersRepository
    {
        private readonly ApplicationDbContext _context;
        public const float PAGE_SIZE_FOR_PAGINATE = 2f;

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

        public async Task<List<Member>> Get(int page)
        {
            //var pageCount = Math.Ceiling(_context.Members.Count() / PAGE_SIZE_FOR_PAGINATE);

            var memversEntities = await _context
                .Members
                .Skip((page - 1) * (int)PAGE_SIZE_FOR_PAGINATE)
                .Take((int)PAGE_SIZE_FOR_PAGINATE)
                //.AsNoTracking()
                //.Select(member => new Member
                //{
                //    Id = member.Id,
                //    Name = member.Name,
                //    YouTubeUserId= member.YouTubeUserId,
                //})
                //.ToArrayAsync();
                .ToListAsync();

            return memversEntities;
        }

        public async Task<Member> Get(string youTubeId)
        {
            return await _context
                .Members
                .Where(member => member.YouTubeUserId == youTubeId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> Count()
        {
            return await _context.Members.CountAsync();
        }
    }
}
