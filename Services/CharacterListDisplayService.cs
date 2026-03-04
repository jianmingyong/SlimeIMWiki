using DynamicData;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;

namespace SlimeIMWiki.Services;

public sealed partial class CharacterListDisplayService : ReactiveObject
{
    public ISourceCache<Filter, string> FilterCache => _filterCache;

    [Reactive]
    private CharacterListDisplayCategory _displayCategory = CharacterListDisplayCategory.Battle;

    [Reactive]
    private CharacterListDisplayOrder _displayOrder = CharacterListDisplayOrder.Release;

    [Reactive]
    private bool _isOrderByDescending = true;

    [Reactive]
    private bool _isOrFilter = true;

    [Reactive]
    private float _scrollPosition;

    private readonly SourceCache<Filter, string> _filterCache = new(filter => filter.Key);
}