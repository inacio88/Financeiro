using Microsoft.AspNetCore.Components;
using Dima.Core.Handlers;
using Dima.Core.Requests.Transactions;
using MudBlazor;
namespace Dima.Web.Pages.Transactions;

public partial class CreateTransactionPage : ComponentBase
{
    #region Propriedades
    public bool IsBusy { get; set; } = false;
    public CreateTransactionRequest InpupModel { get; set; } = new();
    #endregion
    
    #region Servicos
    [Inject]
    public ITransactionHandler handler { get; set; } = null!;
    [Inject]
    public NavigationManager navigatiomManager { get; set; } = null!;
    [Inject]
    public ISnackbar snackBar { get; set; }
    #endregion
    
    #region Métodos

    public async Task OnValidSubmit()
    {
        IsBusy = true;

        try
        {
            var result = await handler.CreateAsync(InpupModel);
            if (result.IsSuccess)
            {
                snackBar.Add(result.Message, Severity.Success);
                navigatiomManager.NavigateTo("/transacoes");
            }
            else
            {
                snackBar.Add(result.Message, Severity.Error);
            }
        }
        catch (Exception e)
        {
            snackBar.Add(e.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
        
        
        
    }
    #endregion

}