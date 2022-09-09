using Architecture.Core.Services.Members;
using Architecture.DAL.Repository.Members;
using Architecture.Domain.Entities;
using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Architecture.Tests
{
    public class MembersTestWithDb : TestCommandBase
    {
        private readonly MembersRepository _repository;
        private readonly MembersService _memberService;
        private readonly Fixture _fixture;

        public MembersTestWithDb()
        {
            _repository = new MembersRepository(Context);
            _memberService = new MembersService(_repository);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAllMembersTest()
        {
            // arrange
            var members = _fixture
                .Build<Member>()
                .Without(m => m.Id)
                .CreateMany(5);

            var membersIds = new List<int>();

            // act
            foreach (var member in members)
            {
                var newId = await _memberService.Create(member);
                membersIds.Add(newId);
            }

            // assert
            Assert.Equal(members.Count(), membersIds.Count);
        }

        [Fact]
        public async Task GetMembersByYouTubeUserIdTest()
        {
            // arrange
            var member = _fixture
                .Build<Member>()
                .Without(m => m.Id)
                .Create();

            // act
            var createdId = await _memberService.Create(member);
            var memberCreated = await _memberService.Get(member.YouTubeUserId);

            // assert
            Assert.NotNull(memberCreated);
            Assert.Equal(member.Name, memberCreated.Name);
            Assert.Equal(member.YouTubeUserId, memberCreated.YouTubeUserId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task Get_MemberByYouTubeIdInvalidOperation(string youtubeUserId)
        {
            // arrange
            // act
            // assert
            await Assert.ThrowsAsync<ArgumentNullException>( () => _memberService.Get(youtubeUserId));
        }
    }
}
