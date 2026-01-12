using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;

namespace SlimeIMWiki.Components;

public partial class NetworkStatusProviderViewModel : ReactiveObject, INetworkStatus
{
    [Reactive]
    private bool _isOnline = true;

    [DynamicDependency(nameof(SetIsOnline))]
    public NetworkStatusProviderViewModel()
    {
    }

    [JSInvokable]
    public void SetIsOnline(bool value)
    {
        IsOnline = value;
    }
}