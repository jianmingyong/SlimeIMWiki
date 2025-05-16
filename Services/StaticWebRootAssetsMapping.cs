using System.Diagnostics.CodeAnalysis;

namespace SlimeIMWiki.Services;

public class StaticWebRootAssetsMapping(IConfiguration configuration)
{
    public Uri BattleUnitData => GenerateUri("data/battle_units.json");
    
    private readonly bool _useContentDeliveryNetwork = configuration.GetValue<bool>("StaticWebRootAssetsMapping:UseContentDeliveryNetwork");
    private readonly string? _contentDeliveryNetwork = configuration.GetValue<string?>("StaticWebRootAssetsMapping:ContentDeliveryNetwork");

    private Uri GenerateUri([StringSyntax(StringSyntaxAttribute.Uri)] string url)
    {
        if (!_useContentDeliveryNetwork || _contentDeliveryNetwork is null) return new Uri(url, UriKind.Relative);
        var cdn = new Uri(_contentDeliveryNetwork, UriKind.Absolute);
        return new Uri(cdn, url);
    }
}