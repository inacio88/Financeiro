using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.EndPoints.Reports;

public class GetFinancialSumaryEndpoint: IEndpoint
{
    private static async Task<IResult> HandleAsync(ClaimsPrincipal user, GetFinancialSummaryRequest request, IReportHandler handler)
    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        var result = await handler.GetFinancialSummaryReportAsync(request);
        
        return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
    }
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/summary", HandleAsync)
            .Produces<Response<FinancialSummary?>>();
    }
}