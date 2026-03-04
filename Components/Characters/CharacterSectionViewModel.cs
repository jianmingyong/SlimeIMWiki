using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;
using SlimeIMWiki.Models.JsonData;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Components.Characters;

public partial class CharacterSectionViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    public ReadOnlyObservableCollection<ICharacterUnit> CharacterUnits => _characterUnits;

    public CharacterListDisplayCategory DisplayCategory => _characterListDisplayService.DisplayCategory;

    public CharacterListDisplayOrder DisplayOrder => _characterListDisplayService.DisplayOrder;

    public bool IsOrderByDescending => _characterListDisplayService.IsOrderByDescending;

    public float ScrollPosition
    {
        get => _characterListDisplayService.ScrollPosition;
        set => _characterListDisplayService.ScrollPosition = value;
    }

    private ReadOnlyObservableCollection<ICharacterUnit> _characterUnits = ReadOnlyObservableCollection<ICharacterUnit>.Empty;

    private readonly CharacterListDisplayService _characterListDisplayService;

    public CharacterSectionViewModel(CharacterListDisplayService characterListDisplayService, JsonDataModelService jsonDataModelService)
    {
        _characterListDisplayService = characterListDisplayService;

        this.WhenActivated(disposable =>
        {
            jsonDataModelService.BattleAttributeCache.Connect()
                .Bind(out var battleAttributes)
                .Subscribe()
                .DisposeWith(disposable);

            jsonDataModelService.BattleAttackTypeCache.Connect()
                .Bind(out var battleAttackTypes)
                .Subscribe()
                .DisposeWith(disposable);

            jsonDataModelService.BattleExpertiseCache.Connect()
                .Bind(out var battleExpertises)
                .Subscribe()
                .DisposeWith(disposable);

            jsonDataModelService.ProtectionAttributeCache.Connect()
                .Bind(out var protectionAttributes)
                .Subscribe()
                .DisposeWith(disposable);

            jsonDataModelService.ProtectionAttackTypeCache.Connect()
                .Bind(out var protectionAttackTypes)
                .Subscribe()
                .DisposeWith(disposable);

            jsonDataModelService.TacticTypeCache.Connect()
                .Bind(out var tacticTypes)
                .Subscribe()
                .DisposeWith(disposable);

            jsonDataModelService.ForceCache.Connect()
                .Bind(out var forces)
                .Subscribe()
                .DisposeWith(disposable);

            jsonDataModelService.FieldBuildingCache.Connect()
                .Bind(out var fieldBuildings)
                .Subscribe()
                .DisposeWith(disposable);

            characterListDisplayService.FilterCache.Connect()
                .Bind(out var filters)
                .Subscribe()
                .DisposeWith(disposable);

            this.WhenAnyValue(model => model._characterListDisplayService.DisplayCategory)
                .DistinctUntilChanged()
                .Select(category =>
                {
                    // ReSharper disable once InvokeAsExtensionMember
                    var comparer = Observable.CombineLatest(
                        this.WhenAnyValue(model => model.DisplayOrder),
                        this.WhenAnyValue(model => model.IsOrderByDescending),
                        (displayOrder, isOrderByDescending) => (displayOrder, isOrderByDescending)
                    ).Select(tuple =>
                    {
                        return Comparer<ICharacterUnit>.Create((unit, unit2) =>
                        {
                            var output = 0;

                            // Descending order
                            switch (tuple.displayOrder)
                            {
                                case CharacterListDisplayOrder.Release:
                                {
                                    if (unit.ReleaseDate > unit2.ReleaseDate)
                                    {
                                        output = -1;
                                    }
                                    else if (unit.ReleaseDate < unit2.ReleaseDate)
                                    {
                                        output = 1;
                                    }
                                    else if (RaritySelector(unit) > RaritySelector(unit2))
                                    {
                                        output = -1;
                                    }
                                    else if (RaritySelector(unit) < RaritySelector(unit2)) output = 1;

                                    break;
                                }

                                case CharacterListDisplayOrder.Health:
                                {
                                    if (unit.MaxHealth > unit2.MaxHealth)
                                    {
                                        output = -1;
                                    }
                                    else if (unit.MaxHealth < unit2.MaxHealth)
                                    {
                                        output = 1;
                                    }
                                    else if (RaritySelector(unit) > RaritySelector(unit2))
                                    {
                                        output = -1;
                                    }
                                    else if (RaritySelector(unit) < RaritySelector(unit2)) output = 1;

                                    break;
                                }

                                case CharacterListDisplayOrder.Attack:
                                {
                                    if (unit.MaxAttack > unit2.MaxAttack)
                                    {
                                        output = -1;
                                    }
                                    else if (unit.MaxAttack < unit2.MaxAttack)
                                    {
                                        output = 1;
                                    }
                                    else if (RaritySelector(unit) > RaritySelector(unit2))
                                    {
                                        output = -1;
                                    }
                                    else if (RaritySelector(unit) < RaritySelector(unit2)) output = 1;

                                    break;
                                }

                                case CharacterListDisplayOrder.Defense:
                                {
                                    if (unit.MaxDefense > unit2.MaxDefense)
                                    {
                                        output = -1;
                                    }
                                    else if (unit.MaxDefense < unit2.MaxDefense)
                                    {
                                        output = 1;
                                    }
                                    else if (RaritySelector(unit) > RaritySelector(unit2))
                                    {
                                        output = -1;
                                    }
                                    else if (RaritySelector(unit) < RaritySelector(unit2)) output = 1;

                                    break;
                                }

                                case CharacterListDisplayOrder.Rarity:
                                {
                                    if (RaritySelector(unit) > RaritySelector(unit2))
                                    {
                                        output = -1;
                                    }
                                    else if (RaritySelector(unit) < RaritySelector(unit2))
                                    {
                                        output = 1;
                                    }
                                    else if (unit.ReleaseDate > unit2.ReleaseDate)
                                    {
                                        output = -1;
                                    }
                                    else if (unit.ReleaseDate < unit2.ReleaseDate) output = 1;

                                    break;
                                }

                                case CharacterListDisplayOrder.Name:
                                {
                                    var result = unit.Name.CompareTo(unit2.Name, StringComparison.OrdinalIgnoreCase);

                                    if (result != 0)
                                    {
                                        output = result * -1;
                                    }
                                    else if (RaritySelector(unit) > RaritySelector(unit2))
                                    {
                                        output = -1;
                                    }
                                    else if (RaritySelector(unit) < RaritySelector(unit2)) output = 1;

                                    break;
                                }

                                default:
                                    output = 0;
                                    break;
                            }

                            if (!tuple.isOrderByDescending) output *= -1;

                            return output;
                        });

                        int RaritySelector(ICharacterUnit unit)
                        {
                            if (unit.IsAttributeUnbound)
                            {
                                return unit.InitialRarity + 2;
                            }

                            if (unit.IsEx)
                            {
                                return unit.InitialRarity + 1;
                            }

                            return unit.InitialRarity;
                        }
                    });

                    return category switch
                    {
                        CharacterListDisplayCategory.Battle => jsonDataModelService.BattleUnitsDataCache.Connect()
                            .Filter(
                                // ReSharper disable once InvokeAsExtensionMember
                                Observable.CombineLatest(
                                    this.WhenAnyValue(model => model._characterListDisplayService.IsOrFilter),
                                    filters.ObserveCollectionChanges().StartWith(new EventPattern<NotifyCollectionChangedEventArgs>(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))).Select(_ => filters),
                                    (isOrFilter, filter) => (isOrFilter, filter)
                                ).Select<(bool isOrFilter, ReadOnlyObservableCollection<Filter> filter), Func<BattleUnitData, bool>>(tuple =>
                                {
                                    return data =>
                                    {
                                        return tuple.isOrFilter
                                            ? tuple.filter.Count == 0 || tuple.filter.Any(f => f.FilterFunction(data))
                                            : tuple.filter.All(f => f.FilterFunction(data));
                                    };
                                })
                            )
                            // ReSharper disable once InvokeAsExtensionMember
                            .AutoRefreshOnObservable(_ => Observable.Merge(
                                battleAttributes.ObserveCollectionChanges().Select(_ => Unit.Default),
                                battleAttackTypes.ObserveCollectionChanges().Select(_ => Unit.Default),
                                battleExpertises.ObserveCollectionChanges().Select(_ => Unit.Default),
                                tacticTypes.ObserveCollectionChanges().Select(_ => Unit.Default),
                                forces.ObserveCollectionChanges().Select(_ => Unit.Default),
                                fieldBuildings.ObserveCollectionChanges().Select(_ => Unit.Default)
                            ).Throttle(TimeSpan.FromMilliseconds(1)))
                            .Transform(ICharacterUnit (data) => BattleUnit.FromBattleUnitData(data, jsonDataModelService))
                            .Batch(TimeSpan.FromMilliseconds(1))
                            .SortAndBind(out _characterUnits, comparer),

                        CharacterListDisplayCategory.Protection => jsonDataModelService.ProtectionUnitsDataCache.Connect()
                            .Filter(
                                // ReSharper disable once InvokeAsExtensionMember
                                Observable.CombineLatest(
                                    this.WhenAnyValue(model => model._characterListDisplayService.IsOrFilter),
                                    filters.ObserveCollectionChanges().StartWith(new EventPattern<NotifyCollectionChangedEventArgs>(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))).Select(_ => filters),
                                    (isOrFilter, filter) => (isOrFilter, filter)
                                ).Select<(bool isOrFilter, ReadOnlyObservableCollection<Filter> filter), Func<ProtectionUnitData, bool>>(tuple =>
                                {
                                    return data =>
                                    {
                                        return tuple.isOrFilter
                                            ? tuple.filter.Count == 0 || tuple.filter.Any(f => f.FilterFunction(data))
                                            : tuple.filter.All(f => f.FilterFunction(data));
                                    };
                                })
                            )
                            // ReSharper disable once InvokeAsExtensionMember
                            .AutoRefreshOnObservable(_ => Observable.Merge(
                                protectionAttributes.ObserveCollectionChanges().Select(_ => Unit.Default),
                                protectionAttackTypes.ObserveCollectionChanges().Select(_ => Unit.Default),
                                tacticTypes.ObserveCollectionChanges().Select(_ => Unit.Default),
                                forces.ObserveCollectionChanges().Select(_ => Unit.Default),
                                fieldBuildings.ObserveCollectionChanges().Select(_ => Unit.Default)
                            ).Throttle(TimeSpan.FromMilliseconds(1)))
                            .Transform(ICharacterUnit (data) => ProtectionUnit.FromProtectionUnitData(data, jsonDataModelService))
                            .Batch(TimeSpan.FromMilliseconds(1))
                            .SortAndBind(out _characterUnits, comparer),

                        var _ => Observable.Empty<IChangeSet<ICharacterUnit, string>>().Bind(out _characterUnits)
                    };
                })
                .Do(_ => this.RaisePropertyChanged(nameof(CharacterUnits)))
                .Switch()
                .Subscribe()
                .DisposeWith(disposable);

            this.WhenAnyValue(model => model._characterListDisplayService.DisplayCategory)
                .DistinctUntilChanged()
                .Subscribe(_ => this.RaisePropertyChanged(nameof(DisplayCategory)))
                .DisposeWith(disposable);

            this.WhenAnyValue(model => model._characterListDisplayService.DisplayOrder)
                .DistinctUntilChanged()
                .Subscribe(_ => this.RaisePropertyChanged(nameof(DisplayOrder)))
                .DisposeWith(disposable);

            this.WhenAnyValue(model => model._characterListDisplayService.IsOrderByDescending)
                .DistinctUntilChanged()
                .Subscribe(_ => this.RaisePropertyChanged(nameof(IsOrderByDescending)))
                .DisposeWith(disposable);

            this.WhenAnyValue(model => model._characterListDisplayService.ScrollPosition)
                .DistinctUntilChanged()
                .Subscribe(_ => this.RaisePropertyChanged(nameof(ScrollPosition)))
                .DisposeWith(disposable);
        });
    }

    public string DisplayText(ICharacterUnit unit)
    {
        return DisplayOrder switch
        {
            CharacterListDisplayOrder.Health => unit.MaxHealth.ToString("N0"),
            CharacterListDisplayOrder.Attack => unit.MaxAttack.ToString("N0"),
            CharacterListDisplayOrder.Defense => unit.MaxDefense.ToString("N0"),
            var _ => unit.Name
        };
    }

    [ReactiveCommand]
    private void ChangeDisplayCategory(CharacterListDisplayCategory category)
    {
        _characterListDisplayService.DisplayCategory = category;
        _characterListDisplayService.FilterCache.Clear();
        _characterListDisplayService.ScrollPosition = 0;
    }

    [ReactiveCommand]
    private void ChangeDisplayOrder(CharacterListDisplayOrder order)
    {
        _characterListDisplayService.DisplayOrder = order;
    }

    [ReactiveCommand]
    private void ToggleDisplayOrder()
    {
        _characterListDisplayService.IsOrderByDescending = !_characterListDisplayService.IsOrderByDescending;
    }
}