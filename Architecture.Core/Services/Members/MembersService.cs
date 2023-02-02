using Architecture.Domain.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Architecture.Core.Services.Members
{
    public class MembersService : IMembersService
    {
        private readonly IMembersRepository _membersRepository;

        public MembersService(IMembersRepository membersRepository)
        {
            _membersRepository = membersRepository;
        }

        public async Task<int> Count()
        {
            return await _membersRepository.Count();
        }

        public async Task<int> Create(Member newMember)
        {
            var validator = new MemberValidator();
            var validationResult = await validator.ValidateAsync(newMember);

            if (!validationResult.IsValid) throw new InvalidOperationException(string.Join("", validationResult.ToString(", ")));

            var existedMember = await _membersRepository.Get(newMember.YouTubeUserId);
            if (existedMember != null) throw new InvalidOperationException("Member already exists");

            return await _membersRepository.Add(newMember);
        }

        public async Task<List<Member>> Get(int page)
        {
            Log.Information("Получение данных из базы");
            return await _membersRepository.Get(page);
        }

        public async Task<Member> Get(string youTubeId)
        {
            if(String.IsNullOrWhiteSpace(youTubeId)) throw new ArgumentNullException($"{nameof(youTubeId)} can not be empty!");
            return await _membersRepository.Get(youTubeId);
        }

        public Task GetStatistics(int memberId)
        {
            throw new NotImplementedException();
        }
    }
}
