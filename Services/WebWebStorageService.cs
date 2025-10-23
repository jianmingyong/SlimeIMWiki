using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;

namespace SlimeIMWiki.Services;

public sealed class WebWebStorageService(IJSRuntime jsRuntime) : IWebStorageService
{
    private readonly IJSInProcessRuntime _jsRuntime = (IJSInProcessRuntime) jsRuntime;
    
    public string? GetFromCookie(string key)
    {
        return _jsRuntime.Invoke<string?>("Cookies.get", key);
    }
    
    public ValueTask<string?> GetFromCookieAsync(string key)
    {
        return jsRuntime.InvokeAsync<string?>("Cookies.get", key);
    }

    public void SetToCookie(string key, string value, TimeSpan? expiration = null)
    {
        if (expiration is null)
        {
            _jsRuntime.Invoke<IJSVoidResult>("Cookies.set", key, value);
        }
        else
        {
            _jsRuntime.Invoke<IJSVoidResult>("Cookies.set", key, value, new { expires = expiration.GetValueOrDefault().TotalDays });
        }
    }
    
    public ValueTask SetToCookieAsync(string key, string value, TimeSpan? expiration = null)
    {
        return expiration is null ? jsRuntime.InvokeVoidAsync("Cookies.set", key, value) : jsRuntime.InvokeVoidAsync("Cookies.set", key, value, new { expires = expiration.GetValueOrDefault().TotalDays });
    }
}