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
    string? ValorTrait,
    DateOnly? ReleaseDate,
    string TraitName,
    string TraitEffect,
    string? ValorTraitName,
    string? ValorTraitEffect) : ICharacterUnit
{
    public string Icon => $"image/battle/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_CharaPartyM.png";

    public string Image => $"image/battle/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_CharaInfo.png";
}