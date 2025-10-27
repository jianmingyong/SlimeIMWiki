using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.CharacterDetail;

public sealed partial class BattleUnitNameSectionViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [ObservableAsProperty(ReadOnly = false)]
    private string? _attributeIcon;

    [ObservableAsProperty(ReadOnly = false)]
    private string? _attackTypeIcon;

    public BattleUnitNameSectionViewModel(BattleUnit unit, JsonDataModelService jsonDataModelService)
    {
        this.WhenActivated(disposable =>
        {
            jsonDataModelService
                .GetObservableBattleAttribute(unit.Attribute)
                .Select(attribute => attribute?.Icon)
                .ToProperty(this, nameof(AttributeIcon), out _attributeIconHelper)
                .DisposeWith(disposable);
            
            jsonDataModelService
                .GetObservableBattleAttackType(unit.AttackType)
                .Select(attackType => attackType?.Icon)
                .ToProperty(this, nameof(AttackTypeIcon), out _attackTypeIconHelper)
                .DisposeWith(disposable);
        });
    }
}