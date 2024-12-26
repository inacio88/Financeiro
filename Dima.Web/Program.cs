using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Dima.Web;
using MudBlazor.Services;
using Dima.Web.Security;
using Microsoft.AspNetCore.Components.Authorization;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
Configuration.BackendUrl = builder.Configuration.GetValue<string>("Backendurl") ?? throw new Exception("Não foi encontrada backendurl");
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CookieHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped( x => (ICookieAuthenticationStateProvider) x.GetRequiredService<AuthenticationState>());
builder.Services.AddMudServices();

builder.Services.AddHttpClient(Configuration.HttpClientName, opt => 
{
    opt.BaseAddress = new Uri(Configuration.BackendUrl);
}).AddHttpMessageHandler<CookieHandler>();


await builder.Build().RunAsync();
