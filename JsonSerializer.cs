﻿using System.Text.Json;
using System.Text.Json.Serialization;
using SlimeIMWiki.Models;

namespace SlimeIMWiki;

[JsonSerializable(typeof(Livestream), GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ProtectionAttackType[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ProtectionAttribute[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ProtectionUnit[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class JsonSerializer : JsonSerializerContext
{
    public static JsonSerializer Custom { get; } = new(new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    });
}