using SlimeIMWiki.Models.JsonData;

namespace SlimeIMWiki.Models;

public record Filter(string Key, Type KeyType, object? Parameter, Func<ICharacterUnitData, bool> FilterFunction);