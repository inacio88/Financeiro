-- Get expenses by category
CREATE OR ALTER VIEW [vwGetExpensesByCCategory]
as
    SELECT
        [Transaction].[UserId],
        [Category].[Title] as [Category],
        YEAR([Transaction].[PaidOrReceivedAt]) as [Year],
        SUM([Transaction].[Amount]) as [Expenses]
    FROM [Transaction]
        INNER JOIN [Category] ON [Transaction].[CategoryId] = [Category].[Id]
    WHERE 
        [Transaction].[PaidOrReceivedAt] >= DATEADD(MONTH, -11, CAST(GETDATE() AS DATE))
        AND
        [Transaction].[PaidOrReceivedAt] < DATEADD(MONTH, 1, CAST(GETDATE() AS DATE))
        AND
        [Transaction].[Type] = 2
    GROUP BY
    [Transaction].[UserId],
    [Category].[Title],
    YEAR([Transaction].[PaidOrReceivedAt])