using System.Collections;
using Blazorise;

namespace SlimeIMWiki;

public record BootstrapThemeBackgroundOptions : ThemeBackgroundOptions, IEnumerable<KeyValuePair<string, Func<string>>>
{
#nullable disable
    public string BodySecondary { get; set; }

    private new Dictionary<string, Func<string>> ColorMap
    {
        get
        {
            return new Dictionary<string, Func<string>>
            {
                {
                    "primary",
                    (Func<string>) (() => Primary)
                },
                {
                    "secondary",
                    (Func<string>) (() => Secondary)
                },
                {
                    "success",
                    (Func<string>) (() => Success)
                },
                {
                    "info",
                    (Func<string>) (() => Info)
                },
                {
                    "warning",
                    (Func<string>) (() => Warning)
                },
                {
                    "danger",
                    (Func<string>) (() => Danger)
                },
                {
                    "light",
                    (Func<string>) (() => Light)
                },
                {
                    "dark",
                    (Func<string>) (() => Dark)
                },
                {
                    "body-secondary",
                    (Func<string>) (() => BodySecondary)
                },
                {
                    "body",
                    (Func<string>) (() => Body)
                },
                {
                    "muted",
                    (Func<string>) (() => Muted)
                }
            };
        }
    }
    
    public new IEnumerator<KeyValuePair<string, Func<string>>> GetEnumerator()
    {
        return ColorMap.GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator() => ColorMap.GetEnumerator();
    
    public new Func<string> this[string key] => ColorMap[key];
}