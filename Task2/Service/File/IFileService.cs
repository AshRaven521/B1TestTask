using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task2.Model.Files;

namespace Task2.Service
{
    public interface IFileService
    {
        public Task<int> PostFileAsync(IFormFile fileData, FileType fileType);

        public Task DownloadFileById(int fileName);

        public Task<FileDetails> GetFileByIdAsync(int id);

        public Task<IEnumerable<SimpleFile>> GetFileNames();
    }
}
