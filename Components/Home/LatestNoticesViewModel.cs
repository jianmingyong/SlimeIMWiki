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

    public LatestNoticesViewModel()
    {
        this.WhenAnyValue(model => model.RegionSelection)
            .DistinctUntilChanged()
            .WhereNotNull()
            .Select(value => value switch
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
    }
}