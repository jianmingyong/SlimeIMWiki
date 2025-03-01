using System.Text.Json.Serialization;

namespace SlimeIMWiki.Models;

public class Livestream
{
    [JsonPropertyName("youtube")]
    public string? YouTubeLink { get; set; }
}