using System.Net.Http.Json;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Web.Handlers;

public class CategoryHandler(IHttpClientFactory httpClientFactory) : ICategoryHandler
{
    private const string categoriesDefaultUrl = "v1/categories";
    private readonly HttpClient _clientHttp = httpClientFactory.CreateClient(Configuration.HttpClientName);
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        var result = await _clientHttp.PostAsJsonAsync(categoriesDefaultUrl, request);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>() 
               ?? new Response<Category?>(null, 400, "Falha ao criar uma categoria");
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        var result = await _clientHttp.PutAsJsonAsync($"v1/categories/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>() 
               ?? new Response<Category?>(null, 400, "Falha ao atualizar uma categoria");
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        var result = await _clientHttp.DeleteAsync($"v1/categories/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Category?>>() 
               ?? new Response<Category?>(null, 400, "Falha ao deletar uma categoria");
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        return await _clientHttp.GetFromJsonAsync<Response<Category?>>("v1/categories/{id}") ?? 
               new Response<Category?>(null,  400, "Não foi possível encontrar a categoria");
    }

    public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
    {
        return await _clientHttp.GetFromJsonAsync<PagedResponse<List<Category>>>(categoriesDefaultUrl) ?? 
            new PagedResponse<List<Category>>(null, 400, "Não foi possível obter a lista e categorias");
    }
}