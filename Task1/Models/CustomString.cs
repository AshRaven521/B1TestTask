using System;

namespace Task1.Models
{
    /// <summary>
    /// Class that represents one file string
    /// </summary>
    public class CustomString
    {
        /// <summary>
        /// Represents date in period of last 5 years
        /// </summary>
        public DateTime RandomDate { get; set; }
        /// <summary>
        /// Represents string of 10 random latin symbols
        /// </summary>
        public string RandomLatinLetters { get; set; }
        /// <summary>
        /// Represents string of 10 random russian symbols
        /// </summary>
        public string RandomRussianLetters { get; set; }
        /// <summary>
        /// Represents positive int number from 1 to 100 000
        /// </summary>
        public int RandomNumber { get; set; }
        /// <summary>
        /// Represents positive double number from 1 to 20 with 8 signs after comma
        /// </summary>
        public double RandomDoubleNumber { get; set; }
        /// <summary>
        /// String representation of class
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{RandomDate.ToString("yyyy.MM.dd")}||{RandomLatinLetters}||{RandomRussianLetters}||{RandomNumber}||{RandomDoubleNumber}||";
        }
    }
}
