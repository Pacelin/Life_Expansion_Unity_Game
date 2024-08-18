using Runtime.Gameplay.Buildings.UI;

namespace Runtime.Gameplay.Buildings
{
    [System.Serializable]
    public struct BuildingParameterEntry
    {
        public float Value;
        public BuildingParameterConfig Config;

        public BuildingParameterEntry(BuildingParameterConfig config, float value)
        {
            Config = config;
            Value = value;
        }

        public void ApplyTo(BuildingTooltipParameterView view) =>
            Config.ApplyTo(view, (int) Value);
    }
}