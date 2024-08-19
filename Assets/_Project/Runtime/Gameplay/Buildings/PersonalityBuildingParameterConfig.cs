using Runtime.Gameplay.Buildings.UI;
using UnityEngine;

namespace Runtime.Gameplay.Buildings
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Parameter Config (Personality)")]
    public class PersonalityBuildingParameterConfig : BuildingParameterConfig
    {
        public override void ApplyTo(BuildingTooltipParameterView view, int value)
        {
            base.ApplyTo(view, value);
            view.SetAsBuildingPersonality();
        }
    }
}