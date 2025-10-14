using System.Diagnostics.CodeAnalysis;

namespace SlimeIMWiki.Services;

public class StaticWebRootAssetsMapping(IConfiguration configuration)
{
    public Uri BattleUnits => GenerateUri("data/battle_units.json");
    public Uri BattleAttackTypes => GenerateUri("data/battle_attack_types.json");
    public Uri BattleAttributes => GenerateUri("data/battle_attributes.json");
    
    public Uri ProtectionUnits => GenerateUri("data/protection_units.json");
    public Uri ProtectionAttackTypes => GenerateUri("data/protection_attack_types.json");
    public Uri ProtectionAttributes => GenerateUri("data/protection_attributes.json");
    
    public Uri Forces => GenerateUri("data/forces.json");
    public Uri TacticTypes => GenerateUri("data/tactics_types.json");
    public Uri FieldBuildings => GenerateUri("data/field_buildings.json");
    
    private readonly bool _useContentDeliveryNetwork = configuration.GetValue<bool>("StaticWebRootAssetsMapping:UseContentDeliveryNetwork");
    private readonly string? _contentDeliveryNetwork = configuration.GetValue<string?>("StaticWebRootAssetsMapping:ContentDeliveryNetwork");

    private Uri GenerateUri([StringSyntax(StringSyntaxAttribute.Uri)] string url)
    {
        if (!_useContentDeliveryNetwork || _contentDeliveryNetwork is null) return new Uri(url, UriKind.Relative);
        var cdn = new Uri(_contentDeliveryNetwork, UriKind.Absolute);
        return new Uri(cdn, url);
    }
}