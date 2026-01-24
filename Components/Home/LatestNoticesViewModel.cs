using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Components.Home;

public sealed partial class LatestNoticesViewModel : ReactiveObject
{
    private readonly IWebStorageService _webStorageService;
    
    [Reactive]
    private string _regionSelection = "NA";

    [ObservableAsProperty]
    private int _regionCode = 3;

    public LatestNoticesViewModel(IWebStorageService webStorageService)
    {
        _webStorageService = webStorageService;
        
        this.WhenAnyValue(model => model.RegionSelection).Select(value => value switch
        {
            "NA" => 3,
            "EU" => 4,
            "Asia" => 2,
            "Japan" => 1,
            var _ => throw new ArgumentOutOfRangeException(nameof(RegionSelection), value, null)
        }).ToProperty(this, nameof(RegionCode), out _regionCodeHelper);
    }

    [ReactiveCommand]
    private void RegionChange(string region)
    {
        RegionSelection = region;
        _webStorageService.SetToCookie(nameof(RegionSelection), region, TimeSpan.FromDays(30));
    }
}