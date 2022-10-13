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

        public void GenerateCustomString(DateTime randDate, string randLatin, string randRussian, int randNum, double randDoubleNum)
        {
            RandomDate = randDate;
            RandomLatinLetters = randLatin;
            RandomRussianLetters = randRussian;
            RandomNumber = randNum;
            RandomDoubleNumber = randDoubleNum;
        }

        public override string ToString()
        {
            return $"{RandomDate}||{RandomLatinLetters}||{RandomRussianLetters}||{RandomNumber}||{RandomDoubleNumber}||";
        }
    }
}
