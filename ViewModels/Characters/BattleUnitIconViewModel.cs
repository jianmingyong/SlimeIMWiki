using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.Characters;

public sealed partial class BattleUnitIconViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    private readonly JsonDataModelService _jsonDataModelService;

    [ObservableAsProperty(ReadOnly = false)]
    private string? _attributeIcon;

    [ObservableAsProperty(ReadOnly = false)]
    private string? _attackTypeIcon;

    public BattleUnitIconViewModel(BattleUnit unit, JsonDataModelService jsonDataModelService)
    {
        _jsonDataModelService = jsonDataModelService;

        this.WhenActivated(disposable =>
        {
            this.WhenAnyValue(model => model._jsonDataModelService.BattleAttributes)
                .Select(attributes => attributes.SingleOrDefault(attribute => attribute.Name.Equals(unit.Attribute, StringComparison.OrdinalIgnoreCase))?.Icon)
                .ToProperty(this, nameof(AttributeIcon), out _attributeIconHelper)
                .DisposeWith(disposable);
            
            this.WhenAnyValue(model => model._jsonDataModelService.BattleAttackTypes)
                .Select(types => types.SingleOrDefault(type => type.Name.Equals(unit.AttackType, StringComparison.OrdinalIgnoreCase))?.Icon)
                .ToProperty(this, nameof(AttackTypeIcon), out _attackTypeIconHelper)
                .DisposeWith(disposable);
        });
    }
}