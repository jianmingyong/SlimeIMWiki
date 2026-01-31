using System.Reactive.Disposables.Fluent;
using Blazorise.Components;
using DynamicData.Binding;
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

    public IEnumerable<Filter> Filters => _characterListService.Filters;

    public bool IsOrFilter
    {
        get => _characterListService.IsOrFilter;
        set => _characterListService.IsOrFilter = value;
    }

    [Reactive]
    private string _searchQuery = "";

    [Reactive]
    private SearchResult? _searchResultSelection;

    [ObservableAsProperty(ReadOnly = false)]
    private IEnumerable<SearchResult>? _searchResults = [];

    [ObservableAsProperty(ReadOnly = false)]
    private IEnumerable<IAttackType>? _attackTypes = [];

    [ObservableAsProperty(ReadOnly = false)]
    private IEnumerable<IAttribute>? _attributes = [];

    [ObservableAsProperty(ReadOnly = false)]
    private IEnumerable<BattleExpertise>? _expertises = [];

    [ObservableAsProperty(ReadOnly = false)]
    private IEnumerable<Force>? _forces = [];

    [ObservableAsProperty(ReadOnly = false)]
    private IEnumerable<FieldBuilding>? _fieldBuildings = [];

    private readonly CharacterListService _characterListService;
    private readonly JsonDataModelService _jsonDataModelService;

    public FilterSectionViewModel(CharacterListService characterListService, JsonDataModelService jsonDataModelService)
    {
        _characterListService = characterListService;
        _jsonDataModelService = jsonDataModelService;

        this.WhenActivated(disposable =>
        {
            characterListService.Filters.ToObservableChangeSet()
                .Subscribe(_ => this.RaisePropertyChanged(nameof(Filters)))
                .DisposeWith(disposable);

            this.WhenAnyValue(
                    model => model._characterListService.DisplayCategory,
                    model => model._jsonDataModelService.BattleAttackTypes,
                    model => model._jsonDataModelService.ProtectionAttackTypes,
                    (displayCategory, battleAttackType, protectionAttackType) =>
                    {
                        return displayCategory switch
                        {
                            "Battle" => battleAttackType?.Cast<IAttackType>(),
                            "Protection" => protectionAttackType?.Cast<IAttackType>(),
                            var _ => throw new ArgumentOutOfRangeException(nameof(displayCategory), displayCategory, null)
                        };
                    })
                .ToProperty(this, nameof(AttackTypes), out _attackTypesHelper)
                .DisposeWith(disposable);

            this.WhenAnyValue(
                    model => model._characterListService.DisplayCategory,
                    model => model._jsonDataModelService.BattleAttributes,
                    model => model._jsonDataModelService.ProtectionAttributes,
                    (displayCategory, battleAttribute, protectionAttribute) =>
                    {
                        return displayCategory switch
                        {
                            "Battle" => battleAttribute?.Cast<IAttribute>(),
                            "Protection" => protectionAttribute?.Cast<IAttribute>(),
                            var _ => throw new ArgumentOutOfRangeException(nameof(displayCategory), displayCategory, null)
                        };
                    })
                .ToProperty(this, nameof(Attributes), out _attributesHelper)
                .DisposeWith(disposable);

            this.WhenAnyValue(model => model._jsonDataModelService.Forces)
                .ToProperty(this, nameof(Forces), out _forcesHelper)
                .DisposeWith(disposable);
            
            this.WhenAnyValue(
                    model => model._characterListService.DisplayCategory,
                    model => model._jsonDataModelService.BattleExpertise,
                    (displayCategory, battleExpertise) =>
                    {
                        return displayCategory switch
                        {
                            "Battle" => battleExpertise,
                            "Protection" => [],
                            var _ => throw new ArgumentOutOfRangeException(nameof(displayCategory), displayCategory, null)
                        };
                    })
                .ToProperty(this, nameof(BattleExpertise), out _expertisesHelper)
                .DisposeWith(disposable);

            this.WhenAnyValue(
                    model => model._characterListService.DisplayCategory,
                    model => model._jsonDataModelService.FieldBuildings,
                    (displayCategory, fieldBuildings) =>
                    {
                        return displayCategory switch
                        {
                            "Battle" => fieldBuildings?.Where(building => building.Category != "Parameter Enhancement Facilities"),
                            "Protection" => fieldBuildings?.Where(building => building.Category == "Parameter Enhancement Facilities"),
                            var _ => throw new ArgumentOutOfRangeException(nameof(displayCategory), displayCategory, null)
                        };
                    })
                .ToProperty(this, nameof(FieldBuildings), out _fieldBuildingsHelper)
                .DisposeWith(disposable);

            this.WhenAnyValue(
                    model => model._characterListService.DisplayCategory,
                    model => model._jsonDataModelService.BattleUnits,
                    model => model._jsonDataModelService.ProtectionUnits,
                    model => model.SearchQuery, (
                        displayCategory,
                        battleUnits, protectionUnits,
                        searchQuery) =>
                    {
                        switch (displayCategory)
                        {
                            case "Battle":
                            {
                                var nameResults = battleUnits?.Where(unit => unit.Name.StartsWith(searchQuery, StringComparison.OrdinalIgnoreCase))
                                    .Select(unit => new SearchResult(unit.Name, true, false, unit)) ?? [];
                                
                                var titleResults = battleUnits?.Where(unit => unit.Title.StartsWith(searchQuery, StringComparison.OrdinalIgnoreCase))
                                    .Select(unit => new SearchResult($"{unit.Name} [{unit.Title}]", false, true, unit)) ?? [];
                                
                                return nameResults.Concat(titleResults).DistinctBy(result => result.DisplayValue);
                            }

                            case "Protection":
                            {
                                var nameResults = protectionUnits?.Where(unit => unit.Name.StartsWith(searchQuery, StringComparison.OrdinalIgnoreCase))
                                    .Select(unit => new SearchResult(unit.Name, true, false, unit)) ?? [];
                                
                                var titleResults = protectionUnits?.Where(unit => unit.Title.StartsWith(searchQuery, StringComparison.OrdinalIgnoreCase))
                                    .Select(unit => new SearchResult($"{unit.Name} [{unit.Title}]", false, true, unit)) ?? [];
                                
                                return nameResults.Concat(titleResults).DistinctBy(result => result.DisplayValue);
                            }
                            
                            case var _:
                                return [];
                        }
                    })
                .ToProperty(this, nameof(SearchResults), out _searchResultsHelper)
                .DisposeWith(disposable);
            
            this.WhenAnyValue(model => model.SearchResultSelection)
                .Subscribe(searchResult =>
                {
                    if (searchResult is null) return;
                    
                    if (characterListService.Filters.All(filter => filter.Key != $"Search_{searchResult.DisplayValue}"))
                    {
                        Filter? filter = null;
                        filter = new Filter($"Search_{searchResult.DisplayValue}", unit =>
                        {
                            return unit switch
                            {
                                BattleUnit battleUnit => searchResult.IsName ? battleUnit.Name.Equals(searchResult.Unit.Name, StringComparison.OrdinalIgnoreCase) : battleUnit.Title.Equals(searchResult.Unit.Title, StringComparison.OrdinalIgnoreCase),
                                ProtectionUnit protectionUnit => searchResult.IsName ? protectionUnit.Name.Equals(searchResult.Unit.Name, StringComparison.OrdinalIgnoreCase) : protectionUnit.Title.Equals(searchResult.Unit.Title, StringComparison.OrdinalIgnoreCase),
                                var _ => false
                            };
                        }, builder =>
                        {
                            var i = 1;
                            builder.OpenComponent<NameFilter>(i++);
                            builder.AddComponentParameter(i++, nameof(NameFilter.Search), searchResult);
                            builder.AddComponentParameter(i++, nameof(NameFilter.IsRemoveMode), true);
                            builder.AddComponentParameter(i, nameof(NameFilter.Filter), filter);
                            builder.CloseComponent();
                        });
                        
                        characterListService.Filters.Add(filter);
                    }
                }).DisposeWith(disposable);
        });
    }

    public void AutoCompleteCallback(AutocompleteReadDataEventArgs args)
    {
        if (args.CancellationToken.IsCancellationRequested) return;
        SearchQuery = args.SearchValue;
    }
}