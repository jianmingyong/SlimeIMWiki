using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.Home;

public sealed partial class LatestNoticesViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
    
    private readonly IStorageService _storageService;
    private readonly WebApplicationService _webApplicationService;

    [ObservableAsProperty(ReadOnly = false)]
    private bool _isOnline;
    
    [Reactive(SetModifier = AccessModifier.Private)]
    private string _regionSelection = "NA";

    [ObservableAsProperty]
    private int _regionCode = 3;
    
    public LatestNoticesViewModel(IStorageService storageService, WebApplicationService webApplicationService)
    {
        _storageService = storageService;
        _webApplicationService = webApplicationService;

        this.WhenAnyValue(model => model.RegionSelection).Select(value => value switch
        {
            "NA" => 3,
            "EU" => 4,
            "Asia" => 2,
            "Japan" => 1,
            var _ => throw new ArgumentOutOfRangeException(nameof(RegionSelection), value, null)
        }).ToProperty(this, nameof(RegionCode), out _regionCodeHelper);
        
        this.WhenActivated(disposable =>
        {
            this.WhenAnyValue(model => model._webApplicationService.IsOnline)
                .ToProperty(this, nameof(IsOnline), out _isOnlineHelper)
                .DisposeWith(disposable);
        });

        RegionChange(_storageService.GetFromCookie(nameof(RegionSelection)) ?? "NA");
    }

    [ReactiveCommand]
    private void RegionChange(string region)
    {
        RegionSelection = region;
        _storageService.SetToCookie(nameof(RegionSelection), region, TimeSpan.FromDays(30));
    }
}