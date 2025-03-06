namespace SlimeIMWiki.Models;

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
    string? Force,
    string[]? Attributes,
    string? AttackType);