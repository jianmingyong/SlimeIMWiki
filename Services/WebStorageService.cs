using Microsoft.JSInterop;

namespace SlimeIMWiki.Services;

public class WebStorageService(IJSInProcessRuntime js) : IWebStorageService
{
    public string? GetFromCookie(string key)
    {
        return js.Invoke<string?>("Cookies.get", key);
    }

    public void SetToCookie(string key, string value, TimeSpan? expiration = null)
    {
        if (expiration is null)
        {
            js.InvokeVoid("Cookies.set", key, value);
        }
        else
        {
            js.InvokeVoid("Cookies.set", key, value, new { expires = expiration.GetValueOrDefault().TotalDays });
        }
    }
}