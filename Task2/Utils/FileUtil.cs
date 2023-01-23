using System.IO;
using System.Threading.Tasks;

namespace Task2.Utils
{
    public static class FileUtil
    {
        /// <summary>
        /// Копирует переданный поток в файл
        /// </summary>
        /// <param name="stream"> Поток байт </param>
        /// <param name="downloadPath"> Путь к файлу, в который будет записан переданный поток </param>
        /// <returns></returns>
        public static async Task CopyStream(Stream stream, string downloadPath)
        {
            using (var fileStream = new FileStream(downloadPath, FileMode.Open, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
    }
}
