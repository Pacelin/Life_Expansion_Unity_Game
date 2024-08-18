using Runtime.Gameplay.Buildings.UI;
using UnityEngine;

namespace Runtime.Gameplay.Buildings
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Parameter Config (Arrow)")]
    public class ArrowBuildingParameterConfig : BuildingParameterConfig
    {
        [SerializeField] private int _positiveSmallArrowMax = 2;
        [SerializeField] private int _positiveMediumArrowMax = 4;
        [SerializeField] private int _negativeSmallArrowMin = -2;
        [SerializeField] private int _negativeMediumArrowMin = -4;
        [SerializeField] private bool _positiveIsGood = true;
        [SerializeField] private bool _isOk = false;

        public override void ApplyTo(BuildingTooltipParameterView view, int value)
        {
            base.ApplyTo(view, value);
            var arrowIndex = GetArrowIndex(value);
            
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
                
            view.SetIcon(arrowIndex, value, opinion);
        }

        private int GetArrowIndex(int value)
        {
            if (value > 0)
            {
                if (value < _positiveSmallArrowMax)
                    return 0;
                if (value < _positiveMediumArrowMax)
                    return 1;
                return 2;
            }
            
            if (value > _negativeSmallArrowMin)
                return 0;
            if (value > _negativeMediumArrowMin)
                return 1;
            return 2;
        }
    }
}