using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.JSInterop;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace SlimeIMWiki.ViewModels.Home;

public partial class TimersViewModel : ReactiveObject, IActivatableViewModel
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
    
    private readonly IJSRuntime _jsRuntime;
    
    public TimersViewModel(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        
        _timerSelection = "NA";
        
        _timerReset = DateTime.Now;
        _timerUpdate = DateTime.Now;
        
        _timerResetIn = TimeSpan.Zero;
        _timerUpdateIn = TimeSpan.Zero;
        
        this.WhenActivated(disposable =>
        {
            Observable.StartAsync(WhenActivatedAsync, RxApp.MainThreadScheduler).Subscribe().DisposeWith(disposable);
            
            Observable.Interval(TimeSpan.FromSeconds(1), RxApp.TaskpoolScheduler).Subscribe(_ =>
            {
                TimerResetIn = TimerReset - DateTime.Now;
                TimerUpdateIn = TimerUpdate - DateTime.Now;
            }).DisposeWith(disposable);
        });
    }

    public async Task SetTimerSelection(string selection)
    {
        TimerSelection = selection;

        await _jsRuntime.InvokeVoidAsync("Cookies.set", nameof(TimerSelection), selection, new { expires = 30 });

        switch (selection)
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

        if (TimerReset - DateTime.Now < TimeSpan.Zero)
        {
            TimerReset = TimerReset.AddDays(1);
        }

        if (TimerUpdate - DateTime.Now < TimeSpan.Zero)
        {
            TimerUpdate = TimerUpdate.AddDays(1);
        }

        TimerResetIn = TimerReset - DateTime.Now;
        TimerUpdateIn = TimerUpdate - DateTime.Now;
    }

    private async Task WhenActivatedAsync()
    {
        _timerSelection = await _jsRuntime.InvokeAsync<string?>("Cookies.get", nameof(TimerSelection)) ?? "NA";
        await SetTimerSelection(_timerSelection);
    }
}