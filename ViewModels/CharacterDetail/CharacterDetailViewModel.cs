using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.CharacterDetail;

public sealed partial class CharacterDetailViewModel : ReactiveObject
{
    [ObservableAsProperty]
    private ICharacterUnit? _unit;

    private readonly JsonDataModelService _jsonDataModelService;

    public CharacterDetailViewModel(string permalink, JsonDataModelService jsonDataModelService)
    {
        _jsonDataModelService = jsonDataModelService;

        this.WhenAnyValue(
                model => model._jsonDataModelService.BattleUnits,
                model => model._jsonDataModelService.ProtectionUnits,
                (battleUnits, protectionUnits) =>
                {
                    battleUnits ??= [];
                    protectionUnits ??= [];
                    return battleUnits.Cast<ICharacterUnit>().Concat(protectionUnits);
                })
            .SelectMany(units => units)
            .FirstOrDefaultAsync(unit => unit.Permalink.Equals(permalink, StringComparison.InvariantCultureIgnoreCase))
            .ToProperty(this, nameof(Unit), out _unitHelper);
    }

    public string? GetTacticTypeImage()
    {
        return Unit is null ? null : _jsonDataModelService.GetTacticType(Unit.TacticsType)?.Image;
    }

    public string? GetForceIcon(string force)
    {
        return Unit is null ? null : _jsonDataModelService.GetForce(force)?.Icon;
    }

    public string? GetFieldBuildingIcon(string fieldBuilding)
    {
        return Unit is null ? null : _jsonDataModelService.GetFieldBuilding(fieldBuilding)?.Icon;
    }
}