namespace SlimeIMWiki.Services;

public interface IWebApplicationService
{
    IObservable<bool> IsOnline { get; }
}