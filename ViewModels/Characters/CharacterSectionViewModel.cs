using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.Characters;

public sealed class CharacterSectionViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
    
    public IEnumerable<ICharacterUnit> CharacterUnits => _characterListService.CharacterUnits;

    public string CharacterUnitsCategory => _characterListService.OrderByCategory;
    
    public bool IsOrderByDescending => _characterListService.IsOrderByDescending;

    public ReactiveCommand<string, Unit> ChangeCharacterUnitsOrderByCategoryCommand => _characterListService.ChangeCharacterUnitsOrderByCategoryCommand;
    
    public ReactiveCommand<Unit, Unit> ChangeCharacterUnitsOrderCommand => _characterListService.ChangeCharacterUnitsOrderCommand;
    
    private readonly CharacterListService _characterListService;

    public CharacterSectionViewModel(CharacterListService characterListService)
    {
        _characterListService = characterListService;
        
        this.WhenActivated(disposable =>
        {
            Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    eventHandler =>
                    {
                        return Handler;
                        void Handler(object? sender, PropertyChangedEventArgs e) => eventHandler(e);
                    },
                    eh => characterListService.PropertyChanged += eh,
                    eh => characterListService.PropertyChanged -= eh)
                .Subscribe(args => this.RaisePropertyChanged(args.PropertyName))
                .DisposeWith(disposable);
        });
    }
    
    public string DisplayText(ICharacterUnit unit)
    {
        return CharacterUnitsCategory switch
        {
            "Release" => unit.Name,
            "Health" => unit.MaxHealth.ToString("N0"),
            "Attack" => unit.MaxAttack.ToString("N0"),
            "Defense" => unit.MaxDefense.ToString("N0"),
            "Rarity" => unit.Name,
            "Name" => unit.Name,
            var _ => unit.Name
        };
    }
}