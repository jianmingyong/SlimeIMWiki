using Blazorise.Localization;
using Microsoft.AspNetCore.Components;

namespace SlimeIMWiki.Layouts;

public partial class MainLayout
{
    [Inject] protected ITextLocalizerService? LocalizationService { get; set; }

    [CascadingParameter] protected Theme? Theme { get; set; }

    protected string layoutType = "fixed-header";

    protected override async Task OnInitializedAsync()
    {
        await SelectCulture("en-US");

        await base.OnInitializedAsync();
    }

    private Task SelectCulture(string name)
    {
        LocalizationService!.ChangeLanguage(name);

        return Task.CompletedTask;
    }

    private Task OnThemeEnabledChanged(bool value)
    {
        if (Theme is null)
        {
            return Task.CompletedTask;
        }

        Theme.Enabled = value;

        return InvokeAsync(Theme.ThemeHasChanged);
    }

    private Task OnThemeGradientChanged(bool value)
    {
        if (Theme is null)
        {
            return Task.CompletedTask;
        }

        Theme.IsGradient = value;

        return InvokeAsync(Theme.ThemeHasChanged);
    }

    private Task OnThemeRoundedChanged(bool value)
    {
        if (Theme is null)
        {
            return Task.CompletedTask;
        }

        Theme.IsRounded = value;

        return InvokeAsync(Theme.ThemeHasChanged);
    }

    private Task OnThemeColorChanged(string value)
    {
        if (Theme is null)
        {
            return Task.CompletedTask;
        }

        Theme.ColorOptions ??= new ThemeColorOptions();

        Theme.BackgroundOptions ??= new ThemeBackgroundOptions();

        Theme.TextColorOptions ??= new ThemeTextColorOptions();

        Theme.ColorOptions.Primary = value;
        Theme.BackgroundOptions.Primary = value;
        Theme.TextColorOptions.Primary = value;

        Theme.InputOptions ??= new ThemeInputOptions();

        Theme.InputOptions.CheckColor = value;
        Theme.InputOptions.SliderColor = value;

        Theme.SpinKitOptions ??= new ThemeSpinKitOptions();

        Theme.SpinKitOptions.Color = value;

        return InvokeAsync(Theme.ThemeHasChanged);
    }
}