using System.Diagnostics;
using DynamicData;
using Microsoft.AspNetCore.Components;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.Components.Characters.Filters;

public class BaseFilterComponent : ComponentBase
{
    [Inject]
    public required CharacterListDisplayService CharacterListDisplayService { get; set; }

    [Parameter]
    public bool IsRemoveMode { get; set; }

    protected Filter? Filter { get; set; }

    protected void Execute()
    {
        Debug.Assert(Filter != null, nameof(Filter) + " != null");

        if (!IsRemoveMode)
        {
            CharacterListDisplayService.FilterCache.AddOrUpdate(Filter);
        }
        else
        {
            CharacterListDisplayService.FilterCache.RemoveKey(Filter.Key);
        }
    }
}