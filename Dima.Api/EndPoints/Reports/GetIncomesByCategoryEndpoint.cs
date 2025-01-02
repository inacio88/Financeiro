using Dima.Api.Common.Api;
using System.Security.Claims;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.EndPoints.Reports;

public class GetIncomesByCategoryEndpoint : IEndpoint
{
    private static async Task<IResult> HandleAsync(ClaimsPrincipal user, GetIncomesByCategoryRequest request, IReportHandler handler)
    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        var result = await handler.GetIncomesByCategoryReportAsync(request);
        
        return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
    }
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/incomes", HandleAsync)
            .Produces<Response<List<IncomesByCategory>?>>();
    }
}