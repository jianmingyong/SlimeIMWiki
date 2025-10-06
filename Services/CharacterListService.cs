using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData.Binding;
using Microsoft.AspNetCore.Components;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;

namespace SlimeIMWiki.Services;

public record Filter(Func<ICharacterUnit, bool> FilterFunction, RenderFragment RemoveRenderFragment);

public sealed partial class CharacterListService : ReactiveObject
{
    public ObservableCollection<Filter> Filters { get; } = [];

    [ObservableAsProperty]
    private IEnumerable<ICharacterUnit> _characterUnits = [];

    [ObservableAsProperty]
    private int _characterUnitsCount;

    [Reactive]
    private string _displayCategory = "Battle";

    [Reactive]
    private string _orderByCategory = "Release";

    [Reactive]
    private bool _isOrderByDescending = true;

    [Reactive]
    private bool _isOrFilter = true;

    private readonly JsonDataModelService _jsonDataModelService;

    public CharacterListService(JsonDataModelService jsonDataModelService)
    {
        _jsonDataModelService = jsonDataModelService;

        this.WhenAnyValue(service => service._jsonDataModelService.BattleUnits, service => service._jsonDataModelService.ProtectionUnits)
            .CombineLatest(
                this.WhenAnyValue(service => service.DisplayCategory),
                this.WhenAnyValue(service => service.OrderByCategory, service => service.IsOrderByDescending, service => service.IsOrFilter),
                Filters.ToObservableChangeSet(),
                (units, displayCategory, _, _) => ApplySort(ApplyFilter<IEnumerable<ICharacterUnit>>(displayCategory switch
                {
                    "Battle" => units.Item1,
                    "Protection" => units.Item2,
                    var _ => throw new ArgumentOutOfRangeException(nameof(displayCategory), displayCategory, null)
                })))
            .ToProperty(this, nameof(CharacterUnits), out _characterUnitsHelper);

        this.WhenAnyValue(service => service.CharacterUnits)
            .Select(units => units.Count())
            .ToProperty(this, nameof(CharacterUnitsCount), out _characterUnitsCountHelper);
    }

    private IEnumerable<ICharacterUnit> ApplySort<TSource>(TSource source) where TSource : IEnumerable<ICharacterUnit>
    {
        if (IsOrderByDescending)
        {
            return OrderByCategory switch
            {
                "Release" => source.OrderByDescending(unit => unit.ReleaseDate).ThenByDescending(RaritySelector),
                "Health" => source.OrderByDescending(unit => unit.MaxHealth).ThenByDescending(RaritySelector),
                "Attack" => source.OrderByDescending(unit => unit.MaxAttack).ThenByDescending(RaritySelector),
                "Defense" => source.OrderByDescending(unit => unit.MaxDefense).ThenByDescending(RaritySelector),
                "Rarity" => source.OrderByDescending(RaritySelector).ThenByDescending(unit => unit.ReleaseDate),
                "Name" => source.OrderByDescending(unit => unit.Name).ThenByDescending(RaritySelector),
                var _ => source.OrderByDescending(unit => unit.ReleaseDate).ThenByDescending(RaritySelector)
            };
        }

        return OrderByCategory switch
        {
            "Release" => source.OrderBy(unit => unit.ReleaseDate).ThenBy(RaritySelector),
            "Health" => source.OrderBy(unit => unit.MaxHealth).ThenBy(RaritySelector),
            "Attack" => source.OrderBy(unit => unit.MaxAttack).ThenBy(RaritySelector),
            "Defense" => source.OrderBy(unit => unit.MaxDefense).ThenBy(RaritySelector),
            "Rarity" => source.OrderBy(RaritySelector).ThenBy(unit => unit.ReleaseDate),
            "Name" => source.OrderBy(unit => unit.Name).ThenBy(RaritySelector),
            var _ => source.OrderBy(unit => unit.ReleaseDate).ThenBy(RaritySelector)
        };

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
    }

    private IEnumerable<ICharacterUnit> ApplyFilter<TSource>(TSource source) where TSource : IEnumerable<ICharacterUnit>
    {
        if (Filters.Count == 0) return source;
        return IsOrFilter ? source.Where(unit => Filters.Any(filter => filter.FilterFunction(unit))) : source.Where(unit => Filters.All(filter => filter.FilterFunction(unit)));
    }

    [ReactiveCommand]
    private void ChangeCharacterUnitsDisplayCategory(string category)
    {
        DisplayCategory = category;
        Filters.Clear();
    }

    [ReactiveCommand]
    private void ChangeCharacterUnitsOrderByCategory(string category)
    {
        OrderByCategory = category;
    }

    [ReactiveCommand]
    private void ChangeCharacterUnitsOrder()
    {
        IsOrderByDescending = !IsOrderByDescending;
    }
}