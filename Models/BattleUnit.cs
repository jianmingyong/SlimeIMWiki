namespace SlimeIMWiki.Models;

public record BattleUnit(
    string Name,
    string Title,
    string Permalink,
    string Icon,
    string Facing,
    string Image,
    int InitialRarity,
    bool IsEx,
    bool IsAttributeUnbound,
    bool HasEx,
    bool HasAttributeUnbound,
    string[] Forces,
    string Attribute,
    string AttackType);