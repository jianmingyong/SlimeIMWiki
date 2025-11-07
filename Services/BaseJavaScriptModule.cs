using Microsoft.JSInterop;

namespace SlimeIMWiki.Services;

public abstract class BaseJavaScriptModule(IJSInProcessRuntime js) : IEagerLoadJavaScriptModule, IDisposable, IAsyncDisposable
{
    protected abstract string ModuleFile { get; }

    private IJSInProcessObjectReference? _module;

    public async ValueTask EagerLoadJavaScriptModule()
    {
        await GetModule();
    }

    protected async ValueTask<IJSInProcessObjectReference> GetModule()
    {
        return _module ??= await InitializeModule();

        async Task<IJSInProcessObjectReference> InitializeModule()
        {
            var module = await js.InvokeAsync<IJSInProcessObjectReference>("import", ModuleFile);
            await OnInitializedModule(module);
            return module;
        }
    }

    protected virtual ValueTask OnInitializedModule(IJSInProcessObjectReference module)
    {
        return ValueTask.CompletedTask;
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_module != null) await _module.DisposeAsync();
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

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _module?.Dispose();
        }
    }
}