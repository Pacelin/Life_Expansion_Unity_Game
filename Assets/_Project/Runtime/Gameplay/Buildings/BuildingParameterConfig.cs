using Runtime.Gameplay.Buildings.UI;
using UnityEngine;
using UnityEngine.Localization;

namespace Runtime.Gameplay.Buildings
{
    public abstract class BuildingParameterConfig : ScriptableObject
    {
        [SerializeField] private LocalizedString _caption;

        public virtual void ApplyTo(BuildingTooltipParameterView view, int value)
        {
            view.SetCaption(_caption.GetLocalizedString());
        }
    }
}