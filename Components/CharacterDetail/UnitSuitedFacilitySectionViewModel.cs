using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;
using SlimeIMWiki.Models.JsonData;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Components.CharacterDetail;

public sealed partial class UnitSuitedFacilitySectionViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
    
    [ObservableAsProperty(ReadOnly = false)]
    private IEnumerable<SuitedFacility> _suitedFacilities = [];

    public UnitSuitedFacilitySectionViewModel(ICharacterUnit unit, JsonDataModelService jsonDataModelService)
    {
        this.WhenActivated(disposables =>
        {
            switch (unit)
            {
                case BattleUnit battleUnit:
                    jsonDataModelService
                        .GetObservableFieldBuildings(battleUnit.SuitedFacilities)
                        .Select(buildings => buildings.Select((building, index) => new SuitedFacility(building, index == 0 ? 30 : 10)))
                        .ToProperty(this, nameof(SuitedFacilities), out _suitedFacilitiesHelper)
                        .DisposeWith(disposables);
                    break;
                
                case ProtectionUnit protectionUnit:
                    jsonDataModelService
                        .GetObservableFieldBuildings(protectionUnit.SuitedFacilities)
                        .Select(buildings => buildings.Select((building, index) => new SuitedFacility(building, index == 0 ? 200 : 100)))
                        .ToProperty(this, nameof(SuitedFacilities), out _suitedFacilitiesHelper)
                        .DisposeWith(disposables);
                    break;
            }
        });
    }
}