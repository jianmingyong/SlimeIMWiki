namespace SlimeIMWiki.Models;

public class ProtectionUnit
{
    public string? Name { get; set; }

    public string? Title { get; set; }

    public string? Permalink { get; set; }

    public string? Icon { get; set; }

    public string? Image { get; set; }

    public int InitialRarity { get; set; }

    public bool IsEx { get; set; }

    public bool IsAttributeUnbound { get; set; }

    public string? AttackType { get; set; }

    public string? Attribute { get; set; }
}