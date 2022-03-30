using Blazored.SessionStorage;
using Blazored.LocalStorage;
using ChatVia.Client;
using ChatVia.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient();
builder.Services.AddHttpClient("chatvia-api", config =>
{
    config.BaseAddress = new Uri("https://localhost:7175/");
});

builder.Services.AddScoped<IFetchService, FetchService>();

builder.Services.AddMudServices();

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();

