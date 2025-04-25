using System.Net.Http.Json;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace SlimeIMWiki.Models;

public partial class OldJsonDataModel : ReactiveObject
{
    [ObservableAsProperty]
    public partial Force[] Forces { get; }

    [ObservableAsProperty]
    public partial BattleUnit[] BattleUnits { get; }

    [ObservableAsProperty]
    public partial BattleAttackType[] BattleAttackTypes { get; }

    [ObservableAsProperty]
    public partial BattleAttribute[] BattleAttributes { get; }

    [ObservableAsProperty]
    public partial ProtectionUnit[] ProtectionUnits { get; }

    [ObservableAsProperty]
    public partial ProtectionAttackType[] ProtectionAttackTypes { get; }

    [ObservableAsProperty]
    public partial ProtectionAttribute[] ProtectionAttributes { get; }

    [ObservableAsProperty]
    public partial TacticType[] TacticTypes { get; }

    private readonly SourceCache<BattleAttackType, string> _battleAttackTypesSourceCache = new(battleAttackType => battleAttackType.Name);

    public OldJsonDataModel(HttpClient httpClient)
    {
        _forces = [];
        _forcesHelper = Observable.StartAsync(token => httpClient.GetFromJsonAsync("data/forces.json", JsonSerializer.Custom.ForceArray, token)).WhereNotNull().ToProperty(this, nameof(Forces), () => []);

        _battleUnits = [];
        _battleUnitsHelper = Observable.StartAsync(token => httpClient.GetFromJsonAsync("data/battle_units.json", JsonSerializer.Custom.BattleUnitArray, token)).WhereNotNull().ToProperty(this, nameof(BattleUnits), () => []);

        _battleAttackTypes = [];
        _battleAttackTypesHelper = Observable.StartAsync(token => httpClient.GetFromJsonAsync("data/battle_attack_types.json", JsonSerializer.Custom.BattleAttackTypeArray, token)).WhereNotNull().ToProperty(this, nameof(BattleAttackTypes), () => []);

        _battleAttributes = [];
        _battleAttributesHelper = Observable.StartAsync(token => httpClient.GetFromJsonAsync("data/battle_attributes.json", JsonSerializer.Custom.BattleAttributeArray, token)).WhereNotNull().ToProperty(this, nameof(BattleAttributes), () => []);

        _protectionUnits = [];
        _protectionUnitsHelper = Observable.StartAsync(token => httpClient.GetFromJsonAsync("data/protection_units.json", JsonSerializer.Custom.ProtectionUnitArray, token)).WhereNotNull().ToProperty(this, nameof(ProtectionUnits), () => []);

        _protectionAttackTypes = [];
        _protectionAttackTypesHelper = Observable.StartAsync(token => httpClient.GetFromJsonAsync("data/protection_attack_types.json", JsonSerializer.Custom.ProtectionAttackTypeArray, token)).WhereNotNull().ToProperty(this, nameof(ProtectionAttackTypes), () => []);

        _protectionAttributes = [];
        _protectionAttributesHelper = Observable.StartAsync(token => httpClient.GetFromJsonAsync("data/protection_attributes.json", JsonSerializer.Custom.ProtectionAttributeArray, token)).WhereNotNull().ToProperty(this, nameof(ProtectionAttributes), () => []);

        _tacticTypes = [];
        _tacticTypesHelper = Observable.StartAsync(token => httpClient.GetFromJsonAsync("data/tactics_types.json", JsonSerializer.Custom.TacticTypeArray, token)).WhereNotNull().ToProperty(this, nameof(TacticTypes), () => []);
    }

    public static string GetUnitIcon(ICharacterUnit unit)
    {
        return unit switch
        {
            BattleUnit => $"image/battle/characters/{unit.Permalink}/{unit.InitialRarity}/{unit.Permalink}_{unit.InitialRarity}_CharaPartyM.png",
            ProtectionUnit => $"image/protection/characters/{unit.Permalink}/{unit.InitialRarity}/{unit.Permalink}_{unit.InitialRarity}_BlessPartyM.png",
            var _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };
    }

    public static string GetUnitImage(ICharacterUnit unit)
    {
        return unit switch
        {
            BattleUnit => $"image/battle/characters/{unit.Permalink}/{unit.InitialRarity}/{unit.Permalink}_{unit.InitialRarity}_CharaInfo.png",
            ProtectionUnit => $"image/protection/characters/{unit.Permalink}/{unit.InitialRarity}/{unit.Permalink}_{unit.InitialRarity}_BlessInfo.png",
            var _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };
    }

    public ICharacterUnit? GetUnit(string permalink)
    {
        return BattleUnits.Union(ProtectionUnits.Cast<ICharacterUnit>()).FirstOrDefault(u => u.Permalink.Equals(permalink, StringComparison.OrdinalIgnoreCase));
    }

    public Force? GetForce(string force)
    {
        return Forces.FirstOrDefault(f => f.Name.Equals(force, StringComparison.OrdinalIgnoreCase));
    }

    public BattleAttackType? GetBattleAttackType(string attackType)
    {
        return BattleAttackTypes.FirstOrDefault(a => a.Name.Equals(attackType, StringComparison.OrdinalIgnoreCase));
    }

    public BattleAttribute? GetBattleAttribute(string attribute)
    {
        return BattleAttributes.FirstOrDefault(a => a.Name.Equals(attribute, StringComparison.OrdinalIgnoreCase));
    }

    public ProtectionAttackType? GetProtectionAttackType(string attackType)
    {
        return ProtectionAttackTypes.FirstOrDefault(a => a.Name.Equals(attackType, StringComparison.OrdinalIgnoreCase));
    }

    public ProtectionAttribute? GetProtectionAttribute(string attribute)
    {
        return ProtectionAttributes.FirstOrDefault(a => a.Name.Equals(attribute, StringComparison.OrdinalIgnoreCase));
    }

    public TacticType? GetTacticType(string tacticType)
    {
        return TacticTypes.FirstOrDefault(t => t.Name.Equals(tacticType, StringComparison.OrdinalIgnoreCase));
    }
}