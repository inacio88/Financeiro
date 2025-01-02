using Dima.Core.Handlers;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Components.Reports;

public partial class ExpensesByCategoryChartComponent : ComponentBase
{
    #region Parameters
    public List<double> Data { get; set; } = [];
    public List<string> Labels { get; set; } = [];
    #endregion
    
    #region Services
    [Inject]
    public IReportHandler handler { get; set; } = null!;
    [Inject]
    public ISnackbar snackbar { get; set; } = null!;
    #endregion
    
    #region overrides

    protected override async Task OnInitializedAsync()
    {
        await GetExpensesByCategoryReportAsync();
    }

    private async Task GetExpensesByCategoryReportAsync()
    {
        try
        {
            var request = new GetExpensesByCategoryRequest();
            var result = await handler.GetExpensesByCategoryReportAsync(request);
            if (result.IsSuccess || result.Data is null)
            {
                snackbar.Add("Falha ao obter dados", Severity.Error);
                return;
            }

            foreach (var item in result.Data)
            {
                Labels.Add($"{item.Category} ({item.Expenses:C})");
                Data.Add(-(double)item.Expenses);
            }
        }
        catch (Exception e)
        {
            snackbar.Add("Falha ao carregar componente", Severity.Error);
        }
    }
    #endregion
}