using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Components.CharacterDetail;

public sealed partial class CharacterDetailViewModel : ReactiveObject
{
    [Reactive]
    private ICharacterUnit? _unit;

    public CharacterDetailViewModel(string permalink, JsonDataModelService jsonDataModelService)
    {
        var battleUnit = jsonDataModelService.BattleUnitsDataCache.Lookup(permalink);

        if (battleUnit.HasValue)
        {
            _unit = BattleUnit.FromBattleUnitData(battleUnit.Value, jsonDataModelService);
            return;
        }

        var protectionUnit = jsonDataModelService.ProtectionUnitsDataCache.Lookup(permalink);

        if (protectionUnit.HasValue)
        {
            _unit = ProtectionUnit.FromProtectionUnitData(protectionUnit.Value, jsonDataModelService);
        }
    }
}