using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.Characters;

public sealed partial class UnitIconViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
    
    private readonly JsonDataModelService _jsonDataModelService;
    
    [ObservableAsProperty(ReadOnly = false)]
    private string? _firstIcon;

    [ObservableAsProperty(ReadOnly = false)]
    private string? _SecondIcon;

    public UnitIconViewModel(ICharacterUnit unit, JsonDataModelService jsonDataModelService)
    {
        _jsonDataModelService = jsonDataModelService;
        
        this.WhenActivated(disposable =>
        {
            switch (unit)
            {
                case BattleUnit battleUnit:
                {
                    this.WhenAnyValue(model => model._jsonDataModelService.BattleAttributes)
                        .Select(attributes => attributes?.SingleOrDefault(attribute => attribute.Name.Equals(battleUnit.Attribute, StringComparison.OrdinalIgnoreCase))?.Icon)
                        .ToProperty(this, nameof(FirstIcon), out _firstIconHelper)
                        .DisposeWith(disposable);
            
                    this.WhenAnyValue(model => model._jsonDataModelService.BattleAttackTypes)
                        .Select(types => types?.SingleOrDefault(type => type.Name.Equals(battleUnit.AttackType, StringComparison.OrdinalIgnoreCase))?.Icon)
                        .ToProperty(this, nameof(SecondIcon), out _SecondIconHelper)
                        .DisposeWith(disposable);
                    
                    break;
                }

                case ProtectionUnit { Forces.Length: >= 1 } protectionUnit:
                {
                    this.WhenAnyValue(model => model._jsonDataModelService.Forces)
                        .Select(forces => forces?.SingleOrDefault(force => force.Name.Equals(protectionUnit.Forces[0], StringComparison.OrdinalIgnoreCase))?.Icon)
                        .ToProperty(this, nameof(FirstIcon), out _firstIconHelper)
                        .DisposeWith(disposable);

                    if (protectionUnit.Attributes is not null && protectionUnit.Attributes.Length >= 1)
                    {
                        this.WhenAnyValue(model => model._jsonDataModelService.ProtectionAttributes)
                            .Select(attributes => attributes?.SingleOrDefault(attribute => attribute.Name.Equals(protectionUnit.Attributes[0], StringComparison.OrdinalIgnoreCase))?.Icon)
                            .ToProperty(this, nameof(SecondIcon), out _SecondIconHelper)
                            .DisposeWith(disposable);
                    }
                    else if (protectionUnit.AttackType is not null)
                    {
                        this.WhenAnyValue(model => model._jsonDataModelService.ProtectionAttackTypes)
                            .Select(types => types?.SingleOrDefault(type => type.Name.Equals(protectionUnit.AttackType, StringComparison.OrdinalIgnoreCase))?.Icon)
                            .ToProperty(this, nameof(SecondIcon), out _SecondIconHelper)
                            .DisposeWith(disposable);
                    }

                    break;
                }

                case ProtectionUnit { Attributes.Length: >= 1 } protectionUnit:
                {
                    this.WhenAnyValue(model => model._jsonDataModelService.ProtectionAttributes)
                        .Select(forces => forces?.SingleOrDefault(force => force.Name.Equals(protectionUnit.Attributes[0], StringComparison.OrdinalIgnoreCase))?.Icon)
                        .ToProperty(this, nameof(FirstIcon), out _firstIconHelper)
                        .DisposeWith(disposable);

                    if (protectionUnit.Attributes.Length >= 2)
                    {
                        this.WhenAnyValue(model => model._jsonDataModelService.ProtectionAttributes)
                            .Select(forces => forces?.SingleOrDefault(force => force.Name.Equals(protectionUnit.Attributes[1], StringComparison.OrdinalIgnoreCase))?.Icon)
                            .ToProperty(this, nameof(SecondIcon), out _SecondIconHelper)
                            .DisposeWith(disposable);
                    }
                    else if (protectionUnit.AttackType is not null)
                    {
                        this.WhenAnyValue(model => model._jsonDataModelService.ProtectionAttackTypes)
                            .Select(types => types?.SingleOrDefault(type => type.Name.Equals(protectionUnit.AttackType, StringComparison.OrdinalIgnoreCase))?.Icon)
                            .ToProperty(this, nameof(SecondIcon), out _SecondIconHelper)
                            .DisposeWith(disposable);
                    }

                    break;
                }

                case ProtectionUnit { AttackType: not null } protectionUnit:
                {
                    this.WhenAnyValue(model => model._jsonDataModelService.ProtectionAttackTypes)
                        .Select(types => types?.SingleOrDefault(type => type.Name.Equals(protectionUnit.AttackType, StringComparison.OrdinalIgnoreCase))?.Icon)
                        .ToProperty(this, nameof(FirstIcon), out _firstIconHelper)
                        .DisposeWith(disposable);

                    break;
                }
            }
        });
    }
}