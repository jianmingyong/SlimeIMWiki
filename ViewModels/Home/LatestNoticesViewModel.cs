using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.JSInterop;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace SlimeIMWiki.ViewModels.Home;

public partial class LatestNoticesViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    public partial string RegionSelection { get; private set; }

    [ObservableAsProperty]
    public partial int RegionCode { get; }

    private readonly IJSRuntime _jsRuntime;

    public LatestNoticesViewModel(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;

        _regionSelection = "NA";
        _regionCodeHelper = this.WhenAnyValue(model => model.RegionSelection).Select(s => s switch
        {
            "NA" => 3,
            "EU" => 4,
            "Asia" => 2,
            "Japan" => 1,
            var _ => throw new ArgumentOutOfRangeException(nameof(RegionSelection), s, null)
        }).ToProperty(this, model => model.RegionCode);

        this.WhenActivated(disposable => { Observable.StartAsync(WhenActivatedAsync, RxApp.MainThreadScheduler).Subscribe().DisposeWith(disposable); });
    }

    public async Task SetRegion(string region)
    {
        RegionSelection = region;
        await _jsRuntime.InvokeVoidAsync("Cookies.set", nameof(RegionSelection), region, new { expires = 30 });
    }

    private async Task WhenActivatedAsync()
    {
        RegionSelection = await _jsRuntime.InvokeAsync<string?>("Cookies.get", nameof(RegionSelection)) ?? "NA";
        await SetRegion(RegionSelection);
    }
}