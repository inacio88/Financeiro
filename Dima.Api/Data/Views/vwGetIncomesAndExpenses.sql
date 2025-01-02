-- Get incomes by category
CREATE OR ALTER VIEW [vwGetIncomesAndExpenses]
as
SELECT
    [Transaction].[UserId],
    MONTH([Transaction].[PaidOrReceivedAt]) as [Month],
    Year([Transaction].[PaidOrReceivedAt]) as [Year],
    SUM(case when [Transaction].[Type] = 1 then [Transaction].[Amount] else 0 end) as [Incomes],
    SUM(case when [Transaction].[Type] = 2 then [Transaction].[Amount] else 0 end) as [Expenses]

FROM [Transaction]
WHERE
    [Transaction].[PaidOrReceivedAt] >= DATEADD(MONTH, -11, CAST(GETDATE() AS DATE))
  AND
    [Transaction].[PaidOrReceivedAt] < DATEADD(MONTH, 1, CAST(GETDATE() AS DATE))

GROUP BY
    [Transaction].[UserId],
    MONTH([Transaction].[PaidOrReceivedAt]),
    Year([Transaction].[PaidOrReceivedAt])