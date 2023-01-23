using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task2.Model.Files;
using Task2.Repository;

namespace Task2.Service
{
    public class FileService : IFileService
    {
        private IFileRepository repository;

        public FileService(IFileRepository repository)
        {
            this.repository = repository;
        }
        public async Task DownloadFileById(int fileName)
        {
            await repository.DownloadFileById(fileName);
        }

        public async Task<FileDetails> GetFileByIdAsync(int id)
        {
            return await repository.GetFileByIdAsync(id);
        }

        public async Task<IEnumerable<SimpleFile>> GetFileNames()
        {
            return await repository.GetFileNames();
        }

        public async Task<int> PostFileAsync(IFormFile fileData, FileType fileType)
        {
            return await repository.PostFileAsync(fileData, fileType);
        }
    }
}
