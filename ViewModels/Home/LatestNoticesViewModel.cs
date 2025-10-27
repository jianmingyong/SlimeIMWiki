using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.Home;

public sealed partial class LatestNoticesViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
    
    private readonly IWebStorageService _webStorageService;

    [ObservableAsProperty(ReadOnly = false)]
    private bool _isOnline;
    
    [Reactive(SetModifier = AccessModifier.Private)]
    private string _regionSelection = "NA";

    [ObservableAsProperty]
    private int _regionCode = 3;
    
    public LatestNoticesViewModel(IWebStorageService webStorageService, IWebApplicationService webApplicationService)
    {
        _webStorageService = webStorageService;
        
        this.WhenActivated(disposable =>
        {
            webApplicationService
                .WhenAnyValue(service => service.IsOnline)
                .ToProperty(this, nameof(IsOnline), out _isOnlineHelper)
                .DisposeWith(disposable);
        });
        
        this.WhenAnyValue(model => model.RegionSelection).Select(value => value switch
        {
            "NA" => 3,
            "EU" => 4,
            "Asia" => 2,
            "Japan" => 1,
            var _ => throw new ArgumentOutOfRangeException(nameof(RegionSelection), value, null)
        }).ToProperty(this, nameof(RegionCode), out _regionCodeHelper);
        
        RegionChange(_webStorageService.GetFromCookie(nameof(RegionSelection)) ?? "NA");
    }

    [ReactiveCommand]
    private void RegionChange(string region)
    {
        RegionSelection = region;
        _webStorageService.SetToCookie(nameof(RegionSelection), region, TimeSpan.FromDays(30));
    }
}