using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using Task2.Model;

namespace Task2.Utils
{
    public static class ExcelUtil
    {
        /// <summary>
        /// Create tuple of list of balance models and list of custom strings models from excel file
        /// </summary>
        /// <param name="path"> Path to excel file </param>
        /// <returns> Tuple of two lists : balance models list and custom string models list </returns>
        public static Tuple<List<Balance>, List<CustomString>> FillModels(Stream stream, int fileId)
        {
            try
            {
                HSSFWorkbook hssfwb;

                using (stream)
                {
                    hssfwb = new HSSFWorkbook(stream);
                }

                var sheet = hssfwb.GetSheetAt(0);

                var balanceList = new List<Balance>();
                var csList = new List<CustomString>();

                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row != null) //null кода все ячейки пустые 
                    {
                        string cellValue = row.GetCell(0).ToString();
                        bool isNumericCell = decimal.TryParse(cellValue, out decimal result);
                        if (isNumericCell)
                        {
                            var balance = new Balance();
                            // Используем приведение к double, а потом к нужному типу, что избежать упаковки/распаковки
                            balance.CountId = Convert.ToInt32(cellValue);
                            balance.FileId = fileId;
                            balance.InputActive = Convert.ToDecimal(row.GetCell(1).NumericCellValue);
                            balance.InputPassive = Convert.ToDecimal(row.GetCell(2).NumericCellValue);
                            balance.Debit = Convert.ToDecimal(row.GetCell(3).NumericCellValue);
                            balance.Credit = Convert.ToDecimal(row.GetCell(4).NumericCellValue);
                            balance.OutputActive = Convert.ToDecimal(row.GetCell(5).NumericCellValue);
                            balance.OutputPassive = Convert.ToDecimal(row.GetCell(6).NumericCellValue);
                            balance.ExcelRowNumber = i;

                            balanceList.Add(balance);
                        }
                        else
                        {
                            var cs = new CustomString();
                            cs.FileId = fileId;
                            cs.ExcelRowNumber = i;
                            cs.Content = row.GetCell(0).ToString();

                            csList.Add(cs);
                        }
                    }
                }

                var tuple = Tuple.Create(balanceList, csList);

                return tuple;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
