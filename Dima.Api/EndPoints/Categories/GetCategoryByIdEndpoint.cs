using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.EndPoints.Categories
{
    public class GetCategoryByIdEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/{id}", HandleAsync)
                .WithName("Categories: Getbyid")
                .WithSummary("Recupera uma nova categoria")
                .WithDescription("Recupera uma nova categoria")
                .WithOrder(4)
                .Produces<Response<Category?>>()
                ;

        private static async Task<IResult> HandleAsync(ClaimsPrincipal user, ICategoryHandler handler, long id)
        {
            var userName = user.Identity?.Name ?? string.Empty;
            var request = new GetCategoryByIdRequest {Id = id, UserId = userName};
            var result = await handler.GetByIdAsync(request);

            if (result.IsSuccess)
                return TypedResults.Ok(result);


            return TypedResults.BadRequest(result);
        }
    }
}