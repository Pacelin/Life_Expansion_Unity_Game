using System.Collections.Generic;
using Runtime.Gameplay.Buildings.General;
using Zenject;

namespace Runtime.Gameplay.Buildings.Builder
{
    public class BuildingFactory
    {
        [Inject] private DiContainer _di;
        
        private List<IBuildingModel> _buildings = new();
        
        public void Add(BuildingConditionalConfig config, BuildingView view)
        {
            var newModel = config.CreateModel(_di, view);
            _buildings.Add(newModel);
            newModel.Build();
        }
    }
}