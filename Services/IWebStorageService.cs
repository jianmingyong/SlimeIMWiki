namespace SlimeIMWiki.Services;

public interface IWebStorageService
{
    ValueTask<string?> GetFromCookie(string key);

    ValueTask SetToCookie(string key, string value, TimeSpan? expiration = null);
}