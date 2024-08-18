using System;
using System.Collections.Generic;
using R3;
using Zenject;
using Object = UnityEngine.Object;

namespace Runtime.Gameplay.Buildings.UI
{
    public class BuildingTooltipPresenter : IInitializable, IDisposable
    {
        private readonly BuildingsToolbarPresenter _toolbar;
        private readonly BuildingTooltipView _view;
        private readonly BuildingTooltipConfig _config;
        private readonly CompositeDisposable _disposables;
        private readonly List<BuildingTooltipParameterView> _parameters;

        private BuildingConditionalConfig _showingBuilding;
        
        public BuildingTooltipPresenter(BuildingsToolbarPresenter toolbar, BuildingTooltipView view, BuildingTooltipConfig config)
        {
            _toolbar = toolbar;
            _view = view;
            _config = config;
            _disposables = new();
            _parameters = new();
        }
        
        public void Initialize()
        {
            _view.gameObject.SetActive(false);
            Observable.CombineLatest(_toolbar.SelectedBuilding, _toolbar.HoveredBuilding,
                    (selected, hovered) =>
                    {
                        if (selected != null)
                            return selected;
                        if (hovered != null)
                            return hovered;
                        return null;
                    }).DistinctUntilChanged()
                .Subscribe(building =>
                {
                    if (building == null)
                        Hide();
                    else
                        Show(building);
                }).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void Show(BuildingConditionalConfig building)
        {
            if (_showingBuilding == building)
                return;
            if (_showingBuilding != null && _showingBuilding != building)
                Hide();
            
            _showingBuilding = building;
            _view.SetBuildingName(_showingBuilding.Name);
            _view.SetDescription(_showingBuilding.Description);
            _view.SetCost(_showingBuilding.MineralsCost);
            if (_showingBuilding.ColonizersCost > 0)
                AddParameter(_config.GetColonizers(-_showingBuilding.ColonizersCost));
            if (_showingBuilding.EnergyCost > 0)
                AddParameter(_config.GetEnergy(-_showingBuilding.EnergyCost));
            foreach (var parameter in _showingBuilding.GetAdditionalParameters())
                AddParameter(parameter);
            
            _view.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _showingBuilding = null;
            _view.gameObject.SetActive(false);
            foreach (var parameter in _parameters)
                Object.Destroy(parameter.gameObject);
            _parameters.Clear();
        }

        private void AddParameter(BuildingParameterEntry entry)
        {
            var view = _view.CreateTooltipParameter();
            entry.ApplyTo(view);
            _parameters.Add(view);
        }
    }
}