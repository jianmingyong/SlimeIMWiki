using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using ReactiveUI.Builder;
using SlimeIMWiki;
using SlimeIMWiki.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

builder.Services.AddSingleton(_ => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddSingleton<IJSInProcessRuntime>(provider => (IJSInProcessRuntime) provider.GetRequiredService<IJSRuntime>());
builder.Services.AddSingleton<IWebStorageService, WebStorageService>();
builder.Services.AddSingleton<StaticWebRootAssets>();
builder.Services.AddSingleton<JsonDataModelService>();
builder.Services.AddSingleton<CharacterListDisplayService>();

builder.Services
    .AddBlazorise(options => { options.ProductToken = GlobalConstants.BlazoriseProductToken; })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

var app = RxAppBuilder.CreateReactiveUIBuilder().WithBlazor().BuildApp();
builder.Services.AddSingleton(app);

await builder.Build().RunAsync();