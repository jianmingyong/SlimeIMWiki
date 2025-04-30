using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.Home;

public sealed partial class TimersViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    public partial string TimerSelection { get; private set; }
    
    [Reactive]
    public partial DateTime TimerReset { get; private set; }
    
    [Reactive]
    public partial DateTime TimerUpdate { get; private set; }
    
    [Reactive]
    public partial TimeSpan TimerResetIn { get; private set; }
    
    [Reactive]
    public partial TimeSpan TimerUpdateIn { get; private set; }
    
    private readonly IStorageService _storageService;

    public TimersViewModel(IStorageService storageService)
    {
        _storageService = storageService;

        _timerSelection = "NA";
        
        _timerReset = DateTime.Now;
        _timerUpdate = DateTime.Now;
        
        _timerResetIn = TimeSpan.Zero;
        _timerUpdateIn = TimeSpan.Zero;
        
        this.WhenActivated(disposable =>
        {
            Observable.FromAsync(WhenActivatedAsync, RxApp.MainThreadScheduler).Subscribe().DisposeWith(disposable);
            
            Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ =>
            {
                while (DateTime.Now > TimerReset)
                {
                    TimerReset = TimerReset.AddDays(1);
                }

                while (DateTime.Now > TimerUpdate)
                {
                    TimerUpdate = TimerUpdate.AddDays(1);
                }
                
                TimerResetIn = TimerReset - DateTime.Now;
                TimerUpdateIn = TimerUpdate - DateTime.Now;
            }).DisposeWith(disposable);
        });
    }

    private async Task WhenActivatedAsync()
    {
        _timerSelection = await _storageService.GetFromCookieAsync(nameof(TimerSelection)) ?? "NA";
        await RegionChange(_timerSelection);
    }
    
    [ReactiveCommand]
    private async Task RegionChange(string region)
    {
        TimerSelection = region;
        
        await _storageService.SetToCookieAsync(nameof(TimerSelection), region, TimeSpan.FromDays(30));
        
        switch (region)
        {
            case "NA":
            {
                TimerReset = new DateTimeOffset(DateTimeOffset.UtcNow.Year, DateTimeOffset.UtcNow.Month, DateTimeOffset.UtcNow.Day, 19, 0, 0, TimeSpan.FromHours(8)).LocalDateTime;
                TimerUpdate = new DateTimeOffset(DateTimeOffset.UtcNow.Year, DateTimeOffset.UtcNow.Month, DateTimeOffset.UtcNow.Day, 10, 0, 0, TimeSpan.FromHours(8)).LocalDateTime;
                break;
            }

            case "EU":
            {
                TimerReset = new DateTimeOffset(DateTimeOffset.UtcNow.Year, DateTimeOffset.UtcNow.Month, DateTimeOffset.UtcNow.Day, 12, 0, 0, TimeSpan.FromHours(8)).LocalDateTime;
                TimerUpdate = new DateTimeOffset(DateTimeOffset.UtcNow.Year, DateTimeOffset.UtcNow.Month, DateTimeOffset.UtcNow.Day, 10, 0, 0, TimeSpan.FromHours(8)).LocalDateTime;
                break;
            }

            case "Asia":
            {
                TimerReset = new DateTimeOffset(DateTimeOffset.UtcNow.Year, DateTimeOffset.UtcNow.Month, DateTimeOffset.UtcNow.Day, 3, 0, 0, TimeSpan.FromHours(8)).LocalDateTime;
                TimerUpdate = new DateTimeOffset(DateTimeOffset.UtcNow.Year, DateTimeOffset.UtcNow.Month, DateTimeOffset.UtcNow.Day, 10, 0, 0, TimeSpan.FromHours(8)).LocalDateTime;
                break;
            }
        }

        while (DateTime.Now > TimerReset)
        {
            TimerReset = TimerReset.AddDays(1);
        }

        while (DateTime.Now > TimerUpdate)
        {
            TimerUpdate = TimerUpdate.AddDays(1);
        }

        TimerResetIn = TimerReset - DateTime.Now;
        TimerUpdateIn = TimerUpdate - DateTime.Now;
    }
}