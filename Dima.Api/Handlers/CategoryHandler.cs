using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers
{
    public class CategoryHandler(AppDbContext appDbContext) : ICategoryHandler
    {
        public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
        {
            try
            {
                var category = new Category
                {
                    UserId = request.UserId,
                    Title = request.Title,
                    Descripition = request.Description
                };

                await appDbContext.Categories.AddAsync(category);
                await appDbContext.SaveChangesAsync();

                return new Response<Category>(category, 201, "Categoria criada com sucesso");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //serilog
                return new Response<Category?>(null, 500, "Não foi possível criar a categoria");
                //throw new Exception("Falha ao criar categoria");
            }
        }

        public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
        {
            try
            {
                var category = await appDbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                if (category is null)
                    return new Response<Category?>(null, 404, "Categoria não encontrada");

                category.Title = request.Title;
                category.Descripition = request.Description;

                appDbContext.Categories.Update(category);
                await appDbContext.SaveChangesAsync();

                return new Response<Category?>(category, message: "Categoria atualizada com sucesso");
            }
            catch (Exception)
            {

                return new Response<Category?>(null, 500, "[FP079] Não foi possível alterar a categoria");
            }

        }
        public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
        {
            try
            {
                var category = await appDbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                if (category is null)
                    return new Response<Category?>(null, 404, "Categoria não encontrada");

                appDbContext.Categories.Remove(category);

                await appDbContext.SaveChangesAsync();

                return new Response<Category?>(category, message: "Categoria excluída com sucesso!");
            }
            catch (Exception)
            {
                return new Response<Category?>(null, 500, "[FP079] Não foi possível alterar a categoria");
            }
        }

        public Task<Response<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
        {
            throw new NotImplementedException();
        }

    }
}