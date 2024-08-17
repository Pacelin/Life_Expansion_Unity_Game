using UnityEngine;

namespace Runtime.Gameplay.Colonizers
{
    [System.Serializable]
    public class ColonizerIndicatorConfig
    {
        public float Min => _min;
        public float Max => _max;
        public float Target => _target;
        
        [SerializeField] private float _min;
        [SerializeField] private float _max;
        [SerializeField] private float _target;
    }
}