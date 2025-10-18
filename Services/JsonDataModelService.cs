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
    private BattleUnit[] _battleUnits = [];

    [ObservableAsProperty]
    private BattleAttackType[] _battleAttackTypes = [];

    [ObservableAsProperty]
    private BattleAttribute[] _battleAttributes = [];

    [ObservableAsProperty]
    private ProtectionUnit[] _protectionUnits = [];

    [ObservableAsProperty]
    private ProtectionAttackType[] _protectionAttackTypes = [];

    [ObservableAsProperty]
    private ProtectionAttribute[] _protectionAttributes = [];

    [ObservableAsProperty]
    private Force[] _forces = [];

    [ObservableAsProperty]
    private TacticType[] _tacticTypes = [];
    
    [ObservableAsProperty]
    private FieldBuilding[] _fieldBuildings = [];

    public JsonDataModelService(HttpClient httpClient, StaticWebRootAssetsMapping staticWebRootAssetsMapping)
    {
        _httpClient = httpClient;
        _staticWebRootAssetsMapping = staticWebRootAssetsMapping;

        Observable.FromAsync(token => httpClient.GetFromJsonAsync(staticWebRootAssetsMapping.BattleUnits, JsonSerializer.Custom.BattleUnitArray, token)).WhereNotNull().ToProperty(this, nameof(BattleUnits), out _battleUnitsHelper);
        Observable.FromAsync(token => httpClient.GetFromJsonAsync(staticWebRootAssetsMapping.BattleAttackTypes, JsonSerializer.Custom.BattleAttackTypeArray, token)).WhereNotNull().ToProperty(this, nameof(BattleAttackTypes), out _battleAttackTypesHelper);
        Observable.FromAsync(token => httpClient.GetFromJsonAsync(staticWebRootAssetsMapping.BattleAttributes, JsonSerializer.Custom.BattleAttributeArray, token)).WhereNotNull().ToProperty(this, nameof(BattleAttributes), out _battleAttributesHelper);

        Observable.FromAsync(token => httpClient.GetFromJsonAsync(staticWebRootAssetsMapping.ProtectionUnits, JsonSerializer.Custom.ProtectionUnitArray, token)).WhereNotNull().ToProperty(this, nameof(ProtectionUnits), out _protectionUnitsHelper);
        Observable.FromAsync(token => httpClient.GetFromJsonAsync(staticWebRootAssetsMapping.ProtectionAttackTypes, JsonSerializer.Custom.ProtectionAttackTypeArray, token)).WhereNotNull().ToProperty(this, nameof(ProtectionAttackTypes), out _protectionAttackTypesHelper);
        Observable.FromAsync(token => httpClient.GetFromJsonAsync(staticWebRootAssetsMapping.ProtectionAttributes, JsonSerializer.Custom.ProtectionAttributeArray, token)).WhereNotNull().ToProperty(this, nameof(ProtectionAttributes), out _protectionAttributesHelper);

        Observable.FromAsync(token => httpClient.GetFromJsonAsync(staticWebRootAssetsMapping.Forces, JsonSerializer.Custom.ForceArray, token)).WhereNotNull().ToProperty(this, nameof(Forces), out _forcesHelper);
        Observable.FromAsync(token => httpClient.GetFromJsonAsync(staticWebRootAssetsMapping.TacticTypes, JsonSerializer.Custom.TacticTypeArray, token)).WhereNotNull().ToProperty(this, nameof(TacticTypes), out _tacticTypesHelper);
        Observable.FromAsync(token => httpClient.GetFromJsonAsync(staticWebRootAssetsMapping.FieldBuildings, JsonSerializer.Custom.FieldBuildingArray, token)).WhereNotNull().ToProperty(this, nameof(FieldBuilding), out _fieldBuildingsHelper);
    }

    public IObservable<Livestream?> GetLivestream()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssetsMapping.LiveStream, JsonSerializer.Custom.Livestream, token));
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

    public FieldBuilding? GetFieldBuilding(string fieldBuilding)
    {
        return FieldBuildings.FirstOrDefault(f => f.Name.Equals(fieldBuilding, StringComparison.OrdinalIgnoreCase));
    }
}