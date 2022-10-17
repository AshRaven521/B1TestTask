SELECT
(
 (SELECT MAX(DoubleNumber) FROM
   (SELECT TOP 50 PERCENT DoubleNumber FROM [B1].[dbo].[CustomsTable] ORDER BY DoubleNumber) AS BottomHalf)
 +
 (SELECT MIN(DoubleNumber) FROM
   (SELECT TOP 50 PERCENT DoubleNumber FROM [B1].[dbo].[CustomsTable] ORDER BY DoubleNumber DESC) AS TopHalf)
) / 2 AS Median