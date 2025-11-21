namespace SlimeIMWiki.Services;

public interface IWebStorageService
{
    ValueTask RegisterJavaScriptModuleAsync();
    
    string? GetFromCookie(string key);

    void SetToCookie(string key, string value, TimeSpan? expiration = null);
}