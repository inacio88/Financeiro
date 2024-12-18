using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.EndPoints.Transactions;

public class DeleteTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id}", HandleAsync)
            .WithName("Transactions: Delete")
            .WithSummary("Deleta uma nova transaction")
            .WithDescription("Deleta uma nova transaction")
            .WithOrder(3)
            .Produces<Response<Transaction?>>()
    ;

    private static async Task<IResult> HandleAsync(ClaimsPrincipal user, ITransactionHandler handler, long id)
    {
        var userName = user.Identity?.Name ?? string.Empty;
        var request = new DeleteTransactionRequest() {Id = id, UserId = userName};
        var result = await handler.DeleteAsync(request);

        if (result.IsSuccess)
            return TypedResults.Ok(result);


        return TypedResults.BadRequest(result);
    }
}