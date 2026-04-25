namespace SlimeIMWiki.Models.JsonData;

public interface ICharacterUnitData
{
    string Name { get; }
    string Title { get; }

    string Permalink { get; }

    int InitialRarity { get; }

    bool IsEx { get; }
    bool IsAttributeUnbound { get; }

    bool HasAttributeUnbound { get; }
    bool HasUltimate { get; }

    int MaxHealth { get; }
    int MaxAttack { get; }
    int MaxDefense { get; }
    int MaxOutput { get; }

    string CharacterType { get; }
    string TacticsType { get; }
    
    string SuitedFacilityOneName { get; }
    int SuitedFacilityOneRate { get; }
    
    string? SuitedFacilityTwoName { get; }
    int? SuitedFacilityTwoRate { get; }

    string ReleaseDate { get; }

    string TraitOneName { get; }

    string TraitOneIcon { get; }

    string TraitOneEffect { get; }

    string TraitOneIconMax { get; }

    string TraitOneEffectMax { get; }

    string? ValorTraitName { get; }

    string? ValorTraitIcon { get; }

    string? ValorTraitEffect { get; }

    string? ValorTraitIconMax { get; }

    string? ValorTraitEffectMax { get; }
}