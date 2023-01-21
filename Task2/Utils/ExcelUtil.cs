using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
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
        /// <returns> Data table </returns>
        public static Tuple<List<Balance>, List<CustomString>> FillModels(string path, int fileId)
        {
            try
            {
                HSSFWorkbook hssfwb;
                using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    hssfwb = new HSSFWorkbook(file);
                }
                FileUtil.DeleteTempFile(path);

                ISheet sheet = hssfwb.GetSheetAt(0);

                var balanceList = new List<Balance>();
                var csList = new List<CustomString>();

                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row != null) //null is when the row only contains empty cells 
                    {
                        string cellValue = row.GetCell(0).ToString();
                        bool isNumericCell = decimal.TryParse(cellValue, out decimal result);
                        if (isNumericCell)
                        {
                            var balance = new Balance();
                            // Используем приведение к double, а потом к нужному типу, что избежать упаковки/распаковки
                            //balance.Id++;
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
                            //cs.Id++;
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

        public static List<Count> FillCountModelFromExcel(string path)
        {
            try
            {
                HSSFWorkbook hssfwb;
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    hssfwb = new HSSFWorkbook(file);
                }

                ISheet sheet = hssfwb.GetSheetAt(0);

                var countList = new List<Count>();

                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row != null) //null is when the row only contains empty cells 
                    {
                        string cellValue = row.GetCell(0).ToString();
                        bool isNumericCell = decimal.TryParse(cellValue, out decimal result);
                        if (isNumericCell)
                        {
                            var count = new Count
                            {
                                Number = Convert.ToInt32(cellValue)
                            };

                            countList.Add(count);
                        }
                    }
                }

                return countList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
