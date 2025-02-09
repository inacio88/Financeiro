using Dima.Api;
using Dima.Api.Common.Api;
using Dima.Api.EndPoints;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Components.WebAssembly.Server;

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

// Habilita arquivos padrão (ex: index.html)
app.UseDefaultFiles();

// Configuração correta para servir arquivos estáticos
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")
    ),
    ContentTypeProvider = new FileExtensionContentTypeProvider()
});

// Blazor WebAssembly requer suporte a Service Workers
app.UseBlazorFrameworkFiles(); 
//app.MapFallbackToFile("index.html");
app.UseCors(ApiConfiguration.CorsPolicyName);
app.UseSecurity();
app.MapEndpoints();

app.Run();
