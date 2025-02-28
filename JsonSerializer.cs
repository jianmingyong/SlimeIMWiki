using System.Text.Json.Serialization;

namespace SlimeIMWiki.Models;

[JsonSerializable(typeof(Livestream), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class JsonSerializer : JsonSerializerContext;