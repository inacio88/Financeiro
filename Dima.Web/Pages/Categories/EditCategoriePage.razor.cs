using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories;

public partial class EditCategoriePage : ComponentBase
{
    #region Propriedades
    public bool IsBusy { get; set; } = false;
    [Parameter]
    public string Id { get; set; } = string.Empty;

    public UpdateCategoryRequest InputModel { get; set; } = new();
    #endregion
    
    #region Services
    [Inject]
    public NavigationManager navigationManager { get; set; } = null!;
    [Inject]
    public ICategoryHandler categoryHandler { get; set; } = null!;
    [Inject]
    public ISnackbar snackbar { get; set; } = null!;
    #endregion
    
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        GetCategoryByIdRequest request = null;
        try
        {
            request = new GetCategoryByIdRequest()
            {
                Id = long.Parse(Id),
            };
        }
        catch (Exception e)
        {
            snackbar.Add("Parâmetro inválido", Severity.Error);
        }
        
        if (request is null)
            return;
        
        IsBusy = true;
        try
        {
            var response = await categoryHandler.GetByIdAsync(request);
            if (response is { IsSuccess: true, Data: not null })
            {
                InputModel = new UpdateCategoryRequest()
                {
                    Id = response.Data.Id,
                    Title = response.Data.Title,
                    Description = response.Data.Descripition
                };
            }

            snackbar.Add("Carregado com sucesso!", Severity.Success);
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

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        try
        {
            var result = await categoryHandler.UpdateAsync(InputModel);
            if (result.IsSuccess)
            {
                snackbar.Add("Editado com sucesso", Severity.Success);
                navigationManager.NavigateTo("/categorias");
            }
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