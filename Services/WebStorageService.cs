using Microsoft.JSInterop;

namespace SlimeIMWiki.Services;

public sealed class WebStorageService(IJSInProcessRuntime js) : BaseJavaScriptModule(js), IWebStorageService
{
    protected override string ModuleFile => "https://cdn.jsdelivr.net/npm/js-cookie@3.0.5/+esm";

    public async ValueTask<string?> GetFromCookie(string key)
    {
        return (await GetModule()).Invoke<string?>("default.get", key);
    }

    public async ValueTask SetToCookie(string key, string value, TimeSpan? expiration = null)
    {
        if (expiration is null)
        {
            (await GetModule()).InvokeVoid("default.set", key, value);
        }
        else
        {
            (await GetModule()).InvokeVoid("default.set", key, value, new { expires = expiration.GetValueOrDefault().TotalDays });
        }
    }
}