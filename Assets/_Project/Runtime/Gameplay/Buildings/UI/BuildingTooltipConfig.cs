using UnityEngine;

namespace Runtime.Gameplay.Buildings.UI
{
    [CreateAssetMenu(menuName = "Gameplay/Building Tooltip Config")]
    public class BuildingTooltipConfig : ScriptableObject
    {
        [SerializeField] private BuildingParameterConfig _colonizersParameter;
        [SerializeField] private BuildingParameterConfig _energyParameter;

        public BuildingParameterEntry GetColonizers(int value) =>
            new BuildingParameterEntry(_colonizersParameter, value);

        public BuildingParameterEntry GetEnergy(int value) =>
            new BuildingParameterEntry(_energyParameter, value);
    }
}