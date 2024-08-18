using System;
using System.Collections.Generic;
using R3;
using Zenject;

namespace Runtime.Gameplay.Buildings.UI
{
    public class BuildingsToolbarPresenter : IDisposable
    {
        public ReadOnlyReactiveProperty<BuildingConfig> SelectedBuilding => _selectedBuilding;
        public ReadOnlyReactiveProperty<BuildingConfig> HoveredBuilding => _hoveredBuilding;

        private readonly DiContainer _di;
        private readonly BuildingsToolbarView _view;
        private readonly Dictionary<BuildingsToolbarItemView, BuildingConfig> _map;
        private readonly ReadOnlyReactiveProperty<BuildingConfig> _selectedBuilding;
        private readonly ReadOnlyReactiveProperty<BuildingConfig> _hoveredBuilding;
        private readonly CompositeDisposable _disposables;

        public BuildingsToolbarPresenter(DiContainer di, BuildingsToolbarView view)
        {
            _di = di;
            _view = view;
            _map = new();
            _selectedBuilding = _view.Selected.Select(view => view == null ? null : _map[view])
                .ToReadOnlyReactiveProperty();
            _hoveredBuilding = _view.Hovered.Select(view => view == null ? null : _map[view])
                .ToReadOnlyReactiveProperty();
            _disposables = new();
        }
        
        public void Dispose() => _disposables.Dispose();

        public void Add(BuildingConfig building)
        {
            var view = _view.CreateItem();
            var presenter = _di.Instantiate<BuildingsToolbarItemPresenter>(new object[] { building, view });
            _map.Add(view, building);
            presenter.Initialize();
            _disposables.Add(presenter);
        }
    }
}