using System.Text.Json.Serialization;

namespace SlimeIMWiki.Models;

public record BattleUnit(
    string Name,
    string Title,
    string Permalink,
    int InitialRarity,
    bool IsEx,
    bool IsAttributeUnbound,
    bool HasEx,
    bool HasAttributeUnbound,
    string[] Forces,
    string Attribute,
    string AttackType,
    int MaxHealth,
    int MaxAttack,
    int MaxDefense,
    int MaxOutput,
    string CharacterType,
    string Expertise,
    string TacticsType,
    string[] SuitedFacilities,
    string[] SecretSkills,
    string[] BattleSkills,
    DateOnly? ReleaseDate,
    string SecretSkillName,
    string SecretSkillTarget,
    string[] SecretSkillTypes,
    string[] SecretSkillDescriptions,
    string BattleSkill1Name,
    string BattleSkill1Icon,
    string[] BattleSkill1Descriptions,
    string BattleSkill2Name,
    string BattleSkill2Icon,
    string[] BattleSkill2Descriptions,
    [property: JsonPropertyName("trait_1_name")]
    string Trait1Name,
    [property: JsonPropertyName("trait_1_icon")]
    string Trait1Icon,
    [property: JsonPropertyName("trait_1_effect")]
    string Trait1Effect,
    [property: JsonPropertyName("trait_1_effect_max")]
    string Trait1EffectMax,
    [property: JsonPropertyName("trait_2_name")]
    string? Trait2Name,
    [property: JsonPropertyName("trait_2_icon")]
    string? Trait2Icon,
    [property: JsonPropertyName("trait_2_effect")]
    string? Trait2Effect,
    [property: JsonPropertyName("trait_2_effect_max")]
    string? Trait2EffectMax,
    string? ValorTraitName,
    string? ValorTraitIcon,
    string? ValorTraitEffect,
    string? ValorTraitEffectMax) : ICharacterUnit
{
    public string Icon => $"image/battle/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_CharaPartyM.png";

    public string Image => $"image/battle/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_CharaInfo.png";
}