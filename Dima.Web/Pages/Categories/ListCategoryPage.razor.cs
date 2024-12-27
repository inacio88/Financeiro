using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories;

public partial class ListCategoryPage : ComponentBase
{
    #region Propriedades
    public bool IsBusy { get; set; } = false;
    public List<Category> categories { get; set; } = [];
    #endregion
    
    #region Services
    [Inject]
    public ISnackbar snackbar { get; set; } = null!;
    [Inject]
    public ICategoryHandler handler { get; set; } = null!;
    #endregion
    
    
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetAllCategoriesRequest();
            var result = await handler.GetAllAsync(request);
            if(result.IsSuccess)
                categories = result.Data ?? [];
        }
        catch (Exception e)
        {
            snackbar.Add(e.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
}