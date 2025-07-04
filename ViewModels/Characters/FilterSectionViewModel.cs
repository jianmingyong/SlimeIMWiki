﻿using System.Reactive.Disposables;
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

    [ObservableAsProperty]
    private IEnumerable<IAttackType> _attackTypes = [];
    
    [ObservableAsProperty]
    private IEnumerable<IAttribute> _attributes = [];
    
    private readonly CharacterListService _characterListService;
    private readonly JsonDataModelService _jsonDataModelService;

    public FilterSectionViewModel(CharacterListService characterListService, JsonDataModelService jsonDataModelService)
    {
        _characterListService = characterListService;
        _jsonDataModelService = jsonDataModelService;

        _attackTypesHelper = this.WhenAnyValue(
            model => model._characterListService.DisplayCategory,
            model => model._jsonDataModelService.BattleAttackTypes,
            model => model._jsonDataModelService.ProtectionAttackTypes,
            (displayCategory, battleAttackType, protectionAttackType) =>
            {
                return displayCategory switch
                {
                    "Battle" => battleAttackType.Cast<IAttackType>(),
                    "Protection" => protectionAttackType.Cast<IAttackType>(),
                    var _ => throw new ArgumentOutOfRangeException(nameof(displayCategory), displayCategory, null)
                };
            }).ToProperty(this, nameof(AttackTypes));

        _attributesHelper = this.WhenAnyValue(
            model => model._characterListService.DisplayCategory,
            model => model._jsonDataModelService.BattleAttributes,
            model => model._jsonDataModelService.ProtectionAttributes,
            (displayCategory, battleAttribute, protectionAttribute) =>
            {
                return displayCategory switch
                {
                    "Battle" => battleAttribute.Cast<IAttribute>(),
                    "Protection" => protectionAttribute.Cast<IAttribute>(),
                    var _ => throw new ArgumentOutOfRangeException(nameof(displayCategory), displayCategory, null)
                };
            }).ToProperty(this, nameof(Attributes));

        this.WhenActivated(disposable =>
        {
            characterListService.Filters.ToObservableChangeSet()
                .Subscribe(_ => this.RaisePropertyChanged(nameof(Filters)))
                .DisposeWith(disposable);
        });
    }
}