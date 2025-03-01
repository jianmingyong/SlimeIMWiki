using System.Text.Json.Serialization;
using SlimeIMWiki.Models;

namespace SlimeIMWiki;

[JsonSerializable(typeof(Livestream), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class JsonSerializer : JsonSerializerContext;