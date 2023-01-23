using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Task2.Data;
using Task2.Model.Files;
using Task2.Utils;

namespace Task2.Repository
{
    public class FileRepository : IFileRepository
    {
        private ApplicationDbContext context;

        public FileRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> PostFileAsync(IFormFile fileData, FileType fileType)
        {
            try
            {
                var fileDetails = new FileDetails()
                {
                    FileName = fileData.FileName,
                    FileType = fileType,
                };

                using (var stream = new MemoryStream())
                {
                    fileData.CopyTo(stream);
                    fileDetails.FileData = stream.ToArray();
                }

                var result = context.FileDetails.Add(fileDetails);
                await context.SaveChangesAsync();
                return result.Entity.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DownloadFileById(int Id)
        {
            try
            {
                var file = await context.FileDetails.Where(x => x.Id == Id).FirstOrDefaultAsync();

                var content = new MemoryStream(file.FileData);
                var path = Path.Combine(
                   Directory.GetCurrentDirectory(), "FileDownloaded",
                   file.FileName);

                await FileUtil.CopyStream(content, path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FileDetails> GetFileByIdAsync(int id)
        {
            var file = await context.FileDetails.FindAsync(id);
            return file;
        }

        public async Task<IEnumerable<SimpleFile>> GetFileNames()
        {
            var files = await context.FileDetails.Select(x => new SimpleFile { Id = x.Id, FileName = x.FileName }).ToListAsync();
            return files;
        }
    }
}
