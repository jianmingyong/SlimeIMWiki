using SlimeIMWiki.Models.JsonData;

namespace SlimeIMWiki.Models;

public record SearchResult(string DisplayValue, bool IsName, bool IsTitle, ICharacterUnit Unit);