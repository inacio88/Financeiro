using Dima.Api.Common.Api;
using Dima.Api.EndPoints.Categories;
using Dima.Api.EndPoints.Identity;
using Dima.Api.EndPoints.Reports;
using Dima.Api.EndPoints.Transactions;
using Dima.Api.Models;

namespace Dima.Api.EndPoints
{
    public static class Endpoint
    {
        public static void MapEndpoints(this WebApplication app)
        {
            var endpoints = app.MapGroup("");

            endpoints.MapGroup("/")
                .WithTags("Health check")
                .MapGet("/", () => new { message = "OK" });

            endpoints.MapGroup("v1/categories")
                .WithTags("Categories")
                .RequireAuthorization()
                .MapEndpoint<CreateCategoryEndpoint>()
                .MapEndpoint<UpdateCategoryEndpoint>()
                .MapEndpoint<DeleteCategoryEndpoint>()
                .MapEndpoint<GetCategoryByIdEndpoint>()
                .MapEndpoint<GetAllCategoriesEndpoint>()
                ;

            endpoints.MapGroup("v1/transactions")
                .WithTags("Transactions")
                .RequireAuthorization()
                .MapEndpoint<CreateTransactionEndpoint>()
                .MapEndpoint<UpdateTransactionEndpoint>()
                .MapEndpoint<DeleteTransactionEndpoint>()
                .MapEndpoint<GetTransactionByIdEndpoint>()
                .MapEndpoint<GetTransactionsByPeriodEndpoint>()
                ;

            endpoints.MapGroup("v1/identity")
                .WithTags("Identity")
                .MapIdentityApi<User>();

            endpoints.MapGroup("v1/identity")
                .WithTags("Identity")
                .MapEndpoint<LogoutEndpoint>()
                .MapEndpoint<GetRolesEndpoint>();

            endpoints.MapGroup("/v1/reports")
                .WithTags("Reports")
                .RequireAuthorization()
                .MapEndpoint<GetExpensesByCategoryEndpoint>()
                .MapEndpoint<GetIncomesByCategoryEndpoint>()
                .MapEndpoint<GetFinancialSumaryEndpoint>()
                .MapEndpoint<GetIncomesAndExpensesEndpoint>()
                ;

        }


        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndpoint
        {
            TEndpoint.Map(app);
            return app;
        }








    }
}