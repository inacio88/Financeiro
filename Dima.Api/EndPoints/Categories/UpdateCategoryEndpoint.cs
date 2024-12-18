using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.EndPoints.Categories
{
    public class UpdateCategoryEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPut("/{id}", HandleAsync)
                .WithName("Categories: Update")
                .WithSummary("Atualiza uma nova categoria")
                .WithDescription("Atualiza uma nova categoria")
                .WithOrder(2)
                .Produces<Response<Category?>>()
                ;


        private static async Task<IResult> HandleAsync(ClaimsPrincipal user, ICategoryHandler handler,
                                                        UpdateCategoryRequest request,
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
}