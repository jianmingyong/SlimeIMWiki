using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.CharacterDetail;

public sealed partial class BattleUnitNameViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [ObservableAsProperty(ReadOnly = false)]
    private string? _attributeIcon;

    [ObservableAsProperty(ReadOnly = false)]
    private string? _attackTypeIcon;

    private readonly JsonDataModelService _jsonDataModelService;

    public BattleUnitNameViewModel(BattleUnit unit, JsonDataModelService jsonDataModelService)
    {
        _jsonDataModelService = jsonDataModelService;

        this.WhenActivated(disposable =>
        {
            this.WhenAnyValue(model => model._jsonDataModelService.BattleAttributes)
                .Select(attributes => attributes.SingleOrDefault(attribute => attribute.Name == unit.Attribute)?.Icon)
                .ToProperty(this, nameof(AttributeIcon), out _attributeIconHelper)
                .DisposeWith(disposable);

            this.WhenAnyValue(model => model._jsonDataModelService.BattleAttackTypes)
                .Select(attackTypes => attackTypes.SingleOrDefault(attackType => attackType.Name == unit.AttackType)?.Icon)
                .ToProperty(this, nameof(AttackTypeIcon), out _attackTypeIconHelper)
                .DisposeWith(disposable);
        });
    }
}