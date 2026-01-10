using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SlimeIMWiki.Services;

public class WebApplicationService : IWebApplicationService
{
    public bool IsOnline => _isOnline.Value;
    
    private readonly BehaviorSubject<bool> _isOnline = new(true);

    public IObservable<bool> GetIsOnlineAsObservable()
    {
        return _isOnline.AsObservable();
    }
    
    public void SetIsOnline(bool value)
    {
        _isOnline.OnNext(value);
    }
}