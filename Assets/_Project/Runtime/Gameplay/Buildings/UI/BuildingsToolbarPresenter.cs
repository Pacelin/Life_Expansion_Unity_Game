using System;
using System.Collections.Generic;
using R3;
using Zenject;

namespace Runtime.Gameplay.Buildings.UI
{
    public class BuildingsToolbarPresenter : IDisposable
    {
        public ReadOnlyReactiveProperty<BuildingConditionalConfig> SelectedBuilding => _selectedBuilding;
        public ReadOnlyReactiveProperty<BuildingConditionalConfig> HoveredBuilding => _hoveredBuilding;

        private readonly DiContainer _di;
        private readonly BuildingsToolbarView _view;
        private readonly Dictionary<BuildingsToolbarItemView, BuildingConditionalConfig> _map;
        private readonly Dictionary<BuildingsToolbarTabConfig, BuildingsToolbarTabView> _tabsMap;
        private readonly ReadOnlyReactiveProperty<BuildingConditionalConfig> _selectedBuilding;
        private readonly ReadOnlyReactiveProperty<BuildingConditionalConfig> _hoveredBuilding;
        private readonly CompositeDisposable _disposables;

        public BuildingsToolbarPresenter(DiContainer di, BuildingsToolbarView view)
        {
            _di = di;
            _view = view;
            _map = new();
            _tabsMap = new();
            _selectedBuilding = _view.Selected.Select(view => view == null ? null : _map[view])
                .ToReadOnlyReactiveProperty();
            _hoveredBuilding = _view.Hovered.Select(view => view == null ? null : _map[view])
                .ToReadOnlyReactiveProperty();
            _disposables = new();
        }
        
        public void Dispose() => _disposables.Dispose();

        public void ResetSelection() => _view.ResetSelection();
        
        public void Add(BuildingConditionalConfig building)
        {
            if (!_tabsMap.ContainsKey(building.ToolbarTab))
                _tabsMap.Add(building.ToolbarTab, _view.CreateTab(building.ToolbarTab.Caption));
            var view = _view.CreateItem();
            _view.AssignTab(view, _tabsMap[building.ToolbarTab]);

            var presenter = _di.Instantiate<BuildingsToolbarItemPresenter>(new object[] { building, view });
            _map.Add(view, building);
            presenter.Initialize();
            _disposables.Add(presenter);
        }

        public void Activate() => _view.SelectFirstTab();
    }
}