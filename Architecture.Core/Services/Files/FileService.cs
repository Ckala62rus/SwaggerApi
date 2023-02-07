using Architecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Architecture.Core.Services.Files
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<int> Create(File file)
        {
            var validator = new FileValidator();
            var validationResult = await validator.ValidateAsync(file);

            if (!validationResult.IsValid)
            {
                throw new InvalidOperationException(string.Join("", validationResult.ToString(", ")));
            }

            return await _fileRepository.Create(file);
        }

        public async Task<bool> Delete(File file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            return await _fileRepository.Delete(file);
        }

        public async Task<File> GetFile(int id)
        {
            return await _fileRepository.Get(id);
        }

        public async Task<List<File>> GetFiles()
        {
            return await _fileRepository.Select();

        }

        public async Task<File> Update(File file)
        {
            return await _fileRepository.Update(file);
        }
    }
}
