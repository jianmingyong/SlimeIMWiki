using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.Home;

public sealed partial class LatestNoticesViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [Reactive(SetModifier = AccessModifier.Private)]
    private string _regionSelection = "NA";

    [ObservableAsProperty]
    private int _regionCode = 3;

    private readonly IStorageService _storageService;

    public LatestNoticesViewModel(IStorageService storageService)
    {
        _storageService = storageService;

        this.WhenAnyValue(model => model.RegionSelection).Select(value => value switch
        {
            "NA" => 3,
            "EU" => 4,
            "Asia" => 2,
            "Japan" => 1,
            var _ => throw new ArgumentOutOfRangeException(nameof(RegionSelection), value, null)
        }).ToProperty(this, nameof(RegionCode), out _regionCodeHelper);

        RegionChange(_storageService.GetFromCookie(nameof(RegionSelection)) ?? "NA");
    }

    [ReactiveCommand]
    private void RegionChange(string region)
    {
        RegionSelection = region;
        _storageService.SetToCookie(nameof(RegionSelection), region, TimeSpan.FromDays(30));
    }
}