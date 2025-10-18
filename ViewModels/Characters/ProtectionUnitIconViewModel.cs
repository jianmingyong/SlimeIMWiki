using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ReactiveUI;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.Characters;

public sealed class ProtectionUnitIconViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
    
    private readonly JsonDataModelService _jsonDataModelService;

    public ProtectionUnitIconViewModel(JsonDataModelService jsonDataModelService)
    {
        _jsonDataModelService = jsonDataModelService;
        
        this.WhenActivated(disposable =>
        {
            this.WhenAnyValue(
                    model => model._jsonDataModelService.ProtectionAttributes,
                    model => model._jsonDataModelService.ProtectionAttackTypes,
                    model => model._jsonDataModelService.Forces)
                .Subscribe(_ => this.RaisePropertyChanged())
                .DisposeWith(disposable);
        });
    }

    public string? GetForceIcon(string force)
    {
        return _jsonDataModelService.GetForce(force)?.Icon;
    }

    public string? GetAttributeIcon(string attribute)
    {
        return _jsonDataModelService.GetProtectionAttribute(attribute)?.Icon;
    }
    
    public string? GetAttackTypeIcon(string attackType)
    {
        return _jsonDataModelService.GetProtectionAttackType(attackType)?.Icon;
    }
}