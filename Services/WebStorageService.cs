using Microsoft.JSInterop;

namespace SlimeIMWiki.Services;

public sealed class WebStorageService(IJSRuntime jsRuntime) : IStorageService
{
    public ValueTask<string?> GetFromCookieAsync(string key)
    {
        return jsRuntime.InvokeAsync<string?>("Cookies.get", key);
    }

    public ValueTask SetToCookieAsync(string key, string value, TimeSpan? expiration = null)
    {
        return expiration is null ? jsRuntime.InvokeVoidAsync("Cookies.set", key, value) : jsRuntime.InvokeVoidAsync("Cookies.set", key, value, new { expires = expiration.GetValueOrDefault().TotalDays });
    }
}