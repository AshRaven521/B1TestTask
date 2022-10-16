using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.Utils;
using Task1.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Threading;

namespace Task1.Data
{
    public static class DB
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static readonly string insertIntoDBFirstPart = "INSERT INTO CustomsTable (Date, Latin, Russian, Number, DoubleNumber) ";

        public static void InsertIntoDB(IEnumerable<string> stringList, Callbacks.insertIntoDB callback)
        {
            //string insertIntoDBStringSecondPart = string.Empty;

            using (var connect = new SqlConnection(connectionString))
            {
                int counter = 0;

                connect.Open();
                foreach (var str in stringList)
                {
                    //if (!str.Contains("\r") && !string.IsNullOrEmpty(str))
                    //{
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
                    //}
                    counter++;
                    if (counter % 10000 == 0)
                    {
                        callback(counter);
                        //Thread.Sleep(150);
                    }

                }
                connect.Close();
            }
        }

    }
}
