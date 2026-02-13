using SlimeIMWiki.Models.JsonData;

namespace SlimeIMWiki.Models;

public class BattleUnit(
    JsonData.BattleUnit json, 
    Force[] forces, 
    BattleAttribute[] battleAttributes, 
    BattleAttackType[] battleAttackTypes,
    BattleExpertise[] battleExpertises,
    TacticType[] tacticTypes,
    FieldBuilding[] fieldBuildings)
{
    public string Name => json.Name;
    public string Title => json.Title;
    public string Permalink => json.Permalink;
    
    public string Icon => $"image/battle/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_CharaPartyM.png";
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
    public string UltimateManifestationImage
    {
        get
        {
            if (IsEx || IsAttributeUnbound || HasAttributeUnbound)
            {
                return $"image/battle/characters/{Permalink}/6/{Permalink}_6_CharaInfoAfter.png";
            }

            return $"image/battle/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_CharaInfoAfter.png";
        }
    }
    public string Card => $"image/battle/characters/{Permalink}/{InitialRarity}/{Permalink}_{InitialRarity}_CharaCard.png";
    
    public int InitialRarity => json.InitialRarity;
    public bool IsEx => json.IsEx;
    public bool IsAttributeUnbound => json.IsAttributeUnbound;
    public bool HasAttributeUnbound => json.HasAttributeUnbound;

    public Force[] Forces = json.Forces.Select(s => forces.SingleOrDefault(force => force.Name.Equals(s, StringComparison.OrdinalIgnoreCase)) ?? new Force(s, string.Empty, string.Empty)).ToArray();
    public BattleAttribute Attribute = battleAttributes.SingleOrDefault(attribute => attribute.Name.Equals(json.Attribute, StringComparison.OrdinalIgnoreCase)) ?? new BattleAttribute(json.Attribute, string.Empty);
    public BattleAttackType AttackType = battleAttackTypes.SingleOrDefault(type => type.Name.Equals(json.AttackType, StringComparison.OrdinalIgnoreCase)) ?? new BattleAttackType(json.AttackType, string.Empty);
    
    public int MaxHealth => json.MaxHealth;
    public int MaxAttack => json.MaxAttack;
    public int MaxDefense => json.MaxDefense;
    public int MaxOutput => json.MaxOutput;
    
    public string CharacterType => json.CharacterType;
    public BattleExpertise Expertise = battleExpertises.SingleOrDefault(expertise => expertise.Name.Equals(json.Expertise, StringComparison.OrdinalIgnoreCase)) ?? new BattleExpertise(json.Expertise, string.Empty);
    public TacticType TacticsType = tacticTypes.SingleOrDefault(type => type.Name.Equals(json.TacticsType, StringComparison.OrdinalIgnoreCase)) ?? new TacticType(json.TacticsType, string.Empty);
    public SuitedFacility[] SuitedFacilities = json.SuitedFacilities.Select((s, index) => new SuitedFacility(fieldBuildings.SingleOrDefault(building => building.Name.Equals(s, StringComparison.OrdinalIgnoreCase)) ?? new FieldBuilding(s, string.Empty, string.Empty), index == 0 ? 30 : 10)).ToArray();
    
    public DateOnly? ReleaseDate => json.ReleaseDate;
    
    
}