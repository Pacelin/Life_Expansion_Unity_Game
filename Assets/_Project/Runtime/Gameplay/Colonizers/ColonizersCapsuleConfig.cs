using UnityEngine;

namespace Runtime.Gameplay.Colonizers
{
    [System.Serializable]
    public class ColonizersCapsuleConfig
    {
        public int InitialPopulation => _initialPopulation;
        public int InitialEnergy => _initialEnergy;
        public int InitialMinerals => _initialMinerals;
        public int InitialMaxMinerals => _initialMaxMinerals;
        
        [SerializeField] private int _initialPopulation;
        [SerializeField] private int _initialEnergy;
        [SerializeField] private int _initialMinerals;
        [SerializeField] private int _initialMaxMinerals;
    }
}