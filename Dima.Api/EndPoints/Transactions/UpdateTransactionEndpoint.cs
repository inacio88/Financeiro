using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
namespace Dima.Api.EndPoints.Transactions;

public class UpdateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id}", HandleAsync)
            .WithName("Transactions: Update")
            .WithSummary("Atualiza uma nova transaction")
            .WithDescription("Atualiza uma nova transaction")
            .WithOrder(2)
            .Produces<Response<Transaction?>>()
    ;


    private static async Task<IResult> HandleAsync(ClaimsPrincipal user, ITransactionHandler handler,
        UpdateTransactionRequest request,
        long id)
    {
        var userName = user.Identity?.Name ?? string.Empty;
        request.Id = id;
        request.UserId = userName;
        var result = await handler.UpdateAsync(request);

        if (result.IsSuccess)
            return TypedResults.Ok(result);


        return TypedResults.BadRequest(result);
    }
}