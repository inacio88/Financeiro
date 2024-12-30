using Dima.Api.Data;
using Dima.Core.Common.Extensions;
using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers
{
    public class TransactionHandler(AppDbContext context) : ITransactionHandler
    {
        public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
        {
            if (request is {Type: ETransactionType.Withdraw, Amount: >= 0})
            {
                request.Amount *= -1;
            }
            try
            {
                var transaction = new Transaction
                {
                    UserId = request.UserId,
                    CategoryId = request.CategoryId,
                    CreatedAt = DateTime.Now,
                    Amount = request.Amount,
                    PaidOrReceivedAt = request.PaidOrReceivedAt,
                    Title = request.Title,
                    Type = request.Type
                };

                await context.Transactions.AddAsync(transaction);
                await context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, code: 201, message: "Transaction created successfully");
            }
            catch
            {
                return new Response<Transaction?>(null, code: 500, message: "Transaction creation failed");
            }
        }
        
        public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
        {
            if (request is {Type: ETransactionType.Withdraw, Amount: >= 0})
            {
                request.Amount *= -1;
            }
            try
            {
                var transaction = await context.Transactions
                    .FirstOrDefaultAsync(x => x.Id == request.Id && request.UserId == x.UserId);
                if (transaction is null)
                    return new Response<Transaction?>(null, code: 404, message: "Transaction not found");

                transaction.CategoryId = request.CategoryId;
                transaction.Amount = request.Amount;
                transaction.Title = request.Title;
                transaction.Type = request.Type;
                transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;
                
                context.Transactions.Update(transaction);
                await context.SaveChangesAsync();
                
                return new Response<Transaction?>(transaction, message: "Transaction updated successfully");
            }
            catch
            {
                return new Response<Transaction?>(null, code: 500, message: "Transaction update failed");
            }
            
        }

        public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
        {
            try
            {
                var transaction = await context.Transactions
                    .FirstOrDefaultAsync(x => x.Id == request.Id && request.UserId == x.UserId);
                if (transaction is null)
                    return new Response<Transaction?>(null, code: 404, message: "Transaction not found");
                
                context.Transactions.Remove(transaction);
                await context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, code: 201, message: "Transaction deleted successfully");
            }
            catch
            {
                return new Response<Transaction?>(null, code: 500, message: "Transaction delete failed");
            }
        }

        public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
        {
            try
            {
                var transaction = await context.Transactions
                    .FirstOrDefaultAsync(x => x.Id == request.Id && request.UserId == x.UserId);
                if (transaction is null)
                    return new Response<Transaction?>(null, code: 404, message: "Transaction not found");
                
                return new Response<Transaction?>(transaction);
            }
            catch
            {
                return new Response<Transaction?>(null, code: 500, message: "Transaction delete failed");
            }
        }

        public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransacionsByPeriodRequest request)
        {
            try
            {
                request.StartDate ??= DateTime.Now.GetFirstDayOfMonth();
                request.EndDate ??= DateTime.Now.GetLastDayOfMonth();

            }
            catch
            {
                return new PagedResponse<List<Transaction>?>(null, code: 500, message: "Não foi possível determinar a data de início ou término");
            }

            try
            {
                var query = context.Transactions.AsNoTracking()
                    .Where(x => x.PaidOrReceivedAt >= request.StartDate &&
                                x.PaidOrReceivedAt <= request.EndDate &&
                                x.UserId == request.UserId)
                    .OrderByDescending(x => x.PaidOrReceivedAt);
            
                var transactrions = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var count = await query.CountAsync();
                
                return new PagedResponse<List<Transaction>?>(transactrions, count, request.PageNumber, request.PageSize);
            }
            catch
            {
                return new PagedResponse<List<Transaction>?>(null, 500, "não foi possivel obter as transaçeõs");
            }


        }

       
    }
}