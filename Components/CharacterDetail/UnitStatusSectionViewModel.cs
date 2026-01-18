using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models.JsonData;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Components.CharacterDetail;

public sealed partial class UnitStatusSectionViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
    
    [ObservableAsProperty(ReadOnly = false)]
    private string? _tacticTypeIcon;

    [ObservableAsProperty(ReadOnly = false)]
    private string? _expertiseIcon;

    public UnitStatusSectionViewModel(ICharacterUnit unit, JsonDataModelService service)
    {
        this.WhenActivated(disposable =>
        {
            service
                .GetObservableTacticType(unit.TacticsType)
                .Select(type => type?.Image)
                .ToProperty(this, nameof(TacticTypeIcon), out _tacticTypeIconHelper)
                .DisposeWith(disposable);

            if (unit is BattleUnit battleUnit)
            {
                service
                    .GetObservableBattleExpertise(battleUnit.Expertise)
                    .Select(expertise => expertise?.Icon)
                    .ToProperty(this, nameof(ExpertiseIcon), out _expertiseIconHelper)
                    .DisposeWith(disposable);
            }
        });
    }
}