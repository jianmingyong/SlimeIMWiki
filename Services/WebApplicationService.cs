using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace SlimeIMWiki.Services;

[method: DynamicDependency(nameof(SetIsOnline))]
public sealed partial class WebApplicationService(IJSInProcessRuntime js) : ReactiveObject, IWebApplicationService, IDisposable, IAsyncDisposable
{
    private IJSInProcessObjectReference? _module;

    [Reactive(SetModifier = AccessModifier.Private)]
    private bool _isOnline = true;

    public async Task RegisterService()
    {
        _module = await js.InvokeAsync<IJSInProcessObjectReference?>("import", "./js/web-application-service.js");
        IsOnline = _module?.Invoke<bool>("isOnline") ?? true;
        _module?.InvokeVoid("registerEventListener", DotNetObjectReference.Create(this));
    }

    [JSInvokable]
    public void SetIsOnline(bool isOnline)
    {
        IsOnline = isOnline;
    }

    public void Dispose()
    {
        _module?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_module != null) await _module.DisposeAsync();
    }
}