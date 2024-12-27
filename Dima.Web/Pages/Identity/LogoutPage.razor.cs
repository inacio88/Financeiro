using Microsoft.AspNetCore.Components;
using Dima.Core.Handlers;
using MudBlazor;
using Dima.Web.Security;
namespace Dima.Web.Pages.Identity
{
    public partial class LogoutPage : ComponentBase
    {
        [Inject]
        public ICookieAuthenticationStateProvider authStateProvider { get; set; } = null!;
        [Inject]
        public IAccountHandler handler { get; set; } = null!;
        [Inject]
        public ISnackbar snackbar { get; set; } = null!;
        [Inject]
        public NavigationManager navigationManager { get; set; } = null!;
        
        protected override async Task OnInitializedAsync()
        {
            if (await authStateProvider.CheckAuthenticatedAsync())
            {
                System.Console.WriteLine("logouttttttttttt");
                await handler.LogoutAsync();
                await authStateProvider.GetAuthenticationStateAsync();
                authStateProvider.NotifyAuthenticationStateChanged();
            }

            await base.OnInitializedAsync();
        }
    }
}