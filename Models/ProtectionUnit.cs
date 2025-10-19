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
    string ProtectionSkillIcon,
    string ProtectionSkillEffect,
    string TraitOneName,
    string TraitOneIcon,
    string TraitOneEffect,
    string TraitOneIconMax,
    string TraitOneEffectMax,
    string? ValorTraitName,
    string? ValorTraitIcon,
    string? ValorTraitEffect,
    string? ValorTraitIconMax,
    string? ValorTraitEffectMax,
    string? GuidanceEnhancementTraitEffect) : ICharacterUnit
{
    public string Icon => $"image/protection/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_BlessPartyM.png";

    public string Image
    {
        get
        {
            if (IsEx || IsAttributeUnbound || HasEx || HasAttributeUnbound)
            {
                return $"image/protection/characters/{Permalink}/6/{Permalink}_6_BlessInfo.png";
            }

            return $"image/protection/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_BlessInfo.png";
        }
    }
}