using System.Reactive.Disposables;
using ReactiveUI;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.Characters;

public sealed class BattleUnitIconViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
    
    private readonly JsonDataModelService _jsonDataModelService;

    public BattleUnitIconViewModel(JsonDataModelService jsonDataModelService)
    {
        _jsonDataModelService = jsonDataModelService;
        
        this.WhenActivated(disposable =>
        {
            this.WhenAnyValue(
                    model => model._jsonDataModelService.BattleAttributes,
                    model => model._jsonDataModelService.BattleAttackTypes)
                .Subscribe(_ => this.RaisePropertyChanged())
                .DisposeWith(disposable);
        });
    }

    public string? GetAttributeIcon(BattleUnit battleUnit)
    {
        return _jsonDataModelService.GetBattleAttribute(battleUnit.Attribute)?.Icon;
    }
    
    public string? GetAttackTypeIcon(BattleUnit battleUnit)
    {
        return _jsonDataModelService.GetBattleAttackType(battleUnit.AttackType)?.Icon;
    }
}