using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;
using Runtime.Gameplay.Buildings.Builder;
using Runtime.Gameplay.Colonizers;
using Zenject;

namespace Runtime.Gameplay.Buildings.General
{
    public class BuildingsValidator : IInitializable, IDisposable
    {
        private readonly BuildingFactory _factory;
        private readonly ColonizersModel _colonizers;
        private readonly List<IBuildingModel> _disabledByEnergy;
        private readonly List<IBuildingModel> _disabledByColonizers;
        private IDisposable _updateDisposable;
        
        public BuildingsValidator(BuildingFactory factory, ColonizersModel colonizers)
        {
            _factory = factory;
            _colonizers = colonizers;
            _disabledByEnergy = new();
            _disabledByColonizers = new();
        }

        public void Initialize()
        {
            _updateDisposable = R3.Observable.Merge(
                _factory.Buildings.ObserveAdd().AsUnitObservable(),
                _factory.Buildings.ObserveRemove().AsUnitObservable(),
                _colonizers.Energy.Energy.AsUnitObservable(),
                _colonizers.Energy.EnergyUsage.AsUnitObservable(),
                _colonizers.Population.CurrentPopulation.AsUnitObservable(),
                _colonizers.Population.BusyPopulation.AsUnitObservable())
            .Subscribe(_ => Update());
        }

        public void Dispose()
        {
            _updateDisposable?.Dispose();
        }

        private void Update()
        {
            var firstBuilding = _factory.Buildings.LastOrDefault(b => b.Enabled);
            if (firstBuilding == null)
                return;
            var noEnergy = _colonizers.Energy.EnergyUsage.CurrentValue > _colonizers.Energy.Energy.CurrentValue;
            var noColonizers = _colonizers.Population.BusyPopulation.CurrentValue > _colonizers.Population.CurrentPopulation.CurrentValue;
            if (noEnergy)
                _disabledByEnergy.Add(firstBuilding);
            if (noColonizers)
                _disabledByColonizers.Add(firstBuilding);
            
            if (noEnergy && noColonizers)
                firstBuilding.SetState(EBuildingState.NoEnergy, EBuildingState.NoColonists);
            else if (noEnergy)
                firstBuilding.SetState(EBuildingState.NoEnergy);
            else if (noColonizers)
                firstBuilding.SetState(EBuildingState.NoColonists);

            if (!noColonizers)
            {
                foreach (var building in _disabledByColonizers)
                {
                    if (building.EnoughColonizers())
                    {
                        building.CancelState(EBuildingState.NoColonists);
                        _disabledByColonizers.Remove(building);
                        break;
                    }
                }
            }
            if (!noEnergy)
            {
                foreach (var building in _disabledByEnergy)
                {
                    if (building.EnoughEnergy())
                    {
                        building.CancelState(EBuildingState.NoEnergy);
                        _disabledByEnergy.Remove(building);
                        break;
                    }
                }
            }
        }
    }
}