using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models.JsonData;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Components.Characters;

public sealed partial class UnitIconViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
    
    [ObservableAsProperty(ReadOnly = false)]
    private string? _firstIcon;

    [ObservableAsProperty(ReadOnly = false)]
    private string? _SecondIcon;

    public UnitIconViewModel(ICharacterUnit unit, JsonDataModelService jsonDataModelService)
    {
        this.WhenActivated(disposable =>
        {
            switch (unit)
            {
                case BattleUnit battleUnit:
                {
                    jsonDataModelService
                        .GetObservableBattleAttribute(battleUnit.Attribute)
                        .Select(attribute => attribute?.Icon)
                        .ToProperty(this, nameof(FirstIcon), out _firstIconHelper)
                        .DisposeWith(disposable);

                    jsonDataModelService
                        .GetObservableBattleAttackType(battleUnit.AttackType)
                        .Select(type => type?.Icon)
                        .ToProperty(this, nameof(SecondIcon), out _SecondIconHelper)
                        .DisposeWith(disposable);

                    break;
                }

                case ProtectionUnit { Forces.Length: >= 1 } protectionUnit:
                {
                    jsonDataModelService
                        .GetObservableForce(protectionUnit.Forces[0])
                        .Select(force => force?.Icon)
                        .ToProperty(this, nameof(FirstIcon), out _firstIconHelper)
                        .DisposeWith(disposable);

                    if (protectionUnit.Attributes is not null && protectionUnit.Attributes.Length >= 1)
                    {
                        jsonDataModelService
                            .GetObservableProtectionAttribute(protectionUnit.Attributes[0])
                            .Select(attribute => attribute?.Icon)
                            .ToProperty(this, nameof(SecondIcon), out _SecondIconHelper)
                            .DisposeWith(disposable);
                    }
                    else if (protectionUnit.AttackType is not null)
                    {
                        jsonDataModelService
                            .GetObservableProtectionAttackType(protectionUnit.AttackType)
                            .Select(type => type?.Icon)
                            .ToProperty(this, nameof(SecondIcon), out _SecondIconHelper)
                            .DisposeWith(disposable);
                    }

                    break;
                }

                case ProtectionUnit { Attributes.Length: >= 1 } protectionUnit:
                {
                    jsonDataModelService
                        .GetObservableProtectionAttribute(protectionUnit.Attributes[0])
                        .Select(attribute => attribute?.Icon)
                        .ToProperty(this, nameof(FirstIcon), out _firstIconHelper)
                        .DisposeWith(disposable);

                    if (protectionUnit.Attributes.Length >= 2)
                    {
                        jsonDataModelService
                            .GetObservableProtectionAttribute(protectionUnit.Attributes[1])
                            .Select(attribute => attribute?.Icon)
                            .ToProperty(this, nameof(SecondIcon), out _SecondIconHelper)
                            .DisposeWith(disposable);
                    }
                    else if (protectionUnit.AttackType is not null)
                    {
                        jsonDataModelService
                            .GetObservableProtectionAttackType(protectionUnit.AttackType)
                            .Select(type => type?.Icon)
                            .ToProperty(this, nameof(SecondIcon), out _SecondIconHelper)
                            .DisposeWith(disposable);
                    }

                    break;
                }

                case ProtectionUnit { AttackType: not null } protectionUnit:
                {
                    jsonDataModelService
                        .GetObservableProtectionAttackType(protectionUnit.AttackType)
                        .Select(type => type?.Icon)
                        .ToProperty(this, nameof(FirstIcon), out _firstIconHelper)
                        .DisposeWith(disposable);

                    break;
                }
            }
        });
    }
}