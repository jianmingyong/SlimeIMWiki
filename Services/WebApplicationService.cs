using Microsoft.JSInterop;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace SlimeIMWiki.Services;

public sealed partial class WebApplicationService(IJSRuntime jsRuntime) : ReactiveObject, IDisposable, IAsyncDisposable
{
    private readonly IJSInProcessRuntime _jsRuntime = (IJSInProcessRuntime) jsRuntime;
    private IJSInProcessObjectReference? _module;

    [Reactive]
    private bool _isOnline = true;

    public async Task RegisterService()
    {
        _module = await _jsRuntime.InvokeAsync<IJSInProcessObjectReference?>("import", "./js/web-application-service.js");
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