using UnityEngine;

namespace Runtime.Cinematic
{
    [System.Serializable]
    public class CameraControllerConfig
    {
        public float MinDistance => _minDistance;
        public float InitialDistance => _initialDistance;
        public float MaxDistance => _maxDistance;
        public float ZoomStep => _zoomStep;
        public float ZoomSpeed => _zoomSpeed;
        public Vector2 RotateSensitivityRange => _rotateSensitivityRange;
        
        [SerializeField] private float _minDistance = 0.5f;
        [SerializeField] private float _initialDistance = 1f;
        [SerializeField] private float _maxDistance = 2f;
        [SerializeField] private float _zoomStep = 0.1f;
        [SerializeField] private float _zoomSpeed = 0.1f;
        [SerializeField] private Vector2 _rotateSensitivityRange = new Vector2(0.1f, 0.2f);
    }
}