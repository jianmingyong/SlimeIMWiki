namespace SlimeIMWiki.Models.JsonData;

public record Heartscape(
    string Name,
    string Category,
    string Icon,
    string Type,
    string[] Descriptions,
    string UnlockCondition,
    string UnlockMaterial,
    DateOnly? ReleaseDate);