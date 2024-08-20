using System;
using System.Collections.Generic;
using ObservableCollections;
using Runtime.Gameplay.Buildings.General;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.Builder
{
    public class BuildingFactory : IDisposable
    {
        public ObservableList<IBuildingModel> Buildings => _buildings;

        [Inject] private DiContainer _di;
        
        private ObservableList<IBuildingModel> _buildings = new();
        
        public void Add(BuildingConditionalConfig config, BuildingView view)
        {
            var newModel = config.CreateModel(_di, view);
            view.SetModel(newModel);
            _buildings.Add(newModel);
            newModel.Build();
        }

        public void Remove(IBuildingModel building)
        {
            building.Delete();
            building.Dispose();
            GameObject.Destroy(building.View.gameObject);
            _buildings.Remove(building);
        }

        public void Dispose()
        {
            foreach (var building in _buildings)
                building.Dispose();
            _buildings.Clear();
        }
    }
}