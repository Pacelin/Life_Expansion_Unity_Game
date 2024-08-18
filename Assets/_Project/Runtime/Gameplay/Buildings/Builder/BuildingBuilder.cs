using System;
using R3;
using Runtime.Gameplay.Buildings.UI;
using Runtime.Gameplay.Planets;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Runtime.Gameplay.Buildings.Builder
{
    public class BuildingBuilder : IInitializable, IDisposable
    {
        private bool _isActive;
            
        private readonly BuildingBuilderConfig _config;
        private readonly Planet _planet;
        private readonly Camera _camera;
        private readonly BuildingsToolbarPresenter _toolbar;
        private readonly BuildingBuilderView _view;
        private readonly CompositeDisposable _disposables;

        private CompositeDisposable _showingDisposables;
        
        public BuildingBuilder(BuildingBuilderConfig config, Planet planet, Camera camera, 
            BuildingsToolbarPresenter toolbar, BuildingBuilderView view)
        {
            _config = config;
            _planet = planet;
            _camera = camera;
            _toolbar = toolbar;
            _disposables = new();
            _view = view;
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
            var prefab = buildingConfig.Prefab;
            _view.SetBuildingMesh(prefab.Mesh);
            _view.SetBuildingRadius(prefab.BuildingRadius);
            
            UpdatePosition();
            Observable.EveryUpdate(UnityFrameProvider.FixedUpdate)
                .Subscribe(_ => UpdatePosition())
                .AddTo(_showingDisposables);
            Observable.EveryUpdate(UnityFrameProvider.Update)
                .Where(_ => !_isActive && Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
                .Subscribe(_ => _toolbar.ResetSelection())
                .AddTo(_showingDisposables);
        }
        
        private void Hide()
        {
            _showingDisposables?.Dispose();
            _view.gameObject.SetActive(false);
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
            }
            else
            {
                _view.gameObject.SetActive(false);
                _isActive = false;
            }
        }
    }
}