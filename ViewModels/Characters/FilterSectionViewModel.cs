using System.Reactive.Disposables.Fluent;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using SlimeIMWiki.Models;
using SlimeIMWiki.Services;

namespace SlimeIMWiki.ViewModels.Characters;

public sealed partial class FilterSectionViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    public IEnumerable<Filter> Filters => _characterListService.Filters;

    public bool IsOrFilter
    {
        get => _characterListService.IsOrFilter;
        set => _characterListService.IsOrFilter = value;
    }

    public IEnumerable<Force> Forces => _jsonDataModelService.Forces ?? [];

    [ObservableAsProperty(ReadOnly = false)]
    private IEnumerable<IAttackType> _attackTypes = [];

    [ObservableAsProperty(ReadOnly = false)]
    private IEnumerable<IAttribute> _attributes = [];

    private readonly CharacterListService _characterListService;
    private readonly JsonDataModelService _jsonDataModelService;

    public FilterSectionViewModel(CharacterListService characterListService, JsonDataModelService jsonDataModelService)
    {
        _characterListService = characterListService;
        _jsonDataModelService = jsonDataModelService;

        this.WhenActivated(disposable =>
        {
            characterListService.Filters.ToObservableChangeSet()
                .Subscribe(_ => this.RaisePropertyChanged(nameof(Filters)))
                .DisposeWith(disposable);

            this.WhenAnyValue(model => model._jsonDataModelService.Forces)
                .Subscribe(_ => this.RaisePropertyChanged(nameof(Forces)))
                .DisposeWith(disposable);

            this.WhenAnyValue(
                    model => model._characterListService.DisplayCategory,
                    model => model._jsonDataModelService.BattleAttackTypes,
                    model => model._jsonDataModelService.ProtectionAttackTypes,
                    (displayCategory, battleAttackType, protectionAttackType) =>
                    {
                        return displayCategory switch
                        {
                            "Battle" => battleAttackType?.Cast<IAttackType>(),
                            "Protection" => protectionAttackType?.Cast<IAttackType>(),
                            var _ => throw new ArgumentOutOfRangeException(nameof(displayCategory), displayCategory, null)
                        };
                    })
                .WhereNotNull()
                .ToProperty(this, nameof(AttackTypes), out _attackTypesHelper)
                .DisposeWith(disposable);

            this.WhenAnyValue(
                    model => model._characterListService.DisplayCategory,
                    model => model._jsonDataModelService.BattleAttributes,
                    model => model._jsonDataModelService.ProtectionAttributes,
                    (displayCategory, battleAttribute, protectionAttribute) =>
                    {
                        return displayCategory switch
                        {
                            "Battle" => battleAttribute?.Cast<IAttribute>(),
                            "Protection" => protectionAttribute?.Cast<IAttribute>(),
                            var _ => throw new ArgumentOutOfRangeException(nameof(displayCategory), displayCategory, null)
                        };
                    })
                .WhereNotNull()
                .ToProperty(this, nameof(Attributes), out _attributesHelper)
                .DisposeWith(disposable);
        });
    }
}