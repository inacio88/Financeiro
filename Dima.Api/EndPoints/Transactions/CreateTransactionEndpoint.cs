using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.EndPoints.Transactions;

public class CreateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("Transactions: Create")
            .WithSummary("Cria uma nova transação")
            .WithDescription("Cria uma nova transação")
            .WithOrder(1)
            .Produces<Response<Transaction?>>()
    ;

    private static async Task<IResult> HandleAsync(ClaimsPrincipal user, ITransactionHandler handler, CreateTransactionRequest request)
    {
        var userName = user.Identity?.Name ?? string.Empty;
        request.UserId = userName;
        var result = await handler.CreateAsync(request);
            
        if (result.IsSuccess)
            return TypedResults.Created($"/{result.Data?.Id}", result);


        return TypedResults.BadRequest(result);
    }
}