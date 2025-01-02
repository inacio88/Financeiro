using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.EndPoints.Reports;

public class GetExpensesByCategoryEndpoint : IEndpoint
{
    private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IReportHandler handler)
    {
        GetExpensesByCategoryRequest request = new()
        {
            UserId = user.Identity?.Name ?? string.Empty
        };
        var result = await handler.GetExpensesByCategoryReportAsync(request);
        
        return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
    }
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/expenses", HandleAsync)
            .Produces<Response<List<ExpensesByCategory>?>>();
    }
}