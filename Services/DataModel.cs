﻿using System.Net.Http.Json;
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
    }

    public Force? GetForce(string force)
    {
        return Forces.FirstOrDefault(f => f.Name.Equals(force, StringComparison.OrdinalIgnoreCase));
    }
}