﻿namespace SlimeIMWiki.Models;

public record ProtectionUnit(
    string Name,
    string Title,
    string Permalink,
    string Icon,
    string Facing,
    string Image,
    int InitialRarity,
    bool IsEx,
    bool IsAttributeUnbound,
    string[]? Forces,
    string[]? Attributes,
    string? AttackType,
    int? MinHealth,
    int? MinAttack,
    int? MinDefense,
    int? MinOutput,
    int MaxHealth,
    int MaxAttack,
    int MaxDefense,
    int MaxOutput,
    string CharacterType,
    string TacticsType,
    string[] SuitedFacilities,
    DateOnly ReleaseDate);