using Runtime.Gameplay.Planets;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Runtime.Cinematic
{
    public class CameraController : ITickable
    {
        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }
        private bool _enabled;
        private Vector3 _lastPoint;
        private float _targetDistance;

        private readonly Planet _planet;
        private readonly Camera _camera;
        private readonly Transform _cameraTransform;
        private readonly CameraControllerConfig _config;
        
        public CameraController(Planet planet, Camera camera, CameraControllerConfig config)
        {
            _planet = planet;
            _camera = camera;
            _config = config;
            _cameraTransform = _camera.transform;
            _enabled = true;
            _targetDistance = _config.InitialDistance;
            _cameraTransform.position = (_cameraTransform.position - _planet.Center).normalized * (_targetDistance + _planet.Radius);
        }

        public void Tick()
        {
            if (!_enabled) return;

            if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
                _lastPoint = GetMousePosition();
            else if (Input.GetMouseButton((int)MouseButton.LeftMouse))
                Rotate();

            if (Input.mouseScrollDelta.y != 0)
                _targetDistance = Mathf.Clamp(_targetDistance - Input.mouseScrollDelta.y * _config.ZoomStep,
                    _config.MinDistance, _config.MaxDistance);

            Zoom();
        }

        private void Zoom()
        {
            var vector = _cameraTransform.position - _planet.Center;
            var currentDistance = vector.magnitude - _planet.Radius;
            var resultDistance = _planet.Radius + Mathf.MoveTowards(currentDistance, _targetDistance,
                _config.ZoomSpeed * Time.deltaTime);
            var resultVector = vector.normalized * resultDistance;
            _cameraTransform.position = _planet.Center + resultVector;
        }
        
        private void Rotate()
        {
            var nextPoint = GetMousePosition();
            var sensitivity = Mathf.Lerp(_config.RotateSensitivityRange.x, _config.RotateSensitivityRange.y,
                (_targetDistance - _config.MinDistance) / (_config.MaxDistance - _config.MinDistance));
            var delta = (nextPoint - _lastPoint) * sensitivity;
            _lastPoint = nextPoint;
            
            _camera.transform.RotateAround(_planet.Center, _cameraTransform.right, -delta.y);
            _camera.transform.RotateAround(_planet.Center, _cameraTransform.up, delta.x);
        }
        
        private Vector3 GetMousePosition() => Input.mousePosition;
    }
}