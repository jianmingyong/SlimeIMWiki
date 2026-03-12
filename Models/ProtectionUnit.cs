using System.Globalization;
using DynamicData.Kernel;
using SlimeIMWiki.Models.JsonData;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Models;

public record ProtectionUnit(
    string Name,
    string Title,
    string Permalink,
    int InitialRarity,
    bool IsEx,
    bool IsAttributeUnbound,
    bool HasAttributeUnbound,
    Force[]? Forces,
    IAttribute[]? Attributes,
    IAttackType? AttackType,
    int MaxHealth,
    int MaxAttack,
    int MaxDefense,
    int MaxOutput,
    string CharacterType,
    TacticType TacticsType,
    SuitedFacility[] SuitedFacilities,
    DateTime ReleaseDate,
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
            if (IsEx || IsAttributeUnbound)
            {
                return $"image/protection/characters/{Permalink}/6/{Permalink}_6_BlessInfo.png";
            }

            return $"image/protection/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_BlessInfo.png";
        }
    }

    public static ProtectionUnit FromProtectionUnitData(ProtectionUnitData data, JsonDataModelService service)
    {
        return new ProtectionUnit(
            data.Name,
            data.Title,
            data.Permalink,
            data.InitialRarity,
            data.IsEx,
            data.IsAttributeUnbound,
            data.HasAttributeUnbound,
            data.Forces?.Select(s => service.ForceCache.Lookup(s).ValueOr(() => new Force(s, string.Empty, string.Empty))).ToArray(),
            data.Attributes?.Select(IAttribute (s) => service.ProtectionAttributeCache.Lookup(s).ValueOr(() => new ProtectionAttribute(s, string.Empty))).ToArray(),
            data.AttackType is null ? null : service.ProtectionAttackTypeCache.Lookup(data.AttackType).ValueOr(() => new ProtectionAttackType(data.AttackType, string.Empty)),
            data.MaxHealth,
            data.MaxAttack,
            data.MaxDefense,
            data.MaxOutput,
            data.CharacterType,
            service.TacticTypeCache.Lookup(data.TacticsType).ValueOr(() => new TacticType(data.TacticsType, string.Empty)),
            data.SuitedFacilities.Select((s, i) => new SuitedFacility(service.FieldBuildingCache.Lookup(s).ValueOr(() => new FieldBuilding(s, string.Empty, string.Empty)), i == 0 ? 200 : 100)).ToArray(),
            DateTimeOffset.Parse(data.ReleaseDate, DateTimeFormatInfo.InvariantInfo).AddHours(10).ToOffset(TimeSpan.FromHours(8)).LocalDateTime,
            data.DivineProtectionName,
            data.DivineProtectionEffect,
            data.DivineProtectionEffectMax,
            data.SupportDivineProtectionEffect,
            data.SupportDivineProtectionEffectMax,
            data.ProtectionSkillName,
            data.ProtectionSkillIcon,
            data.ProtectionSkillEffect,
            data.TraitOneName,
            data.TraitOneIcon,
            data.TraitOneEffect,
            data.TraitOneIconMax,
            data.TraitOneEffectMax,
            data.ValorTraitName,
            data.ValorTraitIcon,
            data.ValorTraitEffect,
            data.ValorTraitIconMax,
            data.ValorTraitEffectMax,
            data.GuidanceEnhancementTraitEffect
        );
    }
}