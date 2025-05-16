namespace SlimeIMWiki.Models;

public record ProtectionUnit(
    string Name,
    string Title,
    string Permalink,
    int InitialRarity,
    bool IsEx,
    bool IsAttributeUnbound,
    bool HasEx,
    bool HasAttributeUnbound,
    string[]? Forces,
    string[]? Attributes,
    string? AttackType,
    int MaxHealth,
    int MaxAttack,
    int MaxDefense,
    int MaxOutput,
    string CharacterType,
    string TacticsType,
    string[] SuitedFacilities,
    DateOnly? ReleaseDate,
    string DivineProtectionName,
    string DivineProtectionEffect,
    string DivineProtectionEffectMax,
    string SupportDivineProtectionEffect,
    string SupportDivineProtectionEffectMax,
    string ProtectionSkillName,
    string ProtectionSkillEffect,
    string ProtectionSkillIcon,
    string TraitName,
    string TraitEffect,
    string TraitEffectMax,
    string TraitIcon,
    string? ValorTraitName,
    string? ValorTraitEffect,
    string? ValorTraitEffectMax,
    string? ValorTraitIcon,
    string? GuidanceEnhancementTraitEffect) : ICharacterUnit
{
    public string Icon => $"image/protection/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_BlessPartyM.png";

    public string Image => $"image/protection/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_BlessInfo.png";
}