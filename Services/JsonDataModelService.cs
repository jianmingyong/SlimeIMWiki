using System.Net.Http.Json;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using DynamicData;
using SlimeIMWiki.Models.JsonData;

namespace SlimeIMWiki.Services;

public sealed class JsonDataModelService : IDisposable
{
    public IObservableCache<BattleUnitData, string> BattleUnitsDataCache { get; }
    public IObservableCache<BattleAttribute, string> BattleAttributeCache { get; }
    public IObservableCache<BattleAttackType, string> BattleAttackTypeCache { get; }
    public IObservableCache<BattleExpertise, string> BattleExpertiseCache { get; }

    public IObservableCache<ProtectionUnitData, string> ProtectionUnitsDataCache { get; }
    public IObservableCache<ProtectionAttribute, string> ProtectionAttributeCache { get; }
    public IObservableCache<ProtectionAttackType, string> ProtectionAttackTypeCache { get; }

    public IObservableCache<TacticType, string> TacticTypeCache { get; }
    public IObservableCache<Force, string> ForceCache { get; }
    public IObservableCache<FieldBuilding, string> FieldBuildingCache { get; }

    private readonly SourceCache<BattleUnitData, string> _battleUnitCache = new(type => type.Permalink);
    private readonly SourceCache<BattleAttribute, string> _battleAttributeCache = new(type => type.Name);
    private readonly SourceCache<BattleAttackType, string> _battleAttackTypeCache = new(type => type.Name);
    private readonly SourceCache<BattleExpertise, string> _battleExpertiseCache = new(type => type.Name);

    private readonly SourceCache<ProtectionUnitData, string> _protectionUnitCache = new(type => type.Permalink);
    private readonly SourceCache<ProtectionAttribute, string> _protectionAttributeCache = new(type => type.Name);
    private readonly SourceCache<ProtectionAttackType, string> _protectionAttackTypeCache = new(type => type.Name);

    private readonly SourceCache<TacticType, string> _tacticTypeCache = new(type => type.Name);
    private readonly SourceCache<Force, string> _forceCache = new(type => type.Name);
    private readonly SourceCache<FieldBuilding, string> _fieldBuildingCache = new(type => type.Name);

    private readonly HttpClient _httpClient;
    private readonly StaticWebRootAssets _staticWebRootAssets;

    private readonly CompositeDisposable _disposables = new();

    public JsonDataModelService(HttpClient httpClient, StaticWebRootAssets staticWebRootAssets)
    {
        _httpClient = httpClient;
        _staticWebRootAssets = staticWebRootAssets;

        BattleUnitsDataCache = _battleUnitCache.AsObservableCache();
        _battleUnitCache.DisposeWith(_disposables);

        BattleAttributeCache = _battleAttributeCache.AsObservableCache();
        _battleAttributeCache.DisposeWith(_disposables);

        BattleAttackTypeCache = _battleAttackTypeCache.AsObservableCache();
        _battleAttackTypeCache.DisposeWith(_disposables);

        BattleExpertiseCache = _battleExpertiseCache.AsObservableCache();
        _battleExpertiseCache.DisposeWith(_disposables);

        ProtectionUnitsDataCache = _protectionUnitCache.AsObservableCache();
        _protectionUnitCache.DisposeWith(_disposables);

        ProtectionAttributeCache = _protectionAttributeCache.AsObservableCache();
        _protectionAttributeCache.DisposeWith(_disposables);

        ProtectionAttackTypeCache = _protectionAttackTypeCache.AsObservableCache();
        _protectionAttackTypeCache.DisposeWith(_disposables);

        TacticTypeCache = _tacticTypeCache.AsObservableCache();
        _tacticTypeCache.DisposeWith(_disposables);

        ForceCache = _forceCache.AsObservableCache();
        _forceCache.DisposeWith(_disposables);

        FieldBuildingCache = _fieldBuildingCache.AsObservableCache();
        _fieldBuildingCache.DisposeWith(_disposables);
    }

    private static void UpdateCache<TObject, TKey>(ISourceCache<TObject, TKey> cache, TObject[]? objects) where TObject : notnull where TKey : notnull
    {
        if (objects is null) return;

        cache.Edit(updater =>
        {
            if (updater.Count == 0)
            {
                updater.Load(objects);
                return;
            }

            updater.AddOrUpdate(objects);
            updater.RemoveKeys(cache.Keys.Except(objects.Select(o => cache.KeySelector(o))));
        });
    }

