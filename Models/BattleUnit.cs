using DynamicData.Kernel;
using SlimeIMWiki.Models.JsonData;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Models;

public record BattleUnit(
    string Name,
    string Title,
    string Permalink,
    int InitialRarity,
    bool IsEx,
    bool IsAttributeUnbound,
    bool HasAttributeUnbound,
    bool HasUltimateManifestation,
    Force[] Forces,
    IAttribute Attribute,
    IAttackType AttackType,
    int MaxHealth,
    int MaxAttack,
    int MaxDefense,
    int MaxOutput,
    string CharacterType,
    BattleExpertise Expertise,
    TacticType TacticsType,
    SuitedFacility[] SuitedFacilities,
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
    string? BattleSkillOneFusedIcon,
    int? BattleSkillOneFusedEffectCost,
    string? BattleSkillOneFusedEffect,
    string? BattleSkillOneUltimateManifestationIcon,
    int? BattleSkillOneUltimateManifestationEffectCost,
    string? BattleSkillOneUltimateManifestationEffect,
    string BattleSkillTwoName,
    string BattleSkillTwoIcon,
    int BattleSkillTwoEffectCost,
    string BattleSkillTwoEffect,
    string? BattleSkillTwoFusedIcon,
    int? BattleSkillTwoFusedEffectCost,
    string? BattleSkillTwoFusedEffect,
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
    string? ValorTraitEffectMax,
    string? SecretSkillEnhancementTrait,
    string ExAbilityEffectOneName,
    string ExAbilityEffectOneCondition,
    string ExAbilityEffectOneEffect,
    string ExAbilityEffectTwoName,
    string ExAbilityEffectTwoCondition,
    string ExAbilityEffectTwoEffect,
    string? TrueAttributeUnbound) : ICharacterUnit
{
    public string Icon => $"image/battle/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_CharaPartyM.png";

    public string IconAfter => $"image/battle/characters/{Permalink}After/{InitialRarity}/{Permalink}After_{InitialRarity}_CharaPartyM.png";

    public string Image
    {
        get
        {
            if (IsEx || IsAttributeUnbound || HasAttributeUnbound)
            {
                return $"image/battle/characters/{Permalink}/6/{Permalink}_6_CharaInfo.png";
            }

            return $"image/battle/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_CharaInfo.png";
        }
    }

    public string ImageAfter
    {
        get
        {
            if (IsEx || IsAttributeUnbound || HasAttributeUnbound)
            {
                return $"image/battle/characters/{Permalink}After/6/{Permalink}_6_CharaInfo.png";
            }

            return $"image/battle/characters/{Permalink}After/{InitialRarity}/{Permalink}After_{InitialRarity}_CharaInfo.png";
        }
    }

    public string Card => $"image/battle/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_CharaCard.png";

    public string CardAfter => $"image/battle/characters/{Permalink}After/{InitialRarity}/{Permalink}After_{InitialRarity}_CharaCard.png";

    public static BattleUnit FromBattleUnitData(BattleUnitData data, JsonDataModelService service)
    {
        return new BattleUnit(
            data.Name,
            data.Title,
            data.Permalink,
            data.InitialRarity,
            data.IsEx,
            data.IsAttributeUnbound,
            data.HasAttributeUnbound,
            data.HasUltimateManifestation,
            data.Forces.Select(s => service.ForceCache.Lookup(s).ValueOr(() => new Force(s, string.Empty, string.Empty))).ToArray(),
            service.BattleAttributeCache.Lookup(data.Attribute).ValueOr(() => new BattleAttribute(data.Attribute, string.Empty)),
            service.BattleAttackTypeCache.Lookup(data.AttackType).ValueOr(() => new BattleAttackType(data.AttackType, string.Empty)),
            data.MaxHealth,
            data.MaxAttack,
            data.MaxDefense,
            data.MaxOutput,
            data.CharacterType,
            service.BattleExpertiseCache.Lookup(data.Expertise).ValueOr(() => new BattleExpertise(data.Expertise, string.Empty)),
            service.TacticTypeCache.Lookup(data.TacticsType).ValueOr(() => new TacticType(data.TacticsType, string.Empty)),
            data.SuitedFacilities.Select((s, i) => new SuitedFacility(service.FieldBuildingCache.Lookup(s).ValueOr(() => new FieldBuilding(s, string.Empty, string.Empty)), i == 0 ? 30 : 10)).ToArray(),
            data.ReleaseDate,
            data.SecretSkillName,
            data.SecretSkillTarget,
            data.SecretSkillNormalEffect,
            data.SecretSkillAttackEffect,
            data.SecretSkillSupportEffect,
            data.BattleSkillOneName,
            data.BattleSkillOneIcon,
            data.BattleSkillOneEffectCost,
            data.BattleSkillOneEffect,
            data.BattleSkillOneFusedIcon,
            data.BattleSkillOneFusedEffectCost,
            data.BattleSkillOneFusedEffect,
            data.BattleSkillOneUltimateManifestationIcon,
            data.BattleSkillOneUltimateManifestationEffectCost,
            data.BattleSkillOneUltimateManifestationEffect,
            data.BattleSkillTwoName,
            data.BattleSkillTwoIcon,
            data.BattleSkillTwoEffectCost,
            data.BattleSkillTwoEffect,
            data.BattleSkillTwoFusedIcon,
            data.BattleSkillTwoFusedEffectCost,
            data.BattleSkillTwoFusedEffect,
            data.TraitOneName,
            data.TraitOneIcon,
            data.TraitOneEffect,
            data.TraitOneIconMax,
            data.TraitOneEffectMax,
            data.TraitTwoName,
            data.TraitTwoIcon,
            data.TraitTwoEffect,
            data.TraitTwoIconMax,
            data.TraitTwoEffectMax,
            data.ValorTraitName,
            data.ValorTraitIcon,
            data.ValorTraitEffect,
            data.ValorTraitIconMax,
            data.ValorTraitEffectMax,
            data.SecretSkillEnhancementTrait,
            data.ExAbilityEffectOneName,
            data.ExAbilityEffectOneCondition,
            data.ExAbilityEffectOneEffect,
            data.ExAbilityEffectTwoName,
            data.ExAbilityEffectTwoCondition,
            data.ExAbilityEffectTwoEffect,
            data.TrueAttributeUnbound
        );
    }
}