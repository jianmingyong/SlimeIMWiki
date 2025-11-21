using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.JSInterop;

namespace SlimeIMWiki.Services.JavaScript;

[method: DynamicDependency(nameof(SetIsOnline))]
public class WebApplicationService(IJSInProcessRuntime js) : JavaScriptModule(js), IWebApplicationService
{
    public IObservable<bool> IsOnline => _isOnline.AsObservable();

    protected override string ModuleFile => "./js/web-application-service.js";
    
    private readonly IJSInProcessRuntime _js = js;
    private readonly BehaviorSubject<bool> _isOnline = new(false);

    [JSInvokable]
    public void SetIsOnline(bool value)
    {
        _isOnline.OnNext(value);
    }

    protected override ValueTask OnInitializedModule(IJSInProcessObjectReference module)
    {
        _isOnline.OnNext(_js.GetValue<bool>("navigator.onLine"));
        module.InvokeVoid("registerEventListener", DotNetObjectReference.Create(this));
        return ValueTask.CompletedTask;
    }
}