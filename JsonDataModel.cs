using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Hybrid;
using SlimeIMWiki.Models;

namespace SlimeIMWiki;

public class JsonDataModel(HybridCache hybridCache, HttpClient httpClient)
{
    public Livestream Livestream { get; private set; } = new(null);

    public Force[] Forces { get; private set; } = [];

    public BattleUnit[] BattleUnits { get; private set; } = [];

    public BattleAttackType[] BattleAttackTypes { get; private set; } = [];

    public BattleAttribute[] BattleAttributes { get; private set; } = [];

    public ProtectionUnit[] ProtectionUnits { get; private set; } = [];

    public ProtectionAttackType[] ProtectionAttackTypes { get; private set; } = [];

    public ProtectionAttribute[] ProtectionAttributes { get; private set; } = [];

    public TacticType[] TacticTypes { get; private set; } = [];

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

    public async Task LoadJsonDataModelAsync()
    {
        Livestream = await hybridCache.GetOrCreateAsync(nameof(Livestream), httpClient, async (client, cancellationToken) => await client.GetFromJsonAsync<Livestream>("data/livestream.json", JsonSerializer.Custom.Livestream, cancellationToken), new HybridCacheEntryOptions { Expiration = TimeSpan.FromHours(1) }) ?? new Livestream(null);

        Forces = await hybridCache.GetOrCreateAsync(nameof(Force), httpClient, async (client, cancellationToken) => await client.GetFromJsonAsync("data/forces.json", JsonSerializer.Custom.ForceArray, cancellationToken), new HybridCacheEntryOptions { Expiration = TimeSpan.FromHours(1) }) ?? [];

        BattleUnits = await hybridCache.GetOrCreateAsync(nameof(BattleUnit), httpClient, async (client, cancellationToken) => await client.GetFromJsonAsync("data/battle_units.json", JsonSerializer.Custom.BattleUnitArray, cancellationToken), new HybridCacheEntryOptions { Expiration = TimeSpan.FromHours(1) }) ?? [];
        BattleAttackTypes = await hybridCache.GetOrCreateAsync(nameof(BattleAttackType), httpClient, async (client, cancellationToken) => await client.GetFromJsonAsync("data/battle_attack_types.json", JsonSerializer.Custom.BattleAttackTypeArray, cancellationToken), new HybridCacheEntryOptions { Expiration = TimeSpan.FromHours(1) }) ?? [];
        BattleAttributes = await hybridCache.GetOrCreateAsync(nameof(BattleAttribute), httpClient, async (client, cancellationToken) => await client.GetFromJsonAsync("data/battle_attributes.json", JsonSerializer.Custom.BattleAttributeArray, cancellationToken), new HybridCacheEntryOptions { Expiration = TimeSpan.FromHours(1) }) ?? [];

        ProtectionUnits = await hybridCache.GetOrCreateAsync(nameof(ProtectionUnit), httpClient, async (client, cancellationToken) => await client.GetFromJsonAsync("data/protection_units.json", JsonSerializer.Custom.ProtectionUnitArray, cancellationToken), new HybridCacheEntryOptions { Expiration = TimeSpan.FromHours(1) }) ?? [];
        ProtectionAttackTypes = await hybridCache.GetOrCreateAsync(nameof(ProtectionAttackType), httpClient, async (client, cancellationToken) => await client.GetFromJsonAsync("data/protection_attack_types.json", JsonSerializer.Custom.ProtectionAttackTypeArray, cancellationToken), new HybridCacheEntryOptions { Expiration = TimeSpan.FromHours(1) }) ?? [];
        ProtectionAttributes = await hybridCache.GetOrCreateAsync(nameof(ProtectionAttribute), httpClient, async (client, cancellationToken) => await client.GetFromJsonAsync("data/protection_attributes.json", JsonSerializer.Custom.ProtectionAttributeArray, cancellationToken), new HybridCacheEntryOptions { Expiration = TimeSpan.FromHours(1) }) ?? [];

        TacticTypes = await hybridCache.GetOrCreateAsync(nameof(TacticType), httpClient, async (client, cancellationToken) => await client.GetFromJsonAsync("data/tactics_types.json", JsonSerializer.Custom.TacticTypeArray, cancellationToken), new HybridCacheEntryOptions { Expiration = TimeSpan.FromHours(1) }) ?? [];
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