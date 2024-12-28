using Microsoft.AspNetCore.Components;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using MudBlazor;
namespace Dima.Web.Pages.Transactions;

public partial class CreateTransactionPage : ComponentBase
{
    #region Propriedades
    public bool IsBusy { get; set; } = false;
    public CreateTransactionRequest InputModel { get; set; } = new();
    public List<Category> categories { get; set; } = new();
    #endregion
    
    #region Servicos
    [Inject]
    public ITransactionHandler transactionHandler { get; set; } = null!;
    [Inject]
    public NavigationManager navigatiomManager { get; set; } = null!;
    [Inject]
    public ISnackbar snackBar { get; set; }
    [Inject]
    public ICategoryHandler categoryHandler { get; set; } = null!;
    #endregion
    
    #region Métodos

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await transactionHandler.CreateAsync(InputModel);
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

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var result = await categoryHandler.GetAllAsync(new GetAllCategoriesRequest { });
            if (result.IsSuccess)
            {
                categories = result.Data ?? [];
                InputModel.CategoryId = categories.FirstOrDefault()?.Id ?? 0;
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