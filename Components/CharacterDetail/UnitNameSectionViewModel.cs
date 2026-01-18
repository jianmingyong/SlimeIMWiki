using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models.JsonData;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Components.CharacterDetail;

public sealed partial class UnitNameSectionViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
    
    [ObservableAsProperty(ReadOnly = false)]
    private IEnumerable<IAttribute?> _attributes = [];

    [ObservableAsProperty(ReadOnly = false)]
    private IAttackType? _attackType;
    
    private readonly JsonDataModelService _jsonDataModelService;

    public UnitNameSectionViewModel(ICharacterUnit unit, JsonDataModelService jsonDataModelService)
    {
        _jsonDataModelService = jsonDataModelService;
        
        this.WhenActivated(disposable =>
        {
            switch (unit)
            {
                case BattleUnit battleUnit:
                {
                    this.WhenAnyValue(model => model._jsonDataModelService.BattleAttributes)
                        .Select(attributes => attributes?.Where(attribute => attribute.Name.Equals(battleUnit.Attribute, StringComparison.OrdinalIgnoreCase)).Cast<IAttribute>() ?? [])
                        .ToProperty(this, nameof(Attributes), out _attributesHelper)
                        .DisposeWith(disposable);

                    jsonDataModelService
                        .GetObservableBattleAttackType(battleUnit.AttackType)
                        .Select(type => type as IAttackType)
                        .ToProperty(this, nameof(AttackType), out _attackTypeHelper)
                        .DisposeWith(disposable);
                    
                    break;
                }

                case ProtectionUnit protectionUnit:
                {
                    if (protectionUnit.Attributes is not null)
                    {
                        jsonDataModelService
                            .WhenAnyValue(service => service.ProtectionAttributes)
                            .Select(attributes => protectionUnit.Attributes.Select(a => attributes?.SingleOrDefault(attribute => a.Equals(attribute.Name, StringComparison.OrdinalIgnoreCase)) as IAttribute))
                            .ToProperty(this, nameof(Attributes), out _attributesHelper)
                            .DisposeWith(disposable);
                    }
                
                    if (protectionUnit.AttackType is not null)
                    {
                        jsonDataModelService
                            .GetObservableProtectionAttackType(protectionUnit.AttackType)
                            .ToProperty(this, nameof(AttackType), out _attackTypeHelper)
                            .DisposeWith(disposable);
                    }

                    break;
                }
            }
        });
    }
}