using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Dima.Web.Pages.Identity
{
    public partial class RegisterPage : ComponentBase
    {
        [Inject]
        public AuthenticationStateProvider authStateProvider { get; set; } = null!;
        [Inject]
        public IAccountHandler handler { get; set; } = null!;
        [Inject]
        public ISnackbar snackbar { get; set; } = null!;
        [Inject]
        public NavigationManager navigationManager { get; set; } = null!;


        public RegisterRequest InputModel { get; set; } = new();
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
                var result = await handler.RegisterAsync(InputModel);
                if (result.IsSuccess)
                {
                    snackbar.Add(result.Message, Severity.Success);
                    navigationManager.NavigateTo("/login");
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