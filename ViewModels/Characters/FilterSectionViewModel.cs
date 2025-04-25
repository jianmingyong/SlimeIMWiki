using System.Reactive.Disposables;
using DynamicData.Binding;
using ReactiveUI;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.Characters;

public sealed class FilterSectionViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    public IReadOnlyList<Filter> Filters => _characterListService.Filters;
    
    private readonly CharacterListService _characterListService;
    
    public FilterSectionViewModel(CharacterListService characterListService)
    {
        _characterListService = characterListService;
        
        this.WhenActivated(disposable =>
        {
            characterListService.Filters.ToObservableChangeSet()
                .Subscribe(_ => this.RaisePropertyChanged())
                .DisposeWith(disposable);
        });
    }
}