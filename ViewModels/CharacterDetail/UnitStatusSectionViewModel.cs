using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.CharacterDetail;

public sealed partial class UnitStatusSectionViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
    
    [ObservableAsProperty(ReadOnly = false)]
    private string? _tacticTypeIcon;

    public UnitStatusSectionViewModel(ICharacterUnit unit, JsonDataModelService service)
    {
        this.WhenActivated(disposable =>
        {
            service
                .GetObservableTacticType(unit.TacticsType)
                .Select(type => type?.Image)
                .ToProperty(this, nameof(TacticTypeIcon), out _tacticTypeIconHelper)
                .DisposeWith(disposable);
        });
    }
}