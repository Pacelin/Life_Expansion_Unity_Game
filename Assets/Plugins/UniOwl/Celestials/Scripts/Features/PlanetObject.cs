using UniOwl.Components;
using UnityEngine;

namespace UniOwl.Celestials
{
    [CreateAssetMenu(menuName = "Game/Celestials/Planet", fileName = "SO_Planet")]
    public class PlanetObject : ScriptableComponentListWithPrefab<PlanetComponent>
    {
        [SerializeField]
        private PhysicalSettings _physicalSettings;
        [SerializeField]
        private TerraformingSettings _terraformingSettings;
        
        public PhysicalSettings Physical => _physicalSettings;
        public TerraformingSettings Terraforming => _terraformingSettings;
    }
}