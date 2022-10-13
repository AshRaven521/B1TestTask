using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Task1.Models;

namespace Task1.Utils
{
    public static class Generator
    {
        private static Random random = new Random();
        private static readonly int minInt = 1;
        private static readonly int maxInt = 100000000;

        private static readonly double minDouble = 1;
        private static readonly double maxDoble = 20;

        private static readonly int maxStringLength = 10;
        private static readonly int maxFileLength = 100000;
        private static readonly int maxFilesCount = 100;

        public static string GetRandomLatinString()
        {
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
            return new string(Enumerable.Repeat(chars, maxStringLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GetRandomRussianString()
        {
            const string chars = "АаБбВвГгДдЕеЁёЖжЗзИиЙйКкЛлМмНнОоПпРрСсТтУуФфХхЦцЧчШшЩщЪъЫыЬьЭэЮюЯя";
            return new string(Enumerable.Repeat(chars, maxStringLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static int GetRandomNumber()
        {
            int val = random.Next();
            if (val < minInt || val > maxInt)
            {
                val = minInt;
            }
            return val;
        }

        public static double GetRandomDoubleNumber()
        {
            return Math.Round(random.NextDouble() * (maxDoble - minDouble) + minDouble, 8);
        }

        public static DateTime GetRandomDate()
        {
            DateTime today = DateTime.Today;
            DateTime start = today.AddYears(-5);
            int range = (today - start).Days;

            return start.AddDays(random.Next(range)).Date;
        }

        public static bool GenerateFile(CustomString custom, string filePath)
        {
            using(var sw = new StreamWriter(filePath))
            {
                for (int i = 1; i <= maxFileLength; i++)
                {
                    custom.RandomDate = GetRandomDate();
                    custom.RandomDoubleNumber = GetRandomDoubleNumber();
                    custom.RandomLatinLetters = GetRandomLatinString();
                    custom.RandomNumber = GetRandomNumber();
                    custom.RandomRussianLetters = GetRandomRussianString();

                    string resString = $"{i}. {custom}";

                    sw.WriteLine(resString);
                }
            }

            return true;
        }

        public static Tuple<bool, long> GenerateFiles(CustomString myStr, string folderPath)
        {
            string filePath = string.Empty;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var watch = new Stopwatch();
            watch.Start();

            for (int i = 1; i <= maxFilesCount; i++)
            {
                filePath = folderPath + "/" + $"File № {i}.txt";
                var isDone = GenerateFile(myStr, filePath);
            }
            watch.Stop();

            var test = Tuple.Create(true, watch.ElapsedMilliseconds / 1000);

            return test;
        }
    }
}
