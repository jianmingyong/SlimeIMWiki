using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Components.Characters.Filters;

public class BaseFilterComponent : ComponentBase
{
    [Inject]
    public required CharacterListService CharacterListService { get; set; }
    
    [Parameter]
    public bool IsRemoveMode { get; set; }

    [Parameter]
    public Filter? Filter { get; set; }

    protected void Execute()
    {
        Debug.Assert(Filter != null, nameof(Filter) + " != null");

        if (!IsRemoveMode)
        {
            if (!CharacterListService.Filters.Contains(Filter))
            {
                CharacterListService.Filters.Add(Filter);
            }
        }
        else
        {
            CharacterListService.Filters.Remove(Filter);
        }
    }
}