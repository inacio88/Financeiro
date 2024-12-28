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
    public string SearchTerm { get; set; } = string.Empty;

    #endregion
    
    #region Services
    [Inject]
    public ISnackbar snackbar { get; set; } = null!;
    [Inject]
    public ICategoryHandler handler { get; set; } = null!;

    [Inject]
    public IDialogService dialogService { get; set; } = null!;
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
    
    #region Methods

    public Func<Category, bool> Filter => cat =>
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
            return true;
        if (cat.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;
        if (cat.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;
        if (cat.Descripition is not null && cat.Descripition.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            return true;
        
        return false;
    };

    public async void OnDeleteButtonClicked(long id, string title)
    {
        var result = await dialogService.ShowMessageBox("ATENÇÃO", 
            $"Ao prosseguir a categoria '{title}' será excluída! Deseja continuar?",
            yesText:"Excluir",
            cancelText:"Cancelar");
        if (result is true)
            await OnDeleteAsync(id, title);
        
        StateHasChanged();
    }

    public async Task OnDeleteAsync(long id, string title)
    {
        try
        {
            await handler.DeleteAsync(new DeleteCategoryRequest { Id = id });
            categories.RemoveAll(cat => cat.Id == id);
            snackbar.Add($"Categoria '{title}' foi removida com sucesso!", Severity.Success);
        }
        catch (Exception e)
        {
            snackbar.Add(e.Message, Severity.Error);
        }
    }

    public async void OnEditButtonClicked(long id, string title)
    {
        
    }

    #endregion
    
    

    
}