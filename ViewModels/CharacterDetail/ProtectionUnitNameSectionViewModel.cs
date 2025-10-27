using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.CharacterDetail;

public sealed partial class ProtectionUnitNameSectionViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [ObservableAsProperty(ReadOnly = false)]
    private IEnumerable<string?> _forceIcons = [];

    [ObservableAsProperty(ReadOnly = false)]
    private IEnumerable<string?> _attributeIcons = [];

    [ObservableAsProperty(ReadOnly = false)]
    private string? _attackTypeIcon;

    public ProtectionUnitNameSectionViewModel(ProtectionUnit unit, JsonDataModelService jsonDataModelService)
    {
        this.WhenActivated(disposable =>
        {
            if (unit.Forces is not null)
            {
                jsonDataModelService
                    .WhenAnyValue(service => service.Forces)
                    .Select(forces => unit.Forces.Select(f => forces?.SingleOrDefault(force => f.Equals(force.Name, StringComparison.OrdinalIgnoreCase))?.Icon))
                    .ToProperty(this, nameof(ForceIcons), out _forceIconsHelper)
                    .DisposeWith(disposable);
            }

            if (unit.Attributes is not null)
            {
                jsonDataModelService
                    .WhenAnyValue(service => service.ProtectionAttributes)
                    .Select(attributes => unit.Attributes.Select(a => attributes?.SingleOrDefault(attribute => a.Equals(attribute.Name, StringComparison.OrdinalIgnoreCase))?.Icon))
                    .ToProperty(this, nameof(AttributeIcons), out _attributeIconsHelper)
                    .DisposeWith(disposable);
            }

            if (unit.AttackType is not null)
            {
                jsonDataModelService
                    .GetObservableProtectionAttackType(unit.AttackType)
                    .Select(attackType => attackType?.Icon)
                    .ToProperty(this, nameof(AttackTypeIcon), out _attackTypeIconHelper)
                    .DisposeWith(disposable);
            }
        });
    }
}