using System;
using System.Linq;

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
            int val = (2 * random.Next(minInt / 2, maxInt / 2));
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

            return start.AddDays(random.Next(range)).ToLocalTime();
        }


    }
}
