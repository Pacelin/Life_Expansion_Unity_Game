using Runtime.Gameplay.Misc;
using TMPro;
using UnityEngine;

namespace Runtime.Gameplay.Buildings.UI
{
    public class BuildingTooltipView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _buildingName;
        [SerializeField] private TextMeshProUGUI _buildingDescription;
        [SerializeField] private TextMeshProUGUI _costText;
        [Space]
        [SerializeField] private Transform _parametersContainer;
        [SerializeField] private BuildingTooltipParameterView _parameterPrefab;

        public BuildingTooltipParameterView CreateTooltipParameter() =>
            Instantiate(_parameterPrefab, _parametersContainer);

        public void SetBuildingName(string buildingName) => _buildingName.text = buildingName;
        public void SetDescription(string buildingDescription) => _buildingDescription.text = buildingDescription;
        public void SetCost(int cost) => _costText.text = cost.ToGameString();
    }
}