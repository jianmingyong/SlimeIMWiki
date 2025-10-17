using System.Text.Json.Serialization;

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
    [property: JsonPropertyName("trait_1_name")]
    string Trait1Name,
    [property: JsonPropertyName("trait_1_icon")]
    string Trait1Icon,
    [property: JsonPropertyName("trait_1_effect")]
    string Trait1Effect,
    [property: JsonPropertyName("trait_1_effect_max")]
    string Trait1EffectMax,
    string? ValorTraitName,
    string? ValorTraitIcon,
    string? ValorTraitEffect,
    string? ValorTraitEffectMax,
    string? GuidanceEnhancementTraitEffect) : ICharacterUnit
{
    public string Icon => $"image/protection/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_BlessPartyM.png";

    public string Image => $"image/protection/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_BlessInfo.png";
}