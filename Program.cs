using System.Net.Http.Headers;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ReactiveUI;
using SlimeIMWiki;
using SlimeIMWiki.ViewModels.Home;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using Splat.ModeDetection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(_ => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress), 
    DefaultRequestHeaders =
    {
        CacheControl = new CacheControlHeaderValue { NoCache = true }
    }
});
builder.Services.AddHybridCache();
builder.Services.AddSingleton<JsonDataModel>();
builder.Services.UseMicrosoftDependencyResolver();

builder.Services.AddSingleton<LatestNoticesViewModel>();
builder.Services.AddSingleton<TimersViewModel>();

builder.Services
    .AddBlazorise()
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

var resolver = Locator.CurrentMutable;
resolver.InitializeSplat();
resolver.InitializeReactiveUI();

ModeDetector.OverrideModeDetector(Mode.Run);

await builder.Build().RunAsync();