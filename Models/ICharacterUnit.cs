﻿namespace SlimeIMWiki.Models;

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
    
    string TraitName { get; }
    
    string TraitEffect { get; }
    
    string? ValorTraitName { get; }
    
    string? ValorTraitEffect { get; }
}