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
        /// Создает кортеж из двух списоков : список объектов модели Balance и список объектов модели CustomString из excel файла
        /// </summary>
        /// <param name="stream"> Поток байт excel файла </param>
        /// <param name="fileId"> id execel файла </param>
        /// <returns></returns>
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
                            var balance = new Balance
                            {
                                // Используем приведение к double, а потом к нужному типу, что избежать упаковки/распаковки
                                CountId = Convert.ToInt32(cellValue),
                                FileId = fileId,
                                InputActive = Convert.ToDecimal(row.GetCell(1).NumericCellValue),
                                InputPassive = Convert.ToDecimal(row.GetCell(2).NumericCellValue),
                                Debit = Convert.ToDecimal(row.GetCell(3).NumericCellValue),
                                Credit = Convert.ToDecimal(row.GetCell(4).NumericCellValue),
                                OutputActive = Convert.ToDecimal(row.GetCell(5).NumericCellValue),
                                OutputPassive = Convert.ToDecimal(row.GetCell(6).NumericCellValue),
                                ExcelRowNumber = i
                            };

                            balanceList.Add(balance);
                        }
                        else
                        {
                            var cs = new CustomString
                            {
                                FileId = fileId,
                                ExcelRowNumber = i,
                                Content = row.GetCell(0).ToString()
                            };

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
