using Runtime.Gameplay.Buildings.UI;
using UnityEngine;

namespace Runtime.Gameplay.Buildings
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Parameter Config (Numeric)")]
    public class NumericBuildingParameterConfig : BuildingParameterConfig
    {
        [SerializeField] private bool _positiveIsGood = true;
        [SerializeField] private bool _isOk = false;
        
        public override void ApplyTo(BuildingTooltipParameterView view, int value)
        {
            base.ApplyTo(view, value);
            
            var opinion = EParameterOpinion.OK;
            if (!_isOk)
            {
                if (value > 0 && _positiveIsGood)
                    opinion = EParameterOpinion.Good;
                else if (value < 0 && !_positiveIsGood)
                    opinion = EParameterOpinion.Good;
                else
                    opinion = EParameterOpinion.Bad;
            }
                
            view.SetIcon(value, opinion);
        }
    }
}