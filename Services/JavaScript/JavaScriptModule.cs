using Microsoft.JSInterop;

namespace SlimeIMWiki.Services.JavaScript;

public abstract class JavaScriptModule(IJSInProcessRuntime js) : IEagerRegisterJavaScriptModule, IDisposable, IAsyncDisposable
{
    protected abstract string ModuleFile { get; }

    private IJSInProcessObjectReference? _module;

    public async ValueTask RegisterJavaScriptModuleAsync()
    {
        await GetModuleAsync();
    }

    protected IJSInProcessObjectReference GetModule()
    {
        return _module ?? throw new InvalidOperationException("Module must be registered before calling module functions.");
    }

    protected virtual ValueTask OnInitializedModule(IJSInProcessObjectReference module)
    {
        return ValueTask.CompletedTask;
    }

    private async ValueTask<IJSInProcessObjectReference> GetModuleAsync()
    {
        return _module ??= await InitializeModule();

        async Task<IJSInProcessObjectReference> InitializeModule()
        {
            var module = await js.InvokeAsync<IJSInProcessObjectReference>("import", ModuleFile);
            await OnInitializedModule(module);
            return module;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_module != null) await _module.DisposeAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _module?.Dispose();
        }
    }
}