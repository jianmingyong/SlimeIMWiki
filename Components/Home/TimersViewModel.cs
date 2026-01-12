using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Components.Home;

public sealed partial class TimersViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [Reactive(SetModifier = AccessModifier.Private)]
    private string _timerSelection;
    
    [Reactive(SetModifier = AccessModifier.Private)]
    private DateTime _timerReset = DateTime.Now;
    
    [Reactive(SetModifier = AccessModifier.Private)]
    private DateTime _timerUpdate = DateTime.Now;
    
    [Reactive(SetModifier = AccessModifier.Private)]
    private TimeSpan _timerResetIn = TimeSpan.Zero;
    
    [Reactive(SetModifier = AccessModifier.Private)]
    private TimeSpan _timerUpdateIn = TimeSpan.Zero;
    
    private readonly IWebStorageService _webStorageService;

    public TimersViewModel(IWebStorageService webStorageService)
    {
        _webStorageService = webStorageService;
        _timerSelection = webStorageService.GetFromCookie(nameof(TimerSelection)) ?? "NA";
        
        this.WhenActivated(disposable =>
        {
            RegionChange(_timerSelection);
            
            Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ =>
            {
                UpdateTimers();
            }).DisposeWith(disposable);
        });
    }
    
    [ReactiveCommand]
    private void RegionChange(string region)
    {
        TimerSelection = region;
        
        _webStorageService.SetToCookie(nameof(TimerSelection), region, TimeSpan.FromDays(30));
        
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

        UpdateTimers();
    }

    private void UpdateTimers()
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
    }
}