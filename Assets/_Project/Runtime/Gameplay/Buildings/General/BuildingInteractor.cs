using System;
using R3;
using Runtime.Gameplay.Buildings.Builder;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Zenject;

namespace Runtime.Gameplay.Buildings.General
{
    public class BuildingInteractor : ITickable, IInitializable, IDisposable
    {
        private readonly BuildingInteractorView _view;
        private readonly BuildingFactory _factory;
        private readonly Camera _mainCamera;
        private readonly int _layerMask;

        private BuildingView _choosedBuilding;
        private IDisposable _clickDisposable;
        
        public BuildingInteractor(BuildingInteractorView view, BuildingFactory factory, Camera mainCamera, int layerMask)
        {
            _view = view;
            _factory = factory;
            _mainCamera = mainCamera;
            _layerMask = layerMask;
        }

        public void Initialize()
        {
            _clickDisposable = _view.OnClick
                .Where(_ => _choosedBuilding != null)
                .Subscribe(_ =>
                {
                    _factory.Remove(_choosedBuilding.Model);
                    _view.Hide();
                });
        }
        
        public void Dispose()
        {
            _clickDisposable.Dispose();
        }

        public void Tick()
        {
            if (Input.GetMouseButtonDown((int)MouseButton.RightMouse))
            {
                if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition),
                        out var hit, float.MaxValue, _layerMask))
                {
                    if (hit.collider.TryGetComponent<BuildingView>(out var buildingView))
                    {
                        _choosedBuilding = buildingView;
                        _view.ShowMessage(_view.DestroyString, Input.mousePosition);
                    }
                }
                else
                {
                    _choosedBuilding = null;
                    _view.Hide();
                }
            }
            else if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse) &&
                 !EventSystem.current.IsPointerOverGameObject())
            {
                _choosedBuilding = null;
                _view.Hide();
            }
        }
    }
}