    public IObservable<Unit> RefreshData()
    {
        return Observable.Create<Unit>(observer =>
        {
            var disposables = new CompositeDisposable();

            // ReSharper disable once InvokeAsExtensionMember
            Observable.Zip(
                GetObservableBattleAttributes().Catch(Observable.Return<BattleAttribute[]?>(null)),
                GetObservableBattleAttackTypes().Catch(Observable.Return<BattleAttackType[]?>(null)),
                GetObservableBattleExpertises().Catch(Observable.Return<BattleExpertise[]?>(null)),
                GetObservableProtectionAttributes().Catch(Observable.Return<ProtectionAttribute[]?>(null)),
                GetObservableProtectionAttackTypes().Catch(Observable.Return<ProtectionAttackType[]?>(null)),
                GetObservableTacticTypes().Catch(Observable.Return<TacticType[]?>(null)),
                GetObservableForces().Catch(Observable.Return<Force[]?>(null)),
                GetObservableFieldBuildings().Catch(Observable.Return<FieldBuilding[]?>(null)),
                (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8)
            ).Subscribe(tuple =>
            {
                UpdateCache(_battleAttributeCache, tuple.arg1);
                UpdateCache(_battleAttackTypeCache, tuple.arg2);
                UpdateCache(_battleExpertiseCache, tuple.arg3);
                UpdateCache(_protectionAttributeCache, tuple.arg4);
                UpdateCache(_protectionAttackTypeCache, tuple.arg5);
                UpdateCache(_tacticTypeCache, tuple.arg6);
                UpdateCache(_forceCache, tuple.arg7);
                UpdateCache(_fieldBuildingCache, tuple.arg8);
            }, observer.OnError, () =>
            {
                // ReSharper disable once InvokeAsExtensionMember
                Observable.Zip(
                    GetObservableBattleUnits().Catch(Observable.Return<BattleUnitData[]?>(null)),
                    GetObservableProtectionUnits().Catch(Observable.Return<ProtectionUnitData[]?>(null)),
                    (arg1, arg2) => (arg1, arg2)
                ).Subscribe(tuple =>
                {
                    UpdateCache(_battleUnitCache, tuple.arg1);
                    UpdateCache(_protectionUnitCache, tuple.arg2);
                }, observer.OnError, observer.OnCompleted).DisposeWith(disposables);
            }).DisposeWith(disposables);

            return disposables;
        });
    }

    public IObservable<Livestream?> GetObservableLivestream()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.LiveStream, SlimeJsonDataSerializerContext.Default.Livestream, token));
    }

    private IObservable<BattleUnitData[]?> GetObservableBattleUnits()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.BattleUnits, SlimeJsonDataSerializerContext.Default.BattleUnitDataArray, token));
    }

    private IObservable<BattleAttribute[]?> GetObservableBattleAttributes()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.BattleAttributes, SlimeJsonDataSerializerContext.Default.BattleAttributeArray, token));
    }

    private IObservable<BattleAttackType[]?> GetObservableBattleAttackTypes()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.BattleAttackTypes, SlimeJsonDataSerializerContext.Default.BattleAttackTypeArray, token));
    }

    private IObservable<BattleExpertise[]?> GetObservableBattleExpertises()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.BattleExpertises, SlimeJsonDataSerializerContext.Default.BattleExpertiseArray, token));
    }

    private IObservable<ProtectionUnitData[]?> GetObservableProtectionUnits()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.ProtectionUnits, SlimeJsonDataSerializerContext.Default.ProtectionUnitDataArray, token));
    }

    private IObservable<ProtectionAttribute[]?> GetObservableProtectionAttributes()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.ProtectionAttributes, SlimeJsonDataSerializerContext.Default.ProtectionAttributeArray, token));
    }

    private IObservable<ProtectionAttackType[]?> GetObservableProtectionAttackTypes()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.ProtectionAttackTypes, SlimeJsonDataSerializerContext.Default.ProtectionAttackTypeArray, token));
    }

    private IObservable<TacticType[]?> GetObservableTacticTypes()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.TacticTypes, SlimeJsonDataSerializerContext.Default.TacticTypeArray, token));
    }

    private IObservable<Force[]?> GetObservableForces()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.Forces, SlimeJsonDataSerializerContext.Default.ForceArray, token));
    }

    private IObservable<FieldBuilding[]?> GetObservableFieldBuildings()
    {
        return Observable.FromAsync(token => _httpClient.GetFromJsonAsync(_staticWebRootAssets.FieldBuildings, SlimeJsonDataSerializerContext.Default.FieldBuildingArray, token));
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}