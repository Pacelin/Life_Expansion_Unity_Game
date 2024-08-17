using UnityEngine;

namespace Runtime.Gameplay.Core
{
    [CreateAssetMenu(menuName = "Gameplay/Gameplay Config")]
    public class GameplayConfig : ScriptableObject
    {
        public float TickDelta => 1 / _tickRate;
        public float TimeScale => _timeScale;
        public int InitialYear => _initialYear;
        
        [SerializeField] private float _tickRate = 20;
        [SerializeField] private float _timeScale = 604800;
        [SerializeField] private int _initialYear = 2107;
    }
}