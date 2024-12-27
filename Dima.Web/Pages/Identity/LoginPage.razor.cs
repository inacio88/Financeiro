using Microsoft.AspNetCore.Components;
using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using MudBlazor;
using Dima.Web.Security;
namespace Dima.Web.Pages.Identity
{
    public partial class LoginPage : ComponentBase
    {
        [Inject]
        public ICookieAuthenticationStateProvider authStateProvider { get; set; } = null!;
        [Inject]
        public IAccountHandler handler { get; set; } = null!;
        [Inject]
        public ISnackbar snackbar { get; set; } = null!;
        [Inject]
        public NavigationManager navigationManager { get; set; } = null!;


        public LoginRequest InputModel { get; set; } = new();
        public bool IsBusy { get; set; } = false;


        protected override async Task OnInitializedAsync()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is { IsAuthenticated: true })
                navigationManager.NavigateTo("/");
        }

        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;

            try
            {
                var result = await handler.LoginAsync(InputModel);
                if (result.IsSuccess)
                {
                    snackbar.Add(result.Message, Severity.Success);
                    await authStateProvider.GetAuthenticationStateAsync();
                    authStateProvider.NotifyAuthenticationStateChanged();
                    navigationManager.NavigateTo("/");
                }
                else
                    snackbar.Add(result.Message, Severity.Error);
            }
            catch (Exception ex)
            {
                snackbar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}