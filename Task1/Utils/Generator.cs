using System;
using System.Linq;

namespace Task1.Utils
{
    /// <summary>
    /// Class that contains functions that fill CustomString object fields
    /// </summary>
    public static class Generator
    {
        private static Random random = new Random();
        private static readonly int minInt = 1;
        private static readonly int maxInt = 100000000;

        private static readonly double minDouble = 1;
        private static readonly double maxDoble = 20;

        private static readonly int maxStringLength = 10;
        /// <summary>
        /// Get string with latin numbers
        /// </summary>
        /// <returns> String that contains of 10 latin symbols </returns>
        public static string GetRandomLatinString()
        {
            const string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
            return new string(Enumerable.Repeat(chars, maxStringLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        /// <summary>
        /// Get string with russian numbers
        /// </summary>
        /// <returns> String that contains of 10 russian symbols </returns>
        public static string GetRandomRussianString()
        {
            const string chars = "АаБбВвГгДдЕеЁёЖжЗзИиЙйКкЛлМмНнОоПпРрСсТтУуФфХхЦцЧчШшЩщЪъЫыЬьЭэЮюЯя";
            return new string(Enumerable.Repeat(chars, maxStringLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        /// <summary>
        /// Get random even number from 1 to 100 000
        /// </summary>
        /// <returns></returns>
        public static int GetRandomNumber()
        {
            int val = (2 * random.Next(minInt / 2, maxInt / 2));
            return val;
        }
        /// <summary>
        /// Get random double number from 1 to 20
        /// </summary>
        /// <returns></returns>
        public static double GetRandomDoubleNumber()
        {
            return Math.Round(random.NextDouble() * (maxDoble - minDouble) + minDouble, 8);
        }
        /// <summary>
        /// Get random date in past 5 years
        /// </summary>
        /// <returns></returns>
        public static DateTime GetRandomDate()
        {
            DateTime today = DateTime.Today;
            DateTime start = today.AddYears(-5);
            int range = (today - start).Days;

            return start.AddDays(random.Next(range)).ToLocalTime();
        }


    }
}
