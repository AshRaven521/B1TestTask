using Microsoft.AspNetCore.Http;

namespace Task2.Model.Files
{
    public class FileUploaded
    {
        public IFormFile FileDetails { get; set; }
        public FileType FileType { get; set; }
    }
}
