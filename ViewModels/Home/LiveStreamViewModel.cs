using System.Net.Http.Json;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace SlimeIMWiki.ViewModels.Home;

public sealed partial class LiveStreamViewModel : ReactiveObject
{
    [ObservableAsProperty]
    public partial string? LivestreamUrl { get; }

    public LiveStreamViewModel(HttpClient httpClient)
    {
        _livestreamUrlHelper = Observable.FromAsync(token => httpClient.GetFromJsonAsync("data/livestream.json", JsonSerializer.Custom.Livestream, token)).WhereNotNull().Select(livestream => livestream.YoutubeLink).ToProperty(this, nameof(LivestreamUrl));
    }
}