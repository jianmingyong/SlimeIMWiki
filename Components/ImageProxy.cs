using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Components;

public class ImageProxy : Blazorise.Image
{
    [Inject]
    public required StaticWebRootAssets StaticWebRootAssets { get; set; }
    
    protected override void BuildRenderTree(RenderTreeBuilder __builder)
    {
        __builder.OpenElement(0, "img");
        __builder.AddAttribute(1, "id", ElementId);
        __builder.AddAttribute(2, "class", ClassNames);
        __builder.AddAttribute(3, "style", StyleNames);
        __builder.AddAttribute(4, "src", Source is not null ? StaticWebRootAssets.ResolveUri(Source).ToString() : null);
        __builder.AddAttribute(5, "alt", Text);
        __builder.AddAttribute(6, "loading", LoadingString);
        __builder.AddMultipleAttributes(7, RuntimeHelpers.TypeCheck<IEnumerable<KeyValuePair<string, object>>>(Attributes));
        __builder.AddElementReferenceCapture(8, __value => ElementRef = __value);
        __builder.CloseElement();
    }
}