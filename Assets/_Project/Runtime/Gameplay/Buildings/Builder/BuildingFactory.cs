using System;
using System.Collections.Generic;
using Runtime.Gameplay.Buildings.General;
using Zenject;

namespace Runtime.Gameplay.Buildings.Builder
{
    public class BuildingFactory : IDisposable
    {
        [Inject] private DiContainer _di;
        
        private List<IBuildingModel> _buildings = new();
        
        public void Add(BuildingConditionalConfig config, BuildingView view)
        {
            var newModel = config.CreateModel(_di, view);
            _buildings.Add(newModel);
            newModel.Build();
        }

        public void Dispose()
        {
            foreach (var building in _buildings)
                building.Dispose();
            _buildings.Clear();
        }
    }
}