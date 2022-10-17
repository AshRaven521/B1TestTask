/****** Скрипт для команды SelectTopNRows из среды SSMS  ******/
SELECT SUM(CAST(Number AS decimal))
  FROM [B1].[dbo].[CustomsTable]