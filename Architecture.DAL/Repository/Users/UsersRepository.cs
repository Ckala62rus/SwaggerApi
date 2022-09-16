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

        public bool Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<User> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context
               .Users
               .Where(user => user.Email == email)
               .FirstOrDefaultAsync();
        }

        public async Task<List<User>> Select()
        {
            var users = await _context.Users
                .AsNoTracking()
                .ToListAsync();
            return users;
        }

        public Task<User> Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}