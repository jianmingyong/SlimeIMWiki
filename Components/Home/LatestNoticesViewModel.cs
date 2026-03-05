using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace SlimeIMWiki.Components.Home;

public sealed partial class LatestNoticesViewModel : ReactiveObject
{
    [Reactive]
    private string? _regionSelection;

    [ObservableAsProperty]
    private int _regionCode = 3;

    [ObservableAsProperty]
    private string _regionLink = "api-us";

    public LatestNoticesViewModel()
    {
        var regionSelection = this.WhenAnyValue(model => model.RegionSelection)
            .DistinctUntilChanged()
            .WhereNotNull()
            .Publish()
            .RefCount(2);

        regionSelection
            .Select(value => value switch
            {
                "NA" => 3,
                "EU" => 4,
                "Asia" => 2,
                "Japan" => 1,
                var _ => throw new ArgumentOutOfRangeException(nameof(RegionSelection), value, null)
            }).ToProperty(this, nameof(RegionCode), out _regionCodeHelper);

        regionSelection
            .Select(value => value switch
            {
                "NA" => "api-us",
                "EU" => "api-eu",
                "Asia" => "api-ap",
                "Japan" => "api",
                var _ => throw new ArgumentOutOfRangeException(nameof(RegionSelection), value, null)
            }).ToProperty(this, nameof(RegionLink), out _regionLinkHelper);
    }

    [ReactiveCommand]
    private void RegionChange(string region)
    {
        RegionSelection = region;
    }
}