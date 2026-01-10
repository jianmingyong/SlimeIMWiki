namespace SlimeIMWiki.Services;

public interface IWebStorageService
{
    string? GetFromCookie(string key);

    void SetToCookie(string key, string value, TimeSpan? expiration = null);
}