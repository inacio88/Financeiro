using Dima.Api.Common.Api;
using Microsoft.AspNetCore.Identity;
using Dima.Api.Models;

namespace Dima.Api.EndPoints.Identity
{
    public class LogoutEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/logout", HandleAsync)
                .RequireAuthorization()
                // .WithName("Identity: logout")
                // .WithSummary("Faz logout")
                // .WithDescription("Faz logout")
                // .WithOrder(1)
                ;

        private static async Task<IResult> HandleAsync(SignInManager<User> signInManager)
        {
            await signInManager.SignOutAsync();
            return Results.Ok();
        }
    }
}