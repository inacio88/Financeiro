using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.EndPoints.Transactions;

public class GetTransactionsByPeriodEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("Transaction: Get all categories")
            .WithSummary("Recupera uma lista de transaction")
            .WithDescription("Recupera uma lista de transaction")
            .WithOrder(5)
            .Produces<PagedResponse<List<Transaction>?>>()
    ;

    private static async Task<IResult> HandleAsync(ITransactionHandler handler,[FromQuery] DateTime? startDate = null,[FromQuery] DateTime? endDate = null, [FromQuery] int pageNumber = Configuration.DefaultPageNumber, [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetTransacionsByPeriodRequest() 
            {
                UserId = "inacioId", 
                PageSize = pageSize, 
                PageNumber = pageNumber,
                StartDate = startDate,
                EndDate = endDate
            };
        var result = await handler.GetByPeriodAsync(request);

        if (result.IsSuccess)
            return TypedResults.Ok(result);


        return TypedResults.BadRequest(result);
    }
}