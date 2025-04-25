namespace SlimeIMWiki.Services;

public interface IStorageService
{
    ValueTask<string?> GetFromCookieAsync(string key);

    ValueTask SetToCookieAsync(string key, string value, TimeSpan? expiration = null);
}