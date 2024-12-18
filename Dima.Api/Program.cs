using Dima.Api.Data;
using Dima.Api.EndPoints;
using Dima.Api.Handlers;
using Dima.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var conexao = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(x => x.FullName);
});

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddIdentityCookies();
builder.Services.AddAuthorization();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(conexao);
});


builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => new {message = "OK"});
app.MapEndpoints();

app.Run();
