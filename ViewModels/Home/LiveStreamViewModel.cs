﻿using System.Reactive.Disposables.Fluent;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.Home;

public sealed partial class LiveStreamViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
    
    private readonly WebApplicationService _webApplicationService;
    
    [ObservableAsProperty(ReadOnly = false)]
    private bool _isOnline;
    
    [ObservableAsProperty(ReadOnly = false)]
    private Livestream? _livestream;

    public LiveStreamViewModel(JsonDataModelService jsonDataModelService, WebApplicationService webApplicationService)
    {
        _webApplicationService = webApplicationService;
        
        this.WhenActivated(disposable =>
        {
            jsonDataModelService
                .GetLivestream()
                .ToProperty(this, nameof(Livestream), out _livestreamHelper)
                .DisposeWith(disposable);
            
            this.WhenAnyValue(model => model._webApplicationService.IsOnline)
                .ToProperty(this, nameof(IsOnline), out _isOnlineHelper)
                .DisposeWith(disposable);
        });
    }
}