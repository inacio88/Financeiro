using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.EndPoints.Transactions;

public class GetTransactionByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandleAsync)
            .WithName("Transactions: Getbyid")
            .WithSummary("Recupera uma nova transaction")
            .WithDescription("Recupera uma nova transaction")
            .WithOrder(4)
            .Produces<Response<Transaction?>>()
    ;

    private static async Task<IResult> HandleAsync(ITransactionHandler handler, long id)
    {
        var request = new GetTransactionByIdRequest() {Id = id, UserId = "inacioId"};
        var result = await handler.GetByIdAsync(request);

        if (result.IsSuccess)
            return TypedResults.Ok(result);


        return TypedResults.BadRequest(result);
    }
}