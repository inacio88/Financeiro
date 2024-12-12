using Dima.Api.Data;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var conexao = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlServer(conexao);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => {
    x.CustomSchemaIds(x => x.FullName);
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/v1/categories", (CreateCategoryRequest request, Handler handler) =>
    handler.Handle(request))
.WithName("Categories: Create")
.WithSummary("Cria uma nova categoria")
.Produces<Response<Category>>()
;

app.Run();
