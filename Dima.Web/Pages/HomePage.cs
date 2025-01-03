using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages;

public partial class HomePage : ComponentBase
{
    #region Fields
    public bool ShowValues { get; set; } = true;
    public FinancialSummary? Summary { get; set; }
    #endregion
    
    #region Services
    [Inject]
    public ISnackbar snackbar { get; set; } = null!;
    [Inject]
    public IReportHandler handler { get; set; } = null!;
    #endregion
    
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        var request = new GetFinancialSummaryRequest();
        var result = await handler.GetFinancialSummaryReportAsync(request);
        if(result.IsSuccess)
            Summary = result.Data;
    }

    #endregion
    
    #region Private Methods

    public void ToggleShowValues()
    {
        ShowValues = !ShowValues;
    }
    #endregion
}