using System.Net.Http.Json;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Dima.Core.Common.Extensions;

namespace Dima.Web.Handlers;

public class TransactionHandler(IHttpClientFactory httpClientFactory) : ITransactionHandler
{
    private const string transactionsDefaultUrl = "v1/transactions";
    private readonly HttpClient _clientHttp = httpClientFactory.CreateClient(Configuration.HttpClientName);
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        var result = await _clientHttp.PostAsJsonAsync(transactionsDefaultUrl, request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() 
               ?? new Response<Transaction?>(null, 400, "Falha ao criar uma transação");
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        var result = await _clientHttp.PutAsJsonAsync($"v1/transactions/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() 
               ?? new Response<Transaction?>(null, 400, "Falha ao atualizar uma transação");
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        var result = await _clientHttp.DeleteAsync($"v1/transactions/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() 
               ?? new Response<Transaction?>(null, 400, "Falha ao deletar uma transação");
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        return await _clientHttp.GetFromJsonAsync<Response<Transaction?>>($"v1/transactions/{request.Id}")
                    ?? new Response<Transaction?>(null, 404, "Não foi possível recuperar a transação");
    }

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransacionsByPeriodRequest request)
    {
        const string format = "yyyy-MM-dd";
        
        var startDate = request.StartDate is not null
                            ? request.StartDate.GetValueOrDefault().ToString(format)
                            : DateTime.Now.GetFirstDayOfMonth().ToString(format);
        
        var endDate = request.EndDate is not null
                            ? request.EndDate.GetValueOrDefault().ToString(format)
                            : DateTime.Now.GetLastDayOfMonth().ToString(format);

        var url = $"v1/transactions?startDate={startDate}&endDate={endDate}&pageNumber=1&pageSize=25";

        return await _clientHttp.GetFromJsonAsync<PagedResponse<List<Transaction>?>>(url)
                    ?? new PagedResponse<List<Transaction>?>(null, 400, "Não foi possível obter as transações");
    }
}