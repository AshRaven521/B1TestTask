using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Task1.Utils;

namespace Task1.Data
{
    /// <summary>
    /// Class that works with Database
    /// </summary>
    public static class DB
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private static readonly string insertIntoDBFirstPart = "INSERT INTO CustomsTable (Date, Latin, Russian, Number, DoubleNumber) ";

        private static readonly string selectNumbersSum = "SELECT SUM(CAST(Number AS decimal)) FROM CustomsTable";

        private static readonly string procedureName = "DoublesMedian";

        /// <summary>
        /// Function that insert given list of data into database
        /// </summary>
        /// <param name="stringList"> Collection of strings from file </param>
        /// <param name="callback"> Delegate calling when 10 000 strings imported </param>
        public static void InsertIntoDB(IEnumerable<string> stringList, Callbacks.insertIntoDB callback)
        {

            using (var connect = new SqlConnection(connectionString))
            {
                int counter = 0;

                connect.Open();
                foreach (var str in stringList)
                {
                    var mas = str.Split("||");
                    if (mas is not null && mas.Length > 1)
                    {
                        var date = mas[0];
                        var latin = mas[1];
                        var rus = mas[2];
                        var num = mas[3];
                        var d_num = mas[4];
                        string insertIntoDBSecondPart = "VALUES ('" + date + "', '" + latin + "', '" + rus + "', " +
                            num + ", " + d_num.Replace(',', '.') + ")";
                        string insertIntoDBCommand = insertIntoDBFirstPart + insertIntoDBSecondPart;
                        SqlCommand insertCommand = new SqlCommand(insertIntoDBCommand, connect);
                        insertCommand.ExecuteNonQuery();
                    }

                    counter++;
                    if (counter % 10000 == 0)
                    {
                        callback(counter);
                    }

                }
            }
        }
        /// <summary>
        /// Function that calculate sum of int numbers from database
        /// </summary>
        /// <returns> Decimal number that represents numbers sum </returns>
        public static decimal CalculateNumbersSum()
        {
            decimal res = 0; 

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var selectSumCommand = new SqlCommand(selectNumbersSum, connection);
                var prob_res = selectSumCommand.ExecuteScalar();
                res = Convert.ToDecimal(prob_res);
            }

            return res;
        }

        /// <summary>
        /// Function that calculate median of double numbers from database
        /// </summary>
        /// <returns> Double number that represents median </returns>
        public static double CalculateDoubleNumbersMedian()
        {
            double res = 0.0;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var selectMedianCommand = new SqlCommand(procedureName, connection);
                selectMedianCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectMedianCommand.CommandTimeout = 150;
                var prob_res = selectMedianCommand.ExecuteScalar();
                res = Convert.ToDouble(prob_res);
            }

            return res;
        }

    }
}
