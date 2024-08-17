using UnityEngine;

namespace Runtime.Gameplay.Planets
{
    [System.Serializable]
    public class PlanetIndicatorConfig
    {
        public float Min => _min;
        public float Max => _max;
        public float Initial => _initial;
        public float Sensitivity => _sensitivity;
        public float Interpolation => _interpolation;
        
        [SerializeField] private float _min = -50;
        [SerializeField] private float _max = 50;
        [SerializeField] private float _initial = -20;
        [SerializeField] private float _sensitivity = 1;
        [SerializeField] private float _interpolation = 0.5f;
    }
}