namespace SlimeIMWiki.Services;

public interface IWebApplicationService
{
    bool IsOnline { get; }
    
    IObservable<bool> GetIsOnlineAsObservable();
}