using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Task1.Models;

namespace Task1.Utils
{
    /// <summary>
    /// Class that responsible for work with file in programm
    /// </summary>
    public static class FilesHandler
    {

        private static readonly int maxFileLength = 100000;
        private static readonly int maxFilesCount = 100;


        /// <summary>
        /// Function that get data from file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns> List of strings from file </returns>
        public static List<string> GetCustomsFromFile(string filePath)
        {
            List<string> list = new List<string>();
            using (var sr = new StreamReader(filePath))
            {
                list = sr.ReadToEnd().Split('\n').ToList();
            }

            return list;
        }
        /// <summary>
        /// Function that join all files from given folder except result file
        /// </summary>
        /// <param name="folderPath"> Path to folder with result file </param>
        /// <param name="resFilePath"> Path to result file </param>
        /// <param name="toDeleteOption"> Option that shows what strings delete from files </param>
        /// <param name="joinFilesCallback"> Delegate calling when files joined </param>
        public static void JoinFiles(string folderPath, string resFilePath, string toDeleteOption, Callbacks.joinFilesCallback joinFilesCallback)
        {
            var files = Directory.GetFiles(folderPath).Where(f => f != resFilePath);

            int deletedStrings = 0;
            IEnumerable<string> linesToKeep;
            using (var stream = new StreamWriter(resFilePath, false, new UTF8Encoding()))
            {
                foreach (var fileName in files)
                {
                    string tempFile = string.Empty;
                    using (var sr = new StreamReader(fileName, new UTF8Encoding()))
                    {
                        tempFile = Path.GetTempFileName();

                        var lines = sr.ReadToEnd().Split('\n').ToList();
                        if (!string.IsNullOrEmpty(toDeleteOption) || !string.IsNullOrWhiteSpace(toDeleteOption))
                        {
                            (deletedStrings, linesToKeep) = DeleteFromFile(lines, toDeleteOption);
                            File.WriteAllLines(tempFile, linesToKeep);
                        }
                        else
                        {
                            File.WriteAllLines(tempFile, lines);
                        }
                    }

                    File.Delete(fileName);
                    File.Move(tempFile, fileName);

                    using (var sr = new StreamReader(fileName, new UTF8Encoding()))
                    {
                        sr.BaseStream.CopyTo(stream.BaseStream);
                    }
                }
            }

            joinFilesCallback(deletedStrings);

        }
        /// <summary>
        /// Delete strings that contains given parametr from given list of string
        /// </summary>
        /// <param name="data"> Source list with data from file </param>
        /// <param name="stringToDelete"> Option based on which function will delete string or not </param>
        /// <returns> Collection that contains files strings except of strings that contain given parametr </returns>
        private static Tuple<int, IEnumerable<string>> DeleteFromFile(List<string> data, string stringToDelete)
        {
            var linesToKeep = data.Where(l => !l.Contains(stringToDelete));
            int deletedLinesCount = data.Count() - linesToKeep.Count();

            return Tuple.Create(deletedLinesCount, linesToKeep);
        }
        /// <summary>
        /// Function that generate file with given CustomString object
        /// </summary>
        /// <param name="custom"> Custom string object that will write in a file </param>
        /// <param name="filePath"> Path where file need to be created </param>
        private static void GenerateFile(CustomString custom, string filePath)
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

                    string resString = $"{custom}";

                    sw.WriteLine(resString);

                }
            }
        }
        /// <summary>
        /// Function that generate 100 files
        /// </summary>
        /// <param name="custom"> CustomString object that will be send to GenerateFile function </param>
        /// <param name="folderPath"> Path to folder with all files </param>
        /// <param name="callback"> Delegate calling when all files created</param>
        public static void GenerateAllFiles(CustomString custom, string folderPath, Callbacks.generateFilesCallback callback)
        {
            string filePath = string.Empty;
            for (int i = 1; i <= maxFilesCount; i++)
            {
                filePath = folderPath + "/" + $"File № {i}.txt";
                GenerateFile(custom, filePath);
            }

            callback();
        }
        /// <summary>
        /// Function that shows my pain)
        /// I don't want to delete it because here shows how I experemented with different variants of generating file
        /// </summary>
        /// <param name="myStr"></param>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static bool GenerateFiles(CustomString myStr, string folderPath)
        {

            /* Несколько приведенных мною способов для создания файлов(просто жалко удалять) */

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

            //    //if (Interlocked.Increment(ref completed) == maxFilesCount)
            //    //{
            //    //    allDone.Set();
            //    //}
            //    allDone.Set();
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

            //string filePath = string.Empty;
            //if (!Directory.Exists(folderPath))
            //{
            //    Directory.CreateDirectory(folderPath);
            //}

            //for (int i = 1; i <= maxFilesCount; i++)
            //{
            //    filePath = folderPath + "/" + $"File № {i}.txt";
            //    GenerateFile(myStr, filePath);
            //}

            //6-ой вариант (1 поток на создание всех файлов)

            //Thread th = new Thread(() => GenerateAllFiles(myStr, folderPath));
            //th.Start();

            //Возвращает true, если функция выполнена успешно
            return true;
        }
    }
}
