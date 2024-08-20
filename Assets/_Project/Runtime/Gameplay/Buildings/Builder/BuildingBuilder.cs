using System;
using R3;
using Runtime.Gameplay.Buildings.UI;
using Runtime.Gameplay.Planets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Zenject;

namespace Runtime.Gameplay.Buildings.Builder
{
    public class BuildingBuilder : IInitializable, IDisposable
    {
        private bool _isActive;
        private BuildingConditionalConfig _currentBuilding;
        private CompositeDisposable _showingDisposables;
            
        private readonly BuildingBuilderConfig _config;
        private readonly Planet _planet;
        private readonly Camera _camera;
        private readonly BuildingsToolbarPresenter _toolbar;
        private readonly BuildingBuilderView _view;
        private readonly BuildingApplier _applier;
        private readonly BuildingFactory _factory;
        private readonly CompositeDisposable _disposables;
        
        public BuildingBuilder(BuildingBuilderConfig config, Planet planet, Camera camera, 
            BuildingsToolbarPresenter toolbar, BuildingBuilderView view, BuildingApplier applier,
            BuildingFactory factory)
        {
            _config = config;
            _planet = planet;
            _camera = camera;
            _toolbar = toolbar;
            _view = view;
            _applier = applier;
            _factory = factory;
            _disposables = new();
        }

        public void Initialize()
        {
            _view.gameObject.SetActive(false);
            _toolbar.SelectedBuilding
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
            _showingDisposables?.Dispose();
            _disposables.Dispose();
        }

        private void Show(BuildingConditionalConfig buildingConfig)
        {
            _showingDisposables = new();
            _currentBuilding = buildingConfig;
            var prefab = buildingConfig.Prefab;
            _view.SetBuildingMesh(prefab.Mesh);
            _view.SetBuildingRadius(prefab.BuildingRadius);
            foreach (var building in _factory.Buildings)
                building.View.EnableProjector();
            
            UpdatePosition();
            Observable.EveryUpdate(UnityFrameProvider.FixedUpdate)
                .Subscribe(_ => UpdatePosition())
                .AddTo(_showingDisposables);
            Observable.EveryUpdate(UnityFrameProvider.Update)
                .Where(_ => !_isActive && Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
                .Subscribe(_ => _toolbar.ResetSelection())
                .AddTo(_showingDisposables);
            Observable.EveryUpdate(UnityFrameProvider.Update)
                .Where(_ => _isActive && Input.GetMouseButtonDown((int)MouseButton.LeftMouse) &&
                     !EventSystem.current.IsPointerOverGameObject())
                .Subscribe(_ =>
                {
                    if (_applier.TryBuild(_currentBuilding, _view))
                        _toolbar.ResetSelection();
                })
                .AddTo(_showingDisposables);
        }
        
        private void Hide()
        {
            foreach (var building in _factory.Buildings)
                building.View.DisableProjector();
            _showingDisposables?.Dispose();
            _view.gameObject.SetActive(false);
            _currentBuilding = null;
            _view.ClearMesh();
        }

        private void UpdatePosition()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            var viewTransform = _view.transform;
            if (Physics.Raycast(ray, out var hit, float.MaxValue, _config.PlanetLayer))
            {
                _view.gameObject.SetActive(true);
                _isActive = true;
                viewTransform.position = hit.point;
                viewTransform.up = (hit.point - _planet.Center).normalized;
                _applier.ValidatePosition(_currentBuilding, _view, hit.collider.CompareTag(_config.WaterTag));
            }
            else
            {
                _view.gameObject.SetActive(false);
                _isActive = false;
            }
        }
    }
}