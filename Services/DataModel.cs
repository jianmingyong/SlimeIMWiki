using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using SlimeIMWiki.Models;

namespace SlimeIMWiki.Services;

public class DataModel(IMemoryCache memoryCache, HttpClient httpClient)
{
    public Force[] Forces { get; private set; } = [];

    public BattleUnit[] BattleUnits { get; private set; } = [];

    public BattleAttackType[] BattleAttackTypes { get; private set; } = [];

    public BattleAttribute[] BattleAttributes { get; private set; } = [];

    public ProtectionUnit[] ProtectionUnits { get; private set; } = [];

    public ProtectionAttackType[] ProtectionAttackTypes { get; private set; } = [];

    public ProtectionAttribute[] ProtectionAttributes { get; private set; } = [];

    public TacticType[] TacticTypes { get; private set; } = [];

    public async Task LoadDataModelAsync()
    {
        Forces = await memoryCache.GetOrCreateAsync(nameof(Force), async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));
            return await httpClient.GetFromJsonAsync("data/forces.json", JsonSerializer.Custom.ForceArray);
        }) ?? [];

        BattleUnits = await memoryCache.GetOrCreateAsync(nameof(BattleUnit), async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));
            return await httpClient.GetFromJsonAsync("data/battle_units.json", JsonSerializer.Custom.BattleUnitArray);
        }) ?? [];

        BattleAttackTypes = await memoryCache.GetOrCreateAsync(nameof(BattleAttackType), async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));
            return await httpClient.GetFromJsonAsync("data/battle_attack_types.json", JsonSerializer.Custom.BattleAttackTypeArray);
        }) ?? [];

        BattleAttributes = await memoryCache.GetOrCreateAsync(nameof(BattleAttribute), async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));
            return await httpClient.GetFromJsonAsync("data/battle_attributes.json", JsonSerializer.Custom.BattleAttributeArray);
        }) ?? [];

        ProtectionUnits = await memoryCache.GetOrCreateAsync(nameof(ProtectionUnit), async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));
            return await httpClient.GetFromJsonAsync("data/protection_units.json", JsonSerializer.Custom.ProtectionUnitArray);
        }) ?? [];

        ProtectionAttackTypes = await memoryCache.GetOrCreateAsync(nameof(ProtectionAttackType), async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));
            return await httpClient.GetFromJsonAsync("data/protection_attack_types.json", JsonSerializer.Custom.ProtectionAttackTypeArray);
        }) ?? [];

        ProtectionAttributes = await memoryCache.GetOrCreateAsync(nameof(ProtectionAttribute), async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));
            return await httpClient.GetFromJsonAsync("data/protection_attributes.json", JsonSerializer.Custom.ProtectionAttributeArray);
        }) ?? [];

        TacticTypes = await memoryCache.GetOrCreateAsync(nameof(TacticType), async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));
            return await httpClient.GetFromJsonAsync("data/tactics_types.json", JsonSerializer.Custom.TacticTypeArray);
        }) ?? [];
    }

    public ICharacterUnit? GetUnit(string permalink)
    {
        return BattleUnits.FirstOrDefault(u => u.Permalink.Equals(permalink, StringComparison.OrdinalIgnoreCase)) as ICharacterUnit ?? ProtectionUnits.FirstOrDefault(u => u.Permalink.Equals(permalink, StringComparison.OrdinalIgnoreCase));
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