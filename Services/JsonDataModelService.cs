using System.Net.Http.Json;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;

namespace SlimeIMWiki.Services;

public sealed partial class JsonDataModelService : ReactiveObject
{
    [ObservableAsProperty]
    private BattleUnit[] _battleUnits = [];

    [ObservableAsProperty]
    private BattleAttackType[] _battleAttackTypes = [];

    [ObservableAsProperty]
    private BattleAttribute[] _battleAttributes = [];

    [ObservableAsProperty]
    private  ProtectionUnit[] _protectionUnits = [];

    [ObservableAsProperty]
    private ProtectionAttackType[] _protectionAttackTypes = [];

    [ObservableAsProperty]
    private ProtectionAttribute[] _protectionAttributes = [];

    [ObservableAsProperty]
    private Force[] _forces = [];

    [ObservableAsProperty]
    private TacticType[] _tacticTypes = [];

    public JsonDataModelService(HttpClient httpClient)
    {
        _battleUnitsHelper = Observable.FromAsync(token => httpClient.GetFromJsonAsync("data/battle_units.json", JsonSerializer.Custom.BattleUnitArray, token)).WhereNotNull().ToProperty(this, nameof(BattleUnits));
        _battleAttackTypesHelper = Observable.FromAsync(token => httpClient.GetFromJsonAsync("data/battle_attack_types.json", JsonSerializer.Custom.BattleAttackTypeArray, token)).WhereNotNull().ToProperty(this, nameof(BattleAttackTypes));
        _battleAttributesHelper = Observable.FromAsync(token => httpClient.GetFromJsonAsync("data/battle_attributes.json", JsonSerializer.Custom.BattleAttributeArray, token)).WhereNotNull().ToProperty(this, nameof(BattleAttributes));
        
        _protectionUnitsHelper = Observable.FromAsync(token => httpClient.GetFromJsonAsync("data/protection_units.json", JsonSerializer.Custom.ProtectionUnitArray, token)).WhereNotNull().ToProperty(this, nameof(ProtectionUnits));
        _protectionAttackTypesHelper = Observable.FromAsync(token => httpClient.GetFromJsonAsync("data/protection_attack_types.json", JsonSerializer.Custom.ProtectionAttackTypeArray, token)).WhereNotNull().ToProperty(this, nameof(ProtectionAttackTypes));
        _protectionAttributesHelper = Observable.FromAsync(token => httpClient.GetFromJsonAsync("data/protection_attributes.json", JsonSerializer.Custom.ProtectionAttributeArray, token)).WhereNotNull().ToProperty(this, nameof(ProtectionAttributes));
        
        _forcesHelper = Observable.FromAsync(token => httpClient.GetFromJsonAsync("data/forces.json", JsonSerializer.Custom.ForceArray, token)).WhereNotNull().ToProperty(this, nameof(Forces));
        _tacticTypesHelper = Observable.FromAsync(token => httpClient.GetFromJsonAsync("data/tactics_types.json", JsonSerializer.Custom.TacticTypeArray, token)).WhereNotNull().ToProperty(this, nameof(TacticTypes));
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
    
    public Force? GetForce(string force)
    {
        return Forces.FirstOrDefault(f => f.Name.Equals(force, StringComparison.OrdinalIgnoreCase));
    }
    
    public TacticType? GetTacticType(string tacticType)
    {
        return TacticTypes.FirstOrDefault(t => t.Name.Equals(tacticType, StringComparison.OrdinalIgnoreCase));
    }
}