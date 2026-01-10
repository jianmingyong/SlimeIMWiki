using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.JavaScript;
using SlimeIMWiki.Services;

namespace SlimeIMWiki;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public partial class GlobalJsExport
{
    public static IServiceProvider? ServiceProvider { get; set; }

    [JSExport]
    private static void SetIsOnline(bool value)
    {
        (ServiceProvider?.GetRequiredService<IWebApplicationService>() as WebApplicationService)?.SetIsOnline(value);
    }
}