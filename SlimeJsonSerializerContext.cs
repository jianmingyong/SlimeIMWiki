using System.Text.Json;
using System.Text.Json.Serialization;
using SlimeIMWiki.Models.JsonData;

namespace SlimeIMWiki;

[JsonSerializable(typeof(BattleUnit[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(BattleAttackType[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(BattleAttribute[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(BattleExpertise[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ProtectionUnit[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ProtectionAttackType[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ProtectionAttribute[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(Force[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(TacticType[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(Livestream), GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(FieldBuilding[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(Heartscape[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSourceGenerationOptions(JsonSerializerDefaults.General, PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
public partial class SlimeJsonSerializerContext : JsonSerializerContext;