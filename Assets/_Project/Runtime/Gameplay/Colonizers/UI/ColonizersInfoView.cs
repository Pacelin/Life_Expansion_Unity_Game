using DG.Tweening;
using Runtime.Gameplay.Misc;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Runtime.Gameplay.Colonizers.UI
{
    public class ColonizersInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _mineralsText;
        [SerializeField] private TextMeshProUGUI _maxMineralsText;
        [SerializeField] private TextMeshProUGUI _energyText;
        [SerializeField] private TextMeshProUGUI _usageEnergyText;
        [SerializeField] private TextMeshProUGUI _populationText;
        [SerializeField] private TextMeshProUGUI _populationCapacityText;
        [SerializeField] private TextMeshProUGUI _populationAliveText;
        [Space] 
        [SerializeField] private TextMeshProUGUI _targetPopulationText;
        [SerializeField] private Image _targetFill;
        [SerializeField] private GameObject _targetReachedMark;  
        [Space]
        [SerializeField] private LocalizedString _usageLocalized;
        [SerializeField] private LocalizedString _maxLocalized;
        [SerializeField] private LocalizedString _capacityLocalized;
        [SerializeField] private LocalizedString _aliveLocalized;

        [Header("Ping")]
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _pingColor;
        [SerializeField] private float _pingScale;
        [SerializeField] private float _pingDuration;
        [SerializeField] private int _pingLoops;
        
        private Tween _pingTween;

        public void SetMinerals(int count) =>
            _mineralsText.text = count.ToGameString();
        public void SetMaxMinerals(int count) =>
            _maxMineralsText.text = _maxLocalized.GetLocalizedString() + ":" + count.ToGameString();

        public void SetEnergy(int count) =>
            _energyText.text = count.ToGameString();
        public void SetEnergyUsage(int usage) =>
            _usageEnergyText.text = _usageLocalized.GetLocalizedString() + ":" + usage.ToGameString();

        public void SetPopulation(int free, int overall)
        {
            _populationText.text = free.ToGameString();
            _populationAliveText.text = _aliveLocalized.GetLocalizedString() + ":" + overall.ToGameString();
        }
        public void SetPopulationCapacity(int capacity) =>
            _populationCapacityText.text = _capacityLocalized.GetLocalizedString() + ":" + capacity.ToGameString();

        public void SetTargetReached(bool reached)
        {
            _targetPopulationText.gameObject.SetActive(!reached);
            _targetFill.gameObject.SetActive(!reached);
            _targetReachedMark.SetActive(reached);
        }

        public void SetTargetProgress(float progress) =>
            _targetFill.fillAmount = progress;
        
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