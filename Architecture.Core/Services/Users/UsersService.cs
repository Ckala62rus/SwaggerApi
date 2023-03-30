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

        public async Task<bool> Delete(User Entity)
        {
            if (Entity == null) throw new ArgumentNullException(nameof(Entity));
            return await _usersRepository.Delete(Entity);
        }

        public async Task<User> GetUser(int id)
        {
            return await _usersRepository.Get(id);
        }

        public async Task<List<User>> GetUsers()
        {
            return await _usersRepository.Select();
        }

        public async Task<User> Update(User user)
        {
            return await _usersRepository.Update(user);
        }
        
        /// <summary>
        /// Get user by email field and return user model
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User> GetUserByEmail(string email)
        {
            return await _usersRepository.GetByEmail(email);
        }

        public string hashPassword(string password)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashedPassword);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            if (String.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException("Refresh token is empty");
            }

            return await _usersRepository.GetUserByRefreshToken(refreshToken);
        }
    }
}
