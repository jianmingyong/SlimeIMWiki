using System.Net.Http.Json;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace SlimeIMWiki.ViewModels.Home;

public sealed partial class LiveStreamViewModel : ReactiveObject
{
    [ObservableAsProperty]
    private string? _livestreamUrl;

    public LiveStreamViewModel(HttpClient httpClient)
    {
        Observable.FromAsync(token => httpClient.GetFromJsonAsync("data/livestream.json", JsonSerializer.Custom.Livestream, token)).WhereNotNull().Select(livestream => livestream.YoutubeLink).ToProperty(this, nameof(LivestreamUrl), out _livestreamUrlHelper);
    }
}