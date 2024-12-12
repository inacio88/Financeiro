using Dima.Api.Data;
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

app.MapGet("/v1/products", () => "Hello World!");

app.Run();
