using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.EndPoints.Categories
{
    public class GetAllCategoriesEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/", HandleAsync)
                .WithName("Categories: Get all categories")
                .WithSummary("Recupera uma lista de categorias")
                .WithDescription("Recupera uma lista de categorias")
                .WithOrder(5)
                .Produces<PagedResponse<List<Category>?>>()
                ;

        private static async Task<IResult> HandleAsync(ClaimsPrincipal user , ICategoryHandler handler, [FromQuery] int pageNumber = Configuration.DefaultPageNumber, [FromQuery] int pageSize = Configuration.DefaultPageSize)
        {
            var userName = user.Identity?.Name ?? string.Empty;
            var request = new GetAllCategoriesRequest {UserId = userName, PageNumber = pageNumber, PageSize = pageSize};
            var result = await handler.GetAllAsync(request);

            if (result.IsSuccess)
                return TypedResults.Ok(result);


            return TypedResults.BadRequest(result);
        }
    }
}