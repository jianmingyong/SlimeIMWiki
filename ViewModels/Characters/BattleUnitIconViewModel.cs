using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.Characters;

public sealed partial class BattleUnitIconViewModel : ReactiveObject
{
    [ObservableAsProperty]
    private string? _attributeIcon;

    [ObservableAsProperty]
    private string? _attackTypeIcon;
    
    private readonly JsonDataModelService _jsonDataModelService;

    public BattleUnitIconViewModel(BattleUnit unit, JsonDataModelService jsonDataModelService)
    {
        _jsonDataModelService = jsonDataModelService;
        
        _attributeIconHelper = this.WhenAnyValue(model => model._jsonDataModelService.BattleAttributes)
            .Select(_ => jsonDataModelService.GetBattleAttribute(unit.Attribute)?.Icon)
            .ToProperty(this, nameof(AttributeIcon));

        _attackTypeIconHelper = this.WhenAnyValue(model => model._jsonDataModelService.BattleAttackTypes)
            .Select(_ => jsonDataModelService.GetBattleAttackType(unit.AttackType)?.Icon)
            .ToProperty(this, nameof(AttackTypeIcon));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(AttributeIcon, AttackTypeIcon);
    }
}