using System;

namespace Task1.Models
{
    public class CustomString
    {
        public DateTime RandomDate { get; set; }
        public string RandomLatinLetters { get; set; }
        public string RandomRussianLetters { get; set; }
        public int RandomNumber { get; set; }
        public double RandomDoubleNumber { get; set; }

        public override string ToString()
        {
            return $"{RandomDate.ToString("yyyy.MM.dd")}||{RandomLatinLetters}||{RandomRussianLetters}||{RandomNumber}||{RandomDoubleNumber}||";
        }
    }
}
