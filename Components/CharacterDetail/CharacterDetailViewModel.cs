using System.Reactive.Disposables.Fluent;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Components.CharacterDetail;

public sealed partial class CharacterDetailViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
    
    [Reactive]
    private ICharacterUnit? _unit;

    [Reactive]
    private bool _loading = true;

    public CharacterDetailViewModel(string permalink, JsonDataModelService jsonDataModelService)
    {
        this.WhenActivated(disposable =>
        {
            jsonDataModelService.RefreshData().Subscribe(_ =>
            {
                var battleUnit = jsonDataModelService.BattleUnitsDataCache.Lookup(permalink);

                if (battleUnit.HasValue)
                {
                    Unit = BattleUnit.FromBattleUnitData(battleUnit.Value, jsonDataModelService);
                    return;
                }

                var protectionUnit = jsonDataModelService.ProtectionUnitsDataCache.Lookup(permalink);

                if (protectionUnit.HasValue)
                {
                    Unit = ProtectionUnit.FromProtectionUnitData(protectionUnit.Value, jsonDataModelService);
                }
            }, () =>
            {
                var battleUnit = jsonDataModelService.BattleUnitsDataCache.Lookup(permalink);

                if (battleUnit.HasValue)
                {
                    Unit = BattleUnit.FromBattleUnitData(battleUnit.Value, jsonDataModelService);
                    return;
                }

                var protectionUnit = jsonDataModelService.ProtectionUnitsDataCache.Lookup(permalink);

                if (protectionUnit.HasValue)
                {
                    Unit = ProtectionUnit.FromProtectionUnitData(protectionUnit.Value, jsonDataModelService);
                }

                Loading = false;
            }).DisposeWith(disposable);
        });
    }
}