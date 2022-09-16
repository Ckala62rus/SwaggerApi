using Architecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Architecture.Core.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<int> Create(User user)
        {
            var validator = new UserValidator();
            var validationResult = await validator.ValidateAsync(user);

            if (!validationResult.IsValid) throw new InvalidOperationException(string.Join("", validationResult.ToString(", ")));

            var mailExist = await _usersRepository.GetByEmail(user.Email);
            if (mailExist != null) throw new InvalidOperationException("Email already exists");

            user.Password = hashPassword(user.Password);

            return await _usersRepository.Create(user);
        }

        public bool Delete(User Entity)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetUsers()
        {
            return await _usersRepository.Select();
        }

        public Task<User> Update(User entity)
        {
            throw new NotImplementedException();
        }

        public string hashPassword(string password)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashedPassword);
        }
    }
}
