using System.Collections.ObjectModel;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using Blazorise.Components;
using DynamicData;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Components.Characters.Filters;
using SlimeIMWiki.Models;
using SlimeIMWiki.Models.JsonData;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Components.Characters;

public sealed partial class FilterSectionViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    public bool IsOrFilter
    {
        get => _characterListDisplayService.IsOrFilter;
        set => _characterListDisplayService.IsOrFilter = value;
    }

    public ReadOnlyObservableCollection<IAttackType> AttackTypes => _attackTypes;

    public ReadOnlyObservableCollection<IAttribute> Attributes => _attributes;

    public ReadOnlyObservableCollection<TacticType> TacticTypes => _tacticTypes;

    public ReadOnlyObservableCollection<BattleExpertise> BattleExpertises => _battleExpertises;

    public ReadOnlyObservableCollection<IGrouping<Force, string, string>> Forces => _forces;

    public ReadOnlyObservableCollection<IGrouping<SuitedFacility, string, string>> SuitedFacilities => _suitedFacilities;

    public ReadOnlyObservableCollection<Filter> Filters => _filters;

    [Reactive]
    private IEnumerable<SearchResult> _searchResults = [];

    private ReadOnlyObservableCollection<IAttackType> _attackTypes = ReadOnlyObservableCollection<IAttackType>.Empty;
    private ReadOnlyObservableCollection<IAttribute> _attributes = ReadOnlyObservableCollection<IAttribute>.Empty;
    private ReadOnlyObservableCollection<TacticType> _tacticTypes = ReadOnlyObservableCollection<TacticType>.Empty;
    private ReadOnlyObservableCollection<BattleExpertise> _battleExpertises = ReadOnlyObservableCollection<BattleExpertise>.Empty;
    private ReadOnlyObservableCollection<IGrouping<Force, string, string>> _forces = ReadOnlyObservableCollection<IGrouping<Force, string, string>>.Empty;
    private ReadOnlyObservableCollection<IGrouping<SuitedFacility, string, string>> _suitedFacilities = ReadOnlyObservableCollection<IGrouping<SuitedFacility, string, string>>.Empty;
    private ReadOnlyObservableCollection<Filter> _filters = ReadOnlyObservableCollection<Filter>.Empty;

    private readonly CharacterListDisplayService _characterListDisplayService;
    private readonly JsonDataModelService _jsonDataService;

    public FilterSectionViewModel(CharacterListDisplayService characterListDisplayService, JsonDataModelService jsonDataService)
    {
        _characterListDisplayService = characterListDisplayService;
        _jsonDataService = jsonDataService;

        this.WhenActivated(disposable =>
        {
            this.WhenAnyValue(model => model._characterListDisplayService.DisplayCategory)
                .DistinctUntilChanged()
                .Select(category => category switch
                {
                    CharacterListDisplayCategory.Battle => jsonDataService.BattleAttackTypeCache.Connect()
                        .TransformImmutable(static IAttackType (type) => type)
                        .Batch(TimeSpan.FromMilliseconds(1))
                        .Bind(out _attackTypes),
                    CharacterListDisplayCategory.Protection => jsonDataService.ProtectionAttackTypeCache.Connect()
                        .TransformImmutable(static IAttackType (type) => type)
                        .Batch(TimeSpan.FromMilliseconds(1))
                        .Bind(out _attackTypes),
                    var _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
                })
                .Do(_ => this.RaisePropertyChanged(nameof(AttackTypes)))
                .Switch()
                .Subscribe()
                .DisposeWith(disposable);

            this.WhenAnyValue(model => model._characterListDisplayService.DisplayCategory)
                .DistinctUntilChanged()
                .Select(category =>
                {
                    return category switch
                    {
                        CharacterListDisplayCategory.Battle => jsonDataService.BattleAttributeCache.Connect()
                            .TransformImmutable(static IAttribute (attribute) => attribute)
                            .Batch(TimeSpan.FromMilliseconds(1))
                            .Bind(out _attributes),
                        CharacterListDisplayCategory.Protection => jsonDataService.ProtectionAttributeCache.Connect()
                            .TransformImmutable(static IAttribute (attribute) => attribute)
                            .Batch(TimeSpan.FromMilliseconds(1))
                            .Bind(out _attributes),
                        var _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
                    };
                })
                .Do(_ => this.RaisePropertyChanged(nameof(Attributes)))
                .Switch()
                .Subscribe()
                .DisposeWith(disposable);

            jsonDataService.TacticTypeCache.Connect()
                .Bind(out _tacticTypes)
                .Subscribe()
                .DisposeWith(disposable);

            this.WhenAnyValue(model => model._characterListDisplayService.DisplayCategory)
                .DistinctUntilChanged()
                .Select(category =>
                {
                    return category switch
                    {
                        CharacterListDisplayCategory.Battle => jsonDataService.BattleExpertiseCache.Connect()
                            .Bind(out _battleExpertises),
                        var _ => Observable.Empty<IChangeSet<BattleExpertise, string>>()
                            .Bind(out _battleExpertises)
                    };
                })
                .Do(_ => this.RaisePropertyChanged(nameof(BattleExpertises)))
                .Switch()
                .Subscribe()
                .DisposeWith(disposable);

            jsonDataService.ForceCache.Connect()
                .GroupWithImmutableState(static force => force.Category)
                .Batch(TimeSpan.FromMilliseconds(1))
                .Bind(out _forces)
                .Subscribe()
                .DisposeWith(disposable);

            jsonDataService.FieldBuildingCache.Connect()
                .Filter(this.WhenAnyValue(model => model._characterListDisplayService.DisplayCategory)
                    .Select<CharacterListDisplayCategory, Func<FieldBuilding, bool>>(category => building => category switch
                    {
                        CharacterListDisplayCategory.Battle => !building.Category.Equals("Parameter Enhancement Facilities"),
                        CharacterListDisplayCategory.Protection => building.Category.Equals("Parameter Enhancement Facilities"),
                        var _ => true
                    })
                )
                .TransformMany<SuitedFacility, string, FieldBuilding, string>(building =>
                [
                    new SuitedFacility(building, building.Category.Equals("Parameter Enhancement Facilities") ? 200 : 30),
                    new SuitedFacility(building, building.Category.Equals("Parameter Enhancement Facilities") ? 100 : 10)
                ], facility => $"{facility.FieldBuilding.Name}_{facility.Value}")
                .GroupWithImmutableState(static group => group.FieldBuilding.Category)
                .Batch(TimeSpan.FromMilliseconds(1))
                .Bind(out _suitedFacilities)
                .Subscribe()
                .DisposeWith(disposable);

            characterListDisplayService.FilterCache.Connect()
                .Bind(out _filters)
                .Subscribe()
                .DisposeWith(disposable);
        });
    }

    public void AutoCompleteCallback(AutocompleteReadDataEventArgs args)
    {
        if (args.CancellationToken.IsCancellationRequested) return;

        if (_characterListDisplayService.DisplayCategory == CharacterListDisplayCategory.Battle)
        {
            _searchResults = _jsonDataService.BattleUnitsDataCache.Items.GroupBy(data => data.Name)
                .Where(grouping => grouping.Key.StartsWith(args.SearchValue, StringComparison.OrdinalIgnoreCase) ||
                    grouping.Any(data => data.Title.StartsWith(args.SearchValue, StringComparison.OrdinalIgnoreCase)))
                .SelectMany(grouping =>
                {
                    if (grouping.Key.StartsWith(args.SearchValue, StringComparison.OrdinalIgnoreCase))
                    {
                        return
                        [
                            new SearchResult(grouping.Key, SearchResultType.Name, grouping.Key),
                            .. grouping.Where(data => data.Title.StartsWith(args.SearchValue, StringComparison.OrdinalIgnoreCase))
                                .Select(data => new SearchResult($"{data.Name} [{data.Title}]", SearchResultType.Title, data.Title))
                        ];
                    }

                    return grouping.Where(data => data.Title.StartsWith(args.SearchValue, StringComparison.OrdinalIgnoreCase))
                        .Select(data => new SearchResult($"{data.Name} [{data.Title}]", SearchResultType.Title, data.Title));
                })
                .OrderBy(result => result.SearchType)
                .ThenBy(result => result.DisplayValue);
        }
        else
        {
            _searchResults = _jsonDataService.ProtectionUnitsDataCache.Items.GroupBy(data => data.Name)
                .Where(grouping => grouping.Key.StartsWith(args.SearchValue, StringComparison.OrdinalIgnoreCase) ||
                    grouping.Any(data => data.Title.StartsWith(args.SearchValue, StringComparison.OrdinalIgnoreCase)))
                .SelectMany(grouping =>
                {
                    if (grouping.Key.StartsWith(args.SearchValue, StringComparison.OrdinalIgnoreCase))
                    {
                        return
                        [
                            new SearchResult(grouping.Key, SearchResultType.Name, grouping.Key),
                            .. grouping.Where(data => data.Title.StartsWith(args.SearchValue, StringComparison.OrdinalIgnoreCase))
                                .Select(data => new SearchResult($"{data.Name} [{data.Title}]", SearchResultType.Title, data.Title))
                        ];
                    }

                    return grouping.Where(data => data.Title.StartsWith(args.SearchValue, StringComparison.OrdinalIgnoreCase))
                        .Select(data => new SearchResult($"{data.Name} [{data.Title}]", SearchResultType.Title, data.Title));
                })
                .OrderBy(result => result.SearchType)
                .ThenBy(result => result.DisplayValue);
        }
    }

    public void SelectedValueChangedCallback(SearchResult searchResult)
    {
        _characterListDisplayService.FilterCache.AddOrUpdate(new Filter($"Search_{searchResult.DisplayValue}", typeof(NameFilter), searchResult, unit =>
        {
            return unit switch
            {
                BattleUnitData battleUnit => searchResult.SearchType switch
                {
                    SearchResultType.Name => battleUnit.Name.Equals(searchResult.SearchValue, StringComparison.OrdinalIgnoreCase),
                    SearchResultType.Title => battleUnit.Title.Equals(searchResult.SearchValue, StringComparison.OrdinalIgnoreCase),
                    var _ => false
                },
                ProtectionUnitData protectionUnit => searchResult.SearchType switch
                {
                    SearchResultType.Name => protectionUnit.Name.Equals(searchResult.SearchValue, StringComparison.OrdinalIgnoreCase),
                    SearchResultType.Title => protectionUnit.Title.Equals(searchResult.SearchValue, StringComparison.OrdinalIgnoreCase),
                    var _ => false
                },
                var _ => false
            };
        }));
    }
}