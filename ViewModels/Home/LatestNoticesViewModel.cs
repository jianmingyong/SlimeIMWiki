using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.Home;

public sealed partial class LatestNoticesViewModel : ReactiveObject, IActivatableViewModel
{
    
    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    public partial string RegionSelection { get; private set; }

    [ObservableAsProperty]
    public partial int RegionCode { get; }

    private readonly IStorageService _storageService;

    public LatestNoticesViewModel(IStorageService storageService)
    {
        _storageService = storageService;

        _regionSelection = "NA";
        _regionCodeHelper = this.WhenAnyValue(model => model.RegionSelection).Select(s => s switch
        {
            "NA" => 3,
            "EU" => 4,
            "Asia" => 2,
            "Japan" => 1,
            var _ => throw new ArgumentOutOfRangeException(nameof(RegionSelection), s, null)
        }).ToProperty(this, nameof(RegionCode));

        this.WhenActivated(disposable => { Observable.FromAsync(WhenActivatedAsync, RxApp.MainThreadScheduler).Subscribe().DisposeWith(disposable); });
    }
    
    private async Task WhenActivatedAsync()
    {
        RegionSelection = await _storageService.GetFromCookieAsync(nameof(RegionSelection)) ?? "NA";
        await RegionChange(RegionSelection);
    }
    
    [ReactiveCommand]
    private async Task RegionChange(string region)
    {
        RegionSelection = region;
        await _storageService.SetToCookieAsync(nameof(RegionSelection), region, TimeSpan.FromDays(30));
    }
}