using System.Reactive.Disposables.Fluent;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models.JsonData;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Components.CharacterDetail;

public sealed partial class UnitForceSectionViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [ObservableAsProperty(ReadOnly = false)]
    private IEnumerable<Force?> _forces = [];
    
    public UnitForceSectionViewModel(ICharacterUnit unit, JsonDataModelService jsonDataModelService)
    {
        this.WhenActivated(disposables =>
        {
            if (unit is BattleUnit battleUnit)
            {
                jsonDataModelService
                    .GetObservableForces(battleUnit.Forces)
                    .ToProperty(this, nameof(Forces), out _forcesHelper)
                    .DisposeWith(disposables);
            }
            else if (unit is ProtectionUnit protectionUnit)
            {
                jsonDataModelService
                    .GetObservableForces(protectionUnit.Forces ?? [])
                    .ToProperty(this, nameof(Forces), out _forcesHelper)
                    .DisposeWith(disposables);
            }
        });
    }
}