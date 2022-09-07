using Architecture.Core.Services.Members;
using Architecture.Domain.Entities;
using AutoFixture;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Architecture.Tests
{
    public class MembersServiceTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IMembersRepository> _membersRepositoryMock;
        private readonly MembersService _service;

        public MembersServiceTests()
        {
            _fixture = new Fixture();
            _membersRepositoryMock = new Mock<IMembersRepository>();
            _service = new MembersService(_membersRepositoryMock.Object);
        }

        [Fact]
        public async Task Create_ValidMember_shouldReturnCreateMemberId()
        {
            //arrange
            var expectedMemberId = _fixture.Create<int>();

            var member = _fixture.Build<Member>()
                .Without(x => x.Id).Create();

            _membersRepositoryMock
                .Setup(x => x.Add(member))
                .ReturnsAsync(expectedMemberId);

            //act
            var memberId = await _service.Create(member);

            //assert
            Assert.Equal(expectedMemberId, memberId);

            _membersRepositoryMock.Verify(x => x.Add(member), Times.Once);
        }

        [Fact]
        public async Task Create_MemberAlreadyExists_ShouldThrowInvalidOperationException()
        {
            //arrange
            var member = _fixture.Build<Member>()
                .Without(x => x.Id)
                .Create();

            _membersRepositoryMock
               .Setup(x => x.Get(member.YouTubeUserId))
               .ReturnsAsync(member);

            //act
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Create(member));

            //assert
            _membersRepositoryMock.Verify(x => x.Get(member.YouTubeUserId), Times.Once);
            //_membersRepositoryMock.Verify(x => x.Add(It.IsAny<Member>()), Times.Once);
        }

        [Fact]
        public async Task Create_MemberIsNull_ShouldThrowArgumentNullException()
        {
            //arrange
            //var service = new MembersService(_membersRepositoryMock.Object);

            //act
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.Create(null));

            //assert
            _membersRepositoryMock.Verify(x => x.Add(It.IsAny<Member>()), Times.Never);
        }

        [Theory]
        [InlineData(142, "test", "test")]
        [InlineData(-53, "test", "test")]
        [InlineData(0, null, "test")]
        [InlineData(0, "", "test")]
        [InlineData(0, "test", null)]
        [InlineData(0, null, null)]
        [InlineData(0, "", "")]
        [InlineData(53, " ", " ")]
        [InlineData(0, null, " ")]
        public async Task Create_InvalidMember_ShouldThrowArgumentNullException(int id = 0, string name = null, string youtubeUserId = null)
        {
            //arrange
            var member = new Member { Id = id, Name = name, YouTubeUserId = youtubeUserId };

            //act
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Create(member));

            //assert
            _membersRepositoryMock.Verify(x => x.Add(It.IsAny<Member>()), Times.Never);
        }

        [Fact]
        public async Task Get_ShouldReturnMembers()
        {
            //arrange
            var expectedMembers = _fixture.CreateMany<Member>(42).ToArray();

            _membersRepositoryMock
                .Setup(x => x.Get())
                .ReturnsAsync(expectedMembers);

            //act
            var members = await _service.Get();

            //assert
            Assert.NotEmpty(members);
            Assert.Equal(42, expectedMembers.Length);

            _membersRepositoryMock.Verify(x => x.Get(), Times.Once);
        }

        [Fact]
        public async Task Get_YouTubeUserIdValid_ShouldReturnMember()
        {
            // arrange
            var youtubeUserId = _fixture.Create<string>();

            var expectedMember = _fixture
                .Build<Member>()
                .With(x => x.YouTubeUserId, youtubeUserId)
                .Create();

            _membersRepositoryMock
                .Setup(x => x.Get(youtubeUserId))
                .ReturnsAsync(expectedMember);

            // act
            var member = await _service.Get(youtubeUserId);

            // assert
            Assert.NotNull(member);
            Assert.Equal(youtubeUserId, member.YouTubeUserId);
            _membersRepositoryMock.Verify(x => x.Get(youtubeUserId), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task Get_YouTubeUserIdIsNotValid_ShouldReturnThrowExceptionArgument(string youtubeUserId)
        {
            // arrange
            // act

            // assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.Get(youtubeUserId));
            _membersRepositoryMock.Verify(x => x.Get(youtubeUserId), Times.Never);
        }
    }
}