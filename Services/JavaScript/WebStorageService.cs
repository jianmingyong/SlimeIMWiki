using Microsoft.JSInterop;

namespace SlimeIMWiki.Services.JavaScript;

public sealed class WebStorageService(IJSInProcessRuntime js) : JavaScriptModule(js), IWebStorageService
{
    protected override string ModuleFile => "https://cdn.jsdelivr.net/npm/js-cookie@3.0.5/+esm";

    public string? GetFromCookie(string key)
    {
        return GetModule().Invoke<string?>("default.get", key);
    }

    public void SetToCookie(string key, string value, TimeSpan? expiration = null)
    {
        if (expiration is null)
        {
            GetModule().InvokeVoid("default.set", key, value);
        }
        else
        {
            GetModule().InvokeVoid("default.set", key, value, new { expires = expiration.GetValueOrDefault().TotalDays });
        }
    }
}