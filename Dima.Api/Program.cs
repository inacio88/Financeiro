using Dima.Api.Common.Api;
using Dima.Api.EndPoints;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();
builder.AddSecurity();
builder.AddDataContexts();
builder.AddCrossOrigin();
builder.AddDocumentation();
builder.AddServices();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.ConfigureDevEnviroment();
}

app.UseSecurity();


app.MapGet("/", () => new {message = "OK"});
app.MapEndpoints();



app.Run();
