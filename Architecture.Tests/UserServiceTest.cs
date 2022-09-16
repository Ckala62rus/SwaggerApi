using Architecture.Core.Services.Users;
using Architecture.Domain.Entities;
using AutoFixture;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Xunit;

namespace Architecture.Tests
{
    public class UserServiceTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<IUsersRepository> _usersRepositoryMock;
        private readonly UsersService _service;

        public UserServiceTest()
        {
            _fixture = new Fixture();
            _usersRepositoryMock = new Mock<IUsersRepository>();
            _service = new UsersService(_usersRepositoryMock.Object);
        }

        [Fact]
        public async Task Test_create_user_should_return_userId()
        {
            // arrange
            var userIdExcpected = _fixture.Create<int>();

            var user = _fixture
                .Build<User>()
                .Without(u => u.Id)
                .With(u => u.Email, _fixture.Create<MailAddress>().Address)
                .Create();

            _usersRepositoryMock
                .Setup(x => x.Create(user))
                .ReturnsAsync(userIdExcpected);

            // act
            var createUserIdActual = await _service.Create(user);

            // assert
            Assert.Equal(userIdExcpected, createUserIdActual);

            _usersRepositoryMock.Verify(x => x.Create(user), Times.Once);
        }

        [Theory]
        [MemberData(nameof(UsersDataWithIncorrectRequestParams))]
        public async Task Test_invalid_create_new_user_without_email_argument_null_exception(User user)
        {
            // arrange
            // act
            // assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Create(user));
        }

        public static IEnumerable<object[]> UsersDataWithIncorrectRequestParams()
        {
            return new List<object[]>
            {
                new object[] { new User { Name = "Ckala", Email = "",                       Password = "" } },
                new object[] { new User { Name = "Ckala", Email = "agr.akyla@mail.ru",      Password = "" } },
                new object[] { new User { Name = "Ckala", Email = "",                       Password = "123456" } },
                new object[] { new User { Name = "Ckala", Email = "incorrectEmailAddress",  Password = "123456" } },
            };
        }

        [Theory]
        [MemberData(nameof(UserDataWhenEmailAlreadyExist))]
        public async Task Test_get_exception_when_email_exist(User user)
        {
            // arrange
            var existUser = _fixture
                .Build<User>()
                .With(x => x.Email, user.Email)
                .Without(x => x.Id)
                .Create();

            // act
            _usersRepositoryMock
                .Setup(x => x.GetByEmail(user.Email))
                .ReturnsAsync(existUser);

            // assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Create(user));

            _usersRepositoryMock.Verify(x => x.GetByEmail(user.Email), Times.Once);
            _usersRepositoryMock.Verify(x => x.Create(user), Times.Never);
        }

        public static IEnumerable<object[]> UserDataWhenEmailAlreadyExist()
        {
            return new List<object[]>
            {
                new object[] { new User { Name = "Ckala", Email = "agr.akyla@mail.ru", Password = "123456" } },
            };
        }

        [Fact]
        public async Task Test_equals_password_hash_code()
        {
            // arrange
            var user = _fixture
                .Build<User>()
                .Without(x => x.Id)
                .With(u => u.Email, _fixture.Create<MailAddress>().Address)
                .Create();

            var hashPassword = _service.hashPassword(user.Password);

            // act
            var userCreated = await _service.Create(user);

            // assert
            Assert.Equal(hashPassword, user.Password);
        }

        [Fact]
        public async Task Test_get_list_users()
        {
            // arrange
            var usersExcpected = _fixture
                .CreateMany<User>(5)
                .ToList();

            _usersRepositoryMock
                .Setup(x => x.Select())
                .ReturnsAsync(usersExcpected);

            // act
            var usersActual = await _service.GetUsers();

            // assert
            Assert.NotNull(usersActual);
            Assert.Equal(usersExcpected.Count , usersActual.Count);
            _usersRepositoryMock.Verify(x => x.Select(), Times.Once);
        }
    }
}
