using Microsoft.AspNetCore.Components;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using MudBlazor;
namespace Dima.Web.Pages.Transactions;

public partial class EditTransactionPage : ComponentBase
{
    #region Propriedades
    public bool IsBusy { get; set; } = false;
    public UpdateTransactionRequest InputModel { get; set; } = new();
    public List<Category> categories { get; set; } = new();
    [Parameter]
    public string Id { get; set; } = string.Empty;
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
            var result = await transactionHandler.UpdateAsync(InputModel);
            if (result.IsSuccess)
            {
                snackBar.Add(result.Message, Severity.Success);
                navigatiomManager.NavigateTo("/lancamentos/historico");
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
        await GetTransactionById();
        await GetCategories();
    }

    private async Task GetCategories()
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
    private async Task GetTransactionById()
    {
        IsBusy = true;
        try
        {
            var result = await transactionHandler.GetByIdAsync(new GetTransactionByIdRequest { Id = long.Parse(Id) });
            if (result is { IsSuccess: true, Data: not null })
            {
                InputModel = new UpdateTransactionRequest
                {
                    CategoryId = result.Data.CategoryId,
                    PaidOrReceivedAt = result.Data.PaidOrReceivedAt,
                    Title = result.Data.Title,
                    Type = result.Data.Type,
                    Amount = result.Data.Amount,
                    Id = result.Data.Id
                };
                
                snackBar.Add("Dados carregados com sucesso!", Severity.Success);
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