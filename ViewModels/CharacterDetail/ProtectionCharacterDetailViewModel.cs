using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.CharacterDetail;

public sealed partial class ProtectionCharacterDetailViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [ObservableAsProperty]
    private ProtectionUnit? _protectionUnit;
    
    private readonly JsonDataModelService _jsonDataModelService;

    public ProtectionCharacterDetailViewModel(string permalink, JsonDataModelService jsonDataModelService)
    {
        _jsonDataModelService = jsonDataModelService;
        
        _protectionUnitHelper = this.WhenAnyValue(model => model._jsonDataModelService.ProtectionUnits)
            .SelectMany(units => units)
            .FirstOrDefaultAsync(unit => unit.Permalink.Equals(permalink, StringComparison.InvariantCultureIgnoreCase))
            .ToProperty(this, nameof(ProtectionUnit));
    }

    public string? GetTacticTypeImage()
    {
        return ProtectionUnit is null ? null : _jsonDataModelService.GetTacticType(ProtectionUnit.TacticsType)?.Image;
    }
    
    public string? GetForceIcon(string force)
    {
        return ProtectionUnit is null ? null : _jsonDataModelService.GetForce(force)?.Icon;
    }
}