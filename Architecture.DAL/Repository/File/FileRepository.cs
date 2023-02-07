using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Architecture.Core.Services.Files;
using Microsoft.EntityFrameworkCore;

namespace Architecture.DAL.Repository.File
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _context;

        public FileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Domain.Entities.File entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;

            await _context.Files.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> Delete(Domain.Entities.File entity)
        {
            _context.Files.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Domain.Entities.File> Get(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var file = await _context.Files
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return file;
        }

        public async Task<List<Domain.Entities.File>> Select()
        {
            var files = await _context.Files
                .AsNoTracking()
                .ToListAsync();
            return files;
        }

        public async Task<Domain.Entities.File> Update(Domain.Entities.File entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.UpdatedAt = DateTime.Now;

            _context.Files.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}