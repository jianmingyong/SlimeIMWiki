using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models.JsonData;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Components.Home;

public sealed partial class LiveStreamViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [ObservableAsProperty(ReadOnly = false)]
    private string? _livestreamSource;

    public LiveStreamViewModel(JsonDataModelService jsonDataModelService)
    {
        this.WhenActivated(disposable =>
        {
            jsonDataModelService
                .GetObservableLivestream()
                .Catch(Observable.Return<Livestream?>(null))
                .Select(livestream => livestream?.YoutubeLink)
                .ToProperty(this, nameof(LivestreamSource), out _livestreamSourceHelper)
                .DisposeWith(disposable);
        });
    }
}