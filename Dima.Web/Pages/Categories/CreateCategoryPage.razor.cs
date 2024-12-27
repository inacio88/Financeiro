using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories;

public class CreateCategoryPage : ComponentBase
{
    #region Propriedades
    public bool IsBusy { get; set; } = false;
    public CreateCategoryRequest InpupModel { get; set; } = new();
    #endregion
    
    #region Servicos
    [Inject]
    public ICategoryHandler handler { get; set; } = null!;
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
                navigatiomManager.NavigateTo("/categories");
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