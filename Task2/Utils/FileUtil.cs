using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Task2.Utils
{
    public static class FileUtil
    {
        public static async Task CopyStream(Stream stream, string downloadPath)
        {
            using (var fileStream = new FileStream(downloadPath, FileMode.Open, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
        }

        public static async void FillFile(string pathToTmpFile, byte[] array)
        {
            var fileContent = new MemoryStream(array);
            await CopyStream(fileContent, pathToTmpFile);
        }

        public static void DeleteTempFile(string path)
        {
            File.Delete(path);
        }

        public static string MapPath(string path)
        {
            var physicalPath = Path.GetTempFileName();
            return physicalPath;
        }

    }
}
