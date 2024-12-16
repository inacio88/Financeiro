using Dima.Api.Data;
using Dima.Api.EndPoints;
using Dima.Api.Handlers;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var conexao = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(conexao);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(x => x.FullName);
});
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => new {message = "OK"});
app.MapEndpoints();

app.Run();
