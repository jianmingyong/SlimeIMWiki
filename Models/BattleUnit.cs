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
    string? SecretSkillNormalEffect,
    string? SecretSkillAttackEffect,
    string? SecretSkillSupportEffect,
    string BattleSkillOneName,
    string BattleSkillOneIcon,
    int BattleSkillOneEffectCost,
    string BattleSkillOneEffect,
    int? BattleSkillOneEffectFusedCost,
    string? BattleSkillOneEffectFused,
    string BattleSkillTwoName,
    string BattleSkillTwoIcon,
    int BattleSkillTwoEffectCost,
    string BattleSkillTwoEffect,
    int? BattleSkillTwoEffectFusedCost,
    string? BattleSkillTwoEffectFused,
    string TraitOneName,
    string TraitOneIcon,
    string TraitOneEffect,
    string TraitOneIconMax,
    string TraitOneEffectMax,
    string? TraitTwoName,
    string? TraitTwoIcon,
    string? TraitTwoEffect,
    string? TraitTwoIconMax,
    string? TraitTwoEffectMax,
    string? ValorTraitName,
    string? ValorTraitIcon,
    string? ValorTraitEffect,
    string? ValorTraitIconMax,
    string? ValorTraitEffectMax) : ICharacterUnit
{
    public string Icon => $"image/battle/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_CharaPartyM.png";

    public string Image
    {
        get
        {
            if (IsEx || IsAttributeUnbound || HasEx || HasAttributeUnbound)
            {
                return $"image/battle/characters/{Permalink}/6/{Permalink}_6_CharaInfo.png";
            }

            return $"image/battle/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_CharaInfo.png";
        }
    }
}