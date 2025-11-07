using System.Diagnostics.CodeAnalysis;

namespace SlimeIMWiki.Services;

public class StaticWebRootAssets(IConfiguration configuration)
{
    public Uri BattleUnits => ResolveUri("data/battle_units.json");
    public Uri BattleAttackTypes => ResolveUri("data/battle_attack_types.json");
    public Uri BattleAttributes => ResolveUri("data/battle_attributes.json");
    
    public Uri ProtectionUnits => ResolveUri("data/protection_units.json");
    public Uri ProtectionAttackTypes => ResolveUri("data/protection_attack_types.json");
    public Uri ProtectionAttributes => ResolveUri("data/protection_attributes.json");
    
    public Uri Forces => ResolveUri("data/forces.json");
    public Uri TacticTypes => ResolveUri("data/tactics_types.json");
    public Uri FieldBuildings => ResolveUri("data/field_buildings.json");
    
    public Uri LiveStream => ResolveUri("data/livestream.json");
    
    private readonly bool _useContentDeliveryNetwork = configuration.GetValue<bool>("StaticWebRootAssetsMapping:UseContentDeliveryNetwork");
    private readonly string? _contentDeliveryNetwork = configuration.GetValue<string?>("StaticWebRootAssetsMapping:ContentDeliveryNetwork");

    public Uri ResolveUri([StringSyntax(StringSyntaxAttribute.Uri)] string url)
    {
        if (!_useContentDeliveryNetwork || _contentDeliveryNetwork is null) return new Uri(url, UriKind.Relative);
        var cdn = new Uri(_contentDeliveryNetwork, UriKind.Absolute);
        return new Uri(cdn, url);
    }
}