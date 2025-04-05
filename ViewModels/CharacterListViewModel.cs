using Microsoft.AspNetCore.Components;
using SlimeIMWiki.Models;

namespace SlimeIMWiki.ViewModels;

public record Filter(string Name, Func<ICharacterUnit, bool> FilterFunction, RenderFragment RemoveRenderFragment);

public sealed class CharacterListViewModel
{
    public event EventHandler? IsUpdated;
    
    public string SortBy { get; set; } = "Release";

    public bool SortDescending { get; set; } = true;

    public List<Filter> Filters { get; private set; } = [];

    public bool IsOrFilter { get; set; } = true;
}