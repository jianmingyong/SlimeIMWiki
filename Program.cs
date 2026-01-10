using System.Runtime.InteropServices.JavaScript;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using ReactiveUI;
using SlimeIMWiki;
using SlimeIMWiki.Services;
using SlimeIMWiki.ViewModels.Characters;
using SlimeIMWiki.ViewModels.Home;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using Splat.ModeDetection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

builder.Services.AddSingleton(_ => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});
builder.Services.UseMicrosoftDependencyResolver();

builder.Services.AddSingleton<IJSInProcessRuntime>(provider => (IJSInProcessRuntime) provider.GetRequiredService<IJSRuntime>());
builder.Services.AddSingleton<IWebApplicationService, WebApplicationService>();
builder.Services.AddSingleton<IWebStorageService, WebStorageService>();
builder.Services.AddSingleton<StaticWebRootAssets>();
builder.Services.AddSingleton<JsonDataModelService>();
builder.Services.AddSingleton<CharacterListService>();

builder.Services.AddTransient<LatestNoticesViewModel>();
builder.Services.AddTransient<LiveStreamViewModel>();
builder.Services.AddTransient<TimersViewModel>();

builder.Services.AddTransient<CharacterSectionViewModel>();
builder.Services.AddTransient<FilterSectionViewModel>();

builder.Services
    .AddBlazorise(options =>
    {
        options.ProductToken = GlobalConstants.BlazoriseProductToken;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

var resolver = Locator.CurrentMutable;
resolver.InitializeSplat();
resolver.InitializeReactiveUI();

ModeDetector.OverrideModeDetector(Mode.Run);

var app = builder.Build();

GlobalJsExport.ServiceProvider = app.Services;

await app.RunAsync();