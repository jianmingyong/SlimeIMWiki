using System.Net.Http.Json;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models.JsonData;

namespace SlimeIMWiki.Services;

public sealed partial class OldJsonDataModelService : ReactiveObject
{
    private readonly HttpClient _httpClient;
    private readonly StaticWebRootAssets _staticWebRootAssets;

    [ObservableAsProperty]
    private BattleUnitData[]? _battleUnits;

    [ObservableAsProperty]
    private BattleAttackType[]? _battleAttackTypes;

    [ObservableAsProperty]
    private BattleAttribute[]? _battleAttributes;
    
    [ObservableAsProperty]
    private BattleExpertise[]? _battleExpertise;

    [ObservableAsProperty]
    private ProtectionUnitData[]? _protectionUnits;

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

    public OldJsonDataModelService(HttpClient httpClient, StaticWebRootAssets staticWebRootAssets)
    {
        _httpClient = httpClient;
        _staticWebRootAssets = staticWebRootAssets;

        GetBattleUnits().ToProperty(this, nameof(BattleUnits), out _battleUnitsHelper);
        GetBattleAttackTypes().ToProperty(this, nameof(BattleAttackTypes), out _battleAttackTypesHelper);
        GetBattleAttributes().ToProperty(this, nameof(BattleAttributes), out _battleAttributesHelper);
        GetBattleExpertises().ToProperty(this, nameof(BattleExpertise), out _battleExpertiseHelper);

        GetProtectionUnits().ToProperty(this, nameof(ProtectionUnits), out _protectionUnitsHelper);
        GetProtectionAttackTypes().ToProperty(this, nameof(ProtectionAttackTypes), out _protectionAttackTypesHelper);
        GetProtectionAttributes().ToProperty(this, nameof(ProtectionAttributes), out _protectionAttributesHelper);

        GetForces().ToProperty(this, nameof(Forces), out _forcesHelper);
        GetTacticTypes().ToProperty(this, nameof(TacticTypes), out _tacticTypesHelper);
        GetFieldBuildings().ToProperty(this, nameof(FieldBuildings), out _fieldBuildingsHelper);
    }

    public IObservable<BattleAttackType?> GetObservableBattleAttackType(string attackType)
    {
        return this
            .WhenAnyValue(service => service.BattleAttackTypes)
            .Select(attackTypes => attackTypes?.SingleOrDefault(a => a.Name.Equals(attackType, StringComparison.OrdinalIgnoreCase)));
    }

    public IObservable<BattleAttribute?> GetObservableBattleAttribute(string attribute)
    {
        return this
            .WhenAnyValue(service => service.BattleAttributes)
            .Select(attributes => attributes?.SingleOrDefault(a => a.Name.Equals(attribute, StringComparison.OrdinalIgnoreCase)));
    }

    public IObservable<BattleExpertise?> GetObservableBattleExpertise(string expertise)
    {
        return this
            .WhenAnyValue(service => service.BattleExpertise)
            .Select(expertises => expertises?.SingleOrDefault(a => a.Name.Equals(expertise, StringComparison.OrdinalIgnoreCase)));
    }

    public IObservable<ProtectionAttackType?> GetObservableProtectionAttackType(string attackType)
    {
        return this
            .WhenAnyValue(service => service.ProtectionAttackTypes)
            .Select(attackTypes => attackTypes?.SingleOrDefault(a => a.Name.Equals(attackType, StringComparison.OrdinalIgnoreCase)));
    }

    public IObservable<ProtectionAttribute?> GetObservableProtectionAttribute(string attribute)
    {
        return this
            .WhenAnyValue(service => service.ProtectionAttributes)
            .Select(attributes => attributes?.SingleOrDefault(a => a.Name.Equals(attribute, StringComparison.OrdinalIgnoreCase)));
    }

    public IObservable<Force?> GetObservableForce(string force)
    {
        return this
            .WhenAnyValue(service => service.Forces)
            .Select(forces => forces?.SingleOrDefault(f => f.Name.Equals(force, StringComparison.OrdinalIgnoreCase)));
    }
    
    public IObservable<IEnumerable<Force?>> GetObservableForces(string[] forces)
    {
        return this
            .WhenAnyValue(service => service.Forces)
            .Select(f => forces.Select(s => f?.SingleOrDefault(force => force.Name.Equals(s, StringComparison.OrdinalIgnoreCase))));
    }

    public IObservable<IEnumerable<FieldBuilding?>> GetObservableFieldBuildings(string[] fieldBuildings)
    {
        return this
            .WhenAnyValue(service => service.FieldBuildings)
            .Select(f => fieldBuildings.Select(s => f?.SingleOrDefault(building => building.Name.Equals(s, StringComparison.OrdinalIgnoreCase))));
    }
    
    public IObservable<TacticType?> GetObservableTacticType(string type)
    {
        return this
            .WhenAnyValue(service => service.TacticTypes)
            .Select(tacticTypes => tacticTypes?.SingleOrDefault(t => t.Name.Equals(type, StringComparison.OrdinalIgnoreCase)));
    }

    public IObservable<Livestream?> GetLivestream()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.LiveStream, SlimeJsonDataSerializerContext.Default.Livestream, token));
    }

    private IObservable<BattleUnitData[]?> GetBattleUnits()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.BattleUnits, SlimeJsonDataSerializerContext.Default.BattleUnitDataArray, token));
    }

    private IObservable<BattleAttackType[]?> GetBattleAttackTypes()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.BattleAttackTypes, SlimeJsonDataSerializerContext.Default.BattleAttackTypeArray, token));
    }

    private IObservable<BattleAttribute[]?> GetBattleAttributes()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.BattleAttributes, SlimeJsonDataSerializerContext.Default.BattleAttributeArray, token));
    }

    private IObservable<BattleExpertise[]?> GetBattleExpertises()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.BattleExpertises, SlimeJsonDataSerializerContext.Default.BattleExpertiseArray, token));
    }

    private IObservable<ProtectionUnitData[]?> GetProtectionUnits()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.ProtectionUnits, SlimeJsonDataSerializerContext.Default.ProtectionUnitDataArray, token));
    }

    private IObservable<ProtectionAttackType[]?> GetProtectionAttackTypes()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.ProtectionAttackTypes, SlimeJsonDataSerializerContext.Default.ProtectionAttackTypeArray, token));
    }

    private IObservable<ProtectionAttribute[]?> GetProtectionAttributes()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.ProtectionAttributes, SlimeJsonDataSerializerContext.Default.ProtectionAttributeArray, token));
    }

    private IObservable<Force[]?> GetForces()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.Forces, SlimeJsonDataSerializerContext.Default.ForceArray, token));
    }

    private IObservable<TacticType[]?> GetTacticTypes()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.TacticTypes, SlimeJsonDataSerializerContext.Default.TacticTypeArray, token));
    }

    private IObservable<FieldBuilding[]?> GetFieldBuildings()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.FieldBuildings, SlimeJsonDataSerializerContext.Default.FieldBuildingArray, token));
    }
}