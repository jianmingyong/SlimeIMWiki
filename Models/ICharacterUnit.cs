namespace SlimeIMWiki.Models;

public interface ICharacterUnit
{
    string Name { get; }
    string Title { get; }

    string Permalink { get; }
    
    string Icon { get; }
    string Image { get; }

    int InitialRarity { get; }

    bool IsEx { get; }
    bool IsAttributeUnbound { get; }

    bool HasEx { get; }
    bool HasAttributeUnbound { get; }

    int MaxHealth { get; }
    int MaxAttack { get; }
    int MaxDefense { get; }
    int MaxOutput { get; }

    string CharacterType { get; }
    string TacticsType { get; }

    string[] SuitedFacilities { get; }

    DateOnly? ReleaseDate { get; }
    
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