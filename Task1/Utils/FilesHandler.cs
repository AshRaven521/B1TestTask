using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task1.Models;

namespace Task1.Utils
{
    public static class FilesHandler
    {

        private static readonly int maxFileLength = 100000;
        private static readonly int maxFilesCount = 100;

        public static bool JoinFiles(string folderPath, string resFilePath)
        {
            var files = Directory.GetFiles(folderPath).Where(f => f != resFilePath);

            using (var stream = new StreamWriter(resFilePath, false, new UTF8Encoding()))
            {
                foreach (var fileName in files)
                {
                    using (var sr = new StreamReader(fileName, new UTF8Encoding()))
                    {
                        sr.BaseStream.CopyTo(stream.BaseStream);
                    }
                }
            }

            //Return true if everything goes ok
            return true;
        }

        public static List<List<string>> SearchInFile(string searchParameter, string filePath)
        {
            List<List<string>> groups = new List<List<string>>();
            List<string> current = null;
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                if (line.Contains(searchParameter) && current == null)
                {
                    current = new List<string>();
                }
                else if (line.Contains(searchParameter) && current != null)
                {
                    groups.Add(current);
                    current = null;
                }
                if (current != null)
                {
                    current.Add(line);
                }
            }


            return groups;
        }

        public static int DeleteFromFile(string filePath, string stringToDelete)
        {
            var tempFile = Path.GetTempFileName();
            //var test = SearchInFile(stringToDelete, filePath);
            //foreach (var tes in test)
            //{
            //    foreach (var t in tes)
            //    {
            //        var temp = t;
            //    }
            //}
            var lines = File.ReadLines(filePath);
            var linesToKeep = lines.Where(l => !l.Contains(stringToDelete));
            int deletedLinesCount = lines.Count() - linesToKeep.Count();

            File.WriteAllLines(tempFile, linesToKeep);

            File.Delete(filePath);
            File.Move(tempFile, filePath);

            return deletedLinesCount;
        }

        public static void GenerateFile(CustomString custom, string filePath)
        {
            using (var sw = new StreamWriter(filePath))
            {
                for (int i = 1; i <= maxFileLength; i++)
                {
                    custom.RandomDate = Generator.GetRandomDate();
                    custom.RandomDoubleNumber = Generator.GetRandomDoubleNumber();
                    custom.RandomLatinLetters = Generator.GetRandomLatinString();
                    custom.RandomNumber = Generator.GetRandomNumber();
                    custom.RandomRussianLetters = Generator.GetRandomRussianString();

                    string resString = $"{i}. {custom}";

                    sw.WriteLine(resString);

                }
            }
        }

        public static bool GenerateFiles(CustomString myStr, string folderPath)
        {

            //const int chunkSize = 10;


            //int chunks = maxFilesCount / chunkSize;
            //int chunkStart = 1;

            //for (int i = 0; i < maxFilesCount; i++)
            //{
            //int chunkEnd = chunkStart + chunkSize;

            // 2-ой вариант(TPL)
            // Генерирует целый файл с хорошими значениями(кажется, не всегда)
            // Работает примерно за 23 секунды, но не возвращает упраление на окно(может получится поправить)

            //int completed = 0;

            //ManualResetEvent allDone = new ManualResetEvent(initialState: false);

            //ThreadPool.QueueUserWorkItem(_ =>
            //{
            //    for (int i = 1; i <= maxFilesCount; i++)
            //    {
            //        string filePath = string.Empty;
            //        if (!Directory.Exists(folderPath))
            //        {
            //            Directory.CreateDirectory(folderPath);
            //        }

            //        filePath = folderPath + "/" + $"File № {i}.txt";
            //        lock (filePath) lock (myStr)
            //            {
            //                GenerateFile(myStr, filePath);
            //            }

            //    }

            //    if (Interlocked.Increment(ref completed) == maxFilesCount)
            //    {
            //        allDone.Set();
            //    }
            //});

            //allDone.WaitOne();

            //1-ый вариант (простое создание потоков)
            //Работает за 15-17 секунд, но генерирует плохие значения (Большая часть файла = одинаковые значения)

            //string filePath = string.Empty;
            //if (!Directory.Exists(folderPath))
            //{
            //    Directory.CreateDirectory(folderPath);
            //}

            //for (int i = 1; i <= maxFilesCount; i++)
            //{
            //    filePath = folderPath + "/" + $"File № {i}.txt";
            //    Thread fileThread = new Thread(() => GenerateFile(myStr, filePath));
            //    fileThread.Name = i.ToString();
            //    fileThread.Start();
            //    Thread.Sleep(150);
            //}


            // 3-ий вариант
            // Генерирует хорошие значения, но выполняется 10 минут(похоже тоже не всегда, может и 110-115 секунд)

            //string filePath = string.Empty;
            //List<string> filePaths = new List<string>();
            //if (!Directory.Exists(folderPath))
            //{
            //    Directory.CreateDirectory(folderPath);
            //}

            //for (int i = 1; i <= maxFilesCount; i++)
            //{
            //    filePath = folderPath + "/" + $"File № {i}.txt";
            //    filePaths.Add(filePath);
            //}

            //await Task.WhenAll(filePaths.Select((file) => GenerateFile(myStr, file)));

            // 4-ый вариант, без потоков, но с асинхонностью(не блокируется UI)
            // Генерирует хорошие значения, но выполняется 240-250 секунд

            //string filePath = string.Empty;
            //if (!Directory.Exists(folderPath))
            //{
            //    Directory.CreateDirectory(folderPath);
            //}

            //for (int i = 1; i <= maxFilesCount; i++)
            //{
            //    filePath = folderPath + "/" + $"File № {i}.txt";
            //    await GenerateFile(myStr, filePath);
            //}

            // 5-ый вариант, без ничего(убрать везде async/await)
            // Генерирует хорошие значения, выполняется 19 - 24 секунды

            string filePath = string.Empty;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            for (int i = 1; i <= maxFilesCount; i++)
            {
                filePath = folderPath + "/" + $"File № {i}.txt";
                GenerateFile(myStr, filePath);
            }

            //Возвращает true, если функция выполнена успешно
            return true;
        }
    }
}
