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

            jsonDataModelService.RefreshData().Subscribe(_ => { }, () =>
            {
                var battleUnit2 = jsonDataModelService.BattleUnitsDataCache.Lookup(permalink);

                if (battleUnit2.HasValue)
                {
                    Unit = BattleUnit.FromBattleUnitData(battleUnit2.Value, jsonDataModelService);
                    return;
                }

                var protectionUnit2 = jsonDataModelService.ProtectionUnitsDataCache.Lookup(permalink);

                if (protectionUnit2.HasValue)
                {
                    Unit = ProtectionUnit.FromProtectionUnitData(protectionUnit2.Value, jsonDataModelService);
                }

                Loading = false;
            }).DisposeWith(disposable);
        });
    }
}