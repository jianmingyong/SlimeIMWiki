using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.JSInterop;

namespace SlimeIMWiki.Services;

[method: DynamicDependency(nameof(SetIsOnline))]
public class WebApplicationService(IJSInProcessRuntime js) : BaseJavaScriptModule(js), IWebApplicationService
{
    public IObservable<bool> IsOnline => _isOnline.AsObservable();

    protected override string ModuleFile => "./js/web-application-service.js";

    private BehaviorSubject<bool> _isOnline { get; } = new(false);

    [JSInvokable]
    public void SetIsOnline(bool value)
    {
        _isOnline.OnNext(value);
    }

    protected override ValueTask OnInitializedModule(IJSInProcessObjectReference module)
    {
        _isOnline.OnNext(module.Invoke<bool>("isOnline"));
        module.InvokeVoid("registerEventListener", DotNetObjectReference.Create(this));
        return ValueTask.CompletedTask;
    }
}