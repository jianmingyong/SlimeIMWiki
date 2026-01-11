using System.Text.RegularExpressions;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Match = System.Text.RegularExpressions.Match;

namespace SlimeIMWiki.Components.CharacterDetail;

public partial class EffectStyler : BaseComponent
{
    [EditorRequired]
    [Parameter]
    public string Text { get; set; }

    [GeneratedRegex(@"(\d*\.?\d+%)|(x\d*\.?\d+)|(Max: \d*\.?\d+%)")]
    private static partial Regex PercentageDigitRegex();

    private static RenderFragment BuildTextContent(int sequence, string text)
    {
        return builder => builder.AddContent(sequence, text);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var texts = Text.Split('\n');
        var sequenceId = 0;

        foreach (var text in texts)
        {
            var matches = PercentageDigitRegex().Matches(text);
            var currentIndex = 0;

            foreach (Match match in matches)
            {
                if (match.Groups[1].Success)
                {
                    builder.OpenComponent<Text>(sequenceId++);
                    builder.AddComponentParameter(sequenceId++, nameof(Blazorise.Text.ChildContent), BuildTextContent(sequenceId++, text[currentIndex..match.Index]));
                    builder.CloseComponent();
                
                    builder.OpenComponent<Text>(sequenceId++);
                    builder.AddComponentParameter(sequenceId++, nameof(Style), "color: #00D100");
                    builder.AddComponentParameter(sequenceId++, nameof(Blazorise.Text.ChildContent), BuildTextContent(sequenceId++, text[new Range(match.Index, match.Index + match.Length)]));
                    builder.CloseComponent();
                }
                else if (match.Groups[2].Success)
                {
                    var group = match.Groups[2];
                    
                    builder.OpenComponent<Text>(sequenceId++);
                    builder.AddComponentParameter(sequenceId++, nameof(Blazorise.Text.ChildContent), BuildTextContent(sequenceId++, text[currentIndex..(group.Index + 1)]));
                    builder.CloseComponent();
                    
                    builder.OpenComponent<Text>(sequenceId++);
                    builder.AddComponentParameter(sequenceId++, nameof(Style), "color: #00D100");
                    builder.AddComponentParameter(sequenceId++, nameof(Blazorise.Text.ChildContent), BuildTextContent(sequenceId++, text[new Range(group.Index + 1, group.Index + group.Length)]));
                    builder.CloseComponent();
                }
                else
                {
                    continue;
                }
                
                currentIndex = match.Index + match.Length;
            }

            if (!string.IsNullOrEmpty(text[currentIndex..]))
            {
                builder.OpenComponent<Text>(sequenceId++);
                builder.AddComponentParameter(sequenceId++, nameof(Blazorise.Text.ChildContent), BuildTextContent(sequenceId++, text[currentIndex..]));
                builder.CloseComponent();
            }
            
            builder.OpenElement(sequenceId++, "div");
            builder.AddContent(sequenceId++, Environment.NewLine);
            builder.CloseElement();
        }
    }
}