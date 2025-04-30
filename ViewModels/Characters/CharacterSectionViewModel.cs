using System.Reactive;
using System.Reactive.Disposables;
using ReactiveUI;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.Characters;

public sealed class CharacterSectionViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
    
    public IEnumerable<ICharacterUnit> CharacterUnits => _characterListService.CharacterUnits;

    public string DisplayCategory => _characterListService.DisplayCategory;

    public string CharacterUnitsCategory => _characterListService.OrderByCategory;
    
    public bool IsOrderByDescending => _characterListService.IsOrderByDescending;

    public ReactiveCommand<string, Unit> ChangeCharacterUnitsDisplayCategoryCommand => _characterListService.ChangeCharacterUnitsDisplayCategoryCommand;

    public ReactiveCommand<string, Unit> ChangeCharacterUnitsOrderByCategoryCommand => _characterListService.ChangeCharacterUnitsOrderByCategoryCommand;
    
    public ReactiveCommand<Unit, Unit> ChangeCharacterUnitsOrderCommand => _characterListService.ChangeCharacterUnitsOrderCommand;
    
    private readonly CharacterListService _characterListService;

    public CharacterSectionViewModel(CharacterListService characterListService)
    {
        _characterListService = characterListService;
        
        this.WhenActivated(disposable =>
        {
            this.WhenAnyValue(model => model._characterListService.CharacterUnits)
                .Subscribe(_ => this.RaisePropertyChanged(nameof(CharacterUnits)))
                .DisposeWith(disposable);
            
            this.WhenAnyValue(model => model._characterListService.DisplayCategory)
                .Subscribe(_ => this.RaisePropertyChanged(nameof(DisplayCategory)))
                .DisposeWith(disposable);
            
            this.WhenAnyValue(model => model._characterListService.OrderByCategory)
                .Subscribe(_ => this.RaisePropertyChanged(nameof(CharacterUnitsCategory)))
                .DisposeWith(disposable);
            
            this.WhenAnyValue(model => model._characterListService.IsOrderByDescending)
                .Subscribe(_ => this.RaisePropertyChanged(nameof(IsOrderByDescending)))
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