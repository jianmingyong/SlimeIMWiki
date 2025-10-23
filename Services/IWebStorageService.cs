namespace SlimeIMWiki.Services;

public interface IWebStorageService
{
    string? GetFromCookie(string key);
    
    ValueTask<string?> GetFromCookieAsync(string key);

    void SetToCookie(string key, string value, TimeSpan? expiration = null);
    
    ValueTask SetToCookieAsync(string key, string value, TimeSpan? expiration = null);
}