using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.Home;

public sealed partial class LiveStreamViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
    
    [ObservableAsProperty(ReadOnly = false)]
    private Livestream? _livestream;

    public LiveStreamViewModel(JsonDataModelService jsonDataModelService)
    {
        this.WhenActivated(disposable =>
        {
            jsonDataModelService
                .GetLivestream()
                .ToProperty(this, nameof(Livestream), out _livestreamHelper)
                .DisposeWith(disposable);
        });
    }
}