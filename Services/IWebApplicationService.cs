namespace SlimeIMWiki.Services;

public interface IWebApplicationService
{
    bool IsOnline { get; }

    Task RegisterService();
}