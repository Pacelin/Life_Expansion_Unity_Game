using Runtime.Gameplay.Buildings.General;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.Concrete
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Cosmoport", fileName = "Cosmoport")]
    public class CosmoportConfig : BuildingConditionalConfig
    {
        public override BuildingParameterEntry[] GetAdditionalParameters() =>
            new BuildingParameterEntry[] { };

        public override IBuildingModel CreateModel(DiContainer container, BuildingView view) =>
            container.Instantiate<CosmoportModel>(new object[] { this, view });
    }
}