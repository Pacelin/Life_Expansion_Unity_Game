using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.Colonizers.UI
{
    public class ColonizersInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _mineralsText;
        [SerializeField] private string _mineralsFormat;
        [SerializeField] private TextMeshProUGUI _energyText;
        [SerializeField] private string _energyFormat;
        [SerializeField] private TextMeshProUGUI _populationText;
        [SerializeField] private string _populationFormat;
        [SerializeField] private TextMeshProUGUI _targetPopulationText;
        [SerializeField] private GameObject _targetReachedMark;
        [SerializeField] private Image _targetSlider;

        [Header("Ping")]
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _pingColor;
        [SerializeField] private float _pingScale;
        [SerializeField] private float _pingDuration;
        [SerializeField] private int _pingLoops;
        
        private Tween _pingTween;
        
        public void SetMinerals(int current, int max) =>
            _mineralsText.text = string.Format(_mineralsFormat, current, max);

        public void SetEnergy(int usage, int max) =>
            _energyText.text = string.Format(_energyFormat, usage, max);

        public void SetTargetReached(bool reached)
        {
            if (reached)
                _targetSlider.fillAmount = 1;
            _targetReachedMark.SetActive(reached);
        }

        public void SetTargetProgress(float progress) =>
            _targetSlider.fillAmount = progress;
        
        public void SetPopulation(int current, int max) =>
            _populationText.text = string.Format(_populationFormat, current, max);

        public void SetTargetPopulation(int target) =>
            _targetPopulationText.text = target.ToString();

        public void PingMinerals()
        {
            _pingTween?.Complete();
            _pingTween = DOTween.Sequence()
                .Append(_mineralsText.DOColor(_pingColor, _pingDuration))
                .Join(_mineralsText.transform.DOScale(_pingScale, _pingDuration))
                .AppendInterval(0.05f)
                .Append(_mineralsText.DOColor(_defaultColor, _pingDuration))
                .Join(_mineralsText.transform.DOScale(1, _pingDuration))
                .SetLoops(_pingLoops);
        }
    }
}