
using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Transactions;

public partial class ListTransactionPage : ComponentBase
{
    #region Parameters

    public bool IsBusy { get; set; } = false;
    public string SearchTerm { get; set; } = string.Empty;
    public List<Transaction> transactions { get; set; } = [];
    public int CurrentYear { get; set; } = DateTime.Now.Year;
    public int CurrentMonth { get; set; } = DateTime.Now.Month;

    public int[] Years { get; set; } =
    {
        DateTime.Now.Year,
        DateTime.Now.AddYears(-1).Year,
        DateTime.Now.AddYears(-2).Year,
        DateTime.Now.AddYears(-3).Year,
        DateTime.Now.AddYears(-4).Year,
    };

    #endregion
    
    #region Servicos
    [Inject]
    public ITransactionHandler transactionHandler { get; set; } = null!;
    [Inject]
    public NavigationManager navigatiomManager { get; set; } = null!;
    [Inject]
    public ISnackbar snackBar { get; set; } = null!;
    [Inject]
    public IDialogService dialogService { get; set; } = null!;
    #endregion
    
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        await GetTransactions();
    }

    #endregion
    
    #region Methods

    private async Task GetTransactions()
    {
        IsBusy = true;
        try
        {
            var request = new GetTransacionsByPeriodRequest()
            {
                StartDate = DateTime.Now.GetFirstDayOfMonth(CurrentYear, CurrentMonth),
                EndDate = DateTime.Now.GetLastDayOfMonth(CurrentYear, CurrentMonth),
                PageNumber = 1,
                PageSize = 100,
            };
            var result = await transactionHandler.GetByPeriodAsync(request);
            if (result.IsSuccess)
                transactions = result.Data ?? [];
            
            snackBar.Add("Lista carregada com sucessso!", Severity.Success);
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

    public Func<Transaction, bool> Filter => transaction =>
    {
        if (string.IsNullOrEmpty(SearchTerm))
            return true;
        return transaction.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
               || transaction.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);
    };

    public async void OnDeleteButtonClickedAsync(long id, string title)
    {
        var result = await dialogService.ShowMessageBox("Atenção",
            $"Ao prosseguir o lançamento {title} será excluído",
            yesText: "Excluir",
            cancelText:"Cancelar");
        if (result is true)
            await OnDelete(id, title);
        
        StateHasChanged();
    }

    private async Task OnDelete(long id, string title)
    {
        IsBusy = true;
        try
        {
            var result = await transactionHandler.DeleteAsync(new DeleteTransactionRequest() { Id = id });

            if (result.IsSuccess)
            {
                snackBar.Add($"Lançamento '{title}' removido!", Severity.Success);
                transactions.RemoveAll(x => x.Id == id);
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