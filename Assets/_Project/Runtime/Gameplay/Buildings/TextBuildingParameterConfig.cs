using Runtime.Gameplay.Buildings.UI;
using UnityEngine;

namespace Runtime.Gameplay.Buildings
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Parameter Config (Text)")]
    public class TextBuildingParameterConfig : BuildingParameterConfig
    {
        [SerializeField] private string _format;
        
        public override void ApplyTo(BuildingTooltipParameterView view, int value)
        {
            base.ApplyTo(view, value);
            view.SetIcon(string.Format(_format, value), EParameterOpinion.OK);
        }
    }
}