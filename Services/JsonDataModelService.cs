using System.Net.Http.Json;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;

namespace SlimeIMWiki.Services;

public sealed partial class JsonDataModelService : ReactiveObject
{
    private readonly HttpClient _httpClient;
    private readonly StaticWebRootAssetsMapping _staticWebRootAssetsMapping;

    [ObservableAsProperty]
    private BattleUnit[]? _battleUnits;

    [ObservableAsProperty]
    private BattleAttackType[]? _battleAttackTypes;

    [ObservableAsProperty]
    private BattleAttribute[]? _battleAttributes;

    [ObservableAsProperty]
    private ProtectionUnit[]? _protectionUnits;

    [ObservableAsProperty]
    private ProtectionAttackType[]? _protectionAttackTypes;

    [ObservableAsProperty]
    private ProtectionAttribute[]? _protectionAttributes;

    [ObservableAsProperty]
    private Force[]? _forces;

    [ObservableAsProperty]
    private TacticType[]? _tacticTypes;
    
    [ObservableAsProperty]
    private FieldBuilding[]? _fieldBuildings;

    public JsonDataModelService(HttpClient httpClient, StaticWebRootAssetsMapping staticWebRootAssetsMapping)
    {
        _httpClient = httpClient;
        _staticWebRootAssetsMapping = staticWebRootAssetsMapping;
        
        GetBattleUnits().ToProperty(this, nameof(BattleUnits), out _battleUnitsHelper);
        GetBattleAttackTypes().ToProperty(this, nameof(BattleAttackTypes), out _battleAttackTypesHelper);
        GetBattleAttributes().ToProperty(this, nameof(BattleAttributes), out _battleAttributesHelper);

        GetProtectionUnits().ToProperty(this, nameof(ProtectionUnits), out _protectionUnitsHelper);
        GetProtectionAttackTypes().ToProperty(this, nameof(ProtectionAttackTypes), out _protectionAttackTypesHelper);
        GetProtectionAttributes().ToProperty(this, nameof(ProtectionAttributes), out _protectionAttributesHelper);

        GetForces().ToProperty(this, nameof(Forces), out _forcesHelper);
        GetTacticTypes().ToProperty(this, nameof(TacticTypes), out _tacticTypesHelper);
        GetFieldBuildings().ToProperty(this, nameof(FieldBuildings), out _fieldBuildingsHelper);
    }

    public IObservable<BattleUnit[]?> GetBattleUnits()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssetsMapping.BattleUnits, JsonSerializer.Custom.BattleUnitArray, token));
    }

    public IObservable<BattleAttackType[]?> GetBattleAttackTypes()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssetsMapping.BattleAttackTypes, JsonSerializer.Custom.BattleAttackTypeArray, token));
    }

    public IObservable<BattleAttribute[]?> GetBattleAttributes()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssetsMapping.BattleAttributes, JsonSerializer.Custom.BattleAttributeArray, token));
    }

    public IObservable<ProtectionUnit[]?> GetProtectionUnits()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssetsMapping.ProtectionUnits, JsonSerializer.Custom.ProtectionUnitArray, token));
    }

    public IObservable<ProtectionAttackType[]?> GetProtectionAttackTypes()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssetsMapping.ProtectionAttackTypes, JsonSerializer.Custom.ProtectionAttackTypeArray, token));
    }
    
    public IObservable<ProtectionAttribute[]?> GetProtectionAttributes()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssetsMapping.ProtectionAttributes, JsonSerializer.Custom.ProtectionAttributeArray, token));
    }

    public IObservable<Force[]?> GetForces()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssetsMapping.Forces, JsonSerializer.Custom.ForceArray, token));
    }

    public IObservable<TacticType[]?> GetTacticTypes()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssetsMapping.TacticTypes, JsonSerializer.Custom.TacticTypeArray, token));
    }

    public IObservable<FieldBuilding[]?> GetFieldBuildings()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssetsMapping.FieldBuildings, JsonSerializer.Custom.FieldBuildingArray, token));
    }
    
    public IObservable<Livestream?> GetLivestream()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssetsMapping.LiveStream, JsonSerializer.Custom.Livestream, token));
    }

    public BattleAttackType? GetBattleAttackType(string attackType)
    {
        return BattleAttackTypes?.SingleOrDefault(a => a.Name.Equals(attackType, StringComparison.OrdinalIgnoreCase));
    }

    public BattleAttribute? GetBattleAttribute(string attribute)
    {
        return BattleAttributes?.SingleOrDefault(a => a.Name.Equals(attribute, StringComparison.OrdinalIgnoreCase));
    }

    public ProtectionAttackType? GetProtectionAttackType(string attackType)
    {
        return ProtectionAttackTypes?.SingleOrDefault(a => a.Name.Equals(attackType, StringComparison.OrdinalIgnoreCase));
    }

    public ProtectionAttribute? GetProtectionAttribute(string attribute)
    {
        return ProtectionAttributes?.SingleOrDefault(a => a.Name.Equals(attribute, StringComparison.OrdinalIgnoreCase));
    }

    public Force? GetForce(string force)
    {
        return Forces?.SingleOrDefault(f => f.Name.Equals(force, StringComparison.OrdinalIgnoreCase));
    }

    public TacticType? GetTacticType(string tacticType)
    {
        return TacticTypes?.SingleOrDefault(t => t.Name.Equals(tacticType, StringComparison.OrdinalIgnoreCase));
    }

    public FieldBuilding? GetFieldBuilding(string fieldBuilding)
    {
        return FieldBuildings?.SingleOrDefault(f => f.Name.Equals(fieldBuilding, StringComparison.OrdinalIgnoreCase));
    }
}