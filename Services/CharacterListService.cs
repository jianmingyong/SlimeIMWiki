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
    [ObservableAsProperty]
    private IEnumerable<ICharacterUnit> _characterUnits = [];
    
    [Reactive]
    public partial string OrderByCategory { get; set; }
    
    [Reactive]
    public partial bool IsOrderByDescending { get; set; }

    public ObservableCollection<Filter> Filters { get; } = [];
    
    [Reactive]
    public partial bool IsOrFilter { get; set; }
    
    private readonly JsonDataModelService _jsonDataModelService;
    
    public CharacterListService(JsonDataModelService jsonDataModelService)
    {
        _jsonDataModelService = jsonDataModelService;
        
        _orderByCategory = "Release";
        _isOrderByDescending = true;
        _isOrFilter = true;

        _characterUnitsHelper = Observable.CombineLatest(
            this.WhenAnyValue(service => service._jsonDataModelService.BattleUnits, service => service._jsonDataModelService.ProtectionUnits),
            this.WhenAnyValue(service => service.OrderByCategory, service => service.IsOrderByDescending, service => service.IsOrFilter),
            Filters.ToObservableChangeSet(),
            (units, _, _) => ApplySort(ApplyFilter(units.Item1.Cast<ICharacterUnit>().Concat(units.Item2))))
            .ToProperty(this, nameof(CharacterUnits));
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
                "Rarity" => source.OrderByDescending(RaritySelector),
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
            "Rarity" => source.OrderBy(RaritySelector),
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