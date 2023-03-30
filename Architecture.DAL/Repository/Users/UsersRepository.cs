using Architecture.Core.Services.Users;
using Architecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architecture.DAL.Repository.Users
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _context;

        public UsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(User entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;

            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> Delete(User entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> Get(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context
               .Users
               .Where(user => user.Email == email)
               .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            return await _context
                .Users
                .Where(user => user.RefreshToken == refreshToken)
                .FirstOrDefaultAsync();
        }

        public async Task<List<User>> Select()
        {
            var users = await _context.Users
                .AsNoTracking()
                .ToListAsync();
            return users;
        }

        public async Task<User> Update(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}