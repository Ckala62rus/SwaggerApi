using Architecture.Contracts;
using Architecture.Controllers;
using Architecture.Core.Services.Members;
using Architecture.Domain.Entities;
using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Architecture.Tests
{
    public class MemberControllerTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<IMembersRepository> _membersRepositoryMock;
        private readonly MembersService _service;
        private readonly MembersController _controller;
        private readonly Mock<IMembersService> _membersServiceMock;
        private readonly IMapper _mapper;

        public MemberControllerTest()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new ApiMappingProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            _fixture = new Fixture();
            _membersRepositoryMock = new Mock<IMembersRepository>();
            _service = new MembersService(_membersRepositoryMock.Object);
            //_controller = new MembersController(_service, _mapper);
            _membersServiceMock = new Mock<IMembersService>();
            _controller = new MembersController(_membersServiceMock.Object, _mapper);
        }

        [Fact]
        public async Task Get_All_Members()
        {
            //arrange
            var expectedMembers = _fixture.CreateMany<Member>(42).ToList();

            _membersServiceMock
                .Setup(x => x.Get(1))
                .ReturnsAsync(expectedMembers);

            _membersServiceMock
                .Setup(x => x.Count())
                .ReturnsAsync(expectedMembers.Count);

            // act
            var okResult = await _controller.Get() as OkObjectResult;

            var Data = (List<MemberCreateDTO>)okResult
                .Value
                .GetType()
                .GetProperty("Data")
                .GetValue(okResult.Value);

            var Total = (double)okResult
                .Value
                .GetType()
                .GetProperty("Total")
                .GetValue(okResult.Value);

            // assert
            Assert.Equal(expectedMembers.Count, Data.Count);
            Assert.Equal(expectedMembers.Count / 2, Total);
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async Task Test_create_member()
        {
            //var user = this.user
            // arrange
            var id = _fixture.Create<int>();
            var inputData = new MemberCreateDTO
            {
                Name = "testing",
                YouTubeUserId = "some_user_id"
            };

            var member = _mapper.Map<Member>(inputData);

            _membersServiceMock
                .Setup(x => x.Create(member))
                .ReturnsAsync(id);

            _membersServiceMock
                .Setup(x => x.Get(member.YouTubeUserId))
                .ReturnsAsync(new Member
                {
                    Id = id,
                    Name = member.Name,
                    YouTubeUserId = member.YouTubeUserId
                });

            // act
            var okResult = await _controller.Create(inputData) as OkObjectResult;

            var res = okResult.Value as Member;

            // assert
            Assert.NotNull(res);
            Assert.Equal(id, res.Id);
        }

        [Fact]
        public async Task Test_get_auth_user()
        {
            // arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userName", "example name"),
                new Claim("id", "5"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // act
            var res = await _controller.TestAuth() as OkObjectResult;
            var userId = res.Value;

            // assert
            Assert.NotNull(userId);
            Assert.Equal(5, userId);
        }
    }
}
