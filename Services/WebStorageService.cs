using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;

namespace SlimeIMWiki.Services;

public sealed class WebStorageService(IJSInProcessRuntime js) : IWebStorageService
{
    public string? GetFromCookie(string key)
    {
        return js.Invoke<string?>("Cookies.get", key);
    }
    
    public ValueTask<string?> GetFromCookieAsync(string key)
    {
        return js.InvokeAsync<string?>("Cookies.get", key);
    }

    public void SetToCookie(string key, string value, TimeSpan? expiration = null)
    {
        if (expiration is null)
        {
            js.Invoke<IJSVoidResult>("Cookies.set", key, value);
        }
        else
        {
            js.Invoke<IJSVoidResult>("Cookies.set", key, value, new { expires = expiration.GetValueOrDefault().TotalDays });
        }
    }
    
    public ValueTask SetToCookieAsync(string key, string value, TimeSpan? expiration = null)
    {
        return expiration is null ? js.InvokeVoidAsync("Cookies.set", key, value) : js.InvokeVoidAsync("Cookies.set", key, value, new { expires = expiration.GetValueOrDefault().TotalDays });
    }
}